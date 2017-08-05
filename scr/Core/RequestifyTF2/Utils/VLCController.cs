using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;
using RequestifyTF2.Api;

//https://gist.github.com/SamSaffron/101357
/*

 +----[ Remote control commands ]

|

| add XYZ  . . . . . . . . . . . . add XYZ to playlist

| enqueue XYZ  . . . . . . . . . queue XYZ to playlist

| playlist . . . . .  show items currently in playlist

| play . . . . . . . . . . . . . . . . . . play stream

| stop . . . . . . . . . . . . . . . . . . stop stream

| next . . . . . . . . . . . . . .  next playlist item

| prev . . . . . . . . . . . .  previous playlist item

| goto . . . . . . . . . . . . . .  goto item at index

| repeat [on|off] . . . .  toggle playlist item repeat

| loop [on|off] . . . . . . . . . toggle playlist loop

| random [on|off] . . . . . . .  toggle random jumping

| clear . . . . . . . . . . . . . . clear the playlist

| status . . . . . . . . . . . current playlist status

| title [X]  . . . . . . set/get title in current item

| title_n  . . . . . . . .  next title in current item

| title_p  . . . . . .  previous title in current item

| chapter [X]  . . . . set/get chapter in current item

| chapter_n  . . . . . .  next chapter in current item

| chapter_p  . . . .  previous chapter in current item

|

| seek X . . . seek in seconds, for instance `seek 12'

| pause  . . . . . . . . . . . . . . . .  toggle pause

| fastforward  . . . . . . . .  .  set to maximum rate

| rewind  . . . . . . . . . . . .  set to minimum rate

| faster . . . . . . . . . .  faster playing of stream

| slower . . . . . . . . . .  slower playing of stream

| normal . . . . . . . . . .  normal playing of stream

| f [on|off] . . . . . . . . . . . . toggle fullscreen

| info . . . . .  information about the current stream

| stats  . . . . . . . .  show statistical information

| get_time . . seconds elapsed since stream's beginning

| is_playing . . . .  1 if a stream plays, 0 otherwise

| get_title . . . . .  the title of the current stream

| get_length . . . .  the length of the current stream

|

| volume [X] . . . . . . . . . .  set/get audio volume

| volup [X]  . . . . . . .  raise audio volume X steps

| voldown [X]  . . . . . .  lower audio volume X steps

| adev [X] . . . . . . . . . . .  set/get audio device

| achan [X]. . . . . . . . . .  set/get audio channels

| atrack [X] . . . . . . . . . . . set/get audio track

| vtrack [X] . . . . . . . . . . . set/get video track

| vratio [X]  . . . . . . . set/get video aspect ratio

| vcrop [X]  . . . . . . . . . . .  set/get video crop

| vzoom [X]  . . . . . . . . . . .  set/get video zoom

| snapshot . . . . . . . . . . . . take video snapshot

| strack [X] . . . . . . . . . set/get subtitles track

| key [hotkey name] . . . . . .  simulate hotkey press

| menu . . [on|off|up|down|left|right|select] use menu

|

| help . . . . . . . . . . . . . . . this help message

| longhelp . . . . . . . . . . . a longer help message

| logout . . . . . . .  exit (if in socket connection)

| quit . . . . . . . . . . . . . . . . . . .  quit vlc

|

+----[ end of help ]

 

 */

namespace RequestifyTF2.VLC
{
    public static class Fixer
    {
        public static void Fix()
        {
            var c = false;
            var guids = Instance.Vlc.Adev();
            var guidsprl = guids.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var a in guidsprl)
                if (a.Contains("Virtual Audio Cable"))
                {
                    var output = "{" + a.Split('{', '}')[1] + "}";
                    output = output.Replace(" ", "");
                    Instance.Vlc.SendRaw("adev", output);
                    c = true;
                }
            Instance.Vlc.SendRaw("loop", "off");
            Instance.Vlc.SendRaw("repeat", "off");
            if (!c)
                MessageBox.Show("VIRTUAL AUDIO CABLE IS NOT FOUND!", "ERROR", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }
    }

    internal enum VlcCommand
    {
        Add,

        Enqueue,

        Play,

        F,
        Adev,

        IsPlaying,

        GetTime,

        Seek,

        Pause,

        FastForward,

        Rewind
    }


    public class VlcRemote
    {
        // maximum 2 second wait on results. 

        private const int WaitTimeout = 3000;


        private static readonly ASCIIEncoding AsciiEncoding = new ASCIIEncoding();

        private readonly TcpClient _client;


        private Process _vlcProcess;


