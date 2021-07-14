using System.Collections.Generic;

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
            if ((int) exactingPKey == (int) simplifiablePKey)
                return true;

            exactingPKey = exactingPKey.FilterDuplicateEnumEntryNames();
            simplifiablePKey = simplifiablePKey.FilterDuplicateEnumEntryNames();

            if (!exactingPKey.ShouldBeSimplified())
                return (int) simplifiablePKey == (int) exactingPKey; // Must be exactly that pKey

            var exactingSimplified = exactingPKey.Simplify();
            var simplifiableSimplified = simplifiablePKey.Simplify();
            return (int) exactingSimplified == (int) simplifiableSimplified;
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
    }
}