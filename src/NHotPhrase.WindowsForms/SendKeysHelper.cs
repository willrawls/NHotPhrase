using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using NHotPhrase.Keyboard;
using NHotPhrase.Phrase;

namespace NHotPhrase.WindowsForms
{
    public class SendKeysProxyForWinForms : ISendKeys
    {
        public bool SendKeysAndWait(PhraseActionRunState phraseActionRunState, List<PKey> keysToSend)
        {
            if (keysToSend is not {Count: > 0}) 
                return true;
            foreach (var key in keysToSend)
                SendKeys.SendWait(key.KeyToSendKeyText());
            return true;
        }

        public bool SendKeysAndWait(string stringToSend, int millisecondThreadSleep = 2)
        {
            SendKeys.SendWait(stringToSend);
            if(millisecondThreadSleep > 0)
                Thread.Sleep(millisecondThreadSleep);
            return true;
        }

        public bool SendKeysAndWait(List<string> stringsToSend, int millisecondThreadSleep = 2)
        {
            foreach (var part in stringsToSend) 
                SendKeysAndWait(part, 2);
            return true;
        }

        private static ISendKeys _singleton;
        public static ISendKeys Singleton => _singleton ??= new SendKeysProxyForWinForms();
    }

    public static class SendKeysHelper
    {
        public static string KeyToSendKeyText(this PKey pKey)
        {
            var keyword = SendKeyEntries.FirstOrDefault(k => k.Number == (int) pKey);
            return keyword != null 
                ? keyword.Name 
                : pKey.ToString();
        }

        public static void SendBackspaces(int backspaceCount)
        {
            var toSend = "";
            for (var i = 0; i < backspaceCount; i++)
            {
                toSend += "{BACKSPACE}";
            }

            SendKeysProxyForWinForms.Singleton.SendKeysAndWait(toSend);
        }

        public static readonly SendKeyEntry[] SendKeyEntries = {
            new("ENTER", 13),
            new("TAB", 9),
            new("ESC", 27),
            new("ESCAPE", 27),
            new("HOME", 36),
            new("END", 35),
            new("LEFT", 37),
            new("RIGHT", 39),
            new("UP", 38),
            new("DOWN", 40),
            new("PGUP", 33),
            new("PGDN", 34),
            new("NUMLOCK", 144),
            new("SCROLLLOCK", 145),
            new("PRTSC", 44),
            new("BREAK", 3),
            new("BACKSPACE", 8),
            new("BKSP", 8),
            new("BS", 8),
            new("CLEAR", 12),
            new("CAPSLOCK", 20),
            new("INS", 45),
            new("INSERT", 45),
            new("DEL", 46),
            new("DELETE", 46),
            new("HELP", 47),
            new("F1", 112),
            new("F2", 113),
            new("F3", 114),
            new("F4", 115),
            new("F5", 116),
            new("F6", 117),
            new("F7", 118),
            new("F8", 119),
            new("F9", 120),
            new("F10", 121),
            new("F11", 122),
            new("F12", 123),
            new("F13", 124),
            new("F14", 125),
            new("F15", 126),
            new("F16", 127),
            new("MULTIPLY", 106),
            new("ADD", 107),
            new("SUBTRACT", 109),
            new("DIVIDE", 111),

            // To specify brace characters, use "{{}" and "{}}". Brackets ([ ]) have no special meaning to SendKeys, but you must enclose them in braces.
            new("{", 123, "{{}"),
            new("}", 125, "}{}"),
            new("[", 91, "{[}"),
            new("]", 93, "}]}"),

            // The plus sign (+), caret (^), percent sign (%), tilde (~), and parentheses () have special meanings to SendKeys.
            new("+", 107, "{ADD}"),
            new("^", 65590, "{^}"),
            new("%", 65589, "{%}"),
            new("~", 13, "{ENTER}"),
            new("(", 40, "{(}"),
            new(")", 41, "{)}"),
        };



        public static List<string> MakeReadyForSendKeys(this string target, int splitLength = 8)
        {
            if (string.IsNullOrEmpty(target))
                return new List<string>();

            foreach (var keyword in SendKeysHelper.SendKeyEntries.Where(k => !string.IsNullOrEmpty(k.ReplaceWith)))
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
    }
}