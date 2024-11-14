using ManualTesting.Client.Services;

namespace ManualTesting.Client;

public sealed partial class App : ServiceComponentBase, IApp {
    public event Action<MouseEventArgs>? Click;
}
