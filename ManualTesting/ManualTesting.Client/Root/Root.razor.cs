using ManualTesting.Client.Languages;
using ManualTesting.Client.PreRendering;

namespace ManualTesting.Client;

public partial class Root : ComponentBase, IDisposable {
    [Inject, AllowNull]
    private IJSInProcessRuntime JsRuntime { get; init; }


    public PageComponentBase? PageComponent { get; set; }


    [AllowNull]
    private RootNavBar navBar;


    public event Action<MouseEventArgs>? MouseDown;
    public event Action<TouchEventArgs>? TouchStart;
    public event Action<MouseEventArgs>? MouseMove;
    public event Action<TouchEventArgs>? TouchMove;
    public event Action<MouseEventArgs>? MouseUp;
    public event Action<TouchEventArgs>? TouchEnd;


    protected override void OnInitialized() {
        Lang.SilentLanguageSetter = Language;
        Lang.OnLanguageChanged += OnLanguageChanged;
    }

    public void Dispose() {
        Lang.OnLanguageChanged -= OnLanguageChanged;
        GC.SuppressFinalize(this);
    }


    #region PreRendering

    [Inject, AllowNull]
    private IPreRenderFlag PreRenderFlag { get; init; }

    /// <summary>
    /// Is this App executed on the server or on the client.
    /// </summary>
    public bool PreRendering => PreRenderFlag.Flag;

    #endregion


    #region Language

    [Inject, AllowNull]
    private ILanguageProvider Lang { get; init; }

    /// <summary>
    /// <para>Initial value for ILanguageProvider.</para>
    /// <para>Derives from cookie when present.</para>
    /// </summary>
    [Parameter]
    public Language Language { private get; init; }

    private void OnLanguageChanged(Language language) {
        JsRuntime.InvokeVoid("SetHtmlLanguage", ILanguageProvider.GetAbbreviation(language));
        Rerender();
    }

    #endregion


    /// <summary>
    /// <para>This will notify the <see cref="Root"/> to Rerender.</para>
    /// </summary>
    public void Rerender() {
        InvokeAsync(StateHasChanged);
        
        navBar.Rerender();
        
        PageComponent?.Rerender();
    }
}
