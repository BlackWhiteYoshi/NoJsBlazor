using Microsoft.JSInterop;
using System;
using System.Runtime.CompilerServices;

namespace ManualTesting {
    /// <summary>
    /// ConstantsBox for global static readonly Data or global static Methods
    /// </summary>
    public static class CBox {
        /// <summary>
        /// Key fot getting the value for language stored in local storage
        /// </summary>
        public const string StorageKeyLanguage = "Language";

        /// <summary>
        /// <para>Random Number Generator.</para>
        /// <para>By using the same Random object, a equal distribution is provided.</para>
        /// </summary>
        public static Random Rng { get; } = new();


        /// <summary>
        /// Gives the display string dependent on the current language
        /// </summary>
        /// <param name="german">text to print if value is german</param>
        /// <param name="english">text to print if value is english</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string LPrint(string english, string german) {
            return DBox.Language switch {
                Language.German => german,
                Language.English => english,
                _ => throw new ArgumentException("No Language Selected")
            };
        }

        /// <summary>
        /// Sets the Language, but also sets the "lang" property in the "html" tag
        /// </summary>
        /// <param name="language"></param>
        /// <param name="jsSyncRuntime"></param>
        public static void SetLanguage(Language language, IJSInProcessRuntime jsSyncRuntime) {
            DBox.Language = language;
            jsSyncRuntime.InvokeVoid("SetHtmlLanguage", language.GetAbbreviation());
        }

        /// <summary>
        /// <para>Parses a string to an enum.</para>
        /// <para>Only the first 2 characters are checked for determining the result.</para>
        /// <para>if string is less than 2 characters or no match is found, the result will be English.</para>
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static Language GetLanguage(string language) {
            if (language.Length < 2)
                return Language.English;

            return language[0..2] switch {
                "de" => Language.German,
                _ => Language.English
            };
        }

        /// <summary>
        /// <para>Gives the correspending short form for the given language.</para>
        /// <para>e.g. English -> "en", German -> "de"</para>
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string GetAbbreviation(this Language language) {
            return language switch {
                Language.German => "de",
                Language.English => "en",
                _ => throw new Exception("Language Error")
            };
        }


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
    }
}
