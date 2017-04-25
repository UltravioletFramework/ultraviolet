
namespace Ultraviolet.Input
{
    /// <summary>
    /// Represents the virtual, mapped keys on a keyboard.
    /// </summary>
    public enum Key
    {
        /// <summary>
        /// No key.
        /// </summary>
        None = 0,

        /// <summary>
        /// The Return/Enter key.
        /// </summary>
        Return = '\r',

        /// <summary>
        /// The Escape key.
        /// </summary>
        Escape = '\x1B',

        /// <summary>
        /// The Backspace key.
        /// </summary>
        Backspace = '\b',

        /// <summary>
        /// The Tab key.
        /// </summary>
        Tab = '\t',

        /// <summary>
        /// The space ( ) key.
        /// </summary>
        Space = ' ',

        /// <summary>
        /// The exclamation (!) key.
        /// </summary>
        Exclamation = '!',

        /// <summary>
        /// The double quote (") key.
        /// </summary>
        DoubleQuote = '"',

        /// <summary>
        /// The hash (#) key.
        /// </summary>
        Hash = '#',

        /// <summary>
        /// The percent (%) key.
        /// </summary>
        Percent = '%',

        /// <summary>
        /// The dollar ($) key.
        /// </summary>
        Dollar = '$',

        /// <summary>
        /// The ampersand (&amp;) key.
        /// </summary>
        Ampersand = '&',

        /// <summary>
        /// The single quote (') key.
        /// </summary>
        SingleQuote = '\'',

        /// <summary>
        /// The left parenthesis key.
        /// </summary>
        LeftParenthesis = '(',

        /// <summary>
        /// The right parenthesis key.
        /// </summary>
        RightParenthesis = ')',

        /// <summary>
        /// The asterisk (*) key.
        /// </summary>
        Asterisk = '*',

        /// <summary>
        /// The plus (+) key.
        /// </summary>
        Plus = '+',

        /// <summary>
        /// The comma (,) key.
        /// </summary>
        Comma = ',',

        /// <summary>
        /// The minus (-) key.
        /// </summary>
        Minus = '-',

        /// <summary>
        /// The period (.) key.
        /// </summary>
        Period = '.',

        /// <summary>
        /// The slash (/) key.
        /// </summary>
        Slash = '/',

        /// <summary>
        /// The 0 key.
        /// </summary>
        D0 = '0',

        /// <summary>
        /// The 1 key.
        /// </summary>
        D1 = '1',

        /// <summary>
        /// The 2 key.
        /// </summary>
        D2 = '2',

        /// <summary>
        /// The 3 key.
        /// </summary>
        D3 = '3',

        /// <summary>
        /// The 4 key.
        /// </summary>
        D4 = '4',

        /// <summary>
        /// The 5 key.
        /// </summary>
        D5 = '5',

        /// <summary>
        /// The 6 key.
        /// </summary>
        D6 = '6',

        /// <summary>
        /// The 7 key.
        /// </summary>
        D7 = '7',

        /// <summary>
        /// The 8 key.
        /// </summary>
        D8 = '8',

        /// <summary>
        /// The 9 key.
        /// </summary>
        D9 = '9',

        /// <summary>
        /// The colon (:) key.
        /// </summary>
        Colon = ':',

        /// <summary>
        /// The semicolon (;) key.
        /// </summary>
        Semicolon = ';',

        /// <summary>
        /// The less than (&lt;) key.
        /// </summary>
        Less = '<',

        /// <summary>
        /// The equals (=) key.
        /// </summary>
        Equals = '=',

        /// <summary>
        /// The greater than (&gt;) key.
        /// </summary>
        Greater = '>',

        /// <summary>
        /// The question mark (?) key.
        /// </summary>
        Question = '?',

        /// <summary>
        /// The at sign (@) key.
        /// </summary>
        At = '@',

        /// <summary>
        /// The left bracket ([) key.
        /// </summary>
        LeftBracket = '[',

        /// <summary>
        /// The backslash (\) key.
        /// </summary>
        Backslash = '\\',

        /// <summary>
        /// The right bracket (]) key.
        /// </summary>
        RightBracket = ']',

        /// <summary>
        /// The caret (^) key.
        /// </summary>
        Caret = '^',

        /// <summary>
        /// The underscore (_) key.
        /// </summary>
        Underscore = '_',

