namespace ManualTesting.Client;

/// <summary>
/// ConstantsBox for global static readonly Data or global static Methods
/// </summary>
public static class CBox {
    /// <summary>
    /// Key fot getting the value for language stored in local storage
    /// </summary>
    public const string COOKIE_KEY_LANGUAGE = "Language";


    /// <summary>
    /// calculates a distance for two strings
    /// algorithm taken from <see cref="https://en.wikipedia.org/wiki/Levenshtein_distance#Iterative_with_two_matrix_rows"/>
    /// </summary>
    /// <param name="s">first string</param>
    /// <param name="t">second string</param>
    /// <returns></returns>
    public static int LevenshteinDistance(this string s, string t) {
        // create two work vectors of integer distances
        int[] v0 = new int[t.Length + 1];
        int[] v1 = new int[t.Length + 1];

        // initialize v0 (the previous row of distances)
        // this row is A[0][i]: edit distance for an empty s
        // the distance is just the number of characters to delete from t
        for (int i = 0; i <= t.Length; i++)
            v0[i] = i;

        for (int i = 0; i < s.Length; i++) {
            // calculate v1 (current row distances) from the previous row v0

            // first element of v1 is A[i+1][0]
            // edit distance is delete (i+1) chars from s to match empty t
            v1[0] = i + 1;

            // use formula to fill in the rest of the row
            for (int j = 0; j < t.Length; j++) {
                // calculating costs for A[i+1][j+1]
                int deletionCost = v0[j + 1] + 1;
                int insertionCost = v1[j] + 1;
                int substitutionCost;
                if (s[i] == t[j])
                    substitutionCost = v0[j];
                else
                    substitutionCost = v0[j] + 1;

                // Take Minimum of deletionCost, insertionCost, substitutionCost
                v1[j + 1] = Math.Min(Math.Min(deletionCost, insertionCost), substitutionCost);
            }
            // copy v1 (current row) to v0 (previous row) for next iteration
            for (int j = 0; j < v1.Length; j++)
                v0[j] = v1[j];
        }
        // after the last copy, the results of v1 are equal v0
        return v0[t.Length];
    }

    /// <summary>
    /// Takes a raw cookieString and returns a key,value array.
    /// </summary>
    /// <param name="cookieString">string with ';' seperated cookies</param>
    /// <returns></returns>
    public static Dictionary<string, string> SplitCookies(string cookieString) {
        string[] cookies = cookieString.Split(';');

        Dictionary<string, string> cookieDic = new(cookies.Length);
        foreach (string cookie in cookies) {
            string[] pairs = cookie.Split('=');
            if (pairs.Length == 2)
                cookieDic.Add(pairs[0].Trim(), pairs[1].Trim());
        }

        return cookieDic;
    }
}
