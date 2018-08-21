// RequestifyTF2GUIOld(unsupported)
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

namespace RequestifyTF2Forms
{
    using System;
    using System.Drawing;
    using MaterialSkin.Controls;
    using RequestifyTF2GUIOld.Properties;

    public partial class MessageBox : MaterialForm
    {
        public MessageBox()
        {
            InitializeComponent();
            lbl_text.Font = SkinManager.ROBOTO_REGULAR_11;
            Icon = Resources.Icon;
        }

        public string Color
        {
            set
            {
                // ReSharper disable once ArrangeThisQualifier
                // ReSharper disable once ArrangeAccessorOwnerBody
                this.lbl_text.ForeColor = ColorTranslator.FromHtml(value);
            }
        }

        public string MessageText
        {
            set
            {
                // ReSharper disable once ArrangeThisQualifier
                // ReSharper disable once ArrangeAccessorOwnerBody
                this.lbl_text.Text = value;
            }
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}