using System.Collections.Generic;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public interface ISendKeys
    {
        bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keys);
        bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep = 2);
        bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep = 2);
    }
}