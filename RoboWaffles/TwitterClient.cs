using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TweetSharp;

namespace Trendster_Bot
{


    public static class TwitterClient
    {
        private static TwitterService _service;
        private static TwitterService service
        {
            get
            {
                if (_service == null)
                {
                    _service = new TwitterService(
                        ConfigurationSettings.AppSettings["ConsumerKey"],
                        ConfigurationSettings.AppSettings["ConsumerSecret"]);
                    _service.AuthenticateWith(
                        ConfigurationSettings.AppSettings["AccessToken"],
                        ConfigurationSettings.AppSettings["AccessTokenSecret"]);
                }
                return _service;
            }
            set { _service = value; }
        }

        public static TwitterSearchResult Search(TwitterSearch search)
        {
            try
            {
                var statuses = service.Search(new SearchOptions
                {
                    Count = search.Count,
                    Q = search.Query,
                    Lang = search.Language,
                    Resulttype = search.Type
                });
                return statuses;
            }
            catch (Exception e) { return new TwitterSearchResult(); }
        }

        public static List<TwitterStatus> Mentions()
        {
            try
            {
                return service.ListTweetsMentioningMe(new ListTweetsMentioningMeOptions()).ToList();
            }
            catch (Exception e) { return new List<TwitterStatus>(); }
        }

        public static List<string> GetCleanTweets(TwitterSearch search)
        {
            var statuses = Search(search).Statuses.Where(x => !x.IsRetweet());
            List<string> returnList = new List<string>();

            foreach (var status in statuses)
            {
                var text = status.Text;
                text = Regex.Replace(text, "RT:? ?", string.Empty);
                text = Regex.Replace(text, "http[^ ]+ ?", string.Empty);
                text = Regex.Replace(text, "@[^ ]+ ?", string.Empty);
                text = text.Trim();

                if (!returnList.Contains(text))
                {
                    returnList.Add(text);
                }
            }

            return returnList;
        }

        public static void SendTweet(string status, TwitterStatus tweetToReplyTo)
        {
            service.SendTweet(new SendTweetOptions { Status = status, InReplyToStatusId = tweetToReplyTo.Id });
        }

        public static long? LastTweetReplyToId()
        {
            return service.ListTweetsOnHomeTimeline(new ListTweetsOnHomeTimelineOptions { Count = 1 }).FirstOrDefault()?.InReplyToStatusId;
        }
    }
}