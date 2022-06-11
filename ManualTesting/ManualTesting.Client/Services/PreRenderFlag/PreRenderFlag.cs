﻿namespace ManualTesting.Client.Services;

/// <summary>
/// Core entrypoint is only executed in the browser, so preRendering is always false.
/// </summary>
public class PreRenderFlag : IPreRenderFlag {
    public bool Flag => false;
}
