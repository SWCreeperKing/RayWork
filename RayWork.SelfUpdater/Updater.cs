using System.Diagnostics;
using Newtonsoft.Json;
using static System.PlatformID;

namespace RayWork.SelfUpdater;

public static class Updater
{
    public static readonly bool IsPlatformSupported = Environment.OSVersion.Platform switch
    {
        Win32S or Win32NT or Win32Windows or WinCE => true,
        _ => false
    };

    public static string UpdaterFolderPath = "update_downloader";
    public static string UpdaterTag = "Release1.4";

    // updater arguments
    private static string DownloadWebsite;
    private static string CopySource;
    private static string CloseOnComplete;

    public static void Initialize(string downloadWebsite, string copySource = "null", string closeOnComplete = "true")
    {
        DownloadWebsite = downloadWebsite;
        CopySource = copySource;
        CloseOnComplete = closeOnComplete;
    }

    public static void RunUpdater()
    {
        if (!IsPlatformSupported) return;

        Directory.CreateDirectory(UpdaterFolderPath);
        DownloadDownloader($"{UpdaterFolderPath}/AutoUpdater.exe").GetAwaiter().GetResult();

        var arguments = new[]
        {
            DownloadWebsite,
            Environment.ProcessPath.Remove(Environment.ProcessPath.Replace("\\", "/").LastIndexOf('/')),
            CloseOnComplete,
            CopySource,
            Environment.ProcessPath
        };

        Process.Start($"{UpdaterFolderPath}/AutoUpdater.exe", arguments);
    }

    private static async Task DownloadDownloader(string fileName)
    {
        using var client = new HttpClient();
        await using var response =
            await client.GetStreamAsync(
                $"https://github.com/SWCreeperKing/AutoUpdater/releases/download/{UpdaterTag}/AutoUpdater.exe");

        await using var fileStream = File.Create(fileName);
        await response.CopyToAsync(fileStream);
    }

    public static void CheckIfUpdateFinished()
    {
        if (Directory.Exists(UpdaterFolderPath))
        {
            UpdateFinished();
        }
    }

    public static void UpdateFinished()
    {
        File.Delete($"{UpdaterFolderPath}/AutoUpdater.exe");
        Directory.Delete(UpdaterFolderPath);
    }

    /// <inheritdoc cref="IsVersionJsonOutdated(string,System.Collections.Generic.Dictionary{string,string},out string)"/>
    public static bool IsVersionJsonLinkOutdated(string version, string versionJsonLink, out string download)
    {
        download = string.Empty;

        try
        {
            var client = new HttpClient();
            using var response = client.GetAsync(versionJsonLink).GetAwaiter().GetResult();
            using var content = response.Content;
            var stringContent = content.ReadAsStringAsync().GetAwaiter().GetResult();

            return IsVersionJsonOutdated(version, stringContent, out download);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <inheritdoc cref="IsVersionJsonOutdated(string,System.Collections.Generic.Dictionary{string,string},out string)"/>
    public static bool IsVersionJsonOutdated(string version, string versionJsonRaw, out string download)
    {
        download = string.Empty;

        var convertedJson =
            JsonConvert.DeserializeObject<(Dictionary<string, string>, Dictionary<string, int>)>(versionJsonRaw)!;
        return IsVersionJsonOutdated(version, convertedJson.Item1, convertedJson.Item2, out download);
    }

    /// <summary>
    /// version json format is like normal json
    /// here is an example of the json
    ///
    /// [
    ///     {
    ///         "Item1" :
    ///         {
    ///             "1.1" : "version 2 download link",
    ///             "1.0" : "version 1 download link"
    ///         },
    ///         "Item2" :
    ///         {
    ///             "1.1" : 1,
    ///             "1.0" : 0
    ///         }
    ///     }
    /// ]
    ///
    /// Item1: download links for each version
    /// Item2: number placing for versions, to see which version is higher than another
    ///         (this allows for custom version names and the removal of ambiguity)
    /// </summary>
    public static bool IsVersionJsonOutdated(string version, Dictionary<string, string> versionJson,
        Dictionary<string, int> versionMap, out string download)
    {
        download = string.Empty;
        var highestVersion = versionMap.Values.Max();
        if (versionMap[version] >= highestVersion) return false;

        download = versionJson[versionMap.First(kv => kv.Value == highestVersion).Key];
        return true;
    }
}