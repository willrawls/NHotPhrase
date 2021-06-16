using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public abstract class HotPhraseManager : IDisposable, ISendKeys
    {
        public Guid ID { get; } = Guid.NewGuid();
        public int SplitLength { get; set; } = 8;

        public KeyboardManager Keyboard { get; set; }
        public KeyHistory History { get; set; } = new();

        public static object SyncRoot { get; } = new();

        public HotPhraseManager()
        {
            Keyboard = KeyboardManager.Factory(OnManagerKeyboardPressEvent);
        }

        public void OnManagerKeyboardPressEvent(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState != KeyboardState.KeyUp)
                return;

            if (!Monitor.TryEnter(SyncRoot)) return;

            try
            {
                Debug.WriteLine($"PKey {e.KeyboardData.PKey}");
                History.AddKeyPress(e.KeyboardData.PKey);
                var trigger = Keyboard.KeySequences.FirstMatch(History, out var matchResult);
                if (trigger == null)
                    return;

                Debug.WriteLine($"Trigger {trigger.Name}");

                if (!string.IsNullOrEmpty(matchResult?.Value))
                    Debug.WriteLine($"  Wilds {matchResult.Value}");

                History.Clear();
                trigger.Run(matchResult);
            }
            finally
            {
                Monitor.Exit(SyncRoot);
            }
        }

        public abstract bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keys);
        public abstract bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep);
        public abstract bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep);
        public abstract bool SendKeysAndWait(List<PKey> keys, int millisecondThreadSleep);
        public abstract List<string> MakeReadyForSending(string target, int splitLength, bool sendAsIs);
        public abstract void SendBackspaces(int backspaceCount, int millisecondsBetweenKeys);
        public abstract void SendString(string textToSend, int millisecondsBetweenKeys, bool sendAsIs);
        public abstract void SendStrings(IList<string> textPartsToSend, int millisecondsBetweenKeys);

        public void Dispose()
        {
            Keyboard?.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}