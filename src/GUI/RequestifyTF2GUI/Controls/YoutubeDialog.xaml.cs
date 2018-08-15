using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace RequestifyTF2GUI.Controls
{
    /// <summary>
    /// Interaction logic for YoutubeDialog.xaml
    /// </summary>
    public partial class YoutubeDialog : UserControl
    {
        public YoutubeDialog()
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
