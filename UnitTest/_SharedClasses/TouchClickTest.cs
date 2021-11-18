namespace UnitTest;

public class TouchClickTest {
    #region OnDown

    [Fact]
    public void OnMouseDown_Invokes_OnDown() {
        int fired = 0;

        TouchClick touchClick = new(OnDown: (EventArgs e) => fired++);

        touchClick.OnMouseDown(new MouseEventArgs());
        Assert.Equal(1, fired);
    }

    [Fact]
    public void OnTouchStart_Invokes_OnDown() {
        int fired = 0;

        TouchClick touchClick = new(OnDown: (EventArgs e) => fired++);

        touchClick.OnTouchStart(new TouchEventArgs());
        Assert.Equal(1, fired);
    }

    #endregion


    #region OnMove


    #region mouse

    [Fact]
    public void OnMouseMove_Invokes_OnMove() {
        int fired = 0;

        TouchClick touchClick = new(OnMove: (EventArgs e) => fired++);

        touchClick.OnMouseDown(new MouseEventArgs());
        touchClick.OnMouseMove(new MouseEventArgs() { Buttons = 1 });
        Assert.Equal(1, fired);
    }

    [Fact]
    public void OnMouseMove_Do_Not_Invoke_Without_OnDown() {
        int fired = 0;

        TouchClick touchClick = new(OnMove: (EventArgs e) => fired++);

        touchClick.OnMouseMove(new MouseEventArgs());
        Assert.Equal(0, fired);
    }

    [Fact]
    public void OnMouseMove_Do_Not_Invoke_Without_Left_Mouse_Button_Down() {
        int fired = 0;

        TouchClick touchClick = new(OnMove: (EventArgs e) => fired++);

        touchClick.OnMouseDown(new MouseEventArgs());
        touchClick.OnMouseMove(new MouseEventArgs());
        Assert.Equal(0, fired);
    }

    #endregion


    #region touch

    [Fact]
    public void OnTouchMove_Invokes_OnTouch() {
        int fired = 0;

        TouchClick touchClick = new(OnMove: (EventArgs e) => fired++);

        touchClick.OnTouchStart(new TouchEventArgs());
        touchClick.OnTouchMove(new TouchEventArgs());
        Assert.Equal(1, fired);
    }

    [Fact]
    public void OnTouchMove_Do_Not_Invoke_Without_OnDown() {
        int fired = 0;

        TouchClick touchClick = new(OnMove: (EventArgs e) => fired++);

        touchClick.OnTouchMove(new TouchEventArgs());
        Assert.Equal(0, fired);
    }

    #endregion


    #endregion


    #region OnUp


    #region mouse

    [Fact]
    public void OnMouseUp_Invokes_OnUp() {
        int fired = 0;

        TouchClick touchClick = new(OnUp: (EventArgs e) => fired++);

        touchClick.OnMouseDown(new MouseEventArgs());
        touchClick.OnMouseUp(new MouseEventArgs());
        Assert.Equal(1, fired);
    }

    [Fact]
    public void OnMouseUp_Do_Not_Invoke_Without_OnDown() {
        int fired = 0;

        TouchClick touchClick = new(OnUp: (EventArgs e) => fired++);

        touchClick.OnMouseUp(new MouseEventArgs());
        Assert.Equal(0, fired);
    }

    [Fact]
    public void OnUp_Get_Invoked_When_OnMouseMove_Without_LeftButton() {
        int fired = 0;

        TouchClick touchClick = new(OnUp: (EventArgs e) => fired++);

        touchClick.OnMouseDown(new MouseEventArgs());
        touchClick.OnMouseMove(new MouseEventArgs());
        Assert.Equal(1, fired);
    }

    #endregion


    #region touch

    [Fact]
    public void OnTouchEnd_Invokes_OnUp() {
        int fired = 0;

        TouchClick touchClick = new(OnUp: (EventArgs e) => fired++);

        touchClick.OnTouchStart(new TouchEventArgs());
        touchClick.OnTouchEnd(new TouchEventArgs());
        Assert.Equal(1, fired);
    }

    [Fact]
    public void OnTouchEnd_Do_Not_Invoke_Without_OnDown() {
        int fired = 0;

        TouchClick touchClick = new(OnUp: (EventArgs e) => fired++);

        touchClick.OnTouchEnd(new TouchEventArgs());
        Assert.Equal(0, fired);
    }

    #endregion


    #endregion
}
