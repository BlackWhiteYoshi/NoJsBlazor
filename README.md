# NoJsBlazor

Library for Blazor applications. It contains some UI components.  
This Library does not use any Javascript code, everything is implemented in Razor, (S)CSS and C#.

For a demo go to [nojsblazor.firerocket.de](https://nojsblazor.firerocket.de).



<br></br>
## Get Started

1. Add PackageReference to your .csproj file

```xml
<ItemGroup>
  <PackageReference Include="NoJsBlazor" Version="x.x.x" />
</ItemGroup>
```

2. Add stylesheet to your html

```html
<link rel="stylesheet" href="_content/NoJsBlazor/style.min.css" />
```



<br></br>
## List of components

Everything is inside the namespace *NoJsBlazor*

@using NoJsBlazor

- Carousel
- CollapseDiv
- ContextMenu
- Dialog
- Input
- LoaderIcon
- NavBar
- ProgressBar
- Slider


<br></br>
**Note**: Every component supports **CaptureUnmatchedValues**.



<br></br>
## Carousel

A Carousel like in Bootstrap. It can also display an overlay, control-arrows, item indicators and a play/stop button.

```razor
<Carousel
        ActiveStart="0"
        Animation="CarouselAnimation.FadeOut"
        IntervalTime="6000.0"
        AutoStartTime="0.0"
        BeginRunning="true"
        ControlArrowsEnable="true"
        IndicatorsEnable="true"
        PlayButtonEnable="true"
        OnActiveChanged="(int index) => { }"
        OnRunningChanged="(bool running) => { }">
    <Items>
        <CarouselItem>image1</CarouselItem>
        <CarouselItem>image2</CarouselItem>
        <CarouselItem>image3</CarouselItem>
    </Items>
    <Overlay>
        RenderFragment
    </Overlay>
</Carousel>
```


### CSS Variables

| **Name**                   | **Default Value**              |
| -------------------------- | ------------------------------ |
| --swap-duration            | 500ms                          |
| --overlay-position         | auto auto 20% 20%              |
| --arrow-left-position      | 0 auto 0 5%                    |
| --arrow-right-position     | 0 5% 0 auto                    |
| --arrow-size               | 6%                             |
| --arrow-color              | #FFF                           |
| --indicator-list-position  | auto 0 1em 0                   |
| --indicator-list-width     | 60%                            |
| --indicator-gap            | 5px                            |
| --indicator-color          | #FFF                           |
| --indicator-progress-color | #0AA                           |
| --play-button-position     | auto auto calc(1em + 25px) 15% |
| --play-button-size         | 6%                             |
| --play-button-color        | #FFF                           |


### Parameters

| **Name**            | **Type**                  | **Default Value**         | **Dexcription**                                                                                                           |
| ------------------- | ------------------------- | ------------------------- | ------------------------------------------------------------------------------------------------------------------------- |
| Items               | RenderFragment            | *required*                | Content of the *Carousel*. This should be a list of *CarouselItem* objects.                                               |
| Overlay             | RenderFragment?           | null                      | Html that will be rendered in the overlay section.                                                                        |
| ActiveStart         | int                       | 0                         | Index of the active item at the beginning.                                                                                |
| Animation           | CarouselAnimation         | CarouselAnimation.FadeOut | Type of swapping animation: (FadeOut, Slide, SlideRotate)                                                                 |
| IntervalTime        | double                    | 6000.0                    | Waiting time before beginning swap animation in ms.                                                                       |
| AutoStartTime       | double                    | 0.0                       | Starts interval after [AutoStartTime] ms, if interval not running and no action occurs. Value of 0 deactivates autostart. |
| BeginRunning        | bool                      | true                      | Carousel Interval starts running or paused.                                                                               |
| ControlArrowsEnable | bool                      | true                      | Next/Previous Arrows available                                                                                            |
| IndicatorsEnable    | bool                      | true                      | Indicators available                                                                                                      |
| PlayButtonEnable    | bool                      | true                      | PlayButton available                                                                                                      |
| OnActiveChanged     | EventCallback&lt;int&gt;  | default                   | Fires every time after *active* item changed. Parameter is index of the new active item.                                  |
| OnRunningChanged    | EventCallback&lt;bool&gt; | default                   | Fires every time after the *Running* state is set. Parameter indicates if the carousel is currently running.              |


### Properties

| **Name**         | **Type** | get/set | **Dexcription**                                                 |
| ---------------- | -------- | ------- | --------------------------------------------------------------- |
| Active           | int      | get/set | The Index of the current Active Item.                           |
| Running          | bool     | get     | Indicates if the interval of the carousel is currently running. |
| AutoStartRunning | bool     | get     | Inidicates if the autoStart Timer is currently ticking.         |


### Methods

| **Name**          | **Parameters**                                         | **ReturnType** | **Dexcription**                                                                                                                                                                              |
| ----------------- | ------------------------------------------------------ | -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| SwapCarouselItems | CarouselItem carouselItem1, CarouselItem carouselItem2 | void           | Finds the indexes of the 2 given CarouselItems and then executes *SwapCarouselItems(int, int)*. If one of the items is not present in the CarouselItem list, an ArgumentException is thrown. |
| SwapCarouselItems | int index1, int index2                                 | void           | Swaps the 2 items in the CarouselItem list at the given indexes. The active item is preserved: e.g. 0 is active, swap(0, 1)  -> 1 will be active, so the active item won't change.           |
| StartInterval     | *empty*                                                | void           | Starts the interval-timer that automatically change to next item.                                                                                                                            |
| StopInterval      | *empty*                                                | void           | Stops the interval-timer, so the current item will not automatically change.                                                                                                                 |
| Dispose           | *empty*                                                | void           | Disposes Interval Timer and Autostart Timer.                                                                                                                                                 |


### Sub Types

#### CarouselItem

A component, that should only be used in the *Carousel.Items* Renderfragment.

##### Parameters

| **Name**     | **Type**       | **Default Value** | **Dexcription**                                       |
| ------------ | -------------- | ----------------- | ----------------------------------------------------- |
| ChildContent | RenderFragment | *required*        | Wrapper for the content that will be a carousel item. |

#### CarouselAnimation

Type of animation, how the items are swapped.

##### Members

| **Name**    | **Dexcription**                                  |
| ----------- | ------------------------------------------------ |
| FadeOut     | Transition on opacity                            |
| Slide       | Transition on translate x-axis                   |
| SlideRotate | Transition on translate x-axis and rotate y-axis |



<br></br>
## CollapseDiv

A container that content can be collapsed.

```razor
<CollapseDiv StartCollapsed="true" OnCollapseChanged="(bool collapsed) => { }">
    <Head>
        RenderFragment<CollapseDivArrow />
    </Head>
    <Content>
        RenderFragment
    </Content>
</CollapseDiv>
```


### CSS Variables

| **Name**                 | **Default Value** |
| ------------------------ | ----------------- |
| --color                  | #DDE              |
| --height-transition-time | 300ms             |

**Note**: The outer div changes it's css-class depending on expanding state: *collapsed* or *collapseable*.  

### Parameters

| **Name**          | **Type**                  | **Default Value** | **Dexcription**                                                                                               |
| ----------------- | ------------------------- | ----------------- | ------------------------------------------------------------------------------------------------------------- |
| Head              | RenderFragment?           | null              | Content that is also visible collapsed. If clicked on it, it will expand/collapse.                            |
| Content           | RenderFragment?           | null              | Content that is hidden when collapsed.                                                                        |
| StartCollapsed    | bool                      | true              | Initializing collapsed or expanded.                                                                           |
| OnCollapseChanged | EventCallback&lt;bool&gt; | default           | Fires every time when collapse state is changed. Parameter indicates if the component is currently collapsed. |


### Properties

| **Name**              | **Type** | get/set | **Dexcription**                                                      |
| --------------------- | -------- | ------- | -------------------------------------------------------------------- |
| Collapsed             | bool     | get/set | The state of collapsed or expanded.                                  |
| SilentCollapsedSetter | bool     | set     | Sets the state of *Collapsed* without notifying *OnCollapseChanged*. |


### Sub Types

#### CollapseDivArrow

A component containing a svg arrow icon.
Can be placed inside CollapseDiv.Head to get a nice expanded-indicator that rotates on expanding.

##### CSS Variables

| **Name**                 | **Default Value** |
| ------------------------ | ----------------- |
| --arrow-size             | 16px              |
| --arrow-stroke-color     | #000B             |
| --arrow-background-color | #FFFB             |



<br></br>
## ContextMenu

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


### CSS Variables

| **Name**                           | **Default Value**                                                                                    |
| ---------------------------------- | ---------------------------------------------------------------------------------------------------- |
| --background-image                 | linear-gradient(90deg, rgba(200, 200, 255, 0.8), rgba(230, 230, 255, 0.8), rgba(200, 200, 255, 0.8)) |
| --arrow-size                       | 10px                                                                                                 |
| --arrow-color                      | #000B                                                                                                |
| --arrow-hover-color                | #FFFB                                                                                                |
| --arrow-hover-stroke               | #000                                                                                                 |
| --hover-dropright-background-color | #FFF6                                                                                                |
| --hover-dropright-shadow-color     | #FFF                                                                                                 |
| --hover-item-background-color      | #FFF8                                                                                                |


### Parameters

| **Name**     | **Type**                  | **Default Value** | **Dexcription**                                                          |
| ------------ | ------------------------- | ----------------- | ------------------------------------------------------------------------ |
| ChildContent | RenderFragment            | *required*        | Content of this ContextMenu.                                             |
| OnToggle     | EventCallback&lt;bool&gt; | default           | Fires every time when *Expanded* get changed. Value is equal *Expanded*. |


### Properties

| **Name** | **Type** | get/set | **Dexcription**                    |
| -------- | -------- | ------- | ---------------------------------- |
| Expanded | bool     | get     | The state of collapsed or visible. |


### Methods

| **Name**    | **Parameters**     | **ReturnType** | **Dexcription**                                                                           |
| ----------- | ------------------ | -------------- | ----------------------------------------------------------------------------------------- |
| Open        | double x, double y | void           | Display the context menu at the given location.                                           |
| Close       | *empty*            | void           | Collapses all expanded submenus and closes the context menu.                              |
| SilentOpen  | double x, double y | void           | Display the context menu at the given location without notifying *OnToggle*.              |
| SilentClose | *empty*            | void           | Collapses all expanded submenus and closes the context menu without notifying *OnToggle*. |
| Reset       | *empty*            | void           | Collapses all expanded submenus.                                                          |


### Sub Types

#### ContextSubMenu

Another *ContextMenu* inside of a *ContextMenu* or *ContextSubMenu*.

##### Parameters

| **Name** | **Type**                  | **Default Value** | **Dexcription**                                                          |
| -------- | ------------------------- | ----------------- | ------------------------------------------------------------------------ |
| Head     | RenderFragment            | *required*        | Content that is collapsed visible.                                       |
| Items    | RenderFragment            | *required*        | Content that is expanded visible.                                        |
| OnToggle | EventCallback&lt;bool&gt; | default           | Fires every time when *Expanded* get changed. Value is equal *Expanded*. |

##### Properties

| **Name**             | **Type** | get/set | **Dexcription**                                                            |
| -------------------- | -------- | ------- | -------------------------------------------------------------------------- |
| Expanded             | bool     | get/set | The state of collapsed or expanded for this submenu.                       |
| SilentExpandedSetter | bool     | set     | Sets the state of *Expanded* without notifying *OnToggle* of this submenu. |

##### Methods

| **Name** | **Parameters** | **ReturnType** | **Dexcription**                                                       |
| -------- | -------------- | -------------- | --------------------------------------------------------------------- |
| Toggle   | *empty*        | void           | Expands/Collapses this menu. Shorthand for: *Expanded = !Expanded*;   |

#### ContextMenuItem

Wrapper for the content that will be a context item.
This should be placed inside of a *ContextMenu* or *ContextSubMenu* instance.

##### Parameters

| **Name**     | **Type**                            | **Default Value** | **Dexcription**                                                |
| ------------ | ----------------------------------- | ----------------- | -------------------------------------------------------------- |
| ChildContent | RenderFragment                      | *required*        | Html that will be displayed.                                   |
| OnPressed    | EventCallback&lt;MouseEventArgs&gt; | default           | Invokes every time when this list item get clicked or touched. |



<br></br>
## Dialog

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


### CSS Variables

| **Name**                        | **Default Value**        |
| ------------------------------- | ------------------------ |
| --modal-screen-background-color | rgba(255, 255, 255, 0.8) |
| --window-border-radius          | 20px                     |
| --title-background-color        | #FFF                     |
| --content-background-color      | #EEE                     |


### Parameters

| **Name**               | **Type**                              | **Default Value** | **Dexcription**                                                                                                                 |
| ---------------------- | ------------------------------------- | ----------------- | ------------------------------------------------------------------------------------------------------------------------------- |
| Title                  | RenderFragment?                       | null              | Html that will be displayed inside the head bar.                                                                                |
| Content                | RenderFragment?                       | null              | Html that will be displayed inside the window.                                                                                  |
| ShowTitle              | bool                                  | true              | Indicates whether to skip the *Title* section.                                                                                  |
| Moveable               | bool                                  | true              | The window can be grabed and draged around the screen.                                                                          |
| ModalScreen            | bool                                  | true              | If the background should be blurred/unavailable.                                                                                |
| CloseOnModalBackground | bool                                  | true              | If on the *ModalBackground* a click occurs, whether the window should close or not close.                                       |
| OnActiveChanged        | EventCallback&lt;bool&gt;             | default           | Invokes every time when the Dialog opens or closes. true: dialog got from close to open, false: dialog got from open to closed. |
| OnTitlePointerDown     | EventCallback&lt;PointerEventArgs&gt; | default           | Fires if pointer starts on title div.                                                                                           |
| OnTitlePointerMove     | EventCallback&lt;PointerEventArgs&gt; | default           | Fires if pointer moves on title div, but only if pointer down is active.                                                        |
| OnTitlePointerUp       | EventCallback&lt;PointerEventArgs&gt; | default           | Fires if pointer releases on title div.                                                                                         |


### Properties

| **Name**           | **Type**         | get/set | **Dexcription**                                                                          |
| ------------------ | ---------------- | ------- | ---------------------------------------------------------------------------------------- |
| XMovement          | double           | get/set | Current offset of x-coordinate, 0 means middle of the screen.                            |
| YMovement          | double           | get/set | Current offset of y-coordinate, 0 means middle of the screen.                            |
| Active             | bool             | get/set | true -> window is shown, false -> window is closed                                       |
| SilentActiveSetter | bool             | set     | Sets the state of *Active* without notifying *OnActiveChanged*.                          |
| TitleDiv           | ElementReference | get     | The Container Element of the title head bar. You can use this to register mouse capture. |


### Methods

| **Name**             | **Parameters** | **ReturnType** | **Dexcription**                                                                        |
| -------------------- | -------------- | -------------- | -------------------------------------------------------------------------------------- |
| Open                 | *empty*        | void           | Display the window. The same as setting *XMovement/YMovement = 0* and *Active = true*. |
| OpenWithLastPosition | *empty*        | void           | Display the window at the last moved position. The same as *Active = true*.            |
| Close                | *empty*        | void           | Closes the window. The same as *Active = false*.                                       |



<br></br>
## Input / EditFormInput

Input field with a embedded label.  
*Input* should be used without *EditForm*, *EditFormInput* should be used inside *EditForm*.

```razor
<Input
        @bind-Value="MyString"
        @bind-Value:event="ValueChanged"
        Title="null"
        Label="null"
        Type="null"
        Id="null"
        Name="null"
        Autocomplete="true"
        InputAttributes="null" />
```


### CSS Variables

| **Name**                | **Default Value** |
| ----------------------- | ----------------- |
| --input-width           | 12.5em            |
| --padding-top-bottom    | 0.6em             |
| --padding-left-right    | 1.2em             |
| --label-top-offset-left | 0.5em             |
| --background-color      | #FFF              |
| --border-color          | #444              |
| --focus-color           | #00F              |


### Parameters

| **Name**        | **Type**                           | **Default Value** | **Dexcription**                                                                                                                                                                    |
| --------------- | ---------------------------------- | ----------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Value           | string?                            | null              | Value of this Input field. Should be used as two-way-binding.                                                                                                                      |
| ValueChanged    | EventCallback&lt;string?&gt;       | default           | Fires every time *Value* changes. This or *OnChange* can be used for two-way-binding.                                                                                              |
| OnChange        | EventCallback&lt;string?&gt;       | default           | Fires every time item looses focus and value has changed. This or *ValueChanged* can be used for two-way-binding.                                                                  |
| Title           | string                             | n/a               | Sets properties Label, Id, Name and Autocomplete to the given string (Autocomplete to true). Returns a comma seperated string with this 4 properties (Label,Id,Name,Autocomplete). |
| Label           | string?                            | null              | Displayed text of this Input field.                                                                                                                                                |
| Type            | string?                            | null              | Sets the type attribute.                                                                                                                                                           |
| Id              | string?                            | null              | Sets the "id"-attribute in input field and the "for"-attribute in the label.                                                                                                       |
| Name            | string?                            | null              | Sets the "name"-attribute in the input field.                                                                                                                                      |
| Autocomplete    | bool                               | true              | Sets the "autocomplete"-attribute in the input field.                                                                                                                              |
| InputAttributes | IDictionary&lt;string, object&gt;? | null              | These values are applied to the input field.                                                                                                                                       |




<br></br>
## LoaderIcon

A funny animation to pass the waiting time.

```razor
<LoaderIcon />
```


### CSS Variables

| **Name**                          | **Default Value**     |
| --------------------------------- | --------------------- |
| --size                            | 20px                  |
| --move-type                       | move-type-1           |
| --move-size                       | calc(var(--size) / 2) |
| --move-duration                   | 3s                    |
| --move-timing-function            | ease-in-out           |
| --spin-type                       | spin-type-1           |
| --spin-duration                   | 2s                    |
| --spin-timing-function            | linear                |
| --spin-direction                  | normal                |
| --color1                          | #D00                  |
| --color2                          | #00D                  |
| --color3                          | #DD0                  |
| --color4                          | #0D0                  |
| --color-duration                  | 2s                    |
| --color-timing-function           | ease-in-out           |
| --top-left-animation-type         | ball-action-square    |
| --top-left-animation-duration     | 8s                    |
| --top-right-animation-type        | ball-action-circle    |
| --top-right-animation-duration    | 6s                    |
| --bottom-left-animation-type      | ball-action-line      |
| --bottom-left-animation-duration  | 4s                    |
| --bottom-right-animation-type     | ball-action-big       |
| --bottom-right-animation-duration | 10s                   |



<br></br>
## NavBar 
A container that contains a nested list of items/links. It automatically collapses to phone view at a configurable threshold. Optional it can also show a brand.

```razor
<NavBar Breakpoint="NavBarBreakpoint.none" OnToggle="(bool expanded) => { }">
    <Brand>
        RenderFragment
    </Brand>
    <Items>
        <NavBarMenu OnToggle="(bool expanded) => { }">
            <Head>
                RenderFragment
            </Head>
            <Items>
                <NavBarItem>RenderFragment</NavBarItem>
            </Items>
        </NavBarMenu>
        <NavBarLink href="">RenderFragment</NavBarLink>
    </Items>
</NavBar>
```


### CSS Variables

| **Name**                             | **Default Value**                                                                                    |
| ------------------------------------ | ---------------------------------------------------------------------------------------------------- |
| --phone-menu-max-width               | 330px                                                                                                |
| --phone-menu-max-height              | calc(100vh - 100px)                                                                                  |
| --phone-nav-dropdown-transition-time | 300ms                                                                                                |
| --background-image                   | linear-gradient(90deg, rgba(200, 200, 255, 0.8), rgba(230, 230, 255, 0.8), rgba(200, 200, 255, 0.8)) |
| --font-color                         | #000                                                                                                 |
| --desktop-link-padding               | 13px                                                                                                 |
| --vertical-link-padding              | 8px                                                                                                  |
| --phone-button-size                  | 65%                                                                                                  |
| --phone-button-color                 | #000B                                                                                                |
| --phone-hover-shadow                 | 0 0 10px #88F                                                                                        |
| --arrow-down-size                    | 12px                                                                                                 |
| --arrow-right-size                   | 10px                                                                                                 |
| --arrow-desktop-color                | #000B                                                                                                |
| --arrow-desktop-hover-color          | #FFFB                                                                                                |
| --arrow-desktop-hover-stroke         | #000                                                                                                 |
| --arrow-phone-size                   | 35px                                                                                                 |
| --arrow-phone-color                  | #000B                                                                                                |
| --arrow-phone-color-hover            | #FFFB                                                                                                |


### Parameters

| **Name**   | **Type**                  | **Default Value**     | **Dexcription**                                                                                   |
| ---------- | ------------------------- | --------------------- | ------------------------------------------------------------------------------------------------- |
| Brand      | RenderFragment?           | null                  | Content/Icon which represents your site. If it is null, the corresponding parts are not rendered. |
| Items      | RenderFragment            | *required*            | This should be a list of *NavBarMenu* of *NavBarItem*/*NavBarLink*.                               |
| Breakpoint | NavBarBreakpoint          | NavBarBreakpoint.none | Changes at the given width between phone and desktop view.                                        |
| OnToggle   | EventCallback&lt;bool&gt; | default               | Fires every time when *Expanded* get changed. Value is equal *Expanded*.                          |


### Properties

| **Name**             | **Type** | get/set | **Dexcription**                                            |
| -------------------- | -------- | ------- | ---------------------------------------------------------- |
| Expanded             | bool     | get/set | Value for Expanding or Collapsing the navbar.              |
| SilentExpandedSetter | bool     | set     | Sets the state of *Expanded* without notifying *OnToggle*. |


### Methods

| **Name** | **Parameters** | **ReturnType** | **Dexcription**                                |
| -------- | -------------- | -------------- | ---------------------------------------------- |
| Reset    | *empty*        | void           | Collapses this menu and all expanded submenus. |


### Sub Types

#### NavBarMenu

A single menu that can hold other *NavBarMenu* and *NavBarItem*/*NavBarLink*.
This should be placed inside of a *NavBar* or *NavBarMenu*.

##### Parameters

| **Name** | **Type**                  | **Default Value** | **Dexcription**                                                          |
| -------- | ------------------------- | ----------------- | ------------------------------------------------------------------------ |
| Head     | RenderFragment            | *required*        | Content that is collapsed visible.                                       |
| Items    | RenderFragment            | *required*        | Content that is expanded visible.                                        |
| OnToggle | EventCallback&lt;bool&gt; | default           | Fires every time when *Expanded* get changed. Value is equal *Expanded*. |

##### Properties

| **Name**             | **Type** | get/set | **Dexcription**                                                            |
| -------------------- | -------- | ------- | -------------------------------------------------------------------------- |
| Expanded             | bool     | get/set | Value for expanding or collapsing this submenu.                            |
| SilentExpandedSetter | bool     | set     | Sets the state of *Expanded* without notifying *OnToggle* of this submenu. |

##### Methods

| **Name** | **Parameters** | **ReturnType** | **Dexcription**                                                       |
| -------- | -------------- | -------------- | --------------------------------------------------------------------- |
| Toggle | *empty* | void | Expands/Collapses this menu. Shorthand for: *Expanded = !Expanded;*                      |

#### NavBarLink

An item representing a hyperlink.
This should be placed inside of a *NavBar* or *NavBarMenu*.

##### Parameters

| **Name**     | **Type**       | **Default Value** | **Dexcription**              |
| ------------ | -------------- | ----------------- | ---------------------------- |
| ChildContent | RenderFragment | *required*        | Html that will be displayed. |


#### NavBarItem

An item that renders the given html.
This should be placed inside of a *NavBar* or *NavBarMenu*.

##### Parameters

| **Name**     | **Type**       | **Default Value** | **Dexcription**              |
| ------------ | -------------- | ----------------- | ---------------------------- |
| ChildContent | RenderFragment | *required*        | Html that will be displayed. |

#### NavBarBreakpoint

Width values for changing between phone and desktop view.

##### Members

| **Name** | **Dexcription**                              |
| -------- | -------------------------------------------- |
| none     | Changes never, stays always at desktop view. |
| px500    | Below width of 500px is phone view.          |
| px600    | Below width of 600px is phone view.          |
| px700    | Below width of 700px is phone view.          |
| px800    | Below width of 800px is phone view.          |
| px900    | Below width of 900px is phone view.          |
| px1000   | Below width of 1000px is phone view.         |
| px1100   | Below width of 1100px is phone view.         |
| px1200   | Below width of 1200px is phone view.         |
| px1300   | Below width of 1300px is phone view.         |
| px1400   | Below width of 1400px is phone view.         |
| px1500   | Below width of 1500px is phone view.         |
| em30     | Below width of em30 is phone view.           |
| em40     | Below width of em40 is phone view.           |
| em50     | Below width of em50 is phone view.           |
| em60     | Below width of em60 is phone view.           |
| em70     | Below width of em70 is phone view.           |
| em80     | Below width of em80 is phone view.           |
| em90     | Below width of em90 is phone view.           |
| em100    | Below width of em100 is phone view.          |



<br></br>
## StandardProgressBar

An animation that shows a bar growing from left to right.

```razor
<StandardProgressBar Progress="0.0f" Text="@string.Empty" />
```


### CSS Variables

| **Name**               | **Default Value** |
| ---------------------- | ----------------- |
| --width                | 300px             |
| --height               | 20px              |
| --bar-color            | #08F              |
| --border-color         | #444              |
| --border-radius        | 20px              |
| --bar-margin           | 0.125em           |
| --bar-transition-speed | 300ms             |


### Parameters

| **Name** | **Type** | **Default Value** | **Dexcription**                                                       |
| -------- | -------- | ----------------- | --------------------------------------------------------------------- |
| Progress | float    | 0.0f              | The relative amount of the progress Bar. 0 means empty, 1 means full. |
| Text     | string   | string.Empty      | Displaying text of this component.                                    |


### Properties

| **Name** | **Type**                 | get/set | **Dexcription**                                                         |
| -------- | ------------------------ | ------- | ----------------------------------------------------------------------- |
| Content  | (float bar, string text) | get/set | Accessing *Progress* and *Text* together. Also rerenders the component. |



<br></br>
## CircleProgressBar

An animation that shows a circle filling clockwise starting at top.

```razor
<CircleProgressBar Progress="0.0f" Text="@string.Empty" />
```


### CSS Variables

| **Name**        | **Default Value** |
| --------------- | ------------------|
| --diameter      | 200px             |
| --circle-margin | 4                 |
| --circle-color  | #04A              |
| --border-color  | #444              |


### Parameters

| **Name** | **Type** | **Default Value** | **Dexcription**                                                       |
| -------- | -------- | ----------------- | --------------------------------------------------------------------- |
| Progress | float    | 0.0f              | The relative amount of the progress Bar. 0 means empty, 1 means full. |
| Text     | string   | string.Empty      | Displaying text of this component.                                    |


### Properties

| **Name** | **Type**                 | get/set | **Dexcription**                                                         |
| -------- | ------------------------ | ------- | ----------------------------------------------------------------------- |
| Content  | (float bar, string text) | get/set | Accessing *Progress* and *Text* together. Also rerenders the component. |



<br></br>
## SpeedometerProgressBar

An animation that shows speedometer where the hand is increasing with progress.

```razor
<SpeedometerProgressBar Progress="0.0f" Text="@string.Empty" />
```


### CSS Variables

| **Name**                 | **Default Value** |
| ------------------------ | ----------------- |
| --diameter               | 200px             |
| --border-color           | #444              |
| --middle-circle-color    | #0006             |
| --small-tick-width       | 1                 |
| --small-tick-color       | #333              |
| --text-color             | #222              |
| --big-tick-width         | 4                 |
| --big-tick-color         | #222              |
| --meter-length           | 87.5              |
| --meter-width            | 5                 |
| --meter-color            | #D22              |
| --meter-transition-speed | 500ms             |


### Parameters

| **Name** | **Type** | **Default Value** | **Dexcription**                                                       |
| -------- | -------- | ----------------- | --------------------------------------------------------------------- |
| Progress | float    | 0.0f              | The relative amount of the progress Bar. 0 means empty, 1 means full. |
| Text     | string   | string.Empty      | Displaying text of this component.                                    |


### Properties

| **Name** | **Type**                 | get/set | **Dexcription**                                                         |
| -------- | ------------------------ | ------- | ----------------------------------------------------------------------- |
| Content  | (float bar, string text) | get/set | Accessing *Progress* and *Text* together. Also rerenders the component. |



<br></br>
## Slider

An object that holds a ranged input together with a decrease and increase button and a label indicating the current value.

```razor
<Slider Type="int"
        @bind-Value="MyNumber"
        @bind-Value:event="ValueChanged"
        Title="@string.Empty"
        Min="0"
        Max="10"
        Step="1"
        Enabled="true"
        Editable="false"
        Display="(int value) => value.ToString() ?? string.Empty"
        ParseEdit="(string? input) => int.TryParse(input, CultureInfo.CurrentCulture, out int result) ? Math.Clamp(result, 0, 10) : null">
    <LeftButtonContent>
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 10 10">
            <line x1="2" y1="5" x2="8" y2="5" />
            <line x1="2" y1="5" x2="4" y2="3" />
            <line x1="2" y1="5" x2="4" y2="7" />
        </svg>
    </LeftButtonContent>
    <RightButtonContent>
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 10 10">
            <line x1="2" y1="5" x2="8" y2="5" />
            <line x1="6" y1="3" x2="8" y2="5" />
            <line x1="6" y1="7" x2="8" y2="5" />
        </svg>
    </RightButtonContent>
</Slider>
```


### CSS Variables

| **Name**                        | **Default Value**            |
| ------------------------------- | ---------------------------- |
| --button-background-color       | #0D6EFD                      |
| --button-border-color           | #EEE                         |
| --button-stroke-color           | #FFF                         |
| --button-background-color-hover | #0B5ED7                      |
| --button-border-color-hover     | #0A58CA                      |
| --button-box-shadow-color-hover | #0A58CA                      |
| --button-disabled-color         | #8AF                         |
| --slider-track-color            | #5FF                         |
| --slider-track-height           | 6px                          |
| --slider-track-boder-radius     | 3px                          |
| --slider-thumb-color            | #07F                         |
| --slider-thumb-width            | 16px                         |
| --slider-thumb-height           | 16px                         |
| --slider-thumb-disabled-color   | var(--button-disabled-color) |


### Type Parameters

| **Name** | **TypeConstraints**         |  **Dexcription**                            |
| -------- | --------------------------- | ------------------------------------------- |
| Type     | struct, INumber&lt;Type&gt; | Type of the underlying value of the Slider. |


### Parameters

| **Name**           | **Type**                    | **Default Value**             | **Dexcription**                                                                                                                                                                                                  |
| ------------------ | --------------------------- | ----------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Value              | Type                        | null                          | Value of the Slider. Should be used as two-way-binding.                                                                                                                                                          |
| ValueChanged       | EventCallback&lt;Type&gt;   | default                       | Invokes every time Value get changed with: LeftButton, RightButton, Slider or EditField. This or *OnChange* can be used for two-way-binding.                                                                     |
| OnChange           | EventCallback&lt;Type&gt;   | default                       | Invokes every time Value get changed with: LeftButton, RightButton, Slider (only on release) or EditField. This or *ValueChanged* can be used for two-way-binding.                                               |
| Title              | string                      | string.Empty                  | An optional label.                                                                                                                                                                                               |
| Min                | Type                        | Type.Zero                     | Slider lower bounds                                                                                                                                                                                              |
| Max                | Type                        | Type.One + … + Type.One (×10) | Slider upper bounds                                                                                                                                                                                              |
| Step               | Type                        | Type.One                      | Slider precision                                                                                                                                                                                                 |
| Enabled            | bool                        | true                          | Enables or disables the controls (left-button, right-button, slider-thumb).
| Editable           | bool                        | false                         | Indicates if the user is able to edit the number directly. Technically the number is displayed in a input field instead of a label.                                                                              |
| LeftButtonContent  | RenderFragment              | DefaultLeftButton (🡸)        | Content inside the left Button.                                                                                                                                                                                  |
| RightButtonContent | RenderFragment              | DefaultRightButton (🡺)       | Content inside the right Button.                                                                                                                                                                                 |
| Display            | Funck&lt;Type, string&gt;   | DefaultDisplay                | The way the value should be printed.                                                                                                                                                                             |
| ParseEdit          | Funck&lt;string?, Type?&gt; | DefaultParseEdit              | It should get the content of the edit field as string and return the appropriated number. It should return null if the value is not valid. Default try parses number and when succeed, clamps to Min,Max-bounds. |

**Note**: 
DefaultLeftButton and DefaultRightButton are private RenderFragments which render "🡸" and "🡺" arrows respectively.  
DefaultDisplay is a private function which performs the ToString()-method on the value and if it returns null, defaults to string.empty.  
DefaultParseEdit performs a TryParse on the value and if it succeed, it clamps to Min,Max-bounds, otherwise null.  
