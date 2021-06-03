using System;
using System.Collections.Generic;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public class KeySequence
    {
        public string Name { get; set; }

        public List<PKey> Sequence = new();
        
        public WildcardMatchType WildcardMatchType { get; set; }
        public int WildcardCount { get; set; }
        
        public PhraseActionList ActionList { get; set; } = new();


        public KeySequence(string name, List<PKey> keys, EventHandler<PhraseEventArguments> hotPhraseEventArgs)
        {
            Name = name;
            Sequence.AddRange(keys);
            ThenCall(hotPhraseEventArgs);
        }

        public KeySequence()
        {
        }

        public static KeySequence Named(string name)
        {
            return new()
            {
                Name = name
            };
        }

        public static KeySequence Factory()
        {
            return new()
            {
                Name = Guid.NewGuid().ToString()
            };
        }

        public KeySequence WhenKeyRepeats(PKey repeatPKey, int repeatCount)
        {
            for (var i = 0; i < repeatCount; i++) Sequence.Add(repeatPKey);
            return this;
        }

        public KeySequence WhenKeyReleased(PKey pKey)
        {
            Sequence.Add(pKey);
            return this;
        }

        public KeySequence WhenKeysReleased(IList<PKey> keys)
        {
            Sequence.AddRange(keys);
            return this;
        }

        public bool Run(MatchResult matchResult)
        {
            var state = new PhraseActionRunState(this, matchResult);
            foreach (var action in ActionList)
            {
                if (!action.RunNow(state))
                    return false;
            }
            return true;
        }

        public bool IsAMatch(List<PKey> keyList, out MatchResult matchResult)
        {
            matchResult = null;

            var sequencePlusWildcardCount = Sequence.Count + WildcardCount;
            if (keyList.Count < sequencePlusWildcardCount)
                return false;

            var possibleMatchRange = keyList.Count == sequencePlusWildcardCount
                ? keyList
                : keyList.GetRange(keyList.Count - sequencePlusWildcardCount, sequencePlusWildcardCount);

            for (var i = 0; i < Sequence.Count; i++)
            {
                if (!Sequence[i].IsAMatch(possibleMatchRange[i]))
                    return false;
            }

            if (WildcardMatchType is WildcardMatchType.Unknown or WildcardMatchType.None 
                || WildcardCount < 1) 
                return true;

            var possibleWildcardRange = keyList.Count == WildcardCount
                ? keyList
                : keyList.GetRange(keyList.Count - WildcardCount, WildcardCount);

            if (possibleWildcardRange.Count != WildcardCount)
                return true;

            switch (WildcardMatchType)
            {
                case WildcardMatchType.Digits:
                    if (possibleWildcardRange.OnlyDigits())
                    {
                        matchResult = new MatchResult(this, possibleWildcardRange.AsString());
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case WildcardMatchType.Letters:
                    if (possibleWildcardRange.OnlyLetters())
                    {
                        matchResult = new MatchResult(this, possibleWildcardRange.AsString());
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case WildcardMatchType.AlphaNumeric:
                    if (possibleWildcardRange.OnlyLetters()
                        || possibleWildcardRange.OnlyDigits()
                    )
                    {
                        matchResult = new MatchResult(this, possibleWildcardRange.AsString());
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case WildcardMatchType.NotAlphaNumeric:
                    if (!possibleWildcardRange.OnlyLetters() 
                        && possibleWildcardRange.OnlyDigits()
                    )
                    {
                        matchResult = new MatchResult(this, possibleWildcardRange.AsString());
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case WildcardMatchType.Anything:
                    matchResult = new MatchResult(this, possibleWildcardRange.AsString());
                    break;
            }

            return true;
        }

        public KeySequence ThenCall(EventHandler<PhraseEventArguments> handler)
        {
            var sequence = new PhraseAction(this, handler);
            ActionList.Add(sequence);
            return this;
        }

        public KeySequence WhenKeyPressed(PKey pKey)
        {
            Sequence.Clear();
            Sequence.Add(pKey);
            return this;
        }

        public KeySequence WhenKeysPressed(List<PKey> keys)
        {
            Sequence.Clear();
            Sequence.AddRange(keys);
            return this;
        }

        public KeySequence ThenKeyPressed(PKey pKey)
        {
            Sequence.Add(pKey);
            return this;
        }

        public KeySequence FollowedByWildcards(WildcardMatchType wildcardMatchType, int wildcardCount)
        {
            WildcardMatchType = wildcardMatchType;
            WildcardCount = wildcardCount;
            return this;
        }
    }
}