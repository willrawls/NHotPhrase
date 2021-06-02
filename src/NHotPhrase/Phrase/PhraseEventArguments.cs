using System;
using System.Collections.Generic;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public class PhraseEventArguments : EventArgs
    {
        public bool Handled { get; set; }
        public PhraseAction Action { get; set; }
        public PhraseActionRunState State { get; set; }
        public List<PKey> KeysToSend { get; set; }
        public string Name => State?.KeySequence?.Name;

        public PhraseEventArguments(PhraseAction action, PhraseActionRunState state, List<PKey> keysToSend)
        {
            Action = action;
            State = state;
            KeysToSend = keysToSend;
        }
    }
}