using ManualTesting.Client.Services;

namespace ManualTesting.Client;

public partial class Root : ServiceComponentBase<IRoot>, IRoot, IDisposable {
    [Inject, AllowNull]
    private IJSRuntime JsRuntime { get; init; }

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

        Lang.OnLanguageChanged += OnLanguageChanged;
        Lang.SilentLanguageSetter = await InitLanguage();


        async ValueTask<Language> InitLanguage() {
            if (StartLanguage != null)
                return StartLanguage.Value;

            if (PreRenderFlag.Flag)
                return await Default();

            Dictionary<string, string> cookies = CBox.SplitCookies(await JsRuntime.InvokeAsync<string>("GetCookies"));
            if (!cookies.TryGetValue(CBox.COOKIE_KEY_LANGUAGE, out string? languageString))
                return await Default();

            if (!ILanguageProvider.TryParse(languageString, out Language language))
                return await Default();

            return language;


            async ValueTask<Language> Default() => ILanguageProvider.GetLanguage(await JsRuntime.InvokeAsync<string>("GetBrowserLanguage"));
        }
    }

    public new void Dispose() {
        base.Dispose();
        Lang.OnLanguageChanged -= OnLanguageChanged;
    }


    private void OnLanguageChanged(Language language) {
        _ = JsRuntime.InvokeVoidAsync("SetHtmlLanguage", ILanguageProvider.GetAbbreviation(language)).Preserve();
    }
}
