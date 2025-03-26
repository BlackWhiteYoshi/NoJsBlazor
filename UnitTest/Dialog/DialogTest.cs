using AngleSharp.Dom;

namespace UnitTest;

public sealed class DialogTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask Title_Is_Rendered_In_TitleDiv() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.Title, TEST_HTML);
            builder.Add((Dialog dialog) => dialog.ShowTitle, true);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement div = dialogContainer.Find(".title");
        await Assert.That(div.InnerHtml).IsEqualTo(TEST_HTML);
    }

    [Test]
    public async ValueTask Content_Is_Rendered_In_ContentDiv() {
        const string TEST_HTML = "<p>Test Text</p>";

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.Content, TEST_HTML);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement div = dialogContainer.Find(".content");
        await Assert.That(div.InnerHtml).IsEqualTo(TEST_HTML);
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask ShowTitle_Shows_Title(bool showTitle) {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.ShowTitle, showTitle);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IRefreshableElementCollection<IElement> divs = dialogContainer.FindAll(".title");
        if (showTitle)
            await Assert.That(divs).HasSingleItem();
        else
            await Assert.That(divs).IsEmpty();
    }

    [Test]
    [Arguments(true, 20.0, 30.0)]
    [Arguments(true, 5.0, 1.0)]
    [Arguments(false, 10.0, 23.0)]
    public async ValueTask Moveable_Allow_Moving_Window(bool moveable, double xMovement, double yMovement) {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.ShowTitle, true);
            builder.Add((Dialog dialog) => dialog.Moveable, moveable);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement titleDiv = dialogContainer.Find(".title");
        titleDiv.PointerDown(clientX: 0, clientY: 0);

        IElement windowDiv = dialogContainer.Find(".dialog-window");
        windowDiv.PointerMove(clientX: xMovement, clientY: yMovement, buttons: 1);

        if (moveable) {
            await Assert.That(dialog.XMovement).IsEqualTo(xMovement);
            await Assert.That(dialog.YMovement).IsEqualTo(yMovement);
        }
        else {
            await Assert.That(dialog.XMovement).IsEqualTo(0);
            await Assert.That(dialog.YMovement).IsEqualTo(0);
        }
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask ModalScreen_Enables_White_Background(bool enabled) {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.ModalScreen, enabled);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement modalDiv = dialogContainer.Find(".dialog-modal-background");
        IAttr style = modalDiv.Attributes["style"]!;

        if (enabled)
            await Assert.That(style.Value).IsEqualTo(string.Empty);
        else
            await Assert.That(style.Value).IsEqualTo("background: #0000; pointer-events: none;");
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async ValueTask CloseOnModalBackground_Trigger_Close_By_Clicking_On_Background(bool enabled) {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.ModalScreen, true);
            builder.Add((Dialog dialog) => dialog.CloseOnModalBackground, enabled);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement modalDiv = dialogContainer.Find(".dialog-modal-background");
        modalDiv.Click();

        await Assert.That(dialog.Active).IsEqualTo(!enabled);
    }

    #endregion


    #region public methods

    [Test]
    public async ValueTask Open_Activates_Window() {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent<Dialog>();
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        await Assert.That(dialog.Active).IsTrue();
    }

    [Test]
    public async ValueTask Close_Deactivates_Window() {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent<Dialog>();
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();
        dialog.Close();

        await Assert.That(dialog.Active).IsFalse();
    }

    [Test]
    public async ValueTask OpenWithLastPosition_Opens_With_Last_Coordinates() {
        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.ShowTitle, true);
            builder.Add((Dialog dialog) => dialog.Moveable, true);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement titleDiv = dialogContainer.Find(".title");
        titleDiv.PointerDown(clientX: 0, clientY: 0);

        const double X_MOVEMENT = 20.0;
        const double Y_MOVEMENT = 30.0;

        IElement windowDiv = dialogContainer.Find(".dialog-window");
        windowDiv.PointerMove(clientX: X_MOVEMENT, clientY: Y_MOVEMENT, buttons: 1);

        dialog.Close();
        await Assert.That(dialog.XMovement).IsEqualTo(X_MOVEMENT);
        await Assert.That(dialog.YMovement).IsEqualTo(Y_MOVEMENT);

        dialog.OpenWithLastPosition();
        await Assert.That(dialog.XMovement).IsEqualTo(X_MOVEMENT);
        await Assert.That(dialog.YMovement).IsEqualTo(Y_MOVEMENT);

        dialog.Close();
        dialog.Open();
        await Assert.That(dialog.XMovement).IsEqualTo(0);
        await Assert.That(dialog.YMovement).IsEqualTo(0);
    }

    [Test]
    public async ValueTask SilentActiveSetter_Sets_Without_Notifying() {
        int fired = 0;

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.OnActiveChanged, () => fired++);
        });
        Dialog dialog = dialogContainer.Instance;

        dialog.SilentActiveSetter = true;
        await Assert.That(dialog.Active).IsTrue();
        await Assert.That(fired).IsEqualTo(0);

        dialog.SilentActiveSetter = false;
        await Assert.That(dialog.Active).IsFalse();
        await Assert.That(fired).IsEqualTo(0);
    }

    #endregion


    #region events

    [Test]
    public async ValueTask OnActiveChanged_Fires_When_Dialog_Active_Changed() {
        int open = 0;
        int close = 0;

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.OnActiveChanged, (bool value) => {
                if (value)
                    open++;
                else
                    close++;
            });
        });
        Dialog dialog = dialogContainer.Instance;
        
        dialog.Open();
        await Assert.That(open).IsEqualTo(1);
        await Assert.That(close).IsEqualTo(0);

        dialog.Close();
        await Assert.That(open).IsEqualTo(1);
        await Assert.That(close).IsEqualTo(1);
    }

    [Test]
    public async ValueTask OnTitlePointerDown_Fires_When_Title_PointerDown() {
        int fired = 0;

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.OnTitlePointerDown, (PointerEventArgs e) => fired++);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement titleDiv = dialogContainer.Find(".title");
        titleDiv.PointerDown(clientX: 0, clientY: 0);
        await Assert.That(fired).IsEqualTo(1);
    }

    [Test]
    public async ValueTask OnTitlePointerMove_Fires_When_Title_PointerMove_After_OnTitlePointerDown() {
        int fired = 0;

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.OnTitlePointerMove, (PointerEventArgs e) => fired++);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement titleDiv = dialogContainer.Find(".title");
        titleDiv.PointerMove(clientX: 0, clientY: 0);
        await Assert.That(fired).IsEqualTo(0);

        titleDiv.PointerDown(clientX: 0, clientY: 0);
        titleDiv.PointerMove(clientX: 0, clientY: 0);
        await Assert.That(fired).IsEqualTo(1);
    }


    [Test]
    public async ValueTask OnTitlePointerUp_Fires_When_Title_PointerUp() {
        int fired = 0;

        IRenderedComponent<Dialog> dialogContainer = RenderComponent((ComponentParameterCollectionBuilder<Dialog> builder) => {
            builder.Add((Dialog dialog) => dialog.OnTitlePointerUp, (PointerEventArgs e) => fired++);
        });
        Dialog dialog = dialogContainer.Instance;
        dialog.Open();

        IElement titleDiv = dialogContainer.Find(".title");
        titleDiv.PointerUp(clientX: 0, clientY: 0);
        await Assert.That(fired).IsEqualTo(1);
    }

    #endregion
}
