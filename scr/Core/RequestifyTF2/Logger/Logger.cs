using System;
using System.IO;

public class Logger
{
    public enum Status
    {
        Code,
        Error,
        Info,
        STATUS
    }

    public static void Write(Status status, string text, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine("[" + status + "][" + DateTime.Now + "] " + text);
        Console.ForegroundColor = ConsoleColor.White;
        Log("[" + status + "][" + DateTime.Now + "] " + text);
    }

    public static void Write(Status status, string text)
    {
        Console.WriteLine("[" + status + "][" + DateTime.Now + "] " + text);

        Log("[" + status + "][" + DateTime.Now + "] " + text);
    }

    public void Write(string text)
    {
        Console.WriteLine("[" + DateTime.Now + "] " + text);

        Log("[" + DateTime.Now + "] " + text);
    }


    public static void Log(string message)
    {
    }

    public class LogWriter
    {
        private string _mExePath = string.Empty;


        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}");
                txtWriter.WriteLine($"{logMessage}");
            }
            catch (Exception)
            {
            }
        }
    }
}