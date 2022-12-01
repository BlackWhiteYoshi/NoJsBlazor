using System.Timers;

namespace NoJsBlazor;

/// <summary>
/// <para>A Carousel like in Bootstrap. It holds <see cref="RenderFragment"/> as items. </para>
/// <para>It can also display an overlay, control-arrows, item indicators and a play/stop button.</para>
/// </summary>
public sealed partial class Carousel : ListholdingComponentBase<CarouselItem>, IDisposable {
    /// <summary>
    /// <para>Index of the active item at the beginning.</para>
    /// <para>Default is 0.</para>
    /// </summary>
    [Parameter]
    public int ActiveStart { get; set; } = 0;

    /// <summary>
    /// <para>Type of swapping animation.</para>
    /// <para>Default is FadeOut.</para>
    /// </summary>
    [Parameter]
    public CarouselAnimation Animation { get; set; } = CarouselAnimation.FadeOut;

    /// <summary>
    /// <para>Waiting time before beginning swap animation in ms.</para>
    /// <para>Default is 6000.</para>
    /// </summary>
    [Parameter]
    public double IntervalTime {
        get => interval.Interval;
        set {
            if (value == interval.Interval)
                return;

            interval.Interval = value;
        }
    }

    /// <summary>
    /// <para>Starts interval after [AutoStartTime] ms, if interval not running and no action occurs.</para>
    /// <para>Value of 0 deactivates autostart.</para>
    /// <para>Default is 0.</para>
    /// </summary>
    [Parameter]
    public double AutoStartTime {
        get {
            if (_isAutoStartTimeNull)
                return 0.0;
            else
                return autoStart.Interval;
        }
        set {
            if (value == 0.0)
                _isAutoStartTimeNull = true;
            else {
                _isAutoStartTimeNull = false;
                if (value == autoStart.Interval)
                    return;

                autoStart.Interval = value;
                autoStart.Stop();
                autoStart.Start();
            }
        }
    }
    private bool _isAutoStartTimeNull = true;

    /// <summary>
    /// <para>Carousel Interval starts at beginning.</para>
    /// <para>Default is true.</para>
    /// </summary>
    [Parameter]
    public bool BeginRunning { get; set; } = true;

    /// <summary>
    /// <para>Next/Previous Arrows available</para>
    /// <para>Default is true.</para>
    /// </summary>
    [Parameter]
    public bool ControlArrowsEnable { get; set; } = true;

    /// <summary>
    /// <para>Indicators available</para>
    /// <para>Default is true.</para>
    /// </summary>
    [Parameter]
    public bool IndicatorsEnable { get; set; } = true;

    /// <summary>
    /// <para>PlayButton available</para>
    /// <para>Default is true.</para>
    /// </summary>
    [Parameter]
    public bool PlayButtonEnable { get; set; } = true;

    /// <summary>
    /// <para>Content of the <see cref="Carousel"/>.</para>
    /// <para>This should be a list of <see cref="CarouselItem"/> objects.</para>
    /// </summary>
    [Parameter, EditorRequired]
    public required RenderFragment Items { get; set; }

    /// <summary>
    /// Html that will be rendered in the overlay section.
    /// </summary>
    [Parameter]
    public RenderFragment? Overlay { get; set; }

    /// <summary>
    /// <para>Fires every time after <see cref="Active">active</see> item changed.</para>
    /// <para>Parameter is index of the new active item.</para>
    /// </summary>
    [Parameter]
    public EventCallback<int> OnActiveChanged { get; set; }

    /// <summary>
    /// <para>Fires every time after the <see cref="Running">Running</see> state is set.</para>
    /// <para>Parameter indicates if the carousel is currently running.</para>
    /// </summary>
    [Parameter]
    public EventCallback<bool> OnRunningChanged { get; set; }

    /// <summary>
    /// Captures unmatched values
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? Attributes { get; set; }


    private int active;
    /// <summary>
    /// The Index of the current Active Item.
    /// </summary>
    public int Active {
        get => active;
        set {
            if (active == value)
                return;

            SwapActive(Active, value, value - Active);
        }
    }

    /// <summary>
    /// Indicates if the interval of the carousel is currently running.
    /// </summary>
    public bool Running => interval.Enabled;

    /// <summary>
    /// Inidicates if the autoStart Timer is currently ticking.
    /// </summary>
    public bool AutoStartRunning => autoStart.Enabled;


    private readonly System.Timers.Timer interval = new();
    private readonly System.Timers.Timer autoStart = new();


    public Carousel() {
        interval.Interval = 6000.0;
        interval.Elapsed += Interval;
        interval.AutoReset = true;

        autoStart.Elapsed += AutoStart;
        autoStart.AutoReset = false;
    }
    
    protected override void OnInitialized() => active = ActiveStart;

    private bool firstRender = true;
    protected override void OnAfterRender(bool firstRender) {
        if (firstRender) {
            if (BeginRunning)
                StartInterval();
            else if (AutoStartTime > 0.0)
                autoStart.Start();
        }

        this.firstRender = false;
    }