        /// <summary>
        /// The back quote (`) key.
        /// </summary>
        BackQuote = '`',

        /// <summary>
        /// The A key.
        /// </summary>
        A = 'a',

        /// <summary>
        /// The B key.
        /// </summary>
        B = 'b',

        /// <summary>
        /// The C key.
        /// </summary>
        C = 'c',

        /// <summary>
        /// The D key.
        /// </summary>
        D = 'd',

        /// <summary>
        /// The E key.
        /// </summary>
        E = 'e',

        /// <summary>
        /// The F key.
        /// </summary>
        F = 'f',

        /// <summary>
        /// The G key.
        /// </summary>
        G = 'g',

        /// <summary>
        /// The H key.
        /// </summary>
        H = 'h',

        /// <summary>
        /// The I key.
        /// </summary>
        I = 'i',

        /// <summary>
        /// The J key.
        /// </summary>
        J = 'j',

        /// <summary>
        /// The K key.
        /// </summary>
        K = 'k',

        /// <summary>
        /// The L key.
        /// </summary>
        L = 'l',

        /// <summary>
        /// The M key.
        /// </summary>
        M = 'm',

        /// <summary>
        /// The N key.
        /// </summary>
        N = 'n',

        /// <summary>
        /// The O key.
        /// </summary>
        O = 'o',

        /// <summary>
        ///  The P key.
        /// </summary>
        P = 'p',

        /// <summary>
        /// The Q key.
        /// </summary>
        Q = 'q',

        /// <summary>
        /// The R key.
        /// </summary>
        R = 'r',

        /// <summary>
        /// The S key.
        /// </summary>
        S = 's',

        /// <summary>
        /// The T key.
        /// </summary>
        T = 't',

        /// <summary>
        /// The U key.
        /// </summary>
        U = 'u',

        /// <summary>
        /// The V key.
        /// </summary>
        V = 'v',

        /// <summary>
        /// The W key.
        /// </summary>
        W = 'w',

        /// <summary>
        /// The X key.
        /// </summary>
        X = 'x',

        /// <summary>
        /// The Y key.
        /// </summary>
        Y = 'y',

        /// <summary>
        /// The Z key.
        /// </summary>
        Z = 'z',

        /// <summary>
        /// The Caps Lock key.
        /// </summary>
        CapsLock = Scancode.CapsLock | 0x40000000,

        /// <summary>
        /// The F1 function key.
        /// </summary>
        F1 = Scancode.F1 | 0x40000000,

        /// <summary>
        /// The F2 function key.
        /// </summary>
        F2 = Scancode.F2 | 0x40000000,

        /// <summary>
        /// The F3 function key.
        /// </summary>
        F3 = Scancode.F3 | 0x40000000,

        /// <summary>
        /// The F4 function key.
        /// </summary>
        F4 = Scancode.F4 | 0x40000000,

        /// <summary>
        /// The F5 function key.
        /// </summary>
        F5 = Scancode.F5 | 0x40000000,

        /// <summary>
        /// The F6 function key.
        /// </summary>
        F6 = Scancode.F6 | 0x40000000,

        /// <summary>
        /// The F7 function key.
        /// </summary>
        F7 = Scancode.F7 | 0x40000000,

        /// <summary>
        /// The F8 function key.
        /// </summary>
        F8 = Scancode.F8 | 0x40000000,

        /// <summary>
        /// The F9 function key.
        /// </summary>
        F9 = Scancode.F9 | 0x40000000,

        /// <summary>
        /// The F10 function key.
        /// </summary>
        F10 = Scancode.F10 | 0x40000000,

        /// <summary>
        /// The F11 function key.
        /// </summary>
        F11 = Scancode.F11 | 0x40000000,

        /// <summary>
        /// The F12 function key.
        /// </summary>
        F12 = Scancode.F12 | 0x40000000,

        /// <summary>
        /// The Print Screen key.
        /// </summary>
        PrintScreen = Scancode.PrintScreen | 0x40000000,

        /// <summary>
        /// The Scroll Lock key.
        /// </summary>
        ScrollLock = Scancode.ScrollLock | 0x40000000,

        /// <summary>
        /// The Pause key.
        /// </summary>
        Pause = Scancode.Pause | 0x40000000,

