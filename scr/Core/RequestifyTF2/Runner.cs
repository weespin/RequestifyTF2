using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using RequestifyTF2.Api;
using RequestifyTF2.VLC;
using RequestifyTF2.VLCUpdater;

namespace RequestifyTF2
{
    public static class Runner
    {
        public static List<string> Ignored = new List<string>();
        public static VlcRemote Vlc = Instances.Vlc;
        public static Instances.Config Cfg = new Instances.Config();
        public static bool IsTf2Running;

        public static void Start()
        {
            Instances.Load();
            //kek:
            //var proc = Process.GetProcessesByName("hl2");
            //try
            //{
            //    foreach (var f in proc)
            //        f.Kill();
            //}
            //catch (Exception)
            //{
            //    if (MessageBox.Show("Please close Game and then press OK", "Game is running", MessageBoxButtons.OK) ==
            //        DialogResult.OK)
            //    {
            //        Thread.Sleep(1000);
            //        goto kek;
            //    }
            //}
            Thread.Sleep(500);
            if (File.Exists(Instances.Config.GameDir + "/console.log"))
                try
                {
                    File.WriteAllText(Instances.Config.GameDir + "/console.log", "");
                }
                catch (Exception)
                {
                }
            Thread.Sleep(4000);
         //   Process.Start("steam://rungameid/440");
            var autoexecChecker = new AutoexecChecker();
            ReaderThread.Starter();
            var update = new Update();


            //Init
        }
    }
}