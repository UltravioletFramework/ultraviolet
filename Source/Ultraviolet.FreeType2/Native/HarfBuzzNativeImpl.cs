using System;
using System.Security;
using System.Runtime.InteropServices;

namespace Ultraviolet.FreeType2.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    public abstract unsafe class HarfBuzzNativeImpl
    {
        public abstract IntPtr hb_buffer_create();
        public abstract void hb_buffer_destroy(IntPtr buffer);
        public abstract void hb_buffer_pre_allocate(IntPtr buffer, UInt32 size);
        public abstract UInt32 hb_buffer_get_length(IntPtr buffer);
        public abstract Boolean hb_buffer_set_length(IntPtr buffer, UInt32 size);
        public abstract void hb_buffer_reset(IntPtr buffer);
        public abstract void hb_buffer_clear_contents(IntPtr buffer);
        public abstract void hb_buffer_add_utf16(IntPtr buffer, IntPtr text, Int32 textLength, UInt32 item_offset, Int32 item_length);
        public abstract void hb_buffer_guess_segment_properties(IntPtr buffer);
        public abstract hb_script_t hb_buffer_get_script(IntPtr buffer);
        public abstract void hb_buffer_set_script(IntPtr buffer, hb_script_t script);
        public abstract IntPtr hb_buffer_get_language(IntPtr buffer);
        public abstract void hb_buffer_set_language(IntPtr buffer, IntPtr language);
        public abstract hb_direction_t hb_buffer_get_direction(IntPtr buffer);
        public abstract void hb_buffer_set_direction(IntPtr buffer, hb_direction_t direction);
        public abstract IntPtr hb_language_to_string(IntPtr language);
        public abstract IntPtr hb_language_from_string(IntPtr str, Int32 len);
        public abstract IntPtr hb_buffer_get_glyph_infos(IntPtr buffer, IntPtr length);
        public abstract IntPtr hb_buffer_get_glyph_positions(IntPtr buffer, IntPtr length);
        public abstract hb_buffer_content_type_t hb_buffer_get_content_type(IntPtr buffer);
        public abstract void hb_shape(IntPtr font, IntPtr buffer, IntPtr features, UInt32 num_features);
        public abstract IntPtr hb_ft_font_create(IntPtr ft_face, IntPtr destroy);
        public abstract void hb_font_destroy(IntPtr font);
        public abstract void hb_ft_font_set_load_flags(IntPtr font, Int32 load_flags);
    }
#pragma warning restore 1591
}