        /// <summary>
        /// The Insert key.
        /// </summary>
        Insert = Scancode.Insert | 0x40000000,

        /// <summary>
        /// The Home key.
        /// </summary>
        Home = Scancode.Home | 0x40000000,

        /// <summary>
        /// The Page Up key.
        /// </summary>
        PageUp = Scancode.PageUp | 0x40000000,

        /// <summary>
        /// The Delete key.
        /// </summary>
        Delete = '\x7F',

        /// <summary>
        /// The End key.
        /// </summary>
        End = Scancode.End | 0x40000000,

        /// <summary>
        /// The Page Down key.
        /// </summary>
        PageDown = Scancode.PageDown | 0x40000000,

        /// <summary>
        /// The right arrow key.
        /// </summary>
        Right = Scancode.Right | 0x40000000,

        /// <summary>
        /// The left arrow key.
        /// </summary>
        Left = Scancode.Left | 0x40000000,

        /// <summary>
        /// The down arrow key.
        /// </summary>
        Down = Scancode.Down | 0x40000000,

        /// <summary>
        /// The up arrow key.
        /// </summary>
        Up = Scancode.Up | 0x40000000,

        /// <summary>
        /// The Num Lock/Clear key.
        /// </summary>
        NumLockClear = Scancode.NumLockClear | 0x40000000,

        /// <summary>
        /// The divide (/) key on the keypad.
        /// </summary>
        KeypadDivide = Scancode.KeypadDivide | 0x40000000,

        /// <summary>
        /// The multiply (*) key on the keypad.
        /// </summary>
        KeypadMultiply = Scancode.KeypadMultiply | 0x40000000,

        /// <summary>
        /// The minus (-) key on the keypad.
        /// </summary>
        KeypadMinus = Scancode.KeypadMinus | 0x40000000,

        /// <summary>
        /// The plus (+) key on the keypad.
        /// </summary>
        KeypadPlus = Scancode.KeypadPlus | 0x40000000,

        /// <summary>
        /// The Enter/Return key on the keypad.
        /// </summary>
        KeypadEnter = Scancode.KeypadEnter | 0x40000000,

        /// <summary>
        /// The 1 key on the keypad.
        /// </summary>
        KeypadD1 = Scancode.KeypadD1 | 0x40000000,

        /// <summary>
        /// The 2 key on the keypad.
        /// </summary>
        KeypadD2 = Scancode.KeypadD2 | 0x40000000,

        /// <summary>
        /// The 3 key on the keypad.
        /// </summary>
        KeypadD3 = Scancode.KeypadD3 | 0x40000000,

        /// <summary>
        /// The 4 key on the keypad.
        /// </summary>
        KeypadD4 = Scancode.KeypadD4 | 0x40000000,

        /// <summary>
        /// The 5 key on the keypad.
        /// </summary>
        KeypadD5 = Scancode.KeypadD5 | 0x40000000,

        /// <summary>
        /// The 6 key on the keypad.
        /// </summary>
        KeypadD6 = Scancode.KeypadD6 | 0x40000000,

        /// <summary>
        /// The 7 key on the keypad.
        /// </summary>
        KeypadD7 = Scancode.KeypadD7 | 0x40000000,

        /// <summary>
        /// The 8 key on the keypad.
        /// </summary>
        KeypadD8 = Scancode.KeypadD8 | 0x40000000,

        /// <summary>
        /// The 9 key on the keypad.
        /// </summary>
        KeypadD9 = Scancode.KeypadD9 | 0x40000000,

        /// <summary>
        /// The 9 key on the keypad.
        /// </summary>
        KeypadD0 = Scancode.KeypadD0 | 0x40000000,

        /// <summary>
        /// The period (.) key on the keypad.
        /// </summary>
        KeypadPeriod = Scancode.KeypadPeriod | 0x40000000,

        /// <summary>
        /// The Application key.
        /// </summary>
        Application = Scancode.Application | 0x40000000,

        /// <summary>
        /// The Power key.
        /// </summary>
        Power = Scancode.Power | 0x40000000,

        /// <summary>
        /// The equals (=) key on the keypad.
        /// </summary>
        KeypadEquals = Scancode.KeypadEquals | 0x40000000,

        /// <summary>
        /// The F13 function key.
        /// </summary>
        F13 = Scancode.F13 | 0x40000000,

