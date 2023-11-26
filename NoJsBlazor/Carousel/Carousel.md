# Carousel

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


## CSS Variables (.carousel)

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


## Parameters

| **Name**            | **Type**                  | **Default Value**         | **Dexcription**                                                                                                                         |
| ------------------- | ------------------------- | ------------------------- | --------------------------------------------------------------------------------------------------------------------------------------- |
| Items               | RenderFragment            | *required*                | Content of the *Carousel*. This should be a list of *CarouselItem* objects.                                                             |
| Overlay             | RenderFragment?           | null                      | Html that will be rendered in the overlay section.                                                                                      |
| ActiveStart         | int                       | 0                         | Index of the active item at the beginning. Default is 0.                                                                                |
| Animation           | CarouselAnimation         | CarouselAnimation.FadeOut | Type of swapping animation: (FadeOut, Slide, SlideRotate). Default is FadeOut.                                                          |
| IntervalTime        | double                    | 6000.0                    | Waiting time before beginning swap animation in ms. Default is 6000.                                                                    |
| AutoStartTime       | double                    | 0.0                       | Starts interval after [AutoStartTime] ms, if interval not running and no action occurs. Value of 0 deactivates autostart. Default is 0. |
| BeginRunning        | bool                      | true                      | Carousel Interval starts running or paused. Default is true.                                                                            |
| ControlArrowsEnable | bool                      | true                      | Next/Previous Arrows available. Default is true.                                                                                        |
| IndicatorsEnable    | bool                      | true                      | Indicators available. Default is true.                                                                                                  |
| PlayButtonEnable    | bool                      | true                      | PlayButton available. Default is true.                                                                                                  |
| OnActiveChanged     | EventCallback&lt;int&gt;  | default                   | Fires every time after *active* item changed. Parameter is index of the new active item.                                                |
| OnRunningChanged    | EventCallback&lt;bool&gt; | default                   | Fires every time after the *Running* state is set. Parameter indicates if the carousel is currently running.                            |


## Properties

| **Name**         | **Type** | get/set | **Dexcription**                                                 |
| ---------------- | -------- | ------- | --------------------------------------------------------------- |
| Active           | int      | get/set | The Index of the current Active Item.                           |
| Running          | bool     | get     | Indicates if the interval of the carousel is currently running. |
| AutoStartRunning | bool     | get     | Inidicates if the autoStart Timer is currently ticking.         |


## Methods

| **Name**          | **Parameters**                                         | **ReturnType** | **Dexcription**                                                                                                                                                                              |
| ----------------- | ------------------------------------------------------ | -------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| SwapCarouselItems | CarouselItem carouselItem1, CarouselItem carouselItem2 | void           | Finds the indexes of the 2 given CarouselItems and then executes *SwapCarouselItems(int, int)*. If one of the items is not present in the CarouselItem list, an ArgumentException is thrown. |
| SwapCarouselItems | int index1, int index2                                 | void           | Swaps the 2 items in the CarouselItem list at the given indexes. The active item is preserved: e.g. 0 is active, swap(0, 1)  -> 1 will be active, so the active item won't change.           |
| StartInterval     | *empty*                                                | void           | Starts the interval-timer that automatically change to next item.                                                                                                                            |
| StopInterval      | *empty*                                                | void           | Stops the interval-timer, so the current item will not automatically change.                                                                                                                 |
| Dispose           | *empty*                                                | void           | Disposes Interval Timer and Autostart Timer.                                                                                                                                                 |


<br></br>
## CarouselItem

A component, that should only be used in the *Carousel.Items* Renderfragment.

### Parameters

| **Name**     | **Type**       | **Default Value** | **Dexcription**                                       |
| ------------ | -------------- | ----------------- | ----------------------------------------------------- |
| ChildContent | RenderFragment | *required*        | Wrapper for the content that will be a carousel item. |


<br></br>
## CarouselAnimation (Enum)

Type of animation, how the items are swapped.

### Members

| **Name**    | **Dexcription**                                  |
| ----------- | ------------------------------------------------ |
| FadeOut     | Transition on opacity                            |
| Slide       | Transition on translate x-axis                   |
| SlideRotate | Transition on translate x-axis and rotate y-axis |
