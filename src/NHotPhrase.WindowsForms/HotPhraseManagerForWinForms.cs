using System;
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
            var keyword = SendKeyHelper.Entries.FirstOrDefault(k => k.Number == (int) pKey);
            return keyword != null 
                ? keyword.SendKeysText()
                : pKey.ToString();
        }

        public List<string> MakeReadyForSending(string target, int splitLength = 8)
        {
            if (string.IsNullOrEmpty(target))
                return new List<string>();

            foreach (var keyword in SendKeyHelper.Entries.Where(k => !string.IsNullOrEmpty(k.ReplaceWith)))
                target = target.Replace(keyword.Name, "⌂" + keyword.ReplaceWith + "⌂");

            var list = target.Split('⌂', StringSplitOptions.RemoveEmptyEntries).ToList();
            while (list.Any(p => p.Length > splitLength))
                for (var i = 0; i < list.Count; i++)
                {
                    if (list[i].Length <= splitLength) continue;

                    var pieces = list[i].SplitInTwo();
                    list.RemoveAt(i);
                    list.InsertRange(i, pieces);
                }

            return list;
        }

        public void SendBackspaces(int backspaceCount, int millisecondsBetweenKeys = 2)
        {
            var keys = new List<PKey>();
            for (var i = 0; i < backspaceCount; i++)
                keys.Add(PKey.Back);
            SendKeysAndWait(keys, millisecondsBetweenKeys);
        }

        public void SendString(string textToSend, int millisecondsBetweenKeys = 2)
        {
            var textParts = MakeReadyForSending(textToSend);
            SendStrings(textParts, millisecondsBetweenKeys);
        }

        public void SendStrings(IList<string> textPartsToSend, int millisecondsBetweenKeys = 2)
        {
            if (textPartsToSend.Count <= 0) return;

            foreach (var part in textPartsToSend) 
                SendKeysAndWait(part, millisecondsBetweenKeys);
        }
    }
}