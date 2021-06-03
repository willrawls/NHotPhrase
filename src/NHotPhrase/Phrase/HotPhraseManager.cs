using System;
using System.Diagnostics;
using NHotPhrase.Keyboard;

namespace NHotPhrase.Phrase
{
    public abstract class HotPhraseManager : IDisposable
    {
        protected HotPhraseManager()
        {
            Keyboard = KeyboardManager.Factory(OnManagerKeyboardPressEvent);
        }

        public Guid ID { get; } = Guid.NewGuid();

        public KeyboardManager Keyboard { get; set; }
        public KeyHistory History { get; set; } = new();

        public void OnManagerKeyboardPressEvent(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardState == KeyboardState.KeyUp)
            {
                Debug.WriteLine($"PKey {e.KeyboardData.PKey}");
                History.AddKeyPress(e.KeyboardData.PKey);
                var trigger = Keyboard.KeySequences.FirstMatch(History, out var matchResult);
                if (trigger == null)
                    return;

                Debug.WriteLine($"Trigger {trigger.Name}");

                if(!string.IsNullOrEmpty(matchResult?.Value))
                    Debug.WriteLine($"  Wilds {matchResult.Value}");

                History.Clear();
                trigger.Run(matchResult);
            }
        }

        public void Dispose()
        {
            if (SendPKeys.Singleton?.ID == ID)
            {
                SendPKeys.Singleton = null;
            }

            Keyboard?.Dispose();
            GC.SuppressFinalize(this);
        }
    }

}
