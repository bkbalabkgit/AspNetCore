using System;
using System.Globalization;

namespace WpfMvvmApp.ViewModels
{
    public class DocFieldViewModel : ViewModelBase
    {
        public string Key { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string Placeholder { get; init; } = string.Empty;

        public bool IsText { get; init; }
        public bool IsDate { get; init; }
        public bool IsMultiline { get; init; }
        public bool IsCheckbox { get; init; }

        public double Confidence { get; init; } = 1.0;
        public bool IsLowConfidence => Confidence < 0.85;
        public string ConfidenceLabel => string.Format(CultureInfo.InvariantCulture, "OCR {0:P0}", Confidence);

        private string? _textValue;
        public string? TextValue
        {
            get => _textValue;
            set
            {
                _textValue = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _dateValue;
        public DateTime? DateValue
        {
            get => _dateValue;
            set
            {
                _dateValue = value;
                OnPropertyChanged();
            }
        }

        private bool _boolValue;
        public bool BoolValue
        {
            get => _boolValue;
            set
            {
                _boolValue = value;
                OnPropertyChanged();
            }
        }
    }
}
