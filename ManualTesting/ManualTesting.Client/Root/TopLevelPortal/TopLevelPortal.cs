namespace ManualTesting.Client;

/// <summary>
/// A Portal located at the top layer of &lt;body&gt;, so e.g. dialog-RenderFragments can be rendered here.
/// </summary>
public sealed partial class DialogBox : Portal<ITopLevelPortal>, ITopLevelPortal { }
