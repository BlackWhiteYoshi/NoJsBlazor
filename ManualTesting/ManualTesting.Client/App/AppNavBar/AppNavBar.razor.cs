﻿using NoJsBlazor;

namespace ManualTesting.Client;

public sealed partial class AppNavBar : ComponentBase {
    [Inject]
    public required IApp Root { private get; init; }


    [AllowNull]
    private NavBar navBar;


    private void PhoneToggle(bool expanded) {
        if (expanded)
            Root.Click += Reset;
        else
            Root.Click -= Reset;
    }

    private void Reset(MouseEventArgs e) => navBar.Reset();
}
