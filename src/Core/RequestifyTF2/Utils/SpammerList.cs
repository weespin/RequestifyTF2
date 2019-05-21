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
      
        private static List<SpamUser> _spammerlist => new List<SpamUser>();

        public static bool IsBlocked(string user)
        {
            if (user == Instance.Config.Admin)
            {
                return false;
            }
            if (_spammerlist.Any(n => n.nickname==user))
            {
                var spamuser = _spammerlist.First(n => n.nickname == user);
                if (spamuser.blocked)
                {
                    return true;
                }
                if(spamuser.requests.IsOverflowed(Instance.Config.MaximumParsesPerMin))
                {
                    Logger.Write(Logger.Status.Info,$"User {user} got blocked for too frequent messages.");
                    spamuser.blocked = true;
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
                _spammerlist.Add(new SpamUser(){nickname = user});
            }
        }
        
    }

    internal class SpamUser
    {
        //CustomList<DateTime> requests
        public bool blocked;
        public string nickname;
        public RequestStamps requests = new RequestStamps();

       
    }

    class RequestStamps : List<long>
    {
        private const int historylimit = 20;
        private long Timestamp => (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        public void RegisterFlood()
        {
            if (this.Count > historylimit)
            {
                this.RemoveAt(this.Count-1);
                base.Add(Timestamp);
            }
        }

        public bool IsOverflowed(int requestspermin)
        {
            var max = this.Max();
            //calculate messages in total
            var overflow = (this.Max() - this.Min()) / this.Count;
            if (overflow > requestspermin)
            {
                //avg messages overflow
                return true;
            }
            else { 

              return this.Count(n => Between(n, n - 60, max, true)) >= requestspermin;
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
