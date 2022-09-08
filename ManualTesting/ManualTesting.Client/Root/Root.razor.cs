using ManualTesting.Client.Services;

namespace ManualTesting.Client;

public partial class Root : ServiceComponentBase<IRoot>, IRoot, IDisposable {
    private const string ROOT_JS = "/Root/Root.razor.js";


    [Inject, AllowNull]
    private IJSModuleRuntime JsModuleRuntime { get; init; }

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


    protected override async Task OnInitializedAsync() {
        _ = base.OnInitializedAsync();

        if (!PreRenderFlag.Flag) {
            _ = JsModuleRuntime.PreLoadModule(CBox.SHARED_JS).Preserve();
            _ = JsModuleRuntime.PreLoadModule(ROOT_JS).Preserve();
        }

        Lang.OnLanguageChanged += OnLanguageChanged;
        Lang.Language = await InitLanguage();


        async ValueTask<Language> InitLanguage() {
            if (StartLanguage != null)
                return StartLanguage.Value;

            if (PreRenderFlag.Flag)
                return await Default();

            Dictionary<string, string> cookies = CBox.SplitCookies(await JsModuleRuntime.InvokeTrySync<string>(CBox.SHARED_JS, "getCookies"));
            if (!cookies.TryGetValue(CBox.COOKIE_KEY_LANGUAGE, out string? languageString))
                return await Default();

            if (!ILanguageProvider.TryParse(languageString, out Language language))
                return await Default();

            return language;


            async ValueTask<Language> Default() => ILanguageProvider.GetLanguage(await JsModuleRuntime.InvokeTrySync<string>(ROOT_JS, "getBrowserLanguage"));
        }
    }

    public new void Dispose() {
        base.Dispose();
        Lang.OnLanguageChanged -= OnLanguageChanged;
    }


    private void OnLanguageChanged(Language language) {
        _ = JsModuleRuntime.InvokeVoidTrySync(ROOT_JS, "setHtmlLanguage", ILanguageProvider.GetAbbreviation(language)).Preserve();
    }
}
