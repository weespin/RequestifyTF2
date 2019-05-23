using System;
using System.Collections.Generic;
using System.Linq;
using RequestifyTF2.API;

namespace RequestifyTF2.Utils
{
    public static class SpammerList
    {
        private static readonly List<SpamUser> _spammerlist = new List<SpamUser>();

        public static bool IsBlocked(string user)
        {
            if (user == Instance.Config.Admin)
            {
                return false;
            }

            if (_spammerlist.Any(n => n.nickname == user))
            {
                var spamuser = _spammerlist.First(n => n.nickname == user);
                if ((DateTime.Now - spamuser.bannedtime).TotalMinutes < 5 * spamuser.bantimes)
                {
                    return true;
                }

                if (spamuser.requests.IsOverflowed(Instance.Config.MaximumParsesPerMin))
                {
                    spamuser.bannedtime = DateTime.Now;
                    spamuser.bantimes++;

                    var bantime = (spamuser.bannedtime.AddMinutes(5 * spamuser.bantimes) - DateTime.Now).TotalMinutes
                        .ToString("###");
                    ConsoleSender.SendCommand($"Now {user} is banned for flood. Wait {bantime}",
                        ConsoleSender.Command.Chat);
                    Logger.Write(Logger.Status.Info, $"User {user} got blocked for too frequent messages.");

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
                var objt = new SpamUser {nickname = user};
                objt.requests.RegisterFlood();
                _spammerlist.Add(objt);
            }
        }
    }

    internal class SpamUser
    {
        public DateTime bannedtime;

        public int bantimes;
        //CustomList<DateTime> requests

        public string nickname;
        public RequestStamps requests = new RequestStamps();
    }

    internal class RequestStamps : Queue<long>
    {
        private const int historylimit = 60;
        private long Timestamp => (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

        public void RegisterFlood()
        {
            var timestamp = Timestamp;
            //while (timestamp - this.Min() <= 60)
            //{
            //    this.Dequeue();
            //}

            if (Count > historylimit)
            {
                Dequeue();
            }

            Enqueue(timestamp);
        }
        
        public bool IsOverflowed(int requestspermin)
        {
            if (requestspermin == 0)
            {
                return false;
            }
            var max = this.Max();
            var min = this.Min();
            var time = max - min;
            int num;
            if (time < 5)
            {
                return false;
            }

            if (time > 60)
            {
                var koef = time / 60;
                if (koef == 0)
                {
                    return false;
                }

                num = Count / (int) koef;
            }
            else
            {
                var lim = 60 / time;
                num = (int) lim * Count;
            }

            if (num > requestspermin)
            {
                //avg messages overflow

                return true;
            }

            return false;
        }

        private bool Between(long num, long lower, long upper, bool inclusive = false)
        {
            return inclusive
                ? lower <= num && num <= upper
                : lower < num && num < upper;
        }
    }
}