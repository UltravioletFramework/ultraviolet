using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class HarfBuzzNativeImpl_Android : HarfBuzzNativeImpl
    {
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_create", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_hb_buffer_create();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_buffer_create() => INTERNAL_hb_buffer_create();
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_destroy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_buffer_destroy(IntPtr buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_destroy(IntPtr buffer) => INTERNAL_hb_buffer_destroy(buffer);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_pre_allocate", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_buffer_pre_allocate(IntPtr buffer, UInt32 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_pre_allocate(IntPtr buffer, UInt32 size) => INTERNAL_hb_buffer_pre_allocate(buffer, size);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_get_length", CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 INTERNAL_hb_buffer_get_length(IntPtr buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 hb_buffer_get_length(IntPtr buffer) => INTERNAL_hb_buffer_get_length(buffer);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_set_length", CallingConvention = CallingConvention.Cdecl)]
        private static extern Boolean INTERNAL_hb_buffer_set_length(IntPtr buffer, UInt32 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean hb_buffer_set_length(IntPtr buffer, UInt32 size) => INTERNAL_hb_buffer_set_length(buffer, size);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_reset", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_buffer_reset(IntPtr buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_reset(IntPtr buffer) => INTERNAL_hb_buffer_reset(buffer);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_clear_contents", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_buffer_clear_contents(IntPtr buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_clear_contents(IntPtr buffer) => INTERNAL_hb_buffer_clear_contents(buffer);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_add_utf16", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_buffer_add_utf16(IntPtr buffer, IntPtr text, Int32 textLength, UInt32 item_offset, Int32 item_length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_add_utf16(IntPtr buffer, IntPtr text, Int32 textLength, UInt32 item_offset, Int32 item_length) => INTERNAL_hb_buffer_add_utf16(buffer, text, textLength, item_offset, item_length);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_guess_segment_properties", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_buffer_guess_segment_properties(IntPtr buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_guess_segment_properties(IntPtr buffer) => INTERNAL_hb_buffer_guess_segment_properties(buffer);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_get_script", CallingConvention = CallingConvention.Cdecl)]
        private static extern hb_script_t INTERNAL_hb_buffer_get_script(IntPtr buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed hb_script_t hb_buffer_get_script(IntPtr buffer) => INTERNAL_hb_buffer_get_script(buffer);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_set_script", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_buffer_set_script(IntPtr buffer, hb_script_t script);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_set_script(IntPtr buffer, hb_script_t script) => INTERNAL_hb_buffer_set_script(buffer, script);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_get_language", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_hb_buffer_get_language(IntPtr buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_buffer_get_language(IntPtr buffer) => INTERNAL_hb_buffer_get_language(buffer);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_set_language", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_buffer_set_language(IntPtr buffer, IntPtr language);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_set_language(IntPtr buffer, IntPtr language) => INTERNAL_hb_buffer_set_language(buffer, language);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_get_direction", CallingConvention = CallingConvention.Cdecl)]
        private static extern hb_direction_t INTERNAL_hb_buffer_get_direction(IntPtr buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed hb_direction_t hb_buffer_get_direction(IntPtr buffer) => INTERNAL_hb_buffer_get_direction(buffer);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_set_direction", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_buffer_set_direction(IntPtr buffer, hb_direction_t direction);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_set_direction(IntPtr buffer, hb_direction_t direction) => INTERNAL_hb_buffer_set_direction(buffer, direction);
        
        [DllImport("harfbuzz", EntryPoint = "hb_language_to_string", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_hb_language_to_string(IntPtr language);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_language_to_string(IntPtr language) => INTERNAL_hb_language_to_string(language);
        
        [DllImport("harfbuzz", EntryPoint = "hb_language_from_string", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_hb_language_from_string(IntPtr str, Int32 len);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_language_from_string(IntPtr str, Int32 len) => INTERNAL_hb_language_from_string(str, len);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_get_glyph_infos", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_hb_buffer_get_glyph_infos(IntPtr buffer, IntPtr length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_buffer_get_glyph_infos(IntPtr buffer, IntPtr length) => INTERNAL_hb_buffer_get_glyph_infos(buffer, length);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_get_glyph_positions", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_hb_buffer_get_glyph_positions(IntPtr buffer, IntPtr length);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_buffer_get_glyph_positions(IntPtr buffer, IntPtr length) => INTERNAL_hb_buffer_get_glyph_positions(buffer, length);
        
        [DllImport("harfbuzz", EntryPoint = "hb_buffer_get_content_type", CallingConvention = CallingConvention.Cdecl)]
        private static extern hb_buffer_content_type_t INTERNAL_hb_buffer_get_content_type(IntPtr buffer);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed hb_buffer_content_type_t hb_buffer_get_content_type(IntPtr buffer) => INTERNAL_hb_buffer_get_content_type(buffer);
        
        [DllImport("harfbuzz", EntryPoint = "hb_shape", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_shape(IntPtr font, IntPtr buffer, IntPtr features, UInt32 num_features);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_shape(IntPtr font, IntPtr buffer, IntPtr features, UInt32 num_features) => INTERNAL_hb_shape(font, buffer, features, num_features);
        
        [DllImport("harfbuzz", EntryPoint = "hb_ft_font_create", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_hb_ft_font_create(IntPtr ft_face, IntPtr destroy);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_ft_font_create(IntPtr ft_face, IntPtr destroy) => INTERNAL_hb_ft_font_create(ft_face, destroy);
        
        [DllImport("harfbuzz", EntryPoint = "hb_font_destroy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_font_destroy(IntPtr font);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_font_destroy(IntPtr font) => INTERNAL_hb_font_destroy(font);
        
        [DllImport("harfbuzz", EntryPoint = "hb_ft_font_set_load_flags", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_hb_ft_font_set_load_flags(IntPtr font, Int32 load_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_ft_font_set_load_flags(IntPtr font, Int32 load_flags) => INTERNAL_hb_ft_font_set_load_flags(font, load_flags);
    }
#pragma warning restore 1591
}
