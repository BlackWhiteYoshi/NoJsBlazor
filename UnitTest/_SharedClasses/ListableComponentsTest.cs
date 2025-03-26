namespace UnitTest;

public sealed class ListableComponentsTest : Bunit.TestContext {
    private class ListholdingComponentDummy : ListholdingComponentBase<ListableComponentDummy> {
        public List<ListableComponentDummy> ChildList => childList;
    }
    private class ListableComponentDummy : ListableComponentBase<ListableComponentDummy> { }


    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(10)]
    public async ValueTask ChildCount_Gives_Number_Of_Registered_Children(int count) {
        IRenderedComponent<ListholdingComponentDummy> listholdingComponentContainer = RenderComponent<ListholdingComponentDummy>();
        ListholdingComponentDummy listholdingComponent = listholdingComponentContainer.Instance;

        for (int i = 0; i < count; i++)
            RenderComponent((ComponentParameterCollectionBuilder<ListableComponentDummy> builder) => {
                builder.AddCascadingValue("Parent", (ListholdingComponentBase<ListableComponentDummy>)listholdingComponent);
            });

        await Assert.That(listholdingComponent.ChildCount).IsEqualTo(count);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(2)]
    [Arguments(3)]
    [Arguments(10)]
    public async ValueTask Children_Are_Registered_In_ChildList(int count) {
        IRenderedComponent<ListholdingComponentDummy> listholdingComponentContainer = RenderComponent<ListholdingComponentDummy>();
        ListholdingComponentDummy listholdingComponent = listholdingComponentContainer.Instance;

        IRenderedComponent<ListableComponentDummy>[] children = new IRenderedComponent<ListableComponentDummy>[count];
        for (int i = 0; i < count; i++)
            children[i] = RenderComponent((ComponentParameterCollectionBuilder<ListableComponentDummy> builder) => {
                builder.AddCascadingValue("Parent", (ListholdingComponentBase<ListableComponentDummy>)listholdingComponent);
            });

        for (int i = 0; i < count; i++)
            await Assert.That(listholdingComponent.ChildList[i]).IsEqualTo(children[i].Instance);
    }
}
