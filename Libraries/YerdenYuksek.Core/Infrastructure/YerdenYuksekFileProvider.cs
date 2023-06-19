using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace YerdenYuksek.Core.Infrastructure;

public class YerdenYuksekFileProvider : PhysicalFileProvider, IYerdenYuksekFileProvider
{
    #region Constructure and Destructure

    public YerdenYuksekFileProvider(IHostEnvironment hostEnvironment)
        : base(File.Exists(hostEnvironment.ContentRootPath) ? Path.GetDirectoryName(hostEnvironment.ContentRootPath)! : hostEnvironment.ContentRootPath)
    {
        var staticFilePath = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot");
        WebRootPath = File.Exists(staticFilePath) ? Path.GetDirectoryName(staticFilePath)! : staticFilePath;
    }

    #endregion

    #region Public Properties

    public string WebRootPath { get; }

    #endregion

    #region Methods

    public string Combine(params string[] paths)
    {
        var path = Path.Combine(paths.SelectMany(p => IsUncPath(p) ? new[] { p } : p.Split('\\', '/')).ToArray());

        if (Environment.OSVersion.Platform == PlatformID.Unix && !IsUncPath(path))
            //add leading slash to correctly form path in the UNIX system
            path = "/" + path;

        return path;
    }

    public void CreateDirectory(string path)
    {
        if (!DirectoryExists(path))
            Directory.CreateDirectory(path);
    }

    public void CreateFile(string path)
    {
        if (FileExists(path))
            return;

        var fileInfo = new FileInfo(path);
        CreateDirectory(fileInfo.DirectoryName);

        //we use 'using' to close the file after it's created
        using (File.Create(path))
        {
        }
    }

    public void DeleteDirectory(string path)
    {
        if (string.IsNullOrEmpty(path))
            throw new ArgumentNullException(path);

        //find more info about directory deletion
        //and why we use this approach at https://stackoverflow.com/questions/329355/cannot-delete-directory-with-directory-deletepath-true

        foreach (var directory in Directory.GetDirectories(path))
        {
            DeleteDirectory(directory);
        }

        try
        {
            DeleteDirectoryRecursive(path);
        }
        catch (IOException)
        {
            DeleteDirectoryRecursive(path);
        }
        catch (UnauthorizedAccessException)
        {
            DeleteDirectoryRecursive(path);
        }
    }

    public void DeleteFile(string filePath)
    {
        if (!FileExists(filePath))
            return;

        File.Delete(filePath);
    }

    public bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    public void DirectoryMove(string sourceDirName, string destDirName)
    {
        Directory.Move(sourceDirName, destDirName);
    }

