using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using Environment = System.Environment;
using Label = System.Windows.Controls.Label;

namespace RequestifyTF2GUIRedone
{
    public class TextBoxStreamWriter : TextWriter
    {
        public System.Windows.Controls.TextBox _output;
        public TextBlock _outp2;

        public TextBoxStreamWriter(System.Windows.Controls.TextBox output,TextBlock outp2)
        {
            this._output = output;
            this._outp2 = outp2;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            base.Write(value);

          _output.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                _output.AppendText(value.ToString());
            }));

      
           Write2(value);

        }

    

        public void Write2(char val)
        {
            _outp2.Dispatcher.BeginInvoke(new Action(delegate() {
                if (_outp2.Text.ToString().EndsWith(Environment.NewLine))
                {
                    _outp2.Text = null;
                }

                _outp2.Text += val.ToString();
            }));
          

        }
    }
}
