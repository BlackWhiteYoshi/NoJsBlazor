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
    /// <para>Initial value for ILanguageProvider.</para>
    /// <para>Derives from cookie when present.</para>
    /// </summary>
    [Parameter]
    public Language Language { private get; init; } = Language.NotInitialized;

    [AllowNull]
    public PageComponentBase PageComponent { get; set; }


    public event Action<MouseEventArgs>? Click;
    public event Action<MouseEventArgs>? MouseDown;
    public event Action<TouchEventArgs>? TouchStart;
    public event Action<MouseEventArgs>? MouseMove;
    public event Action<TouchEventArgs>? TouchMove;
    public event Action<MouseEventArgs>? MouseUp;
    public event Action<TouchEventArgs>? TouchEnd;


    protected override void OnInitialized() {
        base.OnInitialized();
        Lang.SilentLanguageSetter = InitLanguage();
        Lang.OnLanguageChanged += OnLanguageChanged;


        Language InitLanguage() {
            if (Language != Language.NotInitialized)
                return Language;

            if (PreRenderFlag.Flag)
                return Language.English;

            Dictionary<string, string> cookies = CBox.SplitCookies(JsRuntime.Invoke<string>("GetCookies"));
            if (!cookies.TryGetValue(CBox.COOKIE_KEY_LANGUAGE, out string? languageString))
                return Language.English;
            
            if (!ILanguageProvider.TryParse(languageString, out Language language))
                return Language.English;

            return language;
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
