using System;
using System.IO;
using RequestifyTF2.API;

public static class Logger
{
    public enum LogStatus
    {
        Error,
        Info,
        Debug
    }

    private static string logpath => AppDomain.CurrentDomain.BaseDirectory + "\\" + "log.txt";

    public static void Log(IRequestifyPlugin plugin, string message)
    {
        Write(LogStatus.Info, $"{plugin.Name} => {message}");
    }

    public static void LogError(IRequestifyPlugin plugin, string message)
    {
        Write(LogStatus.Error, $"{plugin.Name} => {message}");
    }

    public static void Write(LogStatus status, string text, ConsoleColor color = ConsoleColor.White)
    {
        if (status == LogStatus.Debug && !Requestify.Debug) return;
        Console.ForegroundColor = color;
        LogWrite("[" + status + "][" + DateTime.Now + "] " + text);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void Write(LogStatus status, string text)
    {
        if (status == LogStatus.Debug && !Requestify.Debug) return;
        LogWrite("[" + status + "][" + DateTime.Now + "] " + text);
    }

    public static void Write(string text)
    {

        LogWrite("[" + DateTime.Now + "] " + text);
    }

    private static void Log(string logMessage, TextWriter txtWriter)
    {
        try
        {
            txtWriter.WriteLine($"{logMessage}");
         
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private static void LogWrite(string logMessage)
    {
       
        try
        {
            using (var w = File.AppendText(logpath))
            {
                Console.WriteLine(logMessage);
                Log(logMessage, w);
            }
        }
        catch (Exception )
        {
            // Never used
        }
    }
}