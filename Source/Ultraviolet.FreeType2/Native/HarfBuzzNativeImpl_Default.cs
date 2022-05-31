using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class HarfBuzzNativeImpl_Default : HarfBuzzNativeImpl
    {
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr hb_buffer_createDelegate();
        private readonly hb_buffer_createDelegate phb_buffer_create = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_createDelegate>("hb_buffer_create");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_buffer_create() => phb_buffer_create();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_buffer_destroyDelegate(IntPtr buffer);
        private readonly hb_buffer_destroyDelegate phb_buffer_destroy = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_destroyDelegate>("hb_buffer_destroy");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_destroy(IntPtr buffer) => phb_buffer_destroy(buffer);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_buffer_pre_allocateDelegate(IntPtr buffer, UInt32 size);
        private readonly hb_buffer_pre_allocateDelegate phb_buffer_pre_allocate = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_pre_allocateDelegate>("hb_buffer_pre_allocate");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_pre_allocate(IntPtr buffer, UInt32 size) => phb_buffer_pre_allocate(buffer, size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UInt32 hb_buffer_get_lengthDelegate(IntPtr buffer);
        private readonly hb_buffer_get_lengthDelegate phb_buffer_get_length = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_get_lengthDelegate>("hb_buffer_get_length");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 hb_buffer_get_length(IntPtr buffer) => phb_buffer_get_length(buffer);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean hb_buffer_set_lengthDelegate(IntPtr buffer, UInt32 size);
        private readonly hb_buffer_set_lengthDelegate phb_buffer_set_length = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_set_lengthDelegate>("hb_buffer_set_length");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean hb_buffer_set_length(IntPtr buffer, UInt32 size) => phb_buffer_set_length(buffer, size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_buffer_resetDelegate(IntPtr buffer);
        private readonly hb_buffer_resetDelegate phb_buffer_reset = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_resetDelegate>("hb_buffer_reset");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_reset(IntPtr buffer) => phb_buffer_reset(buffer);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_buffer_clear_contentsDelegate(IntPtr buffer);
        private readonly hb_buffer_clear_contentsDelegate phb_buffer_clear_contents = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_clear_contentsDelegate>("hb_buffer_clear_contents");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_clear_contents(IntPtr buffer) => phb_buffer_clear_contents(buffer);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_buffer_add_utf16Delegate(IntPtr buffer, IntPtr text, Int32 textLength, UInt32 item_offset, Int32 item_length);
        private readonly hb_buffer_add_utf16Delegate phb_buffer_add_utf16 = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_add_utf16Delegate>("hb_buffer_add_utf16");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_add_utf16(IntPtr buffer, IntPtr text, Int32 textLength, UInt32 item_offset, Int32 item_length) => phb_buffer_add_utf16(buffer, text, textLength, item_offset, item_length);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_buffer_guess_segment_propertiesDelegate(IntPtr buffer);
        private readonly hb_buffer_guess_segment_propertiesDelegate phb_buffer_guess_segment_properties = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_guess_segment_propertiesDelegate>("hb_buffer_guess_segment_properties");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_guess_segment_properties(IntPtr buffer) => phb_buffer_guess_segment_properties(buffer);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate hb_script_t hb_buffer_get_scriptDelegate(IntPtr buffer);
        private readonly hb_buffer_get_scriptDelegate phb_buffer_get_script = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_get_scriptDelegate>("hb_buffer_get_script");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed hb_script_t hb_buffer_get_script(IntPtr buffer) => phb_buffer_get_script(buffer);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_buffer_set_scriptDelegate(IntPtr buffer, hb_script_t script);
        private readonly hb_buffer_set_scriptDelegate phb_buffer_set_script = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_set_scriptDelegate>("hb_buffer_set_script");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_set_script(IntPtr buffer, hb_script_t script) => phb_buffer_set_script(buffer, script);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr hb_buffer_get_languageDelegate(IntPtr buffer);
        private readonly hb_buffer_get_languageDelegate phb_buffer_get_language = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_get_languageDelegate>("hb_buffer_get_language");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_buffer_get_language(IntPtr buffer) => phb_buffer_get_language(buffer);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_buffer_set_languageDelegate(IntPtr buffer, IntPtr language);
        private readonly hb_buffer_set_languageDelegate phb_buffer_set_language = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_set_languageDelegate>("hb_buffer_set_language");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_set_language(IntPtr buffer, IntPtr language) => phb_buffer_set_language(buffer, language);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate hb_direction_t hb_buffer_get_directionDelegate(IntPtr buffer);
        private readonly hb_buffer_get_directionDelegate phb_buffer_get_direction = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_get_directionDelegate>("hb_buffer_get_direction");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed hb_direction_t hb_buffer_get_direction(IntPtr buffer) => phb_buffer_get_direction(buffer);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_buffer_set_directionDelegate(IntPtr buffer, hb_direction_t direction);
        private readonly hb_buffer_set_directionDelegate phb_buffer_set_direction = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_set_directionDelegate>("hb_buffer_set_direction");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_buffer_set_direction(IntPtr buffer, hb_direction_t direction) => phb_buffer_set_direction(buffer, direction);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr hb_language_to_stringDelegate(IntPtr language);
        private readonly hb_language_to_stringDelegate phb_language_to_string = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_language_to_stringDelegate>("hb_language_to_string");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_language_to_string(IntPtr language) => phb_language_to_string(language);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr hb_language_from_stringDelegate(IntPtr str, Int32 len);
        private readonly hb_language_from_stringDelegate phb_language_from_string = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_language_from_stringDelegate>("hb_language_from_string");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_language_from_string(IntPtr str, Int32 len) => phb_language_from_string(str, len);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr hb_buffer_get_glyph_infosDelegate(IntPtr buffer, IntPtr length);
        private readonly hb_buffer_get_glyph_infosDelegate phb_buffer_get_glyph_infos = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_get_glyph_infosDelegate>("hb_buffer_get_glyph_infos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_buffer_get_glyph_infos(IntPtr buffer, IntPtr length) => phb_buffer_get_glyph_infos(buffer, length);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr hb_buffer_get_glyph_positionsDelegate(IntPtr buffer, IntPtr length);
        private readonly hb_buffer_get_glyph_positionsDelegate phb_buffer_get_glyph_positions = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_get_glyph_positionsDelegate>("hb_buffer_get_glyph_positions");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_buffer_get_glyph_positions(IntPtr buffer, IntPtr length) => phb_buffer_get_glyph_positions(buffer, length);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate hb_buffer_content_type_t hb_buffer_get_content_typeDelegate(IntPtr buffer);
        private readonly hb_buffer_get_content_typeDelegate phb_buffer_get_content_type = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_buffer_get_content_typeDelegate>("hb_buffer_get_content_type");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed hb_buffer_content_type_t hb_buffer_get_content_type(IntPtr buffer) => phb_buffer_get_content_type(buffer);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_shapeDelegate(IntPtr font, IntPtr buffer, IntPtr features, UInt32 num_features);
        private readonly hb_shapeDelegate phb_shape = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_shapeDelegate>("hb_shape");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_shape(IntPtr font, IntPtr buffer, IntPtr features, UInt32 num_features) => phb_shape(font, buffer, features, num_features);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr hb_ft_font_createDelegate(IntPtr ft_face, IntPtr destroy);
        private readonly hb_ft_font_createDelegate phb_ft_font_create = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_ft_font_createDelegate>("hb_ft_font_create");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr hb_ft_font_create(IntPtr ft_face, IntPtr destroy) => phb_ft_font_create(ft_face, destroy);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_font_destroyDelegate(IntPtr font);
        private readonly hb_font_destroyDelegate phb_font_destroy = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_font_destroyDelegate>("hb_font_destroy");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_font_destroy(IntPtr font) => phb_font_destroy(font);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void hb_ft_font_set_load_flagsDelegate(IntPtr font, Int32 load_flags);
        private readonly hb_ft_font_set_load_flagsDelegate phb_ft_font_set_load_flags = SharedNativeLibraries.libharfbuzz.LoadFunction<hb_ft_font_set_load_flagsDelegate>("hb_ft_font_set_load_flags");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void hb_ft_font_set_load_flags(IntPtr font, Int32 load_flags) => phb_ft_font_set_load_flags(font, load_flags);
    }
#pragma warning restore 1591
}
