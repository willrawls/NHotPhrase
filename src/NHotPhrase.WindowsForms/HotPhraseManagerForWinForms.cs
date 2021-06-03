using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms
{
    public class HotPhraseManagerForWinForms : HotPhraseManager, ISendKeys
    {
        public HotPhraseManagerForWinForms() 
        {
            SendPKeys.Singleton = this;
        }

        public bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keysToSend)
        {
            if (keysToSend is not {Count: > 0}) 
                return true;
            foreach (var key in keysToSend)
                SendKeys.SendWait(ToSendKeysText(key));
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

        public bool SendKeysAndWait(List<PKey> keysToSend, int millisecondThreadSleep)
        {
            if (keysToSend is not {Count: > 0}) 
                return true;
            foreach (var key in keysToSend)
                SendKeys.SendWait(ToSendKeysText(key));
            return true;
        }

        public static string ToSendKeysText(PKey pKey)
        {
            var keyword = SendPKeys.SendKeyEntries.FirstOrDefault(k => k.Number == (int) pKey);
            return keyword != null 
                ? keyword.SendKeysText()
                : pKey.ToString();
        }
    }
}