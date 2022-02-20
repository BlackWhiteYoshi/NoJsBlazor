namespace ManualTesting.Client.Languages;

public interface ILanguageProvider {
    /// <summary>
    /// <para>The Value of this class.</para>
    /// <para>When value changes <see cref="OnLanguageChanged"/> get invoked.</para>
    /// </summary>
    public Language Language { get; set; }
    /// <summary>
    /// Invokes everytime <see cref="Language"/> is set to a new value.
    /// </summary>
    public event Action<Language> OnLanguageChanged;
    /// <summary>
    /// Sets <see cref="Language"/> without notifying <see cref="OnLanguageChanged"/>.
    /// </summary>
    public Language SilentLanguageSetter { set; }


    /// <summary>
    /// Gives the display string dependent on current value of <see cref="Language"/>.
    /// </summary>
    /// <param name="english">text to print if value is english</param>
    /// <param name="german">text to print if value is german</param>
    /// <returns></returns>
    public string Print(string english, string german) {
        return Language switch {
            Language.English => english,
            Language.German => german,
            _ => throw new ArgumentException("No Language Selected")
        };
    }


    /// <summary>
    /// <para>Parses a string to an enum.</para>
    /// <para>Only the first 2 characters are checked for determining the result.</para>
    /// <para>if string is less than 2 characters or no match is found, the result will be English.</para>
    /// </summary>
    /// <param name="language"></param>
    /// <returns></returns>
    public static Language GetLanguage(string languageAbbreviation) {
        if (languageAbbreviation.Length < 2)
            return Language.English;

        return (languageAbbreviation[0], languageAbbreviation[1]) switch {
            ('d', 'e') => Language.German,
            _ => Language.English
        };
    }

    /// <summary>
    /// <para>Gives the correspending short form for the given language.</para>
    /// <para>e.g. English -> "en", German -> "de"</para>
    /// </summary>
    /// <param name="language"></param>
    /// <returns></returns>
    public static string GetAbbreviation(Language language) {
        return language switch {
            Language.German => "de",
            Language.English => "en",
            _ => throw new Exception("Language Error")
        };
    }
}
