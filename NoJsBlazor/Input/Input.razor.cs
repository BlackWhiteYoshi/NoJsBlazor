using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace NoJsBlazor {
    public partial class Input {
        /// <summary>
        /// Value of this Input field.
        /// </summary>
        [Parameter]
        public string Value {
            get => _value;
            set {
                _value = value;
                hasValue = !string.IsNullOrEmpty(Value);
            }
        }
        private string _value;

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        /// <summary>
        /// Displayed text of this Input field.
        /// </summary>
        [Parameter]
        public string Name { get; set; }

        /// <summary>
        /// Indicates if the characters should be display as *****
        /// </summary>
        [Parameter]
        public bool Password { get; set; }

        /// <summary>
        /// Captures unmatched values
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }


        private bool hasValue;

        private void OnInput(ChangeEventArgs e) {
            Value = (string)e.Value;
            ValueChanged.InvokeAsync(Value);
        }
    }
}
