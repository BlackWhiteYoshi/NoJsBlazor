using ManualTesting.Client.Services;

namespace ManualTesting.Client;

public sealed partial class Root : ServiceComponentBase, IRoot, IDisposable {
    private const string ROOT_JS = "/Root/Root.razor.js";


    /// <summary>
    /// Initial value for ILanguageProvider.
    /// </summary>
    [Parameter]
    public Language? StartLanguage { get; set; }

    [AllowNull]
    public PageComponentBase PageComponent { get; set; }

    public event Action<MouseEventArgs>? Click;


    private readonly NavigationManager navigationManager;
    private readonly IJSModuleRuntime jsModuleRuntime;
    private readonly IPreRenderFlag preRenderFlag;
    private readonly ILanguageProvider lang;

    public Root(NavigationManager navigationManager, IJSModuleRuntime jsModuleRuntime, IPreRenderFlag preRenderFlag, ILanguageProvider lang) {
        this.navigationManager = navigationManager;
        this.jsModuleRuntime = jsModuleRuntime;
        this.preRenderFlag = preRenderFlag;
        this.lang = lang;
    }

    protected override async Task OnInitializedAsync() {
        _ = base.OnInitializedAsync();

        if (!preRenderFlag.Flag) {
            _ = jsModuleRuntime.PreLoadModule(CBox.SHARED_JS).Preserve();
            _ = jsModuleRuntime.PreLoadModule(ROOT_JS).Preserve();
        }

        lang.OnLanguageChanged += OnLanguageChanged;
        lang.Language = await InitLanguage();


        async ValueTask<Language> InitLanguage() {
            if (StartLanguage != null)
                return StartLanguage.Value;

            if (preRenderFlag.Flag)
                return await Default();

            Dictionary<string, string> cookies = CBox.SplitCookies(await jsModuleRuntime.InvokeTrySync<string>(CBox.SHARED_JS, "getCookies"));
            if (!cookies.TryGetValue(CBox.COOKIE_KEY_LANGUAGE, out string? languageString))
                return await Default();

            if (!ILanguageProvider.TryParse(languageString, out Language language))
                return await Default();

            return language;


            async ValueTask<Language> Default() => ILanguageProvider.GetLanguage(await jsModuleRuntime.InvokeTrySync<string>(ROOT_JS, "getBrowserLanguage"));
        }
    }

    public override void Dispose() {
        base.Dispose();
        lang.OnLanguageChanged -= OnLanguageChanged;
    }


    private void OnLanguageChanged(Language language) {
        _ = jsModuleRuntime.InvokeVoidTrySync(ROOT_JS, "setHtmlLanguage", ILanguageProvider.GetAbbreviation(language)).Preserve();
    }
}
