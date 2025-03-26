using AngleSharp.Dom;

namespace UnitTest;

public sealed partial class CarouselTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Four_Items_Has_ChildCount_Four() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
        });
        Carousel carousel = carouselContainer.Instance;

        await Assert.That(carousel.ChildCount).IsEqualTo(4);
    }

    [Test]
    public async ValueTask Items_Are_Rendered_In_ItemsDiv() {
        static string TestDiv(string color) => $"<div style=\"background-color: {color};\"></div>";


        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
        });

        IRefreshableElementCollection<IElement> divCollection = carouselContainer.FindAll(".carousel-element");

        await Assert.That(divCollection.Count).IsEqualTo(4);
        await Assert.That(divCollection[0].InnerHtml).IsEqualTo(TestDiv("red"));
        await Assert.That(divCollection[1].InnerHtml).IsEqualTo(TestDiv("blue"));
        await Assert.That(divCollection[2].InnerHtml).IsEqualTo(TestDiv("yellow"));
        await Assert.That(divCollection[3].InnerHtml).IsEqualTo(TestDiv("green"));
    }

    [Test]
    public async ValueTask Items_AddingItemAppends() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment itemsWithPurple = (RenderFragment)CarouselItemPurple + ItemsRedBlueYellowGreen;
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", itemsWithPurple));

        IElement purpleDiv = carouselContainer.FindAll(".carousel-element")[0];
        string purpleInnerHtml = purpleDiv.InnerHtml;
        await Assert.That(purpleInnerHtml).Contains("purple");

        carousel.Active = carousel.ChildCount - 1;
        await Assert.That(purpleDiv.Attributes["style"]!.Value).Contains("z-index: 20");

        await Assert.That(carousel.ChildCount).IsEqualTo(5);
    }

    [Test]
    public async ValueTask Items_RemoveItem_First() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment itemsWithoutRed = (RenderFragment)CarouselItemBlue + CarouselItemYellow + CarouselItemGreen;
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", itemsWithoutRed));

        await Assert.That(carousel.ChildCount).IsEqualTo(3);
        await Assert.That(carousel.Active).IsEqualTo(0);
    }

    [Test]
    public async ValueTask Items_RemoveItem_Last() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment itemsWithoutGreen = (RenderFragment)CarouselItemRed + CarouselItemBlue + CarouselItemYellow;
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", itemsWithoutGreen));

        await Assert.That(carousel.ChildCount).IsEqualTo(3);
        await Assert.That(carousel.Active).IsEqualTo(1);
    }

    [Test]
    public async ValueTask Items_RemoveItem_Active() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment itemsWithoutRed = (RenderFragment)CarouselItemBlue + CarouselItemYellow + CarouselItemGreen;
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", itemsWithoutRed));

        await Assert.That(carousel.ChildCount).IsEqualTo(3);
        await Assert.That(carousel.Active).IsEqualTo(0);
    }

    [Test]
    public async ValueTask Items_RemoveLastChild_SetsActiveToMinus1() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, CarouselItemRed);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment noItems = (RenderTreeBuilder builder) => { };
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", noItems));

        await Assert.That(carousel.ChildCount).IsEqualTo(0);
        await Assert.That(carousel.Active).IsEqualTo(-1);
    }

    [Test]
    public async ValueTask Items_RemoveItem_FirstActive() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 0);
        });
        Carousel carousel = carouselContainer.Instance;

        RenderFragment itemsWithoutBlue = (RenderFragment)CarouselItemRed + CarouselItemYellow + CarouselItemGreen;
        carouselContainer.SetParametersAndRender(ComponentParameter.CreateParameter("Items", itemsWithoutBlue));

        await Assert.That(carousel.ChildCount).IsEqualTo(3);
        await Assert.That(carousel.Active).IsEqualTo(0);
    }

    [Test]
    public async ValueTask Overlay_Is_Rendered_In_OverlayDiv() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.Overlay, TEST_HTML);
        });

        IElement overlayDiv = carouselContainer.Find(".carousel-overlay");

        await Assert.That(overlayDiv.InnerHtml).IsEqualTo(TEST_HTML);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    public async ValueTask Active_Start_Sets_Active_Item(int activeValue) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, activeValue);
        });
        Carousel carousel = carouselContainer.Instance;

        await Assert.That(carousel.Active).IsEqualTo(activeValue);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask BeginRunning_Enables_Intervall(bool enabled) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, enabled);
        });
        Carousel carousel = carouselContainer.Instance;

        await Assert.That(carousel.Running).IsEqualTo(enabled);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask ControlArrowsEnable_Enables_Control_Arrows(bool enabled) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ControlArrowsEnable, enabled);
        });

        if (enabled) {
            await Assert.That(carouselContainer.FindAll(".carousel-arrow.prev")).HasSingleItem();
            await Assert.That(carouselContainer.FindAll(".carousel-arrow.next")).HasSingleItem();
        }
        else {
            await Assert.That(carouselContainer.FindAll(".carousel-arrow.prev")).IsEmpty();
            await Assert.That(carouselContainer.FindAll(".carousel-arrow.next")).IsEmpty();
        }
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask IndicatorsEnable_Enables_Indicators(bool enabled) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.IndicatorsEnable, enabled);
        });

        if (enabled)
            await Assert.That(carouselContainer.FindAll(".carousel-indicator-list")).HasSingleItem();
        else
            await Assert.That(carouselContainer.FindAll(".carousel-indicator-list")).IsEmpty();
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask PlayButtonEnable_Enables_Play_Button(bool enabled) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.PlayButtonEnable, enabled);
        });

        if (enabled)
            await Assert.That(carouselContainer.FindAll(".carousel-play-button")).HasSingleItem();
        else
            await Assert.That(carouselContainer.FindAll(".carousel-play-button")).IsEmpty();
    }

    #endregion


    #region interactive

    [Test]
    public async ValueTask Next_Click_Moves_To_Next_Item() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;
        IElement nextButton = carouselContainer.Find(".carousel-arrow.next");

        await Assert.That(carousel.Active).IsEqualTo(1);
        nextButton.Click();
        await Assert.That(carousel.Active).IsEqualTo(2);
        nextButton.Click();
        await Assert.That(carousel.Active).IsEqualTo(3);
        nextButton.Click();
        await Assert.That(carousel.Active).IsEqualTo(0);
    }

    [Test]
    public async ValueTask Prev_Click_Moves_To_Prev_Item() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 2);
        });
        Carousel carousel = carouselContainer.Instance;
        IElement prevButton = carouselContainer.Find(".carousel-arrow.prev");

        await Assert.That(carousel.Active).IsEqualTo(2);
        prevButton.Click();
        await Assert.That(carousel.Active).IsEqualTo(1);
        prevButton.Click();
        await Assert.That(carousel.Active).IsEqualTo(0);
        prevButton.Click();
        await Assert.That(carousel.Active).IsEqualTo(3);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    public async ValueTask Indicator_Click_Moves_To_Correspondent_Item(int index) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
        });
        Carousel carousel = carouselContainer.Instance;

        IElement indicatorList = carouselContainer.Find(".carousel-indicator-list");

        int index2 = (index + 1) % carousel.ChildCount;
        int index3 = (index + 3) % carousel.ChildCount;


        indicatorList.Children[index].Click();
        await Assert.That(carousel.Active).IsEqualTo(index);

        indicatorList.Children[index2].Click();
        await Assert.That(carousel.Active).IsEqualTo(index2);

        indicatorList.Children[index3].Click();
        await Assert.That(carousel.Active).IsEqualTo(index3);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask PlayButton_Click_Starts_And_Stops_Intervall(bool runAtBeginning) {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, runAtBeginning);
        });
        carouselContainer.Render();
        Carousel carousel = carouselContainer.Instance;
        IElement playButton = carouselContainer.Find(".carousel-play-button");

        await Assert.That(carousel.Running).IsEqualTo(runAtBeginning);
        playButton.Click();
        await Assert.That(carousel.Running).IsNotEqualTo(runAtBeginning);
        playButton.Click();
        await Assert.That(carousel.Running).IsEqualTo(runAtBeginning);
    }

    #endregion


    #region timer related

    [Test]
    public async ValueTask AutoStart_Activates_Interval() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 1000);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.AutoStartTime, 100);
        });
        Carousel carousel = carouselContainer.Instance;

        await Assert.That(carousel.Running).IsFalse();
        carouselContainer.WaitForAssertion(async () => await Assert.That(carousel.Running).IsTrue());
        carouselContainer.WaitForAssertion(async () => await Assert.That(carousel.Active).IsEqualTo(1));
    }

    #endregion


    #region api methods

    [Test]
    public async ValueTask SwapItem_Swaps_Items() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
        });
        Carousel carousel = carouselContainer.Instance;

        IRefreshableElementCollection<IElement> itemDivs = carouselContainer.FindAll(".carousel-element");
        IElement item0 = itemDivs[0];
        IElement item1 = itemDivs[1];
        await Assert.That(item1.Attributes["style"]!.Value).Contains("z-index: 20");
        await Assert.That(carousel.Active).IsEqualTo(1);

        carousel.SwapCarouselItems(0, 1);


        IRefreshableElementCollection<IElement> itemDivsAfter = carouselContainer.FindAll(".carousel-element");
        await Assert.That(item1.Attributes["style"]!.Value).Contains("z-index: 20");
        await Assert.That(carousel.Active).IsEqualTo(0);
    }

    [Test]
    public async ValueTask SetActiveItem_Sets_Item_With_Index_Active() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.Active = 2;
        await Assert.That(carousel.Active).IsEqualTo(2);

        carousel.Active = 0;
        await Assert.That(carousel.Active).IsEqualTo(0);

        carousel.Active = 3;
        await Assert.That(carousel.Active).IsEqualTo(3);
    }

    [Test]
    public async ValueTask StartInterval_Enables_Interval() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 100000);
        });
        Carousel carousel = carouselContainer.Instance;

        await Assert.That(carousel.Running).IsFalse();
        carousel.StartInterval();
        await Assert.That(carousel.Running).IsTrue();
        carousel.StartInterval();
        await Assert.That(carousel.Running).IsTrue();
    }

    [Test]
    public async ValueTask StopInterval_Disables_Interval() {
        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, true);
            builder.Add((Carousel carousel) => carousel.IntervalTime, 100000);
        });
        Carousel carousel = carouselContainer.Instance;

        await Assert.That(carousel.Running).IsTrue();
        carousel.StopInterval();
        await Assert.That(carousel.Running).IsFalse();
        carousel.StopInterval();
        await Assert.That(carousel.Running).IsFalse();
    }

    #endregion


    #region event

    [Test]
    public async ValueTask OnActiveChanged_Fires_When_Active_Item_Changes() {
        int fired = 0;

        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.ActiveStart, 1);
            builder.Add((Carousel carousel) => carousel.OnActiveChanged, (int index) => fired++);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.Active = 0;
        await Assert.That(fired).IsEqualTo(1);
    }

    [Test]
    public async ValueTask OnRunningChanged_Fires_When_Running_State_Changes() {
        int fired = 0;

        IRenderedComponent<Carousel> carouselContainer = RenderComponent((ComponentParameterCollectionBuilder<Carousel> builder) => {
            builder.Add((Carousel carousel) => carousel.Items, ItemsRedBlueYellowGreen);
            builder.Add((Carousel carousel) => carousel.BeginRunning, false);
            builder.Add((Carousel carousel) => carousel.OnRunningChanged, (bool running) => fired++);
        });
        Carousel carousel = carouselContainer.Instance;

        carousel.StartInterval();
        await Assert.That(fired).IsEqualTo(1);

        carousel.StartInterval();
        await Assert.That(fired).IsEqualTo(1);

        carousel.StopInterval();
        await Assert.That(fired).IsEqualTo(2);
    }

    #endregion
}
