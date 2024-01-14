namespace ManualTesting.ClientHost.Services;

/// <summary>
/// Container for holding the flag if server currently prerendering.<br />
/// During SignalR interactive rendering this flag should be false, in all other cases it is prerendering.
/// </summary>
public sealed class PreRenderFlag {
    internal bool Flag { get; set; } = false;

    /// <summary>
    /// Parameter for delegate <see cref="Client.Services.PreRendering"/>
    /// </summary>
    /// <returns></returns>
    internal bool GetFlag() => Flag;
}
