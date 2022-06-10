﻿using ManualTesting.Client.Services;

namespace ManualTesting.Client;

public partial class NotFound : PageComponentBase {
    [Inject, AllowNull]
    private NavigationManager NavigationManager { get; init; }

    [Inject, AllowNull]
    private IPreRenderFlag  PreRenderFlag { get; init; }


    private const int BAD_REQUEST_LIST_MAX_DISTANCE = 8;

    private static string[]? _urlList;
    private static string[] UrlList => _urlList ??= (from type in typeof(Program).Assembly.GetTypes()
                                                     let routeAttributes = type.GetCustomAttributes(typeof(RouteAttribute), inherit: false).OfType<RouteAttribute>()
                                                     from routeAttribute in routeAttributes
                                                     let template = routeAttribute.Template[1..]
                                                     where template != string.Empty
                                                     select template).ToArray();


    private unsafe void SortAndRenderUrls(RenderTreeBuilder builder) {
        /**
         *
         * BucketSort: only elements with distance of 1 to MAX_DISTANCE will be listed
         * index gives the distance, e.g. buckets[2] gives the list of items with distance 2
         * bucket 0 is special: it contains the current lengths of the other buckets e.g. buckets[0][3] is basically buckets[3].Length
         *
         *
         * BAD_REQUEST_LIST_MAX_DISTANCE - times
         * 1,2..UrlLength;1,2..UrlLength;1,2..UrlLength;...;1,2..UrlLength
         *
         * list of buckets            => stackalloc int[BAD_REQUEST_LIST_MAX_DISTANCE * UrlList.Length]
         * current length of a bucket => stackalloc int[BAD_REQUEST_LIST_MAX_DISTANCE];
         * pointer on start of array  => stackalloc int*[BAD_REQUEST_LIST_MAX_DISTANCE + 1];
         *
         *
         * list (BAD_REQUEST_LIST_MAX_DISTANCE = 16, Url.Length = 6)
         * [1] 0,1,2,3,4,5,
         * [2] 0,1,2,3,4,5,
         * [3] 0,1,2,3,4,5,
         * [4] 0,1,2,3,4,5,
         * [5] 0,1,2,3,4,5,
         * [6] 0,1,2,3,4,5,
         * [7] 0,1,2,3,4,5,
         * [8] 0,1,2,3,4,5,
         * [9] 0,1,2,3,4,5,
         * [10] 0,1,2,3,4,5,
         * [11] 0,1,2,3,4,5,
         * [12] 0,1,2,3,4,5,
         * [13] 0,1,2,3,4,5,
         * [14] 0,1,2,3,4,5,
         * [15] 0,1,2,3,4,5,
         * [16] 0,1,2,3,4,5,
         * [0] 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16
         *
         * buckets
         * 0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16
         *
         * buckets[0] is the array with the current size of the buckets
         * buckets[n] is the bucket with distance n
         *
         **/


        // bucketCount * bucketEntrySlots + sizeBucket
        int* fields = stackalloc int[BAD_REQUEST_LIST_MAX_DISTANCE * UrlList.Length + BAD_REQUEST_LIST_MAX_DISTANCE + 1];

        // pointers for each bucket
        int** buckets = stackalloc int*[BAD_REQUEST_LIST_MAX_DISTANCE + 1];
        // index 0 is size-bucket
        buckets[0] = (fields + BAD_REQUEST_LIST_MAX_DISTANCE * UrlList.Length);
        // initialize all fields of size-bucket with size 0, index 0 is omitted/not used
        for (int i = 1; i <= BAD_REQUEST_LIST_MAX_DISTANCE; i++)
            buckets[0][i] = 0;
        // initialize other bucket pointer
        for (int i = 1; i <= BAD_REQUEST_LIST_MAX_DISTANCE; i++)
            buckets[i] = fields + (i - 1) * UrlList.Length;


        string path = NavigationManager.ToBaseRelativePath(NavigationManager.Uri).ToLower();
        bool atLeastOneItem = false;

        for (int i = 0; i < UrlList.Length; i++) {
            int distance = UrlList[i].ToLower().LevenshteinDistance(path);

            if (distance <= 0)
                throw new Exception("invalid UrlList in NotFound.");

            if (distance <= BAD_REQUEST_LIST_MAX_DISTANCE) {
                buckets[distance][buckets[0][distance]++] = i;
                atLeastOneItem = true;
            }
        }


        RenderUrls(builder, buckets, atLeastOneItem);
    }
}
