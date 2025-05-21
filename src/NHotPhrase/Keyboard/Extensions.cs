using MetX.Standard.Strings;
using MetX.Standard.Strings.Tokens;
using NHotPhrase.Phrase;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NHotPhrase.Keyboard
{
    public static class Extensions
    {
        public static string[] SplitInTwo(this string target)
        {
            var index = target.Length / 2;
            return new[]
            {
                target[..index],
                target[index..]
            };
        }

        public static bool OnlyDigits(this IList<PKey> keys)
        {
            foreach (var key in keys)
            {
                var keepGoing = false;
                switch (key)
                {
                    case PKey.D0:
                    case PKey.D1:
                    case PKey.D2:
                    case PKey.D3:
                    case PKey.D4:
                    case PKey.D5:
                    case PKey.D6:
                    case PKey.D7:
                    case PKey.D8:
                    case PKey.D9:
                    case PKey.NumPad0:
                    case PKey.NumPad1:
                    case PKey.NumPad2:
                    case PKey.NumPad3:
                    case PKey.NumPad4:
                    case PKey.NumPad5:
                    case PKey.NumPad6:
                    case PKey.NumPad7:
                    case PKey.NumPad8:
                    case PKey.NumPad9:
                        keepGoing = true;
                        break;
                }

                if (!keepGoing)
                    return false;
            }

            return true;
        }

        public static string AsString(this IList<PKey> keys)
        {
            if (keys == null || keys.Count < 1)
                return "";

            var result = "";
            foreach (var key in keys)
            {
                var keyString = "";
                switch (key)
                {
                    case PKey.Tab:
                        keyString = "\t";
                        break;
                    case PKey.LineFeed:
                        keyString = "\r";
                        break;
                    case PKey.Return:
                        keyString = "\n";
                        break;
                    case PKey.Space:
                        keyString = " ";
                        break;

                    case PKey.NumPad0:
                    case PKey.D0:
                        keyString = "0";
                        break;
                    case PKey.NumPad1:
                    case PKey.D1:
                        keyString = "1";
                        break;
                    case PKey.NumPad2:
                    case PKey.D2:
                        keyString = "2";
                        break;
                    case PKey.NumPad3:
                    case PKey.D3:
                        keyString = "3";
                        break;
                    case PKey.NumPad4:
                    case PKey.D4:
                        keyString = "4";
                        break;
                    case PKey.NumPad5:
                    case PKey.D5:
                        keyString = "5";
                        break;
                    case PKey.NumPad6:
                    case PKey.D6:
                        keyString = "6";
                        break;
                    case PKey.NumPad7:
                    case PKey.D7:
                        keyString = "7";
                        break;
                    case PKey.NumPad8:
                    case PKey.D8:
                        keyString = "8";
                        break;
                    case PKey.NumPad9:
                    case PKey.D9:
                        keyString = "9";
                        break;

                    case PKey.A:
                    case PKey.B:
                    case PKey.C:
                    case PKey.D:
                    case PKey.E:
                    case PKey.F:
                    case PKey.G:
                    case PKey.H:
                    case PKey.I:
                    case PKey.J:
                    case PKey.K:
                    case PKey.L:
                    case PKey.M:
                    case PKey.N:
                    case PKey.O:
                    case PKey.P:
                    case PKey.Q:
                    case PKey.R:
                    case PKey.S:
                    case PKey.T:
                    case PKey.U:
                    case PKey.V:
                    case PKey.W:
                    case PKey.X:
                    case PKey.Y:
                    case PKey.Z:
                        keyString = key.ToString();
                        break;

                    case PKey.Multiply:
                        keyString = "*";
                        break;
                    case PKey.Add:
                        keyString = "+";
                        break;
                    case PKey.Separator:
                    case PKey.Subtract:
                        keyString = "-";
                        break;
                    case PKey.Decimal:
                        keyString = ".";
                        break;
                    case PKey.Divide:
                        keyString = "/";
                        break;

                    case PKey.OemSemicolon:
                        keyString = ";";
                        break;
                    case PKey.Oemplus:
                        keyString = "+";
                        break;
                    case PKey.Oemcomma:
                        keyString = ",";
                        break;
                    case PKey.OemMinus:
                        keyString = "-";
                        break;
                    case PKey.OemPeriod:
                        keyString = ".";
                        break;
                    case PKey.OemQuestion:
                        keyString = "?";
                        break;
                    case PKey.Oemtilde:
                        keyString = "~";
                        break;
                    case PKey.OemOpenBrackets:
                        keyString = "[";
                        break;
                    case PKey.OemPipe:
                        keyString = "|";
                        break;
                    case PKey.OemCloseBrackets:
                        keyString = "]";
                        break;
                    case PKey.OemQuotes:
                        keyString = "\"";
                        break;
                    case PKey.Oem8:
                        keyString = "8";
                        break;
                    case PKey.OemBackslash:
                        keyString = "\\";
                        break;
                }

                result += keyString;
            }

            return result;
        }

        public static bool OnlyLetters(this IList<PKey> keys)
        {
            foreach (var key in keys)
            {
                var keepGoing = false;
                switch (key)
                {
                    case PKey.A:
                    case PKey.B:
                    case PKey.C:
                    case PKey.D:
                    case PKey.E:
                    case PKey.F:
                    case PKey.G:
                    case PKey.H:
                    case PKey.I:
                    case PKey.J:
                    case PKey.K:
                    case PKey.L:
                    case PKey.M:
                    case PKey.N:
                    case PKey.O:
                    case PKey.P:
                    case PKey.Q:
                    case PKey.R:
                    case PKey.S:
                    case PKey.T:
                    case PKey.U:
                    case PKey.V:
                    case PKey.W:
                    case PKey.X:
                    case PKey.Y:
                    case PKey.Z:
                        keepGoing = true;
                        break;
                }

                if (!keepGoing)
                    return false;
            }

            return true;
        }

        public static bool OnlyAlphaNumeric(this IList<PKey> keys)
        {
            foreach (var key in keys)
            {
                var keepGoing = false;
                switch (key)
                {
                    case PKey.A:
                    case PKey.B:
                    case PKey.C:
                    case PKey.D:
                    case PKey.E:
                    case PKey.F:
                    case PKey.G:
                    case PKey.H:
                    case PKey.I:
                    case PKey.J:
                    case PKey.K:
                    case PKey.L:
                    case PKey.M:
                    case PKey.N:
                    case PKey.O:
                    case PKey.P:
                    case PKey.Q:
                    case PKey.R:
                    case PKey.S:
                    case PKey.T:
                    case PKey.U:
                    case PKey.V:
                    case PKey.W:
                    case PKey.X:
                    case PKey.Y:
                    case PKey.Z:
                        keepGoing = true;
                        break;
                }

                if (!keepGoing)
                    return false;
            }

            return true;
        }

        public static bool IsAMatch(this PKey exactingPKey, PKey simplifiablePKey)
        {
            if ((int)exactingPKey == (int)simplifiablePKey)
                return true;

            exactingPKey = exactingPKey.FilterDuplicateEnumEntryNames();
            simplifiablePKey = simplifiablePKey.FilterDuplicateEnumEntryNames();

            if (!exactingPKey.ShouldBeSimplified())
                return (int)simplifiablePKey == (int)exactingPKey; // Must be exactly that pKey

            var exactingSimplified = exactingPKey.Simplify();
            var simplifiableSimplified = simplifiablePKey.Simplify();
            return (int)exactingSimplified == (int)simplifiableSimplified;
            // return ((int) exactingSimplified & (int) simplifiableSimplified) == (int) exactingSimplified;
        }

        public static PKey FilterDuplicateEnumEntryNames(this PKey pKey)
        {
            return pKey switch
            {
                // Some PKey entries have the same value but different enum entry names, which is sometimes troublesome
                PKey.Return => PKey.Enter,
                PKey.CapsLock => PKey.Capital,
                PKey.LButton | PKey.OemClear => PKey.Capital,
                _ => pKey
            };
        }

        public static bool ShouldBeSimplified(this PKey pKey)
        {
            switch (pKey)
            {
                // Specifying .LShiftKey (for instance) says you want to explicitly watch for the left shift pKey,
                //  but Using .Shift or .ShiftKey means either shift will do (likewise with control, alt, etc.)
                case PKey.Shift: // == LShiftKey or RShiftKey or ShiftKey
                case PKey.ShiftKey: // == LShiftKey or RShiftKey or Shift
                case PKey.Control: // == LControlKey or RControlKey or ControlKey
                case PKey.ControlKey: // == LControlKey or RControlKey or Control
                case PKey.Alt: // == LMenu or RMenu
                case PKey.LWin: // == RWin
                case PKey.D0: // == NumPad0
                case PKey.D1:
                case PKey.D2:
                case PKey.D3:
                case PKey.D4:
                case PKey.D5:
                case PKey.D6:
                case PKey.D7:
                case PKey.D8:
                case PKey.D9: // == NumPad9
                case PKey.OemMinus: // == Separator
                case PKey.Oemplus: // == Add
                    //case PKey.Capital:
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Simplifies combinations of pKey-like states to a single known value for use in a sequence
        ///     Example: PKey.Shift == PKey.RShiftKey or PKey.LShiftKey or PKey.ShiftKey
        /// </summary>
        /// <param name="pKey"></param>
        /// <returns>PKey.Enter, PKey.Shift, PKey.Control, PKey.Alt, PKey.LWin (there's no .Windows), D0 - D9, OemPlus, OemMinus</returns>
        public static PKey Simplify(this PKey pKey)
        {
            switch (pKey)
            {
                //case PKey.Enter:
                //case PKey.Return:
                //    return PKey.Enter; // .Enter and .Return return the exact same code (13)

                case PKey.Shift:
                case PKey.ShiftKey:
                case PKey.LShiftKey:
                case PKey.RShiftKey:
                    return PKey.Shift;

                case PKey.Control:
                case PKey.ControlKey:
                case PKey.RControlKey:
                case PKey.LControlKey:
                    return PKey.Control;

                case PKey.Alt:
                case PKey.LMenu:
                case PKey.RMenu:
                    return PKey.Alt;

                case PKey.LWin:
                case PKey.RWin:
                    return PKey.LWin;

                case PKey.D0:
                case PKey.NumPad0:
                    return PKey.D0;

                case PKey.D1:
                case PKey.NumPad1:
                    return PKey.D1;

                case PKey.D2:
                case PKey.NumPad2:
                    return PKey.D2;

                case PKey.D3:
                case PKey.NumPad3:
                    return PKey.D3;

                case PKey.D4:
                case PKey.NumPad4:
                    return PKey.D4;

                case PKey.D5:
                case PKey.NumPad5:
                    return PKey.D5;

                case PKey.D6:
                case PKey.NumPad6:
                    return PKey.D6;

                case PKey.D7:
                case PKey.NumPad7:
                    return PKey.D7;

                case PKey.D8:
                case PKey.NumPad8:
                    return PKey.D8;

                case PKey.D9:
                case PKey.NumPad9:
                    return PKey.D9;

                case PKey.Oemplus:
                case PKey.Add:
                    return PKey.Oemplus;

                case PKey.OemMinus:
                case PKey.Separator:
                    return PKey.OemMinus;
            }
            return pKey;
        }

        public static bool ShouldBeBackspaced(this PKey pKey)
        {
            switch (pKey)
            {
                case PKey.Enter:  // or .Return (same code)
                case PKey.Shift:
                case PKey.ShiftKey:
                case PKey.LShiftKey:
                case PKey.RShiftKey:
                case PKey.Control:
                case PKey.ControlKey:
                case PKey.RControlKey:
                case PKey.LControlKey:
                case PKey.Alt:
                case PKey.LMenu:
                case PKey.RMenu:
                case PKey.LWin:
                case PKey.RWin:
                case PKey.CapsLock:
                case PKey.Escape:
                case PKey.Print:
                case PKey.PrintScreen:
                case PKey.Pause:
                case PKey.Insert:
                case PKey.Delete:
                case PKey.Home:
                case PKey.End:
                case PKey.PageDown:
                case PKey.PageUp:
                case PKey.Up:
                case PKey.Down:
                case PKey.Left:
                case PKey.Right:
                case PKey.Back:
                    return false;
            }

            if (pKey is >= PKey.F1 and PKey.F24)
                return false;

            return true;
        }

        public static List<PKey> ToPKeyList(
            this string keys,
            List<PKey> prepend,
            out WildcardMatchType wildcardMatchType,
            out int wildcardCount)
        {
            var pKeyList = prepend.IsEmpty()
                ? new List<PKey>()
                : new List<PKey>(prepend);

            wildcardMatchType = WildcardMatchType.None;
            wildcardCount = 0;

            if (keys.IsEmpty()) return pKeyList;

            List<string> keyParts;
            if (keys.StartsWith("When")) keyParts = keys[4..].TrimStart().AllTokens();
            else if (keys.StartsWith("Or")) keyParts = keys[2..].TrimStart().AllTokens();
            else keyParts = keys.AllTokens();

            foreach (var keyPart in keyParts)
            {
                List<PKey> additionalKeys = new List<PKey>();
                var pKey = ToPKey(keyPart, out var wildcardMatchTypeInner, out var wildcardCountInner, additionalKeys);

                if (wildcardCountInner < 1)
                {
                    if (pKey != PKey.None)
                        pKeyList.Add(pKey);
                    if (additionalKeys.IsNotEmpty())
                        pKeyList.AddRange(additionalKeys);
                }
                else
                {
                    wildcardMatchType = wildcardMatchTypeInner;
                    wildcardCount = wildcardCountInner;
                }
            }
            return pKeyList;
        }

        public static PKey ToPKey(this string singlePKeyText, out WildcardMatchType wildcardMatchType, out int wildcardCount, List<PKey> additionalKeysFound)
        {
            wildcardMatchType = WildcardMatchType.None;
            wildcardCount = 0;

            if (singlePKeyText.IsEmpty()) return PKey.None;

            if (singlePKeyText.Length == 1 && char.IsLower(singlePKeyText[0]))
                singlePKeyText = singlePKeyText.ToUpper();

            if (singlePKeyText.All(x => x == '#'))
            {
                wildcardMatchType = WildcardMatchType.Digits;
                wildcardCount = singlePKeyText.Length;
                return PKey.None;
            }

            if (singlePKeyText.All(x => x == '*'))
            {
                wildcardMatchType = WildcardMatchType.AlphaNumeric;
                wildcardCount = singlePKeyText.Length;
                return PKey.None;
            }

            switch (singlePKeyText.ToUpper())
            {
                case "0": return PKey.D0;
                case "1": return PKey.D1;
                case "2": return PKey.D2;
                case "3": return PKey.D3;
                case "4": return PKey.D4;
                case "5": return PKey.D5;
                case "6": return PKey.D6;
                case "7": return PKey.D7;
                case "8": return PKey.D8;
                case "9": return PKey.D9;
            }

            if (Enum.TryParse(typeof(PKey), singlePKeyText, true, out object pKeyObject))
            {
                if (pKeyObject != null)
                {
                    PKey pKey = (PKey)pKeyObject;
                    if (pKey.ToString() == singlePKeyText)
                        return pKey;

                    if (pKey == PKey.CapsLock
                       && (singlePKeyText.ToLower() == "capslock"
                        || singlePKeyText.ToLower() == "captial"))
                        return pKey; // Special case

                    pKey = pKey.FilterDuplicateEnumEntryNames();
                    if (pKey.ToString() == singlePKeyText)
                        return pKey;
                }
            }
            if (singlePKeyText.Length > 0)
            {
                // Treat something like "123" as "1 2 3"
                foreach (var c in singlePKeyText)
                {
                    var key = c.ToString();
                    if (char.IsDigit(c))
                        key = "D" + c;
                    if (!Enum.TryParse(typeof(PKey), key, true, out pKeyObject)) continue;
                    if (pKeyObject == null) continue;

                    var pKey = (PKey)pKeyObject;
                    if (pKey.ToString() == key)
                        additionalKeysFound?.Add(pKey);
                    else
                    {
                        pKey = pKey.FilterDuplicateEnumEntryNames();
                        if (pKey.ToString() == key) additionalKeysFound?.Add(pKey);
                    }
                }
            }

            return PKey.None;
        }


    }
}