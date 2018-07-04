using System;

namespace RequestifyTF2.Api
{
    public class Events
    {
        public delegate void PlayerChatHandler(PlayerChatArgs e);

        public delegate void PlayerConnectHandler(PlayerConnectArgs e);

        public delegate void PlayerKillHandler(PlayerKillArgs e);

        public delegate void PlayerSuicideHandler(PlayerSuicideArgs e);

        public delegate void UndefinedMessageHandler(UndefinedMessageArgs e);

        public class PlayerChat
        {
            public static event PlayerChatHandler OnPlayerChat;

            public static void Invoke(string caller, string text)
            {
                var e = new PlayerChatArgs(caller, text);
                OnChat(e);
            }

            protected static void OnChat(PlayerChatArgs e)
            {
                OnPlayerChat?.Invoke(e);
            }
        }

        public class PlayerChatArgs : EventArgs
        {
            // Constructor. 
            public PlayerChatArgs(string caller, string text)
            {
                User = caller;
                Chat = text;
            }

            public string Chat { get; set; } = string.Empty;

            // Properties. 
            public string User { get; set; } = string.Empty;
        }

        public class PlayerConnect
        {
            public static event PlayerConnectHandler OnPlayerConnect;

            public static void Invoke(string nickname)
            {
                var e = new PlayerConnectArgs(nickname);
                OnConnect(e);
            }

            protected static void OnConnect(PlayerConnectArgs e)
            {
                OnPlayerConnect?.Invoke(e);
            }
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

        public class PlayerKill
        {
            public static event PlayerKillHandler OnPlayerKill;

            public static void Invoke(string killer, string killed, string weapon, bool crit = false)
            {
                var e = new PlayerKillArgs(killer, killed, weapon, crit);
                OnKill(e);
            }

            protected static void OnKill(PlayerKillArgs e)
            {
                OnPlayerKill?.Invoke(e);
            }
        }

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

        public class PlayerSuicide
        {
            public static event PlayerSuicideHandler OnPlayerSuicide;

            public static void Invoke(string nickname)
            {
                var e = new PlayerSuicideArgs(nickname);
                OnSuicide(e);
            }

            protected static void OnSuicide(PlayerSuicideArgs e)
            {
                OnPlayerSuicide?.Invoke(e);
            }
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

        public class UndefinedMessage
        {
            public static event UndefinedMessageHandler OnUndefinedMessage;

            public static void Invoke(string nickname)
            {
                var e = new UndefinedMessageArgs(nickname);
                OnSuicide(e);
            }

            protected static void OnSuicide(UndefinedMessageArgs e)
            {
                OnUndefinedMessage?.Invoke(e);
            }
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
    }
}