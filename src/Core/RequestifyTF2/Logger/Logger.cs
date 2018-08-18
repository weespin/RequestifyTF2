using System;
using System.IO;
using System.Linq;
using NLog;
using RequestifyTF2.API;

public static class Logger
{
    static Logger()
    {

        var config = new NLog.Config.LoggingConfiguration();

        var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "log.txt",
            
            Layout = @"[${date:format=yyyy-MM-dd HH\:mm\:ss.fff}] | ${callsite:className=true:includeSourcePath=false:methodName=true} | ${level:uppercase=true} | ${message} | ${exception}"
        };
        var logconsole = new NLog.Targets.ConsoleTarget("logconsole");
        config.AddRule(LogLevel.Info, LogLevel.Fatal,logconsole);
        config.AddRuleForAllLevels(logfile);
        LogManager.Configuration = config;

    }

  
    public static NLog.Logger Nlogger = LogManager.GetCurrentClassLogger();
   
}

