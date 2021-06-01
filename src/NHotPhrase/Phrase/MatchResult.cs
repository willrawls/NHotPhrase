namespace NHotPhrase.Phrase
{
    public class MatchResult
    {
        public KeySequence KeySequence { get; set; }
        public string Value { get; set; }

        public MatchResult(KeySequence keySequence, string value)
        {
            KeySequence = keySequence;
            Value = value;
        }

        public int ValueAsInt() =>
            int.TryParse(Value, out var valueAsInt) 
                ? valueAsInt 
                : 0;
    }
}