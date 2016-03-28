using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace Trendster_Bot
{
    public class TwitterSearch
    {
        public string Query { get; set; }

        private int _count = 10;
        public int Count { get { return _count; } set { _count = value; } }

        private string _language = "en";
        public string Language { get { return _language; } set { _language = value; } }

        private TwitterSearchResultType _type = TwitterSearchResultType.Popular;
        public TwitterSearchResultType Type { get { return _type; } set { _type = value; } }
    }
}
