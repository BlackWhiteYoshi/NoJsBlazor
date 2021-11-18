using AngleSharp.Dom;

namespace UnitTest;

public partial class CarouselTest : TestContext {
    #region parameter

    [Fact]
    public void Four_Items_Has_ChildCount_Four() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
        });
        Carousel carousel = carouselContainer.Instance;

        Assert.Equal(4, carousel.ChildCount);
    }

    [Fact]
    public void Items_Are_Rendered_In_ItemsDiv() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
        });

        IRefreshableElementCollection<IElement> divCollection = carouselContainer.FindAll(".carousel-item");

        Assert.Equal(4, divCollection.Count);
        Assert.Equal(TestDiv("red"), divCollection[0].InnerHtml);
        Assert.Equal(TestDiv("blue"), divCollection[1].InnerHtml);
        Assert.Equal(TestDiv("yellow"), divCollection[2].InnerHtml);
        Assert.Equal(TestDiv("green"), divCollection[3].InnerHtml);


        static string TestDiv(string color) => $"<div style=\"background-color: {color};\"></div>";
    }

    [Fact]
    public void Overlay_Is_Rendered_In_OverlayDiv() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.Overlay, TEST_HTML);
        });

        IElement overlayDiv = carouselContainer.Find(".carousel-overlay");

        Assert.Equal(TEST_HTML, overlayDiv.InnerHtml);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public void Active_Start_Sets_Active_Item(int activeValue) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, activeValue);
        });
        Carousel carousel = carouselContainer.Instance;

        Assert.Equal(activeValue, carousel.Active);
    }

    [Fact]
    public void IntervallTime_Sets_Time_Of_Intervall() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 100);
        });
        Carousel carousel = carouselContainer.Instance;

        carouselContainer.WaitForAssertion(() => Assert.Equal(1, carousel.Active), TimeSpan.FromMilliseconds(200));
    }

    [Theory]
    [InlineData(CarouselAnimation.FadeOut)]
    [InlineData(CarouselAnimation.Slide)]
    [InlineData(CarouselAnimation.SlideRotate)]
    public void Animation_Sets_Current_Animation(CarouselAnimation animation) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.Animation, animation);
        });
        Carousel carousel = carouselContainer.Instance;

        Assert.Equal(animation, carousel.CurrAnimation);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void BeginRunning_Enables_Intervall(bool enabled) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, enabled);
        });
        Carousel carousel = carouselContainer.Instance;

        Assert.Equal(enabled, carousel.Running);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ControlArrowsEnable_Enables_Control_Arrows(bool enabled) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ControlArrowsEnable, enabled);
        });

        if (enabled) {
            Assert.Single(carouselContainer.FindAll(".carousel-control-prev"));
            Assert.Single(carouselContainer.FindAll(".carousel-control-next"));
        }
        else {
            Assert.Empty(carouselContainer.FindAll(".carousel-control-prev"));
            Assert.Empty(carouselContainer.FindAll(".carousel-control-next"));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IndicatorsEnable_Enables_Indicators(bool enabled) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.IndicatorsEnable, enabled);
        });

        if (enabled)
            Assert.Single(carouselContainer.FindAll(".carousel-indicators"));
        else
            Assert.Empty(carouselContainer.FindAll(".carousel-indicators"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void PlayButtonEnable_Enables_Play_Button(bool enabled) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.PlayButtonEnable, enabled);
        });

        if (enabled)
            Assert.Single(carouselContainer.FindAll(".carousel-play-button"));
        else
            Assert.Empty(carouselContainer.FindAll(".carousel-play-button"));
    }

    #endregion


    #region interactive

    [Fact]
    public void Next_Click_Moves_To_Next_Item() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;
        IElement nextButton = carouselContainer.Find(".carousel-control-next");

        Assert.Equal(1, carousel.Active);
        nextButton.MouseDown();
        Assert.Equal(2, carousel.Active);
        nextButton.MouseDown();
        Assert.Equal(3, carousel.Active);
        nextButton.MouseDown();
        Assert.Equal(0, carousel.Active);
    }

    [Fact]
    public void Prev_Click_Moves_To_Prev_Item() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 2);
        });
        Carousel carousel = carouselContainer.Instance;
        IElement prevButton = carouselContainer.Find(".carousel-control-prev");

        Assert.Equal(2, carousel.Active);
        prevButton.MouseDown();
        Assert.Equal(1, carousel.Active);
        prevButton.MouseDown();
        Assert.Equal(0, carousel.Active);
        prevButton.MouseDown();
        Assert.Equal(3, carousel.Active);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void Indicator_Click_Moves_To_Correspondent_Item(int index) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
        });
        Carousel carousel = carouselContainer.Instance;
        IElement indicatorList = carouselContainer.Find(".carousel-indicators");


        int index2 = (index + 1) % carousel.ItemCount;
        int index3 = (index + 3) % carousel.ItemCount;


        indicatorList.Children[index].MouseDown();
        Assert.Equal(index, carousel.Active);

        indicatorList.Children[index2].MouseDown();
        Assert.Equal(index2, carousel.Active);

        indicatorList.Children[index3].MouseDown();
        Assert.Equal(index3, carousel.Active);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void PlayButton_Click_Starts_And_Stops_Intervall(bool runAtBeginning) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, runAtBeginning);
        });
        Carousel carousel = carouselContainer.Instance;
        IElement playButton = carouselContainer.Find(".carousel-play-button");

        Assert.Equal(runAtBeginning, carousel.Running);
        playButton.MouseDown();
        Assert.NotEqual(runAtBeginning, carousel.Running);
        playButton.MouseDown();
        Assert.Equal(runAtBeginning, carousel.Running);
    }

    #endregion


    #region timer related

    [Fact]
    public void Interval_Moves_To_Next() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 100);
        });
        Carousel carousel = carouselContainer.Instance;

        carouselContainer.WaitForAssertion(() => Assert.Equal(2, carousel.Active));
        carouselContainer.WaitForAssertion(() => Assert.Equal(3, carousel.Active));
        carouselContainer.WaitForAssertion(() => Assert.Equal(0, carousel.Active));
    }

    [Fact]
    public void AutoStart_Activates_Interval() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 100);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.AutoStartTime, 100);
        });
        Carousel carousel = carouselContainer.Instance;

        Assert.False(carousel.Running);
        carouselContainer.WaitForAssertion(() => Assert.True(carousel.Running));
        carouselContainer.WaitForAssertion(() => Assert.Equal(2, carousel.Active));
    }

    #endregion


    #region public Methods

    [Fact]
    public void AddItem_Adds_An_Item() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.AddItem(new CarouselItem(CarouselItemRed));

        Assert.Equal(5, carousel.ItemCount);
    }

    [Fact]
    public void RemoveItem_Removes_An_Item() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.RemoveItem(1);

        Assert.Equal(3, carousel.ItemCount);
    }

    [Fact]
    public void SwapItem_Swaps_Items() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
        });
        Carousel carousel = carouselContainer.Instance;

        IRefreshableElementCollection<IElement>? itemDivs = carouselContainer.FindAll(".carousel-item");
        string item0 = itemDivs[0].InnerHtml;
        string item1 = itemDivs[1].InnerHtml;


        carousel.SwapItem(0, 1);


        IRefreshableElementCollection<IElement>? itemDivsAfter = carouselContainer.FindAll(".carousel-item");
        Assert.Equal(item0, itemDivsAfter[1].InnerHtml);
        Assert.Equal(item1, itemDivsAfter[0].InnerHtml);
    }

    [Fact]
    public void SetActiveItem_Sets_Item_With_Index_Active() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
            builder.Add((Carousel carousel) => carousel.BeginRunning, true);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.SetActiveItem(2, false);
        Assert.Equal(2, carousel.Active);
        Assert.True(carousel.Running);

        carousel.SetActiveItem(0, true);
        Assert.Equal(0, carousel.Active);
        Assert.False(carousel.Running);

        carousel.SetActiveItem(3, false);
        Assert.Equal(3, carousel.Active);
        Assert.False(carousel.Running);
    }

    [Fact]
    public void SetAnimation_Sets_To_New_Animation() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.Animation, CarouselAnimation.FadeOut);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.SetAnimation(CarouselAnimation.Slide);
        Assert.Equal(CarouselAnimation.Slide, carousel.CurrAnimation);
    }

    [Fact]
    public void SetIntervalTime_Sets_To_New_Interval() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 100000);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.SetIntervalTime(100);
        carousel.Start();
        carouselContainer.WaitForAssertion(() => Assert.Equal(2, carousel.Active));
    }

    [Fact]
    public void SetAutoStartTime_Sets_To_New_TimeOut() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.AutoStartTime, 0);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.SetAutoStartTime(100);
        Assert.False(carousel.Running);
        carouselContainer.WaitForAssertion(() => Assert.True(carousel.Running));
    }

    [Fact]
    public void Start_Enables_Interval() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 100000);
        });
        Carousel carousel = carouselContainer.Instance;

        Assert.False(carousel.Running);
        carousel.Start();
        Assert.True(carousel.Running);
        carousel.Start();
        Assert.True(carousel.Running);
    }

    [Fact]
    public void Stop_Disables_Interval() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, true);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 100000);
        });
        Carousel carousel = carouselContainer.Instance;

        Assert.True(carousel.Running);
        carousel.Stop();
        Assert.False(carousel.Running);
        carousel.Stop();
        Assert.False(carousel.Running);
    }

    #endregion


    #region event

    [Fact]
    public void OnActiveChanged_Fires_When_Active_Item_Changes() {
        int fired = 0;

        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
            builder.Add((Carousel carousel) => carousel.OnActiveChanged, (int index) => fired++);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.SetActiveItem(0);
        Assert.Equal(1, fired);
    }

    [Fact]
    public void OnRunningChanged_Fires_When_Running_State_Changes() {
        int fired = 0;

        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.OnRunningChanged, (bool running) => fired++);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.Start();
        Assert.Equal(1, fired);
    }

    #endregion
}
