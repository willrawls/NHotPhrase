using System;
using System.Collections.Generic;
using Enum = System.Enum;

namespace NHotPhrase.Keyboard
{
    public class SendKeyEntry
    {
        public string Name { get; }
        public int Number { get; }
        public string ReplaceWith { get; }

        public SendKeyEntry(string name, int number, string replaceWith = null)
        {
            Name = name;
            Number = number;
            ReplaceWith = replaceWith;
        }
    }
}