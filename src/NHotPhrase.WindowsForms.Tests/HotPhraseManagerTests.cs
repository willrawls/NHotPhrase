using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class HotPhraseManagerTests
    {
        [TestMethod]
        public void RightControl3TimesInARow()
        {
            var hotPhraseManager = SetupHotPhraseManagerTest(out var hotPhrase);

            var callCount = 0;
            hotPhraseManager.CallThisEachTimeAKeyIsPressed((_, args) =>
            {
                Assert.AreEqual(PKey.RControlKey, args.KeyboardData.PKey);
                ++callCount;
                args.Handled = true;
            });

            var lowLevelKeyboardInputEvent = new LowLevelKeyboardInputEvent
            {
                AdditionalInformation = IntPtr.Zero,
                HardwareScanCode = (int) PKey.RControlKey,
                Flags = 0,
                TimeStamp = 0,
                VirtualCode = (int) PKey.RControlKey
            };
            var keyboardState = KeyboardState.KeyUp;
            var eventArguments = new GlobalKeyboardHookEventArgs(lowLevelKeyboardInputEvent, keyboardState);

            // Send 3 keys
            hotPhraseManager.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
            hotPhraseManager.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
            hotPhraseManager.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);

            Assert.AreEqual(3, callCount);
        }

        private static KeyboardManager SetupHotPhraseManagerTest(out KeySequence hotPhrase)
        {
            var keys = new List<PKey> {PKey.ControlKey, PKey.ControlKey, PKey.ControlKey};

            void hotPhraseEventArgs(object _, PhraseEventArguments e) => e.Handled = true;
            hotPhrase = new KeySequence("RightControl3TimesInARow", keys, hotPhraseEventArgs);

            void HotGlobalKeyboardHookEventArgs(object _, GlobalKeyboardHookEventArgs e) => e.Handled = true;
            var hotPhraseManager = KeyboardManager.Factory(HotGlobalKeyboardHookEventArgs);
            
            hotPhraseManager.AddOrReplace(hotPhrase);
            return hotPhraseManager;
        }

        private static KeyboardManager SetupHotPhraseManagerTest(List<PKey> keys, out KeySequence hotPhrase)
        {
            void hotPhraseEventArgs(object _, PhraseEventArguments e) => e.Handled = true;
            hotPhrase = new KeySequence("RightControl3TimesInARow", keys, hotPhraseEventArgs);

            void HotGlobalKeyboardHookEventArgs(object _, GlobalKeyboardHookEventArgs e) => e.Handled = true;
            var hotPhraseManager = KeyboardManager.Factory(HotGlobalKeyboardHookEventArgs);
            
            hotPhraseManager.AddOrReplace(hotPhrase);
            return hotPhraseManager;
        }

        [TestMethod]
        public void RightControl3TimesInARow_Dissected()
        {
            var hotPhraseManager = SetupHotPhraseManagerTest(out var hotPhrase);
            
            Assert.IsNotNull(hotPhraseManager.KeySequences[0].Sequence);
            Assert.AreEqual(3, hotPhraseManager.KeySequences[0].Sequence.Count);
            Assert.AreEqual(PKey.ControlKey, hotPhraseManager.KeySequences[0].Sequence[0]);
            Assert.AreEqual(PKey.ControlKey, hotPhraseManager.KeySequences[0].Sequence[1]);
            Assert.AreEqual(PKey.ControlKey, hotPhraseManager.KeySequences[0].Sequence[2]);
            
            Assert.IsNotNull(hotPhraseManager.KeySequences[0].ActionList[0].Handler);
            var hotPhraseEventArgs = new PhraseEventArguments(null, null, null);
            hotPhraseManager.KeySequences[0].ActionList[0].Handler(null, hotPhraseEventArgs);
            Assert.IsTrue(hotPhraseEventArgs.Handled);
        }

        [TestMethod]
        public void ControlControl1_2_or_3_ButOnly2Matches()
        {
            static void HotPhraseEventArgs(object _, PhraseEventArguments e) => e.Handled = true;
            var keysTyped = new List<PKey> {PKey.Shift, PKey.ControlKey, PKey.ControlKey, PKey.D3};

            // Only D1 typed
            var keys = new List<PKey> {PKey.ControlKey, PKey.ControlKey, PKey.D1};
            var hotPhraseManager = SetupHotPhraseManagerTest(keys, out var hotPhrase);
            Assert.IsFalse(hotPhraseManager.KeySequences[0].IsAMatch(keysTyped, out var matchResult));
            

            // D2 typed
            keys = new List<PKey> {PKey.ControlKey, PKey.ControlKey, PKey.D2};
            var hotPhrase2 = new KeySequence("ControlControl2", keys, HotPhraseEventArgs);
            hotPhraseManager.KeySequences.Add(hotPhrase2);
            Assert.IsFalse(hotPhraseManager.KeySequences[0].IsAMatch(keysTyped, out matchResult));
            Assert.IsFalse(hotPhraseManager.KeySequences[0].IsAMatch(keysTyped, out matchResult));


            // D3 typed
            keys = new List<PKey> {PKey.ControlKey, PKey.ControlKey, PKey.D3};
            var hotPhrase3 = new KeySequence("ControlControl3", keys, HotPhraseEventArgs);
            hotPhraseManager.KeySequences.Add(hotPhrase3);
            Assert.IsFalse(hotPhraseManager.KeySequences[0].IsAMatch(keysTyped, out matchResult));
            Assert.IsFalse(hotPhraseManager.KeySequences[1].IsAMatch(keysTyped, out matchResult));
            Assert.IsTrue(hotPhraseManager.KeySequences[2].IsAMatch(keysTyped, out matchResult));
        }
    }
}
