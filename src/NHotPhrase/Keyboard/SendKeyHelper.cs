using System;
using System.Collections.Generic;
using System.Linq;

namespace NHotPhrase.Keyboard
{
    public static class SendKeyHelper
    {
        public static SendPKeyEntry[] Entries { get; set; } =
        {
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
            new(")", 41, "{)}")
        };
    }
}