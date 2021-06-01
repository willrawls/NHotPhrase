using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace NHotPhrase.Keyboard
{
    public static class Extensions
    {
        public static void SendString(this string textToSend)
        {
            var textParts = textToSend.MakeReadyForSendKeys();
            textParts.SendStrings();
        }

        public static void SendStrings(this IList<string> textPartsToSend)
        {
            if (textPartsToSend.Count <= 0) return;

            foreach (var part in textPartsToSend)
            {
                SendKeys.SendWait(part);
                Thread.Sleep(2);
            }
        }

        public static string AsString(this IList<Keys> keys)
        {
            if (keys == null || keys.Count < 1)
                return "";

            var result = "";
            foreach (var key in keys)
            {
                var keyString = "";
                switch (key)
                {
                    case Keys.Tab: keyString = "\t";
                        break;
                    case Keys.LineFeed: keyString = "\r";
                        break;
                    case Keys.Return: keyString = "\n";
                        break;
                    case Keys.Space: keyString = " ";
                        break;

                    case Keys.NumPad0:
                    case Keys.D0: keyString = "0";
                        break;
                    case Keys.NumPad1:
                    case Keys.D1: keyString = "1";
                        break;
                    case Keys.NumPad2:
                    case Keys.D2: keyString = "2";
                        break;
                    case Keys.NumPad3:
                    case Keys.D3: keyString = "3";
                        break;
                    case Keys.NumPad4:
                    case Keys.D4: keyString = "4";
                        break;
                    case Keys.NumPad5:
                    case Keys.D5: keyString = "5";
                        break;
                    case Keys.NumPad6:
                    case Keys.D6: keyString = "6";
                        break;
                    case Keys.NumPad7:
                    case Keys.D7: keyString = "7";
                        break;
                    case Keys.NumPad8:
                    case Keys.D8: keyString = "8";
                        break;
                    case Keys.NumPad9:
                    case Keys.D9: keyString = "9";
                        break;

                    case Keys.A:
                    case Keys.B:
                    case Keys.C:
                    case Keys.D:
                    case Keys.E:
                    case Keys.F:
                    case Keys.G:
                    case Keys.H:
                    case Keys.I:
                    case Keys.J:
                    case Keys.K:
                    case Keys.L:
                    case Keys.M:
                    case Keys.N:
                    case Keys.O:
                    case Keys.P:
                    case Keys.Q:
                    case Keys.R:
                    case Keys.S:
                    case Keys.T:
                    case Keys.U:
                    case Keys.V:
                    case Keys.W:
                    case Keys.X:
                    case Keys.Y:
                    case Keys.Z:
                        keyString = key.ToString();
                        break;

                    case Keys.Multiply:
                        keyString = "*";
                        break;
                    case Keys.Add:
                        keyString = "+";
                        break;
                    case Keys.Separator:
                    case Keys.Subtract:
                        keyString = "-";
                        break;
                    case Keys.Decimal:
                        keyString = ".";
                        break;
                    case Keys.Divide:
                        keyString = "/";
                        break;

                    case Keys.OemSemicolon:
                        keyString = ";";
                        break;
                    case Keys.Oemplus:
                        keyString = "+";
                        break;
                    case Keys.Oemcomma:
                        keyString = ",";
                        break;
                    case Keys.OemMinus:
                        keyString = "-";
                        break;
                    case Keys.OemPeriod:
                        keyString = ".";
                        break;
                    case Keys.OemQuestion:
                        keyString = "?";
                        break;
                    case Keys.Oemtilde:
                        keyString = "~";
                        break;
                    case Keys.OemOpenBrackets:
                        keyString = "[";
                        break;
                    case Keys.OemPipe:
                        keyString = "|";
                        break;
                    case Keys.OemCloseBrackets:
                        keyString = "]";
                        break;
                    case Keys.OemQuotes:
                        keyString = "\"";
                        break;
                    case Keys.Oem8:
                        keyString = "8";
                        break;
                    case Keys.OemBackslash:
                        keyString = "\\";
                        break;
                }
                result += keyString;
            }
            return result;
        }
        
        public static bool OnlyLetters(this IList<Keys> keys)
        {
            foreach (var key in keys)
            {
                var keepGoing = false;
                switch(key)
                {
                    case Keys.A:
                    case Keys.B:
                    case Keys.C:
                    case Keys.D:
                    case Keys.E:
                    case Keys.F:
                    case Keys.G:
                    case Keys.H:
                    case Keys.I:
                    case Keys.J:
                    case Keys.K:
                    case Keys.L:
                    case Keys.M:
                    case Keys.N:
                    case Keys.O:
                    case Keys.P:
                    case Keys.Q:
                    case Keys.R:
                    case Keys.S:
                    case Keys.T:
                    case Keys.U:
                    case Keys.V:
                    case Keys.W:
                    case Keys.X:
                    case Keys.Y:
                    case Keys.Z:
                        keepGoing = true;
                        break;
                }

                if (!keepGoing)
                    return false;
            }

            return true;
        }

        public static bool OnlyAlphaNumeric(this IList<Keys> keys)
        {
            foreach (var key in keys)
            {
                var keepGoing = false;
                switch(key)
                {
                    case Keys.A:
                    case Keys.B:
                    case Keys.C:
                    case Keys.D:
                    case Keys.E:
                    case Keys.F:
                    case Keys.G:
                    case Keys.H:
                    case Keys.I:
                    case Keys.J:
                    case Keys.K:
                    case Keys.L:
                    case Keys.M:
                    case Keys.N:
                    case Keys.O:
                    case Keys.P:
                    case Keys.Q:
                    case Keys.R:
                    case Keys.S:
                    case Keys.T:
                    case Keys.U:
                    case Keys.V:
                    case Keys.W:
                    case Keys.X:
                    case Keys.Y:
                    case Keys.Z:
                        keepGoing = true;
                        break;
                }

                if (!keepGoing)
                    return false;
            }

            return true;
        }

        public static bool OnlyDigits(this IList<Keys> keys)
        {
            foreach (var key in keys)
            {
                var keepGoing = false;
                switch(key)
                {
                    case Keys.D0:
                    case Keys.D1:
                    case Keys.D2:
                    case Keys.D3:
                    case Keys.D4:
                    case Keys.D5:
                    case Keys.D6:
                    case Keys.D7:
                    case Keys.D8:
                    case Keys.D9:
                    case Keys.NumPad0:
                    case Keys.NumPad1:
                    case Keys.NumPad2:
                    case Keys.NumPad3:
                    case Keys.NumPad4:
                    case Keys.NumPad5:
                    case Keys.NumPad6:
                    case Keys.NumPad7:
                    case Keys.NumPad8:
                    case Keys.NumPad9:
                        keepGoing = true;
                        break;
                }

                if (!keepGoing)
                    return false;
            }

            return true;
        }

        public static List<string> MakeReadyForSendKeys(this string target, int splitLength = 8)
        {
            if (string.IsNullOrEmpty(target))
                return new List<string>();

            foreach (var keyword in SendKeysKeyword.Keywords.Where(k => !string.IsNullOrEmpty(k.ReplaceWith)))
            {
                target = target.Replace(keyword.Name, "⌂" + keyword.ReplaceWith + "⌂");
            }

            var list = target.Split('⌂', StringSplitOptions.RemoveEmptyEntries).ToList();
            while(list.Any(p => p.Length > splitLength))
            {
                for (var i = 0; i < list.Count; i++)
                {
                    if (list[i].Length > splitLength)
                    {
                        var pieces = list[i].SplitInTwo();
                        list.RemoveAt(i);
                        list.InsertRange(i, pieces);
                    }
                }
            }   
            return list;
        }

        public static string[] SplitInTwo(this string target)
        {
            var index = (int) target.Length / 2;
            return new[]
            {
                target.Substring(0, index),
                target.Substring(index),
            };
        }

        /*
        public static List<string> SplitIntoEqualPieces(this string str, int splitLength)
        {
            if (string.IsNullOrEmpty(str) || splitLength < 1) 
            {
                throw new ArgumentException();
            }
 
            return Enumerable.Range(0, str.Length / splitLength)
                .Select(i => str.Substring(i * splitLength, splitLength))
                .ToList();
        }
    */
    }
}