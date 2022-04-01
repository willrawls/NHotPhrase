using System;
using System.Collections.Generic;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public class PhraseAction
    {
        public KeySequence Parent { get; set; }
        public EventHandler<PhraseEventArguments> Handler { get; set; }
        public List<PKey> KeysToSend { get; set; }
        public int MillisecondPauseBetweenKeys { get; set; } = 1;

        public static ISendKeys SendKeysProxy { get; set; }

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
            var keysToSend = new List<PKey>();
            if(KeysToSend is {Count: > 0})
                keysToSend.AddRange(KeysToSend);

            if (Handler != null)
            {
                var hotPhraseEventArgs = new PhraseEventArguments(this, phraseActionRunState, keysToSend);
                Handler(this, hotPhraseEventArgs);

                keysToSend = new List<PKey>();
                if(hotPhraseEventArgs.KeysToSend is {Count: > 0})
                    keysToSend.AddRange(hotPhraseEventArgs.KeysToSend);
            }

            SendKeysProxy?.SendKeysAndWait(phraseActionRunState, keysToSend);

            return true;
        }
    }
}