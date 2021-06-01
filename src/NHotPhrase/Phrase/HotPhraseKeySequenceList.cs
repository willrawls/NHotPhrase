using System.Collections.Generic;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public class HotPhraseKeySequenceList : List<HotPhraseKeySequence>
    {
        public static readonly object SyncRoot = new();
        public HotPhraseKeySequence FirstMatch(KeyHistory history, out MatchResult matchResult)
        {
            lock (SyncRoot)
            {
                var cloneOfHistory = history.KeyList();
                matchResult = null;
                HotPhraseKeySequence result = null;
                foreach (var trigger in this)
                {
                    if (!trigger.IsAMatch(cloneOfHistory, out var matchResultFromIsAMatch)) continue;

                    matchResult = matchResultFromIsAMatch;
                    result = trigger;
                    break;
                }
                return result;
            }
        }
    }
}