namespace ManualTesting.Client;

public interface IApp : IComponent {
    public event Action<MouseEventArgs>? Click;
}