        /// <summary>
        /// The F14 function key.
        /// </summary>
        F14 = Scancode.F14 | 0x40000000,

        /// <summary>
        /// The F15 function key.
        /// </summary>
        F15 = Scancode.F15 | 0x40000000,

        /// <summary>
        /// The F16 function key.
        /// </summary>
        F16 = Scancode.F16 | 0x40000000,

        /// <summary>
        /// The F17 function key.
        /// </summary>
        F17 = Scancode.F17 | 0x40000000,

        /// <summary>
        /// The F18 function key.
        /// </summary>
        F18 = Scancode.F18 | 0x40000000,

        /// <summary>
        /// The F19 function key.
        /// </summary>
        F19 = Scancode.F19 | 0x40000000,

        /// <summary>
        /// The F20 function key.
        /// </summary>
        F20 = Scancode.F20 | 0x40000000,

        /// <summary>
        /// The F21 function key.
        /// </summary>
        F21 = Scancode.F21 | 0x40000000,

        /// <summary>
        /// The F22 function key.
        /// </summary>
        F22 = Scancode.F22 | 0x40000000,

        /// <summary>
        /// The F23 function key.
        /// </summary>
        F23 = Scancode.F23 | 0x40000000,

        /// <summary>
        /// The F24 function key.
        /// </summary>
        F24 = Scancode.F24 | 0x40000000,

        /// <summary>
        /// The Execute key.
        /// </summary>
        Execute = Scancode.Execute | 0x40000000,

        /// <summary>
        /// The Help key.
        /// </summary>
        Help = Scancode.Help | 0x40000000,

        /// <summary>
        /// The Menu key.
        /// </summary>
        Menu = Scancode.Menu | 0x40000000,

        /// <summary>
        /// The Select key.
        /// </summary>
        Select = Scancode.Select | 0x40000000,

        /// <summary>
        /// The Stop key.
        /// </summary>
        Stop = Scancode.Stop | 0x40000000,

        /// <summary>
        /// The Again key.
        /// </summary>
        Again = Scancode.Again | 0x40000000,

        /// <summary>
        /// The Undo key.
        /// </summary>
        Undo = Scancode.Undo | 0x40000000,

        /// <summary>
        /// The Cut key.
        /// </summary>
        Cut = Scancode.Cut | 0x40000000,

        /// <summary>
        /// The Copy key.
        /// </summary>
        Copy = Scancode.Copy | 0x40000000,

        /// <summary>
        /// The Paste key.
        /// </summary>
        Paste = Scancode.Paste | 0x40000000,

        /// <summary>
        /// The Find key.
        /// </summary>
        Find = Scancode.Find | 0x40000000,

        /// <summary>
        /// The Mute key.
        /// </summary>
        Mute = Scancode.Mute | 0x40000000,

        /// <summary>
        /// The Volume Up key.
        /// </summary>
        VolumeUp = Scancode.VolumeUp | 0x40000000,

        /// <summary>
        /// The Volume Down key.
        /// </summary>
        VolumeDown = Scancode.VolumeDown | 0x40000000,

        /// <summary>
        /// The comma (,) key on the keypad.
        /// </summary>
        KeypadComma = Scancode.KeypadComma | 0x40000000,

        /// <summary>
        /// The equals key on an AS/400 keypad.
        /// </summary>
        KeypadEqualsAS400 = Scancode.KeypadEqualsAS400 | 0x40000000,
        
        /// <summary>
        /// The Alternate Erase key.
        /// </summary>
        AltErase = Scancode.AltErase | 0x40000000,

        /// <summary>
        /// The SysReq key.
        /// </summary>
        SysReq = Scancode.SysReq | 0x40000000,

        /// <summary>
        /// The Cancel key.
        /// </summary>
        Cancel = Scancode.Cancel | 0x40000000,

        /// <summary>
        /// The Clear key.
        /// </summary>
        Clear = Scancode.Clear | 0x40000000,

        /// <summary>
        /// The Prior key.
        /// </summary>
        Prior = Scancode.Prior | 0x40000000,

        /// <summary>
        /// The second Return/Enter key.
        /// </summary>
        Return2 = Scancode.Return2 | 0x40000000,

        /// <summary>
        /// The Separator key.
        /// </summary>
        Separator = Scancode.Separator | 0x40000000,

