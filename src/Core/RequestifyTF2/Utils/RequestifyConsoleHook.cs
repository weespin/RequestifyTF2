using System;
using System.IO;
using System.Text;

namespace RequestifyTF2.Utils
{
    public class RequestifyConsoleHook : TextWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
        
        public override void Write(string value)
        {
            if (WriteEvent == null) return;
            WriteEvent(this,new RequestifyConsoleHookArgs(value));
            base.Write(value);
        }

        public override void WriteLine(string value)
        {
            if (WriteEvent == null) return;
            WriteLineEvent(this, new RequestifyConsoleHookArgs(value));
            base.WriteLine(value);
        }

        public event EventHandler<RequestifyConsoleHookArgs> WriteEvent;
        public event EventHandler<RequestifyConsoleHookArgs> WriteLineEvent;
    }

    public class RequestifyConsoleHookArgs : EventArgs
    {
        public string Value { get; private set; }

        public RequestifyConsoleHookArgs(string value)
        {
            Value = value;
        }
    }
        
    
}