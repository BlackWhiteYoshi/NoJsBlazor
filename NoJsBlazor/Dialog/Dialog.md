# Dialog

A little message window that it is first hidden and can be displayed on demand.

```razor
<Dialog
        ShowTitle="true"
        Moveable="true"
        ModalScreen="true"
        CloseOnModalBackground="true"
        OnActiveChanged="(bool opened) => { }"
        OnTitlePointerDown="(PointerEventArgs e) => { }"
        OnTitlePointerMove="(PointerEventArgs e) => { }"
        OnTitlePointerUp="(PointerEventArgs e) => { }">
    <Title>
        RenderFragment
    </Title>
    <Content>
        RenderFragment
    </Content>
</Dialog>
```

```css
.dialog-modal-background {
    --modal-screen-background-color: #FFFC;
    --window-border-radius: 20px;
    --title-background-color: #FFF;
    --content-background-color: #EEE;
}
```


## CSS Variables: .dialog-modal-background

| **Name**                        | **Default Value** |
| ------------------------------- | ----------------- |
| --modal-screen-background-color | #FFFC             |
| --window-border-radius          | 20px              |
| --title-background-color        | #FFF              |
| --content-background-color      | #EEE              |


## Parameters

| **Name**               | **Type**                              | **Default Value** | **Description**                                                                                                                 |
| ---------------------- | ------------------------------------- | ----------------- | ------------------------------------------------------------------------------------------------------------------------------- |
| Title                  | RenderFragment?                       | null              | Html that will be displayed inside the head bar.                                                                                |
| Content                | RenderFragment?                       | null              | Html that will be displayed inside the window.                                                                                  |
| ShowTitle              | bool                                  | true              | Indicates whether to skip the *Title* section. Default is true.                                                                 |
| Moveable               | bool                                  | true              | The window can be grabed and draged around the screen. Default is true.                                                         |
| ModalScreen            | bool                                  | true              | If the background should be blurred/unavailable. Default is true.                                                               |
| CloseOnModalBackground | bool                                  | true              | If on the *ModalBackground* a click occurs, whether the window should close or not close. Default is true.                      |
| OnActiveChanged        | EventCallback&lt;bool&gt;             | default           | Invokes every time when the Dialog opens or closes. true: dialog got from close to open, false: dialog got from open to closed. |
| OnTitlePointerDown     | EventCallback&lt;PointerEventArgs&gt; | default           | Fires if pointer starts on title div.                                                                                           |
| OnTitlePointerMove     | EventCallback&lt;PointerEventArgs&gt; | default           | Fires if pointer moves on title div, but only if pointer down is active.                                                        |
| OnTitlePointerUp       | EventCallback&lt;PointerEventArgs&gt; | default           | Fires if pointer releases on title div.                                                                                         |


## Properties

| **Name**           | **Type**         | get/set | **Description**                                                                          |
| ------------------ | ---------------- | ------- | ---------------------------------------------------------------------------------------- |
| XMovement          | double           | get/set | Current offset of x-coordinate, 0 means middle of the screen.                            |
| YMovement          | double           | get/set | Current offset of y-coordinate, 0 means middle of the screen.                            |
| Active             | bool             | get/set | true -> window is shown, false -> window is closed                                       |
| SilentActiveSetter | bool             | set     | Sets the state of *Active* without notifying *OnActiveChanged*.                          |
| TitleDiv           | ElementReference | get     | The Container Element of the title head bar. You can use this to register mouse capture. |


## Methods

| **Name**             | **Parameters** | **ReturnType** | **Description**                                                                        |
| -------------------- | -------------- | -------------- | -------------------------------------------------------------------------------------- |
| Open                 | *empty*        | void           | Display the window. The same as setting *XMovement/YMovement = 0* and *Active = true*. |
| OpenWithLastPosition | *empty*        | void           | Display the window at the last moved position. The same as *Active = true*.            |
| Close                | *empty*        | void           | Closes the window. The same as *Active = false*.                                       |
