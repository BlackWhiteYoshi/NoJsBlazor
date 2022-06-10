namespace ManualTesting.Client.Services;

public interface IPreRenderFlag {
    /// <summary>
    /// <para>Indicates whether the application is prerendering or running with interaction.</para>
    /// <para>True means it is preRendering.</para>
    /// </summary>
    public bool Flag { get; }
}