        /// <summary>
        /// The Out key.
        /// </summary>
        Out = Scancode.Out | 0x40000000,

        /// <summary>
        /// The Oper key.
        /// </summary>
        Oper = Scancode.Oper | 0x40000000,

        /// <summary>
        /// The Clear/Again key.
        /// </summary>
        ClearAgain = Scancode.ClearAgain | 0x40000000,

        /// <summary>
        /// The CrSel key.
        /// </summary>
        CrSel = Scancode.CrSel | 0x40000000,

        /// <summary>
        /// The ExSel key.
        /// </summary>
        ExSel = Scancode.ExSel | 0x40000000,

        /// <summary>
        /// The 00 key on the keypad.
        /// </summary>
        Keypad00 = Scancode.Keypad00 | 0x40000000,

        /// <summary>
        /// The 000 key on the keypad.
        /// </summary>
        Keypad000 = Scancode.Keypad000 | 0x40000000,

        /// <summary>
        /// The thousands separator key
        /// </summary>
        ThousandsSeparator = Scancode.ThousandsSeparator | 0x40000000,

        /// <summary>
        /// The decimal separator key.
        /// </summary>
        DecimalSeparator = Scancode.DecimalSeparator | 0x40000000,

        /// <summary>
        /// The currency unit key.
        /// </summary>
        CurrencyUnit = Scancode.CurrencyUnit | 0x40000000,

        /// <summary>
        /// The currency sub-unit key.
        /// </summary>
        CurrencySubUnit = Scancode.CurrencySubUnit | 0x40000000,

        /// <summary>
        /// The left parenthesis key on the keypad.
        /// </summary>
        KeypadLeftParenthesis = Scancode.KeypadLeftParenthesis | 0x40000000,

        /// <summary>
        /// The right parenthesis key on the keypad.
        /// </summary>
        KeypadRightParenthesis = Scancode.KeypadRightParenthesis | 0x40000000,

        /// <summary>
        /// The left brace ([) key on the keypad.
        /// </summary>
        KeypadLeftBrace = Scancode.KeypadLeftBrace | 0x40000000,

        /// <summary>
        /// The right brace (]) key on the keypad.
        /// </summary>
        KeypadRightBrace = Scancode.KeypadRightBrace | 0x40000000,

        /// <summary>
        /// The Tab key on the keypad.
        /// </summary>
        KeypadTab = Scancode.KeypadTab | 0x40000000,

        /// <summary>
        /// The Backspace key on the keypad.
        /// </summary>
        KeypadBackspace = Scancode.KeypadBackspace | 0x40000000,

        /// <summary>
        /// The A key on the keypad.
        /// </summary>
        KeypadA = Scancode.KeypadA | 0x40000000,

        /// <summary>
        /// The B key on the keypad.
        /// </summary>
        KeypadB = Scancode.KeypadB | 0x40000000,

        /// <summary>
        /// The C key on the keypad.
        /// </summary>
        KeypadC = Scancode.KeypadC | 0x40000000,

        /// <summary>
        /// The D key on the keypad.
        /// </summary>
        KeypadD = Scancode.KeypadD | 0x40000000,

        /// <summary>
        /// The E key on the keypad.
        /// </summary>
        KeypadE = Scancode.KeypadE | 0x40000000,

        /// <summary>
        /// The F key on the keypad.
        /// </summary>
        KeypadF = Scancode.KeypadF | 0x40000000,

        /// <summary>
        /// The XOR key on the keypad.
        /// </summary>
        KeypadXor = Scancode.KeypadXor | 0x40000000,

        /// <summary>
        /// The Power key on the keypad.
        /// </summary>
        KeypadPower = Scancode.KeypadPower | 0x40000000,

        /// <summary>
        /// The percent (%) key on the keypad.
        /// </summary>
        KeypadPercent = Scancode.KeypadPercent | 0x40000000,

        /// <summary>
        /// The less than (&lt;) key on the keypad.
        /// </summary>
        KeypadLess = Scancode.KeypadLess | 0x40000000,

        /// <summary>
        /// The greater than (&gt;) key on the keypad.
        /// </summary>
        KeypadGreater = Scancode.KeypadGreater | 0x40000000,

        /// <summary>
        /// The ampersand (&amp;) key on the keypad.
        /// </summary>
        KeypadAmpersand = Scancode.KeypadAmpersand | 0x40000000,

