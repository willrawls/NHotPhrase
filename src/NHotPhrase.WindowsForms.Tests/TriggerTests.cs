using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class TriggerTests
    {
        public static PKey[] RControl3Times = new[] {PKey.RControlKey, PKey.RControlKey, PKey.RControlKey};
        public static PKey[] Shift3Times = new[] {PKey.Shift, PKey.Shift, PKey.Shift};

        [TestMethod]
        public void RControl3Times_IsAMatch_True()
        {
            var callCount = 0;
            var data = new KeySequence("Fred", RControl3Times, (sender, args) =>
            {
                callCount++;
                args.Handled = true;
            });

            var history = new List<PKey>
            {
                PKey.RControlKey,
                PKey.RControlKey,
                PKey.RControlKey,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone, out var wildcards);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Shift3Times3Times_IsAMatch_True()
        {
            var data = new KeySequence("Fred", Shift3Times, (sender, args) => args.Handled = true);

            var history = new List<PKey>
            {
                PKey.LShiftKey,
                PKey.RShiftKey,
                PKey.ShiftKey,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone, out var wildcards);

            Assert.IsTrue(actual);
            Assert.IsNull(wildcards);
        }

        [TestMethod]
        public void RControl3Times_IsAMatch_True_WhenMoreThan3EntriesInHistory()
        {
            var callCount = 0;
            var data = new KeySequence("Fred", RControl3Times, (sender, args) =>
            {
                callCount++;
                args.Handled = true;
            });

            var history = new List<PKey>
            {
                PKey.RControlKey,
                PKey.Enter,
                PKey.RControlKey,
                PKey.RControlKey,
                PKey.RControlKey,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone, out var wildcards);

            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void RControl3Times_IsAMatch_False_Simple()
        {
            var callCount = 0;
            var data = new KeySequence("Fred", RControl3Times, (sender, args) =>
            {
                callCount++;
                args.Handled = true;
            });

            var history = new List<PKey>
            {
                PKey.RControlKey,
                PKey.A,
                PKey.RControlKey,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone, out var wildcards);

            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void RControl3Times_IsAMatch_False_NotEnoughPKey()
        {
            var callCount = 0;
            var data = new KeySequence("Fred", RControl3Times, (sender, args) =>
            {
                callCount++;
                args.Handled = true;
            });

            var history = new List<PKey>
            {
                PKey.RControlKey,
                PKey.RControlKey,
            };
            var keyPressHistoryClone = new KeyHistory(8, 8, DateTime.Now, history);
            var actual = data.IsAMatch(keyPressHistoryClone, out var wildcards);

            Assert.IsFalse(actual);
        }

        [DataTestMethod]
        [DataRow(new[]{ PKey.A }, new[]{ PKey.A })]
        [DataRow(new[]{ PKey.A, PKey.A }, new[]{ PKey.A, PKey.B, PKey.A, PKey.A })]
        [DataRow(new[]{ PKey.CapsLock, PKey.Control }, new[]{ PKey.A, PKey.B, PKey.CapsLock, PKey.RControlKey })]
        [DataRow(new[]{ PKey.Control, PKey.LControlKey }, new[]{ PKey.A, PKey.RControlKey, PKey.LControlKey })]
        [DataRow(new[]{ PKey.RControlKey, PKey.RControlKey }, new[]{ PKey.RControlKey, PKey.B, PKey.RControlKey, PKey.RControlKey })]
        [DataRow(new[]{ PKey.RControlKey, PKey.Control }, new[]{ PKey.RControlKey, PKey.B, PKey.RControlKey, PKey.LControlKey })]

        [DataRow(new[]{ PKey.RControlKey, PKey.RControlKey, PKey.RControlKey }, new[]{ PKey.RControlKey, PKey.RControlKey, PKey.RControlKey })]
        [DataRow(new[]{ PKey.Control, PKey.Shift, PKey.Alt}, new[]{ PKey.LControlKey, PKey.RShiftKey, PKey.LMenu })]
        
        [DataRow(new[]{ PKey.CapsLock, PKey.Capital, PKey.D, PKey.Back }, new[]{ PKey.RControlKey, PKey.Capital, PKey.Capital, PKey.D, PKey.Back })]
        [DataRow(new[]{ PKey.CapsLock, PKey.CapsLock, PKey.D, PKey.Back }, new[]{ PKey.RControlKey, PKey.CapsLock, PKey.CapsLock, PKey.D, PKey.Back })]
        
        public void VariousSequences_IsAMatch_True(PKey[] hotPhraseSequence, PKey[] userTyped)
        {
            for (var i = 0; i < 100; i++)
            {
                var simulatedHistoryList = userTyped.ToList();
                var sequenceList = hotPhraseSequence.ToList();

                TestSequence(simulatedHistoryList, sequenceList, true);

                simulatedHistoryList.Insert(0, RandomKey(hotPhraseSequence));
                TestSequence(simulatedHistoryList, sequenceList, true);
            
                simulatedHistoryList.Add(RandomKey(hotPhraseSequence));
                TestSequence(simulatedHistoryList, sequenceList, false);
            }
        }

        [DataTestMethod]
        [DataRow(new[]{ PKey.B }, new[]{ PKey.A })]
        [DataRow(new[]{ PKey.A, PKey.A }, new[]{ PKey.A, PKey.B, PKey.A, PKey.B })]
        [DataRow(new[]{ PKey.CapsLock, PKey.Control }, new[]{ PKey.A, PKey.CapsLock, PKey.B, PKey.RControlKey })]
        [DataRow(new[]{ PKey.Control, PKey.LControlKey }, new[]{ PKey.A, PKey.RControlKey, PKey.RControlKey })]
        [DataRow(new[]{ PKey.RControlKey, PKey.RControlKey }, new[]{ PKey.RControlKey, PKey.B, PKey.LControlKey, PKey.RControlKey })]
        [DataRow(new[]{ PKey.RControlKey, PKey.Control }, new[]{ PKey.RControlKey, PKey.C })]
        public void VariousSequences_IsAMatch_False(PKey[] hotPhraseSequence, PKey[] userTyped)
        {
            for (var i = 0; i < 100; i++)
            {
                var simulatedHistoryList = userTyped.ToList();
                var sequenceList = hotPhraseSequence.ToList();

                TestSequence(simulatedHistoryList, sequenceList, false);
            
                simulatedHistoryList.Insert(0, RandomKey(hotPhraseSequence));
                TestSequence(simulatedHistoryList, sequenceList, false);
            
                simulatedHistoryList.Add(RandomKey(hotPhraseSequence));
                TestSequence(simulatedHistoryList, sequenceList, false);
            }
        }
        
        [TestMethod]
        public void SingleKey_IsAMatch_True()
        {
            var pKeyEnumValues = PKeyEnumValues();
            foreach (var key in pKeyEnumValues)
            {
                var sequence = new List<PKey> {key}.ToArray();
                var simulatedHistoryList = sequence.ToList();
                var sequenceList = sequence.ToList();

                TestSequence(simulatedHistoryList, sequenceList, true);

                simulatedHistoryList.Insert(0, RandomKey(sequence));
                TestSequence(simulatedHistoryList, sequenceList, true);
            
                simulatedHistoryList.Add(RandomKey(sequence));
                TestSequence(simulatedHistoryList, sequenceList, false);
            }
        }

        public static void TestSequence(List<PKey> simulatedHistory, List<PKey> sequence, bool expected)
        {
            var hotPhraseKeySequence = new KeySequence("Fred", sequence.ToArray(), (sender, args) => args.Handled = true);
            var keyHistory = new KeyHistory(8, 8, DateTime.Now, simulatedHistory.ToList());
            var actual = hotPhraseKeySequence.IsAMatch(keyHistory, out var wildcards);
            if (actual && !expected)
            {
                // Check the last to see if it's a simplified, if so, actual == expected
                if (sequence[0].ShouldBeSimplified())
                {
                    if (sequence[0].IsAMatch(keyHistory[^1]))
                    {
                        // It's all good
                        actual = expected;
                    }
                }
            }
            var debugText = $"\n    Sequence: {KeyListToString(sequence)}\n     History: {KeyListToString(simulatedHistory)}";
            Assert.AreEqual(expected, actual, debugText);
        }

        private static string KeyListToString(List<PKey> list)
        {
            return list.Aggregate("", (current, item) => current + (" " + item));
        }

        public static List<PKey> PKeyToNotGenerateRandomly = null;
        public static Random Random = new Random();
        public static PKey RandomKey(PKey[] butNotThesePKey)
        {
            if (PKeyToNotGenerateRandomly == null)
            {
                PKeyToNotGenerateRandomly = new List<PKey>();
                var pKeyEnumValues = PKeyEnumValues();
                foreach (var key in pKeyEnumValues)
                    if(key.ShouldBeSimplified())
                        PKeyToNotGenerateRandomly.Add(key);
            }

            var randomKey = (PKey) Random.Next(32, 165);

            while(butNotThesePKey.Any(k => k == randomKey) 
                  || PKeyToNotGenerateRandomly.Any(k => k == randomKey))
            {
                randomKey = (PKey) Random.Next(32, 165);
            }

            return randomKey;
        }

        private static List<PKey> _PKeyEnumValues = null;
        private static List<PKey> PKeyEnumValues()
        {
            if (_PKeyEnumValues != null) 
                return _PKeyEnumValues;

            _PKeyEnumValues = new List<PKey>();
            foreach (PKey key in Enum.GetValues(typeof(PKey)))
            {
                if(key != PKey.None) 
                    _PKeyEnumValues .Add(key);
            }
            return _PKeyEnumValues;
        }
    }
}
