using System.Timers;

namespace NoJsBlazor;

/// <summary>
/// <para>A Carousel like in Bootstrap. It holds <see cref="RenderFragment"/> as items. </para>
/// <para>It can also display an overlay, control-arrows, item indicators and a play/stop button.</para>
/// </summary>
public sealed partial class Carousel : ListholdingComponentBase<CarouselItem>, IDisposable {
    private struct Item {
        /// <summary>
        /// Renderable content of this item.
        /// </summary>
        public CarouselItem carouselItem;
        /// <summary>
        /// It is either 20 or 10.
        /// </summary>
        public int zIndex;
        /// <summary>
        /// It is either 1 or 0.
        /// </summary>
        public int opacity;
        /// <summary>
        /// It is either Fadout=0, Slide=+-100, SlideRotate=+-50 or 0.
        /// </summary>
        public float translateX;
        /// <summary>
        /// It is either +-90 or 0.
        /// </summary>
        public int rotateY;
        /// <summary>
        /// It is either "slide" or string.Empty.
        /// </summary>
        public string slideClass;
        /// <summary>
        /// It is eiher 1.0f or 0.5f.
        /// </summary>
        public float indicatorOpacity;
        /// <summary>
        /// It is either 100 or 0.
        /// </summary>
        public int progressBar;
        /// <summary>
        /// It is either set to Interval time or 0.
        /// </summary>
        public int progressBarTransition;
    }

    private Item[] ItemContainer = Array.Empty<Item>();
    /// <summary>
    /// The Number of Items currently in this carousel.
    /// </summary>
    public int ItemCount => ItemContainer.Length;


    #region Parameters & Fields

    /// <summary>
    /// <para>Index of the active item at the beginning.</para>
    /// <para>Default is 0.</para>
    /// </summary>
    [Parameter]
    public int ActiveStart { get; init; } = 0;
    /// <summary>
    /// The Index of the current Active Item.
    /// </summary>
    public int Active { get; private set; }

    /// <summary>
    /// <para>Waiting time before beginning swap animation in ms.</para>
    /// <para>Default is 6000.</para>
    /// </summary>
    [Parameter]
    public int IntervalTime { get; init; } = 6000;

    /// <summary>
    /// <para>Type of swapping animation.</para>
    /// <para>Default is FadeOut.</para>
    /// </summary>
    [Parameter]
    public CarouselAnimation Animation { get; init; } = CarouselAnimation.FadeOut;
    /// <summary>
    /// Gives the current Animation.
    /// </summary>
    public CarouselAnimation CurrAnimation { get; private set; }

    /// <summary>
    /// <para>Starts interval after [AutoStartTime] ms, if interval not running and no action occurs.</para>
    /// <para>Value of 0 deactivates autostart.</para>
    /// <para>Default is 0.</para>
    /// </summary>
    [Parameter]
    public int AutoStartTime { get; init; } = 0;
    /// <summary>
    /// Holds the current value of the time to autoStart.
    /// </summary>
    public int CurrAutoStartTime { get; private set; }
    /// <summary>
    /// Inidicates if the autoStart Timer is currently ticking.
    /// </summary>
    public bool AutoStartRunning => autoStart.Enabled;

    /// <summary>
    /// <para>Carousel Interval starts at beginning.</para>
    /// <para>Default is true.</para>
    /// </summary>
    [Parameter]
    public bool BeginRunning { get; init; } = true;

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
    [Parameter]
    public RenderFragment? Items { get; set; }

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
    public Dictionary<string, object>? Attributes { get; set; }


    /// <summary>
    /// Indicates if the interval of the carousel is currently running.
    /// </summary>
    public bool Running => interval.Enabled;


    private readonly System.Timers.Timer interval = new();
    private readonly System.Timers.Timer autoStart = new();