        public VlcRemote()
        {
            var ports = GetNetStatPorts();
            Logger.Write(Logger.Status.Info, $"{ports.Where(n=>n.port_number=="9876").Count()} processes using our port!");
            foreach (var port in ports)
            {
                if (port.port_number == "9876")
                {
                    try
                    {
                        Logger.Write(Logger.Status.Info, "Killing VLC!");
                        Process.GetProcessById(port.pid).Kill();
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            string vlcPath = null;

            var vlcKey = Registry.LocalMachine.OpenSubKey(@"Software\VideoLan\VLC");

            if (vlcKey == null)
                vlcKey = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\VideoLan\VLC");

            if (vlcKey != null)
                vlcPath = vlcKey.GetValue(null) as string;


            if (vlcPath == null)
                MessageBox.Show("CANT FIND VLC INSTALLED!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);

            var info =
                new ProcessStartInfo(vlcPath, "-I rc  --rc-host=localhost:9876 --no-video")
                {
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = true
                };
            Logger.Write(Logger.Status.Info, "Starting VLC");
            _vlcProcess = Process.Start(info);
            Logger.Write(Logger.Status.Info, "Started!");
            while (Process.GetProcessesByName("vlc").Length == 0)
            {
                // :^)
            }
            _client = new TcpClient("localhost", 9876);
            Logger.Write(Logger.Status.Info, "Connected to VLC!");
        }


        public Process VlcPlaybackProcess
        {
            get
            {
                var currentProcessId = Process.GetCurrentProcess().Id;

                return Process.GetProcessesByName("vlc").FirstOrDefault(process => GetParentProcessId(process.Id) == currentProcessId);
            }
        }


        public bool IsPlaying
        {
            get
            {
                SendCommand(VlcCommand.IsPlaying);

                var result = WaitForResult().Trim();

                return result == "1";
            }
        }
        public static List<Port> GetNetStatPorts()
        {
            var Ports = new List<Port>();

            try
            {
                using (var p = new Process())
                {

                    var ps = new ProcessStartInfo
                    {
                        Arguments = "-a -n -o",
                        FileName = "netstat.exe",
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    p.StartInfo = ps;
                    p.Start();

                    var stdOutput = p.StandardOutput;
                    var stdError = p.StandardError;

                    var content = stdOutput.ReadToEnd() + stdError.ReadToEnd();
                    var exitStatus = p.ExitCode.ToString();

                    if (exitStatus != "0")
                    {
                        // Command Errored. Handle Here If Need Be
                    }

                    //Get The Rows
                    var rows = Regex.Split(content, "\r\n");
                    Ports.AddRange(from row in rows
                        select Regex.Split(row, "\\s+")
                        into tokens
                        where tokens.Length > 4 && (tokens[1].Equals("UDP") || tokens[1].Equals("TCP"))
                        let localAddress = Regex.Replace(tokens[2], @"\[(.*?)\]", "1.1.1.1")
                        select new Port
                        {
                            protocol = localAddress.Contains("1.1.1.1") ? $"{tokens[1]}v6" : $"{tokens[1]}v4",
                            port_number = localAddress.Split(':')[1],
                            pid = tokens[1] == "UDP" ? Convert.ToInt16(tokens[4]) : Convert.ToInt16(tokens[5])
                        });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ports;
        }
        public class Port
        {

            public string port_number { get; set; }
            public short pid { get; set; }
            public string protocol { get; set; }
        }
        public int Position
        {
            get
            {
                SendCommand(VlcCommand.GetTime);

                var result = WaitForResult().Trim();

                return Convert.ToInt32(result);
            }

            // ReSharper disable once ArrangeAccessorOwnerBody
            set { SendCommand(VlcCommand.Seek, value.ToString()); }
        }


        private static int GetParentProcessId(int id)

        {
            var parentPid = 0;

            using (var mo = new ManagementObject("win32_process.handle='"
                                                 + id + "'"))

            {
                mo.Get();

                parentPid = Convert.ToInt32(mo["ParentProcessId"]);
            }

            return parentPid;
        }


        public void Add(string filename)
        {
            SendCommand(VlcCommand.Add, filename);
        }


        public void Enqueue(string filename)
        {
            SendCommand(VlcCommand.Enqueue, filename);
        }


        public void Play()
        {
            SendCommand(VlcCommand.Play);
        }

        public string Adev()
        {
            SendCommand(VlcCommand.Adev);
            return WaitForResult().Trim();
        }

        public string SendRaw(string command, string param = null)
        {
            SendRawCommand(command, param);
            return WaitForResult().Trim();
        }


        public void GoToFullScreen()
        {
            SendCommand(VlcCommand.F, "on");
        }


        public void FastForward()
        {
            SendCommand(VlcCommand.FastForward);
        }


        public void Rewind()
        {
            SendCommand(VlcCommand.Rewind);
        }


        private string WaitForResult()
        {
            var result = "";

            var start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < WaitTimeout)
            {
                result = ReadTillEnd();

                if (!string.IsNullOrEmpty(result))
                    break;
            }

            return result;
        }


        private void SendCommand(VlcCommand command)
        {
            SendCommand(command, null);
        }


        private void SendCommand(VlcCommand command, string param)
        {
            // flush old stuff

            ReadTillEnd();


            var packet = Enum.GetName(typeof(VlcCommand), command).ToLower();

            if (param != null)
                packet += " " + param;

            packet += Environment.NewLine;


            var buffer = AsciiEncoding.GetBytes(packet);

            _client.GetStream().Write(buffer, 0, buffer.Length);

            _client.GetStream().Flush();


            Trace.Write(packet);
        }

        private void SendRawCommand(string command, string param)
        {
            ReadTillEnd();


            var packet = command.ToLower();

            if (param != null)
                packet += " " + param;

            packet += Environment.NewLine;


            var buffer = AsciiEncoding.GetBytes(packet);

            _client.GetStream().Write(buffer, 0, buffer.Length);

            _client.GetStream().Flush();


            Trace.Write(packet);
        }

        public string ReadTillEnd()
        {
            var sb = new StringBuilder();

            while (_client.GetStream().DataAvailable)
            {
                var b = _client.GetStream().ReadByte();

                if (b >= 0)
                    sb.Append((char) b);
                else
                    break;
            }

            return sb.ToString();
        }
    }
}