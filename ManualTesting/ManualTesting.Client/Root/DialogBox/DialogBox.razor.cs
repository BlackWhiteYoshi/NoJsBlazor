namespace ManualTesting.Client;

public partial class DialogBox : ComponentBase {
    private readonly List<RenderFragment> dialogList = new();

    public void Add(RenderFragment renderFragment) {
        dialogList.Add(renderFragment);
        InvokeAsync(StateHasChanged);
    }

    public void Remove(RenderFragment renderFragment) {
        dialogList.Remove(renderFragment);
        InvokeAsync(StateHasChanged);
    }
}
