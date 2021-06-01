using System;
using System.Windows.Forms;
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
                Assert.AreEqual(Keys.RControlKey, args.KeyboardData.Key);
                ++callCount;
                args.Handled = true;
            });

            var lowLevelKeyboardInputEvent = new LowLevelKeyboardInputEvent
            {
                AdditionalInformation = IntPtr.Zero,
                HardwareScanCode = (int) Keys.RControlKey,
                Flags = 0,
                TimeStamp = 0,
                VirtualCode = (int) Keys.RControlKey
            };
            var keyboardState = KeyboardState.KeyUp;
            var eventArguments = new GlobalKeyboardHookEventArgs(lowLevelKeyboardInputEvent, keyboardState);

            hotPhraseManager.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
            hotPhraseManager.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);
            hotPhraseManager.Hook.HandleKeyEvent(lowLevelKeyboardInputEvent, eventArguments);

            Assert.AreEqual(3, callCount);
        }

        private static KeyboardManager SetupHotPhraseManagerTest(out KeySequence hotPhrase)
        {
            var keys = new[] {Keys.ControlKey, Keys.ControlKey, Keys.ControlKey};

            void hotPhraseEventArgs(object? _, PhraseEventArguments e) => e.Handled = true;
            hotPhrase = new KeySequence("RightControl3TimesInARow", keys, hotPhraseEventArgs);

            void HotGlobalKeyboardHookEventArgs(object? _, GlobalKeyboardHookEventArgs e) => e.Handled = true;
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
            Assert.AreEqual(Keys.ControlKey, hotPhraseManager.KeySequences[0].Sequence[0]);
            Assert.AreEqual(Keys.ControlKey, hotPhraseManager.KeySequences[0].Sequence[1]);
            Assert.AreEqual(Keys.ControlKey, hotPhraseManager.KeySequences[0].Sequence[2]);
            
            Assert.IsNotNull(hotPhraseManager.KeySequences[0].ActionList[0].Handler);
            var hotPhraseEventArgs = new PhraseEventArguments(null, null, null);
            hotPhraseManager.KeySequences[0].ActionList[0].Handler(null, hotPhraseEventArgs);
            Assert.IsTrue(hotPhraseEventArgs.Handled);
        }
    }
}