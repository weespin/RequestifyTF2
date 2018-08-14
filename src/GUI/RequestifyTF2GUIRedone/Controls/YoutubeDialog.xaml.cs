using System.Globalization;
using System.Windows.Controls;

namespace RequestifyTF2GUIRedone.Controls
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class SampleDialog : UserControl
    {
        public SampleDialog()
        {
            InitializeComponent();
        }
    }
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }
}
