using System;
using System.Linq;
using System.Windows.Forms;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms
{
    public class KeyboardManager : IDisposable
    {
        public GlobalKeyboardHook Hook { get; set; }
        public HotPhraseKeySequenceList HotPhraseKeySequences { get; set; } = new();

        public KeyboardManager CallThisEachTimeAKeyIsPressed(
            EventHandler<GlobalKeyboardHookEventArgs> keyEventHandler)
        {
            if (keyEventHandler == null)
                throw new ArgumentNullException(nameof(keyEventHandler));

            Hook.KeyboardPressedEvent += keyEventHandler;
            return this;
        }

        public KeyboardManager AddOrReplace(
            EventHandler<PhraseEventArgs> hotPhraseEventArgs, 
            int wildcardCount, 
            WildcardMatchType matchType, 
            params Keys[] keys)
        {
            if (hotPhraseEventArgs == null)
                throw new ArgumentNullException(nameof(hotPhraseEventArgs));

            if (keys == null || keys.Length == 0)
                throw new ArgumentNullException(nameof(keys));

            var hotPhraseKeySequence = new HotPhraseKeySequence(Guid.NewGuid().ToString(), keys, hotPhraseEventArgs);

            if (wildcardCount > 0 && matchType != WildcardMatchType.None && matchType != WildcardMatchType.Unknown)
            {
                hotPhraseKeySequence.WildcardMatchType = matchType;
                hotPhraseKeySequence.WildcardCount = wildcardCount;

            }

            AddOrReplace(hotPhraseKeySequence);
            return this;
        }

        public KeyboardManager AddOrReplace(string name, Keys[] keys, EventHandler<PhraseEventArgs> hotPhraseEventArgs)
        {
            if (hotPhraseEventArgs == null)
                throw new ArgumentNullException(nameof(hotPhraseEventArgs));

            if (keys == null || keys.Length == 0)
                throw new ArgumentNullException(nameof(keys));

            if (string.IsNullOrEmpty(name))
                name = Guid.NewGuid().ToString();

            return AddOrReplace(new HotPhraseKeySequence(name, keys, hotPhraseEventArgs));
        }

        public KeyboardManager AddOrReplace(HotPhraseKeySequence hotPhraseKeySequence)
        {
            if (hotPhraseKeySequence == null)
                throw new ArgumentNullException(nameof(hotPhraseKeySequence));

            var existingPhraseKeySequence = HotPhraseKeySequences
                .FirstOrDefault(x => x.Name
                    .Equals(hotPhraseKeySequence.Name,
                        StringComparison.InvariantCultureIgnoreCase));

            if (existingPhraseKeySequence != null)
                HotPhraseKeySequences.Remove(existingPhraseKeySequence);
            HotPhraseKeySequences.Add(hotPhraseKeySequence);
            return this;
        }

        public void Dispose()
        {
            Hook?.Dispose();
        }

        public static KeyboardManager Factory(
            EventHandler<GlobalKeyboardHookEventArgs> onManagerKeyboardPressEvent)
        {
            if (onManagerKeyboardPressEvent == null)
                throw new ArgumentNullException(nameof(onManagerKeyboardPressEvent));

            var manager = new KeyboardManager
            {
                Hook = new GlobalKeyboardHook(onManagerKeyboardPressEvent)
            };
            return manager;
        }
    }
}