using AngleSharp.Dom;

namespace UnitTest;

public class LoaderIconTest : TestContext {
    #region parameter

    [Fact]
    public void LoaderIconHas4Children() {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent<LoaderIcon>();

        IElement loaderIconDiv = loaderIconContainer.Find(".loader-icon");
        Assert.Equal(4, loaderIconDiv.ChildElementCount);
    }

    #endregion
}
