﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHotPhrase.Phrase;

namespace NHotPhrase.Keyboard
{
    public class KeyboardManager : IDisposable
    {
        public GlobalKeyboardHook Hook { get; set; }
        public KeySequenceList KeySequences { get; set; } = new();

        public KeyboardManager CallThisEachTimeAKeyIsPressed(
            EventHandler<GlobalKeyboardHookEventArgs> keyEventHandler)
        {
            if (keyEventHandler == null)
                throw new ArgumentNullException(nameof(keyEventHandler));

            Hook.KeyboardPressedEvent += keyEventHandler;
            return this;
        }

        public KeyboardManager AddOrReplace(
            EventHandler<PhraseEventArguments> hotPhraseEventArgs, 
            int wildcardCount, 
            WildcardMatchType matchType, 
            List<PKey> keys)
        {
            if (hotPhraseEventArgs == null)
                throw new ArgumentNullException(nameof(hotPhraseEventArgs));

            if (keys == null || keys.Count == 0)
                throw new ArgumentNullException(nameof(keys));

            var hotPhraseKeySequence = new KeySequence(Guid.NewGuid().ToString(), keys, hotPhraseEventArgs);

            if (wildcardCount > 0 && matchType != WildcardMatchType.None && matchType != WildcardMatchType.Unknown)
            {
                hotPhraseKeySequence.WildcardMatchType = matchType;
                hotPhraseKeySequence.WildcardCount = wildcardCount;

            }

            AddOrReplace(hotPhraseKeySequence);
            return this;
        }

        public KeyboardManager AddOrReplace(string name, List<PKey> keys, EventHandler<PhraseEventArguments> hotPhraseEventArgs)
        {
            if (hotPhraseEventArgs == null)
                throw new ArgumentNullException(nameof(hotPhraseEventArgs));

            if (keys == null || keys.Count == 0)
                throw new ArgumentNullException(nameof(keys));

            if (string.IsNullOrEmpty(name))
                name = Guid.NewGuid().ToString();

            return AddOrReplace(new KeySequence(name, keys, hotPhraseEventArgs));
        }

        public KeyboardManager AddOrReplace(KeySequence keySequence)
        {
            if (keySequence == null)
                throw new ArgumentNullException(nameof(keySequence));

            var existingPhraseKeySequence = KeySequences
                .FirstOrDefault(x => x.Name
                    .Equals(keySequence.Name,
                        StringComparison.InvariantCultureIgnoreCase));

            if (existingPhraseKeySequence != null)
                KeySequences.Remove(existingPhraseKeySequence);
            KeySequences.Add(keySequence);
            return this;
        }

        public void Dispose()
        {
            Hook?.Dispose();
            GC.SuppressFinalize(this);
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