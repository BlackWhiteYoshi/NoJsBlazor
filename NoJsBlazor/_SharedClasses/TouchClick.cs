namespace NoJsBlazor;

/// <summary>
/// <para>Objects of that class can handle mouse and touch input properly, that on mouse/touch down the corresponding given methods invokes.</para>
/// <para>It manages, that the simulated click on phnone devices will not trigger the given method a second time and also only the left click will trigger an invoke.</para>
/// </summary>
public class TouchClick {
    private bool isTouch = false;
    private bool isDown = false;
    private readonly Action<EventArgs>? OnDown;
    private readonly Action<EventArgs>? OnMove;
    private readonly Action<EventArgs>? OnUp;

    /// <summary>
    /// Creates an Object that handles mouse and touch input properly to invoke the corresponding given methods.
    /// </summary>
    /// <param name="OnDown">
    /// <para>Will be executed once every time when someone clickes or touches the item.</para>
    /// <para><see cref="EventArgs"/> is either an instance of  <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>.</para>
    /// </param>
    /// <param name="OnMove">
    /// <para>Wxecutes only if OnDown happend before.</para>
    /// <para><see cref="EventArgs"/> is either an instance of  <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>.</para>
    /// </param>
    /// <param name="OnUp">
    /// <para>Will be executed once every time when someone releases the item but only if OnDown happend.</para>
    /// <para><see cref="EventArgs"/> is either an instance of  <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>.</para>
    /// </param>
    public TouchClick(Action<EventArgs>? OnDown = null, Action<EventArgs>? OnMove = null, Action<EventArgs>? OnUp = null) {
        this.OnDown = OnDown;
        this.OnMove = OnMove;
        this.OnUp = OnUp;
    }


    /// <summary>
    /// usage: @onmousedown="[MyTouchClickObject].OnMouseDown"
    /// </summary>
    public Action<MouseEventArgs> OnMouseDown => OnMouseDownAction;
    /// <summary>
    /// usage: @ontouchstart="[MyTouchClickObject].OnTouchStart"
    /// </summary>
    public Action<TouchEventArgs> OnTouchStart => OnTouchStartAction;
    /// <summary>
    /// usage: @onmousemove="[MyTouchClickObject].OnMouseMove"
    /// </summary>
    public Action<MouseEventArgs> OnMouseMove => OnMouseMoveAction;
    /// <summary>
    /// usage: @ontouchmove="[MyTouchClickObject].OnTouchMove"
    /// </summary>
    public Action<TouchEventArgs> OnTouchMove => OnTouchMoveAction;
    /// <summary>
    /// usage: @onmouseup="[MyTouchClickObject].OnMouseUp"
    /// </summary>
    public Action<MouseEventArgs> OnMouseUp => OnMouseUpAction;
    /// <summary>
    /// usage: @ontouchend="[MyTouchClickObject].OnTouchEnd"
    /// </summary>
    public Action<TouchEventArgs> OnTouchEnd => OnTouchEndAction;


    private void OnMouseDownAction(MouseEventArgs e) {
        if (isTouch || e.Button != 0) {
            isTouch = false;
            return;
        }

        isDown = true;
        OnDown?.Invoke(e);
    }

    private void OnTouchStartAction(TouchEventArgs e) {
        isTouch = true;
        isDown = true;
        OnDown?.Invoke(e);
    }

    private void OnMouseMoveAction(MouseEventArgs e) {
        if (isTouch || !isDown)
            return;

        // "e.Buttons & 0x1" means left mouse button down
        if ((e.Buttons & 0x1) != 1) {
            OnMouseUpAction(e);
            return;
        }

        OnMove?.Invoke(e);
    }

    private void OnTouchMoveAction(TouchEventArgs e) {
        if (!isDown)
            return;

        OnMove?.Invoke(e);
    }

    private void OnMouseUpAction(MouseEventArgs e) {
        if (isTouch || !isDown)
            return;

        isDown = false;
        OnUp?.Invoke(e);
    }

    private void OnTouchEndAction(TouchEventArgs e) {
        if (!isDown)
            return;

        isDown = false;
        OnUp?.Invoke(e);
    }
}

