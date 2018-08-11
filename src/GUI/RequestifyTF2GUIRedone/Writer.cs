using System;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace RequestifyTF2GUIRedone
{
    public class TextBoxStreamWriter : TextWriter
    {
      //  private readonly TextBlock _outp2;
        private readonly TextBox _output;
        public TextBoxStreamWriter(TextBox output/*,TextBlock outp2*/)
        {
            _output = output;
         //   _outp2 = outp2;
        }
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            base.Write(value);
            _output.Dispatcher.BeginInvoke(new Action(delegate { _output.AppendText(value.ToString()); }));
           /* _outp2.Dispatcher.BeginInvoke(new Action(delegate
            {
                if (_outp2.Text.EndsWith(Environment.NewLine))
                {
                    _outp2.Text = null;
                }

                _outp2.Text += value.ToString();
            }));*/
        }
    }
}