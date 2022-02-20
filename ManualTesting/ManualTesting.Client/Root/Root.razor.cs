using ManualTesting.Client.Languages;

namespace ManualTesting.Client;

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


    protected override void OnInitialized() {
        Lang.SilentLanguageSetter = Language;
        Lang.OnLanguageChanged += OnLanguageChanged;

        subscription = ComponentState.RegisterOnPersisting(PersistPreRendering);
        if (ComponentState.TryTakeFromJson<int>(KEY, out _))
            PreRendering = false;
        else
            PreRendering = true;
    }

    public void Dispose() {
        Lang.OnLanguageChanged -= OnLanguageChanged;
        subscription.Dispose();
        GC.SuppressFinalize(this);
    }


    #region Language

    [Inject]
    [AllowNull]
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


    #region PreRendering

    private const string KEY = "PreRendering";

    [Inject]
    [AllowNull]
    private PersistentComponentState ComponentState { get; init; }

    /// <summary>
    /// Is this App executed on the server or on the client.
    /// </summary>
    public bool PreRendering { get; private set; }

    PersistingComponentStateSubscription subscription;


    private Task PersistPreRendering() {
        ComponentState.PersistAsJson(KEY, 0);
        return Task.CompletedTask;
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
