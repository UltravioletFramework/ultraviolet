using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ultraviolet.ImGuiViewProvider.Bindings
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class ImGuiNativeImpl_Android : ImGuiNativeImpl
    {
        [DllImport("cimgui", EntryPoint = "igGetFrameHeight", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetFrameHeight();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetFrameHeight() => INTERNAL_igGetFrameHeight();
        
        [DllImport("cimgui", EntryPoint = "igCreateContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_igCreateContext(ImFontAtlas* shared_font_atlas);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr igCreateContext(ImFontAtlas* shared_font_atlas) => INTERNAL_igCreateContext(shared_font_atlas);
        
        [DllImport("cimgui", EntryPoint = "igTextUnformatted", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igTextUnformatted(byte* text, byte* text_end);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTextUnformatted(byte* text, byte* text_end) => INTERNAL_igTextUnformatted(text, text_end);
        
        [DllImport("cimgui", EntryPoint = "igPopFont", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPopFont();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopFont() => INTERNAL_igPopFont();
        
        [DllImport("cimgui", EntryPoint = "igCombo", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igCombo(byte* label, int* current_item, byte** items, int items_count, int popup_max_height_in_items);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igCombo(byte* label, int* current_item, byte** items, int items_count, int popup_max_height_in_items) => INTERNAL_igCombo(label, current_item, items, items_count, popup_max_height_in_items);
        
        [DllImport("cimgui", EntryPoint = "igComboStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igComboStr(byte* label, int* current_item, byte* items_separated_by_zeros, int popup_max_height_in_items);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igComboStr(byte* label, int* current_item, byte* items_separated_by_zeros, int popup_max_height_in_items) => INTERNAL_igComboStr(label, current_item, items_separated_by_zeros, popup_max_height_in_items);
        
        [DllImport("cimgui", EntryPoint = "igCaptureKeyboardFromApp", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igCaptureKeyboardFromApp(byte capture);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igCaptureKeyboardFromApp(byte capture) => INTERNAL_igCaptureKeyboardFromApp(capture);
        
        [DllImport("cimgui", EntryPoint = "igIsWindowFocused", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsWindowFocused(ImGuiFocusedFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsWindowFocused(ImGuiFocusedFlags flags) => INTERNAL_igIsWindowFocused(flags);
        
        [DllImport("cimgui", EntryPoint = "igRender", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igRender();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igRender() => INTERNAL_igRender();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_ChannelsSetCurrent", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_ChannelsSetCurrent(ImDrawList* self, int channel_index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_ChannelsSetCurrent(ImDrawList* self, int channel_index) => INTERNAL_ImDrawList_ChannelsSetCurrent(self, channel_index);
        
        [DllImport("cimgui", EntryPoint = "igDragFloat4", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragFloat4(byte* label, Vector4* v, float v_speed, float v_min, float v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragFloat4(byte* label, Vector4* v, float v_speed, float v_min, float v_max, byte* format, float power) => INTERNAL_igDragFloat4(label, v, v_speed, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_ChannelsSplit", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_ChannelsSplit(ImDrawList* self, int channels_count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_ChannelsSplit(ImDrawList* self, int channels_count) => INTERNAL_ImDrawList_ChannelsSplit(self, channels_count);
        
        [DllImport("cimgui", EntryPoint = "igIsMousePosValid", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsMousePosValid(Vector2* mouse_pos);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMousePosValid(Vector2* mouse_pos) => INTERNAL_igIsMousePosValid(mouse_pos);
        
        [DllImport("cimgui", EntryPoint = "igGetCursorScreenPos", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetCursorScreenPos();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetCursorScreenPos() => INTERNAL_igGetCursorScreenPos();
        
        [DllImport("cimgui", EntryPoint = "igDebugCheckVersionAndDataLayout", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDebugCheckVersionAndDataLayout(byte* version_str, uint sz_io, uint sz_style, uint sz_vec2, uint sz_vec4, uint sz_drawvert);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDebugCheckVersionAndDataLayout(byte* version_str, uint sz_io, uint sz_style, uint sz_vec2, uint sz_vec4, uint sz_drawvert) => INTERNAL_igDebugCheckVersionAndDataLayout(version_str, sz_io, sz_style, sz_vec2, sz_vec4, sz_drawvert);
        
        [DllImport("cimgui", EntryPoint = "igSetScrollHere", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetScrollHere(float center_y_ratio);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetScrollHere(float center_y_ratio) => INTERNAL_igSetScrollHere(center_y_ratio);
        
        [DllImport("cimgui", EntryPoint = "igSetScrollY", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetScrollY(float scroll_y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetScrollY(float scroll_y) => INTERNAL_igSetScrollY(scroll_y);
        
        [DllImport("cimgui", EntryPoint = "igSetColorEditOptions", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetColorEditOptions(ImGuiColorEditFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetColorEditOptions(ImGuiColorEditFlags flags) => INTERNAL_igSetColorEditOptions(flags);
        
        [DllImport("cimgui", EntryPoint = "igSetScrollFromPosY", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetScrollFromPosY(float pos_y, float center_y_ratio);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetScrollFromPosY(float pos_y, float center_y_ratio) => INTERNAL_igSetScrollFromPosY(pos_y, center_y_ratio);
        
        [DllImport("cimgui", EntryPoint = "igGetStyleColorVec4", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector4* INTERNAL_igGetStyleColorVec4(ImGuiCol idx);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector4* igGetStyleColorVec4(ImGuiCol idx) => INTERNAL_igGetStyleColorVec4(idx);
        
        [DllImport("cimgui", EntryPoint = "igIsMouseHoveringRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsMouseHoveringRect(Vector2 r_min, Vector2 r_max, byte clip);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseHoveringRect(Vector2 r_min, Vector2 r_max, byte clip) => INTERNAL_igIsMouseHoveringRect(r_min, r_max, clip);
        
        [DllImport("cimgui", EntryPoint = "ImVec4_ImVec4", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImVec4_ImVec4(Vector4* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImVec4_ImVec4(Vector4* self) => INTERNAL_ImVec4_ImVec4(self);
        
        [DllImport("cimgui", EntryPoint = "ImVec4_ImVec4Float", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImVec4_ImVec4Float(Vector4* self, float _x, float _y, float _z, float _w);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImVec4_ImVec4Float(Vector4* self, float _x, float _y, float _z, float _w) => INTERNAL_ImVec4_ImVec4Float(self, _x, _y, _z, _w);
        
        [DllImport("cimgui", EntryPoint = "ImColor_SetHSV", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImColor_SetHSV(ImColor* self, float h, float s, float v, float a);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_SetHSV(ImColor* self, float h, float s, float v, float a) => INTERNAL_ImColor_SetHSV(self, h, s, v, a);
        
        [DllImport("cimgui", EntryPoint = "igDragFloat3", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragFloat3(byte* label, Vector3* v, float v_speed, float v_min, float v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragFloat3(byte* label, Vector3* v, float v_speed, float v_min, float v_max, byte* format, float power) => INTERNAL_igDragFloat3(label, v, v_speed, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddPolyline", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddPolyline(ImDrawList* self, Vector2* points, int num_points, uint col, byte closed, float thickness);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddPolyline(ImDrawList* self, Vector2* points, int num_points, uint col, byte closed, float thickness) => INTERNAL_ImDrawList_AddPolyline(self, points, num_points, col, closed, thickness);
        
        [DllImport("cimgui", EntryPoint = "igValueBool", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igValueBool(byte* prefix, byte b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igValueBool(byte* prefix, byte b) => INTERNAL_igValueBool(prefix, b);
        
        [DllImport("cimgui", EntryPoint = "igValueInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igValueInt(byte* prefix, int v);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igValueInt(byte* prefix, int v) => INTERNAL_igValueInt(prefix, v);
        
        [DllImport("cimgui", EntryPoint = "igValueUint", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igValueUint(byte* prefix, uint v);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igValueUint(byte* prefix, uint v) => INTERNAL_igValueUint(prefix, v);
        
        [DllImport("cimgui", EntryPoint = "igValueFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igValueFloat(byte* prefix, float v, byte* float_format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igValueFloat(byte* prefix, float v, byte* float_format) => INTERNAL_igValueFloat(prefix, v, float_format);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextFilter_Build", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiTextFilter_Build(ImGuiTextFilter* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextFilter_Build(ImGuiTextFilter* self) => INTERNAL_ImGuiTextFilter_Build(self);
        
        [DllImport("cimgui", EntryPoint = "igGetItemRectMax", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetItemRectMax();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetItemRectMax() => INTERNAL_igGetItemRectMax();
        
        [DllImport("cimgui", EntryPoint = "igIsItemDeactivated", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsItemDeactivated();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemDeactivated() => INTERNAL_igIsItemDeactivated();
        
        [DllImport("cimgui", EntryPoint = "igPushStyleVarFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushStyleVarFloat(ImGuiStyleVar idx, float val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushStyleVarFloat(ImGuiStyleVar idx, float val) => INTERNAL_igPushStyleVarFloat(idx, val);
        
        [DllImport("cimgui", EntryPoint = "igPushStyleVarVec2", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushStyleVarVec2(ImGuiStyleVar idx, Vector2 val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushStyleVarVec2(ImGuiStyleVar idx, Vector2 val) => INTERNAL_igPushStyleVarVec2(idx, val);
        
        [DllImport("cimgui", EntryPoint = "igSaveIniSettingsToMemory", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_igSaveIniSettingsToMemory(uint* out_ini_size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* igSaveIniSettingsToMemory(uint* out_ini_size) => INTERNAL_igSaveIniSettingsToMemory(out_ini_size);
        
        [DllImport("cimgui", EntryPoint = "igDragIntRange2", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragIntRange2(byte* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, byte* format, byte* format_max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragIntRange2(byte* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, byte* format, byte* format_max) => INTERNAL_igDragIntRange2(label, v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max);
        
        [DllImport("cimgui", EntryPoint = "igUnindent", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igUnindent(float indent_w);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igUnindent(float indent_w) => INTERNAL_igUnindent(indent_w);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_AddFontFromMemoryCompressedBase85TTF", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImFont* INTERNAL_ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(ImFontAtlas* self, byte* compressed_font_data_base85, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(ImFontAtlas* self, byte* compressed_font_data_base85, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => INTERNAL_ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(self, compressed_font_data_base85, size_pixels, font_cfg, glyph_ranges);
        
        [DllImport("cimgui", EntryPoint = "igPopAllowKeyboardFocus", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPopAllowKeyboardFocus();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopAllowKeyboardFocus() => INTERNAL_igPopAllowKeyboardFocus();
        
        [DllImport("cimgui", EntryPoint = "igLoadIniSettingsFromDisk", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igLoadIniSettingsFromDisk(byte* ini_filename);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLoadIniSettingsFromDisk(byte* ini_filename) => INTERNAL_igLoadIniSettingsFromDisk(ini_filename);
        
        [DllImport("cimgui", EntryPoint = "igGetCursorStartPos", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetCursorStartPos();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetCursorStartPos() => INTERNAL_igGetCursorStartPos();
        
        [DllImport("cimgui", EntryPoint = "igSetCursorScreenPos", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetCursorScreenPos(Vector2 screen_pos);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetCursorScreenPos(Vector2 screen_pos) => INTERNAL_igSetCursorScreenPos(screen_pos);
        
        [DllImport("cimgui", EntryPoint = "igInputInt4", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputInt4(byte* label, int* v, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputInt4(byte* label, int* v, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputInt4(label, v, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "ImFont_AddRemapChar", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFont_AddRemapChar(ImFont* self, ushort dst, ushort src, byte overwrite_dst);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_AddRemapChar(ImFont* self, ushort dst, ushort src, byte overwrite_dst) => INTERNAL_ImFont_AddRemapChar(self, dst, src, overwrite_dst);
        
        [DllImport("cimgui", EntryPoint = "ImFont_AddGlyph", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFont_AddGlyph(ImFont* self, ushort c, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1, float advance_x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_AddGlyph(ImFont* self, ushort c, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1, float advance_x) => INTERNAL_ImFont_AddGlyph(self, c, x0, y0, x1, y1, u0, v0, u1, v1, advance_x);
        
        [DllImport("cimgui", EntryPoint = "igIsRectVisible", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsRectVisible(Vector2 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsRectVisible(Vector2 size) => INTERNAL_igIsRectVisible(size);
        
        [DllImport("cimgui", EntryPoint = "igIsRectVisibleVec2", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsRectVisibleVec2(Vector2 rect_min, Vector2 rect_max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsRectVisibleVec2(Vector2 rect_min, Vector2 rect_max) => INTERNAL_igIsRectVisibleVec2(rect_min, rect_max);
        
        [DllImport("cimgui", EntryPoint = "ImFont_GrowIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFont_GrowIndex(ImFont* self, int new_size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_GrowIndex(ImFont* self, int new_size) => INTERNAL_ImFont_GrowIndex(self, new_size);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_Build", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImFontAtlas_Build(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImFontAtlas_Build(ImFontAtlas* self) => INTERNAL_ImFontAtlas_Build(self);
        
        [DllImport("cimgui", EntryPoint = "igLabelText", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igLabelText(byte* label, byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLabelText(byte* label, byte* fmt) => INTERNAL_igLabelText(label, fmt);
        
        [DllImport("cimgui", EntryPoint = "ImFont_RenderText", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFont_RenderText(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, byte* text_begin, byte* text_end, float wrap_width, byte cpu_fine_clip);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_RenderText(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, byte* text_begin, byte* text_end, float wrap_width, byte cpu_fine_clip) => INTERNAL_ImFont_RenderText(self, draw_list, size, pos, col, clip_rect, text_begin, text_end, wrap_width, cpu_fine_clip);
        
        [DllImport("cimgui", EntryPoint = "igLogFinish", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igLogFinish();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogFinish() => INTERNAL_igLogFinish();
        
        [DllImport("cimgui", EntryPoint = "igIsKeyPressed", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsKeyPressed(int user_key_index, byte repeat);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsKeyPressed(int user_key_index, byte repeat) => INTERNAL_igIsKeyPressed(user_key_index, repeat);
        
        [DllImport("cimgui", EntryPoint = "igGetColumnOffset", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetColumnOffset(int column_index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetColumnOffset(int column_index) => INTERNAL_igGetColumnOffset(column_index);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PopClipRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PopClipRect(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PopClipRect(ImDrawList* self) => INTERNAL_ImDrawList_PopClipRect(self);
        
        [DllImport("cimgui", EntryPoint = "ImFont_FindGlyphNoFallback", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImFontGlyph* INTERNAL_ImFont_FindGlyphNoFallback(ImFont* self, ushort c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFontGlyph* ImFont_FindGlyphNoFallback(ImFont* self, ushort c) => INTERNAL_ImFont_FindGlyphNoFallback(self, c);
        
        [DllImport("cimgui", EntryPoint = "igSetNextWindowCollapsed", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetNextWindowCollapsed(byte collapsed, ImGuiCond cond);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowCollapsed(byte collapsed, ImGuiCond cond) => INTERNAL_igSetNextWindowCollapsed(collapsed, cond);
        
        [DllImport("cimgui", EntryPoint = "igGetCurrentContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_igGetCurrentContext();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr igGetCurrentContext() => INTERNAL_igGetCurrentContext();
        
        [DllImport("cimgui", EntryPoint = "igSmallButton", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSmallButton(byte* label);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSmallButton(byte* label) => INTERNAL_igSmallButton(label);
        
        [DllImport("cimgui", EntryPoint = "igOpenPopupOnItemClick", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igOpenPopupOnItemClick(byte* str_id, int mouse_button);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igOpenPopupOnItemClick(byte* str_id, int mouse_button) => INTERNAL_igOpenPopupOnItemClick(str_id, mouse_button);
        
        [DllImport("cimgui", EntryPoint = "igIsAnyMouseDown", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsAnyMouseDown();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsAnyMouseDown() => INTERNAL_igIsAnyMouseDown();
        
        [DllImport("cimgui", EntryPoint = "ImFont_CalcWordWrapPositionA", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_ImFont_CalcWordWrapPositionA(ImFont* self, float scale, byte* text, byte* text_end, float wrap_width);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImFont_CalcWordWrapPositionA(ImFont* self, float scale, byte* text, byte* text_end, float wrap_width) => INTERNAL_ImFont_CalcWordWrapPositionA(self, scale, text, text_end, wrap_width);
        
        [DllImport("cimgui", EntryPoint = "ImFont_CalcTextSizeA", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_ImFont_CalcTextSizeA(ImFont* self, float size, float max_width, float wrap_width, byte* text_begin, byte* text_end, byte** remaining);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 ImFont_CalcTextSizeA(ImFont* self, float size, float max_width, float wrap_width, byte* text_begin, byte* text_end, byte** remaining) => INTERNAL_ImFont_CalcTextSizeA(self, size, max_width, wrap_width, text_begin, text_end, remaining);
        
        [DllImport("cimgui", EntryPoint = "GlyphRangesBuilder_SetBit", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_GlyphRangesBuilder_SetBit(GlyphRangesBuilder* self, int n);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_SetBit(GlyphRangesBuilder* self, int n) => INTERNAL_GlyphRangesBuilder_SetBit(self, n);
        
        [DllImport("cimgui", EntryPoint = "ImFont_IsLoaded", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImFont_IsLoaded(ImFont* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImFont_IsLoaded(ImFont* self) => INTERNAL_ImFont_IsLoaded(self);
        
        [DllImport("cimgui", EntryPoint = "ImFont_GetCharAdvance", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_ImFont_GetCharAdvance(ImFont* self, ushort c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float ImFont_GetCharAdvance(ImFont* self, ushort c) => INTERNAL_ImFont_GetCharAdvance(self, c);
        
        [DllImport("cimgui", EntryPoint = "igImageButton", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igImageButton(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, int frame_padding, Vector4 bg_col, Vector4 tint_col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igImageButton(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, int frame_padding, Vector4 bg_col, Vector4 tint_col) => INTERNAL_igImageButton(user_texture_id, size, uv0, uv1, frame_padding, bg_col, tint_col);
        
        [DllImport("cimgui", EntryPoint = "ImFont_SetFallbackChar", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFont_SetFallbackChar(ImFont* self, ushort c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_SetFallbackChar(ImFont* self, ushort c) => INTERNAL_ImFont_SetFallbackChar(self, c);
        
        [DllImport("cimgui", EntryPoint = "igEndFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndFrame();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndFrame() => INTERNAL_igEndFrame();
        
        [DllImport("cimgui", EntryPoint = "igSliderFloat2", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderFloat2(byte* label, Vector2* v, float v_min, float v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderFloat2(byte* label, Vector2* v, float v_min, float v_max, byte* format, float power) => INTERNAL_igSliderFloat2(label, v, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "ImFont_RenderChar", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFont_RenderChar(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, ushort c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_RenderChar(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, ushort c) => INTERNAL_ImFont_RenderChar(self, draw_list, size, pos, col, c);
        
        [DllImport("cimgui", EntryPoint = "igRadioButtonBool", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igRadioButtonBool(byte* label, byte active);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igRadioButtonBool(byte* label, byte active) => INTERNAL_igRadioButtonBool(label, active);
        
        [DllImport("cimgui", EntryPoint = "igRadioButtonIntPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igRadioButtonIntPtr(byte* label, int* v, int v_button);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igRadioButtonIntPtr(byte* label, int* v, int v_button) => INTERNAL_igRadioButtonIntPtr(label, v, v_button);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PushClipRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PushClipRect(ImDrawList* self, Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PushClipRect(ImDrawList* self, Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect) => INTERNAL_ImDrawList_PushClipRect(self, clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
        
        [DllImport("cimgui", EntryPoint = "ImFont_FindGlyph", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImFontGlyph* INTERNAL_ImFont_FindGlyph(ImFont* self, ushort c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFontGlyph* ImFont_FindGlyph(ImFont* self, ushort c) => INTERNAL_ImFont_FindGlyph(self, c);
        
        [DllImport("cimgui", EntryPoint = "igIsItemDeactivatedAfterEdit", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsItemDeactivatedAfterEdit();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemDeactivatedAfterEdit() => INTERNAL_igIsItemDeactivatedAfterEdit();
        
        [DllImport("cimgui", EntryPoint = "igGetWindowDrawList", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImDrawList* INTERNAL_igGetWindowDrawList();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImDrawList* igGetWindowDrawList() => INTERNAL_igGetWindowDrawList();
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_AddFont", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImFont* INTERNAL_ImFontAtlas_AddFont(ImFontAtlas* self, ImFontConfig* font_cfg);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFont(ImFontAtlas* self, ImFontConfig* font_cfg) => INTERNAL_ImFontAtlas_AddFont(self, font_cfg);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PathBezierCurveTo", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PathBezierCurveTo(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, int num_segments);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathBezierCurveTo(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, int num_segments) => INTERNAL_ImDrawList_PathBezierCurveTo(self, p1, p2, p3, num_segments);
        
        [DllImport("cimgui", EntryPoint = "ImGuiPayload_Clear", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiPayload_Clear(ImGuiPayload* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiPayload_Clear(ImGuiPayload* self) => INTERNAL_ImGuiPayload_Clear(self);
        
        [DllImport("cimgui", EntryPoint = "igNewLine", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igNewLine();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igNewLine() => INTERNAL_igNewLine();
        
        [DllImport("cimgui", EntryPoint = "igIsItemFocused", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsItemFocused();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemFocused() => INTERNAL_igIsItemFocused();
        
        [DllImport("cimgui", EntryPoint = "igLoadIniSettingsFromMemory", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igLoadIniSettingsFromMemory(byte* ini_data, uint ini_size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLoadIniSettingsFromMemory(byte* ini_data, uint ini_size) => INTERNAL_igLoadIniSettingsFromMemory(ini_data, ini_size);
        
        [DllImport("cimgui", EntryPoint = "igSliderInt2", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderInt2(byte* label, int* v, int v_min, int v_max, byte* format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderInt2(byte* label, int* v, int v_min, int v_max, byte* format) => INTERNAL_igSliderInt2(label, v, v_min, v_max, format);
        
        [DllImport("cimgui", EntryPoint = "igSetWindowSizeVec2", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetWindowSizeVec2(Vector2 size, ImGuiCond cond);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowSizeVec2(Vector2 size, ImGuiCond cond) => INTERNAL_igSetWindowSizeVec2(size, cond);
        
        [DllImport("cimgui", EntryPoint = "igSetWindowSizeStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetWindowSizeStr(byte* name, Vector2 size, ImGuiCond cond);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowSizeStr(byte* name, Vector2 size, ImGuiCond cond) => INTERNAL_igSetWindowSizeStr(name, size, cond);
        
        [DllImport("cimgui", EntryPoint = "igInputFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputFloat(byte* label, float* v, float step, float step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputFloat(byte* label, float* v, float step, float step_fast, byte* format, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputFloat(label, v, step, step_fast, format, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "ImFont_ImFont", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFont_ImFont(ImFont* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_ImFont(ImFont* self) => INTERNAL_ImFont_ImFont(self);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_SetFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiStorage_SetFloat(ImGuiStorage* self, uint key, float val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_SetFloat(ImGuiStorage* self, uint key, float val) => INTERNAL_ImGuiStorage_SetFloat(self, key, val);
        
        [DllImport("cimgui", EntryPoint = "igColorConvertRGBtoHSV", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igColorConvertRGBtoHSV(float r, float g, float b, float* out_h, float* out_s, float* out_v);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igColorConvertRGBtoHSV(float r, float g, float b, float* out_h, float* out_s, float* out_v) => INTERNAL_igColorConvertRGBtoHSV(r, g, b, out_h, out_s, out_v);
        
        [DllImport("cimgui", EntryPoint = "igBeginMenuBar", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginMenuBar();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginMenuBar() => INTERNAL_igBeginMenuBar();
        
        [DllImport("cimgui", EntryPoint = "igIsPopupOpen", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsPopupOpen(byte* str_id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsPopupOpen(byte* str_id) => INTERNAL_igIsPopupOpen(str_id);
        
        [DllImport("cimgui", EntryPoint = "igIsItemVisible", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsItemVisible();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemVisible() => INTERNAL_igIsItemVisible();
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_CalcCustomRectUV", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFontAtlas_CalcCustomRectUV(ImFontAtlas* self, CustomRect* rect, Vector2* out_uv_min, Vector2* out_uv_max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_CalcCustomRectUV(ImFontAtlas* self, CustomRect* rect, Vector2* out_uv_min, Vector2* out_uv_max) => INTERNAL_ImFontAtlas_CalcCustomRectUV(self, rect, out_uv_min, out_uv_max);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetCustomRectByIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern CustomRect* INTERNAL_ImFontAtlas_GetCustomRectByIndex(ImFontAtlas* self, int index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed CustomRect* ImFontAtlas_GetCustomRectByIndex(ImFontAtlas* self, int index) => INTERNAL_ImFontAtlas_GetCustomRectByIndex(self, index);
        
        [DllImport("cimgui", EntryPoint = "GlyphRangesBuilder_AddText", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_GlyphRangesBuilder_AddText(GlyphRangesBuilder* self, byte* text, byte* text_end);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_AddText(GlyphRangesBuilder* self, byte* text, byte* text_end) => INTERNAL_GlyphRangesBuilder_AddText(self, text, text_end);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_UpdateTextureID", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_UpdateTextureID(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_UpdateTextureID(ImDrawList* self) => INTERNAL_ImDrawList_UpdateTextureID(self);
        
        [DllImport("cimgui", EntryPoint = "igSetNextWindowSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetNextWindowSize(Vector2 size, ImGuiCond cond);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowSize(Vector2 size, ImGuiCond cond) => INTERNAL_igSetNextWindowSize(size, cond);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_AddCustomRectRegular", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_ImFontAtlas_AddCustomRectRegular(ImFontAtlas* self, uint id, int width, int height);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int ImFontAtlas_AddCustomRectRegular(ImFontAtlas* self, uint id, int width, int height) => INTERNAL_ImFontAtlas_AddCustomRectRegular(self, id, width, height);
        
        [DllImport("cimgui", EntryPoint = "igSetWindowCollapsedBool", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetWindowCollapsedBool(byte collapsed, ImGuiCond cond);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowCollapsedBool(byte collapsed, ImGuiCond cond) => INTERNAL_igSetWindowCollapsedBool(collapsed, cond);
        
        [DllImport("cimgui", EntryPoint = "igSetWindowCollapsedStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetWindowCollapsedStr(byte* name, byte collapsed, ImGuiCond cond);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowCollapsedStr(byte* name, byte collapsed, ImGuiCond cond) => INTERNAL_igSetWindowCollapsedStr(name, collapsed, cond);
        
        [DllImport("cimgui", EntryPoint = "igGetMouseDragDelta", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetMouseDragDelta(int button, float lock_threshold);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetMouseDragDelta(int button, float lock_threshold) => INTERNAL_igGetMouseDragDelta(button, lock_threshold);
        
        [DllImport("cimgui", EntryPoint = "igAcceptDragDropPayload", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImGuiPayload* INTERNAL_igAcceptDragDropPayload(byte* type, ImGuiDragDropFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImGuiPayload* igAcceptDragDropPayload(byte* type, ImGuiDragDropFlags flags) => INTERNAL_igAcceptDragDropPayload(type, flags);
        
        [DllImport("cimgui", EntryPoint = "igBeginDragDropSource", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginDragDropSource(ImGuiDragDropFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginDragDropSource(ImGuiDragDropFlags flags) => INTERNAL_igBeginDragDropSource(flags);
        
        [DllImport("cimgui", EntryPoint = "CustomRect_IsPacked", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_CustomRect_IsPacked(CustomRect* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte CustomRect_IsPacked(CustomRect* self) => INTERNAL_CustomRect_IsPacked(self);
        
        [DllImport("cimgui", EntryPoint = "igPlotLines", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPlotLines(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPlotLines(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride) => INTERNAL_igPlotLines(label, values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_IsBuilt", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImFontAtlas_IsBuilt(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImFontAtlas_IsBuilt(ImFontAtlas* self) => INTERNAL_ImFontAtlas_IsBuilt(self);
        
        [DllImport("cimgui", EntryPoint = "ImVec2_ImVec2", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImVec2_ImVec2(Vector2* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImVec2_ImVec2(Vector2* self) => INTERNAL_ImVec2_ImVec2(self);
        
        [DllImport("cimgui", EntryPoint = "ImVec2_ImVec2Float", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImVec2_ImVec2Float(Vector2* self, float _x, float _y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImVec2_ImVec2Float(Vector2* self, float _x, float _y) => INTERNAL_ImVec2_ImVec2Float(self, _x, _y);
        
        [DllImport("cimgui", EntryPoint = "ImGuiPayload_ImGuiPayload", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiPayload_ImGuiPayload(ImGuiPayload* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiPayload_ImGuiPayload(ImGuiPayload* self) => INTERNAL_ImGuiPayload_ImGuiPayload(self);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_Clear", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_Clear(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_Clear(ImDrawList* self) => INTERNAL_ImDrawList_Clear(self);
        
        [DllImport("cimgui", EntryPoint = "GlyphRangesBuilder_AddRanges", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_GlyphRangesBuilder_AddRanges(GlyphRangesBuilder* self, ushort* ranges);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_AddRanges(GlyphRangesBuilder* self, ushort* ranges) => INTERNAL_GlyphRangesBuilder_AddRanges(self, ranges);
        
        [DllImport("cimgui", EntryPoint = "igGetFrameCount", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_igGetFrameCount();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int igGetFrameCount() => INTERNAL_igGetFrameCount();
        
        [DllImport("cimgui", EntryPoint = "ImFont_GetDebugName", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_ImFont_GetDebugName(ImFont* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImFont_GetDebugName(ImFont* self) => INTERNAL_ImFont_GetDebugName(self);
        
        [DllImport("cimgui", EntryPoint = "igListBoxFooter", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igListBoxFooter();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igListBoxFooter() => INTERNAL_igListBoxFooter();
        
        [DllImport("cimgui", EntryPoint = "igPopClipRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPopClipRect();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopClipRect() => INTERNAL_igPopClipRect();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddBezierCurve", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddBezierCurve(ImDrawList* self, Vector2 pos0, Vector2 cp0, Vector2 cp1, Vector2 pos1, uint col, float thickness, int num_segments);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddBezierCurve(ImDrawList* self, Vector2 pos0, Vector2 cp0, Vector2 cp1, Vector2 pos1, uint col, float thickness, int num_segments) => INTERNAL_ImDrawList_AddBezierCurve(self, pos0, cp0, cp1, pos1, col, thickness, num_segments);
        
        [DllImport("cimgui", EntryPoint = "GlyphRangesBuilder_GlyphRangesBuilder", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_GlyphRangesBuilder_GlyphRangesBuilder(GlyphRangesBuilder* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_GlyphRangesBuilder(GlyphRangesBuilder* self) => INTERNAL_GlyphRangesBuilder_GlyphRangesBuilder(self);
        
        [DllImport("cimgui", EntryPoint = "igGetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetWindowSize();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetWindowSize() => INTERNAL_igGetWindowSize();
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetGlyphRangesThai", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort* INTERNAL_ImFontAtlas_GetGlyphRangesThai(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesThai(ImFontAtlas* self) => INTERNAL_ImFontAtlas_GetGlyphRangesThai(self);
        
        [DllImport("cimgui", EntryPoint = "igCheckboxFlags", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igCheckboxFlags(byte* label, uint* flags, uint flags_value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igCheckboxFlags(byte* label, uint* flags, uint flags_value) => INTERNAL_igCheckboxFlags(label, flags, flags_value);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetGlyphRangesCyrillic", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort* INTERNAL_ImFontAtlas_GetGlyphRangesCyrillic(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesCyrillic(ImFontAtlas* self) => INTERNAL_ImFontAtlas_GetGlyphRangesCyrillic(self);
        
        [DllImport("cimgui", EntryPoint = "igIsWindowHovered", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsWindowHovered(ImGuiHoveredFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsWindowHovered(ImGuiHoveredFlags flags) => INTERNAL_igIsWindowHovered(flags);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort* INTERNAL_ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(ImFontAtlas* self) => INTERNAL_ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(self);
        
        [DllImport("cimgui", EntryPoint = "igPlotHistogramFloatPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPlotHistogramFloatPtr(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPlotHistogramFloatPtr(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride) => INTERNAL_igPlotHistogramFloatPtr(label, values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
        
        [DllImport("cimgui", EntryPoint = "igBeginPopupContextVoid", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginPopupContextVoid(byte* str_id, int mouse_button);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginPopupContextVoid(byte* str_id, int mouse_button) => INTERNAL_igBeginPopupContextVoid(str_id, mouse_button);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetGlyphRangesChineseFull", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort* INTERNAL_ImFontAtlas_GetGlyphRangesChineseFull(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesChineseFull(ImFontAtlas* self) => INTERNAL_ImFontAtlas_GetGlyphRangesChineseFull(self);
        
        [DllImport("cimgui", EntryPoint = "igShowStyleEditor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igShowStyleEditor(ImGuiStyle* @ref);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igShowStyleEditor(ImGuiStyle* @ref) => INTERNAL_igShowStyleEditor(@ref);
        
        [DllImport("cimgui", EntryPoint = "igCheckbox", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igCheckbox(byte* label, byte* v);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igCheckbox(byte* label, byte* v) => INTERNAL_igCheckbox(label, v);
        
        [DllImport("cimgui", EntryPoint = "igGetWindowPos", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetWindowPos();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetWindowPos() => INTERNAL_igGetWindowPos();
        
        [DllImport("cimgui", EntryPoint = "ImGuiInputTextCallbackData_ImGuiInputTextCallbackData", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiInputTextCallbackData_ImGuiInputTextCallbackData(ImGuiInputTextCallbackData* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiInputTextCallbackData_ImGuiInputTextCallbackData(ImGuiInputTextCallbackData* self) => INTERNAL_ImGuiInputTextCallbackData_ImGuiInputTextCallbackData(self);
        
        [DllImport("cimgui", EntryPoint = "igSetNextWindowContentSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetNextWindowContentSize(Vector2 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowContentSize(Vector2 size) => INTERNAL_igSetNextWindowContentSize(size);
        
        [DllImport("cimgui", EntryPoint = "igTextColored", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igTextColored(Vector4 col, byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTextColored(Vector4 col, byte* fmt) => INTERNAL_igTextColored(col, fmt);
        
        [DllImport("cimgui", EntryPoint = "igLogToFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igLogToFile(int max_depth, byte* filename);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogToFile(int max_depth, byte* filename) => INTERNAL_igLogToFile(max_depth, filename);
        
        [DllImport("cimgui", EntryPoint = "igButton", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igButton(byte* label, Vector2 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igButton(byte* label, Vector2 size) => INTERNAL_igButton(label, size);
        
        [DllImport("cimgui", EntryPoint = "igIsItemEdited", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsItemEdited();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemEdited() => INTERNAL_igIsItemEdited();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PushTextureID", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PushTextureID(ImDrawList* self, IntPtr texture_id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PushTextureID(ImDrawList* self, IntPtr texture_id) => INTERNAL_ImDrawList_PushTextureID(self, texture_id);
        
        [DllImport("cimgui", EntryPoint = "igTreeAdvanceToLabelPos", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igTreeAdvanceToLabelPos();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTreeAdvanceToLabelPos() => INTERNAL_igTreeAdvanceToLabelPos();
        
        [DllImport("cimgui", EntryPoint = "ImGuiInputTextCallbackData_DeleteChars", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiInputTextCallbackData_DeleteChars(ImGuiInputTextCallbackData* self, int pos, int bytes_count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiInputTextCallbackData_DeleteChars(ImGuiInputTextCallbackData* self, int pos, int bytes_count) => INTERNAL_ImGuiInputTextCallbackData_DeleteChars(self, pos, bytes_count);
        
        [DllImport("cimgui", EntryPoint = "igDragInt2", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragInt2(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragInt2(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => INTERNAL_igDragInt2(label, v, v_speed, v_min, v_max, format);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetGlyphRangesDefault", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort* INTERNAL_ImFontAtlas_GetGlyphRangesDefault(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesDefault(ImFontAtlas* self) => INTERNAL_ImFontAtlas_GetGlyphRangesDefault(self);
        
        [DllImport("cimgui", EntryPoint = "igIsAnyItemActive", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsAnyItemActive();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsAnyItemActive() => INTERNAL_igIsAnyItemActive();
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_SetTexID", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFontAtlas_SetTexID(ImFontAtlas* self, IntPtr id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_SetTexID(ImFontAtlas* self, IntPtr id) => INTERNAL_ImFontAtlas_SetTexID(self, id);
        
        [DllImport("cimgui", EntryPoint = "igMenuItemBool", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igMenuItemBool(byte* label, byte* shortcut, byte selected, byte enabled);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igMenuItemBool(byte* label, byte* shortcut, byte selected, byte enabled) => INTERNAL_igMenuItemBool(label, shortcut, selected, enabled);
        
        [DllImport("cimgui", EntryPoint = "igMenuItemBoolPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igMenuItemBoolPtr(byte* label, byte* shortcut, byte* p_selected, byte enabled);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igMenuItemBoolPtr(byte* label, byte* shortcut, byte* p_selected, byte enabled) => INTERNAL_igMenuItemBoolPtr(label, shortcut, p_selected, enabled);
        
        [DllImport("cimgui", EntryPoint = "igSliderFloat4", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderFloat4(byte* label, Vector4* v, float v_min, float v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderFloat4(byte* label, Vector4* v, float v_min, float v_max, byte* format, float power) => INTERNAL_igSliderFloat4(label, v, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "igGetCursorPosX", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetCursorPosX();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetCursorPosX() => INTERNAL_igGetCursorPosX();
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_ClearTexData", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFontAtlas_ClearTexData(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_ClearTexData(ImFontAtlas* self) => INTERNAL_ImFontAtlas_ClearTexData(self);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_ClearFonts", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFontAtlas_ClearFonts(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_ClearFonts(ImFontAtlas* self) => INTERNAL_ImFontAtlas_ClearFonts(self);
        
        [DllImport("cimgui", EntryPoint = "igGetColumnsCount", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_igGetColumnsCount();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int igGetColumnsCount() => INTERNAL_igGetColumnsCount();
        
        [DllImport("cimgui", EntryPoint = "igPopButtonRepeat", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPopButtonRepeat();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopButtonRepeat() => INTERNAL_igPopButtonRepeat();
        
        [DllImport("cimgui", EntryPoint = "igDragScalarN", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragScalarN(byte* label, ImGuiDataType data_type, void* v, int components, float v_speed, void* v_min, void* v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragScalarN(byte* label, ImGuiDataType data_type, void* v, int components, float v_speed, void* v_min, void* v_max, byte* format, float power) => INTERNAL_igDragScalarN(label, data_type, v, components, v_speed, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "ImGuiPayload_IsPreview", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImGuiPayload_IsPreview(ImGuiPayload* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiPayload_IsPreview(ImGuiPayload* self) => INTERNAL_ImGuiPayload_IsPreview(self);
        
        [DllImport("cimgui", EntryPoint = "igSpacing", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSpacing();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSpacing() => INTERNAL_igSpacing();
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_Clear", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFontAtlas_Clear(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_Clear(ImFontAtlas* self) => INTERNAL_ImFontAtlas_Clear(self);
        
        [DllImport("cimgui", EntryPoint = "igIsAnyItemFocused", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsAnyItemFocused();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsAnyItemFocused() => INTERNAL_igIsAnyItemFocused();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddRectFilled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddRectFilled(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddRectFilled(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags) => INTERNAL_ImDrawList_AddRectFilled(self, a, b, col, rounding, rounding_corners_flags);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_AddFontFromMemoryCompressedTTF", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImFont* INTERNAL_ImFontAtlas_AddFontFromMemoryCompressedTTF(ImFontAtlas* self, void* compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFontFromMemoryCompressedTTF(ImFontAtlas* self, void* compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => INTERNAL_ImFontAtlas_AddFontFromMemoryCompressedTTF(self, compressed_font_data, compressed_font_size, size_pixels, font_cfg, glyph_ranges);
        
        [DllImport("cimgui", EntryPoint = "igMemFree", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igMemFree(void* ptr);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igMemFree(void* ptr) => INTERNAL_igMemFree(ptr);
        
        [DllImport("cimgui", EntryPoint = "igGetFontTexUvWhitePixel", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetFontTexUvWhitePixel();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetFontTexUvWhitePixel() => INTERNAL_igGetFontTexUvWhitePixel();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddDrawCmd", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddDrawCmd(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddDrawCmd(ImDrawList* self) => INTERNAL_ImDrawList_AddDrawCmd(self);
        
        [DllImport("cimgui", EntryPoint = "igIsItemClicked", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsItemClicked(int mouse_button);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemClicked(int mouse_button) => INTERNAL_igIsItemClicked(mouse_button);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_AddFontFromMemoryTTF", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImFont* INTERNAL_ImFontAtlas_AddFontFromMemoryTTF(ImFontAtlas* self, void* font_data, int font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFontFromMemoryTTF(ImFontAtlas* self, void* font_data, int font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => INTERNAL_ImFontAtlas_AddFontFromMemoryTTF(self, font_data, font_size, size_pixels, font_cfg, glyph_ranges);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_AddFontFromFileTTF", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImFont* INTERNAL_ImFontAtlas_AddFontFromFileTTF(ImFontAtlas* self, byte* filename, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFontFromFileTTF(ImFontAtlas* self, byte* filename, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => INTERNAL_ImFontAtlas_AddFontFromFileTTF(self, filename, size_pixels, font_cfg, glyph_ranges);
        
        [DllImport("cimgui", EntryPoint = "igProgressBar", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igProgressBar(float fraction, Vector2 size_arg, byte* overlay);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igProgressBar(float fraction, Vector2 size_arg, byte* overlay) => INTERNAL_igProgressBar(fraction, size_arg, overlay);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_AddFontDefault", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImFont* INTERNAL_ImFontAtlas_AddFontDefault(ImFontAtlas* self, ImFontConfig* font_cfg);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFontDefault(ImFontAtlas* self, ImFontConfig* font_cfg) => INTERNAL_ImFontAtlas_AddFontDefault(self, font_cfg);
        
        [DllImport("cimgui", EntryPoint = "igSetNextWindowBgAlpha", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetNextWindowBgAlpha(float alpha);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowBgAlpha(float alpha) => INTERNAL_igSetNextWindowBgAlpha(alpha);
        
        [DllImport("cimgui", EntryPoint = "igBeginPopup", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginPopup(byte* str_id, ImGuiWindowFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginPopup(byte* str_id, ImGuiWindowFlags flags) => INTERNAL_igBeginPopup(str_id, flags);
        
        [DllImport("cimgui", EntryPoint = "ImFont_BuildLookupTable", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFont_BuildLookupTable(ImFont* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_BuildLookupTable(ImFont* self) => INTERNAL_ImFont_BuildLookupTable(self);
        
        [DllImport("cimgui", EntryPoint = "igGetScrollX", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetScrollX();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetScrollX() => INTERNAL_igGetScrollX();
        
        [DllImport("cimgui", EntryPoint = "igGetKeyIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_igGetKeyIndex(ImGuiKey imgui_key);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int igGetKeyIndex(ImGuiKey imgui_key) => INTERNAL_igGetKeyIndex(imgui_key);
        
        [DllImport("cimgui", EntryPoint = "igGetOverlayDrawList", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImDrawList* INTERNAL_igGetOverlayDrawList();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImDrawList* igGetOverlayDrawList() => INTERNAL_igGetOverlayDrawList();
        
        [DllImport("cimgui", EntryPoint = "igGetIDStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint INTERNAL_igGetIDStr(byte* str_id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetIDStr(byte* str_id) => INTERNAL_igGetIDStr(str_id);
        
        [DllImport("cimgui", EntryPoint = "igGetIDRange", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint INTERNAL_igGetIDRange(byte* str_id_begin, byte* str_id_end);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetIDRange(byte* str_id_begin, byte* str_id_end) => INTERNAL_igGetIDRange(str_id_begin, str_id_end);
        
        [DllImport("cimgui", EntryPoint = "igGetIDPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint INTERNAL_igGetIDPtr(void* ptr_id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetIDPtr(void* ptr_id) => INTERNAL_igGetIDPtr(ptr_id);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetGlyphRangesJapanese", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort* INTERNAL_ImFontAtlas_GetGlyphRangesJapanese(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesJapanese(ImFontAtlas* self) => INTERNAL_ImFontAtlas_GetGlyphRangesJapanese(self);
        
        [DllImport("cimgui", EntryPoint = "igListBoxHeaderVec2", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igListBoxHeaderVec2(byte* label, Vector2 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igListBoxHeaderVec2(byte* label, Vector2 size) => INTERNAL_igListBoxHeaderVec2(label, size);
        
        [DllImport("cimgui", EntryPoint = "igListBoxHeaderInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igListBoxHeaderInt(byte* label, int items_count, int height_in_items);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igListBoxHeaderInt(byte* label, int items_count, int height_in_items) => INTERNAL_igListBoxHeaderInt(label, items_count, height_in_items);
        
        [DllImport("cimgui", EntryPoint = "ImFontConfig_ImFontConfig", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFontConfig_ImFontConfig(ImFontConfig* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontConfig_ImFontConfig(ImFontConfig* self) => INTERNAL_ImFontConfig_ImFontConfig(self);
        
        [DllImport("cimgui", EntryPoint = "igIsMouseReleased", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsMouseReleased(int button);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseReleased(int button) => INTERNAL_igIsMouseReleased(button);
        
        [DllImport("cimgui", EntryPoint = "ImDrawData_ScaleClipRects", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawData_ScaleClipRects(ImDrawData* self, Vector2 sc);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawData_ScaleClipRects(ImDrawData* self, Vector2 sc) => INTERNAL_ImDrawData_ScaleClipRects(self, sc);
        
        [DllImport("cimgui", EntryPoint = "igGetItemRectMin", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetItemRectMin();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetItemRectMin() => INTERNAL_igGetItemRectMin();
        
        [DllImport("cimgui", EntryPoint = "ImDrawData_DeIndexAllBuffers", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawData_DeIndexAllBuffers(ImDrawData* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawData_DeIndexAllBuffers(ImDrawData* self) => INTERNAL_ImDrawData_DeIndexAllBuffers(self);
        
        [DllImport("cimgui", EntryPoint = "igLogText", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igLogText(byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogText(byte* fmt) => INTERNAL_igLogText(fmt);
        
        [DllImport("cimgui", EntryPoint = "ImDrawData_Clear", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawData_Clear(ImDrawData* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawData_Clear(ImDrawData* self) => INTERNAL_ImDrawData_Clear(self);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_GetVoidPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void* INTERNAL_ImGuiStorage_GetVoidPtr(ImGuiStorage* self, uint key);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void* ImGuiStorage_GetVoidPtr(ImGuiStorage* self, uint key) => INTERNAL_ImGuiStorage_GetVoidPtr(self, key);
        
        [DllImport("cimgui", EntryPoint = "igTextWrapped", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igTextWrapped(byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTextWrapped(byte* fmt) => INTERNAL_igTextWrapped(fmt);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_UpdateClipRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_UpdateClipRect(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_UpdateClipRect(ImDrawList* self) => INTERNAL_ImDrawList_UpdateClipRect(self);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PrimVtx", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PrimVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col) => INTERNAL_ImDrawList_PrimVtx(self, pos, uv, col);
        
        [DllImport("cimgui", EntryPoint = "igEndGroup", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndGroup();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndGroup() => INTERNAL_igEndGroup();
        
        [DllImport("cimgui", EntryPoint = "igGetFont", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImFont* INTERNAL_igGetFont();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* igGetFont() => INTERNAL_igGetFont();
        
        [DllImport("cimgui", EntryPoint = "igTreePushStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igTreePushStr(byte* str_id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTreePushStr(byte* str_id) => INTERNAL_igTreePushStr(str_id);
        
        [DllImport("cimgui", EntryPoint = "igTreePushPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igTreePushPtr(void* ptr_id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTreePushPtr(void* ptr_id) => INTERNAL_igTreePushPtr(ptr_id);
        
        [DllImport("cimgui", EntryPoint = "igTextDisabled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igTextDisabled(byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTextDisabled(byte* fmt) => INTERNAL_igTextDisabled(fmt);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PrimRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PrimRect(ImDrawList* self, Vector2 a, Vector2 b, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimRect(ImDrawList* self, Vector2 a, Vector2 b, uint col) => INTERNAL_ImDrawList_PrimRect(self, a, b, col);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddQuad", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddQuad(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col, float thickness);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddQuad(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col, float thickness) => INTERNAL_ImDrawList_AddQuad(self, a, b, c, d, col, thickness);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_ClearFreeMemory", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_ClearFreeMemory(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_ClearFreeMemory(ImDrawList* self) => INTERNAL_ImDrawList_ClearFreeMemory(self);
        
        [DllImport("cimgui", EntryPoint = "igSetNextTreeNodeOpen", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetNextTreeNodeOpen(byte is_open, ImGuiCond cond);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextTreeNodeOpen(byte is_open, ImGuiCond cond) => INTERNAL_igSetNextTreeNodeOpen(is_open, cond);
        
        [DllImport("cimgui", EntryPoint = "igLogToTTY", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igLogToTTY(int max_depth);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogToTTY(int max_depth) => INTERNAL_igLogToTTY(max_depth);
        
        [DllImport("cimgui", EntryPoint = "GlyphRangesBuilder_BuildRanges", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_GlyphRangesBuilder_BuildRanges(GlyphRangesBuilder* self, ImVector* out_ranges);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_BuildRanges(GlyphRangesBuilder* self, ImVector* out_ranges) => INTERNAL_GlyphRangesBuilder_BuildRanges(self, out_ranges);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_CloneOutput", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImDrawList* INTERNAL_ImDrawList_CloneOutput(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImDrawList* ImDrawList_CloneOutput(ImDrawList* self) => INTERNAL_ImDrawList_CloneOutput(self);
        
        [DllImport("cimgui", EntryPoint = "igGetIO", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImGuiIO* INTERNAL_igGetIO();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImGuiIO* igGetIO() => INTERNAL_igGetIO();
        
        [DllImport("cimgui", EntryPoint = "igDragInt4", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragInt4(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragInt4(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => INTERNAL_igDragInt4(label, v, v_speed, v_min, v_max, format);
        
        [DllImport("cimgui", EntryPoint = "igNextColumn", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igNextColumn();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igNextColumn() => INTERNAL_igNextColumn();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddRect(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags, float thickness);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddRect(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags, float thickness) => INTERNAL_ImDrawList_AddRect(self, a, b, col, rounding, rounding_corners_flags, thickness);
        
        [DllImport("cimgui", EntryPoint = "TextRange_split", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_TextRange_split(TextRange* self, byte separator, ImVector* @out);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void TextRange_split(TextRange* self, byte separator, ImVector* @out) => INTERNAL_TextRange_split(self, separator, @out);
        
        [DllImport("cimgui", EntryPoint = "igSetCursorPos", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetCursorPos(Vector2 local_pos);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetCursorPos(Vector2 local_pos) => INTERNAL_igSetCursorPos(local_pos);
        
        [DllImport("cimgui", EntryPoint = "igBeginPopupModal", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginPopupModal(byte* name, byte* p_open, ImGuiWindowFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginPopupModal(byte* name, byte* p_open, ImGuiWindowFlags flags) => INTERNAL_igBeginPopupModal(name, p_open, flags);
        
        [DllImport("cimgui", EntryPoint = "igSliderInt4", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderInt4(byte* label, int* v, int v_min, int v_max, byte* format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderInt4(byte* label, int* v, int v_min, int v_max, byte* format) => INTERNAL_igSliderInt4(label, v, v_min, v_max, format);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddCallback", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddCallback(ImDrawList* self, IntPtr callback, void* callback_data);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddCallback(ImDrawList* self, IntPtr callback, void* callback_data) => INTERNAL_ImDrawList_AddCallback(self, callback, callback_data);
        
        [DllImport("cimgui", EntryPoint = "igShowMetricsWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igShowMetricsWindow(byte* p_open);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igShowMetricsWindow(byte* p_open) => INTERNAL_igShowMetricsWindow(p_open);
        
        [DllImport("cimgui", EntryPoint = "igGetScrollMaxY", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetScrollMaxY();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetScrollMaxY() => INTERNAL_igGetScrollMaxY();
        
        [DllImport("cimgui", EntryPoint = "igBeginTooltip", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igBeginTooltip();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igBeginTooltip() => INTERNAL_igBeginTooltip();
        
        [DllImport("cimgui", EntryPoint = "igSetScrollX", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetScrollX(float scroll_x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetScrollX(float scroll_x) => INTERNAL_igSetScrollX(scroll_x);
        
        [DllImport("cimgui", EntryPoint = "igGetDrawData", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImDrawData* INTERNAL_igGetDrawData();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImDrawData* igGetDrawData() => INTERNAL_igGetDrawData();
        
        [DllImport("cimgui", EntryPoint = "igGetTextLineHeight", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetTextLineHeight();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetTextLineHeight() => INTERNAL_igGetTextLineHeight();
        
        [DllImport("cimgui", EntryPoint = "igSeparator", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSeparator();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSeparator() => INTERNAL_igSeparator();
        
        [DllImport("cimgui", EntryPoint = "igBeginChild", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginChild(byte* str_id, Vector2 size, byte border, ImGuiWindowFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginChild(byte* str_id, Vector2 size, byte border, ImGuiWindowFlags flags) => INTERNAL_igBeginChild(str_id, size, border, flags);
        
        [DllImport("cimgui", EntryPoint = "igBeginChildID", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginChildID(uint id, Vector2 size, byte border, ImGuiWindowFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginChildID(uint id, Vector2 size, byte border, ImGuiWindowFlags flags) => INTERNAL_igBeginChildID(id, size, border, flags);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PathRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PathRect(ImDrawList* self, Vector2 rect_min, Vector2 rect_max, float rounding, int rounding_corners_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathRect(ImDrawList* self, Vector2 rect_min, Vector2 rect_max, float rounding, int rounding_corners_flags) => INTERNAL_ImDrawList_PathRect(self, rect_min, rect_max, rounding, rounding_corners_flags);
        
        [DllImport("cimgui", EntryPoint = "igIsMouseClicked", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsMouseClicked(int button, byte repeat);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseClicked(int button, byte repeat) => INTERNAL_igIsMouseClicked(button, repeat);
        
        [DllImport("cimgui", EntryPoint = "igCalcItemWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igCalcItemWidth();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igCalcItemWidth() => INTERNAL_igCalcItemWidth();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PathArcToFast", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PathArcToFast(ImDrawList* self, Vector2 centre, float radius, int a_min_of_12, int a_max_of_12);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathArcToFast(ImDrawList* self, Vector2 centre, float radius, int a_min_of_12, int a_max_of_12) => INTERNAL_ImDrawList_PathArcToFast(self, centre, radius, a_min_of_12, a_max_of_12);
        
        [DllImport("cimgui", EntryPoint = "igEndChildFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndChildFrame();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndChildFrame() => INTERNAL_igEndChildFrame();
        
        [DllImport("cimgui", EntryPoint = "igIndent", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igIndent(float indent_w);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igIndent(float indent_w) => INTERNAL_igIndent(indent_w);
        
        [DllImport("cimgui", EntryPoint = "igSetDragDropPayload", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSetDragDropPayload(byte* type, void* data, uint size, ImGuiCond cond);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSetDragDropPayload(byte* type, void* data, uint size, ImGuiCond cond) => INTERNAL_igSetDragDropPayload(type, data, size, cond);
        
        [DllImport("cimgui", EntryPoint = "GlyphRangesBuilder_GetBit", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_GlyphRangesBuilder_GetBit(GlyphRangesBuilder* self, int n);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte GlyphRangesBuilder_GetBit(GlyphRangesBuilder* self, int n) => INTERNAL_GlyphRangesBuilder_GetBit(self, n);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextFilter_Draw", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImGuiTextFilter_Draw(ImGuiTextFilter* self, byte* label, float width);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiTextFilter_Draw(ImGuiTextFilter* self, byte* label, float width) => INTERNAL_ImGuiTextFilter_Draw(self, label, width);
        
        [DllImport("cimgui", EntryPoint = "igShowDemoWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igShowDemoWindow(byte* p_open);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igShowDemoWindow(byte* p_open) => INTERNAL_igShowDemoWindow(p_open);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PathStroke", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PathStroke(ImDrawList* self, uint col, byte closed, float thickness);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathStroke(ImDrawList* self, uint col, byte closed, float thickness) => INTERNAL_ImDrawList_PathStroke(self, col, closed, thickness);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PathFillConvex", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PathFillConvex(ImDrawList* self, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathFillConvex(ImDrawList* self, uint col) => INTERNAL_ImDrawList_PathFillConvex(self, col);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PathLineToMergeDuplicate", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PathLineToMergeDuplicate(ImDrawList* self, Vector2 pos);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathLineToMergeDuplicate(ImDrawList* self, Vector2 pos) => INTERNAL_ImDrawList_PathLineToMergeDuplicate(self, pos);
        
        [DllImport("cimgui", EntryPoint = "igEndMenu", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndMenu();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndMenu() => INTERNAL_igEndMenu();
        
        [DllImport("cimgui", EntryPoint = "igColorButton", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igColorButton(byte* desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igColorButton(byte* desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size) => INTERNAL_igColorButton(desc_id, col, flags, size);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetTexDataAsAlpha8", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFontAtlas_GetTexDataAsAlpha8(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_GetTexDataAsAlpha8(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel) => INTERNAL_ImFontAtlas_GetTexDataAsAlpha8(self, out_pixels, out_width, out_height, out_bytes_per_pixel);
        
        [DllImport("cimgui", EntryPoint = "igIsKeyReleased", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsKeyReleased(int user_key_index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsKeyReleased(int user_key_index) => INTERNAL_igIsKeyReleased(user_key_index);
        
        [DllImport("cimgui", EntryPoint = "igSetClipboardText", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetClipboardText(byte* text);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetClipboardText(byte* text) => INTERNAL_igSetClipboardText(text);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PathArcTo", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PathArcTo(ImDrawList* self, Vector2 centre, float radius, float a_min, float a_max, int num_segments);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathArcTo(ImDrawList* self, Vector2 centre, float radius, float a_min, float a_max, int num_segments) => INTERNAL_ImDrawList_PathArcTo(self, centre, radius, a_min, a_max, num_segments);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddConvexPolyFilled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddConvexPolyFilled(ImDrawList* self, Vector2* points, int num_points, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddConvexPolyFilled(ImDrawList* self, Vector2* points, int num_points, uint col) => INTERNAL_ImDrawList_AddConvexPolyFilled(self, points, num_points, col);
        
        [DllImport("cimgui", EntryPoint = "igIsWindowCollapsed", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsWindowCollapsed();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsWindowCollapsed() => INTERNAL_igIsWindowCollapsed();
        
        [DllImport("cimgui", EntryPoint = "igShowFontSelector", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igShowFontSelector(byte* label);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igShowFontSelector(byte* label) => INTERNAL_igShowFontSelector(label);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddImageQuad", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddImageQuad(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddImageQuad(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col) => INTERNAL_ImDrawList_AddImageQuad(self, user_texture_id, a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
        
        [DllImport("cimgui", EntryPoint = "igSetNextWindowFocus", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetNextWindowFocus();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowFocus() => INTERNAL_igSetNextWindowFocus();
        
        [DllImport("cimgui", EntryPoint = "igSameLine", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSameLine(float pos_x, float spacing_w);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSameLine(float pos_x, float spacing_w) => INTERNAL_igSameLine(pos_x, spacing_w);
        
        [DllImport("cimgui", EntryPoint = "igBegin", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBegin(byte* name, byte* p_open, ImGuiWindowFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBegin(byte* name, byte* p_open, ImGuiWindowFlags flags) => INTERNAL_igBegin(name, p_open, flags);
        
        [DllImport("cimgui", EntryPoint = "igColorEdit3", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igColorEdit3(byte* label, Vector3* col, ImGuiColorEditFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igColorEdit3(byte* label, Vector3* col, ImGuiColorEditFlags flags) => INTERNAL_igColorEdit3(label, col, flags);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddImage", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddImage(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddImage(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col) => INTERNAL_ImDrawList_AddImage(self, user_texture_id, a, b, uv_a, uv_b, col);
        
        [DllImport("cimgui", EntryPoint = "ImGuiIO_AddInputCharactersUTF8", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiIO_AddInputCharactersUTF8(ImGuiIO* self, byte* utf8_chars);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiIO_AddInputCharactersUTF8(ImGuiIO* self, byte* utf8_chars) => INTERNAL_ImGuiIO_AddInputCharactersUTF8(self, utf8_chars);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddText", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddText(ImDrawList* self, Vector2 pos, uint col, byte* text_begin, byte* text_end);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddText(ImDrawList* self, Vector2 pos, uint col, byte* text_begin, byte* text_end) => INTERNAL_ImDrawList_AddText(self, pos, col, text_begin, text_end);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddTextFontPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddTextFontPtr(ImDrawList* self, ImFont* font, float font_size, Vector2 pos, uint col, byte* text_begin, byte* text_end, float wrap_width, Vector4* cpu_fine_clip_rect);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddTextFontPtr(ImDrawList* self, ImFont* font, float font_size, Vector2 pos, uint col, byte* text_begin, byte* text_end, float wrap_width, Vector4* cpu_fine_clip_rect) => INTERNAL_ImDrawList_AddTextFontPtr(self, font, font_size, pos, col, text_begin, text_end, wrap_width, cpu_fine_clip_rect);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddCircleFilled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddCircleFilled(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddCircleFilled(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments) => INTERNAL_ImDrawList_AddCircleFilled(self, centre, radius, col, num_segments);
        
        [DllImport("cimgui", EntryPoint = "igInputFloat2", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputFloat2(byte* label, Vector2* v, byte* format, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputFloat2(byte* label, Vector2* v, byte* format, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputFloat2(label, v, format, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "igPushButtonRepeat", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushButtonRepeat(byte repeat);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushButtonRepeat(byte repeat) => INTERNAL_igPushButtonRepeat(repeat);
        
        [DllImport("cimgui", EntryPoint = "igPopItemWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPopItemWidth();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopItemWidth() => INTERNAL_igPopItemWidth();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddCircle", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddCircle(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments, float thickness);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddCircle(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments, float thickness) => INTERNAL_ImDrawList_AddCircle(self, centre, radius, col, num_segments, thickness);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddTriangleFilled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddTriangleFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddTriangleFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col) => INTERNAL_ImDrawList_AddTriangleFilled(self, a, b, c, col);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddTriangle", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddTriangle(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col, float thickness);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddTriangle(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col, float thickness) => INTERNAL_ImDrawList_AddTriangle(self, a, b, c, col, thickness);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddQuadFilled", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddQuadFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddQuadFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col) => INTERNAL_ImDrawList_AddQuadFilled(self, a, b, c, d, col);
        
        [DllImport("cimgui", EntryPoint = "igGetFontSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetFontSize();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetFontSize() => INTERNAL_igGetFontSize();
        
        [DllImport("cimgui", EntryPoint = "igInputDouble", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputDouble(byte* label, double* v, double step, double step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputDouble(byte* label, double* v, double step, double step_fast, byte* format, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputDouble(label, v, step, step_fast, format, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PrimReserve", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PrimReserve(ImDrawList* self, int idx_count, int vtx_count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimReserve(ImDrawList* self, int idx_count, int vtx_count) => INTERNAL_ImDrawList_PrimReserve(self, idx_count, vtx_count);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddRectFilledMultiColor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddRectFilledMultiColor(ImDrawList* self, Vector2 a, Vector2 b, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddRectFilledMultiColor(ImDrawList* self, Vector2 a, Vector2 b, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left) => INTERNAL_ImDrawList_AddRectFilledMultiColor(self, a, b, col_upr_left, col_upr_right, col_bot_right, col_bot_left);
        
        [DllImport("cimgui", EntryPoint = "igEndPopup", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndPopup();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndPopup() => INTERNAL_igEndPopup();
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_ClearInputData", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFontAtlas_ClearInputData(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_ClearInputData(ImFontAtlas* self) => INTERNAL_ImFontAtlas_ClearInputData(self);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddLine", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddLine(ImDrawList* self, Vector2 a, Vector2 b, uint col, float thickness);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddLine(ImDrawList* self, Vector2 a, Vector2 b, uint col, float thickness) => INTERNAL_ImDrawList_AddLine(self, a, b, col, thickness);
        
        [DllImport("cimgui", EntryPoint = "igInputTextMultiline", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputTextMultiline(byte* label, byte* buf, uint buf_size, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputTextMultiline(byte* label, byte* buf, uint buf_size, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data) => INTERNAL_igInputTextMultiline(label, buf, buf_size, size, flags, callback, user_data);
        
        [DllImport("cimgui", EntryPoint = "igSelectable", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSelectable(byte* label, byte selected, ImGuiSelectableFlags flags, Vector2 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSelectable(byte* label, byte selected, ImGuiSelectableFlags flags, Vector2 size) => INTERNAL_igSelectable(label, selected, flags, size);
        
        [DllImport("cimgui", EntryPoint = "igSelectableBoolPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSelectableBoolPtr(byte* label, byte* p_selected, ImGuiSelectableFlags flags, Vector2 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSelectableBoolPtr(byte* label, byte* p_selected, ImGuiSelectableFlags flags, Vector2 size) => INTERNAL_igSelectableBoolPtr(label, p_selected, flags, size);
        
        [DllImport("cimgui", EntryPoint = "igListBoxStr_arr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igListBoxStr_arr(byte* label, int* current_item, byte** items, int items_count, int height_in_items);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igListBoxStr_arr(byte* label, int* current_item, byte** items, int items_count, int height_in_items) => INTERNAL_igListBoxStr_arr(label, current_item, items, items_count, height_in_items);
        
        [DllImport("cimgui", EntryPoint = "igGetCursorPos", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetCursorPos();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetCursorPos() => INTERNAL_igGetCursorPos();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_GetClipRectMin", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_ImDrawList_GetClipRectMin(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 ImDrawList_GetClipRectMin(ImDrawList* self) => INTERNAL_ImDrawList_GetClipRectMin(self);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PopTextureID", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PopTextureID(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PopTextureID(ImDrawList* self) => INTERNAL_ImDrawList_PopTextureID(self);
        
        [DllImport("cimgui", EntryPoint = "igInputFloat4", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputFloat4(byte* label, Vector4* v, byte* format, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputFloat4(byte* label, Vector4* v, byte* format, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputFloat4(label, v, format, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "igSetCursorPosY", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetCursorPosY(float y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetCursorPosY(float y) => INTERNAL_igSetCursorPosY(y);
        
        [DllImport("cimgui", EntryPoint = "igGetVersion", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_igGetVersion();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* igGetVersion() => INTERNAL_igGetVersion();
        
        [DllImport("cimgui", EntryPoint = "igEndCombo", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndCombo();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndCombo() => INTERNAL_igEndCombo();
        
        [DllImport("cimgui", EntryPoint = "igPushIDStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushIDStr(byte* str_id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushIDStr(byte* str_id) => INTERNAL_igPushIDStr(str_id);
        
        [DllImport("cimgui", EntryPoint = "igPushIDRange", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushIDRange(byte* str_id_begin, byte* str_id_end);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushIDRange(byte* str_id_begin, byte* str_id_end) => INTERNAL_igPushIDRange(str_id_begin, str_id_end);
        
        [DllImport("cimgui", EntryPoint = "igPushIDPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushIDPtr(void* ptr_id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushIDPtr(void* ptr_id) => INTERNAL_igPushIDPtr(ptr_id);
        
        [DllImport("cimgui", EntryPoint = "igPushIDInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushIDInt(int int_id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushIDInt(int int_id) => INTERNAL_igPushIDInt(int_id);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_ImDrawList", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_ImDrawList(ImDrawList* self, IntPtr shared_data);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_ImDrawList(ImDrawList* self, IntPtr shared_data) => INTERNAL_ImDrawList_ImDrawList(self, shared_data);
        
        [DllImport("cimgui", EntryPoint = "ImDrawCmd_ImDrawCmd", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawCmd_ImDrawCmd(ImDrawCmd* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawCmd_ImDrawCmd(ImDrawCmd* self) => INTERNAL_ImDrawCmd_ImDrawCmd(self);
        
        [DllImport("cimgui", EntryPoint = "ImGuiListClipper_End", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiListClipper_End(ImGuiListClipper* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiListClipper_End(ImGuiListClipper* self) => INTERNAL_ImGuiListClipper_End(self);
        
        [DllImport("cimgui", EntryPoint = "igAlignTextToFramePadding", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igAlignTextToFramePadding();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igAlignTextToFramePadding() => INTERNAL_igAlignTextToFramePadding();
        
        [DllImport("cimgui", EntryPoint = "igPopStyleColor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPopStyleColor(int count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopStyleColor(int count) => INTERNAL_igPopStyleColor(count);
        
        [DllImport("cimgui", EntryPoint = "ImGuiListClipper_Begin", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiListClipper_Begin(ImGuiListClipper* self, int items_count, float items_height);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiListClipper_Begin(ImGuiListClipper* self, int items_count, float items_height) => INTERNAL_ImGuiListClipper_Begin(self, items_count, items_height);
        
        [DllImport("cimgui", EntryPoint = "igText", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igText(byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igText(byte* fmt) => INTERNAL_igText(fmt);
        
        [DllImport("cimgui", EntryPoint = "ImGuiListClipper_Step", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImGuiListClipper_Step(ImGuiListClipper* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiListClipper_Step(ImGuiListClipper* self) => INTERNAL_ImGuiListClipper_Step(self);
        
        [DllImport("cimgui", EntryPoint = "igGetTextLineHeightWithSpacing", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetTextLineHeightWithSpacing();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetTextLineHeightWithSpacing() => INTERNAL_igGetTextLineHeightWithSpacing();
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_GetFloatRef", CallingConvention = CallingConvention.Cdecl)]
        private static extern float* INTERNAL_ImGuiStorage_GetFloatRef(ImGuiStorage* self, uint key, float default_val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float* ImGuiStorage_GetFloatRef(ImGuiStorage* self, uint key, float default_val) => INTERNAL_ImGuiStorage_GetFloatRef(self, key, default_val);
        
        [DllImport("cimgui", EntryPoint = "igEndTooltip", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndTooltip();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndTooltip() => INTERNAL_igEndTooltip();
        
        [DllImport("cimgui", EntryPoint = "ImGuiListClipper_ImGuiListClipper", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiListClipper_ImGuiListClipper(ImGuiListClipper* self, int items_count, float items_height);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiListClipper_ImGuiListClipper(ImGuiListClipper* self, int items_count, float items_height) => INTERNAL_ImGuiListClipper_ImGuiListClipper(self, items_count, items_height);
        
        [DllImport("cimgui", EntryPoint = "igDragInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragInt(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragInt(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => INTERNAL_igDragInt(label, v, v_speed, v_min, v_max, format);
        
        [DllImport("cimgui", EntryPoint = "igSliderFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderFloat(byte* label, float* v, float v_min, float v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderFloat(byte* label, float* v, float v_min, float v_max, byte* format, float power) => INTERNAL_igSliderFloat(label, v, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "igColorConvertFloat4ToU32", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint INTERNAL_igColorConvertFloat4ToU32(Vector4 @in);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igColorConvertFloat4ToU32(Vector4 @in) => INTERNAL_igColorConvertFloat4ToU32(@in);
        
        [DllImport("cimgui", EntryPoint = "ImGuiIO_ClearInputCharacters", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiIO_ClearInputCharacters(ImGuiIO* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiIO_ClearInputCharacters(ImGuiIO* self) => INTERNAL_ImGuiIO_ClearInputCharacters(self);
        
        [DllImport("cimgui", EntryPoint = "igPushClipRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect) => INTERNAL_igPushClipRect(clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
        
        [DllImport("cimgui", EntryPoint = "igSetColumnWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetColumnWidth(int column_index, float width);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetColumnWidth(int column_index, float width) => INTERNAL_igSetColumnWidth(column_index, width);
        
        [DllImport("cimgui", EntryPoint = "ImGuiPayload_IsDataType", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImGuiPayload_IsDataType(ImGuiPayload* self, byte* type);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiPayload_IsDataType(ImGuiPayload* self, byte* type) => INTERNAL_ImGuiPayload_IsDataType(self, type);
        
        [DllImport("cimgui", EntryPoint = "igBeginMainMenuBar", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginMainMenuBar();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginMainMenuBar() => INTERNAL_igBeginMainMenuBar();
        
        [DllImport("cimgui", EntryPoint = "CustomRect_CustomRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_CustomRect_CustomRect(CustomRect* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void CustomRect_CustomRect(CustomRect* self) => INTERNAL_CustomRect_CustomRect(self);
        
        [DllImport("cimgui", EntryPoint = "ImGuiInputTextCallbackData_HasSelection", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImGuiInputTextCallbackData_HasSelection(ImGuiInputTextCallbackData* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiInputTextCallbackData_HasSelection(ImGuiInputTextCallbackData* self) => INTERNAL_ImGuiInputTextCallbackData_HasSelection(self);
        
        [DllImport("cimgui", EntryPoint = "ImGuiInputTextCallbackData_InsertChars", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiInputTextCallbackData_InsertChars(ImGuiInputTextCallbackData* self, int pos, byte* text, byte* text_end);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiInputTextCallbackData_InsertChars(ImGuiInputTextCallbackData* self, int pos, byte* text, byte* text_end) => INTERNAL_ImGuiInputTextCallbackData_InsertChars(self, pos, text, text_end);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetMouseCursorTexData", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImFontAtlas_GetMouseCursorTexData(ImFontAtlas* self, ImGuiMouseCursor cursor, Vector2* out_offset, Vector2* out_size, Vector2* out_uv_border, Vector2* out_uv_fill);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImFontAtlas_GetMouseCursorTexData(ImFontAtlas* self, ImGuiMouseCursor cursor, Vector2* out_offset, Vector2* out_size, Vector2* out_uv_border, Vector2* out_uv_fill) => INTERNAL_ImFontAtlas_GetMouseCursorTexData(self, cursor, out_offset, out_size, out_uv_border, out_uv_fill);
        
        [DllImport("cimgui", EntryPoint = "igVSliderScalar", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igVSliderScalar(byte* label, Vector2 size, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igVSliderScalar(byte* label, Vector2 size, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power) => INTERNAL_igVSliderScalar(label, size, data_type, v, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_SetAllInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiStorage_SetAllInt(ImGuiStorage* self, int val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_SetAllInt(ImGuiStorage* self, int val) => INTERNAL_ImGuiStorage_SetAllInt(self, val);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_GetVoidPtrRef", CallingConvention = CallingConvention.Cdecl)]
        private static extern void** INTERNAL_ImGuiStorage_GetVoidPtrRef(ImGuiStorage* self, uint key, void* default_val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void** ImGuiStorage_GetVoidPtrRef(ImGuiStorage* self, uint key, void* default_val) => INTERNAL_ImGuiStorage_GetVoidPtrRef(self, key, default_val);
        
        [DllImport("cimgui", EntryPoint = "igStyleColorsLight", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igStyleColorsLight(ImGuiStyle* dst);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igStyleColorsLight(ImGuiStyle* dst) => INTERNAL_igStyleColorsLight(dst);
        
        [DllImport("cimgui", EntryPoint = "igSliderFloat3", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderFloat3(byte* label, Vector3* v, float v_min, float v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderFloat3(byte* label, Vector3* v, float v_min, float v_max, byte* format, float power) => INTERNAL_igSliderFloat3(label, v, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "igDragFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragFloat(byte* label, float* v, float v_speed, float v_min, float v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragFloat(byte* label, float* v, float v_speed, float v_min, float v_max, byte* format, float power) => INTERNAL_igDragFloat(label, v, v_speed, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_GetBoolRef", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_ImGuiStorage_GetBoolRef(ImGuiStorage* self, uint key, byte default_val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImGuiStorage_GetBoolRef(ImGuiStorage* self, uint key, byte default_val) => INTERNAL_ImGuiStorage_GetBoolRef(self, key, default_val);
        
        [DllImport("cimgui", EntryPoint = "igGetWindowHeight", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetWindowHeight();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetWindowHeight() => INTERNAL_igGetWindowHeight();
        
        [DllImport("cimgui", EntryPoint = "igGetMousePosOnOpeningCurrentPopup", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetMousePosOnOpeningCurrentPopup();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetMousePosOnOpeningCurrentPopup() => INTERNAL_igGetMousePosOnOpeningCurrentPopup();
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_GetIntRef", CallingConvention = CallingConvention.Cdecl)]
        private static extern int* INTERNAL_ImGuiStorage_GetIntRef(ImGuiStorage* self, uint key, int default_val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int* ImGuiStorage_GetIntRef(ImGuiStorage* self, uint key, int default_val) => INTERNAL_ImGuiStorage_GetIntRef(self, key, default_val);
        
        [DllImport("cimgui", EntryPoint = "igCalcListClipping", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igCalcListClipping(int items_count, float items_height, int* out_items_display_start, int* out_items_display_end);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igCalcListClipping(int items_count, float items_height, int* out_items_display_start, int* out_items_display_end) => INTERNAL_igCalcListClipping(items_count, items_height, out_items_display_start, out_items_display_end);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_SetVoidPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiStorage_SetVoidPtr(ImGuiStorage* self, uint key, void* val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_SetVoidPtr(ImGuiStorage* self, uint key, void* val) => INTERNAL_ImGuiStorage_SetVoidPtr(self, key, val);
        
        [DllImport("cimgui", EntryPoint = "igEndDragDropSource", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndDragDropSource();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndDragDropSource() => INTERNAL_igEndDragDropSource();
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_BuildSortByKey", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiStorage_BuildSortByKey(ImGuiStorage* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_BuildSortByKey(ImGuiStorage* self) => INTERNAL_ImGuiStorage_BuildSortByKey(self);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_GetFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_ImGuiStorage_GetFloat(ImGuiStorage* self, uint key, float default_val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float ImGuiStorage_GetFloat(ImGuiStorage* self, uint key, float default_val) => INTERNAL_ImGuiStorage_GetFloat(self, key, default_val);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_SetBool", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiStorage_SetBool(ImGuiStorage* self, uint key, byte val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_SetBool(ImGuiStorage* self, uint key, byte val) => INTERNAL_ImGuiStorage_SetBool(self, key, val);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_GetBool", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImGuiStorage_GetBool(ImGuiStorage* self, uint key, byte default_val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiStorage_GetBool(ImGuiStorage* self, uint key, byte default_val) => INTERNAL_ImGuiStorage_GetBool(self, key, default_val);
        
        [DllImport("cimgui", EntryPoint = "igGetFrameHeightWithSpacing", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetFrameHeightWithSpacing();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetFrameHeightWithSpacing() => INTERNAL_igGetFrameHeightWithSpacing();
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_SetInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiStorage_SetInt(ImGuiStorage* self, uint key, int val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_SetInt(ImGuiStorage* self, uint key, int val) => INTERNAL_ImGuiStorage_SetInt(self, key, val);
        
        [DllImport("cimgui", EntryPoint = "igCloseCurrentPopup", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igCloseCurrentPopup();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igCloseCurrentPopup() => INTERNAL_igCloseCurrentPopup();
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextBuffer_clear", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiTextBuffer_clear(ImGuiTextBuffer* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextBuffer_clear(ImGuiTextBuffer* self) => INTERNAL_ImGuiTextBuffer_clear(self);
        
        [DllImport("cimgui", EntryPoint = "igBeginGroup", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igBeginGroup();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igBeginGroup() => INTERNAL_igBeginGroup();
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_Clear", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiStorage_Clear(ImGuiStorage* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_Clear(ImGuiStorage* self) => INTERNAL_ImGuiStorage_Clear(self);
        
        [DllImport("cimgui", EntryPoint = "Pair_PairInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_Pair_PairInt(Pair* self, uint _key, int _val_i);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void Pair_PairInt(Pair* self, uint _key, int _val_i) => INTERNAL_Pair_PairInt(self, _key, _val_i);
        
        [DllImport("cimgui", EntryPoint = "Pair_PairFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_Pair_PairFloat(Pair* self, uint _key, float _val_f);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void Pair_PairFloat(Pair* self, uint _key, float _val_f) => INTERNAL_Pair_PairFloat(self, _key, _val_f);
        
        [DllImport("cimgui", EntryPoint = "Pair_PairPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_Pair_PairPtr(Pair* self, uint _key, void* _val_p);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void Pair_PairPtr(Pair* self, uint _key, void* _val_p) => INTERNAL_Pair_PairPtr(self, _key, _val_p);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextBuffer_appendf", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiTextBuffer_appendf(ImGuiTextBuffer* self, byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextBuffer_appendf(ImGuiTextBuffer* self, byte* fmt) => INTERNAL_ImGuiTextBuffer_appendf(self, fmt);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextBuffer_c_str", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_ImGuiTextBuffer_c_str(ImGuiTextBuffer* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImGuiTextBuffer_c_str(ImGuiTextBuffer* self) => INTERNAL_ImGuiTextBuffer_c_str(self);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextBuffer_reserve", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiTextBuffer_reserve(ImGuiTextBuffer* self, int capacity);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextBuffer_reserve(ImGuiTextBuffer* self, int capacity) => INTERNAL_ImGuiTextBuffer_reserve(self, capacity);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextBuffer_empty", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImGuiTextBuffer_empty(ImGuiTextBuffer* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiTextBuffer_empty(ImGuiTextBuffer* self) => INTERNAL_ImGuiTextBuffer_empty(self);
        
        [DllImport("cimgui", EntryPoint = "igSliderScalar", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderScalar(byte* label, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderScalar(byte* label, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power) => INTERNAL_igSliderScalar(label, data_type, v, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "igBeginCombo", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginCombo(byte* label, byte* preview_value, ImGuiComboFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginCombo(byte* label, byte* preview_value, ImGuiComboFlags flags) => INTERNAL_igBeginCombo(label, preview_value, flags);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextBuffer_size", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_ImGuiTextBuffer_size(ImGuiTextBuffer* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int ImGuiTextBuffer_size(ImGuiTextBuffer* self) => INTERNAL_ImGuiTextBuffer_size(self);
        
        [DllImport("cimgui", EntryPoint = "igBeginMenu", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginMenu(byte* label, byte enabled);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginMenu(byte* label, byte enabled) => INTERNAL_igBeginMenu(label, enabled);
        
        [DllImport("cimgui", EntryPoint = "igIsItemHovered", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsItemHovered(ImGuiHoveredFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemHovered(ImGuiHoveredFlags flags) => INTERNAL_igIsItemHovered(flags);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PrimWriteVtx", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PrimWriteVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimWriteVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col) => INTERNAL_ImDrawList_PrimWriteVtx(self, pos, uv, col);
        
        [DllImport("cimgui", EntryPoint = "igBullet", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igBullet();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igBullet() => INTERNAL_igBullet();
        
        [DllImport("cimgui", EntryPoint = "igInputText", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputText(byte* label, byte* buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputText(byte* label, byte* buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data) => INTERNAL_igInputText(label, buf, buf_size, flags, callback, user_data);
        
        [DllImport("cimgui", EntryPoint = "igInputInt3", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputInt3(byte* label, int* v, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputInt3(byte* label, int* v, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputInt3(label, v, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "ImGuiIO_ImGuiIO", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiIO_ImGuiIO(ImGuiIO* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiIO_ImGuiIO(ImGuiIO* self) => INTERNAL_ImGuiIO_ImGuiIO(self);
        
        [DllImport("cimgui", EntryPoint = "igStyleColorsDark", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igStyleColorsDark(ImGuiStyle* dst);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igStyleColorsDark(ImGuiStyle* dst) => INTERNAL_igStyleColorsDark(dst);
        
        [DllImport("cimgui", EntryPoint = "igInputInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputInt(byte* label, int* v, int step, int step_fast, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputInt(byte* label, int* v, int step, int step_fast, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputInt(label, v, step, step_fast, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "igSetWindowFontScale", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetWindowFontScale(float scale);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowFontScale(float scale) => INTERNAL_igSetWindowFontScale(scale);
        
        [DllImport("cimgui", EntryPoint = "igSliderInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderInt(byte* label, int* v, int v_min, int v_max, byte* format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderInt(byte* label, int* v, int v_min, int v_max, byte* format) => INTERNAL_igSliderInt(label, v, v_min, v_max, format);
        
        [DllImport("cimgui", EntryPoint = "TextRange_end", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_TextRange_end(TextRange* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* TextRange_end(TextRange* self) => INTERNAL_TextRange_end(self);
        
        [DllImport("cimgui", EntryPoint = "TextRange_begin", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_TextRange_begin(TextRange* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* TextRange_begin(TextRange* self) => INTERNAL_TextRange_begin(self);
        
        [DllImport("cimgui", EntryPoint = "igSetNextWindowPos", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetNextWindowPos(Vector2 pos, ImGuiCond cond, Vector2 pivot);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowPos(Vector2 pos, ImGuiCond cond, Vector2 pivot) => INTERNAL_igSetNextWindowPos(pos, cond, pivot);
        
        [DllImport("cimgui", EntryPoint = "igDragInt3", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragInt3(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragInt3(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => INTERNAL_igDragInt3(label, v, v_speed, v_min, v_max, format);
        
        [DllImport("cimgui", EntryPoint = "igOpenPopup", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igOpenPopup(byte* str_id);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igOpenPopup(byte* str_id) => INTERNAL_igOpenPopup(str_id);
        
        [DllImport("cimgui", EntryPoint = "TextRange_TextRange", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_TextRange_TextRange(TextRange* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void TextRange_TextRange(TextRange* self) => INTERNAL_TextRange_TextRange(self);
        
        [DllImport("cimgui", EntryPoint = "TextRange_TextRangeStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_TextRange_TextRangeStr(TextRange* self, byte* _b, byte* _e);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void TextRange_TextRangeStr(TextRange* self, byte* _b, byte* _e) => INTERNAL_TextRange_TextRangeStr(self, _b, _e);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_GetClipRectMax", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_ImDrawList_GetClipRectMax(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 ImDrawList_GetClipRectMax(ImDrawList* self) => INTERNAL_ImDrawList_GetClipRectMax(self);
        
        [DllImport("cimgui", EntryPoint = "igCalcTextSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igCalcTextSize(byte* text, byte* text_end, byte hide_text_after_double_hash, float wrap_width);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igCalcTextSize(byte* text, byte* text_end, byte hide_text_after_double_hash, float wrap_width) => INTERNAL_igCalcTextSize(text, text_end, hide_text_after_double_hash, wrap_width);
        
        [DllImport("cimgui", EntryPoint = "igGetDrawListSharedData", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_igGetDrawListSharedData();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr igGetDrawListSharedData() => INTERNAL_igGetDrawListSharedData();
        
        [DllImport("cimgui", EntryPoint = "igColumns", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igColumns(int count, byte* id, byte border);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igColumns(int count, byte* id, byte border) => INTERNAL_igColumns(count, id, border);
        
        [DllImport("cimgui", EntryPoint = "igIsItemActive", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsItemActive();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemActive() => INTERNAL_igIsItemActive();
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextFilter_ImGuiTextFilter", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiTextFilter_ImGuiTextFilter(ImGuiTextFilter* self, byte* default_filter);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextFilter_ImGuiTextFilter(ImGuiTextFilter* self, byte* default_filter) => INTERNAL_ImGuiTextFilter_ImGuiTextFilter(self, default_filter);
        
        [DllImport("cimgui", EntryPoint = "ImGuiOnceUponAFrame_ImGuiOnceUponAFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiOnceUponAFrame_ImGuiOnceUponAFrame(ImGuiOnceUponAFrame* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiOnceUponAFrame_ImGuiOnceUponAFrame(ImGuiOnceUponAFrame* self) => INTERNAL_ImGuiOnceUponAFrame_ImGuiOnceUponAFrame(self);
        
        [DllImport("cimgui", EntryPoint = "igBeginDragDropTarget", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginDragDropTarget();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginDragDropTarget() => INTERNAL_igBeginDragDropTarget();
        
        [DllImport("cimgui", EntryPoint = "TextRange_empty", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_TextRange_empty(TextRange* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte TextRange_empty(TextRange* self) => INTERNAL_TextRange_empty(self);
        
        [DllImport("cimgui", EntryPoint = "ImGuiPayload_IsDelivery", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImGuiPayload_IsDelivery(ImGuiPayload* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiPayload_IsDelivery(ImGuiPayload* self) => INTERNAL_ImGuiPayload_IsDelivery(self);
        
        [DllImport("cimgui", EntryPoint = "ImGuiIO_AddInputCharacter", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiIO_AddInputCharacter(ImGuiIO* self, ushort c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiIO_AddInputCharacter(ImGuiIO* self, ushort c) => INTERNAL_ImGuiIO_AddInputCharacter(self, c);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_AddImageRounded", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_AddImageRounded(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col, float rounding, int rounding_corners);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddImageRounded(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col, float rounding, int rounding_corners) => INTERNAL_ImDrawList_AddImageRounded(self, user_texture_id, a, b, uv_a, uv_b, col, rounding, rounding_corners);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStyle_ImGuiStyle", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiStyle_ImGuiStyle(ImGuiStyle* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStyle_ImGuiStyle(ImGuiStyle* self) => INTERNAL_ImGuiStyle_ImGuiStyle(self);
        
        [DllImport("cimgui", EntryPoint = "igColorPicker3", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igColorPicker3(byte* label, Vector3* col, ImGuiColorEditFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igColorPicker3(byte* label, Vector3* col, ImGuiColorEditFlags flags) => INTERNAL_igColorPicker3(label, col, flags);
        
        [DllImport("cimgui", EntryPoint = "igGetContentRegionMax", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetContentRegionMax();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetContentRegionMax() => INTERNAL_igGetContentRegionMax();
        
        [DllImport("cimgui", EntryPoint = "igBeginChildFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags) => INTERNAL_igBeginChildFrame(id, size, flags);
        
        [DllImport("cimgui", EntryPoint = "igSaveIniSettingsToDisk", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSaveIniSettingsToDisk(byte* ini_filename);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSaveIniSettingsToDisk(byte* ini_filename) => INTERNAL_igSaveIniSettingsToDisk(ini_filename);
        
        [DllImport("cimgui", EntryPoint = "ImFont_ClearOutputData", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFont_ClearOutputData(ImFont* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_ClearOutputData(ImFont* self) => INTERNAL_ImFont_ClearOutputData(self);
        
        [DllImport("cimgui", EntryPoint = "igGetClipboardText", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_igGetClipboardText();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* igGetClipboardText() => INTERNAL_igGetClipboardText();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PrimQuadUV", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PrimQuadUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimQuadUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col) => INTERNAL_ImDrawList_PrimQuadUV(self, a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
        
        [DllImport("cimgui", EntryPoint = "igEndDragDropTarget", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndDragDropTarget();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndDragDropTarget() => INTERNAL_igEndDragDropTarget();
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetGlyphRangesKorean", CallingConvention = CallingConvention.Cdecl)]
        private static extern ushort* INTERNAL_ImFontAtlas_GetGlyphRangesKorean(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesKorean(ImFontAtlas* self) => INTERNAL_ImFontAtlas_GetGlyphRangesKorean(self);
        
        [DllImport("cimgui", EntryPoint = "igGetKeyPressedAmount", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_igGetKeyPressedAmount(int key_index, float repeat_delay, float rate);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int igGetKeyPressedAmount(int key_index, float repeat_delay, float rate) => INTERNAL_igGetKeyPressedAmount(key_index, repeat_delay, rate);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_GetTexDataAsRGBA32", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFontAtlas_GetTexDataAsRGBA32(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_GetTexDataAsRGBA32(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel) => INTERNAL_ImFontAtlas_GetTexDataAsRGBA32(self, out_pixels, out_width, out_height, out_bytes_per_pixel);
        
        [DllImport("cimgui", EntryPoint = "igNewFrame", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igNewFrame();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igNewFrame() => INTERNAL_igNewFrame();
        
        [DllImport("cimgui", EntryPoint = "igResetMouseDragDelta", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igResetMouseDragDelta(int button);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igResetMouseDragDelta(int button) => INTERNAL_igResetMouseDragDelta(button);
        
        [DllImport("cimgui", EntryPoint = "igGetTreeNodeToLabelSpacing", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetTreeNodeToLabelSpacing();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetTreeNodeToLabelSpacing() => INTERNAL_igGetTreeNodeToLabelSpacing();
        
        [DllImport("cimgui", EntryPoint = "igGetMousePos", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetMousePos();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetMousePos() => INTERNAL_igGetMousePos();
        
        [DllImport("cimgui", EntryPoint = "GlyphRangesBuilder_AddChar", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_GlyphRangesBuilder_AddChar(GlyphRangesBuilder* self, ushort c);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_AddChar(GlyphRangesBuilder* self, ushort c) => INTERNAL_GlyphRangesBuilder_AddChar(self, c);
        
        [DllImport("cimgui", EntryPoint = "igPopID", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPopID();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopID() => INTERNAL_igPopID();
        
        [DllImport("cimgui", EntryPoint = "igIsMouseDoubleClicked", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsMouseDoubleClicked(int button);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseDoubleClicked(int button) => INTERNAL_igIsMouseDoubleClicked(button);
        
        [DllImport("cimgui", EntryPoint = "igStyleColorsClassic", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igStyleColorsClassic(ImGuiStyle* dst);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igStyleColorsClassic(ImGuiStyle* dst) => INTERNAL_igStyleColorsClassic(dst);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextFilter_IsActive", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImGuiTextFilter_IsActive(ImGuiTextFilter* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiTextFilter_IsActive(ImGuiTextFilter* self) => INTERNAL_ImGuiTextFilter_IsActive(self);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PathClear", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PathClear(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathClear(ImDrawList* self) => INTERNAL_ImDrawList_PathClear(self);
        
        [DllImport("cimgui", EntryPoint = "igSetWindowFocus", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetWindowFocus();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowFocus() => INTERNAL_igSetWindowFocus();
        
        [DllImport("cimgui", EntryPoint = "igSetWindowFocusStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetWindowFocusStr(byte* name);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowFocusStr(byte* name) => INTERNAL_igSetWindowFocusStr(name);
        
        [DllImport("cimgui", EntryPoint = "igColorConvertHSVtoRGB", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igColorConvertHSVtoRGB(float h, float s, float v, float* out_r, float* out_g, float* out_b);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igColorConvertHSVtoRGB(float h, float s, float v, float* out_r, float* out_g, float* out_b) => INTERNAL_igColorConvertHSVtoRGB(h, s, v, out_r, out_g, out_b);
        
        [DllImport("cimgui", EntryPoint = "ImColor_ImColor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImColor_ImColor(ImColor* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_ImColor(ImColor* self) => INTERNAL_ImColor_ImColor(self);
        
        [DllImport("cimgui", EntryPoint = "ImColor_ImColorInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImColor_ImColorInt(ImColor* self, int r, int g, int b, int a);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_ImColorInt(ImColor* self, int r, int g, int b, int a) => INTERNAL_ImColor_ImColorInt(self, r, g, b, a);
        
        [DllImport("cimgui", EntryPoint = "ImColor_ImColorU32", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImColor_ImColorU32(ImColor* self, uint rgba);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_ImColorU32(ImColor* self, uint rgba) => INTERNAL_ImColor_ImColorU32(self, rgba);
        
        [DllImport("cimgui", EntryPoint = "ImColor_ImColorFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImColor_ImColorFloat(ImColor* self, float r, float g, float b, float a);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_ImColorFloat(ImColor* self, float r, float g, float b, float a) => INTERNAL_ImColor_ImColorFloat(self, r, g, b, a);
        
        [DllImport("cimgui", EntryPoint = "ImColor_ImColorVec4", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImColor_ImColorVec4(ImColor* self, Vector4 col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_ImColorVec4(ImColor* self, Vector4 col) => INTERNAL_ImColor_ImColorVec4(self, col);
        
        [DllImport("cimgui", EntryPoint = "igVSliderFloat", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igVSliderFloat(byte* label, Vector2 size, float* v, float v_min, float v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igVSliderFloat(byte* label, Vector2 size, float* v, float v_min, float v_max, byte* format, float power) => INTERNAL_igVSliderFloat(label, size, v, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "igColorConvertU32ToFloat4", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector4 INTERNAL_igColorConvertU32ToFloat4(uint @in);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector4 igColorConvertU32ToFloat4(uint @in) => INTERNAL_igColorConvertU32ToFloat4(@in);
        
        [DllImport("cimgui", EntryPoint = "igPopTextWrapPos", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPopTextWrapPos();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopTextWrapPos() => INTERNAL_igPopTextWrapPos();
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextFilter_Clear", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiTextFilter_Clear(ImGuiTextFilter* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextFilter_Clear(ImGuiTextFilter* self) => INTERNAL_ImGuiTextFilter_Clear(self);
        
        [DllImport("cimgui", EntryPoint = "igGetStateStorage", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImGuiStorage* INTERNAL_igGetStateStorage();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImGuiStorage* igGetStateStorage() => INTERNAL_igGetStateStorage();
        
        [DllImport("cimgui", EntryPoint = "igGetColumnWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetColumnWidth(int column_index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetColumnWidth(int column_index) => INTERNAL_igGetColumnWidth(column_index);
        
        [DllImport("cimgui", EntryPoint = "igEndMenuBar", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndMenuBar();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndMenuBar() => INTERNAL_igEndMenuBar();
        
        [DllImport("cimgui", EntryPoint = "igSetStateStorage", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetStateStorage(ImGuiStorage* storage);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetStateStorage(ImGuiStorage* storage) => INTERNAL_igSetStateStorage(storage);
        
        [DllImport("cimgui", EntryPoint = "igGetStyleColorName", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_igGetStyleColorName(ImGuiCol idx);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* igGetStyleColorName(ImGuiCol idx) => INTERNAL_igGetStyleColorName(idx);
        
        [DllImport("cimgui", EntryPoint = "igIsMouseDragging", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsMouseDragging(int button, float lock_threshold);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseDragging(int button, float lock_threshold) => INTERNAL_igIsMouseDragging(button, lock_threshold);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PrimWriteIdx", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PrimWriteIdx(ImDrawList* self, ushort idx);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimWriteIdx(ImDrawList* self, ushort idx) => INTERNAL_ImDrawList_PrimWriteIdx(self, idx);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStyle_ScaleAllSizes", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiStyle_ScaleAllSizes(ImGuiStyle* self, float scale_factor);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStyle_ScaleAllSizes(ImGuiStyle* self, float scale_factor) => INTERNAL_ImGuiStyle_ScaleAllSizes(self, scale_factor);
        
        [DllImport("cimgui", EntryPoint = "igPushStyleColorU32", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushStyleColorU32(ImGuiCol idx, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushStyleColorU32(ImGuiCol idx, uint col) => INTERNAL_igPushStyleColorU32(idx, col);
        
        [DllImport("cimgui", EntryPoint = "igPushStyleColor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushStyleColor(ImGuiCol idx, Vector4 col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushStyleColor(ImGuiCol idx, Vector4 col) => INTERNAL_igPushStyleColor(idx, col);
        
        [DllImport("cimgui", EntryPoint = "igMemAlloc", CallingConvention = CallingConvention.Cdecl)]
        private static extern void* INTERNAL_igMemAlloc(uint size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void* igMemAlloc(uint size) => INTERNAL_igMemAlloc(size);
        
        [DllImport("cimgui", EntryPoint = "igSetCurrentContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetCurrentContext(IntPtr ctx);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetCurrentContext(IntPtr ctx) => INTERNAL_igSetCurrentContext(ctx);
        
        [DllImport("cimgui", EntryPoint = "igPushItemWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushItemWidth(float item_width);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushItemWidth(float item_width) => INTERNAL_igPushItemWidth(item_width);
        
        [DllImport("cimgui", EntryPoint = "igIsWindowAppearing", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsWindowAppearing();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsWindowAppearing() => INTERNAL_igIsWindowAppearing();
        
        [DllImport("cimgui", EntryPoint = "igGetStyle", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImGuiStyle* INTERNAL_igGetStyle();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImGuiStyle* igGetStyle() => INTERNAL_igGetStyle();
        
        [DllImport("cimgui", EntryPoint = "igSetItemAllowOverlap", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetItemAllowOverlap();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetItemAllowOverlap() => INTERNAL_igSetItemAllowOverlap();
        
        [DllImport("cimgui", EntryPoint = "igEndChild", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndChild();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndChild() => INTERNAL_igEndChild();
        
        [DllImport("cimgui", EntryPoint = "igCollapsingHeader", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igCollapsingHeader(byte* label, ImGuiTreeNodeFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igCollapsingHeader(byte* label, ImGuiTreeNodeFlags flags) => INTERNAL_igCollapsingHeader(label, flags);
        
        [DllImport("cimgui", EntryPoint = "igCollapsingHeaderBoolPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igCollapsingHeaderBoolPtr(byte* label, byte* p_open, ImGuiTreeNodeFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igCollapsingHeaderBoolPtr(byte* label, byte* p_open, ImGuiTreeNodeFlags flags) => INTERNAL_igCollapsingHeaderBoolPtr(label, p_open, flags);
        
        [DllImport("cimgui", EntryPoint = "igDragFloatRange2", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragFloatRange2(byte* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, byte* format, byte* format_max, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragFloatRange2(byte* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, byte* format, byte* format_max, float power) => INTERNAL_igDragFloatRange2(label, v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max, power);
        
        [DllImport("cimgui", EntryPoint = "igSetMouseCursor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetMouseCursor(ImGuiMouseCursor type);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetMouseCursor(ImGuiMouseCursor type) => INTERNAL_igSetMouseCursor(type);
        
        [DllImport("cimgui", EntryPoint = "igGetWindowContentRegionMax", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetWindowContentRegionMax();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetWindowContentRegionMax() => INTERNAL_igGetWindowContentRegionMax();
        
        [DllImport("cimgui", EntryPoint = "igInputScalar", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputScalar(byte* label, ImGuiDataType data_type, void* v, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputScalar(byte* label, ImGuiDataType data_type, void* v, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputScalar(label, data_type, v, step, step_fast, format, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PushClipRectFullScreen", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PushClipRectFullScreen(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PushClipRectFullScreen(ImDrawList* self) => INTERNAL_ImDrawList_PushClipRectFullScreen(self);
        
        [DllImport("cimgui", EntryPoint = "igGetColorU32", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint INTERNAL_igGetColorU32(ImGuiCol idx, float alpha_mul);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetColorU32(ImGuiCol idx, float alpha_mul) => INTERNAL_igGetColorU32(idx, alpha_mul);
        
        [DllImport("cimgui", EntryPoint = "igGetColorU32Vec4", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint INTERNAL_igGetColorU32Vec4(Vector4 col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetColorU32Vec4(Vector4 col) => INTERNAL_igGetColorU32Vec4(col);
        
        [DllImport("cimgui", EntryPoint = "igGetColorU32U32", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint INTERNAL_igGetColorU32U32(uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetColorU32U32(uint col) => INTERNAL_igGetColorU32U32(col);
        
        [DllImport("cimgui", EntryPoint = "igGetTime", CallingConvention = CallingConvention.Cdecl)]
        private static extern double INTERNAL_igGetTime();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed double igGetTime() => INTERNAL_igGetTime();
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_ChannelsMerge", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_ChannelsMerge(ImDrawList* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_ChannelsMerge(ImDrawList* self) => INTERNAL_ImDrawList_ChannelsMerge(self);
        
        [DllImport("cimgui", EntryPoint = "igGetColumnIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_igGetColumnIndex();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int igGetColumnIndex() => INTERNAL_igGetColumnIndex();
        
        [DllImport("cimgui", EntryPoint = "igBeginPopupContextItem", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginPopupContextItem(byte* str_id, int mouse_button);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginPopupContextItem(byte* str_id, int mouse_button) => INTERNAL_igBeginPopupContextItem(str_id, mouse_button);
        
        [DllImport("cimgui", EntryPoint = "igSetCursorPosX", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetCursorPosX(float x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetCursorPosX(float x) => INTERNAL_igSetCursorPosX(x);
        
        [DllImport("cimgui", EntryPoint = "igGetItemRectSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetItemRectSize();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetItemRectSize() => INTERNAL_igGetItemRectSize();
        
        [DllImport("cimgui", EntryPoint = "igArrowButton", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igArrowButton(byte* str_id, ImGuiDir dir);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igArrowButton(byte* str_id, ImGuiDir dir) => INTERNAL_igArrowButton(str_id, dir);
        
        [DllImport("cimgui", EntryPoint = "igGetMouseCursor", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImGuiMouseCursor INTERNAL_igGetMouseCursor();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImGuiMouseCursor igGetMouseCursor() => INTERNAL_igGetMouseCursor();
        
        [DllImport("cimgui", EntryPoint = "igPushAllowKeyboardFocus", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushAllowKeyboardFocus(byte allow_keyboard_focus);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushAllowKeyboardFocus(byte allow_keyboard_focus) => INTERNAL_igPushAllowKeyboardFocus(allow_keyboard_focus);
        
        [DllImport("cimgui", EntryPoint = "igGetScrollY", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetScrollY();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetScrollY() => INTERNAL_igGetScrollY();
        
        [DllImport("cimgui", EntryPoint = "igSetColumnOffset", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetColumnOffset(int column_index, float offset_x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetColumnOffset(int column_index, float offset_x) => INTERNAL_igSetColumnOffset(column_index, offset_x);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextBuffer_begin", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_ImGuiTextBuffer_begin(ImGuiTextBuffer* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImGuiTextBuffer_begin(ImGuiTextBuffer* self) => INTERNAL_ImGuiTextBuffer_begin(self);
        
        [DllImport("cimgui", EntryPoint = "igSetWindowPosVec2", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetWindowPosVec2(Vector2 pos, ImGuiCond cond);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowPosVec2(Vector2 pos, ImGuiCond cond) => INTERNAL_igSetWindowPosVec2(pos, cond);
        
        [DllImport("cimgui", EntryPoint = "igSetWindowPosStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetWindowPosStr(byte* name, Vector2 pos, ImGuiCond cond);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowPosStr(byte* name, Vector2 pos, ImGuiCond cond) => INTERNAL_igSetWindowPosStr(name, pos, cond);
        
        [DllImport("cimgui", EntryPoint = "igSetKeyboardFocusHere", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetKeyboardFocusHere(int offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetKeyboardFocusHere(int offset) => INTERNAL_igSetKeyboardFocusHere(offset);
        
        [DllImport("cimgui", EntryPoint = "igGetCursorPosY", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetCursorPosY();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetCursorPosY() => INTERNAL_igGetCursorPosY();
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_AddCustomRectFontGlyph", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_ImFontAtlas_AddCustomRectFontGlyph(ImFontAtlas* self, ImFont* font, ushort id, int width, int height, float advance_x, Vector2 offset);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int ImFontAtlas_AddCustomRectFontGlyph(ImFontAtlas* self, ImFont* font, ushort id, int width, int height, float advance_x, Vector2 offset) => INTERNAL_ImFontAtlas_AddCustomRectFontGlyph(self, font, id, width, height, advance_x, offset);
        
        [DllImport("cimgui", EntryPoint = "igEndMainMenuBar", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEndMainMenuBar();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndMainMenuBar() => INTERNAL_igEndMainMenuBar();
        
        [DllImport("cimgui", EntryPoint = "igGetContentRegionAvailWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetContentRegionAvailWidth();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetContentRegionAvailWidth() => INTERNAL_igGetContentRegionAvailWidth();
        
        [DllImport("cimgui", EntryPoint = "igIsKeyDown", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsKeyDown(int user_key_index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsKeyDown(int user_key_index) => INTERNAL_igIsKeyDown(user_key_index);
        
        [DllImport("cimgui", EntryPoint = "igIsMouseDown", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsMouseDown(int button);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseDown(int button) => INTERNAL_igIsMouseDown(button);
        
        [DllImport("cimgui", EntryPoint = "igGetWindowContentRegionMin", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetWindowContentRegionMin();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetWindowContentRegionMin() => INTERNAL_igGetWindowContentRegionMin();
        
        [DllImport("cimgui", EntryPoint = "igLogButtons", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igLogButtons();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogButtons() => INTERNAL_igLogButtons();
        
        [DllImport("cimgui", EntryPoint = "igGetWindowContentRegionWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetWindowContentRegionWidth();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetWindowContentRegionWidth() => INTERNAL_igGetWindowContentRegionWidth();
        
        [DllImport("cimgui", EntryPoint = "igSliderAngle", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderAngle(byte* label, float* v_rad, float v_degrees_min, float v_degrees_max);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderAngle(byte* label, float* v_rad, float v_degrees_min, float v_degrees_max) => INTERNAL_igSliderAngle(label, v_rad, v_degrees_min, v_degrees_max);
        
        [DllImport("cimgui", EntryPoint = "igTreeNodeExStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igTreeNodeExStr(byte* label, ImGuiTreeNodeFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodeExStr(byte* label, ImGuiTreeNodeFlags flags) => INTERNAL_igTreeNodeExStr(label, flags);
        
        [DllImport("cimgui", EntryPoint = "igTreeNodeExStrStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igTreeNodeExStrStr(byte* str_id, ImGuiTreeNodeFlags flags, byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodeExStrStr(byte* str_id, ImGuiTreeNodeFlags flags, byte* fmt) => INTERNAL_igTreeNodeExStrStr(str_id, flags, fmt);
        
        [DllImport("cimgui", EntryPoint = "igTreeNodeExPtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igTreeNodeExPtr(void* ptr_id, ImGuiTreeNodeFlags flags, byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodeExPtr(void* ptr_id, ImGuiTreeNodeFlags flags, byte* fmt) => INTERNAL_igTreeNodeExPtr(ptr_id, flags, fmt);
        
        [DllImport("cimgui", EntryPoint = "igGetWindowWidth", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetWindowWidth();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetWindowWidth() => INTERNAL_igGetWindowWidth();
        
        [DllImport("cimgui", EntryPoint = "igPushTextWrapPos", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushTextWrapPos(float wrap_pos_x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushTextWrapPos(float wrap_pos_x) => INTERNAL_igPushTextWrapPos(wrap_pos_x);
        
        [DllImport("cimgui", EntryPoint = "ImGuiStorage_GetInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern int INTERNAL_ImGuiStorage_GetInt(ImGuiStorage* self, uint key, int default_val);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int ImGuiStorage_GetInt(ImGuiStorage* self, uint key, int default_val) => INTERNAL_ImGuiStorage_GetInt(self, key, default_val);
        
        [DllImport("cimgui", EntryPoint = "igSliderInt3", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderInt3(byte* label, int* v, int v_min, int v_max, byte* format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderInt3(byte* label, int* v, int v_min, int v_max, byte* format) => INTERNAL_igSliderInt3(label, v, v_min, v_max, format);
        
        [DllImport("cimgui", EntryPoint = "igShowUserGuide", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igShowUserGuide();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igShowUserGuide() => INTERNAL_igShowUserGuide();
        
        [DllImport("cimgui", EntryPoint = "igSliderScalarN", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igSliderScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* v_min, void* v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* v_min, void* v_max, byte* format, float power) => INTERNAL_igSliderScalarN(label, data_type, v, components, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "ImColor_HSV", CallingConvention = CallingConvention.Cdecl)]
        private static extern ImColor INTERNAL_ImColor_HSV(ImColor* self, float h, float s, float v, float a);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImColor ImColor_HSV(ImColor* self, float h, float s, float v, float a) => INTERNAL_ImColor_HSV(self, h, s, v, a);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PathLineTo", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PathLineTo(ImDrawList* self, Vector2 pos);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathLineTo(ImDrawList* self, Vector2 pos) => INTERNAL_ImDrawList_PathLineTo(self, pos);
        
        [DllImport("cimgui", EntryPoint = "igImage", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igImage(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igImage(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col) => INTERNAL_igImage(user_texture_id, size, uv0, uv1, tint_col, border_col);
        
        [DllImport("cimgui", EntryPoint = "igSetNextWindowSizeConstraints", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback, void* custom_callback_data);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback, void* custom_callback_data) => INTERNAL_igSetNextWindowSizeConstraints(size_min, size_max, custom_callback, custom_callback_data);
        
        [DllImport("cimgui", EntryPoint = "igDummy", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igDummy(Vector2 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igDummy(Vector2 size) => INTERNAL_igDummy(size);
        
        [DllImport("cimgui", EntryPoint = "igVSliderInt", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igVSliderInt(byte* label, Vector2 size, int* v, int v_min, int v_max, byte* format);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igVSliderInt(byte* label, Vector2 size, int* v, int v_min, int v_max, byte* format) => INTERNAL_igVSliderInt(label, size, v, v_min, v_max, format);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextBuffer_ImGuiTextBuffer", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImGuiTextBuffer_ImGuiTextBuffer(ImGuiTextBuffer* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextBuffer_ImGuiTextBuffer(ImGuiTextBuffer* self) => INTERNAL_ImGuiTextBuffer_ImGuiTextBuffer(self);
        
        [DllImport("cimgui", EntryPoint = "igBulletText", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igBulletText(byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igBulletText(byte* fmt) => INTERNAL_igBulletText(fmt);
        
        [DllImport("cimgui", EntryPoint = "igColorEdit4", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igColorEdit4(byte* label, Vector4* col, ImGuiColorEditFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igColorEdit4(byte* label, Vector4* col, ImGuiColorEditFlags flags) => INTERNAL_igColorEdit4(label, col, flags);
        
        [DllImport("cimgui", EntryPoint = "igColorPicker4", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igColorPicker4(byte* label, Vector4* col, ImGuiColorEditFlags flags, float* ref_col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igColorPicker4(byte* label, Vector4* col, ImGuiColorEditFlags flags, float* ref_col) => INTERNAL_igColorPicker4(label, col, flags, ref_col);
        
        [DllImport("cimgui", EntryPoint = "ImDrawList_PrimRectUV", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawList_PrimRectUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimRectUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col) => INTERNAL_ImDrawList_PrimRectUV(self, a, b, uv_a, uv_b, col);
        
        [DllImport("cimgui", EntryPoint = "igInvisibleButton", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInvisibleButton(byte* str_id, Vector2 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInvisibleButton(byte* str_id, Vector2 size) => INTERNAL_igInvisibleButton(str_id, size);
        
        [DllImport("cimgui", EntryPoint = "igLogToClipboard", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igLogToClipboard(int max_depth);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogToClipboard(int max_depth) => INTERNAL_igLogToClipboard(max_depth);
        
        [DllImport("cimgui", EntryPoint = "igBeginPopupContextWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igBeginPopupContextWindow(byte* str_id, int mouse_button, byte also_over_items);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginPopupContextWindow(byte* str_id, int mouse_button, byte also_over_items) => INTERNAL_igBeginPopupContextWindow(str_id, mouse_button, also_over_items);
        
        [DllImport("cimgui", EntryPoint = "ImFontAtlas_ImFontAtlas", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImFontAtlas_ImFontAtlas(ImFontAtlas* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_ImFontAtlas(ImFontAtlas* self) => INTERNAL_ImFontAtlas_ImFontAtlas(self);
        
        [DllImport("cimgui", EntryPoint = "igDragScalar", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragScalar(byte* label, ImGuiDataType data_type, void* v, float v_speed, void* v_min, void* v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragScalar(byte* label, ImGuiDataType data_type, void* v, float v_speed, void* v_min, void* v_max, byte* format, float power) => INTERNAL_igDragScalar(label, data_type, v, v_speed, v_min, v_max, format, power);
        
        [DllImport("cimgui", EntryPoint = "igSetItemDefaultFocus", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetItemDefaultFocus();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetItemDefaultFocus() => INTERNAL_igSetItemDefaultFocus();
        
        [DllImport("cimgui", EntryPoint = "igCaptureMouseFromApp", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igCaptureMouseFromApp(byte capture);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igCaptureMouseFromApp(byte capture) => INTERNAL_igCaptureMouseFromApp(capture);
        
        [DllImport("cimgui", EntryPoint = "igIsAnyItemHovered", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igIsAnyItemHovered();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsAnyItemHovered() => INTERNAL_igIsAnyItemHovered();
        
        [DllImport("cimgui", EntryPoint = "igPushFont", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPushFont(ImFont* font);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushFont(ImFont* font) => INTERNAL_igPushFont(font);
        
        [DllImport("cimgui", EntryPoint = "igInputInt2", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputInt2(byte* label, int* v, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputInt2(byte* label, int* v, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputInt2(label, v, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "igTreePop", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igTreePop();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTreePop() => INTERNAL_igTreePop();
        
        [DllImport("cimgui", EntryPoint = "igEnd", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igEnd();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEnd() => INTERNAL_igEnd();
        
        [DllImport("cimgui", EntryPoint = "ImDrawData_ImDrawData", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_ImDrawData_ImDrawData(ImDrawData* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawData_ImDrawData(ImDrawData* self) => INTERNAL_ImDrawData_ImDrawData(self);
        
        [DllImport("cimgui", EntryPoint = "igDestroyContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igDestroyContext(IntPtr ctx);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igDestroyContext(IntPtr ctx) => INTERNAL_igDestroyContext(ctx);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextBuffer_end", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte* INTERNAL_ImGuiTextBuffer_end(ImGuiTextBuffer* self);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImGuiTextBuffer_end(ImGuiTextBuffer* self) => INTERNAL_ImGuiTextBuffer_end(self);
        
        [DllImport("cimgui", EntryPoint = "igPopStyleVar", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igPopStyleVar(int count);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopStyleVar(int count) => INTERNAL_igPopStyleVar(count);
        
        [DllImport("cimgui", EntryPoint = "ImGuiTextFilter_PassFilter", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_ImGuiTextFilter_PassFilter(ImGuiTextFilter* self, byte* text, byte* text_end);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiTextFilter_PassFilter(ImGuiTextFilter* self, byte* text, byte* text_end) => INTERNAL_ImGuiTextFilter_PassFilter(self, text, text_end);
        
        [DllImport("cimgui", EntryPoint = "igShowStyleSelector", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igShowStyleSelector(byte* label);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igShowStyleSelector(byte* label) => INTERNAL_igShowStyleSelector(label);
        
        [DllImport("cimgui", EntryPoint = "igInputScalarN", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputScalarN(label, data_type, v, components, step, step_fast, format, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "igTreeNodeStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igTreeNodeStr(byte* label);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodeStr(byte* label) => INTERNAL_igTreeNodeStr(label);
        
        [DllImport("cimgui", EntryPoint = "igTreeNodeStrStr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igTreeNodeStrStr(byte* str_id, byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodeStrStr(byte* str_id, byte* fmt) => INTERNAL_igTreeNodeStrStr(str_id, fmt);
        
        [DllImport("cimgui", EntryPoint = "igTreeNodePtr", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igTreeNodePtr(void* ptr_id, byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodePtr(void* ptr_id, byte* fmt) => INTERNAL_igTreeNodePtr(ptr_id, fmt);
        
        [DllImport("cimgui", EntryPoint = "igGetScrollMaxX", CallingConvention = CallingConvention.Cdecl)]
        private static extern float INTERNAL_igGetScrollMaxX();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetScrollMaxX() => INTERNAL_igGetScrollMaxX();
        
        [DllImport("cimgui", EntryPoint = "igSetTooltip", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_igSetTooltip(byte* fmt);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetTooltip(byte* fmt) => INTERNAL_igSetTooltip(fmt);
        
        [DllImport("cimgui", EntryPoint = "igGetContentRegionAvail", CallingConvention = CallingConvention.Cdecl)]
        private static extern Vector2 INTERNAL_igGetContentRegionAvail();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetContentRegionAvail() => INTERNAL_igGetContentRegionAvail();
        
        [DllImport("cimgui", EntryPoint = "igInputFloat3", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igInputFloat3(byte* label, Vector3* v, byte* format, ImGuiInputTextFlags extra_flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputFloat3(byte* label, Vector3* v, byte* format, ImGuiInputTextFlags extra_flags) => INTERNAL_igInputFloat3(label, v, format, extra_flags);
        
        [DllImport("cimgui", EntryPoint = "igDragFloat2", CallingConvention = CallingConvention.Cdecl)]
        private static extern byte INTERNAL_igDragFloat2(byte* label, Vector2* v, float v_speed, float v_min, float v_max, byte* format, float power);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragFloat2(byte* label, Vector2* v, float v_speed, float v_min, float v_max, byte* format, float power) => INTERNAL_igDragFloat2(label, v, v_speed, v_min, v_max, format, power);
    }
#pragma warning restore 1591
}
