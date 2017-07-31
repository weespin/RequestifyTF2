using System;
using System.Diagnostics;
using System.Linq;
using RequestifyTF2.Api;

namespace RTTSPlugin
{
    public class RttsPlugin : IPlugin

    {
        public string Name => "RTTS";

        public string Command => "!rtts";
        public bool OnlyCode => false;

        public double Version => 1;
        public string Author => "Cancer";

        public void Execute(string[] command, bool admin)
        {
            var i = command.Count(a => a == "!rtts");
            if (i != 1)
                return;
            var index = Array.IndexOf(command, "!rtts");
            if (command.Length < index + 1)
                return;

            var text = "";

            for (var j = index + 1; j < command.Length; j++)
                text += command[j];

            MakeWav(text, true);
            Instances.Vlc.Add(@"C:\test\ttsfile.wav");
        }

        public static void MakeWav(string text, bool waitFlag)
        {
            var outfile = "-E \"ELAN TTS Russian(Nicolai 16Khz)\"  -I  -S20 -V100 \"" + text +
                          "\" -D \"DigaloRussianNicolai.dic\"  -TO \"C:\\test\\ttsfile.wav\" \r\n";
            var psi = new ProcessStartInfo();
            psi.FileName = "C:/test/govorilka_cp.exe";
            psi.Arguments = outfile;
            psi.WindowStyle = ProcessWindowStyle.Minimized;
            var p = Process.Start(psi);
            if (waitFlag)
                p.WaitForExit();
        }
    }
}