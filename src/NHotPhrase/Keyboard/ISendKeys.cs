﻿using System;
using System.Collections.Generic;
using NHotPhrase.Phrase;

namespace NHotPhrase.Keyboard
{
    public interface ISendKeys
    {
        Guid ID { get; }
        bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keys);
        bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep);
        bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep);
    }
}