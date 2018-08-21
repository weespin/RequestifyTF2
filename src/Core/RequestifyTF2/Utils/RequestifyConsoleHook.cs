// RequestifyTF2
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
            WriteEvent(this, new RequestifyConsoleHookArgs(value));
            base.Write(value);
        }

        public override void WriteLine(string value)
        {
            if (WriteEvent == null) return;
            WriteLineEvent?.Invoke(this, new RequestifyConsoleHookArgs(value));
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