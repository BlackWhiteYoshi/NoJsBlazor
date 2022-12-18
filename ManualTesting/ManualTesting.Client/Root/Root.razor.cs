using ManualTesting.Client.Services;

namespace ManualTesting.Client;

public sealed partial class Root : ServiceComponentBase, IRoot {
    public event Action<MouseEventArgs>? Click;


    private readonly PreRendering isPreRendering;

    public Root(PreRendering isPreRendering) {
        this.isPreRendering = isPreRendering;
    }
}
