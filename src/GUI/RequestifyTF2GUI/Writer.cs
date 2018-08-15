using System;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace RequestifyTF2GUI
{
    public class TextBoxStreamWriter : TextWriter
    {
     
        private readonly TextBlock _output;
        public TextBoxStreamWriter(TextBlock output)
        {
            _output = output;
         
        }
        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            base.Write(value);
            _output.Dispatcher.BeginInvoke(new Action(delegate { _output.Text+=(value.ToString()); }));
         
        }
    }
}