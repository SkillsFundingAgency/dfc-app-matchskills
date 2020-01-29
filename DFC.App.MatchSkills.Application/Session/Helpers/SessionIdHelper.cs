using System;
using System.Collections.Generic;
using System.Text;
using HashidsNet;

namespace DFC.App.MatchSkills.Application.Session.Helpers
{
    public static class SessionIdHelper
    {
        private const string Alphabet = "acefghjkmnrstwxyz23456789";
        public static string GenerateSessionId(string salt, DateTime date)
        {
            var hashids = new Hashids(salt, 4, Alphabet);
            int rand = Counter();
            string year = (date.Year - 2018).ToString();
            long digits = Convert.ToInt64($"{year}{date.ToString("MMddHHmmssfff")}{rand}");
            var code = hashids.EncodeLong(digits);
            var decode = Decode(salt, code);
            if (digits.ToString() != decode)
            {
                throw new Exception("Invalid decode");
            }
            return code;
        }
        public static string GenerateSessionId(string salt) => GenerateSessionId(salt, DateTime.UtcNow);

        public static string Decode(string salt, string code)
        {
            var hashids = new Hashids(salt, 4, Alphabet);
            var decode = hashids.DecodeLong(code);
            if (decode.Length > 0)
            {
                return decode[0].ToString();
            }
            return null;
        }

        private static int _counter = 10;
        private static readonly object _syncLock = new object();
        public static int Counter()
        {
            lock (_syncLock)
            {
                if (_counter >= 99) _counter = 0;
                _counter++;
                return _counter;
            }
        }
    }
}
