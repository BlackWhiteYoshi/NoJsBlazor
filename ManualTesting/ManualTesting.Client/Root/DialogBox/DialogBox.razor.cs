namespace ManualTesting.Client;

public partial class DialogBox : ComponentBase {
    private readonly List<RenderFragment> dialogList = new();

    public void AddDialog(RenderFragment renderFragment) {
        dialogList.Add(renderFragment);
        InvokeAsync(StateHasChanged);
    }

    public void RemoveDialog(RenderFragment renderFragment) {
        dialogList.Remove(renderFragment);
        InvokeAsync(StateHasChanged);
    }
}
