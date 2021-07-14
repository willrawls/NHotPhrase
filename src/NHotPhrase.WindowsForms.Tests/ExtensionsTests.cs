using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHotPhrase.Keyboard;

namespace NHotPhrase.WindowsForms.Tests
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void EveryKeyIsAMatchToItself()
        {
            bool anyFailures = false;
            int max = (int) PKey.BrowserBack;
            for(var i = 1; i < max; i++)
            {
                var key = (PKey) i;
                if (key.IsAMatch(key)) continue;

                Console.WriteLine(key + ": " + i);
                anyFailures = true;
            }
            Assert.IsFalse(anyFailures);
        }

        [TestMethod]
        public void EveryKeyIsNotAMatchToAnythingElse()
        {
            bool anySuccesses = false;
            int max = (int) PKey.BrowserBack;
            for(var i = 1; i < max; i++)
            {
                for (var j = 1; j < max; j++)
                {
                    if (i == j) continue;

                    var key = (PKey) i;
                    if (!key.IsAMatch((PKey) j)) continue;

                    if (key.ShouldBeSimplified()) continue;
                    Console.WriteLine($"{key} == {(PKey) j}");
                    anySuccesses = true;

                }
            }
            Assert.IsFalse(anySuccesses);
        }
    }
}