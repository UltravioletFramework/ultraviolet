using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core;

namespace Ultraviolet.ImGuiViewProvider.Bindings
{
#pragma warning disable 1591
    public static unsafe partial class ImGuiNative
    {
        private static readonly ImGuiNativeImpl impl;
        
        static ImGuiNative()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Android:
                    impl = new ImGuiNativeImpl_Android();
                    break;
                    
                default:
                    impl = new ImGuiNativeImpl_Default();
                    break;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetFrameHeight() => impl.igGetFrameHeight();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr igCreateContext(ImFontAtlas* shared_font_atlas) => impl.igCreateContext(shared_font_atlas);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igTextUnformatted(byte* text, byte* text_end) => impl.igTextUnformatted(text, text_end);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPopFont() => impl.igPopFont();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igCombo(byte* label, int* current_item, byte** items, int items_count, int popup_max_height_in_items) => impl.igCombo(label, current_item, items, items_count, popup_max_height_in_items);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igComboStr(byte* label, int* current_item, byte* items_separated_by_zeros, int popup_max_height_in_items) => impl.igComboStr(label, current_item, items_separated_by_zeros, popup_max_height_in_items);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igCaptureKeyboardFromApp(byte capture) => impl.igCaptureKeyboardFromApp(capture);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsWindowFocused(ImGuiFocusedFlags flags) => impl.igIsWindowFocused(flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igRender() => impl.igRender();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_ChannelsSetCurrent(ImDrawList* self, int channel_index) => impl.ImDrawList_ChannelsSetCurrent(self, channel_index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragFloat4(byte* label, Vector4* v, float v_speed, float v_min, float v_max, byte* format, float power) => impl.igDragFloat4(label, v, v_speed, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_ChannelsSplit(ImDrawList* self, int channels_count) => impl.ImDrawList_ChannelsSplit(self, channels_count);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsMousePosValid(Vector2* mouse_pos) => impl.igIsMousePosValid(mouse_pos);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetCursorScreenPos() => impl.igGetCursorScreenPos();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDebugCheckVersionAndDataLayout(byte* version_str, uint sz_io, uint sz_style, uint sz_vec2, uint sz_vec4, uint sz_drawvert) => impl.igDebugCheckVersionAndDataLayout(version_str, sz_io, sz_style, sz_vec2, sz_vec4, sz_drawvert);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetScrollHere(float center_y_ratio) => impl.igSetScrollHere(center_y_ratio);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetScrollY(float scroll_y) => impl.igSetScrollY(scroll_y);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetColorEditOptions(ImGuiColorEditFlags flags) => impl.igSetColorEditOptions(flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetScrollFromPosY(float pos_y, float center_y_ratio) => impl.igSetScrollFromPosY(pos_y, center_y_ratio);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4* igGetStyleColorVec4(ImGuiCol idx) => impl.igGetStyleColorVec4(idx);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsMouseHoveringRect(Vector2 r_min, Vector2 r_max, byte clip) => impl.igIsMouseHoveringRect(r_min, r_max, clip);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImVec4_ImVec4(Vector4* self) => impl.ImVec4_ImVec4(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImVec4_ImVec4Float(Vector4* self, float _x, float _y, float _z, float _w) => impl.ImVec4_ImVec4Float(self, _x, _y, _z, _w);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImColor_SetHSV(ImColor* self, float h, float s, float v, float a) => impl.ImColor_SetHSV(self, h, s, v, a);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragFloat3(byte* label, Vector3* v, float v_speed, float v_min, float v_max, byte* format, float power) => impl.igDragFloat3(label, v, v_speed, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddPolyline(ImDrawList* self, Vector2* points, int num_points, uint col, byte closed, float thickness) => impl.ImDrawList_AddPolyline(self, points, num_points, col, closed, thickness);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igValueBool(byte* prefix, byte b) => impl.igValueBool(prefix, b);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igValueInt(byte* prefix, int v) => impl.igValueInt(prefix, v);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igValueUint(byte* prefix, uint v) => impl.igValueUint(prefix, v);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igValueFloat(byte* prefix, float v, byte* float_format) => impl.igValueFloat(prefix, v, float_format);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiTextFilter_Build(ImGuiTextFilter* self) => impl.ImGuiTextFilter_Build(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetItemRectMax() => impl.igGetItemRectMax();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsItemDeactivated() => impl.igIsItemDeactivated();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushStyleVarFloat(ImGuiStyleVar idx, float val) => impl.igPushStyleVarFloat(idx, val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushStyleVarVec2(ImGuiStyleVar idx, Vector2 val) => impl.igPushStyleVarVec2(idx, val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* igSaveIniSettingsToMemory(uint* out_ini_size) => impl.igSaveIniSettingsToMemory(out_ini_size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragIntRange2(byte* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, byte* format, byte* format_max) => impl.igDragIntRange2(label, v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igUnindent(float indent_w) => impl.igUnindent(indent_w);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImFont* ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(ImFontAtlas* self, byte* compressed_font_data_base85, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => impl.ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(self, compressed_font_data_base85, size_pixels, font_cfg, glyph_ranges);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPopAllowKeyboardFocus() => impl.igPopAllowKeyboardFocus();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igLoadIniSettingsFromDisk(byte* ini_filename) => impl.igLoadIniSettingsFromDisk(ini_filename);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetCursorStartPos() => impl.igGetCursorStartPos();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetCursorScreenPos(Vector2 screen_pos) => impl.igSetCursorScreenPos(screen_pos);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputInt4(byte* label, int* v, ImGuiInputTextFlags extra_flags) => impl.igInputInt4(label, v, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFont_AddRemapChar(ImFont* self, ushort dst, ushort src, byte overwrite_dst) => impl.ImFont_AddRemapChar(self, dst, src, overwrite_dst);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFont_AddGlyph(ImFont* self, ushort c, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1, float advance_x) => impl.ImFont_AddGlyph(self, c, x0, y0, x1, y1, u0, v0, u1, v1, advance_x);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsRectVisible(Vector2 size) => impl.igIsRectVisible(size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsRectVisibleVec2(Vector2 rect_min, Vector2 rect_max) => impl.igIsRectVisibleVec2(rect_min, rect_max);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFont_GrowIndex(ImFont* self, int new_size) => impl.ImFont_GrowIndex(self, new_size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImFontAtlas_Build(ImFontAtlas* self) => impl.ImFontAtlas_Build(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igLabelText(byte* label, byte* fmt) => impl.igLabelText(label, fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFont_RenderText(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, byte* text_begin, byte* text_end, float wrap_width, byte cpu_fine_clip) => impl.ImFont_RenderText(self, draw_list, size, pos, col, clip_rect, text_begin, text_end, wrap_width, cpu_fine_clip);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igLogFinish() => impl.igLogFinish();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsKeyPressed(int user_key_index, byte repeat) => impl.igIsKeyPressed(user_key_index, repeat);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetColumnOffset(int column_index) => impl.igGetColumnOffset(column_index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PopClipRect(ImDrawList* self) => impl.ImDrawList_PopClipRect(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImFontGlyph* ImFont_FindGlyphNoFallback(ImFont* self, ushort c) => impl.ImFont_FindGlyphNoFallback(self, c);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetNextWindowCollapsed(byte collapsed, ImGuiCond cond) => impl.igSetNextWindowCollapsed(collapsed, cond);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr igGetCurrentContext() => impl.igGetCurrentContext();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSmallButton(byte* label) => impl.igSmallButton(label);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igOpenPopupOnItemClick(byte* str_id, int mouse_button) => impl.igOpenPopupOnItemClick(str_id, mouse_button);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsAnyMouseDown() => impl.igIsAnyMouseDown();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* ImFont_CalcWordWrapPositionA(ImFont* self, float scale, byte* text, byte* text_end, float wrap_width) => impl.ImFont_CalcWordWrapPositionA(self, scale, text, text_end, wrap_width);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ImFont_CalcTextSizeA(ImFont* self, float size, float max_width, float wrap_width, byte* text_begin, byte* text_end, byte** remaining) => impl.ImFont_CalcTextSizeA(self, size, max_width, wrap_width, text_begin, text_end, remaining);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GlyphRangesBuilder_SetBit(GlyphRangesBuilder* self, int n) => impl.GlyphRangesBuilder_SetBit(self, n);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImFont_IsLoaded(ImFont* self) => impl.ImFont_IsLoaded(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ImFont_GetCharAdvance(ImFont* self, ushort c) => impl.ImFont_GetCharAdvance(self, c);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igImageButton(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, int frame_padding, Vector4 bg_col, Vector4 tint_col) => impl.igImageButton(user_texture_id, size, uv0, uv1, frame_padding, bg_col, tint_col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFont_SetFallbackChar(ImFont* self, ushort c) => impl.ImFont_SetFallbackChar(self, c);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndFrame() => impl.igEndFrame();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderFloat2(byte* label, Vector2* v, float v_min, float v_max, byte* format, float power) => impl.igSliderFloat2(label, v, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFont_RenderChar(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, ushort c) => impl.ImFont_RenderChar(self, draw_list, size, pos, col, c);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igRadioButtonBool(byte* label, byte active) => impl.igRadioButtonBool(label, active);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igRadioButtonIntPtr(byte* label, int* v, int v_button) => impl.igRadioButtonIntPtr(label, v, v_button);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PushClipRect(ImDrawList* self, Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect) => impl.ImDrawList_PushClipRect(self, clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImFontGlyph* ImFont_FindGlyph(ImFont* self, ushort c) => impl.ImFont_FindGlyph(self, c);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsItemDeactivatedAfterEdit() => impl.igIsItemDeactivatedAfterEdit();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImDrawList* igGetWindowDrawList() => impl.igGetWindowDrawList();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImFont* ImFontAtlas_AddFont(ImFontAtlas* self, ImFontConfig* font_cfg) => impl.ImFontAtlas_AddFont(self, font_cfg);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PathBezierCurveTo(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, int num_segments) => impl.ImDrawList_PathBezierCurveTo(self, p1, p2, p3, num_segments);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiPayload_Clear(ImGuiPayload* self) => impl.ImGuiPayload_Clear(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igNewLine() => impl.igNewLine();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsItemFocused() => impl.igIsItemFocused();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igLoadIniSettingsFromMemory(byte* ini_data, uint ini_size) => impl.igLoadIniSettingsFromMemory(ini_data, ini_size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderInt2(byte* label, int* v, int v_min, int v_max, byte* format) => impl.igSliderInt2(label, v, v_min, v_max, format);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetWindowSizeVec2(Vector2 size, ImGuiCond cond) => impl.igSetWindowSizeVec2(size, cond);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetWindowSizeStr(byte* name, Vector2 size, ImGuiCond cond) => impl.igSetWindowSizeStr(name, size, cond);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputFloat(byte* label, float* v, float step, float step_fast, byte* format, ImGuiInputTextFlags extra_flags) => impl.igInputFloat(label, v, step, step_fast, format, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFont_ImFont(ImFont* self) => impl.ImFont_ImFont(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiStorage_SetFloat(ImGuiStorage* self, uint key, float val) => impl.ImGuiStorage_SetFloat(self, key, val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igColorConvertRGBtoHSV(float r, float g, float b, float* out_h, float* out_s, float* out_v) => impl.igColorConvertRGBtoHSV(r, g, b, out_h, out_s, out_v);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginMenuBar() => impl.igBeginMenuBar();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsPopupOpen(byte* str_id) => impl.igIsPopupOpen(str_id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsItemVisible() => impl.igIsItemVisible();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFontAtlas_CalcCustomRectUV(ImFontAtlas* self, CustomRect* rect, Vector2* out_uv_min, Vector2* out_uv_max) => impl.ImFontAtlas_CalcCustomRectUV(self, rect, out_uv_min, out_uv_max);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CustomRect* ImFontAtlas_GetCustomRectByIndex(ImFontAtlas* self, int index) => impl.ImFontAtlas_GetCustomRectByIndex(self, index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GlyphRangesBuilder_AddText(GlyphRangesBuilder* self, byte* text, byte* text_end) => impl.GlyphRangesBuilder_AddText(self, text, text_end);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_UpdateTextureID(ImDrawList* self) => impl.ImDrawList_UpdateTextureID(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetNextWindowSize(Vector2 size, ImGuiCond cond) => impl.igSetNextWindowSize(size, cond);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ImFontAtlas_AddCustomRectRegular(ImFontAtlas* self, uint id, int width, int height) => impl.ImFontAtlas_AddCustomRectRegular(self, id, width, height);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetWindowCollapsedBool(byte collapsed, ImGuiCond cond) => impl.igSetWindowCollapsedBool(collapsed, cond);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetWindowCollapsedStr(byte* name, byte collapsed, ImGuiCond cond) => impl.igSetWindowCollapsedStr(name, collapsed, cond);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetMouseDragDelta(int button, float lock_threshold) => impl.igGetMouseDragDelta(button, lock_threshold);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImGuiPayload* igAcceptDragDropPayload(byte* type, ImGuiDragDropFlags flags) => impl.igAcceptDragDropPayload(type, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginDragDropSource(ImGuiDragDropFlags flags) => impl.igBeginDragDropSource(flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte CustomRect_IsPacked(CustomRect* self) => impl.CustomRect_IsPacked(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPlotLines(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride) => impl.igPlotLines(label, values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImFontAtlas_IsBuilt(ImFontAtlas* self) => impl.ImFontAtlas_IsBuilt(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImVec2_ImVec2(Vector2* self) => impl.ImVec2_ImVec2(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImVec2_ImVec2Float(Vector2* self, float _x, float _y) => impl.ImVec2_ImVec2Float(self, _x, _y);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiPayload_ImGuiPayload(ImGuiPayload* self) => impl.ImGuiPayload_ImGuiPayload(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_Clear(ImDrawList* self) => impl.ImDrawList_Clear(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GlyphRangesBuilder_AddRanges(GlyphRangesBuilder* self, ushort* ranges) => impl.GlyphRangesBuilder_AddRanges(self, ranges);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int igGetFrameCount() => impl.igGetFrameCount();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* ImFont_GetDebugName(ImFont* self) => impl.ImFont_GetDebugName(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igListBoxFooter() => impl.igListBoxFooter();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPopClipRect() => impl.igPopClipRect();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddBezierCurve(ImDrawList* self, Vector2 pos0, Vector2 cp0, Vector2 cp1, Vector2 pos1, uint col, float thickness, int num_segments) => impl.ImDrawList_AddBezierCurve(self, pos0, cp0, cp1, pos1, col, thickness, num_segments);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GlyphRangesBuilder_GlyphRangesBuilder(GlyphRangesBuilder* self) => impl.GlyphRangesBuilder_GlyphRangesBuilder(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetWindowSize() => impl.igGetWindowSize();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort* ImFontAtlas_GetGlyphRangesThai(ImFontAtlas* self) => impl.ImFontAtlas_GetGlyphRangesThai(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igCheckboxFlags(byte* label, uint* flags, uint flags_value) => impl.igCheckboxFlags(label, flags, flags_value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort* ImFontAtlas_GetGlyphRangesCyrillic(ImFontAtlas* self) => impl.ImFontAtlas_GetGlyphRangesCyrillic(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsWindowHovered(ImGuiHoveredFlags flags) => impl.igIsWindowHovered(flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(ImFontAtlas* self) => impl.ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPlotHistogramFloatPtr(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride) => impl.igPlotHistogramFloatPtr(label, values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginPopupContextVoid(byte* str_id, int mouse_button) => impl.igBeginPopupContextVoid(str_id, mouse_button);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort* ImFontAtlas_GetGlyphRangesChineseFull(ImFontAtlas* self) => impl.ImFontAtlas_GetGlyphRangesChineseFull(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igShowStyleEditor(ImGuiStyle* @ref) => impl.igShowStyleEditor(@ref);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igCheckbox(byte* label, byte* v) => impl.igCheckbox(label, v);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetWindowPos() => impl.igGetWindowPos();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiInputTextCallbackData_ImGuiInputTextCallbackData(ImGuiInputTextCallbackData* self) => impl.ImGuiInputTextCallbackData_ImGuiInputTextCallbackData(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetNextWindowContentSize(Vector2 size) => impl.igSetNextWindowContentSize(size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igTextColored(Vector4 col, byte* fmt) => impl.igTextColored(col, fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igLogToFile(int max_depth, byte* filename) => impl.igLogToFile(max_depth, filename);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igButton(byte* label, Vector2 size) => impl.igButton(label, size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsItemEdited() => impl.igIsItemEdited();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PushTextureID(ImDrawList* self, IntPtr texture_id) => impl.ImDrawList_PushTextureID(self, texture_id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igTreeAdvanceToLabelPos() => impl.igTreeAdvanceToLabelPos();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiInputTextCallbackData_DeleteChars(ImGuiInputTextCallbackData* self, int pos, int bytes_count) => impl.ImGuiInputTextCallbackData_DeleteChars(self, pos, bytes_count);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragInt2(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => impl.igDragInt2(label, v, v_speed, v_min, v_max, format);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort* ImFontAtlas_GetGlyphRangesDefault(ImFontAtlas* self) => impl.ImFontAtlas_GetGlyphRangesDefault(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsAnyItemActive() => impl.igIsAnyItemActive();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFontAtlas_SetTexID(ImFontAtlas* self, IntPtr id) => impl.ImFontAtlas_SetTexID(self, id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igMenuItemBool(byte* label, byte* shortcut, byte selected, byte enabled) => impl.igMenuItemBool(label, shortcut, selected, enabled);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igMenuItemBoolPtr(byte* label, byte* shortcut, byte* p_selected, byte enabled) => impl.igMenuItemBoolPtr(label, shortcut, p_selected, enabled);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderFloat4(byte* label, Vector4* v, float v_min, float v_max, byte* format, float power) => impl.igSliderFloat4(label, v, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetCursorPosX() => impl.igGetCursorPosX();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFontAtlas_ClearTexData(ImFontAtlas* self) => impl.ImFontAtlas_ClearTexData(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFontAtlas_ClearFonts(ImFontAtlas* self) => impl.ImFontAtlas_ClearFonts(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int igGetColumnsCount() => impl.igGetColumnsCount();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPopButtonRepeat() => impl.igPopButtonRepeat();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragScalarN(byte* label, ImGuiDataType data_type, void* v, int components, float v_speed, void* v_min, void* v_max, byte* format, float power) => impl.igDragScalarN(label, data_type, v, components, v_speed, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImGuiPayload_IsPreview(ImGuiPayload* self) => impl.ImGuiPayload_IsPreview(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSpacing() => impl.igSpacing();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFontAtlas_Clear(ImFontAtlas* self) => impl.ImFontAtlas_Clear(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsAnyItemFocused() => impl.igIsAnyItemFocused();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddRectFilled(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags) => impl.ImDrawList_AddRectFilled(self, a, b, col, rounding, rounding_corners_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImFont* ImFontAtlas_AddFontFromMemoryCompressedTTF(ImFontAtlas* self, void* compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => impl.ImFontAtlas_AddFontFromMemoryCompressedTTF(self, compressed_font_data, compressed_font_size, size_pixels, font_cfg, glyph_ranges);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igMemFree(void* ptr) => impl.igMemFree(ptr);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetFontTexUvWhitePixel() => impl.igGetFontTexUvWhitePixel();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddDrawCmd(ImDrawList* self) => impl.ImDrawList_AddDrawCmd(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsItemClicked(int mouse_button) => impl.igIsItemClicked(mouse_button);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImFont* ImFontAtlas_AddFontFromMemoryTTF(ImFontAtlas* self, void* font_data, int font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => impl.ImFontAtlas_AddFontFromMemoryTTF(self, font_data, font_size, size_pixels, font_cfg, glyph_ranges);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImFont* ImFontAtlas_AddFontFromFileTTF(ImFontAtlas* self, byte* filename, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => impl.ImFontAtlas_AddFontFromFileTTF(self, filename, size_pixels, font_cfg, glyph_ranges);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igProgressBar(float fraction, Vector2 size_arg, byte* overlay) => impl.igProgressBar(fraction, size_arg, overlay);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImFont* ImFontAtlas_AddFontDefault(ImFontAtlas* self, ImFontConfig* font_cfg) => impl.ImFontAtlas_AddFontDefault(self, font_cfg);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetNextWindowBgAlpha(float alpha) => impl.igSetNextWindowBgAlpha(alpha);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginPopup(byte* str_id, ImGuiWindowFlags flags) => impl.igBeginPopup(str_id, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFont_BuildLookupTable(ImFont* self) => impl.ImFont_BuildLookupTable(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetScrollX() => impl.igGetScrollX();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int igGetKeyIndex(ImGuiKey imgui_key) => impl.igGetKeyIndex(imgui_key);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImDrawList* igGetOverlayDrawList() => impl.igGetOverlayDrawList();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint igGetIDStr(byte* str_id) => impl.igGetIDStr(str_id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint igGetIDRange(byte* str_id_begin, byte* str_id_end) => impl.igGetIDRange(str_id_begin, str_id_end);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint igGetIDPtr(void* ptr_id) => impl.igGetIDPtr(ptr_id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort* ImFontAtlas_GetGlyphRangesJapanese(ImFontAtlas* self) => impl.ImFontAtlas_GetGlyphRangesJapanese(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igListBoxHeaderVec2(byte* label, Vector2 size) => impl.igListBoxHeaderVec2(label, size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igListBoxHeaderInt(byte* label, int items_count, int height_in_items) => impl.igListBoxHeaderInt(label, items_count, height_in_items);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFontConfig_ImFontConfig(ImFontConfig* self) => impl.ImFontConfig_ImFontConfig(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsMouseReleased(int button) => impl.igIsMouseReleased(button);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawData_ScaleClipRects(ImDrawData* self, Vector2 sc) => impl.ImDrawData_ScaleClipRects(self, sc);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetItemRectMin() => impl.igGetItemRectMin();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawData_DeIndexAllBuffers(ImDrawData* self) => impl.ImDrawData_DeIndexAllBuffers(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igLogText(byte* fmt) => impl.igLogText(fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawData_Clear(ImDrawData* self) => impl.ImDrawData_Clear(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* ImGuiStorage_GetVoidPtr(ImGuiStorage* self, uint key) => impl.ImGuiStorage_GetVoidPtr(self, key);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igTextWrapped(byte* fmt) => impl.igTextWrapped(fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_UpdateClipRect(ImDrawList* self) => impl.ImDrawList_UpdateClipRect(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PrimVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col) => impl.ImDrawList_PrimVtx(self, pos, uv, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndGroup() => impl.igEndGroup();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImFont* igGetFont() => impl.igGetFont();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igTreePushStr(byte* str_id) => impl.igTreePushStr(str_id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igTreePushPtr(void* ptr_id) => impl.igTreePushPtr(ptr_id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igTextDisabled(byte* fmt) => impl.igTextDisabled(fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PrimRect(ImDrawList* self, Vector2 a, Vector2 b, uint col) => impl.ImDrawList_PrimRect(self, a, b, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddQuad(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col, float thickness) => impl.ImDrawList_AddQuad(self, a, b, c, d, col, thickness);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_ClearFreeMemory(ImDrawList* self) => impl.ImDrawList_ClearFreeMemory(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetNextTreeNodeOpen(byte is_open, ImGuiCond cond) => impl.igSetNextTreeNodeOpen(is_open, cond);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igLogToTTY(int max_depth) => impl.igLogToTTY(max_depth);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GlyphRangesBuilder_BuildRanges(GlyphRangesBuilder* self, ImVector* out_ranges) => impl.GlyphRangesBuilder_BuildRanges(self, out_ranges);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImDrawList* ImDrawList_CloneOutput(ImDrawList* self) => impl.ImDrawList_CloneOutput(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImGuiIO* igGetIO() => impl.igGetIO();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragInt4(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => impl.igDragInt4(label, v, v_speed, v_min, v_max, format);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igNextColumn() => impl.igNextColumn();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddRect(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags, float thickness) => impl.ImDrawList_AddRect(self, a, b, col, rounding, rounding_corners_flags, thickness);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextRange_split(TextRange* self, byte separator, ImVector* @out) => impl.TextRange_split(self, separator, @out);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetCursorPos(Vector2 local_pos) => impl.igSetCursorPos(local_pos);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginPopupModal(byte* name, byte* p_open, ImGuiWindowFlags flags) => impl.igBeginPopupModal(name, p_open, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderInt4(byte* label, int* v, int v_min, int v_max, byte* format) => impl.igSliderInt4(label, v, v_min, v_max, format);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddCallback(ImDrawList* self, IntPtr callback, void* callback_data) => impl.ImDrawList_AddCallback(self, callback, callback_data);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igShowMetricsWindow(byte* p_open) => impl.igShowMetricsWindow(p_open);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetScrollMaxY() => impl.igGetScrollMaxY();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igBeginTooltip() => impl.igBeginTooltip();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetScrollX(float scroll_x) => impl.igSetScrollX(scroll_x);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImDrawData* igGetDrawData() => impl.igGetDrawData();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetTextLineHeight() => impl.igGetTextLineHeight();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSeparator() => impl.igSeparator();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginChild(byte* str_id, Vector2 size, byte border, ImGuiWindowFlags flags) => impl.igBeginChild(str_id, size, border, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginChildID(uint id, Vector2 size, byte border, ImGuiWindowFlags flags) => impl.igBeginChildID(id, size, border, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PathRect(ImDrawList* self, Vector2 rect_min, Vector2 rect_max, float rounding, int rounding_corners_flags) => impl.ImDrawList_PathRect(self, rect_min, rect_max, rounding, rounding_corners_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsMouseClicked(int button, byte repeat) => impl.igIsMouseClicked(button, repeat);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igCalcItemWidth() => impl.igCalcItemWidth();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PathArcToFast(ImDrawList* self, Vector2 centre, float radius, int a_min_of_12, int a_max_of_12) => impl.ImDrawList_PathArcToFast(self, centre, radius, a_min_of_12, a_max_of_12);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndChildFrame() => impl.igEndChildFrame();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igIndent(float indent_w) => impl.igIndent(indent_w);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSetDragDropPayload(byte* type, void* data, uint size, ImGuiCond cond) => impl.igSetDragDropPayload(type, data, size, cond);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte GlyphRangesBuilder_GetBit(GlyphRangesBuilder* self, int n) => impl.GlyphRangesBuilder_GetBit(self, n);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImGuiTextFilter_Draw(ImGuiTextFilter* self, byte* label, float width) => impl.ImGuiTextFilter_Draw(self, label, width);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igShowDemoWindow(byte* p_open) => impl.igShowDemoWindow(p_open);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PathStroke(ImDrawList* self, uint col, byte closed, float thickness) => impl.ImDrawList_PathStroke(self, col, closed, thickness);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PathFillConvex(ImDrawList* self, uint col) => impl.ImDrawList_PathFillConvex(self, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PathLineToMergeDuplicate(ImDrawList* self, Vector2 pos) => impl.ImDrawList_PathLineToMergeDuplicate(self, pos);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndMenu() => impl.igEndMenu();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igColorButton(byte* desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size) => impl.igColorButton(desc_id, col, flags, size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFontAtlas_GetTexDataAsAlpha8(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel) => impl.ImFontAtlas_GetTexDataAsAlpha8(self, out_pixels, out_width, out_height, out_bytes_per_pixel);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsKeyReleased(int user_key_index) => impl.igIsKeyReleased(user_key_index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetClipboardText(byte* text) => impl.igSetClipboardText(text);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PathArcTo(ImDrawList* self, Vector2 centre, float radius, float a_min, float a_max, int num_segments) => impl.ImDrawList_PathArcTo(self, centre, radius, a_min, a_max, num_segments);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddConvexPolyFilled(ImDrawList* self, Vector2* points, int num_points, uint col) => impl.ImDrawList_AddConvexPolyFilled(self, points, num_points, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsWindowCollapsed() => impl.igIsWindowCollapsed();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igShowFontSelector(byte* label) => impl.igShowFontSelector(label);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddImageQuad(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col) => impl.ImDrawList_AddImageQuad(self, user_texture_id, a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetNextWindowFocus() => impl.igSetNextWindowFocus();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSameLine(float pos_x, float spacing_w) => impl.igSameLine(pos_x, spacing_w);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBegin(byte* name, byte* p_open, ImGuiWindowFlags flags) => impl.igBegin(name, p_open, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igColorEdit3(byte* label, Vector3* col, ImGuiColorEditFlags flags) => impl.igColorEdit3(label, col, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddImage(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col) => impl.ImDrawList_AddImage(self, user_texture_id, a, b, uv_a, uv_b, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiIO_AddInputCharactersUTF8(ImGuiIO* self, byte* utf8_chars) => impl.ImGuiIO_AddInputCharactersUTF8(self, utf8_chars);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddText(ImDrawList* self, Vector2 pos, uint col, byte* text_begin, byte* text_end) => impl.ImDrawList_AddText(self, pos, col, text_begin, text_end);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddTextFontPtr(ImDrawList* self, ImFont* font, float font_size, Vector2 pos, uint col, byte* text_begin, byte* text_end, float wrap_width, Vector4* cpu_fine_clip_rect) => impl.ImDrawList_AddTextFontPtr(self, font, font_size, pos, col, text_begin, text_end, wrap_width, cpu_fine_clip_rect);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddCircleFilled(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments) => impl.ImDrawList_AddCircleFilled(self, centre, radius, col, num_segments);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputFloat2(byte* label, Vector2* v, byte* format, ImGuiInputTextFlags extra_flags) => impl.igInputFloat2(label, v, format, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushButtonRepeat(byte repeat) => impl.igPushButtonRepeat(repeat);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPopItemWidth() => impl.igPopItemWidth();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddCircle(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments, float thickness) => impl.ImDrawList_AddCircle(self, centre, radius, col, num_segments, thickness);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddTriangleFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col) => impl.ImDrawList_AddTriangleFilled(self, a, b, c, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddTriangle(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col, float thickness) => impl.ImDrawList_AddTriangle(self, a, b, c, col, thickness);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddQuadFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col) => impl.ImDrawList_AddQuadFilled(self, a, b, c, d, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetFontSize() => impl.igGetFontSize();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputDouble(byte* label, double* v, double step, double step_fast, byte* format, ImGuiInputTextFlags extra_flags) => impl.igInputDouble(label, v, step, step_fast, format, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PrimReserve(ImDrawList* self, int idx_count, int vtx_count) => impl.ImDrawList_PrimReserve(self, idx_count, vtx_count);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddRectFilledMultiColor(ImDrawList* self, Vector2 a, Vector2 b, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left) => impl.ImDrawList_AddRectFilledMultiColor(self, a, b, col_upr_left, col_upr_right, col_bot_right, col_bot_left);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndPopup() => impl.igEndPopup();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFontAtlas_ClearInputData(ImFontAtlas* self) => impl.ImFontAtlas_ClearInputData(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddLine(ImDrawList* self, Vector2 a, Vector2 b, uint col, float thickness) => impl.ImDrawList_AddLine(self, a, b, col, thickness);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputTextMultiline(byte* label, byte* buf, uint buf_size, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data) => impl.igInputTextMultiline(label, buf, buf_size, size, flags, callback, user_data);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSelectable(byte* label, byte selected, ImGuiSelectableFlags flags, Vector2 size) => impl.igSelectable(label, selected, flags, size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSelectableBoolPtr(byte* label, byte* p_selected, ImGuiSelectableFlags flags, Vector2 size) => impl.igSelectableBoolPtr(label, p_selected, flags, size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igListBoxStr_arr(byte* label, int* current_item, byte** items, int items_count, int height_in_items) => impl.igListBoxStr_arr(label, current_item, items, items_count, height_in_items);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetCursorPos() => impl.igGetCursorPos();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ImDrawList_GetClipRectMin(ImDrawList* self) => impl.ImDrawList_GetClipRectMin(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PopTextureID(ImDrawList* self) => impl.ImDrawList_PopTextureID(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputFloat4(byte* label, Vector4* v, byte* format, ImGuiInputTextFlags extra_flags) => impl.igInputFloat4(label, v, format, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetCursorPosY(float y) => impl.igSetCursorPosY(y);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* igGetVersion() => impl.igGetVersion();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndCombo() => impl.igEndCombo();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushIDStr(byte* str_id) => impl.igPushIDStr(str_id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushIDRange(byte* str_id_begin, byte* str_id_end) => impl.igPushIDRange(str_id_begin, str_id_end);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushIDPtr(void* ptr_id) => impl.igPushIDPtr(ptr_id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushIDInt(int int_id) => impl.igPushIDInt(int_id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_ImDrawList(ImDrawList* self, IntPtr shared_data) => impl.ImDrawList_ImDrawList(self, shared_data);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawCmd_ImDrawCmd(ImDrawCmd* self) => impl.ImDrawCmd_ImDrawCmd(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiListClipper_End(ImGuiListClipper* self) => impl.ImGuiListClipper_End(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igAlignTextToFramePadding() => impl.igAlignTextToFramePadding();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPopStyleColor(int count) => impl.igPopStyleColor(count);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiListClipper_Begin(ImGuiListClipper* self, int items_count, float items_height) => impl.ImGuiListClipper_Begin(self, items_count, items_height);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igText(byte* fmt) => impl.igText(fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImGuiListClipper_Step(ImGuiListClipper* self) => impl.ImGuiListClipper_Step(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetTextLineHeightWithSpacing() => impl.igGetTextLineHeightWithSpacing();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float* ImGuiStorage_GetFloatRef(ImGuiStorage* self, uint key, float default_val) => impl.ImGuiStorage_GetFloatRef(self, key, default_val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndTooltip() => impl.igEndTooltip();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiListClipper_ImGuiListClipper(ImGuiListClipper* self, int items_count, float items_height) => impl.ImGuiListClipper_ImGuiListClipper(self, items_count, items_height);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragInt(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => impl.igDragInt(label, v, v_speed, v_min, v_max, format);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderFloat(byte* label, float* v, float v_min, float v_max, byte* format, float power) => impl.igSliderFloat(label, v, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint igColorConvertFloat4ToU32(Vector4 @in) => impl.igColorConvertFloat4ToU32(@in);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiIO_ClearInputCharacters(ImGuiIO* self) => impl.ImGuiIO_ClearInputCharacters(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect) => impl.igPushClipRect(clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetColumnWidth(int column_index, float width) => impl.igSetColumnWidth(column_index, width);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImGuiPayload_IsDataType(ImGuiPayload* self, byte* type) => impl.ImGuiPayload_IsDataType(self, type);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginMainMenuBar() => impl.igBeginMainMenuBar();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CustomRect_CustomRect(CustomRect* self) => impl.CustomRect_CustomRect(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImGuiInputTextCallbackData_HasSelection(ImGuiInputTextCallbackData* self) => impl.ImGuiInputTextCallbackData_HasSelection(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiInputTextCallbackData_InsertChars(ImGuiInputTextCallbackData* self, int pos, byte* text, byte* text_end) => impl.ImGuiInputTextCallbackData_InsertChars(self, pos, text, text_end);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImFontAtlas_GetMouseCursorTexData(ImFontAtlas* self, ImGuiMouseCursor cursor, Vector2* out_offset, Vector2* out_size, Vector2* out_uv_border, Vector2* out_uv_fill) => impl.ImFontAtlas_GetMouseCursorTexData(self, cursor, out_offset, out_size, out_uv_border, out_uv_fill);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igVSliderScalar(byte* label, Vector2 size, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power) => impl.igVSliderScalar(label, size, data_type, v, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiStorage_SetAllInt(ImGuiStorage* self, int val) => impl.ImGuiStorage_SetAllInt(self, val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void** ImGuiStorage_GetVoidPtrRef(ImGuiStorage* self, uint key, void* default_val) => impl.ImGuiStorage_GetVoidPtrRef(self, key, default_val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igStyleColorsLight(ImGuiStyle* dst) => impl.igStyleColorsLight(dst);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderFloat3(byte* label, Vector3* v, float v_min, float v_max, byte* format, float power) => impl.igSliderFloat3(label, v, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragFloat(byte* label, float* v, float v_speed, float v_min, float v_max, byte* format, float power) => impl.igDragFloat(label, v, v_speed, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* ImGuiStorage_GetBoolRef(ImGuiStorage* self, uint key, byte default_val) => impl.ImGuiStorage_GetBoolRef(self, key, default_val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetWindowHeight() => impl.igGetWindowHeight();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetMousePosOnOpeningCurrentPopup() => impl.igGetMousePosOnOpeningCurrentPopup();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int* ImGuiStorage_GetIntRef(ImGuiStorage* self, uint key, int default_val) => impl.ImGuiStorage_GetIntRef(self, key, default_val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igCalcListClipping(int items_count, float items_height, int* out_items_display_start, int* out_items_display_end) => impl.igCalcListClipping(items_count, items_height, out_items_display_start, out_items_display_end);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiStorage_SetVoidPtr(ImGuiStorage* self, uint key, void* val) => impl.ImGuiStorage_SetVoidPtr(self, key, val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndDragDropSource() => impl.igEndDragDropSource();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiStorage_BuildSortByKey(ImGuiStorage* self) => impl.ImGuiStorage_BuildSortByKey(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ImGuiStorage_GetFloat(ImGuiStorage* self, uint key, float default_val) => impl.ImGuiStorage_GetFloat(self, key, default_val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiStorage_SetBool(ImGuiStorage* self, uint key, byte val) => impl.ImGuiStorage_SetBool(self, key, val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImGuiStorage_GetBool(ImGuiStorage* self, uint key, byte default_val) => impl.ImGuiStorage_GetBool(self, key, default_val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetFrameHeightWithSpacing() => impl.igGetFrameHeightWithSpacing();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiStorage_SetInt(ImGuiStorage* self, uint key, int val) => impl.ImGuiStorage_SetInt(self, key, val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igCloseCurrentPopup() => impl.igCloseCurrentPopup();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiTextBuffer_clear(ImGuiTextBuffer* self) => impl.ImGuiTextBuffer_clear(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igBeginGroup() => impl.igBeginGroup();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiStorage_Clear(ImGuiStorage* self) => impl.ImGuiStorage_Clear(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Pair_PairInt(Pair* self, uint _key, int _val_i) => impl.Pair_PairInt(self, _key, _val_i);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Pair_PairFloat(Pair* self, uint _key, float _val_f) => impl.Pair_PairFloat(self, _key, _val_f);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Pair_PairPtr(Pair* self, uint _key, void* _val_p) => impl.Pair_PairPtr(self, _key, _val_p);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiTextBuffer_appendf(ImGuiTextBuffer* self, byte* fmt) => impl.ImGuiTextBuffer_appendf(self, fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* ImGuiTextBuffer_c_str(ImGuiTextBuffer* self) => impl.ImGuiTextBuffer_c_str(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiTextBuffer_reserve(ImGuiTextBuffer* self, int capacity) => impl.ImGuiTextBuffer_reserve(self, capacity);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImGuiTextBuffer_empty(ImGuiTextBuffer* self) => impl.ImGuiTextBuffer_empty(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderScalar(byte* label, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power) => impl.igSliderScalar(label, data_type, v, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginCombo(byte* label, byte* preview_value, ImGuiComboFlags flags) => impl.igBeginCombo(label, preview_value, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ImGuiTextBuffer_size(ImGuiTextBuffer* self) => impl.ImGuiTextBuffer_size(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginMenu(byte* label, byte enabled) => impl.igBeginMenu(label, enabled);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsItemHovered(ImGuiHoveredFlags flags) => impl.igIsItemHovered(flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PrimWriteVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col) => impl.ImDrawList_PrimWriteVtx(self, pos, uv, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igBullet() => impl.igBullet();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputText(byte* label, byte* buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data) => impl.igInputText(label, buf, buf_size, flags, callback, user_data);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputInt3(byte* label, int* v, ImGuiInputTextFlags extra_flags) => impl.igInputInt3(label, v, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiIO_ImGuiIO(ImGuiIO* self) => impl.ImGuiIO_ImGuiIO(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igStyleColorsDark(ImGuiStyle* dst) => impl.igStyleColorsDark(dst);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputInt(byte* label, int* v, int step, int step_fast, ImGuiInputTextFlags extra_flags) => impl.igInputInt(label, v, step, step_fast, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetWindowFontScale(float scale) => impl.igSetWindowFontScale(scale);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderInt(byte* label, int* v, int v_min, int v_max, byte* format) => impl.igSliderInt(label, v, v_min, v_max, format);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* TextRange_end(TextRange* self) => impl.TextRange_end(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* TextRange_begin(TextRange* self) => impl.TextRange_begin(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetNextWindowPos(Vector2 pos, ImGuiCond cond, Vector2 pivot) => impl.igSetNextWindowPos(pos, cond, pivot);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragInt3(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => impl.igDragInt3(label, v, v_speed, v_min, v_max, format);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igOpenPopup(byte* str_id) => impl.igOpenPopup(str_id);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextRange_TextRange(TextRange* self) => impl.TextRange_TextRange(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void TextRange_TextRangeStr(TextRange* self, byte* _b, byte* _e) => impl.TextRange_TextRangeStr(self, _b, _e);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ImDrawList_GetClipRectMax(ImDrawList* self) => impl.ImDrawList_GetClipRectMax(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igCalcTextSize(byte* text, byte* text_end, byte hide_text_after_double_hash, float wrap_width) => impl.igCalcTextSize(text, text_end, hide_text_after_double_hash, wrap_width);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr igGetDrawListSharedData() => impl.igGetDrawListSharedData();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igColumns(int count, byte* id, byte border) => impl.igColumns(count, id, border);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsItemActive() => impl.igIsItemActive();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiTextFilter_ImGuiTextFilter(ImGuiTextFilter* self, byte* default_filter) => impl.ImGuiTextFilter_ImGuiTextFilter(self, default_filter);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiOnceUponAFrame_ImGuiOnceUponAFrame(ImGuiOnceUponAFrame* self) => impl.ImGuiOnceUponAFrame_ImGuiOnceUponAFrame(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginDragDropTarget() => impl.igBeginDragDropTarget();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte TextRange_empty(TextRange* self) => impl.TextRange_empty(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImGuiPayload_IsDelivery(ImGuiPayload* self) => impl.ImGuiPayload_IsDelivery(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiIO_AddInputCharacter(ImGuiIO* self, ushort c) => impl.ImGuiIO_AddInputCharacter(self, c);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_AddImageRounded(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col, float rounding, int rounding_corners) => impl.ImDrawList_AddImageRounded(self, user_texture_id, a, b, uv_a, uv_b, col, rounding, rounding_corners);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiStyle_ImGuiStyle(ImGuiStyle* self) => impl.ImGuiStyle_ImGuiStyle(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igColorPicker3(byte* label, Vector3* col, ImGuiColorEditFlags flags) => impl.igColorPicker3(label, col, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetContentRegionMax() => impl.igGetContentRegionMax();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags) => impl.igBeginChildFrame(id, size, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSaveIniSettingsToDisk(byte* ini_filename) => impl.igSaveIniSettingsToDisk(ini_filename);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFont_ClearOutputData(ImFont* self) => impl.ImFont_ClearOutputData(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* igGetClipboardText() => impl.igGetClipboardText();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PrimQuadUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col) => impl.ImDrawList_PrimQuadUV(self, a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndDragDropTarget() => impl.igEndDragDropTarget();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ushort* ImFontAtlas_GetGlyphRangesKorean(ImFontAtlas* self) => impl.ImFontAtlas_GetGlyphRangesKorean(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int igGetKeyPressedAmount(int key_index, float repeat_delay, float rate) => impl.igGetKeyPressedAmount(key_index, repeat_delay, rate);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFontAtlas_GetTexDataAsRGBA32(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel) => impl.ImFontAtlas_GetTexDataAsRGBA32(self, out_pixels, out_width, out_height, out_bytes_per_pixel);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igNewFrame() => impl.igNewFrame();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igResetMouseDragDelta(int button) => impl.igResetMouseDragDelta(button);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetTreeNodeToLabelSpacing() => impl.igGetTreeNodeToLabelSpacing();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetMousePos() => impl.igGetMousePos();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void GlyphRangesBuilder_AddChar(GlyphRangesBuilder* self, ushort c) => impl.GlyphRangesBuilder_AddChar(self, c);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPopID() => impl.igPopID();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsMouseDoubleClicked(int button) => impl.igIsMouseDoubleClicked(button);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igStyleColorsClassic(ImGuiStyle* dst) => impl.igStyleColorsClassic(dst);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImGuiTextFilter_IsActive(ImGuiTextFilter* self) => impl.ImGuiTextFilter_IsActive(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PathClear(ImDrawList* self) => impl.ImDrawList_PathClear(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetWindowFocus() => impl.igSetWindowFocus();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetWindowFocusStr(byte* name) => impl.igSetWindowFocusStr(name);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igColorConvertHSVtoRGB(float h, float s, float v, float* out_r, float* out_g, float* out_b) => impl.igColorConvertHSVtoRGB(h, s, v, out_r, out_g, out_b);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImColor_ImColor(ImColor* self) => impl.ImColor_ImColor(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImColor_ImColorInt(ImColor* self, int r, int g, int b, int a) => impl.ImColor_ImColorInt(self, r, g, b, a);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImColor_ImColorU32(ImColor* self, uint rgba) => impl.ImColor_ImColorU32(self, rgba);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImColor_ImColorFloat(ImColor* self, float r, float g, float b, float a) => impl.ImColor_ImColorFloat(self, r, g, b, a);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImColor_ImColorVec4(ImColor* self, Vector4 col) => impl.ImColor_ImColorVec4(self, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igVSliderFloat(byte* label, Vector2 size, float* v, float v_min, float v_max, byte* format, float power) => impl.igVSliderFloat(label, size, v, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector4 igColorConvertU32ToFloat4(uint @in) => impl.igColorConvertU32ToFloat4(@in);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPopTextWrapPos() => impl.igPopTextWrapPos();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiTextFilter_Clear(ImGuiTextFilter* self) => impl.ImGuiTextFilter_Clear(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImGuiStorage* igGetStateStorage() => impl.igGetStateStorage();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetColumnWidth(int column_index) => impl.igGetColumnWidth(column_index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndMenuBar() => impl.igEndMenuBar();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetStateStorage(ImGuiStorage* storage) => impl.igSetStateStorage(storage);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* igGetStyleColorName(ImGuiCol idx) => impl.igGetStyleColorName(idx);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsMouseDragging(int button, float lock_threshold) => impl.igIsMouseDragging(button, lock_threshold);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PrimWriteIdx(ImDrawList* self, ushort idx) => impl.ImDrawList_PrimWriteIdx(self, idx);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiStyle_ScaleAllSizes(ImGuiStyle* self, float scale_factor) => impl.ImGuiStyle_ScaleAllSizes(self, scale_factor);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushStyleColorU32(ImGuiCol idx, uint col) => impl.igPushStyleColorU32(idx, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushStyleColor(ImGuiCol idx, Vector4 col) => impl.igPushStyleColor(idx, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* igMemAlloc(uint size) => impl.igMemAlloc(size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetCurrentContext(IntPtr ctx) => impl.igSetCurrentContext(ctx);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushItemWidth(float item_width) => impl.igPushItemWidth(item_width);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsWindowAppearing() => impl.igIsWindowAppearing();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImGuiStyle* igGetStyle() => impl.igGetStyle();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetItemAllowOverlap() => impl.igSetItemAllowOverlap();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndChild() => impl.igEndChild();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igCollapsingHeader(byte* label, ImGuiTreeNodeFlags flags) => impl.igCollapsingHeader(label, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igCollapsingHeaderBoolPtr(byte* label, byte* p_open, ImGuiTreeNodeFlags flags) => impl.igCollapsingHeaderBoolPtr(label, p_open, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragFloatRange2(byte* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, byte* format, byte* format_max, float power) => impl.igDragFloatRange2(label, v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetMouseCursor(ImGuiMouseCursor type) => impl.igSetMouseCursor(type);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetWindowContentRegionMax() => impl.igGetWindowContentRegionMax();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputScalar(byte* label, ImGuiDataType data_type, void* v, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags) => impl.igInputScalar(label, data_type, v, step, step_fast, format, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PushClipRectFullScreen(ImDrawList* self) => impl.ImDrawList_PushClipRectFullScreen(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint igGetColorU32(ImGuiCol idx, float alpha_mul) => impl.igGetColorU32(idx, alpha_mul);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint igGetColorU32Vec4(Vector4 col) => impl.igGetColorU32Vec4(col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint igGetColorU32U32(uint col) => impl.igGetColorU32U32(col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double igGetTime() => impl.igGetTime();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_ChannelsMerge(ImDrawList* self) => impl.ImDrawList_ChannelsMerge(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int igGetColumnIndex() => impl.igGetColumnIndex();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginPopupContextItem(byte* str_id, int mouse_button) => impl.igBeginPopupContextItem(str_id, mouse_button);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetCursorPosX(float x) => impl.igSetCursorPosX(x);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetItemRectSize() => impl.igGetItemRectSize();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igArrowButton(byte* str_id, ImGuiDir dir) => impl.igArrowButton(str_id, dir);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImGuiMouseCursor igGetMouseCursor() => impl.igGetMouseCursor();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushAllowKeyboardFocus(byte allow_keyboard_focus) => impl.igPushAllowKeyboardFocus(allow_keyboard_focus);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetScrollY() => impl.igGetScrollY();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetColumnOffset(int column_index, float offset_x) => impl.igSetColumnOffset(column_index, offset_x);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* ImGuiTextBuffer_begin(ImGuiTextBuffer* self) => impl.ImGuiTextBuffer_begin(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetWindowPosVec2(Vector2 pos, ImGuiCond cond) => impl.igSetWindowPosVec2(pos, cond);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetWindowPosStr(byte* name, Vector2 pos, ImGuiCond cond) => impl.igSetWindowPosStr(name, pos, cond);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetKeyboardFocusHere(int offset) => impl.igSetKeyboardFocusHere(offset);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetCursorPosY() => impl.igGetCursorPosY();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ImFontAtlas_AddCustomRectFontGlyph(ImFontAtlas* self, ImFont* font, ushort id, int width, int height, float advance_x, Vector2 offset) => impl.ImFontAtlas_AddCustomRectFontGlyph(self, font, id, width, height, advance_x, offset);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEndMainMenuBar() => impl.igEndMainMenuBar();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetContentRegionAvailWidth() => impl.igGetContentRegionAvailWidth();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsKeyDown(int user_key_index) => impl.igIsKeyDown(user_key_index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsMouseDown(int button) => impl.igIsMouseDown(button);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetWindowContentRegionMin() => impl.igGetWindowContentRegionMin();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igLogButtons() => impl.igLogButtons();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetWindowContentRegionWidth() => impl.igGetWindowContentRegionWidth();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderAngle(byte* label, float* v_rad, float v_degrees_min, float v_degrees_max) => impl.igSliderAngle(label, v_rad, v_degrees_min, v_degrees_max);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igTreeNodeExStr(byte* label, ImGuiTreeNodeFlags flags) => impl.igTreeNodeExStr(label, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igTreeNodeExStrStr(byte* str_id, ImGuiTreeNodeFlags flags, byte* fmt) => impl.igTreeNodeExStrStr(str_id, flags, fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igTreeNodeExPtr(void* ptr_id, ImGuiTreeNodeFlags flags, byte* fmt) => impl.igTreeNodeExPtr(ptr_id, flags, fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetWindowWidth() => impl.igGetWindowWidth();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushTextWrapPos(float wrap_pos_x) => impl.igPushTextWrapPos(wrap_pos_x);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ImGuiStorage_GetInt(ImGuiStorage* self, uint key, int default_val) => impl.ImGuiStorage_GetInt(self, key, default_val);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderInt3(byte* label, int* v, int v_min, int v_max, byte* format) => impl.igSliderInt3(label, v, v_min, v_max, format);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igShowUserGuide() => impl.igShowUserGuide();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igSliderScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* v_min, void* v_max, byte* format, float power) => impl.igSliderScalarN(label, data_type, v, components, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ImColor ImColor_HSV(ImColor* self, float h, float s, float v, float a) => impl.ImColor_HSV(self, h, s, v, a);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PathLineTo(ImDrawList* self, Vector2 pos) => impl.ImDrawList_PathLineTo(self, pos);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igImage(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col) => impl.igImage(user_texture_id, size, uv0, uv1, tint_col, border_col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback, void* custom_callback_data) => impl.igSetNextWindowSizeConstraints(size_min, size_max, custom_callback, custom_callback_data);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igDummy(Vector2 size) => impl.igDummy(size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igVSliderInt(byte* label, Vector2 size, int* v, int v_min, int v_max, byte* format) => impl.igVSliderInt(label, size, v, v_min, v_max, format);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImGuiTextBuffer_ImGuiTextBuffer(ImGuiTextBuffer* self) => impl.ImGuiTextBuffer_ImGuiTextBuffer(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igBulletText(byte* fmt) => impl.igBulletText(fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igColorEdit4(byte* label, Vector4* col, ImGuiColorEditFlags flags) => impl.igColorEdit4(label, col, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igColorPicker4(byte* label, Vector4* col, ImGuiColorEditFlags flags, float* ref_col) => impl.igColorPicker4(label, col, flags, ref_col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawList_PrimRectUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col) => impl.ImDrawList_PrimRectUV(self, a, b, uv_a, uv_b, col);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInvisibleButton(byte* str_id, Vector2 size) => impl.igInvisibleButton(str_id, size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igLogToClipboard(int max_depth) => impl.igLogToClipboard(max_depth);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igBeginPopupContextWindow(byte* str_id, int mouse_button, byte also_over_items) => impl.igBeginPopupContextWindow(str_id, mouse_button, also_over_items);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImFontAtlas_ImFontAtlas(ImFontAtlas* self) => impl.ImFontAtlas_ImFontAtlas(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragScalar(byte* label, ImGuiDataType data_type, void* v, float v_speed, void* v_min, void* v_max, byte* format, float power) => impl.igDragScalar(label, data_type, v, v_speed, v_min, v_max, format, power);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetItemDefaultFocus() => impl.igSetItemDefaultFocus();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igCaptureMouseFromApp(byte capture) => impl.igCaptureMouseFromApp(capture);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igIsAnyItemHovered() => impl.igIsAnyItemHovered();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPushFont(ImFont* font) => impl.igPushFont(font);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputInt2(byte* label, int* v, ImGuiInputTextFlags extra_flags) => impl.igInputInt2(label, v, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igTreePop() => impl.igTreePop();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igEnd() => impl.igEnd();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ImDrawData_ImDrawData(ImDrawData* self) => impl.ImDrawData_ImDrawData(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igDestroyContext(IntPtr ctx) => impl.igDestroyContext(ctx);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte* ImGuiTextBuffer_end(ImGuiTextBuffer* self) => impl.ImGuiTextBuffer_end(self);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igPopStyleVar(int count) => impl.igPopStyleVar(count);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ImGuiTextFilter_PassFilter(ImGuiTextFilter* self, byte* text, byte* text_end) => impl.ImGuiTextFilter_PassFilter(self, text, text_end);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igShowStyleSelector(byte* label) => impl.igShowStyleSelector(label);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags) => impl.igInputScalarN(label, data_type, v, components, step, step_fast, format, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igTreeNodeStr(byte* label) => impl.igTreeNodeStr(label);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igTreeNodeStrStr(byte* str_id, byte* fmt) => impl.igTreeNodeStrStr(str_id, fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igTreeNodePtr(void* ptr_id, byte* fmt) => impl.igTreeNodePtr(ptr_id, fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float igGetScrollMaxX() => impl.igGetScrollMaxX();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void igSetTooltip(byte* fmt) => impl.igSetTooltip(fmt);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 igGetContentRegionAvail() => impl.igGetContentRegionAvail();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igInputFloat3(byte* label, Vector3* v, byte* format, ImGuiInputTextFlags extra_flags) => impl.igInputFloat3(label, v, format, extra_flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte igDragFloat2(byte* label, Vector2* v, float v_speed, float v_min, float v_max, byte* format, float power) => impl.igDragFloat2(label, v, v_speed, v_min, v_max, format, power);
    }
#pragma warning restore 1591
}
