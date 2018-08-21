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

namespace RequestifyTF2.API.Events
{
    public class Events
    {
        #region Deletgates

        public delegate void PlayerChatHandler(RequestifyEventArgs.PlayerChatArgs e);

        public delegate void PlayerConnectHandler(RequestifyEventArgs.PlayerConnectArgs e);

        public delegate void PlayerKillHandler(RequestifyEventArgs.PlayerKillArgs e);

        public delegate void PlayerSuicideHandler(RequestifyEventArgs.PlayerSuicideArgs e);

        public delegate void UndefinedMessageHandler(RequestifyEventArgs.UndefinedMessageArgs e);

        public delegate void CommandRegisteredHandler(RequestifyEventArgs.CommandRegisteredArgs e);

        public delegate void PluginLoadedHandler(RequestifyEventArgs.PluginLoadedArgs e);

        #endregion

        #region Events

        public static event PlayerChatHandler OnPlayerChat;
        public static event PlayerConnectHandler OnPlayerConnect;
        public static event PlayerKillHandler OnPlayerKill;
        public static event PlayerSuicideHandler OnPlayerSuicide;
        public static event UndefinedMessageHandler OnUndefinedMessage;
        public static event PluginLoadedHandler OnPluginLoaded;
        public static event CommandRegisteredHandler OnCommandRegistered;

        #endregion

        public static class PlayerChat
        {
            public static void Invoke(User caller, string text)
            {
                var e = new RequestifyEventArgs.PlayerChatArgs(caller, text);
                OnChat(e);
            }

            private static void OnChat(RequestifyEventArgs.PlayerChatArgs e)
            {
                Logger.Nlogger.Debug($"Invoked PlayerChat. User: {e.User.Name} Message: {e.Chat}");
                if (OnPlayerChat != null)
                {
                    OnPlayerChat(e);
                }
            }
        }


        public static class PlayerConnect
        {
            public static void Invoke(string nickname)
            {
                var e = new RequestifyEventArgs.PlayerConnectArgs(nickname);
                OnConnect(e);
            }

            private static void OnConnect(RequestifyEventArgs.PlayerConnectArgs e)
            {
                Logger.Nlogger.Debug($"Invoked PlayerConnect. User: {e.NickName}");
                OnPlayerConnect?.Invoke(e);
            }
        }


        public static class PlayerKill
        {
            public static void Invoke(string killer, string killed, string weapon, bool crit = false)
            {
                var e = new RequestifyEventArgs.PlayerKillArgs(killer, killed, weapon, crit);
                OnKill(e);
            }

            private static void OnKill(RequestifyEventArgs.PlayerKillArgs e)
            {
                Logger.Nlogger.Debug(
                    $"Invoked PlayerKill. Killer: {e.Killer} Weapon: {e.Weapon} Killed: {e.Killed} Crit: {e.Crit}");
                OnPlayerKill?.Invoke(e);
            }
        }

        public static class PlayerSuicide
        {
            internal static void Invoke(string nickname)
            {
                var e = new RequestifyEventArgs.PlayerSuicideArgs(nickname);
                OnSuicide(e);
            }

            private static void OnSuicide(RequestifyEventArgs.PlayerSuicideArgs e)
            {
                Logger.Nlogger.Debug($"Invoked PlayerSuicide. User: {e.NickName}");
                OnPlayerSuicide?.Invoke(e);
            }
        }


        public static class UndefinedMessage
        {
            internal static void Invoke(string nickname)
            {
                var e = new RequestifyEventArgs.UndefinedMessageArgs(nickname);
                OnUndef(e);
            }

            private static void OnUndef(RequestifyEventArgs.UndefinedMessageArgs e)
            {
                Logger.Nlogger.Debug($"Invoked Undefined Message. Message: {e.Message}");
                OnUndefinedMessage?.Invoke(e);
            }
        }


        public static class PluginLoaded
        {
            internal static void Invoke(IRequestifyPlugin Plugin)
            {
                var e = new RequestifyEventArgs.PluginLoadedArgs(Plugin);
                OnPluginLoad(e);
            }

            private static void OnPluginLoad(RequestifyEventArgs.PluginLoadedArgs e)
            {
                Logger.Nlogger.Debug($"Invoked PluginLoaded. Plugin: {e.Plugin.Name}");
                OnPluginLoaded?.Invoke(e);
            }
        }


        public static class CommandRegistered
        {
            internal static void Invoke(IRequestifyCommand Command)
            {
                var e = new RequestifyEventArgs.CommandRegisteredArgs(Command);
                OnCommandRegister(e);
            }

            private static void OnCommandRegister(RequestifyEventArgs.CommandRegisteredArgs e)
            {
                Logger.Nlogger.Debug($"Invoked CommandRegistered. Command: {e.Command.Name}");
                OnCommandRegistered?.Invoke(e);
            }
        }
    }

    public class RequestifyEventArgs
    {
        public class PlayerKillArgs : EventArgs
        {
            // Constructor. 
            public PlayerKillArgs(string killer, string killed, string weapon, bool crit = false)
            {
                Killer = killer;
                Crit = crit;
                Weapon = weapon;
                Killed = killed;
            }

            public bool Crit { get; set; }

            public string Killed { get; set; } = string.Empty;

            // Properties. 
            public string Killer { get; set; } = string.Empty;

            public string Weapon { get; set; } = string.Empty;
        }

        public class PlayerSuicideArgs : EventArgs
        {
            // Constructor. 
            public PlayerSuicideArgs(string nickname)
            {
                NickName = nickname;
            }

            // Properties. 
            public string NickName { get; set; } = string.Empty;
        }

        public class UndefinedMessageArgs : EventArgs
        {
            // Constructor. 
            public UndefinedMessageArgs(string message)
            {
                Message = message;
            }

            // Properties. 
            public string Message { get; set; } = string.Empty;
        }

        public class PluginLoadedArgs : EventArgs
        {
            // Constructor. 
            public PluginLoadedArgs(IRequestifyPlugin plugin)
            {
                Plugin = plugin;
            }

            // Properties. 
            public IRequestifyPlugin Plugin { get; set; }
        }

        public class PlayerConnectArgs : EventArgs
        {
            // Constructor. 
            public PlayerConnectArgs(string nickname)
            {
                NickName = nickname;
            }

            // Properties. 
            public string NickName { get; set; } = string.Empty;
        }

        public class PlayerChatArgs : EventArgs
        {
            // Constructor. 
            public PlayerChatArgs(User caller, string text)
            {
                User = caller;
                Chat = text;
            }

            public string Chat { get; set; } = string.Empty;

            // Properties. 
            public User User { get; set; }
        }

        public class CommandRegisteredArgs : EventArgs
        {
            // Constructor. 
            public CommandRegisteredArgs(IRequestifyCommand command)
            {
                Command = command;
            }

            // Properties. 
            public IRequestifyCommand Command { get; set; }
        }
    }
}