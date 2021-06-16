using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class SendKeysKeywordTests
    {
        [TestMethod]
        public void SplitInTwo_EvenLength()
        {
            var actual = "abcdefgh".SplitInTwo();
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual("abcd", actual[0]);
            Assert.AreEqual("efgh", actual[1]);
        }

        [TestMethod]
        public void SplitInTwo_OddLength()
        {
            var actual = "abcdefghi".SplitInTwo();
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual("abcd", actual[0]);
            Assert.AreEqual("efghi", actual[1]);
        }

        [TestMethod]
        public void MakeReadyForSendKeys_NeedsToBeBrokenDown()
        {

        }

        [TestMethod]
        public void MakeReadyForSendKeys_HasSpecialCharacters()
        {
            var data = "abcde+fgyzyzyzyzyzyzyzyzyzyz@hijk.lmn";

            var expected = "abcde{ADD}fgyzyzyzyzyzyzyzyzyzyz@hijk.lmn";

            var parent = new HotPhraseManagerForWinForms();
            var actual = parent.MakeReadyForSending(data, 8, false);
            Assert.IsNotNull(actual);
            Assert.AreEqual(6, actual.Count);
            
            Assert.AreEqual("abcde", actual[0]);
            Assert.AreEqual("{ADD}", actual[1]);
            Assert.AreEqual("fgyzyzy", actual[2]);
            Assert.AreEqual("zyzyzyzy", actual[3]);
            Assert.AreEqual("zyzyzyz@", actual[4]);
            Assert.AreEqual("hijk.lmn", actual[5]);

            Assert.AreEqual(expected, string.Join('⌂', actual).Replace("⌂", ""));
            
        }

        [TestMethod]
        public void ShouldBeSimplified_True()
        {
            Assert.IsTrue(PKey.Shift.ShouldBeSimplified());
            Assert.IsTrue(PKey.Control.ShouldBeSimplified());
            Assert.IsTrue(PKey.ShiftKey.ShouldBeSimplified());
            Assert.IsTrue(PKey.ControlKey.ShouldBeSimplified());
            Assert.IsTrue(PKey.Alt.ShouldBeSimplified());
            Assert.IsTrue(PKey.LWin.ShouldBeSimplified());
            Assert.IsTrue(PKey.D5.ShouldBeSimplified());
        }

        [TestMethod]
        public void ShouldBeSimplified_False()
        {
            Assert.IsFalse(PKey.A.ShouldBeSimplified());
            Assert.IsFalse(PKey.Enter.ShouldBeSimplified());
            Assert.IsFalse(PKey.NumPad0.ShouldBeSimplified());
            Assert.IsFalse(PKey.Home.ShouldBeSimplified());
            Assert.IsFalse(PKey.LMenu.ShouldBeSimplified());
        }

        /*
        [TestMethod]
        public void IsExacting_True()
        {
            Assert.IsTrue(SendKeysKeyword.IsExacting(PKey.LShiftKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(PKey.RShiftKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(PKey.RMenu));
            Assert.IsTrue(SendKeysKeyword.IsExacting(PKey.LControlKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(PKey.NumPad5));
        }

        [TestMethod]
        public void IsExacting_False()
        {
            Assert.IsFalse(SendKeysKeyword.IsExacting(PKey.A));
            Assert.IsFalse(SendKeysKeyword.IsExacting(PKey.Enter));
            Assert.IsFalse(SendKeysKeyword.IsExacting(PKey.D0));
            Assert.IsFalse(SendKeysKeyword.IsExacting(PKey.Home));
        }
        */

        [TestMethod]
        public void IsAMatch_WhenTheSame_True()
        {
            Assert.IsTrue(PKey.A.IsAMatch(PKey.A));
        }

        [TestMethod]
        public void IsAMatch_WhenSimilar_True()
        {
            Assert.IsTrue(PKey.Control.IsAMatch(PKey.RControlKey));
        }

        [TestMethod]
        public void IsAMatch_WhenSimilar_False()
        {
            Assert.IsFalse(PKey.RControlKey.IsAMatch(PKey.ControlKey));
        }
    }
}