namespace ManualTesting.Client;

public interface IRoot : IComponent {
    public event Action<MouseEventArgs>? Click;
}
