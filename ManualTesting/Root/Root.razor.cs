using ManualTesting.Languages;

namespace ManualTesting;

public partial class Root : ComponentBase, IDisposable {
    [Inject]
    [AllowNull]
    private IJSInProcessRuntime JsRuntime { get; init; }


    public PageComponentBase? PageComponent { get; set; }


    public event Action<MouseEventArgs>? MouseDown;
    public event Action<TouchEventArgs>? TouchStart;
    public event Action<MouseEventArgs>? MouseMove;
    public event Action<TouchEventArgs>? TouchMove;
    public event Action<MouseEventArgs>? MouseUp;
    public event Action<TouchEventArgs>? TouchEnd;


    [AllowNull]
    private RootNavBar navBar;

    [AllowNull]
    private RootFooter footer;


    #region Language

    [Inject]
    [AllowNull]
    private ILanguageProvider Lang { get; init; }

    private void OnLanguageChanged(Language language) {
        JsRuntime.InvokeVoid("SetHtmlLanguage", ILanguageProvider.GetAbbreviation(language));
        Rerender();
    }

    protected override void OnInitialized() {
        string answer = JsRuntime.Invoke<string>("localStorage.getItem", CBox.STORAGE_KEY_LANGUAGE);

        if (Enum.TryParse(answer, false, out Language language))
            if (0 <= language && language < (Language)Enum.GetValues(typeof(Language)).Length)
                goto LanguageIsAssigned;
        // else
        {
            // no or invalid stored enum
            language = ILanguageProvider.GetLanguage(JsRuntime.Invoke<string>("GetBrowserLanguage"));
            JsRuntime.InvokeVoid("localStorage.setItem", CBox.STORAGE_KEY_LANGUAGE, language);
        }

        LanguageIsAssigned:
        Lang.Language = language;
        Lang.OnLanguageChanged += OnLanguageChanged;
        JsRuntime.InvokeVoid("SetHtmlLanguage", ILanguageProvider.GetAbbreviation(language));
    }

    public void Dispose() {
        Lang.OnLanguageChanged -= OnLanguageChanged;
        GC.SuppressFinalize(this);
    }

    #endregion

    
    /// <summary>
    /// <para>This will notify the <see cref="Root"/> to Rerender.</para>
    /// </summary>
    public void Rerender() {
        InvokeAsync(StateHasChanged);
        
        navBar.Rerender();
        footer.Rerender();
        
        PageComponent?.Rerender();
    }
}
