using ManualTesting.Client.Services;

namespace ManualTesting.Server.Services;

/// <summary>
/// Webassembly entrypoint runs always on the server, so preRendering is always true.
/// </summary>
public sealed class PreRenderFlag : IPreRenderFlag {
    public bool Flag => true;
}
