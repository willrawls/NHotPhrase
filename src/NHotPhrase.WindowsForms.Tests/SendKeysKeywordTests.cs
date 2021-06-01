using System.Windows.Forms;
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
            var actual = data.MakeReadyForSendKeys(8);
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
            Assert.IsTrue(Keys.Shift.ShouldBeSimplified());
            Assert.IsTrue(Keys.Control.ShouldBeSimplified());
            Assert.IsTrue(Keys.ShiftKey.ShouldBeSimplified());
            Assert.IsTrue(Keys.ControlKey.ShouldBeSimplified());
            Assert.IsTrue(Keys.Alt.ShouldBeSimplified());
            Assert.IsTrue(Keys.LWin.ShouldBeSimplified());
            Assert.IsTrue(Keys.D5.ShouldBeSimplified());
        }

        [TestMethod]
        public void ShouldBeSimplified_False()
        {
            Assert.IsFalse(Keys.A.ShouldBeSimplified());
            Assert.IsFalse(Keys.Enter.ShouldBeSimplified());
            Assert.IsFalse(Keys.NumPad0.ShouldBeSimplified());
            Assert.IsFalse(Keys.Home.ShouldBeSimplified());
            Assert.IsFalse(Keys.LMenu.ShouldBeSimplified());
        }

        /*
        [TestMethod]
        public void IsExacting_True()
        {
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.LShiftKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.RShiftKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.RMenu));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.LControlKey));
            Assert.IsTrue(SendKeysKeyword.IsExacting(Keys.NumPad5));
        }

        [TestMethod]
        public void IsExacting_False()
        {
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.A));
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.Enter));
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.D0));
            Assert.IsFalse(SendKeysKeyword.IsExacting(Keys.Home));
        }
        */

        [TestMethod]
        public void IsAMatch_WhenTheSame_True()
        {
            Assert.IsTrue(Keys.A.IsAMatch(Keys.A));
        }

        [TestMethod]
        public void IsAMatch_WhenSimilar_True()
        {
            Assert.IsTrue(Keys.Control.IsAMatch(Keys.RControlKey));
        }

        [TestMethod]
        public void IsAMatch_WhenSimilar_False()
        {
            Assert.IsFalse(Keys.RControlKey.IsAMatch(Keys.ControlKey));
        }
    }
}