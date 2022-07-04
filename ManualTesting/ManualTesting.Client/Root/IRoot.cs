namespace ManualTesting.Client;

public interface IRoot {
    public PageComponentBase? PageComponent { get; set; }

    public event Action<MouseEventArgs>? Click;
}
