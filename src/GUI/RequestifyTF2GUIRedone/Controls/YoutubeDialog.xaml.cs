using System.Globalization;
using System.Windows.Controls;
using System.Text.RegularExpressions;
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
        Regex youtube = new Regex(@"youtube\..+?/watch.*?v=(.*?)(?:&|/|$)");
        Regex shortregex = new Regex(@"youtu\.be/(.*?)(?:\?|&|/|$)");

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Field is required.");
            }

        var ret = false || youtube.Match(value.ToString()).Success || shortregex.Match(value.ToString()).Success;

            if (ret)
                return ValidationResult.ValidResult;
            else
            return new ValidationResult(false, "Field is required.");
           
        }
    }
}
