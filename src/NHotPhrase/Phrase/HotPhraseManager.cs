using System;
using System.Diagnostics;
using System.Threading;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public abstract class HotPhraseManager : IDisposable
    {
        public Guid ID { get; } = Guid.NewGuid();

        // public SendKeyHelper KeySender { get; set; }
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

        public void Dispose()
        {
            Keyboard?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}