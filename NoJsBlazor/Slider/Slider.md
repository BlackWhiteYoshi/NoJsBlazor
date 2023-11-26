# Slider

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


## CSS Variables (.slider)

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


## Type Parameters

| **Name** | **TypeConstraints**         |  **Dexcription**                                                     |
| -------- | --------------------------- | -------------------------------------------------------------------- |
| Type     | struct, INumber&lt;Type&gt; | Number-type of the underlying value of the Slider. e.g. int, decimal |


## Parameters

| **Name**           | **Type**                    | **Default Value**             | **Dexcription**                                                                                                                                                                                                  |
| ------------------ | --------------------------- | ----------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Value              | Type                        | null                          | Value of the Slider. Should be used as two-way-binding.                                                                                                                                                          |
| ValueChanged       | EventCallback&lt;Type&gt;   | default                       | Invokes every time Value get changed with: LeftButton, RightButton, Slider or EditField. This or *OnChange* can be used for two-way-binding.                                                                     |
| OnChange           | EventCallback&lt;Type&gt;   | default                       | Invokes every time Value get changed with: LeftButton, RightButton, Slider (only on release) or EditField. This or *ValueChanged* can be used for two-way-binding.                                               |
| Title              | string                      | string.Empty                  | An optional label. Default is string.Empty                                                                                                                                                                       |
| Min                | Type                        | Type.Zero                     | Slider lower bounds. Default is 0.                                                                                                                                                                               |
| Max                | Type                        | Type.One + … + Type.One (×10) | Slider upper bounds. Default is 10.                                                                                                                                                                              |
| Step               | Type                        | Type.One                      | Slider precision. Default is 1.                                                                                                                                                                                  |
| Enabled            | bool                        | true                          | Enables or disables the controls (left-button, right-button, slider-thumb). Default is true.                                                                                                                     |
| Editable           | bool                        | false                         | Indicates if the user is able to edit the number directly. Technically the number is displayed in a input field instead of a label. Dafault is false.                                                            |
| LeftButtonContent  | RenderFragment              | DefaultLeftButton (🡸)        | Content inside the left Button. Default is a svg showing "🡸".                                                                                                                                                   |
| RightButtonContent | RenderFragment              | DefaultRightButton (🡺)       | Content inside the right Button. Default is a svg showing "🡺".                                                                                                                                                  |
| Display            | Funck&lt;Type, string&gt;   | DefaultDisplay                | The way the value should be printed. Default is value.ToString().                                                                                                                                                |
| ParseEdit          | Funck&lt;string?, Type?&gt; | DefaultParseEdit              | It should get the content of the edit field as string and return the appropriated number. It should return null if the value is not valid. Default try parses number and when succeed, clamps to Min,Max-bounds. |

**Note**: 
DefaultLeftButton and DefaultRightButton are private RenderFragments which render "🡸" and "🡺" arrows respectively.  
DefaultDisplay is a private function which performs the ToString()-method on the value and if it returns null, defaults to string.empty.  
DefaultParseEdit performs a TryParse on the value and if it succeed, it clamps to Min,Max-bounds, otherwise null.  
