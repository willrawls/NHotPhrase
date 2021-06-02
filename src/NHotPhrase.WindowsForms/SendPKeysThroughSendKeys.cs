using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms
{
    public class SendPKeysThroughSendKeys : ISendKeys
    {
        public int MillisecondsBetweenKeyPress { get; set; } = 2;

        private SendPKeysThroughSendKeys()
        {
            ForSendingKeys.Singleton = this;
        }

        public bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keysToSend)
        {
            if (keysToSend is not {Count: > 0}) 
                return true;
            foreach (var key in keysToSend)
                SendKeys.SendWait(key.KeyToSendKeyText());
            return true;
        }

        public bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep = 2)
        {
            SendKeys.SendWait(stringToSend);
            if(millisecondThreadSleep > 0)
                Thread.Sleep(millisecondThreadSleep);
            return true;
        }

        public bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep = 2)
        {
            foreach (var part in stringsToSend) 
                SendKeysAndWait(part, 2);
            return true;
        }

        private static ISendKeys _singleton;
        public static ISendKeys Singleton => _singleton ??= new SendPKeysThroughSendKeys();
    }
}