using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace NoJsBlazor {
    /// <summary>
    /// Types of spinning
    /// </summary>
    public enum LoaderAnimation {
        /// <summary>
        /// normal spin
        /// </summary>
        Spin,
        /// <summary>
        /// spin with distortion
        /// </summary>
        Spin_skew
    }

    /// <summary>
    /// Speed/Acceleration selection
    /// </summary>
    public enum LoaderTimingFunction {
        /// <summary>
        /// constant speed
        /// </summary>
        Linear,
        /// <summary>
        /// slow start, then fast, ends slowly
        /// </summary>
        Ease,
        /// <summary>
        /// slow start, fast end
        /// </summary>
        Ease_in,
        /// <summary>
        /// fast start, slow end
        /// </summary>
        Ease_out,
        /// <summary>
        /// same as <see cref="Ease">Ease</see>, only even slower start and end
        /// </summary>
        Ease_in_out,
        /// <summary>
        /// fast rotation, but in the middle part very slow
        /// </summary>
        Slow_middle
    }

    /// <summary>
    /// Rotation directions
    /// </summary>
    public enum LoaderDirection {
        /// <summary>
        /// clockwise
        /// </summary>
        Normal,
        /// <summary>
        /// counterclockwise
        /// </summary>
        Reverse,
        /// <summary>
        /// begins with clockwise, then counterclockwise, ...
        /// </summary>
        Alternate,
        /// <summary>
        /// begins with counterclockwise, then clockwise, ...
        /// </summary>
        Alternate_reverse
    }

    /// <summary>
    /// A little, animated icon that can be used to indicate loading.
    /// </summary>
    public partial class LoaderIcon {
        #region Parameters

        /// <summary>
        /// <para>Base size of the icon.</para>
        /// <para>Default is 100.</para>
        /// </summary>
        [Parameter]
        public int Size { get; set; } = 100;

        private float _ballSize = 0.875F;
        private float ballSize = 35.0F;
        /// <summary>
        /// <para>Ball size relativ to the <see cref="Size">base size</see>.</para>
        /// <para>This value can range from 0 to 1, default is 0.875.</para>
        /// </summary>
        [Parameter]
        public float BallSize {
            get => _ballSize;
            set {
                if (value > 1.0F)
                    value = 1.0F;
                if (value < 0.0F)
                    value = 0.0F;
                _ballSize = value;
                ballSize = value * 40.0F;
            }
        }

        private float _ballMargin = 0.25F;
        private float ballMarginOuter;
        private float ballMarginInner;
        /// <summary>
        /// <para>Space between the balls relativ to the ball size.</para>
        /// <para>If ball size is 1.0, this value has no effect.</para>
        /// <para>This value can range from 0 to 1, default is 0.25.</para>
        /// </summary>
        [Parameter]
        public float BallMargin {
            get => _ballMargin;
            set {
                if (value > 1.0F)
                    value = 1.0F;
                if (value < 0.0F)
                    value = 0.0F;
                _ballMargin = value;
                ballMarginOuter = ((40.0F - ballSize) * (1.0F - value) + 10.0F);
                ballMarginInner = 100 - ballSize - ballMarginOuter;
            }
        }

        private LoaderAnimation _animation = LoaderAnimation.Spin;
        private string animation = "spin";
        /// <summary>
        /// <para>Type of animation to play.</para>
        /// <para>Default is LoaderAnimation.Spin.</para>
        /// </summary>
        [Parameter]
        public LoaderAnimation Animation {
            get => _animation;
            set {
                _animation = value;
                animation = AnimationToString(value);
            }
        }

        /// <summary>
        /// <para>Speed of the rotation in ms.</para>
        /// <para>Defailt is 2000.</para>
        /// </summary>
        [Parameter]
        public int SpinDuration { get; set; } = 2000;

        private LoaderTimingFunction _spinTimingFunction = LoaderTimingFunction.Linear;
        private string spinTimingFunction = "linear";
        /// <summary>
        /// <para>Timing function of the rotation.</para>
        /// <para>Defailt is LoaderTimingFunction.Linear.</para>
        /// </summary>
        [Parameter]
        public LoaderTimingFunction SpinTimingFunction {
            get => _spinTimingFunction;
            set {
                _spinTimingFunction = value;
                spinTimingFunction = TimingFunctionToString(value);
            }
        }

        private LoaderDirection _spinDircetion = LoaderDirection.Normal;
        private string spinDirection = "normal";
        /// <summary>
        /// <para>Animation direction of the rotation.</para>
        /// <para>Defailt is LoaderDirection.Normal.</para>
        /// </summary>
        [Parameter]
        public LoaderDirection SpinDirection {
            get => _spinDircetion;
            set {
                _spinDircetion = value;
                spinDirection = DirectionToString(value);
            }
        }

        /// <summary>
        /// <para>Speed of color changing in ms.</para>
        /// <para>Default is 1500.</para>
        /// </summary>
        [Parameter]
        public int ColorDuration { get; set; } = 1500;

        private LoaderTimingFunction _colorTimingFunction = LoaderTimingFunction.Linear;
        private string colorTimingFunction = "linear";
        /// <summary>
        /// <para>Timing function of color changing.</para>
        /// <para>Default is LoaderTimingFunction.Linear.</para>
        /// </summary>
        [Parameter]
        public LoaderTimingFunction ColorTimingFunction {
            get => _colorTimingFunction;
            set {
                _colorTimingFunction = value;
                colorTimingFunction = TimingFunctionToString(value);
            }
        }

        /// <summary>
        /// <para>Speed of moving the balls outwards in ms.</para>
        /// <para>Value of 0 will remove this animation.</para>
        /// <para>Default is 2000.</para>
        /// </summary>
        [Parameter]
        public int BallMoveDuration { get; set; } = 2000;

        private LoaderTimingFunction _ballmoveTimingFunction = LoaderTimingFunction.Ease_in_out;
        private string ballmoveTimingFunction = "ease-in-out";
        /// <summary>
        /// <para>Timing funtion of moving the balls outwards.</para>
        /// <para>Default is LoaderTimingFunction.Ease_in_out.</para>
        /// </summary>
        [Parameter]
        public LoaderTimingFunction BallMoveTimingFunction {
            get => _ballmoveTimingFunction;
            set {
                _ballmoveTimingFunction = value;
                ballmoveTimingFunction = TimingFunctionToString(value);
            }
        }

        /// <summary>
        /// Captures unmatched values
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? Attributes { get; set; }

        #endregion

        #region Functions

        protected override void OnInitialized() => BallMargin = _ballMargin;

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

        private object? AddStyles() {
            if (Attributes == null)
                return null;

            if (Attributes.TryGetValue("style", out object? styles))
                return styles;
            else
                return null;
        }

        #endregion
    }
}
