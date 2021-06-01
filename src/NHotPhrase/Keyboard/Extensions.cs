using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace NHotPhrase.Keyboard
{
    public static class Extensions
    {
        public static string[] SplitInTwo(this string target)
        {
            var index = (int) target.Length / 2;
            return new[]
            {
                target.Substring(0, index),
                target.Substring(index),
            };
        }
    }
}