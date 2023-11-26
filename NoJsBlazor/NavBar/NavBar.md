# NavBar 
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


## CSS Variables (.nav-root)

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


## Parameters

| **Name**   | **Type**                  | **Default Value**     | **Dexcription**                                                                                   |
| ---------- | ------------------------- | --------------------- | ------------------------------------------------------------------------------------------------- |
| Brand      | RenderFragment?           | null                  | Content/Icon which represents your site. If it is null, the corresponding parts are not rendered. |
| Items      | RenderFragment            | *required*            | This should be a list of *NavBarMenu* of *NavBarItem*/*NavBarLink*.                               |
| Breakpoint | NavBarBreakpoint          | NavBarBreakpoint.none | Changes at the given width between phone and desktop view. Default is NavBarBreakpoint.none.      |
| OnToggle   | EventCallback&lt;bool&gt; | default               | Fires every time when *Expanded* get changed. Value is equal *Expanded*.                          |


## Properties

| **Name**             | **Type** | get/set | **Dexcription**                                            |
| -------------------- | -------- | ------- | ---------------------------------------------------------- |
| Expanded             | bool     | get/set | Value for Expanding or Collapsing the navbar.              |
| SilentExpandedSetter | bool     | set     | Sets the state of *Expanded* without notifying *OnToggle*. |


## Methods

| **Name** | **Parameters** | **ReturnType** | **Dexcription**                                                     |
| -------- | -------------- | -------------- | ------------------------------------------------------------------- |
| Toggle   | *empty*        | void           | Expands/Collapses this menu. Shorthand for: *Expanded = !Expanded;* |
| Reset    | *empty*        | void           | Collapses this menu and all expanded submenus.                      |


<br></br>
## NavBarMenu

A single menu that can hold other *NavBarMenu* and *NavBarItem*/*NavBarLink*.
This should be placed inside of a *NavBar* or *NavBarMenu*.

### Parameters

| **Name** | **Type**                  | **Default Value** | **Dexcription**                                                          |
| -------- | ------------------------- | ----------------- | ------------------------------------------------------------------------ |
| Head     | RenderFragment            | *required*        | Content that is collapsed visible.                                       |
| Items    | RenderFragment            | *required*        | Content that is expanded visible.                                        |
| OnToggle | EventCallback&lt;bool&gt; | default           | Fires every time when *Expanded* get changed. Value is equal *Expanded*. |

### Properties

| **Name**             | **Type** | get/set | **Dexcription**                                                            |
| -------------------- | -------- | ------- | -------------------------------------------------------------------------- |
| Expanded             | bool     | get/set | Value for expanding or collapsing this submenu.                            |
| SilentExpandedSetter | bool     | set     | Sets the state of *Expanded* without notifying *OnToggle* of this submenu. |

### Methods

| **Name** | **Parameters** | **ReturnType** | **Dexcription**                                                     |
| -------- | -------------- | -------------- | ------------------------------------------------------------------- |
| Toggle   | *empty*        | void           | Expands/Collapses this menu. Shorthand for: *Expanded = !Expanded;* |


<br></br>
## NavBarLink

An item representing a hyperlink.
This should be placed inside of a *NavBar* or *NavBarMenu*.

### Parameters

| **Name**     | **Type**       | **Default Value** | **Dexcription**              |
| ------------ | -------------- | ----------------- | ---------------------------- |
| ChildContent | RenderFragment | *required*        | Html that will be displayed. |


<br></br>
## NavBarItem

An item that renders the given html.
This should be placed inside of a *NavBar* or *NavBarMenu*.

### Parameters

| **Name**     | **Type**       | **Default Value** | **Dexcription**              |
| ------------ | -------------- | ----------------- | ---------------------------- |
| ChildContent | RenderFragment | *required*        | Html that will be displayed. |


<br></br>
## NavBarBreakpoint

Width values for changing between phone and desktop view.

### Members

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
