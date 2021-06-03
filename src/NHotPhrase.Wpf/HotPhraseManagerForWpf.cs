using System.Collections.Generic;
using System.Threading;
using WindowsInput;
using WindowsInput.Native;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.Wpf
{
    public class HotPhraseManagerForWpf : HotPhraseManager, ISendKeys
    {
        public HotPhraseManagerForWpf()
        {
            SendPKeys.Singleton = this;
        }

        public int MillisecondsBetweenKeyPress { get; set; } = 1;
        public InputSimulator InputSimulator { get; set; } = new();

        private static ISendKeys _singleton;
        public static ISendKeys Singleton => _singleton ??= new HotPhraseManagerForWpf();

        public static VirtualKeyCode[] MakePKeysReadyForInputSimulator(List<PKey> keys)
        {
            var convertedKeys = new VirtualKeyCode[keys.Count];
            for (var i = 0; i < keys.Count; i++)
            {
                convertedKeys[i] = (VirtualKeyCode) keys[i];
            }

            return convertedKeys;
        }

        public bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keysToSend)
        {
            if (keysToSend is not {Count: > 0}) 
                return true;
            var inputSimulatorKeys = MakePKeysReadyForInputSimulator(keysToSend);
            foreach(var key in inputSimulatorKeys)
            {
                InputSimulator.Keyboard.KeyPress(key);
                Thread.Sleep(MillisecondsBetweenKeyPress);
            }
            return true;
        }

        public bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep = 2)
        {
            InputSimulator.Keyboard.TextEntry(stringToSend);
            if(millisecondThreadSleep > 0)
                Thread.Sleep(millisecondThreadSleep);
            return true;
        }

        public bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep = 2)
        {
            foreach (var part in stringsToSend) 
                SendKeysAndWait(part, millisecondThreadSleep);
            return true;
        }
    }
}