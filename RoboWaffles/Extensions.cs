using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace Trendster_Bot
{
    public static class Extensions
    {

        #region SeaSharp

        public static int ParseInt(this string value, int defaultValue = 0)
        {
            var nullableValue = value.ParseNullableInt();
            return nullableValue.HasValue ? nullableValue.Value : defaultValue;
        }

        public static int? ParseNullableInt(this string value)
        {
            int result = 0;
            return int.TryParse(value, out result) ? result : ((int?)null);
        }

        public static TimeSpan AddSeconds(this TimeSpan ts, int seconds)
        {
            return ts.Add(new TimeSpan(0, 0, seconds));
        }        

        #endregion

        #region TweetSharp

        public static bool IsRetweet(this TwitterStatus tweet)
        {
            return tweet.RetweetedStatus != null;
        }

        #endregion
    }
}