/// <summary>
/// <para>Objects of that class can handle mouse and touch input properly, that on mouse/touch down the corresponding given methods invokes.</para>
/// <para>It manages, that the simulated click on phnone devices will not trigger the given method a second time and also only the left click will trigger an invoke.</para>
/// </summary>
/// <typeparam name="T">T should be a data-model that can hold all necessary parameters.</typeparam>
public class TouchClick<T> : TouchClick {
    /// <summary>
    /// Creates an Object that handles mouse and touch input properly to invoke the corresponding given methods.
    /// </summary>
    /// <param name="OnDown">
    /// <para>Will be executed once every time when someone clickes or touches the item.</para>
    /// <para><see cref="EventArgs"/> is either an instance of  <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>.</para>
    /// </param>
    /// <param name="OnMove">
    /// <para>Wxecutes only if OnDown happend before.</para>
    /// <para><see cref="EventArgs"/> is either an instance of  <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>.</para>
    /// </param>
    /// <param name="OnUp">
    /// <para>Will be executed once every time when someone releases the item but only if OnDown happend.</para>
    /// <para><see cref="EventArgs"/> is either an instance of  <see cref="MouseEventArgs"/> or <see cref="TouchEventArgs"/>.</para>
    /// </param>
    public TouchClick(Action<EventArgs>? OnDown = null, Action<EventArgs>? OnMove = null, Action<EventArgs>? OnUp = null) : base(OnDown, OnMove, OnUp) { }


    /// <summary>
    /// <para>T should be a model that can hold all necessary parameters.</para>
    /// <para>Set the parameters before invoking the Action, so the parameters can be used in the given method.</para>
    /// </summary>
    public T? Parameter { get; private set; }


    /// <summary>
    /// usage: @onmousedown="(MouseEventArgs e) => [MyTouchClickObject].OnMouseDown(e, parameter)"
    /// </summary>
    public new Action<MouseEventArgs, T> OnMouseDown => OnMouseDownAction;
    /// <summary>
    /// usage: @ontouchstart="(TouchEventArgs e) => [MyTouchClickObject].OnTouchStart(e, parameter)"
    /// </summary>
    public new Action<TouchEventArgs, T> OnTouchStart => OnTouchStartAction;
    /// <summary>
    /// usage: @onmousemove="(MouseEventArgs e) => [MyTouchClickObject].OnMouseMove(e, parameter)"
    /// </summary>
    public new Action<MouseEventArgs, T> OnMouseMove => OnMouseMoveAction;
    /// <summary>
    /// usage: @ontouchmove="(TouchEventArgs e) => [MyTouchClickObject].OnTouchMove(e, parameter)"
    /// </summary>
    public new Action<TouchEventArgs, T> OnTouchMove => OnTouchMoveAction;
    /// <summary>
    /// usage: @onmouseup="(MouseEventArgs e) => [MyTouchClickObject].OnMouseUp(e, parameter)"
    /// </summary>
    public new Action<MouseEventArgs, T> OnMouseUp => OnMouseUpAction;
    /// <summary>
    /// usage: @ontouchend="(TouchEventArgs e) => [MyTouchClickObject].OnTouchEnd(e, parameter)"
    /// </summary>
    public new Action<TouchEventArgs, T> OnTouchEnd => OnTouchEndAction;


    private void OnMouseDownAction(MouseEventArgs e, T parameter) {
        Parameter = parameter;
        base.OnMouseDown(e);
    }

    private void OnTouchStartAction(TouchEventArgs e, T parameter) {
        Parameter = parameter;
        base.OnTouchStart(e);
    }

    private void OnMouseMoveAction(MouseEventArgs e, T parameter) {
        Parameter = parameter;
        base.OnMouseMove(e);
    }

    private void OnTouchMoveAction(TouchEventArgs e, T parameter) {
        Parameter = parameter;
        base.OnTouchMove(e);
    }

    private void OnMouseUpAction(MouseEventArgs e, T parameter) {
        Parameter = parameter;
        base.OnMouseUp(e);
    }

    private void OnTouchEndAction(TouchEventArgs e, T parameter) {
        Parameter = parameter;
        base.OnTouchEnd(e);
    }
}
