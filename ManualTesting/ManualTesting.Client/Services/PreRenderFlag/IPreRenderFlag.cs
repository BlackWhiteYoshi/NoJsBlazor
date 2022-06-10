namespace ManualTesting.Client.Services;

/// <summary>
/// <para>Indicates whether the application is prerendering or running with interaction.</para>
/// <para>True means it is preRendering.</para>
/// </summary>
public interface IPreRenderFlag {
    public bool Flag { get; }
}
