namespace ManualTesting;

public partial class Root : ComponentBase {
    [Inject]
    [AllowNull]
    public IJSInProcessRuntime JsRuntime { get; init; }

    public PageComponentBase? PageComponent { get; set; }

    public event Action<MouseEventArgs>? MouseDown;
    public event Action<TouchEventArgs>? TouchStart;
    public event Action<MouseEventArgs>? MouseMove;
    public event Action<TouchEventArgs>? TouchMove;
    public event Action<MouseEventArgs>? MouseUp;
    public event Action<TouchEventArgs>? TouchEnd;

    [AllowNull]
    private RootFooter footer;


    protected override void OnInitialized() {
        string answer = JsRuntime.Invoke<string>("localStorage.getItem", CBox.StorageKeyLanguage);


        if (Enum.TryParse(answer, false, out Language language))
            if (0 <= language && language < (Language)Enum.GetValues(typeof(Language)).Length)
                goto LanguageIsAssigned;

        // no or invalid stored enum
        language = CBox.GetLanguage(JsRuntime.Invoke<string>("GetBrowserLanguage"));
        JsRuntime.InvokeVoid("localStorage.setItem", CBox.StorageKeyLanguage, language);

        LanguageIsAssigned:
        CBox.SetLanguage(language, JsRuntime);
    }

    /// <summary>
    /// <para>This will notify the <see cref="Root"/> to Rerender.</para>
    /// </summary>
    public void Rerender() {
        StateHasChanged();
        // navbar.Rerender() is missing, because it is currently not needed. So technically not everything is rerendered
        footer.Rerender();
        PageComponent?.Rerender();
    }
}
