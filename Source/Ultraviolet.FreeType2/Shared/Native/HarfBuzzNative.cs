using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    public static unsafe partial class HarfBuzzNative
    {
        private static readonly HarfBuzzNativeImpl impl;
        
        static HarfBuzzNative()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Android:
                    impl = new HarfBuzzNativeImpl_Android();
                    break;
                    
                default:
                    impl = new HarfBuzzNativeImpl_Default();
                    break;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr hb_buffer_create() => impl.hb_buffer_create();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_buffer_destroy(IntPtr buffer) => impl.hb_buffer_destroy(buffer);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_buffer_pre_allocate(IntPtr buffer, UInt32 size) => impl.hb_buffer_pre_allocate(buffer, size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 hb_buffer_get_length(IntPtr buffer) => impl.hb_buffer_get_length(buffer);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean hb_buffer_set_length(IntPtr buffer, UInt32 size) => impl.hb_buffer_set_length(buffer, size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_buffer_reset(IntPtr buffer) => impl.hb_buffer_reset(buffer);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_buffer_clear_contents(IntPtr buffer) => impl.hb_buffer_clear_contents(buffer);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_buffer_add_utf16(IntPtr buffer, IntPtr text, Int32 textLength, UInt32 item_offset, Int32 item_length) => impl.hb_buffer_add_utf16(buffer, text, textLength, item_offset, item_length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_buffer_guess_segment_properties(IntPtr buffer) => impl.hb_buffer_guess_segment_properties(buffer);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static hb_script_t hb_buffer_get_script(IntPtr buffer) => impl.hb_buffer_get_script(buffer);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_buffer_set_script(IntPtr buffer, hb_script_t script) => impl.hb_buffer_set_script(buffer, script);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr hb_buffer_get_language(IntPtr buffer) => impl.hb_buffer_get_language(buffer);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_buffer_set_language(IntPtr buffer, IntPtr language) => impl.hb_buffer_set_language(buffer, language);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static hb_direction_t hb_buffer_get_direction(IntPtr buffer) => impl.hb_buffer_get_direction(buffer);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_buffer_set_direction(IntPtr buffer, hb_direction_t direction) => impl.hb_buffer_set_direction(buffer, direction);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr hb_language_to_string(IntPtr language) => impl.hb_language_to_string(language);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr hb_language_from_string(IntPtr str, Int32 len) => impl.hb_language_from_string(str, len);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr hb_buffer_get_glyph_infos(IntPtr buffer, IntPtr length) => impl.hb_buffer_get_glyph_infos(buffer, length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr hb_buffer_get_glyph_positions(IntPtr buffer, IntPtr length) => impl.hb_buffer_get_glyph_positions(buffer, length);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static hb_buffer_content_type_t hb_buffer_get_content_type(IntPtr buffer) => impl.hb_buffer_get_content_type(buffer);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_shape(IntPtr font, IntPtr buffer, IntPtr features, UInt32 num_features) => impl.hb_shape(font, buffer, features, num_features);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr hb_ft_font_create(IntPtr ft_face, IntPtr destroy) => impl.hb_ft_font_create(ft_face, destroy);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_font_destroy(IntPtr font) => impl.hb_font_destroy(font);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void hb_ft_font_set_load_flags(IntPtr font, Int32 load_flags) => impl.hb_ft_font_set_load_flags(font, load_flags);
    }
#pragma warning restore 1591
}
