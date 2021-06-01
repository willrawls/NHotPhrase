namespace NHotPhrase.Keyboard
{
    public static class Extensions
    {
        public static string[] SplitInTwo(this string target)
        {
            var index = (int) target.Length / 2;
            return new[]
            {
                target[..index],
                target[index..],
            };
        }
    }
}