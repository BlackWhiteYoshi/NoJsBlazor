using System.Text.Json.Nodes;

namespace ManualTesting.PostGenerator;

/** config.json hints
 * 
 * working directory:
 *   path to published core folder
 * 
 * remove folder list:
 *   remove exceeding folders: "folderPath1, folderPath2, ..."
 *
 * remove file list:
 *   remove exceeding files: "filePath1, filePath2, ..."
 * 
 * zip exclude list startsWith:
 *   skips zipping paths that begins with given pattern: "startsWith1, startsWith2, ..."
 * 
 * zip exclude list endsWith:
 *   skips zipping paths that ends with given pattern: "endsWith1, endsWith2, ..."
 *
 */

file static class JsonNodeExtension {
    internal static JsonNode Get(this JsonNode node, string key) => node[key] ?? throw new ArgumentException($"Cannot find key '{key}' in json config.");
    internal static string GetString(this JsonNode node, string key) => (string?)node.Get(key) ?? throw new ArgumentException($"key '{key}' must be a string.");
    internal static bool GetBool(this JsonNode node, string key) => (bool?)node.Get(key) ?? throw new ArgumentException($"key '{key}' must be a boolean.");
    internal static string[] AsStringArray(this JsonNode node) => node.AsArray().Select((JsonNode? node) => (string?)node ?? throw new ArgumentException($"key '{node}' must be a string.")).ToArray();
}

public sealed class Config {
    public string WorkingDirectory { get; init; } = string.Empty;
    public string WorkingDirectoryWithTrailingSlash { get; init; } = string.Empty;
    public string PageFolderPath { get; init; } = string.Empty;
    public string PageFolderPathWithTrailingSlash { get; init; } = string.Empty;

    public string SiteUrl { get; init; } = string.Empty;

    public bool GenerateHtmlPages { get; init; } = false;

    public bool CreateRobotsTxt { get; init; } = false;

    public bool CreateSitemapXml { get; init; } = false;

    public bool RemoveFiles { get; init; } = false;
    public (string[] folders, string[] files) RemoveFileList { get; init; } = (Array.Empty<string>(), Array.Empty<string>());

    public bool MinifyCssJs { get; init; } = false;
    public (string[] startsWith, string[] endsWith) MinifyExcludeList { get; init; } = (Array.Empty<string>(), Array.Empty<string>());

    public bool ZipFiles { get; init; } = false;
    public (string[] startsWith, string[] endsWith) ZipExcludeList { get; init; } = (Array.Empty<string>(), Array.Empty<string>());


    public static Config FromJson(string json) {
        JsonNode root = JsonNode.Parse(json) ?? throw new Exception($"json is not in a valid format:\n{json}");
        
        string workingDirectory = root.GetString("working directory");
        if (!Directory.Exists(workingDirectory))
            throw new InvalidDataException($"working directory does not exist:\n{Path.Combine(Directory.GetCurrentDirectory(), workingDirectory)}");
        
        string relativePageFolderPath = root.Get("generate html pages").GetString("page folder");


        return new Config() {
            WorkingDirectory = workingDirectory,
            WorkingDirectoryWithTrailingSlash = $"{workingDirectory}{Path.DirectorySeparatorChar}",
            PageFolderPath = $"{workingDirectory}{Path.DirectorySeparatorChar}{relativePageFolderPath}",
            PageFolderPathWithTrailingSlash = $"{workingDirectory}{Path.DirectorySeparatorChar}{relativePageFolderPath}{Path.DirectorySeparatorChar}",

            SiteUrl = root.GetString("site url"),

            GenerateHtmlPages = root.Get("generate html pages").GetBool("enabled"),

            CreateRobotsTxt = root.GetBool("create robots.txt"),

            CreateSitemapXml = root.GetBool("create sitemap.xml"),

            RemoveFiles = root.Get("remove files").GetBool("enabled"),
            RemoveFileList = (root.Get("remove files").Get("folder list").AsStringArray(), root.Get("remove files").Get("file list").AsStringArray()),

            MinifyCssJs = root.Get("minify css/js").GetBool("enabled"),
            MinifyExcludeList = (root.Get("minify css/js").Get("exclude list startsWith").AsStringArray(), root.Get("minify css/js").Get("exclude list endsWith").AsStringArray()),

            ZipFiles = root.Get("zip files").GetBool("enabled"),
            ZipExcludeList = (root.Get("zip files").Get("exclude list startsWith").AsStringArray(), root.Get("zip files").Get("exclude list endsWith").AsStringArray())
        };
    }
}
