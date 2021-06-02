using System;
using System.Diagnostics;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms
{
    public class HotPhraseManager : IDisposable
    {
        public KeyboardManager Keyboard { get; set; }
        public KeyHistory History { get; set; } = new();

        public HotPhraseManager()
        {
            Keyboard = KeyboardManager.Factory(OnManagerKeyboardPressEvent);
        }

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

                if(!string.IsNullOrEmpty(matchResult.Value))
                    Debug.WriteLine($"  Wilds {matchResult.Value}");

                History.Clear();
                trigger.Run(matchResult);
            }
        }

        public void Dispose()
        {
            Keyboard?.Dispose();
        }
    }

}
