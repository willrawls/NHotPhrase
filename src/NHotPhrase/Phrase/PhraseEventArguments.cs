using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NHotPhrase.Phrase
{
    public class PhraseEventArguments : EventArgs
    {
        public bool Handled { get; set; }
        public PhraseAction Action { get; set; }
        public PhraseActionRunState State { get; set; }
        public List<Keys> KeysToSend { get; set; }
        public string Name => State?.KeySequence?.Name;

        public PhraseEventArguments(PhraseAction action, PhraseActionRunState state, List<Keys> keysToSend)
        {
            Action = action;
            State = state;
            KeysToSend = keysToSend;
        }
    }
}