    public IEnumerable<string> EnumerateFiles(string directoryPath, string searchPattern,
        bool topDirectoryOnly = true)
    {
        return Directory.EnumerateFiles(directoryPath, searchPattern,
            topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
    }

    public void FileCopy(string sourceFileName, string destFileName, bool overwrite = false)
    {
        File.Copy(sourceFileName, destFileName, overwrite);
    }

    public bool FileExists(string filePath)
    {
        return File.Exists(filePath);
    }

    public long FileLength(string path)
    {
        if (!FileExists(path))
            return -1;

        return new FileInfo(path).Length;
    }

    public void FileMove(string sourceFileName, string destFileName)
    {
        File.Move(sourceFileName, destFileName);
    }

    public string GetAbsolutePath(params string[] paths)
    {
        var allPaths = new List<string>();

        if (paths.Any() && !paths[0].Contains(WebRootPath, StringComparison.InvariantCulture))
            allPaths.Add(WebRootPath);

        allPaths.AddRange(paths);

        return Combine(allPaths.ToArray());
    }

    [SupportedOSPlatform("windows")]
    public DirectorySecurity GetAccessControl(string path)
    {
        return new DirectoryInfo(path).GetAccessControl();
    }

    public DateTime GetCreationTime(string path)
    {
        return File.GetCreationTime(path);
    }

    public string[] GetDirectories(string path, string searchPattern = "", bool topDirectoryOnly = true)
    {
        if (string.IsNullOrEmpty(searchPattern))
            searchPattern = "*";

        return Directory.GetDirectories(path, searchPattern,
            topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories);
    }

    public string GetDirectoryName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    public string GetDirectoryNameOnly(string path)
    {
        return new DirectoryInfo(path).Name;
    }

    public string GetFileExtension(string filePath)
    {
        return Path.GetExtension(filePath);
    }

    public string GetFileName(string path)
    {
        return Path.GetFileName(path);
    }

    public string GetFileNameWithoutExtension(string filePath)
    {
        return Path.GetFileNameWithoutExtension(filePath);
    }

    public string[] GetFiles(string directoryPath, string searchPattern = "", bool topDirectoryOnly = true)
    {
        if (string.IsNullOrEmpty(searchPattern))
            searchPattern = "*.*";

        return Directory.GetFileSystemEntries(directoryPath, searchPattern,
            new EnumerationOptions
            {
                IgnoreInaccessible = true,
                MatchCasing = MatchCasing.CaseInsensitive,
                RecurseSubdirectories = !topDirectoryOnly,

            });
    }

    public DateTime GetLastAccessTime(string path)
    {
        return File.GetLastAccessTime(path);
    }

    public DateTime GetLastWriteTime(string path)
    {
        return File.GetLastWriteTime(path);
    }

    public DateTime GetLastWriteTimeUtc(string path)
    {
        return File.GetLastWriteTimeUtc(path);
    }

    public FileStream GetOrCreateFile(string path)
    {
        if (FileExists(path))
            return File.Open(path, FileMode.Open, FileAccess.ReadWrite);

        var fileInfo = new FileInfo(path);
        CreateDirectory(fileInfo.DirectoryName);

        return File.Create(path);
    }

    public string GetParentDirectory(string directoryPath)
    {
        return Directory.GetParent(directoryPath).FullName;
    }

    public string GetVirtualPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return path;

        if (!IsDirectory(path) && FileExists(path))
            path = new FileInfo(path).DirectoryName;

        path = path?.Replace(WebRootPath, string.Empty).Replace('\\', '/').Trim('/').TrimStart('~', '/');

        return $"~/{path ?? string.Empty}";
    }

    public bool IsDirectory(string path)
    {
        return DirectoryExists(path);
    }

    public string MapPath(string path)
    {
        path = path.Replace("~/", string.Empty).TrimStart('/');

        //if virtual path has slash on the end, it should be after transform the virtual path to physical path too
        var pathEnd = path.EndsWith('/') ? Path.DirectorySeparatorChar.ToString() : string.Empty;

        return Combine(Root ?? string.Empty, path) + pathEnd;
    }

    public async Task<byte[]> ReadAllBytesAsync(string filePath)
    {
        return File.Exists(filePath) ? await File.ReadAllBytesAsync(filePath) : Array.Empty<byte>();
    }

    public async Task<string> ReadAllTextAsync(string path, Encoding encoding)
    {
        await using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream, encoding);

        return await streamReader.ReadToEndAsync();
    }

    public string ReadAllText(string path, Encoding encoding)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var streamReader = new StreamReader(fileStream, encoding);

        return streamReader.ReadToEnd();
    }

    public async Task WriteAllBytesAsync(string filePath, byte[] bytes)
    {
        await File.WriteAllBytesAsync(filePath, bytes);
    }

    public async Task WriteAllTextAsync(string path, string contents, Encoding encoding)
    {
        await File.WriteAllTextAsync(path, contents, encoding);
    }

    public void WriteAllText(string path, string contents, Encoding encoding)
    {
        File.WriteAllText(path, contents, encoding);
    }

    public new IFileInfo GetFileInfo(string subpath)
    {
        subpath = subpath.Replace(Root, string.Empty);

        return base.GetFileInfo(subpath);
    }

    #endregion

    #region Methods

    private static void DeleteDirectoryRecursive(string path)
    {
        Directory.Delete(path, true);
        const int maxIterationToWait = 10;
        var curIteration = 0;

        //according to the documentation(https://msdn.microsoft.com/ru-ru/library/windows/desktop/aa365488.aspx) 
        //System.IO.Directory.Delete method ultimately (after removing the files) calls native 
        //RemoveDirectory function which marks the directory as "deleted". That's why we wait until 
        //the directory is actually deleted. For more details see https://stackoverflow.com/a/4245121
        while (Directory.Exists(path))
        {
            curIteration += 1;

            if (curIteration > maxIterationToWait)
                return;

            Thread.Sleep(100);
        }
    }

    private static bool IsUncPath(string path)
    {
        return Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.IsUnc;
    }

    #endregion
}