using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace NoJsBlazor {
    /// <summary>
    /// <para>An object that holds a ranged input together with a decrease and increase button and a label indicating the current value.</para>
    /// <para>The value of this object is from type <see cref="int"/>.</para>
    /// </summary>
    public partial class Slider {
        /// <summary>
        /// Value of the Slider
        /// </summary>
        [Parameter]
        public int Value { get; set; }

        [Parameter]
        public EventCallback<int> ValueChanged { get; set; }

        [Parameter]
        public EventCallback<int> OnChange { get; set; }

        /// <summary>
        /// An optional label.
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Slider lower bounds.
        /// </summary>
        [Parameter]
        public int Min { get; set; } = 0;

        /// <summary>
        /// Slider upper bounds.
        /// </summary>
        [Parameter]
        public int Max { get; set; } = 100;

        /// <summary>
        /// Slider precision
        /// </summary>
        [Parameter]
        public int Step { get; set; } = 1;

        /// <summary>
        /// <para>Indicates if the user is able to edit the number directly.</para>
        /// <para>Technically the number is displayed in a input field instead of a label,</para>
        /// </summary>
        [Parameter]
        public bool Editable { get; set; } = false;

        /// <summary>
        /// Content inside the left Button.
        /// </summary>
        [Parameter]
        public RenderFragment LeftButtonText { get; set; } = new RenderFragment((builder => builder.AddContent(0, "🡸")));

        /// <summary>
        /// Content inside the right Button.
        /// </summary>
        [Parameter]
        public RenderFragment RightButtonText { get; set; } = new RenderFragment((builder => builder.AddContent(0, "🡺")));

        /// <summary>
        /// The way the value should be printed.
        /// </summary>
        [Parameter]
        public Func<int, string> Display { get; set; }

        /// <summary>
        /// <para>Parsing function for the edit field.</para>
        /// <para>It gets the content of the edit field as string and returns the appropriated integer. It Returns null if the value is not valid.</para>
        /// <para>Default succeed if the value is int-parseable and in Min/Max bounds.</para>
        /// </summary>
        [Parameter]
        public Func<string, int?> ParseEdit { get; set; }

        /// <summary>
        /// Captures unmatched values
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        private readonly TouchClick leftButtonTC;
        private readonly TouchClick rightButtonTC;


        public Slider() {
            leftButtonTC = new TouchClick(LeftButton);
            rightButtonTC = new TouchClick(RightButton);
            Display = (int value) => value.ToString();
            ParseEdit = DefaultParseEdit;
        }

        #region private Methods

        private void LeftButton(EventArgs e) {
            if (Value > Min) {
                Value -= Step;
                ValueChanged.InvokeAsync(Value);
                OnChange.InvokeAsync(Value);
            }
        }

        private void RightButton(EventArgs e) {
            if (Value < Max) {
                Value += Step;
                ValueChanged.InvokeAsync(Value);
                OnChange.InvokeAsync(Value);
            }
        }

        private void OnSlider(ChangeEventArgs e) {
            Value = int.Parse((string)e.Value);
            ValueChanged.InvokeAsync(Value);
        }

        private void OnChangeEditField(ChangeEventArgs input) {
            int? buffer = ParseEdit((string)input.Value);
            if (buffer != null) {
                Value = (int)buffer;
                ValueChanged.InvokeAsync(Value);
                OnChange.InvokeAsync(Value);
            }
        }


        private int? DefaultParseEdit(string input) {
            if (int.TryParse(input, out int result))
                if (result < Min)
                    return Min;
                else if (Max < result)
                    return Max;
                else
                    return result;

            return null;
        }

        #endregion
    }
}
