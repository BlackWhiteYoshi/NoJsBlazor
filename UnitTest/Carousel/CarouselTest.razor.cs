using AngleSharp.Dom;

namespace UnitTest;

public sealed partial class CarouselTest : TestContext {
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

        IRefreshableElementCollection<IElement> divCollection = carouselContainer.FindAll(".carousel-element");

        Assert.Equal(4, divCollection.Count);
        Assert.Equal(TestDiv("red"), divCollection[0].InnerHtml);
        Assert.Equal(TestDiv("blue"), divCollection[1].InnerHtml);
        Assert.Equal(TestDiv("yellow"), divCollection[2].InnerHtml);
        Assert.Equal(TestDiv("green"), divCollection[3].InnerHtml);


        static string TestDiv(string color) => $"<div style=\"background-color: {color};\"></div>";
    }

    [Fact]
    public void Items_AddingItemAppends() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment itemsWithPurple = (RenderFragment)CarouselItemPurple + ItemsRedBlueYellowGreen;
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", itemsWithPurple));

        IElement purpleDiv = carouselContainer.FindAll(".carousel-element")[0];
        string purpleInnerHtml = purpleDiv.InnerHtml;
        Assert.Contains("purple", purpleInnerHtml);

        carousel.Active = carousel.ChildCount - 1;
        Assert.Contains("z-index: 20", purpleDiv.Attributes["style"]!.Value);

        Assert.Equal(5, carousel.ChildCount);
    }

    [Fact]
    public void Items_RemoveItem_First() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment itemsWithoutRed = (RenderFragment)CarouselItemBlue + CarouselItemYellow + CarouselItemGreen;
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", itemsWithoutRed));

        Assert.Equal(3, carousel.ChildCount);
        Assert.Equal(0, carousel.Active);
    }

    [Fact]
    public void Items_RemoveItem_Last() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment itemsWithoutGreen = (RenderFragment)CarouselItemRed + CarouselItemBlue + CarouselItemYellow;
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", itemsWithoutGreen));

        Assert.Equal(3, carousel.ChildCount);
        Assert.Equal(1, carousel.Active);
    }

    [Fact]
    public void Items_RemoveItem_Active() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment itemsWithoutRed = (RenderFragment)CarouselItemBlue + CarouselItemYellow + CarouselItemGreen;
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", itemsWithoutRed));

        Assert.Equal(3, carousel.ChildCount);
        Assert.Equal(0, carousel.Active);
    }

    [Fact]
    public void Items_RemoveLastChild_SetsActiveToMinus1() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, CarouselItemRed);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment noItems = (RenderTreeBuilder builder) => { };
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", noItems));

        Assert.Equal(0, carousel.ChildCount);
        Assert.Equal(-1, carousel.Active);
    }

    [Fact]
    public void Items_RemoveItem_FirstActive() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 0);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment itemsWithoutBlue = (RenderFragment)CarouselItemRed + CarouselItemYellow + CarouselItemGreen;
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", itemsWithoutBlue));

        Assert.Equal(3, carousel.ChildCount);
        Assert.Equal(0, carousel.Active);
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
            Assert.Single(carouselContainer.FindAll(".carousel-arrow.prev"));
            Assert.Single(carouselContainer.FindAll(".carousel-arrow.next"));
        }
        else {
            Assert.Empty(carouselContainer.FindAll(".carousel-arrow.prev"));
            Assert.Empty(carouselContainer.FindAll(".carousel-arrow.next"));
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
            Assert.Single(carouselContainer.FindAll(".carousel-indicator-list"));
        else
            Assert.Empty(carouselContainer.FindAll(".carousel-indicator-list"));
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
        IElement nextButton = carouselContainer.Find(".carousel-arrow.next");

        Assert.Equal(1, carousel.Active);
        nextButton.Click();
        Assert.Equal(2, carousel.Active);
        nextButton.Click();
        Assert.Equal(3, carousel.Active);
        nextButton.Click();
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
        IElement prevButton = carouselContainer.Find(".carousel-arrow.prev");

        Assert.Equal(2, carousel.Active);
        prevButton.Click();
        Assert.Equal(1, carousel.Active);
        prevButton.Click();
        Assert.Equal(0, carousel.Active);
        prevButton.Click();
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

        IElement indicatorList = carouselContainer.Find(".carousel-indicator-list");

        int index2 = (index + 1) % carousel.ChildCount;
        int index3 = (index + 3) % carousel.ChildCount;


        indicatorList.Children[index].Click();
        Assert.Equal(index, carousel.Active);

        indicatorList.Children[index2].Click();
        Assert.Equal(index2, carousel.Active);

        indicatorList.Children[index3].Click();
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
        carouselContainer.Render();
        Carousel carousel = carouselContainer.Instance;
        IElement playButton = carouselContainer.Find(".carousel-play-button");

        Assert.Equal(runAtBeginning, carousel.Running);
        playButton.Click();
        Assert.NotEqual(runAtBeginning, carousel.Running);
        playButton.Click();
        Assert.Equal(runAtBeginning, carousel.Running);
    }

    #endregion


    #region timer related

    [Fact]
    public void AutoStart_Activates_Interval() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 1000);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.AutoStartTime, 100);
        });
        Carousel carousel = carouselContainer.Instance;

        Assert.False(carousel.Running);
        carouselContainer.WaitForAssertion(() => Assert.True(carousel.Running));
        carouselContainer.WaitForAssertion(() => Assert.Equal(1, carousel.Active));
    }

    #endregion


    #region api methods

    [Fact]
    public void SwapItem_Swaps_Items() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;

        IRefreshableElementCollection<IElement> itemDivs = carouselContainer.FindAll(".carousel-element");
        IElement item0 = itemDivs[0];
        IElement item1 = itemDivs[1];
        Assert.Contains("z-index: 20", item1.Attributes["style"]!.Value);
        Assert.Equal(1, carousel.Active);

        carousel.SwapCarouselItems(0, 1);


        IRefreshableElementCollection<IElement> itemDivsAfter = carouselContainer.FindAll(".carousel-element");
        Assert.Contains("z-index: 20", item1.Attributes["style"]!.Value);
        Assert.Equal(0, carousel.Active);
    }

    [Fact]
    public void SetActiveItem_Sets_Item_With_Index_Active() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.Active = 2;
        Assert.Equal(2, carousel.Active);

        carousel.Active = 0;
        Assert.Equal(0, carousel.Active);

        carousel.Active = 3;
        Assert.Equal(3, carousel.Active);
    }

    [Fact]
    public void StartInterval_Enables_Interval() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 100000);
        });
        Carousel carousel = carouselContainer.Instance;

        Assert.False(carousel.Running);
        carousel.StartInterval();
        Assert.True(carousel.Running);
        carousel.StartInterval();
        Assert.True(carousel.Running);
    }

    [Fact]
    public void StopInterval_Disables_Interval() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, true);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 100000);
        });
        Carousel carousel = carouselContainer.Instance;

        Assert.True(carousel.Running);
        carousel.StopInterval();
        Assert.False(carousel.Running);
        carousel.StopInterval();
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

        carousel.Active = 0;
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

        carousel.StartInterval();
        Assert.Equal(1, fired);

        carousel.StartInterval();
        Assert.Equal(1, fired);

        carousel.StopInterval();
        Assert.Equal(2, fired);
    }

    #endregion
}
