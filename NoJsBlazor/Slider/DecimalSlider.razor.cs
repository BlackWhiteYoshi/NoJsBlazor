using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;

namespace NoJsBlazor {
    /// <summary>
    /// <para>An object that holds a ranged input together with a decrease and increase button and a label indicating the current value.</para>
    /// <para>The value of this object is from type <see cref="decimal"/>.</para>
    /// </summary>
    public partial class DecimalSlider {
        /// <summary>
        /// Value of the Slider
        /// </summary>
        [Parameter]
        public decimal Value { get; set; }

        [Parameter]
        public EventCallback<decimal> ValueChanged { get; set; }

        [Parameter]
        public EventCallback<decimal> OnChange { get; set; }

        /// <summary>
        /// An optional label.
        /// </summary>
        [Parameter]
        public string Title { get; set; }

        /// <summary>
        /// Initial value.
        /// </summary>
        [Parameter]
        public decimal StartValue { get; set; } = 0;

        /// <summary>
        /// Slider lower bounds.
        /// </summary>
        [Parameter]
        public decimal Min { get; set; } = 0;

        /// <summary>
        /// Slider upper bounds.
        /// </summary>
        [Parameter]
        public decimal Max { get; set; } = 100;

        /// <summary>
        /// Slider precision
        /// </summary>
        [Parameter]
        public decimal Step { get; set; } = 1;

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
        public Func<decimal, string> Display { get; set; } = (decimal value) => value.ToString();

        /// <summary>
        /// <para>Parsing function for the edit field.</para>
        /// <para>It gets the content of the edit field as string and returns the appropriated decimal number. It Returns null if the value is not valid.</para>
        /// <para>Default succeed if the value is decimal-parseable and in Min/Max bounds.</para>
        /// </summary>
        [Parameter]
        public Func<string, decimal?> ParseEdit { get; set; }

        /// <summary>
        /// Captures unmatched values
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        private decimal value;

        private readonly TouchClick leftButtonTC;
        private readonly TouchClick rightButtonTC;


        public DecimalSlider() {
            leftButtonTC = new TouchClick(LeftButton);
            rightButtonTC = new TouchClick(RightButton);
            Display = (decimal value) => value.ToString();
            ParseEdit = DefaultParseEdit;
        }

        protected override void OnInitialized() {
            if (StartValue < Min)
                value = Min;
            else
                value = StartValue;
        }

        #region private Methods

        private void LeftButton(EventArgs e) {
            if (value > Min) {
                value -= Step;
                ValueChanged.InvokeAsync(Value);
                OnChange.InvokeAsync(Value);
            }
        }

        private void RightButton(EventArgs e) {
            if (value < Max) {
                value += Step;
                ValueChanged.InvokeAsync(Value);
                OnChange.InvokeAsync(Value);
            }
        }


        private void OnSlider(ChangeEventArgs input) {
            value = decimal.Parse((string)input.Value);
            ValueChanged.InvokeAsync(Value);
        }

        private void OnChangeEditField(ChangeEventArgs input) {
            decimal? buffer = ParseEdit((string)input.Value);
            if (buffer != null) {
                value = (decimal)buffer;
                ValueChanged.InvokeAsync(Value);
                OnChange.InvokeAsync(Value);
            }
        }


        private decimal? DefaultParseEdit(string input) {
            if (decimal.TryParse(input, out decimal result))
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
