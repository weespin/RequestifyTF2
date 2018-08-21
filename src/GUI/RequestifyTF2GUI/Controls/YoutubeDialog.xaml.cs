// RequestifyTF2GUI
// Copyright (C) 2018  Villiam Nmerukini
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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
            return new ValidationResult(false, "Field is required.");
        }
    }
}