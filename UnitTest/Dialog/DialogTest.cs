using AngleSharp.Dom;

namespace UnitTest;

public class DialogTest : TestContext {
    #region parameter

    [Fact]
    public void Title_Is_Rendered_In_TitleDiv() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.Title, TEST_HTML);
            builder.Add((Dialog dialog) => dialog.ShowTitle, true);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement div = dialogContainer.Find(".title");
        Assert.Equal(TEST_HTML, div.InnerHtml);
    }

    [Fact]
    public void Content_Is_Rendered_In_ContentDiv() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.Content, TEST_HTML);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement div = dialogContainer.Find(".content");
        Assert.Equal(TEST_HTML, div.InnerHtml);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShowTitle_Shows_Title(bool showTitle) {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.ShowTitle, showTitle);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IRefreshableElementCollection<IElement> divs = dialogContainer.FindAll(".title");
        if (showTitle)
            Assert.Single(divs);
        else
            Assert.Empty(divs);
    }

    [Theory]
    [InlineData(true, 20.0, 30.0)]
    [InlineData(true, 5.0, 1.0)]
    [InlineData(false, 10.0, 23.0)]
    public void Moveable_Allow_Moving_Window(bool moveable, double xMovement, double yMovement) {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.ShowTitle, true);
            builder.Add((Dialog dialog) => dialog.Moveable, moveable);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement titleDiv = dialogContainer.Find(".title");
        titleDiv.MouseDown(clientX: 0, clientY: 0);

        IElement windowDiv = dialogContainer.Find(".dialog-window");
        windowDiv.MouseMove(clientX: xMovement, clientY: yMovement, buttons: 1);

        if (moveable) {
            Assert.Equal(xMovement, dialog.XMovement);
            Assert.Equal(yMovement, dialog.YMovement);
        }
        else {
            Assert.Equal(0, dialog.XMovement);
            Assert.Equal(0, dialog.YMovement);
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ModalScreen_Enables_White_Background(bool enabled) {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.ModalScreen, enabled);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement modalDiv = dialogContainer.Find(".dialog-modal-background");
        IAttr style = modalDiv.Attributes["style"]!;

        if (enabled)
            Assert.Equal(string.Empty, style.Value);
        else
            Assert.Equal("background: #0000; pointer-events: none;", style.Value);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CloseOnModalBackground_Trigger_Close_By_Clicking_On_Background(bool enabled) {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.ModalScreen, true);
            builder.Add((Dialog dialog) => dialog.CloseOnModalBackground, enabled);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement modalDiv = dialogContainer.Find(".dialog-modal-background");
        modalDiv.MouseDown();

        Assert.Equal(!enabled, dialog.Active);
    }

    #endregion


    #region public methods

    [Fact]
    public void Open_Activates_Window() {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent<Dialog>();
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        Assert.True(dialog.Active);
    }

    [Fact]
    public void Close_Deactivates_Window() {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent<Dialog>();
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();
        dialog.Close();

        Assert.False(dialog.Active);
    }

    [Fact]
    public void OpenWithLastPosition_Opens_With_Last_Coordinates() {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.ShowTitle, true);
            builder.Add((Dialog dialog) => dialog.Moveable, true);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement titleDiv = dialogContainer.Find(".title");
        titleDiv.MouseDown(clientX: 0, clientY: 0);

        const double X_MOVEMENT = 20.0;
        const double Y_MOVEMENT = 30.0;

        IElement windowDiv = dialogContainer.Find(".dialog-window");
        windowDiv.MouseMove(clientX: X_MOVEMENT, clientY: Y_MOVEMENT, buttons: 1);

        dialog.Close();
        Assert.Equal(X_MOVEMENT, dialog.XMovement);
        Assert.Equal(Y_MOVEMENT, dialog.YMovement);

        dialog.OpenWithLastPosition();
        Assert.Equal(X_MOVEMENT, dialog.XMovement);
        Assert.Equal(Y_MOVEMENT, dialog.YMovement);

        dialog.Close();
        dialog.Open();
        Assert.Equal(0, dialog.XMovement);
        Assert.Equal(0, dialog.YMovement);
    }

    #endregion


    #region events

    [Fact]
    public void OnOpen_Fires_When_Dialog_Opens() {
        int fired = 0;

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.OnOpen, () => fired++);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        Assert.Equal(1, fired);
    }

    [Fact]
    public void OnClose_Fires_When_Dialog_Closes() {
        int fired = 0;

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.OnClose, () => fired++);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();
        dialog.Close();

        Assert.Equal(1, fired);
    }

    #endregion
}
