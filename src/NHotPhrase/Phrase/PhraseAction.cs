using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public class PhraseAction
    {
        public KeySequence Parent { get; set; }
        public EventHandler<PhraseEventArguments> Handler { get; set; }
        public List<Keys> KeysToSend { get; set; }
        public int MillisecondPauseBetweenKeys { get; set; } = 2;

        public PhraseAction(KeySequence parent, EventHandler<PhraseEventArguments> handler = null)
        {
            Parent = parent;
            if (handler != null)
                ThenCall(handler);
        }

        public KeySequence ThenCall(EventHandler<PhraseEventArguments> handler)
        {
            Handler = handler;
            return Parent;
        }

        public bool RunNow(PhraseActionRunState phraseActionRunState)
        {
            var keysToSend = new List<Keys>();
            if(KeysToSend is {Count: > 0})
                keysToSend.AddRange(KeysToSend);

            if (Handler != null)
            {
                var hotPhraseEventArgs = new PhraseEventArguments(this, phraseActionRunState, keysToSend);
                Handler(this, hotPhraseEventArgs);

                keysToSend = new List<Keys>();
                if(hotPhraseEventArgs.KeysToSend is {Count: > 0})
                    keysToSend.AddRange(hotPhraseEventArgs.KeysToSend);
            }

            if (keysToSend is not {Count: > 0}) 
                return true;

            foreach (var key in KeysToSend)
                SendKeys.SendWait(key.KeyToSendKeyText());

            return true;
        }
    }
}