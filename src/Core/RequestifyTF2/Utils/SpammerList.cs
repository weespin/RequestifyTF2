using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestifyTF2.API;

namespace RequestifyTF2.Utils
{
  public static class SpammerList
    {
      
        private static List<SpamUser> _spammerlist = new List<SpamUser>();

        public static bool IsBlocked(string user)
        {
            if (user == Instance.Config.Admin)
            {
                return false;
            }
            if (_spammerlist.Any(n => n.nickname==user))
            {
                var spamuser = _spammerlist.First(n => n.nickname == user);
                if ((DateTime.Now-spamuser.bannedtime).TotalMinutes<(5*spamuser.bantimes))
                {
                    return true;
                }

                if(spamuser.requests.IsOverflowed(Instance.Config.MaximumParsesPerMin))
                {
                    spamuser.bannedtime = DateTime.Now;
                    spamuser.bantimes++;

                    var bantime = (spamuser.bannedtime.AddMinutes(5 * spamuser.bantimes) - DateTime.Now).TotalMinutes
                        .ToString("###");
                    ConsoleSender.SendCommand($"Now {user} is banned for fllod. Wait {bantime}", ConsoleSender.Command.Chat);
                    Logger.Write(Logger.Status.Info,$"User {user} got blocked for too frequent messages.");
                   
                    return true;
                }
            }
            return false;
        }

        public static void Messaged(string user)
        {   
            if (_spammerlist.Any(n => n.nickname == user))
            {
              _spammerlist.First(n => n.nickname == user).requests.RegisterFlood();
            }
            else
            {
                var objt = new SpamUser() {nickname = user};
                objt.requests.RegisterFlood();
                _spammerlist.Add(objt);
            }
        }
        
    }

    internal class SpamUser
    {
        //CustomList<DateTime> requests
        
        public string nickname;
        public RequestStamps requests = new RequestStamps();
        public DateTime bannedtime;
        public int bantimes;

    }

    class RequestStamps : Queue<long>
    {
        private const int historylimit = 60;
        private long Timestamp => (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        public void RegisterFlood()
        {
            if (this.Count > historylimit)
            {
                this.Dequeue();
               
            }
            base.Enqueue(Timestamp);
        }

        public bool IsOverflowed(int requestspermin)
        {
            var max = this.Max();
            var min = this.Min();
            //calculate messages in total
           
            var clamp = 60- (max - min);
           
            if (clamp > requestspermin)
            {
                //avg messages overflow
                
                return true;
            }
            else
            {
                var reqpm = this.Count(n => Between(n, n - 60, max, true));
              return reqpm >= requestspermin;
            }
        }

        private bool Between(long num, long lower, long upper, bool inclusive = false)
        {
            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }

    }

}
