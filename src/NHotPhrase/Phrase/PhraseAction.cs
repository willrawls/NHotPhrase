using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public class PhraseAction
    {
        public HotPhraseKeySequence Parent { get; set; }
        public EventHandler<PhraseEventArgs> Handler { get; set; }
        public List<Keys> KeysToSend { get; set; }
        public int MillisecondPauseBetweenKeys { get; set; } = 2;

        public PhraseAction(HotPhraseKeySequence parent, EventHandler<PhraseEventArgs> handler = null)
        {
            Parent = parent;
            if (handler != null)
                ThenCall(handler);
        }

        public HotPhraseKeySequence ThenCall(EventHandler<PhraseEventArgs> handler)
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
                var hotPhraseEventArgs = new PhraseEventArgs(this, phraseActionRunState, keysToSend);
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