    /// <summary>
    /// Disposes Interval Timer and Autostart Timer.
    /// </summary>
    public void Dispose() {
        interval?.Dispose();
        autoStart?.Dispose();
    }


    /// <summary>
    /// <para>Finds the indexes of the 2 given CarouselItems and then executes <see cref="SwapCarouselItems(int, int)"/>.</para>
    /// <para>If one of the items is not present in the CarouselItem list, an <see cref="ArgumentException"/> is thrown.</para>
    /// </summary>
    /// <param name="carouselItem1"></param>
    /// <param name="carouselItem2"></param>
    /// <exception cref="ArgumentException"></exception>
    public void SwapCarouselItems(CarouselItem carouselItem1, CarouselItem carouselItem2) {
        int index1;
        for (index1 = 0; index1 < childList.Count; index1++)
            if (childList[index1] == carouselItem1)
                break;
        if (index1 == childList.Count)
            throw new ArgumentException("first parameter is not an item of this carousel");

        int index2;
        for (index2 = 0; index2 < childList.Count; index2++)
            if (childList[index2] == carouselItem2)
                break;
        if (index2 == childList.Count)
            throw new ArgumentException("second parameter is not an item of this carousel");

        SwapCarouselItems(index1, index2);
    }

    /// <summary>
    /// <para>Swaps the 2 items in the CarouselItem list at the given indexes.</para>
    /// <para>The active item is preserved.<br />
    /// e.g. 0 is active, swap(0, 1)  -&gt; 1 will be active, so the active item won't change.</para>
    /// </summary>
    /// <param name="index1"></param>
    /// <param name="index2"></param>
    public void SwapCarouselItems(int index1, int index2) {
        (childList[index1], childList[index2]) = (childList[index2], childList[index1]);

        if (active == index1)
            active = index2;
        else if (active == index2)
            active = index1;
    }


    #region ListholdingComponentBase functions

    internal override void Add(CarouselItem carouselItem) {
        if (firstRender)
            carouselItem.Active = ChildCount == ActiveStart;
        else
            if (ChildCount == 0) {
                active = 0;
                carouselItem.Active = true;
            }

        base.Add(carouselItem);
    }

    internal override void Remove(CarouselItem carouselItem) {
        if (childList.Count == 1) {
            active = -1;
            childList.RemoveAt(0);
            StateHasChanged();
            return;
        }

        int index;
        for (index = 0; index < childList.Count; index++)
            if (childList[index] == carouselItem)
                break;

        if (active > index)
            active--;
        else if (Active == index) {
            if (active == 0)
                childList[Active + 1].Active = true;
            else {
                active--;
                childList[Active].Active = true;
            }
        }

        childList.RemoveAt(index);
        StateHasChanged();
    }

    #endregion


    #region input Funtions

    private void PrevButton(MouseEventArgs e) {
        StopInterval();
        Prev();
    }

    private void NextButton(MouseEventArgs e) {
        StopInterval();
        Next();
    }

    private void IndicatorButton(int indicatorIndex) {
        StopInterval();
        Active = indicatorIndex;
    }

    private void PlayButton(MouseEventArgs e) {
        if (!interval.Enabled)
            StartInterval();
        else
            StopInterval();
    }

    #endregion


    private void Prev() {
        int next = Active - 1;
        if (next < 0)
            next = ChildCount - 1;

        SwapActive(Active, next, -1);
    }

    private void Next() {
        int next = Active + 1;
        if (next == ChildCount)
            next = 0;

        SwapActive(Active, next, 1);
    }

    private void SwapActive(int active, int next, int direction) {
        if (active == next)
            return;

        switch (Animation) {
            case CarouselAnimation.FadeOut:
                childList[active].Active = false;
                childList[next].Active = true;
                break;

            case CarouselAnimation.Slide:
            case CarouselAnimation.SlideRotate:
                int sgnDirection = Math.Sign(direction);
                for (int i = 0; i != direction; i += sgnDirection)
                    childList[next - i].Rotate(direction - i, direction);
                childList[active].Rotate(0, direction);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(CarouselAnimation), "Enum is out of range");
        }

        this.active = next;
        InvokeAsync(StateHasChanged);
        OnActiveChanged.InvokeAsync(Active);
    }


    #region Interval / AutoStart

    /// <summary>
    /// Starts the interval-timer that automatically change to next item.
    /// </summary>
    public void StartInterval() {
        if (interval.Enabled)
            return;

        autoStart.Stop();
        interval.Start();

        InvokeAsync(StateHasChanged);
        OnRunningChanged.InvokeAsync(true);
    }

    /// <summary>
    /// Stops the interval-timer, so the current item will not automatically change.
    /// </summary>
    public void StopInterval() {
        if (!interval.Enabled)
            return;

        interval.Stop();
        if (AutoStartTime > 0.0) {
            autoStart.Stop();
            autoStart.Start();
        }

        InvokeAsync(StateHasChanged);
        OnRunningChanged.InvokeAsync(false);
    }

    private void Interval(object? source, ElapsedEventArgs e) => Next();

    private void AutoStart(object? source, ElapsedEventArgs e) {
        if (!interval.Enabled)
            StartInterval();
    }

    #endregion
}
