namespace NHotPhrase.Keyboard
{
    public class SendPKeyEntry
    {
        public string Name { get; }
        public int Number { get; }
        public string ReplaceWith { get; }

        public SendPKeyEntry(string name, int number, string replaceWith = null)
        {
            Name = name;
            Number = number;
            ReplaceWith = replaceWith;
        }
    }
}