using System;

namespace RequestifyTF2.API
{
    public class Events
    {
        public delegate void PlayerChatHandler(PlayerChatArgs e);

        public delegate void PlayerConnectHandler(PlayerConnectArgs e);

        public delegate void PlayerKillHandler(PlayerKillArgs e);

        public delegate void PlayerSuicideHandler(PlayerSuicideArgs e);

        public delegate void UndefinedMessageHandler(UndefinedMessageArgs e);

       public static class PlayerChat
        {
            public static event PlayerChatHandler OnPlayerChat;

            public static void Invoke(RequestifyTF2.API.User caller, string text)
            {
                var e = new PlayerChatArgs(caller, text);
                OnChat(e);
            }

            private static void OnChat(PlayerChatArgs e)
            {
                OnPlayerChat?.Invoke(e);
            }
        }

        public  class PlayerChatArgs : EventArgs
        {
            // Constructor. 
            public PlayerChatArgs(RequestifyTF2.API.User caller, string text)
            {
                User = caller;
                Chat = text;
            }

            public string Chat { get; set; } = string.Empty;

            // Properties. 
            public RequestifyTF2.API.User User { get; set; }
        }

        public static class PlayerConnect
        {
            public static event PlayerConnectHandler OnPlayerConnect;

            public static void Invoke(string nickname)
            {
                var e = new PlayerConnectArgs(nickname);
                OnConnect(e);
            }

            private static void OnConnect(PlayerConnectArgs e)
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

        public static class PlayerKill
        {
            public static event PlayerKillHandler OnPlayerKill;

            public static void Invoke(string killer, string killed, string weapon, bool crit = false)
            {
                var e = new PlayerKillArgs(killer, killed, weapon, crit);
                OnKill(e);
            }

           private static void OnKill(PlayerKillArgs e)
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

        public static class PlayerSuicide
        {
            public static event PlayerSuicideHandler OnPlayerSuicide;

            internal static void Invoke(string nickname)
            {
                var e = new PlayerSuicideArgs(nickname);
                OnSuicide(e);
            }

            private static void OnSuicide(PlayerSuicideArgs e)
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

        public static class UndefinedMessage
        {
            public static event UndefinedMessageHandler OnUndefinedMessage;

            internal static void Invoke(string nickname)
            {
                var e = new UndefinedMessageArgs(nickname);
                OnSuicide(e);
            }

            private static void OnSuicide(UndefinedMessageArgs e)
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