        /// <summary>
        /// The double ampersand (&amp;&amp;) key on the keypad.
        /// </summary>
        KeypadDoubleAmpersand = Scancode.KeypadDoubleAmpersand | 0x40000000,

        /// <summary>
        /// The vertical bar (|) key on the keypad.
        /// </summary>
        KeypadVerticalBar = Scancode.KeypadVerticalBar | 0x40000000,

        /// <summary>
        /// The double vertical bar (||) key on the keypad.
        /// </summary>
        KeypadDoubleVerticalBar = Scancode.KeypadDoubleVerticalBar | 0x40000000,

        /// <summary>
        /// The colon (:) key on the keypad.
        /// </summary>
        KeypadColon = Scancode.KeypadColon | 0x40000000,

        /// <summary>
        /// The hash (#) key on the keypad.
        /// </summary>
        KeypadHash = Scancode.KeypadHash | 0x40000000,

        /// <summary>
        /// The space ( ) key on the keypad.
        /// </summary>
        KeypadSpace = Scancode.KeypadSpace | 0x40000000,
        
        /// <summary>
        /// The at sign (@) key on the keypad.
        /// </summary>
        KeypadAt = Scancode.KeypadAt | 0x40000000,

        /// <summary>
        /// The exclamation mark (!) key on the keypad.
        /// </summary>
        KeypadExclamation = Scancode.KeypadExclamation | 0x40000000,

        /// <summary>
        /// The Mem Store key on the keypad.
        /// </summary>
        KeypadMemStore = Scancode.KeypadMemStore | 0x40000000,

        /// <summary>
        /// The Mem Recall key on the keypad.
        /// </summary>
        KeypadMemRecall = Scancode.KeypadMemRecall | 0x40000000,

        /// <summary>
        /// The Mem Clear key on the keypad.
        /// </summary>
        KeypadMemClear = Scancode.KeypadMemClear | 0x40000000,

        /// <summary>
        /// The Mem Add key on the keypad.
        /// </summary>
        KeypadMemAdd = Scancode.KeypadMemAdd | 0x40000000,

        /// <summary>
        /// The Mem Subtract key on the keypad.
        /// </summary>
        KeypadMemSubtract = Scancode.KeypadMemSubtract | 0x40000000,

        /// <summary>
        /// The Mem Multiply key on the keypad.
        /// </summary>
        KeypadMemMultiply = Scancode.KeypadMemMultiply | 0x40000000,

        /// <summary>
        /// The Mem Divide key on the keypad.
        /// </summary>
        KeypadMemDivide = Scancode.KeypadMemDivide | 0x40000000,

        /// <summary>
        /// The plus/minus key on the keypad.
        /// </summary>
        KeypadPlusMinus = Scancode.KeypadPlusMinus | 0x40000000,

        /// <summary>
        /// The Clear key on the keypad.
        /// </summary>
        KeypadClear = Scancode.KeypadClear | 0x40000000,

        /// <summary>
        /// The Clear Entry key on the keypad.
        /// </summary>
        KeypadClearEntry = Scancode.KeypadClearEntry | 0x40000000,

        /// <summary>
        /// The Binary key on the keypad.
        /// </summary>
        KeypadBinary = Scancode.KeypadBinary | 0x40000000,

        /// <summary>
        /// The Octal key on the keypad.
        /// </summary>
        KeypadOctal = Scancode.KeypadOctal | 0x40000000,

        /// <summary>
        /// The Decimal key on the keypad.
        /// </summary>
        KeypadDecimal = Scancode.KeypadDecimal | 0x40000000,

        /// <summary>
        /// The Hexadecimal key on the keypad.
        /// </summary>
        KeypadHexadecimal = Scancode.KeypadHexadecimal | 0x40000000,

        /// <summary>
        /// The left Control key.
        /// </summary>
        LeftControl = Scancode.LeftControl | 0x40000000,

        /// <summary>
        /// The left Shift key.
        /// </summary>
        LeftShift = Scancode.LeftShift | 0x40000000,

        /// <summary>
        /// The left Alt key.
        /// </summary>
        LeftAlt = Scancode.LeftAlt | 0x40000000,

        /// <summary>
        /// The left GUI key (i.e. the Windows key on Windows).
        /// </summary>
        LeftGui = Scancode.LeftGui | 0x40000000,

