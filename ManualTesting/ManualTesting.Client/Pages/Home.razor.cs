using NoJsBlazor;

namespace ManualTesting.Client;

public sealed partial class Home : ComponentBase, IDisposable {
    [Inject]
    public required IApp Root { private get; init; }

    [Inject]
    public required IJSRuntime JsRuntime { private get; init; }


    public void Dispose() {
        Root.Click -= CloseContextMenu;
    }

    #region Carousel

    [AllowNull]
    private Carousel carousel;

    private readonly List<string> colorList = new() {
        "linear-gradient(90deg, red, blue)",
        "linear-gradient(90deg, blue, yellow)",
        "linear-gradient(90deg, yellow, green)",
        "linear-gradient(90deg, green, red)"
    };

    private int swap1;
    private int swap2;

    private CarouselAnimation carouselAnimation = CarouselAnimation.FadeOut;
    private bool controlArrowsEnable = true;
    private bool indicatorsEnable = true;
    private bool playButtonEnable = true;
    private double intervalTime = 4000.0;
    private double autoStartTime = 0.0;

    #endregion


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

    private void OnTitleDown(PointerEventArgs e) => ((IJSInProcessRuntime)JsRuntime).InvokeVoid("setPointerCapture", dialog.TitleDiv, e.PointerId);

    private void OnTitleUp(PointerEventArgs e) => ((IJSInProcessRuntime)JsRuntime).InvokeVoid("releasePointerCapture", dialog.TitleDiv, e.PointerId);

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
