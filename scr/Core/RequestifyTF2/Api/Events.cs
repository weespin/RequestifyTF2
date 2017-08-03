using System;

namespace RequestifyTF2.Api
{
    public class Events
    {
        public delegate void PlayerChatHandler(PlayerChatArgs e);

        public delegate void PlayerConnectHandler(PlayerConnectArgs e);

        public delegate void PlayerKillHandler(PlayerKillArgs e);

        public delegate void PlayerSuicideHandler(PlayerSuicideArgs e);

        public class PlayerConnect
        {
            public static void Invoke(string nickname)
            {
                var e = new PlayerConnectArgs(nickname);
                OnConnect(e);
            }

            public static event PlayerConnectHandler OnPlayerConnect;

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
            public string NickName { get; set; } = "";
        }

        public class PlayerSuicide
        {
            public static void Invoke(string nickname)
            {
                var e = new PlayerSuicideArgs(nickname);
                OnSuicide(e);
            }

            public static event PlayerSuicideHandler OnPlayerSuicide;

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
            public string NickName { get; set; } = "";
        }

        public class PlayerKill
        {
            public static void Invoke(string killer, string killed, string weapon, bool crit = false)
            {
                var e = new PlayerKillArgs(killer, killed, weapon,crit);
                OnKill(e);
            }

            public static event PlayerKillHandler OnPlayerKill;

            protected static void OnKill(PlayerKillArgs e)
            {
                OnPlayerKill?.Invoke(e);
            }
        }

        public class PlayerKillArgs : EventArgs
        {
          

            // Constructor. 
            public PlayerKillArgs(string killer, string killed, string weapon,bool crit = false)
            {
                Killer = killer;
                Crit = crit;
                Weapon = weapon;
                Killed = killed;
            }

            // Properties. 
            public string Killer { get; set; } = "";

            public bool Crit { get; set; } = false;

            public string Killed { get; set; } = "";

            public string Weapon { get; set; } = "";
        }

        public class PlayerChat
        {
            public static void Invoke(string caller, string text)
            {
                var e = new PlayerChatArgs(caller, text);
                OnKill(e);
            }

            public static event PlayerChatHandler OnPlayerChat;

            protected static void OnKill(PlayerChatArgs e)
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

            // Properties. 
            public string User { get; set; } = "";

            public string Chat { get; set; } = "";
        }
    }
}