        /// <summary>
        /// The right Control key.
        /// </summary>
        RightControl = Scancode.RightControl | 0x40000000,

        /// <summary>
        /// The right Shift key.
        /// </summary>
        RightShift = Scancode.RightShift | 0x40000000,

        /// <summary>
        /// The right Alt key.
        /// </summary>
        RightAlt = Scancode.RightAlt | 0x40000000,

        /// <summary>
        /// The right GUI key (i.e. the Windows key on Windows).
        /// </summary>
        RightGui = Scancode.RightGui | 0x40000000,

        /// <summary>
        /// The Mode key.
        /// </summary>
        Mode = Scancode.Mode | 0x40000000,

        /// <summary>
        /// The Audio Next key.
        /// </summary>
        AudioNext = Scancode.AudioNext | 0x40000000,

        /// <summary>
        /// The Audio Prev key.
        /// </summary>
        AudioPrev = Scancode.AudioPrev | 0x40000000,

        /// <summary>
        /// The Audio Stop key.
        /// </summary>
        AudioStop = Scancode.AudioStop | 0x40000000,

        /// <summary>
        /// The Audio Play key.
        /// </summary>
        AudioPlay = Scancode.AudioPlay | 0x40000000,

        /// <summary>
        /// The Audio Mute key.
        /// </summary>
        AudioMute = Scancode.AudioMute | 0x40000000,

        /// <summary>
        /// The Media Select key.
        /// </summary>
        MediaSelect = Scancode.MediaSelect | 0x40000000,

        /// <summary>
        /// The World Wide Web key.
        /// </summary>
        WorldWideWeb = Scancode.WorldWideWeb | 0x40000000,

        /// <summary>
        /// The Mail key.
        /// </summary>
        Mail = Scancode.Mail | 0x40000000,

        /// <summary>
        /// The Calculator key.
        /// </summary>
        Calculator = Scancode.Calculator | 0x40000000,

        /// <summary>
        /// The Computer key.
        /// </summary>
        Computer = Scancode.Computer | 0x40000000,

        /// <summary>
        /// The Search application control key.
        /// </summary>
        AppControlSearch = Scancode.AppControlSearch | 0x40000000,

        /// <summary>
        /// The Home application control key.
        /// </summary>
        AppControlHome = Scancode.AppControlHome | 0x40000000,

        /// <summary>
        /// The Back application control key.
        /// </summary>
        AppControlBack = Scancode.AppControlBack | 0x40000000,

        /// <summary>
        /// The Forward application control key.
        /// </summary>
        AppControlForward = Scancode.AppControlForward | 0x40000000,

        /// <summary>
        /// The Stop application control key.
        /// </summary>
        AppControlStop = Scancode.AppControlStop | 0x40000000,

        /// <summary>
        /// The Refresh application control key.
        /// </summary>
        AppControlRefresh = Scancode.AppControlRefresh | 0x40000000,

        /// <summary>
        /// The Bookmarks application control key.
        /// </summary>
        AppControlBookmarks = Scancode.AppControlBookmarks | 0x40000000,

        /// <summary>
        /// The Brightness Down key.
        /// </summary>
        BrightnessDown = Scancode.BrightnessDown | 0x40000000,

        /// <summary>
        /// The Brightness Up key.
        /// </summary>
        BrightnessUp = Scancode.BrightnessUp | 0x40000000,

        /// <summary>
        /// The Display Switch key.
        /// </summary>
        DisplaySwitch = Scancode.DisplaySwitch | 0x40000000,

        /// <summary>
        /// The Illumination Toggle key.
        /// </summary>
        IlluminationToggle = Scancode.IlluminationToggle | 0x40000000,

        /// <summary>
        /// The Illumination Down key.
        /// </summary>
        IlluminationDown = Scancode.IlluminationDown | 0x40000000,

        /// <summary>
        /// The Illumination Up key.
        /// </summary>
        IlluminationUp = Scancode.IlluminationUp | 0x40000000,

        /// <summary>
        /// The Eject key.
        /// </summary>
        Eject = Scancode.Eject | 0x40000000,

        /// <summary>
        /// The Sleep key.
        /// </summary>
        Sleep = Scancode.Sleep | 0x40000000
    }
}
