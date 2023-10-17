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

- [Carousel](NoJsBlazor/Carousel/Carousel.md)
- [CollapseDiv](NoJsBlazor/CollapseDiv/CollapseDiv.md)
- [ContextMenu](NoJsBlazor/ContextMenu/ContextMenu.md)
- [Dialog](NoJsBlazor/Dialog/Dialog.md)
- [Input](NoJsBlazor/Input/Input.md)
- [LoaderIcon](NoJsBlazor/LoaderIcon/LoaderIcon.md)
- [NavBar](NoJsBlazor/NavBar/NavBar.md)
- [ProgressBar](NoJsBlazor/ProgressBar/ProgressBar.md)
- [Slider](NoJsBlazor/Slider/Slider.md)

**Note**:  
Every component is inside the namespace **NoJsBlazor**  
and supports **CaptureUnmatchedValues**.
