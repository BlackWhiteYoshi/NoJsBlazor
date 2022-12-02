namespace ManualTesting.Client.Services;

/// <summary>
/// <para>Indicates whether the application is prerendering or running with interaction.</para>
/// <para>True means it is preRendering.</para>
/// </summary>
/// <returns>True, if it is preRendering, otherwise false.</returns>
public delegate bool PreRendering();
