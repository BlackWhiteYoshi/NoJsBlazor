# ContextMenu

A menu that can be toggled and shown at mouse position.
Triggering the menu is not included.

```razor
<ContextMenu OnToggle="(bool expanded) => { }">
    <ContextSubMenu OnToggle="(bool expanded) => { }">
        <Head>RenderFragment</Head>
        <Items>
            <ContextMenuItem OnPressed="(MouseEventArgs e) => { }">RenderFragment</ContextMenuItem>
        </Items>
    </ContextSubMenu>
    <ContextMenuItem OnPressed="(MouseEventArgs e) => { }">RenderFragment</ContextMenuItem>
</ContextMenu>
```

```css
.context-menu {
    --background-image: linear-gradient(90deg, #CCFC, #EEFC, #CCFC);
    --arrow-size: 10px;
    --arrow-color: #000B;
    --arrow-hover-color: #FFFB;
    --arrow-hover-stroke: #000;
    --hover-dropright-background-color: #FFF6;
    --hover-dropright-shadow-color: #FFF;
    --hover-item-background-color: #FFF8;
}
```


## CSS Variables: .context-menu

| **Name**                           | **Default Value**                           |
| ---------------------------------- | ------------------------------------------- |
| --background-image                 | linear-gradient(90deg, #CCFC, #EEFC, #CCFC) |
| --arrow-size                       | 10px                                        |
| --arrow-color                      | #000B                                       |
| --arrow-hover-color                | #FFFB                                       |
| --arrow-hover-stroke               | #000                                        |
| --hover-dropright-background-color | #FFF6                                       |
| --hover-dropright-shadow-color     | #FFF                                        |
| --hover-item-background-color      | #FFF8                                       |


## Parameters

| **Name**     | **Type**                  | **Default Value** | **Dexcription**                                                          |
| ------------ | ------------------------- | ----------------- | ------------------------------------------------------------------------ |
| ChildContent | RenderFragment            | *required*        | Content of this ContextMenu.                                             |
| OnToggle     | EventCallback&lt;bool&gt; | default           | Fires every time when *Expanded* get changed. Value is equal *Expanded*. |


## Properties

| **Name** | **Type** | get/set | **Dexcription**                    |
| -------- | -------- | ------- | ---------------------------------- |
| Expanded | bool     | get     | The state of collapsed or visible. |


## Methods

| **Name**    | **Parameters**     | **ReturnType** | **Dexcription**                                                                           |
| ----------- | ------------------ | -------------- | ----------------------------------------------------------------------------------------- |
| Open        | double x, double y | void           | Display the context menu at the given location.                                           |
| Close       | *empty*            | void           | Collapses all expanded submenus and closes the context menu.                              |
| SilentOpen  | double x, double y | void           | Display the context menu at the given location without notifying *OnToggle*.              |
| SilentClose | *empty*            | void           | Collapses all expanded submenus and closes the context menu without notifying *OnToggle*. |
| Reset       | *empty*            | void           | Collapses all expanded submenus.                                                          |


<br></br>
## ContextSubMenu

Another *ContextMenu* inside of a *ContextMenu* or *ContextSubMenu*.

### Parameters

| **Name** | **Type**                  | **Default Value** | **Dexcription**                                                          |
| -------- | ------------------------- | ----------------- | ------------------------------------------------------------------------ |
| Head     | RenderFragment            | *required*        | Content that is collapsed visible.                                       |
| Items    | RenderFragment            | *required*        | Content that is expanded visible.                                        |
| OnToggle | EventCallback&lt;bool&gt; | default           | Fires every time when *Expanded* get changed. Value is equal *Expanded*. |

### Properties

| **Name**             | **Type** | get/set | **Dexcription**                                                            |
| -------------------- | -------- | ------- | -------------------------------------------------------------------------- |
| Expanded             | bool     | get/set | The state of collapsed or expanded for this submenu.                       |
| SilentExpandedSetter | bool     | set     | Sets the state of *Expanded* without notifying *OnToggle* of this submenu. |

### Methods

| **Name** | **Parameters** | **ReturnType** | **Dexcription**                                                       |
| -------- | -------------- | -------------- | --------------------------------------------------------------------- |
| Toggle   | *empty*        | void           | Expands/Collapses this menu. Shorthand for: *Expanded = !Expanded*;   |


<br></br>
## ContextMenuItem

Wrapper for the content that will be a context item.
This should be placed inside of a *ContextMenu* or *ContextSubMenu* instance.

### Parameters

| **Name**     | **Type**                            | **Default Value** | **Dexcription**                                                |
| ------------ | ----------------------------------- | ----------------- | -------------------------------------------------------------- |
| ChildContent | RenderFragment                      | *required*        | Html that will be displayed.                                   |
| OnPressed    | EventCallback&lt;MouseEventArgs&gt; | default           | Invokes every time when this list item get clicked or touched. |
