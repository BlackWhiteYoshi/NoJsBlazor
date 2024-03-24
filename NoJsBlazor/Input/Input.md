# Input / EditFormInput

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

```css
.input-field-container {
    --input-width: 12.5em;
    --padding-top-bottom: 0.6em;
    --padding-left-right: 1.2em;
    --label-top-offset-left: 0.5em;
    --background-color: #FFF;
    --border-color: #444;
    --focus-color: #00F;
}
```


## CSS Variables: .input-field-container

| **Name**                | **Default Value** |
| ----------------------- | ----------------- |
| --input-width           | 12.5em            |
| --padding-top-bottom    | 0.6em             |
| --padding-left-right    | 1.2em             |
| --label-top-offset-left | 0.5em             |
| --background-color      | #FFF              |
| --border-color          | #444              |
| --focus-color           | #00F              |


## Parameters

| **Name**        | **Type**                           | **Default Value** | **Description**                                                                                                                                                                    |
| --------------- | ---------------------------------- | ----------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Value           | string?                            | null              | Value of this Input field. Should be used as two-way-binding.                                                                                                                      |
| ValueChanged    | EventCallback&lt;string?&gt;       | default           | Fires every time *Value* changes. This or *OnChange* can be used for two-way-binding.                                                                                              |
| OnChange        | EventCallback&lt;string?&gt;       | default           | Fires every time item looses focus and value has changed. This or *ValueChanged* can be used for two-way-binding.                                                                  |
| Title           | string                             | n/a               | Sets properties Label, Id, Name and Autocomplete to the given string (Autocomplete to true). Returns a comma seperated string with this 4 properties (Label,Id,Name,Autocomplete). |
| Label           | string?                            | null              | Displayed text of this Input field.                                                                                                                                                |
| Type            | string?                            | null              | Sets the type attribute. e.g. normal text(abcd), password(****), numberField(0123), …                                                                                              |
| Id              | string?                            | null              | Sets the "id"-attribute in input field and the "for"-attribute in the label.                                                                                                       |
| Name            | string?                            | null              | Sets the "name"-attribute in the input field.                                                                                                                                      |
| Autocomplete    | bool                               | true              | Sets the "autocomplete"-attribute in the input field. Default is true.                                                                                                             |
| InputAttributes | IDictionary&lt;string, object&gt;? | null              | These values are applied to the input field.                                                                                                                                       |
