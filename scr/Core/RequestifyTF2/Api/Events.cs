using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestifyTF2.Api
{
    public class Events
    {
        public delegate void PlayerConnectHandler(PlayerConnectArgs e);
        public class PlayerConnect
        {
            public static void Invoke(string nickname)
            {


                PlayerConnectArgs e = new PlayerConnectArgs(nickname);
                OnConnect(e);
            }

            public static event PlayerConnectHandler OnPlayerConnect;

            protected static void OnConnect(PlayerConnectArgs e)
            {
                OnPlayerConnect?.Invoke( e);
            }
        }
        public class PlayerConnectArgs : EventArgs
        {
         
            private string text = "";

            // Constructor. 
            public PlayerConnectArgs(string nickname)
            {
                this.text = nickname;
               
            }

            // Properties. 
            public string NickName
            {
                get { return text; }
                set { this.text = value; }
            }

           
        }
        public delegate void PlayerSuicideHandler(PlayerSuicideArgs e);
        public class PlayerSuicide
        {
            public static void Invoke(string nickname)
            {


                PlayerSuicideArgs e = new PlayerSuicideArgs(nickname);
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

            private string text = "";

            // Constructor. 
            public PlayerSuicideArgs(string nickname)
            {
                this.text = nickname;

            }

            // Properties. 
            public string NickName
            {
                get { return text; }
                set { this.text = value; }
            }


        }
        public delegate void PlayerKillHandler(PlayerKillArgs e);
        public class PlayerKill
        {
            public static void Invoke(string killer,string killed,string weapon)
            {


                PlayerKillArgs e = new PlayerKillArgs(killer,killed,weapon);
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

            private string killer = "";
            private string killed = "";
            private string weapon = "";
            // Constructor. 
            public PlayerKillArgs(string killer, string killed, string weapon)
            {
                this.killer = killer;
                this.weapon = weapon;
                this.killed = killed;

            }

            // Properties. 
            public string Killer
            {
                get { return killer; }
                set { this.killer = value; }
            }
            public string Killed
            {
                get { return killed; }
                set { this.killed = value; }
            }
            public string Weapon
            {
                get { return weapon; }
                set { this.weapon = value; }
            }


        }
        public delegate void PlayerChatHandler(PlayerChatArgs e);
        public class PlayerChat
        {
            public static void Invoke(string caller, string text)
            {


                PlayerChatArgs e = new PlayerChatArgs(caller,text);
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

            private string caller = "";
            private string text = "";
        
            // Constructor. 
            public PlayerChatArgs(string caller, string text)
            {
                this.caller = caller;
                this.text = text;
              

            }

            // Properties. 
            public string User
            {
                get { return caller; }
                set { this.caller = value; }
            }
            public string Chat
            {
                get { return text; }
                set { this.text = value; }
            }
           


        }

    }
}
