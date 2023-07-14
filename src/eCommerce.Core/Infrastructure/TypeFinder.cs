﻿using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace eCommerce.Core.Infrastructure;

public partial class TypeFinder : ITypeFinder
{
    #region Fields

    private readonly bool _ignoreReflectionErrors = true;

    private readonly ICustomFileProvider _fileProvider;

    #endregion

    #region Constructure and Destructure

    public TypeFinder(ICustomFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    #endregion

    #region Public Properties

    public AppDomain App => AppDomain.CurrentDomain;

    public bool LoadAppDomainAssemblies { get; set; } = true;

    public IList<string> AssemblyNames { get; set; } = new List<string>();

    public string AssemblySkipLoadingPattern { get; set; } = "^System|^mscorlib|^Microsoft|^AjaxControlToolkit|^Antlr3|^Autofac|^AutoMapper|^Castle|^ComponentArt|^CppCodeProvider|^DotNetOpenAuth|^EntityFramework|^EPPlus|^FluentValidation|^ImageResizer|^itextsharp|^log4net|^MaxMind|^MbUnit|^MiniProfiler|^Mono.Math|^MvcContrib|^Newtonsoft|^NHibernate|^nunit|^Org.Mentalis|^PerlRegex|^QuickGraph|^Recaptcha|^Remotion|^RestSharp|^Rhino|^Telerik|^Iesi|^TestDriven|^TestFu|^UserAgentStringLibrary|^VJSharpCodeProvider|^WebActivator|^WebDev|^WebGrease";

    public string AssemblyRestrictToLoadingPattern { get; set; } = ".*";

    #endregion

    #region Public Methods

    public IEnumerable<Type> FindClassesOfType<T>(bool onlyConcreteClasses = true)
    {
        return FindClassesOfType(typeof(T), onlyConcreteClasses);
    }

    public IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, bool onlyConcreteClasses = true)
    {
        return FindClassesOfType(assignTypeFrom, GetAssemblies(), onlyConcreteClasses);
    }

    public virtual IList<Assembly> GetAssemblies()
    {
        var addedAssemblyNames = new List<string>();
        var assemblies = new List<Assembly>();

        if (LoadAppDomainAssemblies)
            AddAssembliesInAppDomain(addedAssemblyNames, assemblies);
        AddConfiguredAssemblies(addedAssemblyNames, assemblies);

        return assemblies;
    }

    #endregion

    #region Methods

    private void AddAssembliesInAppDomain(List<string> addedAssemblyNames, List<Assembly> assemblies)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            if (!Matches(assembly.FullName))
                continue;

            if (addedAssemblyNames.Contains(assembly.FullName))
                continue;

            assemblies.Add(assembly);
            addedAssemblyNames.Add(assembly.FullName);
        }
    }

    private void AddConfiguredAssemblies(List<string> addedAssemblyNames, List<Assembly> assemblies)
    {
        foreach (var assemblyName in AssemblyNames)
        {
            var assembly = Assembly.Load(assemblyName);
            if (addedAssemblyNames.Contains(assembly.FullName))
                continue;

            assemblies.Add(assembly);
            addedAssemblyNames.Add(assembly.FullName);
        }
    }

    private bool Matches(string assemblyFullName)
    {
        return !Matches(assemblyFullName, AssemblySkipLoadingPattern)
               && Matches(assemblyFullName, AssemblyRestrictToLoadingPattern);
    }

    private bool Matches(string assemblyFullName, string pattern)
    {
        return Regex.IsMatch(assemblyFullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    private void LoadMatchingAssemblies(string directoryPath)
    {
        var loadedAssemblyNames = new List<string>();

        foreach (var a in GetAssemblies())
        {
            loadedAssemblyNames.Add(a.FullName);
        }

        if (!_fileProvider.DirectoryExists(directoryPath))
        {
            return;
        }

        foreach (var dllPath in _fileProvider.GetFiles(directoryPath, "*.dll"))
        {
            try
            {
                var an = AssemblyName.GetAssemblyName(dllPath);
                if (Matches(an.FullName) && !loadedAssemblyNames.Contains(an.FullName))
                {
                    App.Load(an);
                }

                //old loading stuff
                //Assembly a = Assembly.ReflectionOnlyLoadFrom(dllPath);
                //if (Matches(a.FullName) && !loadedAssemblyNames.Contains(a.FullName))
                //{
                //    App.Load(a.FullName);
                //}
            }
            catch (BadImageFormatException ex)
            {
                Trace.TraceError(ex.ToString());
            }
        }
    }

    private bool DoesTypeImplementOpenGeneric(Type type, Type openGeneric)
    {
        try
        {
            var genericTypeDefinition = openGeneric.GetGenericTypeDefinition();
            foreach (var implementedInterface in type.FindInterfaces((objType, objCriteria) => true, null))
            {
                if (!implementedInterface.IsGenericType)
                    continue;

                if (genericTypeDefinition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition()))
                    return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    private IEnumerable<Type> FindClassesOfType(Type assignTypeFrom, IEnumerable<Assembly> assemblies, bool onlyConcreteClasses = true)
    {
        var result = new List<Type>();
        try
        {
            foreach (var a in assemblies)
            {
                Type[] types = null;
                try
                {
                    types = a.GetTypes();
                }
                catch
                {
                    //Entity Framework 6 doesn't allow getting types (throws an exception)
                    if (!_ignoreReflectionErrors)
                    {
                        throw;
                    }
                }

                if (types == null)
                    continue;

                foreach (var t in types)
                {
                    if (!assignTypeFrom.IsAssignableFrom(t) && (!assignTypeFrom.IsGenericTypeDefinition || !DoesTypeImplementOpenGeneric(t, assignTypeFrom)))
                        continue;

                    if (t.IsInterface)
                        continue;

                    if (onlyConcreteClasses)
                    {
                        if (t.IsClass && !t.IsAbstract)
                        {
                            result.Add(t);
                        }
                    }
                    else
                    {
                        result.Add(t);
                    }
                }
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            var msg = string.Empty;
            foreach (var e in ex.LoaderExceptions)
                msg += e.Message + Environment.NewLine;

            var fail = new Exception(msg, ex);

            throw fail;
        }

        return result;
    }

    #endregion    
}
