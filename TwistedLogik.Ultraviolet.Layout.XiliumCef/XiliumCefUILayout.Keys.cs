using System;
using System.Collections.Generic;
using TwistedLogik.Ultraviolet.Input;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    public sealed partial class XiliumCefUILayout
    {
        /// <summary>
        /// Maps Ultraviolet Key values to Win32 virtual-key codes.
        /// </summary>
        private static readonly Dictionary<Int32, Int32> KeyMap = new Dictionary<Int32, Int32>
        {
            { (Int32)Key.Backspace,         	0x08 },
            { (Int32)Key.Tab,               	0x09 },
            { (Int32)Key.Clear,             	0x0C },
            { (Int32)Key.Return,            	0x0D },
            /* VK_SHIFT                         	*/
            /* VK_CONTROL                       	*/
            { (Int32)Key.Menu,              	0x12 },
            { (Int32)Key.Pause,             	0x13 },
            { (Int32)Key.CapsLock,          	0x14 },
            /* VK_KANA                          	*/
            /* VK_HANGUEL                       	*/
            /* VK_HANGUL                        	*/
            /* VK_JUNJA                         	*/
            /* VK_FINAL                         	*/
            /* VK_HANJA                         	*/
            /* VK_KANJI                         	*/
            { (Int32)Key.Escape,            	0x1B },
            /* VK_CONVERT                       	*/
            /* VK_NONCONVERT                    	*/
            /* VK_ACCEPT                        	*/
            /* VK_MODECHANGE                    	*/
            { (Int32)Key.Space,             	0x20 },
            { (Int32)Key.PageUp,            	0x21 },
            { (Int32)Key.PageDown,          	0x22 },
            { (Int32)Key.End,               	0x23 },
            { (Int32)Key.Home,              	0x24 },            
            { (Int32)Key.Left,              	0x25 },
            { (Int32)Key.Up,                	0x26 },
            { (Int32)Key.Right,             	0x27 },
            { (Int32)Key.Down,              	0x28 },
            { (Int32)Key.Select,            	0x29 },
            /* VK_PRINT                         	*/
            { (Int32)Key.Execute,           	0x2B },
            { (Int32)Key.PrintScreen,       	0x2C },
            { (Int32)Key.Insert,            	0x2D },
            { (Int32)Key.Delete,            	0x2E },
            { (Int32)Key.Help,              	0x2F },
            { (Int32)Key.D0,                	0x30 },
            { (Int32)Key.D1,                	0x31 },
            { (Int32)Key.D2,                	0x32 },
            { (Int32)Key.D3,                	0x33 },
            { (Int32)Key.D4,                	0x34 },
            { (Int32)Key.D5,                	0x35 },
            { (Int32)Key.D6,                	0x36 },
            { (Int32)Key.D7,                	0x37 },
            { (Int32)Key.D8,                	0x38 },
            { (Int32)Key.D9,                	0x39 },
            { (Int32)Key.A,                 	0x41 },
            { (Int32)Key.B,                 	0x42 },
            { (Int32)Key.C,                 	0x43 },
            { (Int32)Key.D,                 	0x44 },
            { (Int32)Key.E,                 	0x45 },
            { (Int32)Key.F,                 	0x46 },
            { (Int32)Key.G,                 	0x47 },
            { (Int32)Key.H,                 	0x48 },
            { (Int32)Key.I,                 	0x49 },
            { (Int32)Key.J,                 	0x4A },
            { (Int32)Key.K,                 	0x4B },
            { (Int32)Key.L,                 	0x4C },
            { (Int32)Key.M,                 	0x4D },
            { (Int32)Key.N,                 	0x4E },
            { (Int32)Key.O,                 	0x4F },
            { (Int32)Key.P,                 	0x50 },
            { (Int32)Key.Q,                 	0x51 },
            { (Int32)Key.R,                 	0x52 },
            { (Int32)Key.S,                 	0x53 },
            { (Int32)Key.T,                 	0x54 },
            { (Int32)Key.U,                 	0x55 },
            { (Int32)Key.V,                 	0x56 },
            { (Int32)Key.W,                 	0x57 },
            { (Int32)Key.X,                 	0x58 },
            { (Int32)Key.Y,                 	0x59 },
            { (Int32)Key.Z,                 	0x5A },
            { (Int32)Key.LeftGui,           	0x5B },
            { (Int32)Key.RightGui,          	0x5C },
            { (Int32)Key.Application,       	0x5D },
            { (Int32)Key.Sleep,             	0x5F },
            { (Int32)Key.KeypadD0,          	0x60 },
            { (Int32)Key.KeypadD1,          	0x61 },
            { (Int32)Key.KeypadD2,          	0x62 },
            { (Int32)Key.KeypadD3,          	0x63 },
            { (Int32)Key.KeypadD4,          	0x64 },
            { (Int32)Key.KeypadD5,          	0x65 },
            { (Int32)Key.KeypadD6,          	0x66 },
            { (Int32)Key.KeypadD7,          	0x67 },
            { (Int32)Key.KeypadD8,          	0x68 },
            { (Int32)Key.KeypadD9,          	0x69 },
            { (Int32)Key.KeypadMultiply,    	0x6A },
            { (Int32)Key.KeypadPlus,        	0x6B },
            { (Int32)Key.Separator,         	0x6C },
            { (Int32)Key.KeypadMinus,       	0x6D },
            { (Int32)Key.KeypadDecimal,     	0x6E },
            { (Int32)Key.KeypadDivide,      	0x6F },
            { (Int32)Key.F1,                	0x70 },
            { (Int32)Key.F2,                	0x71 },
            { (Int32)Key.F3,                	0x72 },
            { (Int32)Key.F4,                	0x73 },
            { (Int32)Key.F5,                	0x74 },
            { (Int32)Key.F6,                	0x75 },
            { (Int32)Key.F7,                	0x76 },
            { (Int32)Key.F8,                	0x77 },
            { (Int32)Key.F9,                	0x78 },
            { (Int32)Key.F10,               	0x79 },
            { (Int32)Key.F11,               	0x7A },
            { (Int32)Key.F12,               	0x7B },
            { (Int32)Key.F13,               	0x7C },
            { (Int32)Key.F14,               	0x7D },
            { (Int32)Key.F15,               	0x7E },
            { (Int32)Key.F16,               	0x7F },
            { (Int32)Key.F17,               	0x80 },
            { (Int32)Key.F18,               	0x81 },
            { (Int32)Key.F19,               	0x82 },
            { (Int32)Key.F20,               	0x83 },
            { (Int32)Key.F21,               	0x84 },
            { (Int32)Key.F22,               	0x85 },
            { (Int32)Key.F23,               	0x86 },
            { (Int32)Key.F24,               	0x87 },
            { (Int32)Key.NumLockClear,      	0x90 },
            { (Int32)Key.ScrollLock,        	0x91 },
            { (Int32)Key.LeftShift,         	0xA0 },
            { (Int32)Key.RightShift,        	0xA1 },
            { (Int32)Key.LeftControl,       	0xA2 },
            { (Int32)Key.RightControl,      	0xA3 },
            { (Int32)Key.LeftAlt,           	0xA4 },
            { (Int32)Key.RightAlt,          	0xA5 },
            { (Int32)Key.AppControlBack,    	0xA6 },
            { (Int32)Key.AppControlForward, 	0xA7 },
            { (Int32)Key.AppControlRefresh, 	0xA8 },
            { (Int32)Key.AppControlStop,    	0xA9 },
            { (Int32)Key.AppControlSearch,  	0xAA },
            { (Int32)Key.AppControlBookmarks,	0xAB },
            { (Int32)Key.AppControlHome,	    0xAC },
            { (Int32)Key.Mute,	                0xAD },
            { (Int32)Key.VolumeDown,	        0xAE },
            { (Int32)Key.VolumeUp,  	        0xAF },
            { (Int32)Key.AudioNext,	            0xB0 },
            { (Int32)Key.AudioPrev,	            0xB1 },
            { (Int32)Key.AudioStop,	            0xB2 },
            { (Int32)Key.AudioPlay,	            0xB3 },
            { (Int32)Key.Mail,	                0xB4 },
            { (Int32)Key.MediaSelect,	        0xB5 },
            { (Int32)Key.Computer,	            0xB6 }, // VK_LAUNCH_APP1
            { (Int32)Key.Calculator,            0xB7 }, // VK_LAUNCH_APP2
            { (Int32)Key.Semicolon,             0xBA },
            { (Int32)Key.Plus,                  0xBB },
            { (Int32)Key.Comma,                 0xBC },
            { (Int32)Key.Minus,                 0xBD },
            { (Int32)Key.Period,                0xBE },
            { (Int32)Key.Question,              0xBF },
            { (Int32)Key.BackQuote,             0xC0 },
            { (Int32)Key.LeftBracket,           0xDB },
            { (Int32)Key.Backslash,             0xDC },
            { (Int32)Key.RightBracket,          0xDD },
            { (Int32)Key.SingleQuote,           0xDE },
            /* VK_OEM_8                             */
            /* VK_OEM_102                           */
            /* VK_PROCESSKEY                        */
            /* VK_PACKET                            */            
            /* VK_ATTN                              */            
            { (Int32)Key.CrSel,                 0xF7 },
            { (Int32)Key.ExSel,                 0xD8 },
            /* VK_EREOF                             */
            /* VK_PLAY                              */
            /* VK_ZOOM                              */
            /* VK_NONAME                            */
            /* VK_PA1                               */
            /* VK_OEM_CLEAR                         */
        };  
    }
}
