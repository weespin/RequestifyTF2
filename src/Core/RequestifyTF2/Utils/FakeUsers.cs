using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestifyTF2.Utils
{
  
   public static class FakeUsers
   {
       static FakeUsers()
       {
            /*
             * hostname: Team Fortress
              version : 5097991/24 5097991 secure
              udp/ip  : 169.254.15.182:27015  (public ip: 46.33.32.163)
              steamid : [A:1:2114265096:12570] (90125982400585736)
              map     : itemtest at: 570 x, -468 y, -126 z
              account : not logged in  (No account specified)
              tags    : 
              players : 1 humans, 0 bots (24 max)
              edicts  : 132 used of 2048 max
              
             */
            UserList = new List<string>();
            UserList.Add("hostname");
            UserList.Add("version");
            UserList.Add("udp/ip ");
            UserList.Add("steamid");
            UserList.Add("account");
            UserList.Add("map    ");
            UserList.Add("tags");
            UserList.Add("edicts ");
            UserList.Add("players");
        }
       public static List<string> UserList;

    }
}
