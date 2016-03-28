using Markov;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trendster_Bot
{
    public class MarkovChain
    {
        private MarkovChain<string> _markovModel;

        public MarkovChain(int order, List<string> strings)
        {
            _markovModel = new MarkovChain<string>(order);
            foreach (var statusString in strings)
            {
                _markovModel.Add(statusString.Split(' '));
            }
        }

        public string WalkChain(int? minLength = null, int? maxLength = null)
        {
            return WalkChain(10, minLength, maxLength);
        }

        private string WalkChain(int trys, int? minLength = null, int? maxLength = null)
        {
            if (trys < 0) return string.Empty;
            var candidateString = _markovModel.Chain().Aggregate((x, y) => string.Format("{0} {1}", x, y));

            if (minLength.HasValue && candidateString.Length <= minLength.Value)
                return WalkChain(trys - 1, minLength, maxLength);

            if (maxLength.HasValue && candidateString.Length >= maxLength.Value)
                return WalkChain(trys - 1, minLength, maxLength);

            return candidateString;
        }
    }
}
