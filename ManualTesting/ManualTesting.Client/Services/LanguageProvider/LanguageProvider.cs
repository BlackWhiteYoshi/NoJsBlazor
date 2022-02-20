namespace ManualTesting.Client.Languages;

public class LanguageProvider : ILanguageProvider {
    private Language _language = Language.English;
    /// <summary>
    /// <para>The Value of this class.</para>
    /// <para>When value changes <see cref="OnLanguageChanged"/> get invoked.</para>
    /// </summary>
    public Language Language {
        get => _language;
        set {
            if (_language != value) {
                _language = value;
                OnLanguageChanged?.Invoke(value);
            }
        }
    }

    /// <summary>
    /// Invokes everytime <see cref="Language"/> is set to a new value.
    /// </summary>
    public event Action<Language>? OnLanguageChanged;

    /// <summary>
    /// Sets <see cref="Language"/> without notifying <see cref="OnLanguageChanged"/>.
    /// </summary>
    public Language SilentLanguageSetter {
        set {
            _language = value;
        }
    }
}
