using NoJsBlazor;

namespace ManualTesting;

public partial class Home : PageComponentBase, IDisposable {
    #region ContextMenu

    [AllowNull]
    private ContextMenu contextMenu;

    private void OpenContextMenu(MouseEventArgs e) {
        contextMenu.Open(e.OffsetX, e.OffsetY);
        Root.MouseDown += CloseContextMenu;
    }

    private void CloseContextMenu(MouseEventArgs e) {
        contextMenu.Close();
        Root.MouseDown -= CloseContextMenu;
    }

    #endregion


    #region Dialog

    [AllowNull]
    private Dialog dialog;

    private void OpenDialog(MouseEventArgs e) => dialog.Open();

    private void CloseDialog(MouseEventArgs e) => dialog.Close();

    #endregion


    #region ProgressBar

    [AllowNull]
    private StandardProgressBar standardProgress;
    private decimal standardPercentage = 0.0m;

    private void StandardProgressBarButtonClick(MouseEventArgs e) {
        standardPercentage += 0.01m;
        if (standardPercentage > 1.0m)
            standardPercentage = 0.0m;

        standardProgress.Progress = (float)standardPercentage;
        standardProgress.Text = $"Load {standardPercentage * 100:0}%";
    }


    [AllowNull]
    private CircleProgressBar circleProgress;
    private decimal circlePercentage = 0.0m;

    private void CircleProgressBarButtonClick(MouseEventArgs e) {
        circlePercentage += 0.01m;
        if (circlePercentage > 1.0m)
            circlePercentage = 0.0m;

        circleProgress.Progress = (float)circlePercentage;
        circleProgress.Text = $"Load {circlePercentage * 100:0}%";
    }


    [AllowNull]
    private SpeedometerProgressBar speedometerProgress;
    private decimal speedometerPercentage = 0.0m;

    private void SpeedometerProgressBarButtonClick(MouseEventArgs e) {
        speedometerPercentage += 0.01m;
        if (speedometerPercentage > 1.0m)
            speedometerPercentage = 0.0m;

        speedometerProgress.Progress = (float)speedometerPercentage;
        speedometerProgress.Text = $"Load {speedometerPercentage * 100:0}%";
    }

    #endregion


    public void Dispose() {
        Root.MouseDown -= CloseContextMenu;
    }
}
