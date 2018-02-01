using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RequestifyTF2.Api;

namespace RequestifyTF2.Commands
{
    internal class Executer
    {
        public static void Execute(string caller, string command, List<string> arguments)
        {
            var calledcommand = Instance.ActivePlugins.FirstOrDefault(n => n.Command == command);
            if (calledcommand == null)
            {
                var argstostring = "";
                if (arguments.Count > 0)
                    argstostring = string.Join(" ", argstostring.ToArray());
                Events.PlayerChat.Invoke(caller, command + " " + argstostring);
            }
            else
            {

                if (!Instance.Config.Ignored.Contains(caller))
                {

                    if (!Instance.Config.IgnoredReversed)
                    {
                        Task.Run(() =>
                        {
                            try
                            {


                                calledcommand.Execute(caller, arguments);
                            }
                            catch (Exception e)
                            {
                                Logger.Write(Logger.Status.Error,e.ToString());
                            }
                        });
                        Logger.Write(Logger.Status.Info, $"{caller} executed {command}");
                    }
                    else
                    {
                        Logger.Write(Logger.Status.Info, $"{caller} was blacklisted to execute {command}");
                    }
                }
                else
                {
                    if (Instance.Config.IgnoredReversed)
                    {

                        Task.Run(() =>
                        {
                            try
                            {


                                calledcommand.Execute(caller, arguments);
                            }
                            catch (Exception e)
                            {
                                Logger.Write(Logger.Status.Error, e.ToString());
                            }
                        });
                        Logger.Write(Logger.Status.Info, $"{caller} invoked {command}");
                    }
                    else
                    {
                        Logger.Write(Logger.Status.Info, $"{caller} was blacklisted to execute {command}");
                    }
                }
            }
        }
    }
}