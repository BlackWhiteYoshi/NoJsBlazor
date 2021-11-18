namespace UnitTest;

public class ListableComponentsTest : TestContext {
    private class ListholdingComponentDummy : ListholdingComponentBase<ListableComponentDummy> {
        public List<ListableComponentDummy> ChildList => childList;
    }
    private class ListableComponentDummy : ListableComponentBase<ListableComponentDummy> { }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(10)]
    public void ChildCount_Gives_Number_Of_Registered_Children(int count) {
        IRenderedComponent<ListholdingComponentDummy> listholdingComponentContainer = RenderComponent<ListholdingComponentDummy>();
        ListholdingComponentDummy listholdingComponent = listholdingComponentContainer.Instance;

        for (int i = 0; i < count; i++)
            RenderComponent((ComponentParameterCollectionBuilder<ListableComponentDummy> builder) => {
                builder.AddCascadingValue("Parent", (ListholdingComponentBase<ListableComponentDummy>)listholdingComponent);
            });

        Assert.Equal(count, listholdingComponent.ChildCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(10)]
    public void Children_Are_Registered_In_ChildList(int count) {
        IRenderedComponent<ListholdingComponentDummy> listholdingComponentContainer = RenderComponent<ListholdingComponentDummy>();
        ListholdingComponentDummy listholdingComponent = listholdingComponentContainer.Instance;

        IRenderedComponent<ListableComponentDummy>[] children = new IRenderedComponent<ListableComponentDummy>[count];
        for (int i = 0; i < count; i++)
            children[i] = RenderComponent((ComponentParameterCollectionBuilder<ListableComponentDummy> builder) => {
                builder.AddCascadingValue("Parent", (ListholdingComponentBase<ListableComponentDummy>)listholdingComponent);
            });

        for (int i = 0; i < count; i++)
            Assert.Equal(children[i].Instance, listholdingComponent.ChildList[i]);
    }
}
