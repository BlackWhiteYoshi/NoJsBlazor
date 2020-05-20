using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace NoJsBlazor {
    /// <summary>
    /// Type of animation, how the items are swapped.
    /// </summary>
    public enum CarouselAnimation {
        /// <summary>
        /// Transition to opacity
        /// </summary>
        FadeOut,
        /// <summary>
        /// Transition to transform: x-axis
        /// </summary>
        Slide,
        /// <summary>
        /// Transition to transform: x-axis and y-axis
        /// </summary>
        SlideRotate
    }

    public partial class Carousel : IDisposable {
        private struct Item {
            public CarouselItem CarouselItem;
            public int Zindex;
            public int Opacity;
            public float TranslateX;
            public float RotateY;
            public string Class;
            public float IndicatorOpacity;
            public int ProgressBar;
            public int ProgressBarTransition;
        }
        private Item[] ItemContainer = new Item[0];

        #region Parameters & Fields

        /// <summary>
        /// Active image at the beginning.
        /// </summary>
        [Parameter]
        public int Active { get; set; } = 0;
        private int active;

        /// <summary>
        /// Waiting time before beginning swap animation in ms.
        /// </summary>
        [Parameter]
        public int IntervalTime { get; set; } = 6000;

        /// <summary>
        /// Type of swapping animation.
        /// </summary>
        [Parameter]
        public CarouselAnimation Animation { get; set; } = CarouselAnimation.FadeOut;
        CarouselAnimation animation;

        /// <summary>
        /// <para>Starts interval after [AutoStartTime] ms, if interval not running and no action occurs.</para>
        /// <para>Value of 0 deactivates autostart.</para>
        /// </summary>
        [Parameter]
        public int AutoStartTime { get; set; } = 0;
        private bool autoStartActive = false;

        /// <summary>
        /// Interval Thread starts at beginning.
        /// </summary>
        [Parameter]
        public bool BeginRunning { get; set; } = true;

        /// <summary>
        /// Next/Previous Arrows available
        /// </summary>
        [Parameter]
        public bool ControlArrowsEnable { get; set; } = true;

        /// <summary>
        /// Indicators available
        /// </summary>
        [Parameter]
        public bool IndicatorsEnable { get; set; } = true;

        /// <summary>
        /// PlayButton available
        /// </summary>
        [Parameter]
        public bool PlayButtonEnable { get; set; } = true;

        /// <summary>
        /// <para>Content of the <see cref="Carousel"/>.</para>
        /// <para>This should be a list of <see cref="CarouselItem"/> objects.</para>
        /// </summary>
        [Parameter]
        public RenderFragment Items { get; set; }

        /// <summary>
        /// Html that will be rendered in the overlay section.
        /// </summary>
        [Parameter]
        public RenderFragment Overlay { get; set; }

        /// <summary>
        /// Captures unmatched values
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        /// <summary>
        /// Indicates if the interval of the carousel is currently running.
        /// </summary>
        public bool Running => interval.Enabled;

        private readonly TouchClick prevTC;
        private readonly TouchClick nextTC;
        private readonly TouchClick<int> indicatorTC;
        private readonly TouchClick playButtonTC;

        private readonly Timer interval = new Timer();
        private readonly Timer autoStart = new Timer();

        #endregion

        public Carousel() {
            prevTC = new TouchClick(PrevButton);
            nextTC = new TouchClick(NextButton);
            indicatorTC = new TouchClick<int>(IndicatorButton);
            playButtonTC = new TouchClick(PlayButton);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender) {
            if (firstRender) {
                Initialize();
                StateHasChanged();
                await Task.Yield();

                if (BeginRunning)
                    StartInterval();
                else if (autoStartActive)
                    autoStart.Start();
            }
        }

        #region input Funtions

        private void PrevButton(EventArgs e) {
            StopInterval();
            Prev();
        }

        private void NextButton(EventArgs e) {
            StopInterval();
            Next();
        }

        private void IndicatorButton(EventArgs e) {
            StopInterval();
            SwapImages(active, indicatorTC.Parameter, indicatorTC.Parameter - active);
        }

        private void PlayButton(EventArgs e) {
            if (!interval.Enabled)
                StartInterval();
            else
                StopInterval();
        }

        #endregion

        #region private Functions

        private void Initialize() {
            ItemContainer = new Item[childList.Count];
            active = Active;
            animation = Animation;
            interval.Interval = IntervalTime;
            interval.Elapsed += Interval;
            interval.AutoReset = true;

            if (AutoStartTime > 0) {
                autoStartActive = true;
                autoStart.Interval = AutoStartTime;
                autoStart.Elapsed += AutoStart;
                autoStart.AutoReset = false;
            }
            else
                autoStartActive = false;


            for (int i = 0; i < ItemContainer.Length; i++) {
                ItemContainer[i].CarouselItem = childList[i];
                ItemContainer[i].Zindex = 10;
                ItemContainer[i].IndicatorOpacity = 0.5F;
                ItemContainer[i].ProgressBar = 0;
                ItemContainer[i].ProgressBarTransition = 0;
                ItemContainer[i].Class = null;
            }
            // set first one active
            ItemContainer[active].Zindex = 20;
            ItemContainer[active].IndicatorOpacity = 1.0F;

            switch (animation) {
                case CarouselAnimation.FadeOut:
                    for (int i = 0; i < ItemContainer.Length; i++) {
                        ItemContainer[i].Opacity = 0;
                        ItemContainer[i].TranslateX = 0;
                        ItemContainer[i].RotateY = 0;
                    }
                    // set first one active
                    ItemContainer[active].Opacity = 1;
                    break;

                case CarouselAnimation.Slide:
                    for (int i = 0; i < ItemContainer.Length; i++) {
                        ItemContainer[i].Opacity = 1;
                        ItemContainer[i].TranslateX = 100;
                        ItemContainer[i].RotateY = 0;
                    }
                    // set first one active
                    ItemContainer[active].TranslateX = 0;
                    break;

                case CarouselAnimation.SlideRotate:
                    for (int i = 0; i < ItemContainer.Length; i++) {
                        ItemContainer[i].Opacity = 1;
                        ItemContainer[i].TranslateX = 50;
                        ItemContainer[i].RotateY = 100;
                    }
                    // set first one active
                    ItemContainer[active].TranslateX = 0;
                    ItemContainer[active].RotateY = 0;
                    break;
            }
        }

        private void Prev() {
            int next = active - 1;
            if (next < 0)
                next = ItemContainer.Length - 1;

            SwapImages(active, next, -1);
        }

        private void Next() {
            int next = (active + 1) % ItemContainer.Length;

            SwapImages(active, next, 1);
        }

        private async void SwapImages(int active, int next, int direction) {
            ItemContainer[active].ProgressBarTransition = 0;
            ItemContainer[active].ProgressBar = 0;

            if (active == next)
                return;

            ItemContainer[active].Zindex = 10;
            ItemContainer[next].Zindex = 20;
            ItemContainer[active].IndicatorOpacity = 0.5F;
            ItemContainer[next].IndicatorOpacity = 1.0F;

            switch (animation) {
                case CarouselAnimation.FadeOut:
                    ItemContainer[active].Opacity = 0;
                    ItemContainer[next].Opacity = 1;
                    break;

                case CarouselAnimation.Slide:
                case CarouselAnimation.SlideRotate:
                    int translateX;
                    int rotateY;
                    if (animation == CarouselAnimation.Slide) {
                        translateX = 100;
                        rotateY = 0;
                    }
                    else {
                        translateX = 50;
                        rotateY = 100;
                    }
                    int sgnDirection = Math.Sign(direction);

                    for (int i = 0; i != direction; i += sgnDirection) {
                        ItemContainer[next - i].Class = null;
                        ItemContainer[next - i].TranslateX = translateX * (direction - i);
                        ItemContainer[next - i].RotateY = rotateY * (direction - i);
                    }

                    await InvokeAsync(StateHasChanged);
                    // TODO waiting of rendering instead of waiting 30ms
                    await Task.Delay(30);

                    ItemContainer[next].Class = "slide";
                    ItemContainer[next].TranslateX = 0;
                    ItemContainer[next].RotateY = 0;

                    for (int i = 0; i != direction; i += sgnDirection) {
                        ItemContainer[active + i].Class = "slide";
                        ItemContainer[active + i].TranslateX = -translateX * (direction - i);
                        ItemContainer[active + i].RotateY = -rotateY * (direction - i);
                    }

                    break;

                default:
                    throw new Exception("pigs have learned to fly");
            }

            _ = InvokeAsync(StateHasChanged);
            this.active = next;
        }

        private void StopInterval() {
            interval.Stop();
            ItemContainer[active].ProgressBarTransition = 0;
            ItemContainer[active].ProgressBar = 0;
            if (autoStartActive) {
                autoStart.Stop();
                autoStart.Start();
            }
        }


        #region Interval / AutoStart

        private void StartInterval() {
            ItemContainer[active].ProgressBarTransition = IntervalTime;
            ItemContainer[active].ProgressBar = 100;
            interval.Start();
            InvokeAsync(StateHasChanged);
        }

        private void Interval(object source, ElapsedEventArgs e) {
            int next = (active + 1) % ItemContainer.Length;
            ItemContainer[next].ProgressBarTransition = IntervalTime;
            ItemContainer[next].ProgressBar = 100;
            Next();
            InvokeAsync(StateHasChanged);
        }

        private void AutoStart(object source, ElapsedEventArgs e) {
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
        /// Adds an item to the carousel.
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
            Item[] images = new Item[ItemContainer.Length + 1];
            int i = 0;
            for (; i < index; i++)
                images[i] = ItemContainer[i];
            for (; i < ItemContainer.Length; i++)
                images[i + 1] = ItemContainer[i];

            // fill space position with default values
            images[index].CarouselItem = item;
            images[index].IndicatorOpacity = 0.5F;
            images[index].ProgressBar = 0;
            images[index].ProgressBarTransition = 0;
            images[index].Class = null;
            switch (animation) {
                case CarouselAnimation.FadeOut:
                    images[index].Opacity = 0;
                    images[index].TranslateX = 0;
                    images[index].RotateY = 0;
                    break;

                case CarouselAnimation.Slide:
                    images[index].Opacity = 1;
                    images[index].TranslateX = 100;
                    images[index].RotateY = 0;
                    break;

                case CarouselAnimation.SlideRotate:
                    images[index].Opacity = 1;
                    images[index].TranslateX = 50;
                    images[index].RotateY = 100;
                    break;
            }

            // adjust active
            if (index <= active) {
                active++;

                // Progressbar Transition goes from start, so refesh Interval
                if (interval.Enabled) {
                    interval.Stop();
                    StartInterval();
                }
            }

            ItemContainer = images;

            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Removes an item from the carousel.
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
            Item[] images = new Item[ItemContainer.Length - 1];
            int i = 0;
            for (; i < index; i++)
                images[i] = ItemContainer[i];
            for (; i < images.Length; i++)
                images[i] = ItemContainer[i + 1];

            // adjust active
            if (index < active) {
                active--;
                // Progressbar Transition goes from start, so refesh Interval
                if (interval.Enabled) {
                    interval.Stop();
                    StartInterval();
                }
            }
            else if (index == active) {
                if (active < ItemContainer.Length) {
                    int newActive = active;
                    if (active == images.Length) {
                        newActive--;
                        if (interval.Enabled) {
                            interval.Stop();
                            StartInterval();
                        }
                    }

                    images[newActive].IndicatorOpacity = ItemContainer[active].IndicatorOpacity;
                    images[newActive].Opacity = ItemContainer[active].Opacity;
                    images[newActive].ProgressBar = ItemContainer[active].ProgressBar;
                    images[newActive].ProgressBarTransition = ItemContainer[active].ProgressBarTransition;
                    images[newActive].TranslateX = ItemContainer[active].TranslateX;
                    images[newActive].RotateY = ItemContainer[active].RotateY;

                    active = newActive;
                }
            }

            ItemContainer = images;

            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Swaps 2 items in the carousel.
        /// </summary>
        /// <param name="index1">0 = first item, Length - 1 = last item</param>
        /// <param name="index2">0 = first item, Length - 1 = last item</param>
        public void SwapImage(int index1, int index2) {
            CarouselItem buffer = ItemContainer[index1].CarouselItem;
            ItemContainer[index1].CarouselItem = ItemContainer[index2].CarouselItem;
            ItemContainer[index2].CarouselItem = buffer;

            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Set the item with the given index to active.
        /// </summary>
        /// <param name="index">0 = first item, Length - 1 = last item</param>
        /// <param name="intervalStop">stopping interval with this change</param>
        public void SetActiveImage(int index, bool intervalStop = true) {
            if (active != index) {
                SwapImages(active, index, index - active);
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
            this.animation = animation;

            switch (animation) {
                case CarouselAnimation.FadeOut:
                    for (int i = 0; i < ItemContainer.Length; i++) {
                        ItemContainer[i].Opacity = 0;
                        ItemContainer[i].TranslateX = 0;
                        ItemContainer[i].RotateY = 0;
                        ItemContainer[i].Class = null;
                    }
                    // set first one active
                    ItemContainer[active].Opacity = 1;
                    break;

                case CarouselAnimation.Slide:
                    for (int i = 0; i < ItemContainer.Length; i++) {
                        ItemContainer[i].Opacity = 1;
                        ItemContainer[i].TranslateX = 100;
                        ItemContainer[i].RotateY = 0;
                        ItemContainer[i].Class = null;
                    }
                    // set first one active
                    ItemContainer[active].TranslateX = 0;
                    break;

                case CarouselAnimation.SlideRotate:
                    for (int i = 0; i < ItemContainer.Length; i++) {
                        ItemContainer[i].Opacity = 1;
                        ItemContainer[i].TranslateX = 50;
                        ItemContainer[i].RotateY = 100;
                        ItemContainer[i].Class = null;
                    }
                    // set first one active
                    ItemContainer[active].TranslateX = 0;
                    ItemContainer[active].RotateY = 0;
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
        /// <para>Sets autostart time of the carousel.</para>
        /// <para>Value of 0 deactivates autostart.</para>
        /// <para>Restarts autoStart if currently running.</para>
        /// </summary>
        /// <param name="autoStartTime"></param>
        public void SetAutoStartTime(int autoStartTime) {
            autoStart.Stop();

            if (AutoStartTime > 0) {
                autoStartActive = true;
                autoStart.Interval = autoStartTime;
                autoStart.Start();
            }
            else
                autoStartActive = false;

            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Starts the interval and sets Running = true.
        /// </summary>
        public void Start() {
            if (!interval.Enabled)
                StartInterval();
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Stops the interval, sets Running = false and starts the Autostart timer if AutoStartTime greater 0.
        /// </summary>
        public void Stop() {
            StopInterval();
            InvokeAsync(StateHasChanged);
        }

        /// <summary>
        /// Disposes Interval Timer and Autostart Timer.
        /// </summary>
        public void Dispose() {
            interval?.Dispose();
            autoStart?.Dispose();
        }

        #endregion
    }
}