    #endregion


    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (firstRender) {
            Initialize();
            StateHasChanged();

            if (BeginRunning) {
                // first render has no indicators because ChildCount = 0 
                // a second render is needed to render indicators with progress 0
                // then start timer, which sets progress to full (with transition)
                await Task.Yield();
                StartInterval();
            }
            else if (CurrAutoStartTime > 0)
                autoStart.Start();
        }
    }

    /// <summary>
    /// Disposes Interval Timer and Autostart Timer.
    /// </summary>
    public void Dispose() {
        interval?.Dispose();
        autoStart?.Dispose();
    }


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
        SwapItems(Active, indicatorIndex, indicatorIndex - Active);
    }

    private void PlayButton(MouseEventArgs e) {
        if (!interval.Enabled)
            StartInterval();
        else
            StopInterval();
    }

    #endregion


    #region private Functions

    private void Initialize() {
        ItemContainer = new Item[childList.Count];
        Active = ActiveStart;
        CurrAnimation = Animation;
        interval.Interval = IntervalTime;
        interval.Elapsed += Interval;
        interval.AutoReset = true;

        CurrAutoStartTime = AutoStartTime;
        if (CurrAutoStartTime > 0)
            autoStart.Interval = CurrAutoStartTime;
        autoStart.Elapsed += AutoStart;
        autoStart.AutoReset = false;


        for (int i = 0; i < ItemContainer.Length; i++) {
            ItemContainer[i].carouselItem = childList[i];
            ItemContainer[i].zIndex = 10;
            ItemContainer[i].indicatorOpacity = 0.5f;
            ItemContainer[i].progressBar = 0;
            ItemContainer[i].progressBarTransition = 0;
            ItemContainer[i].slideClass = string.Empty;
        }
        // set first one active
        ItemContainer[Active].zIndex = 20;
        ItemContainer[Active].indicatorOpacity = 1.0f;

        switch (CurrAnimation) {
            case CarouselAnimation.FadeOut:
                for (int i = 0; i < ItemContainer.Length; i++) {
                    ItemContainer[i].opacity = 0;
                    ItemContainer[i].translateX = 0;
                    ItemContainer[i].rotateY = 0;
                }
                // set first one active
                ItemContainer[Active].opacity = 1;
                break;

            case CarouselAnimation.Slide:
                for (int i = 0; i < ItemContainer.Length; i++) {
                    ItemContainer[i].opacity = 1;
                    ItemContainer[i].translateX = 90;
                    ItemContainer[i].rotateY = 0;
                }
                // set first one active
                ItemContainer[Active].translateX = 0;
                break;

            case CarouselAnimation.SlideRotate:
                for (int i = 0; i < ItemContainer.Length; i++) {
                    ItemContainer[i].opacity = 1;
                    ItemContainer[i].translateX = 50;
                    ItemContainer[i].rotateY = 90;
                }
                // set first one active
                ItemContainer[Active].translateX = 0;
                ItemContainer[Active].rotateY = 0;
                break;
        }
    }

    private void Prev() {
        int next = Active - 1;
        if (next < 0)
            next = ItemContainer.Length - 1;

        SwapItems(Active, next, -1);
    }

    private void Next() {
        int next = (Active + 1) % ItemContainer.Length;

        SwapItems(Active, next, 1);
    }

    private async void SwapItems(int active, int next, int direction) {
        ItemContainer[active].progressBarTransition = 0;
        ItemContainer[active].progressBar = 0;

        if (active == next)
            return;

        ItemContainer[active].zIndex = 10;
        ItemContainer[next].zIndex = 20;
        ItemContainer[active].indicatorOpacity = 0.5f;
        ItemContainer[next].indicatorOpacity = 1.0f;

        switch (CurrAnimation) {
            case CarouselAnimation.FadeOut:
                ItemContainer[active].opacity = 0;
                ItemContainer[next].opacity = 1;
                break;

            case CarouselAnimation.Slide:
            case CarouselAnimation.SlideRotate:
                int translateX;
                int rotateY;
                if (CurrAnimation == CarouselAnimation.Slide) {
                    translateX = 100;
                    rotateY = 0;
                }
                else {
                    translateX = 50;
                    rotateY = 90;
                }
                int sgnDirection = Math.Sign(direction);

                for (int i = 0; i != direction; i += sgnDirection) {
                    ItemContainer[next - i].slideClass = string.Empty;
                    ItemContainer[next - i].translateX = translateX * (direction - i);
                    ItemContainer[next - i].rotateY = rotateY * (direction - i);
                }

                await InvokeAsync(StateHasChanged);
                // TODO waiting of rendering instead of waiting 30ms
                await Task.Delay(30);

                ItemContainer[next].slideClass = "slide";
                ItemContainer[next].translateX = 0;
                ItemContainer[next].rotateY = 0;

                for (int i = 0; i != direction; i += sgnDirection) {
                    ItemContainer[active + i].slideClass = "slide";
                    ItemContainer[active + i].translateX = -translateX * (direction - i);
                    ItemContainer[active + i].rotateY = -rotateY * (direction - i);
                }

                break;

            default:
                throw new Exception("pigs have learned to fly");
        }

        _ = InvokeAsync(StateHasChanged);
        Active = next;
        _ = OnActiveChanged.InvokeAsync(Active);
    }

    private void StopInterval() {
        interval.Stop();
        ItemContainer[Active].progressBarTransition = 0;
        ItemContainer[Active].progressBar = 0;
        if (CurrAutoStartTime > 0) {
            autoStart.Stop();
            autoStart.Start();
        }
        _ = OnRunningChanged.InvokeAsync(false);
    }


    #region Interval / AutoStart

    private void StartInterval() {
        ItemContainer[Active].progressBarTransition = IntervalTime;
        ItemContainer[Active].progressBar = 100;
        interval.Start();
        InvokeAsync(StateHasChanged);
        _ = OnRunningChanged.InvokeAsync(true);
    }

    private void Interval(object? source, ElapsedEventArgs e) {
        int next = (Active + 1) % ItemContainer.Length;
        ItemContainer[next].progressBarTransition = IntervalTime;
        ItemContainer[next].progressBar = 100;
        Next();
        InvokeAsync(StateHasChanged);
    }

    private void AutoStart(object? source, ElapsedEventArgs e) {
        if (!interval.Enabled)
            StartInterval();
    }

    #endregion

    #endregion


    #region public Functions

    /// <summary>
    /// <para>If one of theses parameters are changed manually, this method should be called:</para>
    /// <para>CarouselItem, Active, Animation, Running, IntervalTime, AutoStartTime</para>
    /// </summary>
    public void ReInitialize() {
        Initialize();
        if (!Running && BeginRunning)
            StartInterval();

        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// <para>Adds an item to the carousel.</para>
    /// <para>Default (index = -1) is at the end of the carousel.</para>
    /// </summary>
    /// <param name="item">RenderFragment to Render</param>
    /// <param name="index">0 = first item, Length or -1 = last item</param>
    public void AddItem(CarouselItem item, int index = -1) {
        if (index < -1)
            throw new IndexOutOfRangeException("index must be equal or greater -1");

        if (index > ItemContainer.Length)
            throw new IndexOutOfRangeException($"index must be less or equal Length");

        if (index == -1)
            index = ItemContainer.Length;

        // create array with one more space and copy array in the new array with one space at position
        Item[] items = new Item[ItemContainer.Length + 1];
        int i = 0;
        for (; i < index; i++)
            items[i] = ItemContainer[i];
        for (; i < ItemContainer.Length; i++)
            items[i + 1] = ItemContainer[i];

        // fill space position with default values
        items[index].carouselItem = item;
        items[index].indicatorOpacity = 0.5f;
        items[index].progressBar = 0;
        items[index].progressBarTransition = 0;
        items[index].slideClass = string.Empty;
        switch (CurrAnimation) {
            case CarouselAnimation.FadeOut:
                items[index].opacity = 0;
                items[index].translateX = 0;
                items[index].rotateY = 0;
                break;

            case CarouselAnimation.Slide:
                items[index].opacity = 1;
                items[index].translateX = 100;
                items[index].rotateY = 0;
                break;

            case CarouselAnimation.SlideRotate:
                items[index].opacity = 1;
                items[index].translateX = 50;
                items[index].rotateY = 90;
                break;
        }

        // adjust active
        if (index <= Active) {
            Active++;

            // Progressbar Transition goes from start, so refesh Interval
            if (interval.Enabled) {
                interval.Stop();
                StartInterval();
            }
        }

        ItemContainer = items;

        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Removes the item with the given index from the carousel.
    /// </summary>
    /// <param name="index">0 = first item, Length - 1 = last item</param>
    public void RemoveItem(int index) {
        if (ItemContainer.Length <= 1)
            return;

        if (index < 0)
            throw new IndexOutOfRangeException("index must be equal or greater 0");

        if (index >= ItemContainer.Length)
            throw new IndexOutOfRangeException($"index must be smaller than Length");

        // create array with one less space and copy array in the new array with removed item at index
        Item[] items = new Item[ItemContainer.Length - 1];
        int i = 0;
        for (; i < index; i++)
            items[i] = ItemContainer[i];
        for (; i < items.Length; i++)
            items[i] = ItemContainer[i + 1];

        // adjust active
        if (index < Active) {
            Active--;
            // Progressbar Transition goes from start, so refesh Interval
            if (interval.Enabled) {
                interval.Stop();
                StartInterval();
            }
        }
        else if (index == Active) {
            if (Active < ItemContainer.Length) {
                int newActive = Active;
                if (Active == items.Length) {
                    newActive--;
                    if (interval.Enabled) {
                        interval.Stop();
                        StartInterval();
                    }
                }

                items[newActive].indicatorOpacity = ItemContainer[Active].indicatorOpacity;
                items[newActive].opacity = ItemContainer[Active].opacity;
                items[newActive].progressBar = ItemContainer[Active].progressBar;
                items[newActive].progressBarTransition = ItemContainer[Active].progressBarTransition;
                items[newActive].translateX = ItemContainer[Active].translateX;
                items[newActive].rotateY = ItemContainer[Active].rotateY;

                Active = newActive;
            }
        }

        ItemContainer = items;

        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Removes the first appearance of the given item from the carousel.
    /// </summary>
    /// <param name="carouselItem"></param>
    public bool RemoveItem(CarouselItem carouselItem) {
        for (int i = 0; i < ItemContainer.Length; i++)
            if (ItemContainer[i].carouselItem == carouselItem) {
                RemoveItem(i);
                return true;
            }

        return false;
    }

    /// <summary>
    /// Swaps 2 items in the carousel.
    /// </summary>
    /// <param name="index1">0 = first item, Length - 1 = last item</param>
    /// <param name="index2">0 = first item, Length - 1 = last item</param>
    public void SwapItem(int index1, int index2) {
        (ItemContainer[index1].carouselItem, ItemContainer[index2].carouselItem) = (ItemContainer[index2].carouselItem, ItemContainer[index1].carouselItem);
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Set the item with the given index to active.
    /// </summary>
    /// <param name="index">0 = first item, Length - 1 = last item</param>
    /// <param name="intervalStop">stopping interval with this change</param>
    public void SetActiveItem(int index, bool intervalStop = true) {
        if (Active != index) {
            SwapItems(Active, index, index - Active);
            if (intervalStop)
                StopInterval();
            else if (interval.Enabled) {
                interval.Stop();
                StartInterval();
            }
        }
        else if (intervalStop)
            StopInterval();

        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Sets the swapping animation of the carousel.
    /// </summary>
    /// <param name="animation"></param>
    public void SetAnimation(CarouselAnimation animation) {
        CurrAnimation = animation;

        switch (animation) {
            case CarouselAnimation.FadeOut:
                for (int i = 0; i < ItemContainer.Length; i++) {
                    ItemContainer[i].opacity = 0;
                    ItemContainer[i].translateX = 0;
                    ItemContainer[i].rotateY = 0;
                    ItemContainer[i].slideClass = string.Empty;
                }
                // set first one active
                ItemContainer[Active].opacity = 1;
                break;

            case CarouselAnimation.Slide:
                for (int i = 0; i < ItemContainer.Length; i++) {
                    ItemContainer[i].opacity = 1;
                    ItemContainer[i].translateX = 100;
                    ItemContainer[i].rotateY = 0;
                    ItemContainer[i].slideClass = string.Empty;
                }
                // set first one active
                ItemContainer[Active].translateX = 0;
                break;

            case CarouselAnimation.SlideRotate:
                for (int i = 0; i < ItemContainer.Length; i++) {
                    ItemContainer[i].opacity = 1;
                    ItemContainer[i].translateX = 50;
                    ItemContainer[i].rotateY = 90;
                    ItemContainer[i].slideClass = string.Empty;
                }
                // set first one active
                ItemContainer[Active].translateX = 0;
                ItemContainer[Active].rotateY = 0;
                break;
        }
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// <para>Sets interval time of the carousel.</para>
    /// <para>Stops interval if currently running.</para>
    /// </summary>
    /// <param name="intervalTime"></param>
    public void SetIntervalTime(int intervalTime) {
        StopInterval();
        interval.Interval = intervalTime;
        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// <para>Sets autostart time in ms of the carousel.</para>
    /// <para>Value of 0 deactivates autostart.</para>
    /// <para>Restarts autoStart if currently running.</para>
    /// </summary>
    /// <param name="autoStartTime"></param>
    public void SetAutoStartTime(int autoStartTime) {
        CurrAutoStartTime = autoStartTime;
        autoStart.Stop();

        if (CurrAutoStartTime > 0) {
            autoStart.Interval = autoStartTime;
            autoStart.Start();
        }

        InvokeAsync(StateHasChanged);
    }

    /// <summary>
    /// Starts the interval and sets Running = true.
    /// </summary>
    public void Start() {
        if (!interval.Enabled) {
            StartInterval();
            InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Stops the interval, sets Running = false and starts the Autostart timer if AutoStartTime greater 0.
    /// </summary>
    public void Stop() {
        StopInterval();
        InvokeAsync(StateHasChanged);
    }

    #endregion
}
