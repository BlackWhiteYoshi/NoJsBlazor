using Microsoft.AspNetCore.Components;

namespace NoJsBlazor {
    public class ContextChild : ListableHoldingComponentBase<ContextChild> {
        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
