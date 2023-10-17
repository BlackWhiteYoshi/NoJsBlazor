# ProgressBar

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
