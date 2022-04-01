using System;
using System.Collections.Generic;
using NHotPhrase.Phrase;

namespace NHotPhrase.Keyboard
{
    public interface ISendKeys : IDisposable
    {
        Guid ID { get; }
        int SplitLength { get; set; }
        KeyboardManager Keyboard { get; set; }
        KeyHistory History { get; set; }

        bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keys);
        bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep = 1);
        bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep = 1);
        bool SendKeysAndWait(List<PKey> keys, int millisecondThreadSleep = 1);

        List<string> MakeReadyForSending(string target, int splitLength = 8, bool sendAsIs = false);

        void SendString(string textToSend, int millisecondsBetweenKeys = 1, bool sendAsIs = false);
        void SendStrings(IList<string> textPartsToSend, int millisecondsBetweenKeys = 1);
        void SendBackspaces(int backspaceCount, int millisecondsBetweenKeys = 1);
    }
}