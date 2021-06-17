using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.Wpf
{
    public class HotPhraseManagerForWpf : HotPhraseManager, ISendKeys
    {
        public int MillisecondsBetweenKeyPress { get; set; } = 1;
        public InputSimulator InputSimulator { get; set; } = new();

        public static VirtualKeyCode[] MakePKeysReadyForInputSimulator(List<PKey> keys)
        {
            var convertedKeys = new VirtualKeyCode[keys.Count];
            for (var i = 0; i < keys.Count; i++) convertedKeys[i] = (VirtualKeyCode) keys[i];

            return convertedKeys;
        }

        public override bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keysToSend)
        {
            if (keysToSend is not {Count: > 0})
                return true;
            var inputSimulatorKeys = MakePKeysReadyForInputSimulator(keysToSend);
            foreach (var key in inputSimulatorKeys)
            {
                InputSimulator.Keyboard.KeyPress(key);
                Thread.Sleep(MillisecondsBetweenKeyPress);
            }

            return true;
        }

        public override bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep = 2)
        {
            InputSimulator.Keyboard.TextEntry(stringToSend);
            if (millisecondThreadSleep > 0)
                Thread.Sleep(millisecondThreadSleep);
            return true;
        }

        public override bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep = 2)
        {
            foreach (var part in stringsToSend)
                SendKeysAndWait(part, millisecondThreadSleep);
            return true;
        }

        public override bool SendKeysAndWait(List<PKey> keysToSend, int millisecondThreadSleep)
        {
            if (keysToSend is not {Count: > 0})
                return true;
            var inputSimulatorKeys = MakePKeysReadyForInputSimulator(keysToSend);
            foreach (var key in inputSimulatorKeys)
            {
                InputSimulator.Keyboard.KeyPress(key);
                Thread.Sleep(millisecondThreadSleep);
            }

            return true;
        }

        public override List<string> MakeReadyForSending(string target, int splitLength, bool sendAsIs)
        {
            if (string.IsNullOrEmpty(target))
                return new List<string>();

            var list = new List<string> { target };
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