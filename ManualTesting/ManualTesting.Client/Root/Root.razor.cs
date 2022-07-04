using ManualTesting.Client.Services;

namespace ManualTesting.Client;

public partial class Root : ServiceComponentBase<IRoot>, IRoot, IDisposable {
    [Inject, AllowNull]
    private IJSInProcessRuntime JsRuntime { get; init; }

    [Inject, AllowNull]
    private IPreRenderFlag PreRenderFlag { get; init; }

    [Inject, AllowNull]
    private ILanguageProvider Lang { get; init; }


    /// <summary>
    /// Initial value for ILanguageProvider.
    /// </summary>
    [Parameter]
    public Language? StartLanguage { private get; init; }

    [AllowNull]
    public PageComponentBase PageComponent { get; set; }

    public event Action<MouseEventArgs>? Click;


    protected override void OnInitialized() {
        base.OnInitialized();

        Lang.SilentLanguageSetter = InitLanguage();
        Lang.OnLanguageChanged += OnLanguageChanged;


        Language InitLanguage() {
            if (StartLanguage != null)
                return StartLanguage.Value;

            if (PreRenderFlag.Flag)
                return Default();

            Dictionary<string, string> cookies = CBox.SplitCookies(JsRuntime.Invoke<string>("GetCookies"));
            if (!cookies.TryGetValue(CBox.COOKIE_KEY_LANGUAGE, out string? languageString))
                return Default();
            
            if (!ILanguageProvider.TryParse(languageString, out Language language))
                return Default();

            return language;


            Language Default() => ILanguageProvider.GetLanguage(JsRuntime.Invoke<string>("GetBrowserLanguage"));
        }
    }

    public new void Dispose() {
        base.Dispose();
        Lang.OnLanguageChanged -= OnLanguageChanged;
    }


    private void OnLanguageChanged(Language language) {
        JsRuntime.InvokeVoid("SetHtmlLanguage", ILanguageProvider.GetAbbreviation(language));
    }
}
