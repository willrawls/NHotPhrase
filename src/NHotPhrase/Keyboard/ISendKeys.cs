using System.Collections.Generic;
using NHotPhrase.Phrase;

namespace NHotPhrase.Keyboard
{
    public interface ISendKeys
    {
        int MillisecondsBetweenKeyPress { get; set; }
        bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keys);
        bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep = -1);
        bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep = -1);
    }
}