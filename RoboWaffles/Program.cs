using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using TweetSharp;

namespace Trendster_Bot
{
    class Program
    {        
        static void Main(string[] args)
        {
            var lastReplyToId = TwitterClient.LastTweetReplyToId();
            if (!lastReplyToId.HasValue)
            {
                Console.WriteLine("No last reply ID");
                return;
            }

            var mention = TwitterClient.Mentions().FirstOrDefault();
            if (mention == null)
            {
                Console.WriteLine("No mention");
                return;
            }

            if (mention.Id == lastReplyToId.Value)
            {
                Console.WriteLine("No new mention");
                return;
            }

            var query = mention.Text.ToLower().Replace(ConfigurationSettings.AppSettings["TwitterName"], string.Empty).Trim(); 
                             
            Tweet(mention, query);
        }

        public static void Tweet(TwitterStatus mention, string query)
        {
            var search = new TwitterSearch
            {
                Query = query,
                Count = 100,
            };

            List<string> statusStrings = TwitterClient.GetCleanTweets(search);

            var markovChain = new MarkovChain(ConfigurationSettings.AppSettings["MarkovOrder"].ParseInt(1), statusStrings);

            var tweet = String.Format("@{0}: {1}", mention.Author.ScreenName, markovChain.WalkChain(null, (140 - 3 - mention.Author.ScreenName.Length)));

            Console.WriteLine("Tweeting: " + tweet);
            TwitterClient.SendTweet(tweet, mention);
        }
    }
}