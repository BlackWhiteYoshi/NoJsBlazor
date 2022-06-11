namespace ManualTesting.Client;

public interface IRoot {
    public PageComponentBase? PageComponent { get; set; }

    public event Action<MouseEventArgs>? Click;
    public event Action<MouseEventArgs>? MouseDown;
    public event Action<TouchEventArgs>? TouchStart;
    public event Action<MouseEventArgs>? MouseMove;
    public event Action<TouchEventArgs>? TouchMove;
    public event Action<MouseEventArgs>? MouseUp;
    public event Action<TouchEventArgs>? TouchEnd;
}
