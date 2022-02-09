using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace RequestifyTF2.API
{
    public static class ConsoleSender
    {
        static ConsoleSender()
        {
            new Task(Reader).Start();

        }
        public static Queue<string> CommandQueue = new Queue<string>();
        private static DateTime lastsend = DateTime.Now;
        static void Reader()
        {
            while (true)
            {
                if (CommandQueue.Count > 0)
                {
                    if ((DateTime.Now - lastsend).TotalMilliseconds > 800)
                    {
                        var cmnd = CommandQueue.Dequeue();
                        File.WriteAllText(Instance.Config.GameDir + "/cfg/requestify.cfg", cmnd);
                        Task.Run(
                            () =>
                            {
                                keybd_event(0x2E, 0x53, 0, 0);
                                Thread.Sleep(30);
                                keybd_event(0x2E, 0x53, 0x2, 0);
                            });
                        Thread.Sleep(100);
                        File.WriteAllText(Instance.Config.GameDir + "/cfg/requestify.cfg", string.Empty);
                        lastsend = DateTime.Now;
                    }
                }
                Thread.Sleep(10);
            }
        }

        
        public enum Command
        {
            Chat,
            Echo,
            Raw
        }

        public static void SendCommand(string cmnd, Command cmd)
        {
            
            var text = string.Empty;
            switch (cmd)
            {
                case Command.Chat:
                    if (!Instance.IsMuted)
                    {
                        text = "say " + cmnd;
                    }

                    break;
                case Command.Echo:
                    text = "echo " + cmnd;
                    break;
                case Command.Raw:
                    text = cmnd;
                    break;
                default:
                    throw new InvalidOperationException();
                  
            }
            CommandQueue.Enqueue(text);

         
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
    }
}