using NoJsBlazor;

namespace ManualTesting.Client;

public sealed partial class Home : PageComponentBase, IDisposable {
    [Inject]
    public required ITopLevelPortal TopLevelPortal { get; init; }


    protected override void OnInitialized() {
        base.OnInitialized();
        TopLevelPortal.Add(RenderDialog);
    }

    public new void Dispose() {
        base.Dispose();
        Root.Click -= CloseContextMenu;
        TopLevelPortal.Remove(RenderDialog);
    }


    #region ContextMenu

    [AllowNull]
    private ContextMenu contextMenu;

    private void OpenContextMenu(MouseEventArgs e) {
        // padding-left = 20px, padding-top = 20px
        contextMenu.Open(e.OffsetX + 20, e.OffsetY + 20);
        Root.Click += CloseContextMenu;
    }

    private void CloseContextMenu(MouseEventArgs e) {
        contextMenu.Close();
        Root.Click -= CloseContextMenu;
    }

    #endregion


    #region Dialog

    [AllowNull]
    private Dialog dialog;

    private void OpenDialog(MouseEventArgs e) => dialog.Open();

    private void CloseDialog(MouseEventArgs e) => dialog.Close();

    #endregion


    #region ProgressBar

    private float standardBarProgress;
    private string standardBarText = string.Empty;
    private decimal standardPercentage = 0.0m;

    private void StandardProgressBarButtonClick(MouseEventArgs e) {
        standardPercentage += 0.01m;
        if (standardPercentage > 1.0m)
            standardPercentage = 0.0m;

        standardBarProgress = (float)standardPercentage;
        standardBarText = $"Load {standardPercentage * 100:0}%";
    }


    [AllowNull]
    private CircleProgressBar circleProgress;
    private decimal circlePercentage = 0.0m;

    private void CircleProgressBarButtonClick(MouseEventArgs e) {
        circlePercentage += 0.01m;
        if (circlePercentage > 1.0m)
            circlePercentage = 0.0m;

        circleProgress.Content = ((float)circlePercentage, $"Load {circlePercentage * 100:0}%");
    }


    [AllowNull]
    private SpeedometerProgressBar speedometerProgress;
    private decimal speedometerPercentage = 0.0m;

    private void SpeedometerProgressBarButtonClick(MouseEventArgs e) {
        speedometerPercentage += 0.1m;
        if (speedometerPercentage > 1.0m)
            speedometerPercentage = 0.0m;

        speedometerProgress.Content = ((float)speedometerPercentage, $"Load {speedometerPercentage * 100:0}%");
    }

    #endregion
}
