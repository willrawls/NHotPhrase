namespace NHotPhrase.Phrase
{
    public class PhraseActionRunState
    {
        public KeySequence KeySequence { get; set; }
        public MatchResult MatchResult { get; set; }

        public PhraseActionRunState(KeySequence keySequence, MatchResult matchResult)
        {
            KeySequence = keySequence;
            MatchResult = matchResult;
        }
    }
}