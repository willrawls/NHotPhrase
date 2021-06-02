using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms
{
    public class SendPKeysThroughSendKeys : ISendKeys
    {
        static SendPKeysThroughSendKeys()
        {
            SendPKeys.Singleton = new SendPKeysThroughSendKeys();
        }

        public bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keysToSend)
        {
            if (keysToSend is not {Count: > 0}) 
                return true;
            foreach (var key in keysToSend)
                SendKeys.SendWait(key.PKeyToSendKeysText());
            return true;
        }

        public bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep)
        {
            SendKeys.SendWait(stringToSend);
            if(millisecondThreadSleep > 0)
                Thread.Sleep(millisecondThreadSleep);
            return true;
        }

        public bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep)
        {
            foreach (var part in stringsToSend) 
                SendKeysAndWait(part, 2);
            return true;
        }
    }
}