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
    public Language Language { private get; init; }

    public PageComponentBase? PageComponent { get; set; }


    [AllowNull]
    private RootNavBar navBar;


    public event Action<MouseEventArgs>? Click;
    public event Action<MouseEventArgs>? MouseDown;
    public event Action<TouchEventArgs>? TouchStart;
    public event Action<MouseEventArgs>? MouseMove;
    public event Action<TouchEventArgs>? TouchMove;
    public event Action<MouseEventArgs>? MouseUp;
    public event Action<TouchEventArgs>? TouchEnd;


    protected override void OnInitialized() {
        base.OnInitialized();
        Lang.SilentLanguageSetter = Language;
        Lang.OnLanguageChanged += OnLanguageChanged;
    }

    public new void Dispose() {
        base.OnInitialized();
        Lang.OnLanguageChanged -= OnLanguageChanged;
    }


    private void OnLanguageChanged(Language language) {
        JsRuntime.InvokeVoid("SetHtmlLanguage", ILanguageProvider.GetAbbreviation(language));
        Rerender();
    }


    /// <summary>
    /// This will notify all components to Rerender.
    /// </summary>
    public void Rerender() {
        InvokeAsync(StateHasChanged);
        
        navBar.Rerender();
        
        PageComponent?.Rerender();
    }
}
