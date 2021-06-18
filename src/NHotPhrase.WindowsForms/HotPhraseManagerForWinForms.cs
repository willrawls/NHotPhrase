using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MetX.Standard.Library;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms
{
    public class HotPhraseManagerForWinForms : HotPhraseManager
    {
        public override bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keysToSend)
        {
            if (keysToSend is not {Count: > 0}) 
                return true;
            foreach (var key in keysToSend)
                SendKeys.SendWait(key.ToSendKeysText());
            return true;
        }

        public override bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep)
        {
            var sent = false;
            for (var i = 0; i < 5 && !sent; i++)
            {
                try
                {
                    stringToSend = stringToSend.ToSendKeysFormat();
                    SendKeys.SendWait(stringToSend);
                    sent = true;
                }
                catch (InvalidOperationException)
                {
                    Debug.WriteLine($"SendKeysAndWait: InvalidOperation: i={i}");
                    Thread.Sleep(100 + 100*(i+1));
                }
            }
            if(millisecondThreadSleep > 0)
                Thread.Sleep(millisecondThreadSleep);
            return true;
        }

        public override bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep)
        {
            foreach (var part in stringsToSend) 
                SendKeysAndWait(part, 2);
            return true;
        }

        public override bool SendKeysAndWait(List<PKey> keysToSend, int millisecondThreadSleep)
        {
            if (keysToSend is not {Count: > 0}) 
                return true;
            foreach (var key in keysToSend)
                SendKeys.SendWait(key.ToSendKeysText());
            return true;
        }

        public override List<string> MakeReadyForSending(string target, int splitLength, bool sendAsIs)
        {
            if (string.IsNullOrEmpty(target))
                return new List<string>();

            if (splitLength < 4)
                splitLength = 4;

            List<string> list;
            if (sendAsIs)
            {
                list = new List<string>() {target};
            }
            else
            {
                foreach (var keyword in SendKeyHelper.Entries.Where(k => !string.IsNullOrEmpty(k.ReplaceWith)))
                    target = target.Replace(keyword.Name, "⌂" + keyword.ReplaceWith + "⌂");
                list = target.Split('⌂', StringSplitOptions.RemoveEmptyEntries).ToList();
            }

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

        public override void SendBackspaces(int backspaceCount, int millisecondsBetweenKeys = 2)
        {
            var keys = new List<PKey>();
            for (var i = 0; i < backspaceCount; i++)
                keys.Add(PKey.Back);
            SendKeysAndWait(keys, millisecondsBetweenKeys);
        }

        public override void SendString(string textToSend, int millisecondsBetweenKeys, bool sendAsIs)
        {
            var textParts = MakeReadyForSending(textToSend, SplitLength, sendAsIs);
            SendStrings(textParts, millisecondsBetweenKeys);
        }

        public override void SendStrings(IList<string> textPartsToSend, int millisecondsBetweenKeys)
        {
            if (textPartsToSend.Count <= 0) return;

            foreach (var part in textPartsToSend) 
                SendKeysAndWait(part, millisecondsBetweenKeys);
        }
    }
}