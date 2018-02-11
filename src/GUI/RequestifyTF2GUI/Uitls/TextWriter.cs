namespace ConsoleRedirection
{
    using System.IO;
    using System.Text;
    using System.Windows.Forms;

    public class TextBoxStreamWriter : TextWriter
    {
        public TextBox _output;

        public TextBoxStreamWriter(TextBox output)
        {
            this._output = output;
        }

        public override Encoding Encoding => Encoding.UTF8;

        public override void Write(char value)
        {
            base.Write(value);

            this._output.Invoke(new MethodInvoker(delegate { this._output.AppendText(value.ToString()); }));
        }
    }
}