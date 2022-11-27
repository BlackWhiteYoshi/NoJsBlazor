namespace ManualTesting.Client;

public interface IRoot : IComponent {
    [AllowNull]
    public PageComponentBase PageComponent { get; set; }

    public event Action<MouseEventArgs>? Click;
}
