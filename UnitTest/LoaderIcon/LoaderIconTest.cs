using AngleSharp.Dom;

namespace UnitTest;

public sealed class LoaderIconTest : Bunit.TestContext {
    #region parameter

    [Test]
    public async ValueTask LoaderIconHas4Children() {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent<LoaderIcon>();

        IElement loaderIconDiv = loaderIconContainer.Find(".loader-icon");
        await Assert.That(loaderIconDiv.ChildElementCount).IsEqualTo(4);
    }

    #endregion
}
