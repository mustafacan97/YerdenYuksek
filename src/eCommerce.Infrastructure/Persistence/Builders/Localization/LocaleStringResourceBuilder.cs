using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using eCommerce.Application.Services.Common;
using eCommerce.Core.Domain.Localization;

namespace eCommerce.Infrastructure.Persistence.Builders.Localization;

public sealed class LocaleStringResourceBuilder : IEntityTypeConfiguration<LocaleStringResource>
{
    #region Public Methods

    public void Configure(EntityTypeBuilder<LocaleStringResource> builder)
    {
        builder.ToTable("LocaleStringResource");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.ResourceName)
            .HasMaxLength(256);

        builder.HasData(SeedLocaleStringResourceData());
    }

    #endregion

    #region Methods

    private static IList<LocaleStringResource> SeedLocaleStringResourceData()
    {        
        var filePath = GetFullXmlPath();
        using var streamReader = new StreamReader(filePath);
        var lsNamesList = new Dictionary<string, LocaleStringResource>();
        var resources = new List<LocaleStringResource>() { };

        foreach (var (name, value) in LoadLocaleResourcesFromStream(streamReader))
        {
            resources.Add(new LocaleStringResource 
            { 
                LanguageId = CommonDefaults.DefaultLanguageId,
                ResourceName = name, 
                ResourceValue = value 
            });
        }

        return resources;
    }

    public static string GetFullXmlPath()
    {
        var appDataPath = Path.GetFullPath(@"AppData");
        var fullPath = Combine(appDataPath, "defaultResources.xml");

        return fullPath;
    }

    private static string Combine(params string[] paths)
    {
        var path = Path.Combine(paths.SelectMany(p => IsUncPath(p) ? new[] { p } : p.Split('\\', '/')).ToArray());

        if (Environment.OSVersion.Platform == PlatformID.Unix && !IsUncPath(path))
        {
            path = "/" + path;
        }

        return path;
    }

    private static bool IsUncPath(string path)
    {
        return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
    }

    private static HashSet<(string name, string value)> LoadLocaleResourcesFromStream(StreamReader xmlStreamReader)
    {
        var result = new HashSet<(string name, string value)>();

        using (var xmlReader = XmlReader.Create(xmlStreamReader))
            while (xmlReader.ReadToFollowing("Language"))
            {
                if (xmlReader.NodeType != XmlNodeType.Element)
                {
                    continue;
                }

                using var languageReader = xmlReader.ReadSubtree();
                while (languageReader.ReadToFollowing("LocaleResource"))
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.GetAttribute("Name") is string name)
                    {
                        using var lrReader = languageReader.ReadSubtree();
                        if (lrReader.ReadToFollowing("Value") && lrReader.NodeType == XmlNodeType.Element)
                            result.Add((name.ToLowerInvariant(), lrReader.ReadString()));
                    }

                break;
            }

        return result;
    }

    #endregion
}