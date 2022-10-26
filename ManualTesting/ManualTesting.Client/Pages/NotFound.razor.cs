using ManualTesting.Client.Services;

namespace ManualTesting.Client;

public sealed partial class NotFound : PageComponentBase {
    [Inject]
    private NavigationManager NavigationManager { get; init; } = null!;

    [Inject]
    private IPreRenderFlag  PreRenderFlag { get; init; } = null!;


    private const int BAD_REQUEST_LIST_MAX_DISTANCE = 8;

    private static string[]? _urlList;
    private static string[] UrlList => _urlList ??= (from type in typeof(Program).Assembly.GetTypes()
                                                     let routeAttributes = type.GetCustomAttributes(typeof(RouteAttribute), inherit: false).OfType<RouteAttribute>()
                                                     from routeAttribute in routeAttributes
                                                     let template = routeAttribute.Template[1..]
                                                     where template != string.Empty
                                                     select template).ToArray();


    private (List<string>[], bool) SortUrls() {
        bool atLeastOneItem = false;
        List<string>[] buckets = new List<string>[BAD_REQUEST_LIST_MAX_DISTANCE];
        for (int i = 0; i < buckets.Length; i++)
            buckets[i] = new List<string>(UrlList.Length / 2);

        string path = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToLower();

        foreach (string url in UrlList) {
            int distance = url.ToLower().LevenshteinDistance(path);
            if (distance <= BAD_REQUEST_LIST_MAX_DISTANCE) {
                buckets[distance - 1].Add(url);
                atLeastOneItem = true;
            }
        }

        return (buckets, atLeastOneItem);
    }
}
