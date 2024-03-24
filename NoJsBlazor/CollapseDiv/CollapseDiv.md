# CollapseDiv

A container that content can be collapsed.

```razor
<CollapseDiv StartExpanded="true" OnExpandedChanged="(bool collapsed) => { }">
    <Head>
        RenderFragment<CollapseDivArrow />
    </Head>
    <Content>
        RenderFragment
    </Content>
</CollapseDiv>
```

```css
.collapse-div {
    --color: #DDE;
    --height-transition-time: 300ms;
}

.collapse-arrow {
    --arrow-size: 16px;
    --arrow-stroke-color: #000B;
    --arrow-background-color: #FFFB;
}
```


## CSS Variables: .collapse-div

| **Name**                 | **Default Value** |
| ------------------------ | ----------------- |
| --color                  | #DDE              |
| --height-transition-time | 300ms             |

## Parameters

| **Name**          | **Type**                  | **Default Value** | **Description**                                                                                               |
| ----------------- | ------------------------- | ----------------- | ------------------------------------------------------------------------------------------------------------- |
| Head              | RenderFragment?           | null              | Content that is also visible collapsed. If clicked on it, it will expand/collapse.                            |
| Content           | RenderFragment?           | null              | Content that is hidden when collapsed.                                                                        |
| StartExpanded     | bool                      | true              | Initializing collapsed or expanded. Default is false.                                                         |
| OnExpandedChanged | EventCallback&lt;bool&gt; | default           | Fires every time when collapse state is changed. Parameter indicates if the component is currently collapsed. |


## Properties

| **Name**             | **Type** | get/set | **Description**                                                      |
| -------------------- | -------- | ------- | -------------------------------------------------------------------- |
| Expanded             | bool     | get/set | The state of collapsed or expanded.                                  |
| SilentExpandedSetter | bool     | set     | Sets the state of *Collapsed* without notifying *OnCollapseChanged*. |


<br></br>
## CollapseDivArrow

A component containing a svg arrow icon.
Can be placed inside CollapseDiv.Head to get a nice expanded-indicator that rotates on expanding.

### CSS Variables: .collapse-arrow

| **Name**                 | **Default Value** |
| ------------------------ | ----------------- |
| --arrow-size             | 16px              |
| --arrow-stroke-color     | #000B             |
| --arrow-background-color | #FFFB             |
