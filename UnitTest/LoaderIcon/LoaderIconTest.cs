using AngleSharp.Dom;

namespace UnitTest;

public class LoaderIconTest : TestContext {
    #region parameter

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public void Size_Sets_Width_And_Height(int size) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.Size, size);
        });

        IElement loaderIconDiv = loaderIconContainer.Find(".loader-icon");
        IAttr style = loaderIconDiv.Attributes["style"]!;
        Assert.Contains($"width: {size}px", style.Value);
        Assert.Contains($"height: {size}px", style.Value);
    }

    [Theory]
    [InlineData(0.0f)]
    [InlineData(0.5f)]
    [InlineData(0.6f)]
    [InlineData(1.0f)]
    public void BallSize_Sets_RelativeSize_Of_Balls(float size) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.BallSize, size);
        });

        for (int i = 1; i <= 4; i++) {
            IElement ballDiv = loaderIconContainer.Find($".ball-{i}");
            IAttr ballStyle = ballDiv.Attributes["style"]!;
            Assert.Contains($"width: {40.0f * size}%", ballStyle.Value);
            Assert.Contains($"height: {40.0f * size}%", ballStyle.Value);
        }
    }

    [Theory]
    [InlineData(0.0f)]
    [InlineData(0.5f)]
    [InlineData(1.0f)]
    public void BallMargin_Sets_Left_And_Top_Gap(float margin) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.BallSize, 0.5f);
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.BallMargin, margin);
        });

        (float ballMarginInner, float ballMarginOuter) = margin switch {
            0.0f => (50.0f, 30.0f),
            1.0f => (70.0f, 10.0f),
            0.5f => (60.0f, 20.0f),
            _ => throw new ArgumentException("incorrect InlineData")
        };

        for (int i = 1; i <= 4; i++) {
            IElement ballDiv = loaderIconContainer.Find($".ball-{i}");
            IAttr ballStyle = ballDiv.Attributes["style"]!;

            (float leftMargin, float rightMargin) = i switch {
                1 => (ballMarginOuter, ballMarginOuter),
                2 => (ballMarginInner, ballMarginOuter),
                3 => (ballMarginOuter, ballMarginInner),
                4 => (ballMarginInner, ballMarginInner),
                _ => throw new Exception("impossible state")
            };
            Assert.Contains($"left: {leftMargin}%", ballStyle.Value);
            Assert.Contains($"top: {rightMargin}%", ballStyle.Value);
        }
    }

    [Theory]
    [InlineData(LoaderAnimation.Spin)]
    [InlineData(LoaderAnimation.Spin_skew)]
    public void Animation_Sets_AnimationType(LoaderAnimation animation) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.Animation, animation);
        });

        IElement loaderIconDiv = loaderIconContainer.Find(".loader-icon");
        IAttr style = loaderIconDiv.Attributes["style"]!;
        Assert.Contains(AnimationToString(animation), style.Value);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public void SpinDuration_Sets_AnimationSpeed(int duration) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.SpinDuration, duration);
        });

        IElement loaderIconDiv = loaderIconContainer.Find(".loader-icon");
        IAttr style = loaderIconDiv.Attributes["style"]!;
        Assert.Contains($"{duration}ms", style.Value);
    }

    [Theory]
    [InlineData(LoaderTimingFunction.Ease)]
    [InlineData(LoaderTimingFunction.Ease_in)]
    [InlineData(LoaderTimingFunction.Ease_in_out)]
    [InlineData(LoaderTimingFunction.Ease_out)]
    [InlineData(LoaderTimingFunction.Linear)]
    [InlineData(LoaderTimingFunction.Slow_middle)]
    public void SpinTimingFunction_Sets_TimingFunction(LoaderTimingFunction timeingFunction) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.SpinTimingFunction, timeingFunction);
        });

        IElement loaderIconDiv = loaderIconContainer.Find(".loader-icon");
        IAttr style = loaderIconDiv.Attributes["style"]!;
        Assert.Contains(TimingFunctionToString(timeingFunction), style.Value);
    }

    [Theory]
    [InlineData(LoaderDirection.Alternate)]
    [InlineData(LoaderDirection.Alternate_reverse)]
    [InlineData(LoaderDirection.Normal)]
    [InlineData(LoaderDirection.Reverse)]
    public void SpinDirection_Sets_Type_Of_Spinning(LoaderDirection direction) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.SpinDirection, direction);
        });

        IElement loaderIconDiv = loaderIconContainer.Find(".loader-icon");
        IAttr style = loaderIconDiv.Attributes["style"]!;
        Assert.Contains(DirectionToString(direction), style.Value);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public void ColorDuration_Sets_Speed_Of_Ball_ColorChanges(int duration) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.ColorDuration, duration);
        });

        for (int i = 1; i <= 4; i++) {
            IElement ballDiv = loaderIconContainer.Find($".ball-{i}");
            IAttr ballStyle = ballDiv.Attributes["style"]!;
            Assert.Contains($"{duration}ms", ballStyle.Value);
        }
    }

    [Theory]
    [InlineData(LoaderTimingFunction.Ease)]
    [InlineData(LoaderTimingFunction.Ease_in)]
    [InlineData(LoaderTimingFunction.Ease_in_out)]
    [InlineData(LoaderTimingFunction.Ease_out)]
    [InlineData(LoaderTimingFunction.Linear)]
    [InlineData(LoaderTimingFunction.Slow_middle)]
    public void ColorTimingFunction_Sets_Ball_TimingFunction(LoaderTimingFunction timeingFunction) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.ColorTimingFunction, timeingFunction);
        });

        for (int i = 1; i <= 4; i++) {
            IElement ballDiv = loaderIconContainer.Find($".ball-{i}");
            IAttr ballStyle = ballDiv.Attributes["style"]!;
            Assert.Contains(TimingFunctionToString(timeingFunction), ballStyle.Value);
        }
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    [InlineData(30)]
    public void BallMoveDuration_Sets_Speed_Of_Ball_Moving(int duration) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.BallMoveDuration, duration);
        });

        for (int i = 1; i <= 4; i++) {
            IElement ballDiv = loaderIconContainer.Find($".ball-{i}");
            IAttr ballStyle = ballDiv.Attributes["style"]!;
            Assert.Contains($"{duration}ms", ballStyle.Value);
        }
    }

    [Theory]
    [InlineData(LoaderTimingFunction.Ease)]
    [InlineData(LoaderTimingFunction.Ease_in)]
    [InlineData(LoaderTimingFunction.Ease_in_out)]
    [InlineData(LoaderTimingFunction.Ease_out)]
    [InlineData(LoaderTimingFunction.Linear)]
    [InlineData(LoaderTimingFunction.Slow_middle)]
    public void BallMoveTimingFunction_Sets_Ball_TimeingFunction_For_Moving_Back_And_Forth(LoaderTimingFunction timeingFunction) {
        IRenderedComponent<LoaderIcon> loaderIconContainer = RenderComponent((ComponentParameterCollectionBuilder<LoaderIcon> builder) => {
            builder.Add((LoaderIcon loaderIcon) => loaderIcon.BallMoveTimingFunction, timeingFunction);
        });

        for (int i = 1; i <= 4; i++) {
            IElement ballDiv = loaderIconContainer.Find($".ball-{i}");
            IAttr ballStyle = ballDiv.Attributes["style"]!;
            Assert.Contains(TimingFunctionToString(timeingFunction), ballStyle.Value);
        }
    }

    #endregion


    #region helper methods

    private static string AnimationToString(LoaderAnimation value) {
        return value switch {
            LoaderAnimation.Spin => "spin",
            LoaderAnimation.Spin_skew => "spin-skew",
            _ => throw new Exception("pigs have learned to spin")
        };
    }

    private static string TimingFunctionToString(LoaderTimingFunction value) {
        return value switch {
            LoaderTimingFunction.Linear => "linear",
            LoaderTimingFunction.Ease => "ease",
            LoaderTimingFunction.Ease_in => "ease-in",
            LoaderTimingFunction.Ease_out => "ease-out",
            LoaderTimingFunction.Ease_in_out => "ease-in-out",
            LoaderTimingFunction.Slow_middle => "cubic-bezier(0.2, 0.8, 0.8, 0.2)",
            _ => throw new Exception("pigs have learned to time")
        };
    }

    private static string DirectionToString(LoaderDirection value) {
        return value switch {
            LoaderDirection.Normal => "normal",
            LoaderDirection.Reverse => "reverse",
            LoaderDirection.Alternate => "alternate",
            LoaderDirection.Alternate_reverse => "alternate-reverse",
            _ => throw new Exception("pigs have learned to orientate")
        };
    }

    #endregion
}
