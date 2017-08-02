using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ConsoleRedirection
{
    public class TextBoxStreamWriter : TextWriter
    {
        public TextBox _output;

        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            base.Write(value);
            _output.AppendText(value.ToString()); // When character data is written, append it to the text box.
        }
    }
}