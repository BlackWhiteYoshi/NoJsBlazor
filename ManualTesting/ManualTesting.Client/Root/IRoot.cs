namespace ManualTesting.Client;

public interface IRoot {
    [AllowNull]
    public PageComponentBase PageComponent { get; set; }

    public event Action<MouseEventArgs>? Click;
}
