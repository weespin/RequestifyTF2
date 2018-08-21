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

using NLog;
using NLog.Config;
using NLog.Targets;

public static class Logger
{
    static Logger()
    {
        var config = new LoggingConfiguration();

        var logfile = new FileTarget("logfile")
        {
            FileName = "log.txt",

            Layout =
                @"[${date:format=yyyy-MM-dd HH\:mm\:ss.fff}] | ${callsite:className=true:includeSourcePath=false:methodName=true} | ${level:uppercase=true} | ${message} | ${exception}"
        };
        var logconsole = new ConsoleTarget("logconsole");
        config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
        config.AddRuleForAllLevels(logfile);
        LogManager.Configuration = config;
    }


    public static NLog.Logger Nlogger = LogManager.GetCurrentClassLogger();
}