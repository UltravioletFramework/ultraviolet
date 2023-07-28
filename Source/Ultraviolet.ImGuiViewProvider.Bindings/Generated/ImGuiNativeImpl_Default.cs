using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.ImGuiViewProvider.Bindings
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class ImGuiNativeImpl_Default : ImGuiNativeImpl
    {
        private static readonly NativeLibrary lib;
        
        static ImGuiNativeImpl_Default()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Linux:
                    lib = new NativeLibrary("libcimgui");
                    break;
                case UltravioletPlatform.macOS:
                    lib = new NativeLibrary("libcimgui");
                    break;
                default:
                    lib = new NativeLibrary("cimgui");
                    break;
            }
        }
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetFrameHeightDelegate();
        private readonly igGetFrameHeightDelegate pigGetFrameHeight = lib.LoadFunction<igGetFrameHeightDelegate>("igGetFrameHeight");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetFrameHeight() => pigGetFrameHeight();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr igCreateContextDelegate(ImFontAtlas* shared_font_atlas);
        private readonly igCreateContextDelegate pigCreateContext = lib.LoadFunction<igCreateContextDelegate>("igCreateContext");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr igCreateContext(ImFontAtlas* shared_font_atlas) => pigCreateContext(shared_font_atlas);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTextUnformattedDelegate(byte* text, byte* text_end);
        private readonly igTextUnformattedDelegate pigTextUnformatted = lib.LoadFunction<igTextUnformattedDelegate>("igTextUnformatted");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTextUnformatted(byte* text, byte* text_end) => pigTextUnformatted(text, text_end);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopFontDelegate();
        private readonly igPopFontDelegate pigPopFont = lib.LoadFunction<igPopFontDelegate>("igPopFont");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopFont() => pigPopFont();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igComboDelegate(byte* label, int* current_item, byte** items, int items_count, int popup_max_height_in_items);
        private readonly igComboDelegate pigCombo = lib.LoadFunction<igComboDelegate>("igCombo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igCombo(byte* label, int* current_item, byte** items, int items_count, int popup_max_height_in_items) => pigCombo(label, current_item, items, items_count, popup_max_height_in_items);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igComboStrDelegate(byte* label, int* current_item, byte* items_separated_by_zeros, int popup_max_height_in_items);
        private readonly igComboStrDelegate pigComboStr = lib.LoadFunction<igComboStrDelegate>("igComboStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igComboStr(byte* label, int* current_item, byte* items_separated_by_zeros, int popup_max_height_in_items) => pigComboStr(label, current_item, items_separated_by_zeros, popup_max_height_in_items);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igCaptureKeyboardFromAppDelegate(byte capture);
        private readonly igCaptureKeyboardFromAppDelegate pigCaptureKeyboardFromApp = lib.LoadFunction<igCaptureKeyboardFromAppDelegate>("igCaptureKeyboardFromApp");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igCaptureKeyboardFromApp(byte capture) => pigCaptureKeyboardFromApp(capture);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsWindowFocusedDelegate(ImGuiFocusedFlags flags);
        private readonly igIsWindowFocusedDelegate pigIsWindowFocused = lib.LoadFunction<igIsWindowFocusedDelegate>("igIsWindowFocused");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsWindowFocused(ImGuiFocusedFlags flags) => pigIsWindowFocused(flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igRenderDelegate();
        private readonly igRenderDelegate pigRender = lib.LoadFunction<igRenderDelegate>("igRender");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igRender() => pigRender();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ChannelsSetCurrentDelegate(ImDrawList* self, int channel_index);
        private readonly ImDrawList_ChannelsSetCurrentDelegate pImDrawList_ChannelsSetCurrent = lib.LoadFunction<ImDrawList_ChannelsSetCurrentDelegate>("ImDrawList_ChannelsSetCurrent");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_ChannelsSetCurrent(ImDrawList* self, int channel_index) => pImDrawList_ChannelsSetCurrent(self, channel_index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragFloat4Delegate(byte* label, Vector4* v, float v_speed, float v_min, float v_max, byte* format, float power);
        private readonly igDragFloat4Delegate pigDragFloat4 = lib.LoadFunction<igDragFloat4Delegate>("igDragFloat4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragFloat4(byte* label, Vector4* v, float v_speed, float v_min, float v_max, byte* format, float power) => pigDragFloat4(label, v, v_speed, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ChannelsSplitDelegate(ImDrawList* self, int channels_count);
        private readonly ImDrawList_ChannelsSplitDelegate pImDrawList_ChannelsSplit = lib.LoadFunction<ImDrawList_ChannelsSplitDelegate>("ImDrawList_ChannelsSplit");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_ChannelsSplit(ImDrawList* self, int channels_count) => pImDrawList_ChannelsSplit(self, channels_count);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMousePosValidDelegate(Vector2* mouse_pos);
        private readonly igIsMousePosValidDelegate pigIsMousePosValid = lib.LoadFunction<igIsMousePosValidDelegate>("igIsMousePosValid");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMousePosValid(Vector2* mouse_pos) => pigIsMousePosValid(mouse_pos);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetCursorScreenPosDelegate();
        private readonly igGetCursorScreenPosDelegate pigGetCursorScreenPos = lib.LoadFunction<igGetCursorScreenPosDelegate>("igGetCursorScreenPos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetCursorScreenPos() => pigGetCursorScreenPos();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDebugCheckVersionAndDataLayoutDelegate(byte* version_str, uint sz_io, uint sz_style, uint sz_vec2, uint sz_vec4, uint sz_drawvert);
        private readonly igDebugCheckVersionAndDataLayoutDelegate pigDebugCheckVersionAndDataLayout = lib.LoadFunction<igDebugCheckVersionAndDataLayoutDelegate>("igDebugCheckVersionAndDataLayout");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDebugCheckVersionAndDataLayout(byte* version_str, uint sz_io, uint sz_style, uint sz_vec2, uint sz_vec4, uint sz_drawvert) => pigDebugCheckVersionAndDataLayout(version_str, sz_io, sz_style, sz_vec2, sz_vec4, sz_drawvert);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetScrollHereDelegate(float center_y_ratio);
        private readonly igSetScrollHereDelegate pigSetScrollHere = lib.LoadFunction<igSetScrollHereDelegate>("igSetScrollHere");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetScrollHere(float center_y_ratio) => pigSetScrollHere(center_y_ratio);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetScrollYDelegate(float scroll_y);
        private readonly igSetScrollYDelegate pigSetScrollY = lib.LoadFunction<igSetScrollYDelegate>("igSetScrollY");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetScrollY(float scroll_y) => pigSetScrollY(scroll_y);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetColorEditOptionsDelegate(ImGuiColorEditFlags flags);
        private readonly igSetColorEditOptionsDelegate pigSetColorEditOptions = lib.LoadFunction<igSetColorEditOptionsDelegate>("igSetColorEditOptions");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetColorEditOptions(ImGuiColorEditFlags flags) => pigSetColorEditOptions(flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetScrollFromPosYDelegate(float pos_y, float center_y_ratio);
        private readonly igSetScrollFromPosYDelegate pigSetScrollFromPosY = lib.LoadFunction<igSetScrollFromPosYDelegate>("igSetScrollFromPosY");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetScrollFromPosY(float pos_y, float center_y_ratio) => pigSetScrollFromPosY(pos_y, center_y_ratio);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector4* igGetStyleColorVec4Delegate(ImGuiCol idx);
        private readonly igGetStyleColorVec4Delegate pigGetStyleColorVec4 = lib.LoadFunction<igGetStyleColorVec4Delegate>("igGetStyleColorVec4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector4* igGetStyleColorVec4(ImGuiCol idx) => pigGetStyleColorVec4(idx);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseHoveringRectDelegate(Vector2 r_min, Vector2 r_max, byte clip);
        private readonly igIsMouseHoveringRectDelegate pigIsMouseHoveringRect = lib.LoadFunction<igIsMouseHoveringRectDelegate>("igIsMouseHoveringRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseHoveringRect(Vector2 r_min, Vector2 r_max, byte clip) => pigIsMouseHoveringRect(r_min, r_max, clip);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImVec4_ImVec4Delegate(Vector4* self);
        private readonly ImVec4_ImVec4Delegate pImVec4_ImVec4 = lib.LoadFunction<ImVec4_ImVec4Delegate>("ImVec4_ImVec4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImVec4_ImVec4(Vector4* self) => pImVec4_ImVec4(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImVec4_ImVec4FloatDelegate(Vector4* self, float _x, float _y, float _z, float _w);
        private readonly ImVec4_ImVec4FloatDelegate pImVec4_ImVec4Float = lib.LoadFunction<ImVec4_ImVec4FloatDelegate>("ImVec4_ImVec4Float");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImVec4_ImVec4Float(Vector4* self, float _x, float _y, float _z, float _w) => pImVec4_ImVec4Float(self, _x, _y, _z, _w);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_SetHSVDelegate(ImColor* self, float h, float s, float v, float a);
        private readonly ImColor_SetHSVDelegate pImColor_SetHSV = lib.LoadFunction<ImColor_SetHSVDelegate>("ImColor_SetHSV");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_SetHSV(ImColor* self, float h, float s, float v, float a) => pImColor_SetHSV(self, h, s, v, a);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragFloat3Delegate(byte* label, Vector3* v, float v_speed, float v_min, float v_max, byte* format, float power);
        private readonly igDragFloat3Delegate pigDragFloat3 = lib.LoadFunction<igDragFloat3Delegate>("igDragFloat3");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragFloat3(byte* label, Vector3* v, float v_speed, float v_min, float v_max, byte* format, float power) => pigDragFloat3(label, v, v_speed, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddPolylineDelegate(ImDrawList* self, Vector2* points, int num_points, uint col, byte closed, float thickness);
        private readonly ImDrawList_AddPolylineDelegate pImDrawList_AddPolyline = lib.LoadFunction<ImDrawList_AddPolylineDelegate>("ImDrawList_AddPolyline");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddPolyline(ImDrawList* self, Vector2* points, int num_points, uint col, byte closed, float thickness) => pImDrawList_AddPolyline(self, points, num_points, col, closed, thickness);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igValueBoolDelegate(byte* prefix, byte b);
        private readonly igValueBoolDelegate pigValueBool = lib.LoadFunction<igValueBoolDelegate>("igValueBool");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igValueBool(byte* prefix, byte b) => pigValueBool(prefix, b);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igValueIntDelegate(byte* prefix, int v);
        private readonly igValueIntDelegate pigValueInt = lib.LoadFunction<igValueIntDelegate>("igValueInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igValueInt(byte* prefix, int v) => pigValueInt(prefix, v);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igValueUintDelegate(byte* prefix, uint v);
        private readonly igValueUintDelegate pigValueUint = lib.LoadFunction<igValueUintDelegate>("igValueUint");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igValueUint(byte* prefix, uint v) => pigValueUint(prefix, v);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igValueFloatDelegate(byte* prefix, float v, byte* float_format);
        private readonly igValueFloatDelegate pigValueFloat = lib.LoadFunction<igValueFloatDelegate>("igValueFloat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igValueFloat(byte* prefix, float v, byte* float_format) => pigValueFloat(prefix, v, float_format);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextFilter_BuildDelegate(ImGuiTextFilter* self);
        private readonly ImGuiTextFilter_BuildDelegate pImGuiTextFilter_Build = lib.LoadFunction<ImGuiTextFilter_BuildDelegate>("ImGuiTextFilter_Build");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextFilter_Build(ImGuiTextFilter* self) => pImGuiTextFilter_Build(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetItemRectMaxDelegate();
        private readonly igGetItemRectMaxDelegate pigGetItemRectMax = lib.LoadFunction<igGetItemRectMaxDelegate>("igGetItemRectMax");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetItemRectMax() => pigGetItemRectMax();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemDeactivatedDelegate();
        private readonly igIsItemDeactivatedDelegate pigIsItemDeactivated = lib.LoadFunction<igIsItemDeactivatedDelegate>("igIsItemDeactivated");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemDeactivated() => pigIsItemDeactivated();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushStyleVarFloatDelegate(ImGuiStyleVar idx, float val);
        private readonly igPushStyleVarFloatDelegate pigPushStyleVarFloat = lib.LoadFunction<igPushStyleVarFloatDelegate>("igPushStyleVarFloat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushStyleVarFloat(ImGuiStyleVar idx, float val) => pigPushStyleVarFloat(idx, val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushStyleVarVec2Delegate(ImGuiStyleVar idx, Vector2 val);
        private readonly igPushStyleVarVec2Delegate pigPushStyleVarVec2 = lib.LoadFunction<igPushStyleVarVec2Delegate>("igPushStyleVarVec2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushStyleVarVec2(ImGuiStyleVar idx, Vector2 val) => pigPushStyleVarVec2(idx, val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* igSaveIniSettingsToMemoryDelegate(uint* out_ini_size);
        private readonly igSaveIniSettingsToMemoryDelegate pigSaveIniSettingsToMemory = lib.LoadFunction<igSaveIniSettingsToMemoryDelegate>("igSaveIniSettingsToMemory");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* igSaveIniSettingsToMemory(uint* out_ini_size) => pigSaveIniSettingsToMemory(out_ini_size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragIntRange2Delegate(byte* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, byte* format, byte* format_max);
        private readonly igDragIntRange2Delegate pigDragIntRange2 = lib.LoadFunction<igDragIntRange2Delegate>("igDragIntRange2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragIntRange2(byte* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, byte* format, byte* format_max) => pigDragIntRange2(label, v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igUnindentDelegate(float indent_w);
        private readonly igUnindentDelegate pigUnindent = lib.LoadFunction<igUnindentDelegate>("igUnindent");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igUnindent(float indent_w) => pigUnindent(indent_w);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontFromMemoryCompressedBase85TTFDelegate(ImFontAtlas* self, byte* compressed_font_data_base85, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        private readonly ImFontAtlas_AddFontFromMemoryCompressedBase85TTFDelegate pImFontAtlas_AddFontFromMemoryCompressedBase85TTF = lib.LoadFunction<ImFontAtlas_AddFontFromMemoryCompressedBase85TTFDelegate>("ImFontAtlas_AddFontFromMemoryCompressedBase85TTF");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(ImFontAtlas* self, byte* compressed_font_data_base85, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => pImFontAtlas_AddFontFromMemoryCompressedBase85TTF(self, compressed_font_data_base85, size_pixels, font_cfg, glyph_ranges);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopAllowKeyboardFocusDelegate();
        private readonly igPopAllowKeyboardFocusDelegate pigPopAllowKeyboardFocus = lib.LoadFunction<igPopAllowKeyboardFocusDelegate>("igPopAllowKeyboardFocus");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopAllowKeyboardFocus() => pigPopAllowKeyboardFocus();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLoadIniSettingsFromDiskDelegate(byte* ini_filename);
        private readonly igLoadIniSettingsFromDiskDelegate pigLoadIniSettingsFromDisk = lib.LoadFunction<igLoadIniSettingsFromDiskDelegate>("igLoadIniSettingsFromDisk");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLoadIniSettingsFromDisk(byte* ini_filename) => pigLoadIniSettingsFromDisk(ini_filename);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetCursorStartPosDelegate();
        private readonly igGetCursorStartPosDelegate pigGetCursorStartPos = lib.LoadFunction<igGetCursorStartPosDelegate>("igGetCursorStartPos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetCursorStartPos() => pigGetCursorStartPos();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetCursorScreenPosDelegate(Vector2 screen_pos);
        private readonly igSetCursorScreenPosDelegate pigSetCursorScreenPos = lib.LoadFunction<igSetCursorScreenPosDelegate>("igSetCursorScreenPos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetCursorScreenPos(Vector2 screen_pos) => pigSetCursorScreenPos(screen_pos);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputInt4Delegate(byte* label, int* v, ImGuiInputTextFlags extra_flags);
        private readonly igInputInt4Delegate pigInputInt4 = lib.LoadFunction<igInputInt4Delegate>("igInputInt4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputInt4(byte* label, int* v, ImGuiInputTextFlags extra_flags) => pigInputInt4(label, v, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_AddRemapCharDelegate(ImFont* self, ushort dst, ushort src, byte overwrite_dst);
        private readonly ImFont_AddRemapCharDelegate pImFont_AddRemapChar = lib.LoadFunction<ImFont_AddRemapCharDelegate>("ImFont_AddRemapChar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_AddRemapChar(ImFont* self, ushort dst, ushort src, byte overwrite_dst) => pImFont_AddRemapChar(self, dst, src, overwrite_dst);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_AddGlyphDelegate(ImFont* self, ushort c, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1, float advance_x);
        private readonly ImFont_AddGlyphDelegate pImFont_AddGlyph = lib.LoadFunction<ImFont_AddGlyphDelegate>("ImFont_AddGlyph");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_AddGlyph(ImFont* self, ushort c, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1, float advance_x) => pImFont_AddGlyph(self, c, x0, y0, x1, y1, u0, v0, u1, v1, advance_x);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsRectVisibleDelegate(Vector2 size);
        private readonly igIsRectVisibleDelegate pigIsRectVisible = lib.LoadFunction<igIsRectVisibleDelegate>("igIsRectVisible");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsRectVisible(Vector2 size) => pigIsRectVisible(size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsRectVisibleVec2Delegate(Vector2 rect_min, Vector2 rect_max);
        private readonly igIsRectVisibleVec2Delegate pigIsRectVisibleVec2 = lib.LoadFunction<igIsRectVisibleVec2Delegate>("igIsRectVisibleVec2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsRectVisibleVec2(Vector2 rect_min, Vector2 rect_max) => pigIsRectVisibleVec2(rect_min, rect_max);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_GrowIndexDelegate(ImFont* self, int new_size);
        private readonly ImFont_GrowIndexDelegate pImFont_GrowIndex = lib.LoadFunction<ImFont_GrowIndexDelegate>("ImFont_GrowIndex");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_GrowIndex(ImFont* self, int new_size) => pImFont_GrowIndex(self, new_size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImFontAtlas_BuildDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_BuildDelegate pImFontAtlas_Build = lib.LoadFunction<ImFontAtlas_BuildDelegate>("ImFontAtlas_Build");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImFontAtlas_Build(ImFontAtlas* self) => pImFontAtlas_Build(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLabelTextDelegate(byte* label, byte* fmt);
        private readonly igLabelTextDelegate pigLabelText = lib.LoadFunction<igLabelTextDelegate>("igLabelText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLabelText(byte* label, byte* fmt) => pigLabelText(label, fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_RenderTextDelegate(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, byte* text_begin, byte* text_end, float wrap_width, byte cpu_fine_clip);
        private readonly ImFont_RenderTextDelegate pImFont_RenderText = lib.LoadFunction<ImFont_RenderTextDelegate>("ImFont_RenderText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_RenderText(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, byte* text_begin, byte* text_end, float wrap_width, byte cpu_fine_clip) => pImFont_RenderText(self, draw_list, size, pos, col, clip_rect, text_begin, text_end, wrap_width, cpu_fine_clip);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogFinishDelegate();
        private readonly igLogFinishDelegate pigLogFinish = lib.LoadFunction<igLogFinishDelegate>("igLogFinish");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogFinish() => pigLogFinish();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsKeyPressedDelegate(int user_key_index, byte repeat);
        private readonly igIsKeyPressedDelegate pigIsKeyPressed = lib.LoadFunction<igIsKeyPressedDelegate>("igIsKeyPressed");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsKeyPressed(int user_key_index, byte repeat) => pigIsKeyPressed(user_key_index, repeat);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetColumnOffsetDelegate(int column_index);
        private readonly igGetColumnOffsetDelegate pigGetColumnOffset = lib.LoadFunction<igGetColumnOffsetDelegate>("igGetColumnOffset");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetColumnOffset(int column_index) => pigGetColumnOffset(column_index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PopClipRectDelegate(ImDrawList* self);
        private readonly ImDrawList_PopClipRectDelegate pImDrawList_PopClipRect = lib.LoadFunction<ImDrawList_PopClipRectDelegate>("ImDrawList_PopClipRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PopClipRect(ImDrawList* self) => pImDrawList_PopClipRect(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFontGlyph* ImFont_FindGlyphNoFallbackDelegate(ImFont* self, ushort c);
        private readonly ImFont_FindGlyphNoFallbackDelegate pImFont_FindGlyphNoFallback = lib.LoadFunction<ImFont_FindGlyphNoFallbackDelegate>("ImFont_FindGlyphNoFallback");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFontGlyph* ImFont_FindGlyphNoFallback(ImFont* self, ushort c) => pImFont_FindGlyphNoFallback(self, c);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowCollapsedDelegate(byte collapsed, ImGuiCond cond);
        private readonly igSetNextWindowCollapsedDelegate pigSetNextWindowCollapsed = lib.LoadFunction<igSetNextWindowCollapsedDelegate>("igSetNextWindowCollapsed");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowCollapsed(byte collapsed, ImGuiCond cond) => pigSetNextWindowCollapsed(collapsed, cond);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr igGetCurrentContextDelegate();
        private readonly igGetCurrentContextDelegate pigGetCurrentContext = lib.LoadFunction<igGetCurrentContextDelegate>("igGetCurrentContext");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr igGetCurrentContext() => pigGetCurrentContext();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSmallButtonDelegate(byte* label);
        private readonly igSmallButtonDelegate pigSmallButton = lib.LoadFunction<igSmallButtonDelegate>("igSmallButton");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSmallButton(byte* label) => pigSmallButton(label);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igOpenPopupOnItemClickDelegate(byte* str_id, int mouse_button);
        private readonly igOpenPopupOnItemClickDelegate pigOpenPopupOnItemClick = lib.LoadFunction<igOpenPopupOnItemClickDelegate>("igOpenPopupOnItemClick");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igOpenPopupOnItemClick(byte* str_id, int mouse_button) => pigOpenPopupOnItemClick(str_id, mouse_button);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsAnyMouseDownDelegate();
        private readonly igIsAnyMouseDownDelegate pigIsAnyMouseDown = lib.LoadFunction<igIsAnyMouseDownDelegate>("igIsAnyMouseDown");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsAnyMouseDown() => pigIsAnyMouseDown();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImFont_CalcWordWrapPositionADelegate(ImFont* self, float scale, byte* text, byte* text_end, float wrap_width);
        private readonly ImFont_CalcWordWrapPositionADelegate pImFont_CalcWordWrapPositionA = lib.LoadFunction<ImFont_CalcWordWrapPositionADelegate>("ImFont_CalcWordWrapPositionA");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImFont_CalcWordWrapPositionA(ImFont* self, float scale, byte* text, byte* text_end, float wrap_width) => pImFont_CalcWordWrapPositionA(self, scale, text, text_end, wrap_width);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 ImFont_CalcTextSizeADelegate(ImFont* self, float size, float max_width, float wrap_width, byte* text_begin, byte* text_end, byte** remaining);
        private readonly ImFont_CalcTextSizeADelegate pImFont_CalcTextSizeA = lib.LoadFunction<ImFont_CalcTextSizeADelegate>("ImFont_CalcTextSizeA");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 ImFont_CalcTextSizeA(ImFont* self, float size, float max_width, float wrap_width, byte* text_begin, byte* text_end, byte** remaining) => pImFont_CalcTextSizeA(self, size, max_width, wrap_width, text_begin, text_end, remaining);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_SetBitDelegate(GlyphRangesBuilder* self, int n);
        private readonly GlyphRangesBuilder_SetBitDelegate pGlyphRangesBuilder_SetBit = lib.LoadFunction<GlyphRangesBuilder_SetBitDelegate>("GlyphRangesBuilder_SetBit");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_SetBit(GlyphRangesBuilder* self, int n) => pGlyphRangesBuilder_SetBit(self, n);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImFont_IsLoadedDelegate(ImFont* self);
        private readonly ImFont_IsLoadedDelegate pImFont_IsLoaded = lib.LoadFunction<ImFont_IsLoadedDelegate>("ImFont_IsLoaded");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImFont_IsLoaded(ImFont* self) => pImFont_IsLoaded(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float ImFont_GetCharAdvanceDelegate(ImFont* self, ushort c);
        private readonly ImFont_GetCharAdvanceDelegate pImFont_GetCharAdvance = lib.LoadFunction<ImFont_GetCharAdvanceDelegate>("ImFont_GetCharAdvance");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float ImFont_GetCharAdvance(ImFont* self, ushort c) => pImFont_GetCharAdvance(self, c);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igImageButtonDelegate(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, int frame_padding, Vector4 bg_col, Vector4 tint_col);
        private readonly igImageButtonDelegate pigImageButton = lib.LoadFunction<igImageButtonDelegate>("igImageButton");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igImageButton(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, int frame_padding, Vector4 bg_col, Vector4 tint_col) => pigImageButton(user_texture_id, size, uv0, uv1, frame_padding, bg_col, tint_col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_SetFallbackCharDelegate(ImFont* self, ushort c);
        private readonly ImFont_SetFallbackCharDelegate pImFont_SetFallbackChar = lib.LoadFunction<ImFont_SetFallbackCharDelegate>("ImFont_SetFallbackChar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_SetFallbackChar(ImFont* self, ushort c) => pImFont_SetFallbackChar(self, c);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndFrameDelegate();
        private readonly igEndFrameDelegate pigEndFrame = lib.LoadFunction<igEndFrameDelegate>("igEndFrame");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndFrame() => pigEndFrame();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderFloat2Delegate(byte* label, Vector2* v, float v_min, float v_max, byte* format, float power);
        private readonly igSliderFloat2Delegate pigSliderFloat2 = lib.LoadFunction<igSliderFloat2Delegate>("igSliderFloat2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderFloat2(byte* label, Vector2* v, float v_min, float v_max, byte* format, float power) => pigSliderFloat2(label, v, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_RenderCharDelegate(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, ushort c);
        private readonly ImFont_RenderCharDelegate pImFont_RenderChar = lib.LoadFunction<ImFont_RenderCharDelegate>("ImFont_RenderChar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_RenderChar(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, ushort c) => pImFont_RenderChar(self, draw_list, size, pos, col, c);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igRadioButtonBoolDelegate(byte* label, byte active);
        private readonly igRadioButtonBoolDelegate pigRadioButtonBool = lib.LoadFunction<igRadioButtonBoolDelegate>("igRadioButtonBool");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igRadioButtonBool(byte* label, byte active) => pigRadioButtonBool(label, active);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igRadioButtonIntPtrDelegate(byte* label, int* v, int v_button);
        private readonly igRadioButtonIntPtrDelegate pigRadioButtonIntPtr = lib.LoadFunction<igRadioButtonIntPtrDelegate>("igRadioButtonIntPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igRadioButtonIntPtr(byte* label, int* v, int v_button) => pigRadioButtonIntPtr(label, v, v_button);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PushClipRectDelegate(ImDrawList* self, Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect);
        private readonly ImDrawList_PushClipRectDelegate pImDrawList_PushClipRect = lib.LoadFunction<ImDrawList_PushClipRectDelegate>("ImDrawList_PushClipRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PushClipRect(ImDrawList* self, Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect) => pImDrawList_PushClipRect(self, clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFontGlyph* ImFont_FindGlyphDelegate(ImFont* self, ushort c);
        private readonly ImFont_FindGlyphDelegate pImFont_FindGlyph = lib.LoadFunction<ImFont_FindGlyphDelegate>("ImFont_FindGlyph");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFontGlyph* ImFont_FindGlyph(ImFont* self, ushort c) => pImFont_FindGlyph(self, c);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemDeactivatedAfterEditDelegate();
        private readonly igIsItemDeactivatedAfterEditDelegate pigIsItemDeactivatedAfterEdit = lib.LoadFunction<igIsItemDeactivatedAfterEditDelegate>("igIsItemDeactivatedAfterEdit");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemDeactivatedAfterEdit() => pigIsItemDeactivatedAfterEdit();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImDrawList* igGetWindowDrawListDelegate();
        private readonly igGetWindowDrawListDelegate pigGetWindowDrawList = lib.LoadFunction<igGetWindowDrawListDelegate>("igGetWindowDrawList");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImDrawList* igGetWindowDrawList() => pigGetWindowDrawList();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontDelegate(ImFontAtlas* self, ImFontConfig* font_cfg);
        private readonly ImFontAtlas_AddFontDelegate pImFontAtlas_AddFont = lib.LoadFunction<ImFontAtlas_AddFontDelegate>("ImFontAtlas_AddFont");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFont(ImFontAtlas* self, ImFontConfig* font_cfg) => pImFontAtlas_AddFont(self, font_cfg);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathBezierCurveToDelegate(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, int num_segments);
        private readonly ImDrawList_PathBezierCurveToDelegate pImDrawList_PathBezierCurveTo = lib.LoadFunction<ImDrawList_PathBezierCurveToDelegate>("ImDrawList_PathBezierCurveTo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathBezierCurveTo(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, int num_segments) => pImDrawList_PathBezierCurveTo(self, p1, p2, p3, num_segments);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiPayload_ClearDelegate(ImGuiPayload* self);
        private readonly ImGuiPayload_ClearDelegate pImGuiPayload_Clear = lib.LoadFunction<ImGuiPayload_ClearDelegate>("ImGuiPayload_Clear");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiPayload_Clear(ImGuiPayload* self) => pImGuiPayload_Clear(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igNewLineDelegate();
        private readonly igNewLineDelegate pigNewLine = lib.LoadFunction<igNewLineDelegate>("igNewLine");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igNewLine() => pigNewLine();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemFocusedDelegate();
        private readonly igIsItemFocusedDelegate pigIsItemFocused = lib.LoadFunction<igIsItemFocusedDelegate>("igIsItemFocused");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemFocused() => pigIsItemFocused();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLoadIniSettingsFromMemoryDelegate(byte* ini_data, uint ini_size);
        private readonly igLoadIniSettingsFromMemoryDelegate pigLoadIniSettingsFromMemory = lib.LoadFunction<igLoadIniSettingsFromMemoryDelegate>("igLoadIniSettingsFromMemory");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLoadIniSettingsFromMemory(byte* ini_data, uint ini_size) => pigLoadIniSettingsFromMemory(ini_data, ini_size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderInt2Delegate(byte* label, int* v, int v_min, int v_max, byte* format);
        private readonly igSliderInt2Delegate pigSliderInt2 = lib.LoadFunction<igSliderInt2Delegate>("igSliderInt2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderInt2(byte* label, int* v, int v_min, int v_max, byte* format) => pigSliderInt2(label, v, v_min, v_max, format);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowSizeVec2Delegate(Vector2 size, ImGuiCond cond);
        private readonly igSetWindowSizeVec2Delegate pigSetWindowSizeVec2 = lib.LoadFunction<igSetWindowSizeVec2Delegate>("igSetWindowSizeVec2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowSizeVec2(Vector2 size, ImGuiCond cond) => pigSetWindowSizeVec2(size, cond);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowSizeStrDelegate(byte* name, Vector2 size, ImGuiCond cond);
        private readonly igSetWindowSizeStrDelegate pigSetWindowSizeStr = lib.LoadFunction<igSetWindowSizeStrDelegate>("igSetWindowSizeStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowSizeStr(byte* name, Vector2 size, ImGuiCond cond) => pigSetWindowSizeStr(name, size, cond);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputFloatDelegate(byte* label, float* v, float step, float step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        private readonly igInputFloatDelegate pigInputFloat = lib.LoadFunction<igInputFloatDelegate>("igInputFloat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputFloat(byte* label, float* v, float step, float step_fast, byte* format, ImGuiInputTextFlags extra_flags) => pigInputFloat(label, v, step, step_fast, format, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_ImFontDelegate(ImFont* self);
        private readonly ImFont_ImFontDelegate pImFont_ImFont = lib.LoadFunction<ImFont_ImFontDelegate>("ImFont_ImFont");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_ImFont(ImFont* self) => pImFont_ImFont(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_SetFloatDelegate(ImGuiStorage* self, uint key, float val);
        private readonly ImGuiStorage_SetFloatDelegate pImGuiStorage_SetFloat = lib.LoadFunction<ImGuiStorage_SetFloatDelegate>("ImGuiStorage_SetFloat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_SetFloat(ImGuiStorage* self, uint key, float val) => pImGuiStorage_SetFloat(self, key, val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igColorConvertRGBtoHSVDelegate(float r, float g, float b, float* out_h, float* out_s, float* out_v);
        private readonly igColorConvertRGBtoHSVDelegate pigColorConvertRGBtoHSV = lib.LoadFunction<igColorConvertRGBtoHSVDelegate>("igColorConvertRGBtoHSV");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igColorConvertRGBtoHSV(float r, float g, float b, float* out_h, float* out_s, float* out_v) => pigColorConvertRGBtoHSV(r, g, b, out_h, out_s, out_v);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginMenuBarDelegate();
        private readonly igBeginMenuBarDelegate pigBeginMenuBar = lib.LoadFunction<igBeginMenuBarDelegate>("igBeginMenuBar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginMenuBar() => pigBeginMenuBar();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsPopupOpenDelegate(byte* str_id);
        private readonly igIsPopupOpenDelegate pigIsPopupOpen = lib.LoadFunction<igIsPopupOpenDelegate>("igIsPopupOpen");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsPopupOpen(byte* str_id) => pigIsPopupOpen(str_id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemVisibleDelegate();
        private readonly igIsItemVisibleDelegate pigIsItemVisible = lib.LoadFunction<igIsItemVisibleDelegate>("igIsItemVisible");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemVisible() => pigIsItemVisible();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_CalcCustomRectUVDelegate(ImFontAtlas* self, CustomRect* rect, Vector2* out_uv_min, Vector2* out_uv_max);
        private readonly ImFontAtlas_CalcCustomRectUVDelegate pImFontAtlas_CalcCustomRectUV = lib.LoadFunction<ImFontAtlas_CalcCustomRectUVDelegate>("ImFontAtlas_CalcCustomRectUV");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_CalcCustomRectUV(ImFontAtlas* self, CustomRect* rect, Vector2* out_uv_min, Vector2* out_uv_max) => pImFontAtlas_CalcCustomRectUV(self, rect, out_uv_min, out_uv_max);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate CustomRect* ImFontAtlas_GetCustomRectByIndexDelegate(ImFontAtlas* self, int index);
        private readonly ImFontAtlas_GetCustomRectByIndexDelegate pImFontAtlas_GetCustomRectByIndex = lib.LoadFunction<ImFontAtlas_GetCustomRectByIndexDelegate>("ImFontAtlas_GetCustomRectByIndex");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed CustomRect* ImFontAtlas_GetCustomRectByIndex(ImFontAtlas* self, int index) => pImFontAtlas_GetCustomRectByIndex(self, index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_AddTextDelegate(GlyphRangesBuilder* self, byte* text, byte* text_end);
        private readonly GlyphRangesBuilder_AddTextDelegate pGlyphRangesBuilder_AddText = lib.LoadFunction<GlyphRangesBuilder_AddTextDelegate>("GlyphRangesBuilder_AddText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_AddText(GlyphRangesBuilder* self, byte* text, byte* text_end) => pGlyphRangesBuilder_AddText(self, text, text_end);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_UpdateTextureIDDelegate(ImDrawList* self);
        private readonly ImDrawList_UpdateTextureIDDelegate pImDrawList_UpdateTextureID = lib.LoadFunction<ImDrawList_UpdateTextureIDDelegate>("ImDrawList_UpdateTextureID");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_UpdateTextureID(ImDrawList* self) => pImDrawList_UpdateTextureID(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowSizeDelegate(Vector2 size, ImGuiCond cond);
        private readonly igSetNextWindowSizeDelegate pigSetNextWindowSize = lib.LoadFunction<igSetNextWindowSizeDelegate>("igSetNextWindowSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowSize(Vector2 size, ImGuiCond cond) => pigSetNextWindowSize(size, cond);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ImFontAtlas_AddCustomRectRegularDelegate(ImFontAtlas* self, uint id, int width, int height);
        private readonly ImFontAtlas_AddCustomRectRegularDelegate pImFontAtlas_AddCustomRectRegular = lib.LoadFunction<ImFontAtlas_AddCustomRectRegularDelegate>("ImFontAtlas_AddCustomRectRegular");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int ImFontAtlas_AddCustomRectRegular(ImFontAtlas* self, uint id, int width, int height) => pImFontAtlas_AddCustomRectRegular(self, id, width, height);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowCollapsedBoolDelegate(byte collapsed, ImGuiCond cond);
        private readonly igSetWindowCollapsedBoolDelegate pigSetWindowCollapsedBool = lib.LoadFunction<igSetWindowCollapsedBoolDelegate>("igSetWindowCollapsedBool");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowCollapsedBool(byte collapsed, ImGuiCond cond) => pigSetWindowCollapsedBool(collapsed, cond);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowCollapsedStrDelegate(byte* name, byte collapsed, ImGuiCond cond);
        private readonly igSetWindowCollapsedStrDelegate pigSetWindowCollapsedStr = lib.LoadFunction<igSetWindowCollapsedStrDelegate>("igSetWindowCollapsedStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowCollapsedStr(byte* name, byte collapsed, ImGuiCond cond) => pigSetWindowCollapsedStr(name, collapsed, cond);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetMouseDragDeltaDelegate(int button, float lock_threshold);
        private readonly igGetMouseDragDeltaDelegate pigGetMouseDragDelta = lib.LoadFunction<igGetMouseDragDeltaDelegate>("igGetMouseDragDelta");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetMouseDragDelta(int button, float lock_threshold) => pigGetMouseDragDelta(button, lock_threshold);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImGuiPayload* igAcceptDragDropPayloadDelegate(byte* type, ImGuiDragDropFlags flags);
        private readonly igAcceptDragDropPayloadDelegate pigAcceptDragDropPayload = lib.LoadFunction<igAcceptDragDropPayloadDelegate>("igAcceptDragDropPayload");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImGuiPayload* igAcceptDragDropPayload(byte* type, ImGuiDragDropFlags flags) => pigAcceptDragDropPayload(type, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginDragDropSourceDelegate(ImGuiDragDropFlags flags);
        private readonly igBeginDragDropSourceDelegate pigBeginDragDropSource = lib.LoadFunction<igBeginDragDropSourceDelegate>("igBeginDragDropSource");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginDragDropSource(ImGuiDragDropFlags flags) => pigBeginDragDropSource(flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte CustomRect_IsPackedDelegate(CustomRect* self);
        private readonly CustomRect_IsPackedDelegate pCustomRect_IsPacked = lib.LoadFunction<CustomRect_IsPackedDelegate>("CustomRect_IsPacked");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte CustomRect_IsPacked(CustomRect* self) => pCustomRect_IsPacked(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPlotLinesDelegate(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
        private readonly igPlotLinesDelegate pigPlotLines = lib.LoadFunction<igPlotLinesDelegate>("igPlotLines");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPlotLines(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride) => pigPlotLines(label, values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImFontAtlas_IsBuiltDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_IsBuiltDelegate pImFontAtlas_IsBuilt = lib.LoadFunction<ImFontAtlas_IsBuiltDelegate>("ImFontAtlas_IsBuilt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImFontAtlas_IsBuilt(ImFontAtlas* self) => pImFontAtlas_IsBuilt(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImVec2_ImVec2Delegate(Vector2* self);
        private readonly ImVec2_ImVec2Delegate pImVec2_ImVec2 = lib.LoadFunction<ImVec2_ImVec2Delegate>("ImVec2_ImVec2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImVec2_ImVec2(Vector2* self) => pImVec2_ImVec2(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImVec2_ImVec2FloatDelegate(Vector2* self, float _x, float _y);
        private readonly ImVec2_ImVec2FloatDelegate pImVec2_ImVec2Float = lib.LoadFunction<ImVec2_ImVec2FloatDelegate>("ImVec2_ImVec2Float");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImVec2_ImVec2Float(Vector2* self, float _x, float _y) => pImVec2_ImVec2Float(self, _x, _y);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiPayload_ImGuiPayloadDelegate(ImGuiPayload* self);
        private readonly ImGuiPayload_ImGuiPayloadDelegate pImGuiPayload_ImGuiPayload = lib.LoadFunction<ImGuiPayload_ImGuiPayloadDelegate>("ImGuiPayload_ImGuiPayload");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiPayload_ImGuiPayload(ImGuiPayload* self) => pImGuiPayload_ImGuiPayload(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ClearDelegate(ImDrawList* self);
        private readonly ImDrawList_ClearDelegate pImDrawList_Clear = lib.LoadFunction<ImDrawList_ClearDelegate>("ImDrawList_Clear");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_Clear(ImDrawList* self) => pImDrawList_Clear(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_AddRangesDelegate(GlyphRangesBuilder* self, ushort* ranges);
        private readonly GlyphRangesBuilder_AddRangesDelegate pGlyphRangesBuilder_AddRanges = lib.LoadFunction<GlyphRangesBuilder_AddRangesDelegate>("GlyphRangesBuilder_AddRanges");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_AddRanges(GlyphRangesBuilder* self, ushort* ranges) => pGlyphRangesBuilder_AddRanges(self, ranges);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int igGetFrameCountDelegate();
        private readonly igGetFrameCountDelegate pigGetFrameCount = lib.LoadFunction<igGetFrameCountDelegate>("igGetFrameCount");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int igGetFrameCount() => pigGetFrameCount();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImFont_GetDebugNameDelegate(ImFont* self);
        private readonly ImFont_GetDebugNameDelegate pImFont_GetDebugName = lib.LoadFunction<ImFont_GetDebugNameDelegate>("ImFont_GetDebugName");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImFont_GetDebugName(ImFont* self) => pImFont_GetDebugName(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igListBoxFooterDelegate();
        private readonly igListBoxFooterDelegate pigListBoxFooter = lib.LoadFunction<igListBoxFooterDelegate>("igListBoxFooter");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igListBoxFooter() => pigListBoxFooter();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopClipRectDelegate();
        private readonly igPopClipRectDelegate pigPopClipRect = lib.LoadFunction<igPopClipRectDelegate>("igPopClipRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopClipRect() => pigPopClipRect();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddBezierCurveDelegate(ImDrawList* self, Vector2 pos0, Vector2 cp0, Vector2 cp1, Vector2 pos1, uint col, float thickness, int num_segments);
        private readonly ImDrawList_AddBezierCurveDelegate pImDrawList_AddBezierCurve = lib.LoadFunction<ImDrawList_AddBezierCurveDelegate>("ImDrawList_AddBezierCurve");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddBezierCurve(ImDrawList* self, Vector2 pos0, Vector2 cp0, Vector2 cp1, Vector2 pos1, uint col, float thickness, int num_segments) => pImDrawList_AddBezierCurve(self, pos0, cp0, cp1, pos1, col, thickness, num_segments);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_GlyphRangesBuilderDelegate(GlyphRangesBuilder* self);
        private readonly GlyphRangesBuilder_GlyphRangesBuilderDelegate pGlyphRangesBuilder_GlyphRangesBuilder = lib.LoadFunction<GlyphRangesBuilder_GlyphRangesBuilderDelegate>("GlyphRangesBuilder_GlyphRangesBuilder");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_GlyphRangesBuilder(GlyphRangesBuilder* self) => pGlyphRangesBuilder_GlyphRangesBuilder(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetWindowSizeDelegate();
        private readonly igGetWindowSizeDelegate pigGetWindowSize = lib.LoadFunction<igGetWindowSizeDelegate>("igGetWindowSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetWindowSize() => pigGetWindowSize();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesThaiDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_GetGlyphRangesThaiDelegate pImFontAtlas_GetGlyphRangesThai = lib.LoadFunction<ImFontAtlas_GetGlyphRangesThaiDelegate>("ImFontAtlas_GetGlyphRangesThai");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesThai(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesThai(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igCheckboxFlagsDelegate(byte* label, uint* flags, uint flags_value);
        private readonly igCheckboxFlagsDelegate pigCheckboxFlags = lib.LoadFunction<igCheckboxFlagsDelegate>("igCheckboxFlags");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igCheckboxFlags(byte* label, uint* flags, uint flags_value) => pigCheckboxFlags(label, flags, flags_value);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesCyrillicDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_GetGlyphRangesCyrillicDelegate pImFontAtlas_GetGlyphRangesCyrillic = lib.LoadFunction<ImFontAtlas_GetGlyphRangesCyrillicDelegate>("ImFontAtlas_GetGlyphRangesCyrillic");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesCyrillic(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesCyrillic(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsWindowHoveredDelegate(ImGuiHoveredFlags flags);
        private readonly igIsWindowHoveredDelegate pigIsWindowHovered = lib.LoadFunction<igIsWindowHoveredDelegate>("igIsWindowHovered");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsWindowHovered(ImGuiHoveredFlags flags) => pigIsWindowHovered(flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommonDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_GetGlyphRangesChineseSimplifiedCommonDelegate pImFontAtlas_GetGlyphRangesChineseSimplifiedCommon = lib.LoadFunction<ImFontAtlas_GetGlyphRangesChineseSimplifiedCommonDelegate>("ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPlotHistogramFloatPtrDelegate(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
        private readonly igPlotHistogramFloatPtrDelegate pigPlotHistogramFloatPtr = lib.LoadFunction<igPlotHistogramFloatPtrDelegate>("igPlotHistogramFloatPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPlotHistogramFloatPtr(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride) => pigPlotHistogramFloatPtr(label, values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginPopupContextVoidDelegate(byte* str_id, int mouse_button);
        private readonly igBeginPopupContextVoidDelegate pigBeginPopupContextVoid = lib.LoadFunction<igBeginPopupContextVoidDelegate>("igBeginPopupContextVoid");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginPopupContextVoid(byte* str_id, int mouse_button) => pigBeginPopupContextVoid(str_id, mouse_button);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesChineseFullDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_GetGlyphRangesChineseFullDelegate pImFontAtlas_GetGlyphRangesChineseFull = lib.LoadFunction<ImFontAtlas_GetGlyphRangesChineseFullDelegate>("ImFontAtlas_GetGlyphRangesChineseFull");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesChineseFull(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesChineseFull(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igShowStyleEditorDelegate(ImGuiStyle* @ref);
        private readonly igShowStyleEditorDelegate pigShowStyleEditor = lib.LoadFunction<igShowStyleEditorDelegate>("igShowStyleEditor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igShowStyleEditor(ImGuiStyle* @ref) => pigShowStyleEditor(@ref);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igCheckboxDelegate(byte* label, byte* v);
        private readonly igCheckboxDelegate pigCheckbox = lib.LoadFunction<igCheckboxDelegate>("igCheckbox");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igCheckbox(byte* label, byte* v) => pigCheckbox(label, v);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetWindowPosDelegate();
        private readonly igGetWindowPosDelegate pigGetWindowPos = lib.LoadFunction<igGetWindowPosDelegate>("igGetWindowPos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetWindowPos() => pigGetWindowPos();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiInputTextCallbackData_ImGuiInputTextCallbackDataDelegate(ImGuiInputTextCallbackData* self);
        private readonly ImGuiInputTextCallbackData_ImGuiInputTextCallbackDataDelegate pImGuiInputTextCallbackData_ImGuiInputTextCallbackData = lib.LoadFunction<ImGuiInputTextCallbackData_ImGuiInputTextCallbackDataDelegate>("ImGuiInputTextCallbackData_ImGuiInputTextCallbackData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiInputTextCallbackData_ImGuiInputTextCallbackData(ImGuiInputTextCallbackData* self) => pImGuiInputTextCallbackData_ImGuiInputTextCallbackData(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowContentSizeDelegate(Vector2 size);
        private readonly igSetNextWindowContentSizeDelegate pigSetNextWindowContentSize = lib.LoadFunction<igSetNextWindowContentSizeDelegate>("igSetNextWindowContentSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowContentSize(Vector2 size) => pigSetNextWindowContentSize(size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTextColoredDelegate(Vector4 col, byte* fmt);
        private readonly igTextColoredDelegate pigTextColored = lib.LoadFunction<igTextColoredDelegate>("igTextColored");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTextColored(Vector4 col, byte* fmt) => pigTextColored(col, fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogToFileDelegate(int max_depth, byte* filename);
        private readonly igLogToFileDelegate pigLogToFile = lib.LoadFunction<igLogToFileDelegate>("igLogToFile");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogToFile(int max_depth, byte* filename) => pigLogToFile(max_depth, filename);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igButtonDelegate(byte* label, Vector2 size);
        private readonly igButtonDelegate pigButton = lib.LoadFunction<igButtonDelegate>("igButton");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igButton(byte* label, Vector2 size) => pigButton(label, size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemEditedDelegate();
        private readonly igIsItemEditedDelegate pigIsItemEdited = lib.LoadFunction<igIsItemEditedDelegate>("igIsItemEdited");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemEdited() => pigIsItemEdited();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PushTextureIDDelegate(ImDrawList* self, IntPtr texture_id);
        private readonly ImDrawList_PushTextureIDDelegate pImDrawList_PushTextureID = lib.LoadFunction<ImDrawList_PushTextureIDDelegate>("ImDrawList_PushTextureID");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PushTextureID(ImDrawList* self, IntPtr texture_id) => pImDrawList_PushTextureID(self, texture_id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTreeAdvanceToLabelPosDelegate();
        private readonly igTreeAdvanceToLabelPosDelegate pigTreeAdvanceToLabelPos = lib.LoadFunction<igTreeAdvanceToLabelPosDelegate>("igTreeAdvanceToLabelPos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTreeAdvanceToLabelPos() => pigTreeAdvanceToLabelPos();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiInputTextCallbackData_DeleteCharsDelegate(ImGuiInputTextCallbackData* self, int pos, int bytes_count);
        private readonly ImGuiInputTextCallbackData_DeleteCharsDelegate pImGuiInputTextCallbackData_DeleteChars = lib.LoadFunction<ImGuiInputTextCallbackData_DeleteCharsDelegate>("ImGuiInputTextCallbackData_DeleteChars");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiInputTextCallbackData_DeleteChars(ImGuiInputTextCallbackData* self, int pos, int bytes_count) => pImGuiInputTextCallbackData_DeleteChars(self, pos, bytes_count);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragInt2Delegate(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        private readonly igDragInt2Delegate pigDragInt2 = lib.LoadFunction<igDragInt2Delegate>("igDragInt2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragInt2(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => pigDragInt2(label, v, v_speed, v_min, v_max, format);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesDefaultDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_GetGlyphRangesDefaultDelegate pImFontAtlas_GetGlyphRangesDefault = lib.LoadFunction<ImFontAtlas_GetGlyphRangesDefaultDelegate>("ImFontAtlas_GetGlyphRangesDefault");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesDefault(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesDefault(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsAnyItemActiveDelegate();
        private readonly igIsAnyItemActiveDelegate pigIsAnyItemActive = lib.LoadFunction<igIsAnyItemActiveDelegate>("igIsAnyItemActive");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsAnyItemActive() => pigIsAnyItemActive();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_SetTexIDDelegate(ImFontAtlas* self, IntPtr id);
        private readonly ImFontAtlas_SetTexIDDelegate pImFontAtlas_SetTexID = lib.LoadFunction<ImFontAtlas_SetTexIDDelegate>("ImFontAtlas_SetTexID");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_SetTexID(ImFontAtlas* self, IntPtr id) => pImFontAtlas_SetTexID(self, id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igMenuItemBoolDelegate(byte* label, byte* shortcut, byte selected, byte enabled);
        private readonly igMenuItemBoolDelegate pigMenuItemBool = lib.LoadFunction<igMenuItemBoolDelegate>("igMenuItemBool");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igMenuItemBool(byte* label, byte* shortcut, byte selected, byte enabled) => pigMenuItemBool(label, shortcut, selected, enabled);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igMenuItemBoolPtrDelegate(byte* label, byte* shortcut, byte* p_selected, byte enabled);
        private readonly igMenuItemBoolPtrDelegate pigMenuItemBoolPtr = lib.LoadFunction<igMenuItemBoolPtrDelegate>("igMenuItemBoolPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igMenuItemBoolPtr(byte* label, byte* shortcut, byte* p_selected, byte enabled) => pigMenuItemBoolPtr(label, shortcut, p_selected, enabled);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderFloat4Delegate(byte* label, Vector4* v, float v_min, float v_max, byte* format, float power);
        private readonly igSliderFloat4Delegate pigSliderFloat4 = lib.LoadFunction<igSliderFloat4Delegate>("igSliderFloat4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderFloat4(byte* label, Vector4* v, float v_min, float v_max, byte* format, float power) => pigSliderFloat4(label, v, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetCursorPosXDelegate();
        private readonly igGetCursorPosXDelegate pigGetCursorPosX = lib.LoadFunction<igGetCursorPosXDelegate>("igGetCursorPosX");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetCursorPosX() => pigGetCursorPosX();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_ClearTexDataDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_ClearTexDataDelegate pImFontAtlas_ClearTexData = lib.LoadFunction<ImFontAtlas_ClearTexDataDelegate>("ImFontAtlas_ClearTexData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_ClearTexData(ImFontAtlas* self) => pImFontAtlas_ClearTexData(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_ClearFontsDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_ClearFontsDelegate pImFontAtlas_ClearFonts = lib.LoadFunction<ImFontAtlas_ClearFontsDelegate>("ImFontAtlas_ClearFonts");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_ClearFonts(ImFontAtlas* self) => pImFontAtlas_ClearFonts(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int igGetColumnsCountDelegate();
        private readonly igGetColumnsCountDelegate pigGetColumnsCount = lib.LoadFunction<igGetColumnsCountDelegate>("igGetColumnsCount");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int igGetColumnsCount() => pigGetColumnsCount();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopButtonRepeatDelegate();
        private readonly igPopButtonRepeatDelegate pigPopButtonRepeat = lib.LoadFunction<igPopButtonRepeatDelegate>("igPopButtonRepeat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopButtonRepeat() => pigPopButtonRepeat();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragScalarNDelegate(byte* label, ImGuiDataType data_type, void* v, int components, float v_speed, void* v_min, void* v_max, byte* format, float power);
        private readonly igDragScalarNDelegate pigDragScalarN = lib.LoadFunction<igDragScalarNDelegate>("igDragScalarN");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragScalarN(byte* label, ImGuiDataType data_type, void* v, int components, float v_speed, void* v_min, void* v_max, byte* format, float power) => pigDragScalarN(label, data_type, v, components, v_speed, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiPayload_IsPreviewDelegate(ImGuiPayload* self);
        private readonly ImGuiPayload_IsPreviewDelegate pImGuiPayload_IsPreview = lib.LoadFunction<ImGuiPayload_IsPreviewDelegate>("ImGuiPayload_IsPreview");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiPayload_IsPreview(ImGuiPayload* self) => pImGuiPayload_IsPreview(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSpacingDelegate();
        private readonly igSpacingDelegate pigSpacing = lib.LoadFunction<igSpacingDelegate>("igSpacing");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSpacing() => pigSpacing();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_ClearDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_ClearDelegate pImFontAtlas_Clear = lib.LoadFunction<ImFontAtlas_ClearDelegate>("ImFontAtlas_Clear");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_Clear(ImFontAtlas* self) => pImFontAtlas_Clear(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsAnyItemFocusedDelegate();
        private readonly igIsAnyItemFocusedDelegate pigIsAnyItemFocused = lib.LoadFunction<igIsAnyItemFocusedDelegate>("igIsAnyItemFocused");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsAnyItemFocused() => pigIsAnyItemFocused();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddRectFilledDelegate(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags);
        private readonly ImDrawList_AddRectFilledDelegate pImDrawList_AddRectFilled = lib.LoadFunction<ImDrawList_AddRectFilledDelegate>("ImDrawList_AddRectFilled");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddRectFilled(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags) => pImDrawList_AddRectFilled(self, a, b, col, rounding, rounding_corners_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontFromMemoryCompressedTTFDelegate(ImFontAtlas* self, void* compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        private readonly ImFontAtlas_AddFontFromMemoryCompressedTTFDelegate pImFontAtlas_AddFontFromMemoryCompressedTTF = lib.LoadFunction<ImFontAtlas_AddFontFromMemoryCompressedTTFDelegate>("ImFontAtlas_AddFontFromMemoryCompressedTTF");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFontFromMemoryCompressedTTF(ImFontAtlas* self, void* compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => pImFontAtlas_AddFontFromMemoryCompressedTTF(self, compressed_font_data, compressed_font_size, size_pixels, font_cfg, glyph_ranges);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igMemFreeDelegate(void* ptr);
        private readonly igMemFreeDelegate pigMemFree = lib.LoadFunction<igMemFreeDelegate>("igMemFree");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igMemFree(void* ptr) => pigMemFree(ptr);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetFontTexUvWhitePixelDelegate();
        private readonly igGetFontTexUvWhitePixelDelegate pigGetFontTexUvWhitePixel = lib.LoadFunction<igGetFontTexUvWhitePixelDelegate>("igGetFontTexUvWhitePixel");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetFontTexUvWhitePixel() => pigGetFontTexUvWhitePixel();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddDrawCmdDelegate(ImDrawList* self);
        private readonly ImDrawList_AddDrawCmdDelegate pImDrawList_AddDrawCmd = lib.LoadFunction<ImDrawList_AddDrawCmdDelegate>("ImDrawList_AddDrawCmd");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddDrawCmd(ImDrawList* self) => pImDrawList_AddDrawCmd(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemClickedDelegate(int mouse_button);
        private readonly igIsItemClickedDelegate pigIsItemClicked = lib.LoadFunction<igIsItemClickedDelegate>("igIsItemClicked");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemClicked(int mouse_button) => pigIsItemClicked(mouse_button);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontFromMemoryTTFDelegate(ImFontAtlas* self, void* font_data, int font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        private readonly ImFontAtlas_AddFontFromMemoryTTFDelegate pImFontAtlas_AddFontFromMemoryTTF = lib.LoadFunction<ImFontAtlas_AddFontFromMemoryTTFDelegate>("ImFontAtlas_AddFontFromMemoryTTF");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFontFromMemoryTTF(ImFontAtlas* self, void* font_data, int font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => pImFontAtlas_AddFontFromMemoryTTF(self, font_data, font_size, size_pixels, font_cfg, glyph_ranges);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontFromFileTTFDelegate(ImFontAtlas* self, byte* filename, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        private readonly ImFontAtlas_AddFontFromFileTTFDelegate pImFontAtlas_AddFontFromFileTTF = lib.LoadFunction<ImFontAtlas_AddFontFromFileTTFDelegate>("ImFontAtlas_AddFontFromFileTTF");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFontFromFileTTF(ImFontAtlas* self, byte* filename, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => pImFontAtlas_AddFontFromFileTTF(self, filename, size_pixels, font_cfg, glyph_ranges);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igProgressBarDelegate(float fraction, Vector2 size_arg, byte* overlay);
        private readonly igProgressBarDelegate pigProgressBar = lib.LoadFunction<igProgressBarDelegate>("igProgressBar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igProgressBar(float fraction, Vector2 size_arg, byte* overlay) => pigProgressBar(fraction, size_arg, overlay);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontDefaultDelegate(ImFontAtlas* self, ImFontConfig* font_cfg);
        private readonly ImFontAtlas_AddFontDefaultDelegate pImFontAtlas_AddFontDefault = lib.LoadFunction<ImFontAtlas_AddFontDefaultDelegate>("ImFontAtlas_AddFontDefault");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* ImFontAtlas_AddFontDefault(ImFontAtlas* self, ImFontConfig* font_cfg) => pImFontAtlas_AddFontDefault(self, font_cfg);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowBgAlphaDelegate(float alpha);
        private readonly igSetNextWindowBgAlphaDelegate pigSetNextWindowBgAlpha = lib.LoadFunction<igSetNextWindowBgAlphaDelegate>("igSetNextWindowBgAlpha");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowBgAlpha(float alpha) => pigSetNextWindowBgAlpha(alpha);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginPopupDelegate(byte* str_id, ImGuiWindowFlags flags);
        private readonly igBeginPopupDelegate pigBeginPopup = lib.LoadFunction<igBeginPopupDelegate>("igBeginPopup");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginPopup(byte* str_id, ImGuiWindowFlags flags) => pigBeginPopup(str_id, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_BuildLookupTableDelegate(ImFont* self);
        private readonly ImFont_BuildLookupTableDelegate pImFont_BuildLookupTable = lib.LoadFunction<ImFont_BuildLookupTableDelegate>("ImFont_BuildLookupTable");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_BuildLookupTable(ImFont* self) => pImFont_BuildLookupTable(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetScrollXDelegate();
        private readonly igGetScrollXDelegate pigGetScrollX = lib.LoadFunction<igGetScrollXDelegate>("igGetScrollX");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetScrollX() => pigGetScrollX();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int igGetKeyIndexDelegate(ImGuiKey imgui_key);
        private readonly igGetKeyIndexDelegate pigGetKeyIndex = lib.LoadFunction<igGetKeyIndexDelegate>("igGetKeyIndex");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int igGetKeyIndex(ImGuiKey imgui_key) => pigGetKeyIndex(imgui_key);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImDrawList* igGetOverlayDrawListDelegate();
        private readonly igGetOverlayDrawListDelegate pigGetOverlayDrawList = lib.LoadFunction<igGetOverlayDrawListDelegate>("igGetOverlayDrawList");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImDrawList* igGetOverlayDrawList() => pigGetOverlayDrawList();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetIDStrDelegate(byte* str_id);
        private readonly igGetIDStrDelegate pigGetIDStr = lib.LoadFunction<igGetIDStrDelegate>("igGetIDStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetIDStr(byte* str_id) => pigGetIDStr(str_id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetIDRangeDelegate(byte* str_id_begin, byte* str_id_end);
        private readonly igGetIDRangeDelegate pigGetIDRange = lib.LoadFunction<igGetIDRangeDelegate>("igGetIDRange");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetIDRange(byte* str_id_begin, byte* str_id_end) => pigGetIDRange(str_id_begin, str_id_end);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetIDPtrDelegate(void* ptr_id);
        private readonly igGetIDPtrDelegate pigGetIDPtr = lib.LoadFunction<igGetIDPtrDelegate>("igGetIDPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetIDPtr(void* ptr_id) => pigGetIDPtr(ptr_id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesJapaneseDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_GetGlyphRangesJapaneseDelegate pImFontAtlas_GetGlyphRangesJapanese = lib.LoadFunction<ImFontAtlas_GetGlyphRangesJapaneseDelegate>("ImFontAtlas_GetGlyphRangesJapanese");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesJapanese(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesJapanese(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igListBoxHeaderVec2Delegate(byte* label, Vector2 size);
        private readonly igListBoxHeaderVec2Delegate pigListBoxHeaderVec2 = lib.LoadFunction<igListBoxHeaderVec2Delegate>("igListBoxHeaderVec2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igListBoxHeaderVec2(byte* label, Vector2 size) => pigListBoxHeaderVec2(label, size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igListBoxHeaderIntDelegate(byte* label, int items_count, int height_in_items);
        private readonly igListBoxHeaderIntDelegate pigListBoxHeaderInt = lib.LoadFunction<igListBoxHeaderIntDelegate>("igListBoxHeaderInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igListBoxHeaderInt(byte* label, int items_count, int height_in_items) => pigListBoxHeaderInt(label, items_count, height_in_items);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontConfig_ImFontConfigDelegate(ImFontConfig* self);
        private readonly ImFontConfig_ImFontConfigDelegate pImFontConfig_ImFontConfig = lib.LoadFunction<ImFontConfig_ImFontConfigDelegate>("ImFontConfig_ImFontConfig");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontConfig_ImFontConfig(ImFontConfig* self) => pImFontConfig_ImFontConfig(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseReleasedDelegate(int button);
        private readonly igIsMouseReleasedDelegate pigIsMouseReleased = lib.LoadFunction<igIsMouseReleasedDelegate>("igIsMouseReleased");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseReleased(int button) => pigIsMouseReleased(button);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawData_ScaleClipRectsDelegate(ImDrawData* self, Vector2 sc);
        private readonly ImDrawData_ScaleClipRectsDelegate pImDrawData_ScaleClipRects = lib.LoadFunction<ImDrawData_ScaleClipRectsDelegate>("ImDrawData_ScaleClipRects");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawData_ScaleClipRects(ImDrawData* self, Vector2 sc) => pImDrawData_ScaleClipRects(self, sc);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetItemRectMinDelegate();
        private readonly igGetItemRectMinDelegate pigGetItemRectMin = lib.LoadFunction<igGetItemRectMinDelegate>("igGetItemRectMin");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetItemRectMin() => pigGetItemRectMin();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawData_DeIndexAllBuffersDelegate(ImDrawData* self);
        private readonly ImDrawData_DeIndexAllBuffersDelegate pImDrawData_DeIndexAllBuffers = lib.LoadFunction<ImDrawData_DeIndexAllBuffersDelegate>("ImDrawData_DeIndexAllBuffers");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawData_DeIndexAllBuffers(ImDrawData* self) => pImDrawData_DeIndexAllBuffers(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogTextDelegate(byte* fmt);
        private readonly igLogTextDelegate pigLogText = lib.LoadFunction<igLogTextDelegate>("igLogText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogText(byte* fmt) => pigLogText(fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawData_ClearDelegate(ImDrawData* self);
        private readonly ImDrawData_ClearDelegate pImDrawData_Clear = lib.LoadFunction<ImDrawData_ClearDelegate>("ImDrawData_Clear");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawData_Clear(ImDrawData* self) => pImDrawData_Clear(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void* ImGuiStorage_GetVoidPtrDelegate(ImGuiStorage* self, uint key);
        private readonly ImGuiStorage_GetVoidPtrDelegate pImGuiStorage_GetVoidPtr = lib.LoadFunction<ImGuiStorage_GetVoidPtrDelegate>("ImGuiStorage_GetVoidPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void* ImGuiStorage_GetVoidPtr(ImGuiStorage* self, uint key) => pImGuiStorage_GetVoidPtr(self, key);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTextWrappedDelegate(byte* fmt);
        private readonly igTextWrappedDelegate pigTextWrapped = lib.LoadFunction<igTextWrappedDelegate>("igTextWrapped");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTextWrapped(byte* fmt) => pigTextWrapped(fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_UpdateClipRectDelegate(ImDrawList* self);
        private readonly ImDrawList_UpdateClipRectDelegate pImDrawList_UpdateClipRect = lib.LoadFunction<ImDrawList_UpdateClipRectDelegate>("ImDrawList_UpdateClipRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_UpdateClipRect(ImDrawList* self) => pImDrawList_UpdateClipRect(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimVtxDelegate(ImDrawList* self, Vector2 pos, Vector2 uv, uint col);
        private readonly ImDrawList_PrimVtxDelegate pImDrawList_PrimVtx = lib.LoadFunction<ImDrawList_PrimVtxDelegate>("ImDrawList_PrimVtx");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col) => pImDrawList_PrimVtx(self, pos, uv, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndGroupDelegate();
        private readonly igEndGroupDelegate pigEndGroup = lib.LoadFunction<igEndGroupDelegate>("igEndGroup");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndGroup() => pigEndGroup();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* igGetFontDelegate();
        private readonly igGetFontDelegate pigGetFont = lib.LoadFunction<igGetFontDelegate>("igGetFont");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImFont* igGetFont() => pigGetFont();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTreePushStrDelegate(byte* str_id);
        private readonly igTreePushStrDelegate pigTreePushStr = lib.LoadFunction<igTreePushStrDelegate>("igTreePushStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTreePushStr(byte* str_id) => pigTreePushStr(str_id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTreePushPtrDelegate(void* ptr_id);
        private readonly igTreePushPtrDelegate pigTreePushPtr = lib.LoadFunction<igTreePushPtrDelegate>("igTreePushPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTreePushPtr(void* ptr_id) => pigTreePushPtr(ptr_id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTextDisabledDelegate(byte* fmt);
        private readonly igTextDisabledDelegate pigTextDisabled = lib.LoadFunction<igTextDisabledDelegate>("igTextDisabled");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTextDisabled(byte* fmt) => pigTextDisabled(fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimRectDelegate(ImDrawList* self, Vector2 a, Vector2 b, uint col);
        private readonly ImDrawList_PrimRectDelegate pImDrawList_PrimRect = lib.LoadFunction<ImDrawList_PrimRectDelegate>("ImDrawList_PrimRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimRect(ImDrawList* self, Vector2 a, Vector2 b, uint col) => pImDrawList_PrimRect(self, a, b, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddQuadDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col, float thickness);
        private readonly ImDrawList_AddQuadDelegate pImDrawList_AddQuad = lib.LoadFunction<ImDrawList_AddQuadDelegate>("ImDrawList_AddQuad");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddQuad(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col, float thickness) => pImDrawList_AddQuad(self, a, b, c, d, col, thickness);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ClearFreeMemoryDelegate(ImDrawList* self);
        private readonly ImDrawList_ClearFreeMemoryDelegate pImDrawList_ClearFreeMemory = lib.LoadFunction<ImDrawList_ClearFreeMemoryDelegate>("ImDrawList_ClearFreeMemory");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_ClearFreeMemory(ImDrawList* self) => pImDrawList_ClearFreeMemory(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextTreeNodeOpenDelegate(byte is_open, ImGuiCond cond);
        private readonly igSetNextTreeNodeOpenDelegate pigSetNextTreeNodeOpen = lib.LoadFunction<igSetNextTreeNodeOpenDelegate>("igSetNextTreeNodeOpen");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextTreeNodeOpen(byte is_open, ImGuiCond cond) => pigSetNextTreeNodeOpen(is_open, cond);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogToTTYDelegate(int max_depth);
        private readonly igLogToTTYDelegate pigLogToTTY = lib.LoadFunction<igLogToTTYDelegate>("igLogToTTY");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogToTTY(int max_depth) => pigLogToTTY(max_depth);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_BuildRangesDelegate(GlyphRangesBuilder* self, ImVector* out_ranges);
        private readonly GlyphRangesBuilder_BuildRangesDelegate pGlyphRangesBuilder_BuildRanges = lib.LoadFunction<GlyphRangesBuilder_BuildRangesDelegate>("GlyphRangesBuilder_BuildRanges");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_BuildRanges(GlyphRangesBuilder* self, ImVector* out_ranges) => pGlyphRangesBuilder_BuildRanges(self, out_ranges);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImDrawList* ImDrawList_CloneOutputDelegate(ImDrawList* self);
        private readonly ImDrawList_CloneOutputDelegate pImDrawList_CloneOutput = lib.LoadFunction<ImDrawList_CloneOutputDelegate>("ImDrawList_CloneOutput");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImDrawList* ImDrawList_CloneOutput(ImDrawList* self) => pImDrawList_CloneOutput(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImGuiIO* igGetIODelegate();
        private readonly igGetIODelegate pigGetIO = lib.LoadFunction<igGetIODelegate>("igGetIO");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImGuiIO* igGetIO() => pigGetIO();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragInt4Delegate(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        private readonly igDragInt4Delegate pigDragInt4 = lib.LoadFunction<igDragInt4Delegate>("igDragInt4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragInt4(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => pigDragInt4(label, v, v_speed, v_min, v_max, format);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igNextColumnDelegate();
        private readonly igNextColumnDelegate pigNextColumn = lib.LoadFunction<igNextColumnDelegate>("igNextColumn");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igNextColumn() => pigNextColumn();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddRectDelegate(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags, float thickness);
        private readonly ImDrawList_AddRectDelegate pImDrawList_AddRect = lib.LoadFunction<ImDrawList_AddRectDelegate>("ImDrawList_AddRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddRect(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags, float thickness) => pImDrawList_AddRect(self, a, b, col, rounding, rounding_corners_flags, thickness);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TextRange_splitDelegate(TextRange* self, byte separator, ImVector* @out);
        private readonly TextRange_splitDelegate pTextRange_split = lib.LoadFunction<TextRange_splitDelegate>("TextRange_split");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void TextRange_split(TextRange* self, byte separator, ImVector* @out) => pTextRange_split(self, separator, @out);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetCursorPosDelegate(Vector2 local_pos);
        private readonly igSetCursorPosDelegate pigSetCursorPos = lib.LoadFunction<igSetCursorPosDelegate>("igSetCursorPos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetCursorPos(Vector2 local_pos) => pigSetCursorPos(local_pos);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginPopupModalDelegate(byte* name, byte* p_open, ImGuiWindowFlags flags);
        private readonly igBeginPopupModalDelegate pigBeginPopupModal = lib.LoadFunction<igBeginPopupModalDelegate>("igBeginPopupModal");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginPopupModal(byte* name, byte* p_open, ImGuiWindowFlags flags) => pigBeginPopupModal(name, p_open, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderInt4Delegate(byte* label, int* v, int v_min, int v_max, byte* format);
        private readonly igSliderInt4Delegate pigSliderInt4 = lib.LoadFunction<igSliderInt4Delegate>("igSliderInt4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderInt4(byte* label, int* v, int v_min, int v_max, byte* format) => pigSliderInt4(label, v, v_min, v_max, format);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddCallbackDelegate(ImDrawList* self, IntPtr callback, void* callback_data);
        private readonly ImDrawList_AddCallbackDelegate pImDrawList_AddCallback = lib.LoadFunction<ImDrawList_AddCallbackDelegate>("ImDrawList_AddCallback");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddCallback(ImDrawList* self, IntPtr callback, void* callback_data) => pImDrawList_AddCallback(self, callback, callback_data);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igShowMetricsWindowDelegate(byte* p_open);
        private readonly igShowMetricsWindowDelegate pigShowMetricsWindow = lib.LoadFunction<igShowMetricsWindowDelegate>("igShowMetricsWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igShowMetricsWindow(byte* p_open) => pigShowMetricsWindow(p_open);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetScrollMaxYDelegate();
        private readonly igGetScrollMaxYDelegate pigGetScrollMaxY = lib.LoadFunction<igGetScrollMaxYDelegate>("igGetScrollMaxY");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetScrollMaxY() => pigGetScrollMaxY();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igBeginTooltipDelegate();
        private readonly igBeginTooltipDelegate pigBeginTooltip = lib.LoadFunction<igBeginTooltipDelegate>("igBeginTooltip");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igBeginTooltip() => pigBeginTooltip();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetScrollXDelegate(float scroll_x);
        private readonly igSetScrollXDelegate pigSetScrollX = lib.LoadFunction<igSetScrollXDelegate>("igSetScrollX");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetScrollX(float scroll_x) => pigSetScrollX(scroll_x);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImDrawData* igGetDrawDataDelegate();
        private readonly igGetDrawDataDelegate pigGetDrawData = lib.LoadFunction<igGetDrawDataDelegate>("igGetDrawData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImDrawData* igGetDrawData() => pigGetDrawData();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetTextLineHeightDelegate();
        private readonly igGetTextLineHeightDelegate pigGetTextLineHeight = lib.LoadFunction<igGetTextLineHeightDelegate>("igGetTextLineHeight");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetTextLineHeight() => pigGetTextLineHeight();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSeparatorDelegate();
        private readonly igSeparatorDelegate pigSeparator = lib.LoadFunction<igSeparatorDelegate>("igSeparator");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSeparator() => pigSeparator();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginChildDelegate(byte* str_id, Vector2 size, byte border, ImGuiWindowFlags flags);
        private readonly igBeginChildDelegate pigBeginChild = lib.LoadFunction<igBeginChildDelegate>("igBeginChild");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginChild(byte* str_id, Vector2 size, byte border, ImGuiWindowFlags flags) => pigBeginChild(str_id, size, border, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginChildIDDelegate(uint id, Vector2 size, byte border, ImGuiWindowFlags flags);
        private readonly igBeginChildIDDelegate pigBeginChildID = lib.LoadFunction<igBeginChildIDDelegate>("igBeginChildID");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginChildID(uint id, Vector2 size, byte border, ImGuiWindowFlags flags) => pigBeginChildID(id, size, border, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathRectDelegate(ImDrawList* self, Vector2 rect_min, Vector2 rect_max, float rounding, int rounding_corners_flags);
        private readonly ImDrawList_PathRectDelegate pImDrawList_PathRect = lib.LoadFunction<ImDrawList_PathRectDelegate>("ImDrawList_PathRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathRect(ImDrawList* self, Vector2 rect_min, Vector2 rect_max, float rounding, int rounding_corners_flags) => pImDrawList_PathRect(self, rect_min, rect_max, rounding, rounding_corners_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseClickedDelegate(int button, byte repeat);
        private readonly igIsMouseClickedDelegate pigIsMouseClicked = lib.LoadFunction<igIsMouseClickedDelegate>("igIsMouseClicked");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseClicked(int button, byte repeat) => pigIsMouseClicked(button, repeat);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igCalcItemWidthDelegate();
        private readonly igCalcItemWidthDelegate pigCalcItemWidth = lib.LoadFunction<igCalcItemWidthDelegate>("igCalcItemWidth");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igCalcItemWidth() => pigCalcItemWidth();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathArcToFastDelegate(ImDrawList* self, Vector2 centre, float radius, int a_min_of_12, int a_max_of_12);
        private readonly ImDrawList_PathArcToFastDelegate pImDrawList_PathArcToFast = lib.LoadFunction<ImDrawList_PathArcToFastDelegate>("ImDrawList_PathArcToFast");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathArcToFast(ImDrawList* self, Vector2 centre, float radius, int a_min_of_12, int a_max_of_12) => pImDrawList_PathArcToFast(self, centre, radius, a_min_of_12, a_max_of_12);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndChildFrameDelegate();
        private readonly igEndChildFrameDelegate pigEndChildFrame = lib.LoadFunction<igEndChildFrameDelegate>("igEndChildFrame");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndChildFrame() => pigEndChildFrame();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igIndentDelegate(float indent_w);
        private readonly igIndentDelegate pigIndent = lib.LoadFunction<igIndentDelegate>("igIndent");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igIndent(float indent_w) => pigIndent(indent_w);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSetDragDropPayloadDelegate(byte* type, void* data, uint size, ImGuiCond cond);
        private readonly igSetDragDropPayloadDelegate pigSetDragDropPayload = lib.LoadFunction<igSetDragDropPayloadDelegate>("igSetDragDropPayload");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSetDragDropPayload(byte* type, void* data, uint size, ImGuiCond cond) => pigSetDragDropPayload(type, data, size, cond);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte GlyphRangesBuilder_GetBitDelegate(GlyphRangesBuilder* self, int n);
        private readonly GlyphRangesBuilder_GetBitDelegate pGlyphRangesBuilder_GetBit = lib.LoadFunction<GlyphRangesBuilder_GetBitDelegate>("GlyphRangesBuilder_GetBit");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte GlyphRangesBuilder_GetBit(GlyphRangesBuilder* self, int n) => pGlyphRangesBuilder_GetBit(self, n);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiTextFilter_DrawDelegate(ImGuiTextFilter* self, byte* label, float width);
        private readonly ImGuiTextFilter_DrawDelegate pImGuiTextFilter_Draw = lib.LoadFunction<ImGuiTextFilter_DrawDelegate>("ImGuiTextFilter_Draw");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiTextFilter_Draw(ImGuiTextFilter* self, byte* label, float width) => pImGuiTextFilter_Draw(self, label, width);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igShowDemoWindowDelegate(byte* p_open);
        private readonly igShowDemoWindowDelegate pigShowDemoWindow = lib.LoadFunction<igShowDemoWindowDelegate>("igShowDemoWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igShowDemoWindow(byte* p_open) => pigShowDemoWindow(p_open);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathStrokeDelegate(ImDrawList* self, uint col, byte closed, float thickness);
        private readonly ImDrawList_PathStrokeDelegate pImDrawList_PathStroke = lib.LoadFunction<ImDrawList_PathStrokeDelegate>("ImDrawList_PathStroke");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathStroke(ImDrawList* self, uint col, byte closed, float thickness) => pImDrawList_PathStroke(self, col, closed, thickness);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathFillConvexDelegate(ImDrawList* self, uint col);
        private readonly ImDrawList_PathFillConvexDelegate pImDrawList_PathFillConvex = lib.LoadFunction<ImDrawList_PathFillConvexDelegate>("ImDrawList_PathFillConvex");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathFillConvex(ImDrawList* self, uint col) => pImDrawList_PathFillConvex(self, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathLineToMergeDuplicateDelegate(ImDrawList* self, Vector2 pos);
        private readonly ImDrawList_PathLineToMergeDuplicateDelegate pImDrawList_PathLineToMergeDuplicate = lib.LoadFunction<ImDrawList_PathLineToMergeDuplicateDelegate>("ImDrawList_PathLineToMergeDuplicate");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathLineToMergeDuplicate(ImDrawList* self, Vector2 pos) => pImDrawList_PathLineToMergeDuplicate(self, pos);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndMenuDelegate();
        private readonly igEndMenuDelegate pigEndMenu = lib.LoadFunction<igEndMenuDelegate>("igEndMenu");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndMenu() => pigEndMenu();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igColorButtonDelegate(byte* desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size);
        private readonly igColorButtonDelegate pigColorButton = lib.LoadFunction<igColorButtonDelegate>("igColorButton");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igColorButton(byte* desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size) => pigColorButton(desc_id, col, flags, size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_GetTexDataAsAlpha8Delegate(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);
        private readonly ImFontAtlas_GetTexDataAsAlpha8Delegate pImFontAtlas_GetTexDataAsAlpha8 = lib.LoadFunction<ImFontAtlas_GetTexDataAsAlpha8Delegate>("ImFontAtlas_GetTexDataAsAlpha8");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_GetTexDataAsAlpha8(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel) => pImFontAtlas_GetTexDataAsAlpha8(self, out_pixels, out_width, out_height, out_bytes_per_pixel);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsKeyReleasedDelegate(int user_key_index);
        private readonly igIsKeyReleasedDelegate pigIsKeyReleased = lib.LoadFunction<igIsKeyReleasedDelegate>("igIsKeyReleased");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsKeyReleased(int user_key_index) => pigIsKeyReleased(user_key_index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetClipboardTextDelegate(byte* text);
        private readonly igSetClipboardTextDelegate pigSetClipboardText = lib.LoadFunction<igSetClipboardTextDelegate>("igSetClipboardText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetClipboardText(byte* text) => pigSetClipboardText(text);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathArcToDelegate(ImDrawList* self, Vector2 centre, float radius, float a_min, float a_max, int num_segments);
        private readonly ImDrawList_PathArcToDelegate pImDrawList_PathArcTo = lib.LoadFunction<ImDrawList_PathArcToDelegate>("ImDrawList_PathArcTo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathArcTo(ImDrawList* self, Vector2 centre, float radius, float a_min, float a_max, int num_segments) => pImDrawList_PathArcTo(self, centre, radius, a_min, a_max, num_segments);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddConvexPolyFilledDelegate(ImDrawList* self, Vector2* points, int num_points, uint col);
        private readonly ImDrawList_AddConvexPolyFilledDelegate pImDrawList_AddConvexPolyFilled = lib.LoadFunction<ImDrawList_AddConvexPolyFilledDelegate>("ImDrawList_AddConvexPolyFilled");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddConvexPolyFilled(ImDrawList* self, Vector2* points, int num_points, uint col) => pImDrawList_AddConvexPolyFilled(self, points, num_points, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsWindowCollapsedDelegate();
        private readonly igIsWindowCollapsedDelegate pigIsWindowCollapsed = lib.LoadFunction<igIsWindowCollapsedDelegate>("igIsWindowCollapsed");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsWindowCollapsed() => pigIsWindowCollapsed();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igShowFontSelectorDelegate(byte* label);
        private readonly igShowFontSelectorDelegate pigShowFontSelector = lib.LoadFunction<igShowFontSelectorDelegate>("igShowFontSelector");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igShowFontSelector(byte* label) => pigShowFontSelector(label);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddImageQuadDelegate(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);
        private readonly ImDrawList_AddImageQuadDelegate pImDrawList_AddImageQuad = lib.LoadFunction<ImDrawList_AddImageQuadDelegate>("ImDrawList_AddImageQuad");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddImageQuad(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col) => pImDrawList_AddImageQuad(self, user_texture_id, a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowFocusDelegate();
        private readonly igSetNextWindowFocusDelegate pigSetNextWindowFocus = lib.LoadFunction<igSetNextWindowFocusDelegate>("igSetNextWindowFocus");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowFocus() => pigSetNextWindowFocus();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSameLineDelegate(float pos_x, float spacing_w);
        private readonly igSameLineDelegate pigSameLine = lib.LoadFunction<igSameLineDelegate>("igSameLine");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSameLine(float pos_x, float spacing_w) => pigSameLine(pos_x, spacing_w);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginDelegate(byte* name, byte* p_open, ImGuiWindowFlags flags);
        private readonly igBeginDelegate pigBegin = lib.LoadFunction<igBeginDelegate>("igBegin");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBegin(byte* name, byte* p_open, ImGuiWindowFlags flags) => pigBegin(name, p_open, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igColorEdit3Delegate(byte* label, Vector3* col, ImGuiColorEditFlags flags);
        private readonly igColorEdit3Delegate pigColorEdit3 = lib.LoadFunction<igColorEdit3Delegate>("igColorEdit3");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igColorEdit3(byte* label, Vector3* col, ImGuiColorEditFlags flags) => pigColorEdit3(label, col, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddImageDelegate(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);
        private readonly ImDrawList_AddImageDelegate pImDrawList_AddImage = lib.LoadFunction<ImDrawList_AddImageDelegate>("ImDrawList_AddImage");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddImage(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col) => pImDrawList_AddImage(self, user_texture_id, a, b, uv_a, uv_b, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiIO_AddInputCharactersUTF8Delegate(ImGuiIO* self, byte* utf8_chars);
        private readonly ImGuiIO_AddInputCharactersUTF8Delegate pImGuiIO_AddInputCharactersUTF8 = lib.LoadFunction<ImGuiIO_AddInputCharactersUTF8Delegate>("ImGuiIO_AddInputCharactersUTF8");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiIO_AddInputCharactersUTF8(ImGuiIO* self, byte* utf8_chars) => pImGuiIO_AddInputCharactersUTF8(self, utf8_chars);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddTextDelegate(ImDrawList* self, Vector2 pos, uint col, byte* text_begin, byte* text_end);
        private readonly ImDrawList_AddTextDelegate pImDrawList_AddText = lib.LoadFunction<ImDrawList_AddTextDelegate>("ImDrawList_AddText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddText(ImDrawList* self, Vector2 pos, uint col, byte* text_begin, byte* text_end) => pImDrawList_AddText(self, pos, col, text_begin, text_end);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddTextFontPtrDelegate(ImDrawList* self, ImFont* font, float font_size, Vector2 pos, uint col, byte* text_begin, byte* text_end, float wrap_width, Vector4* cpu_fine_clip_rect);
        private readonly ImDrawList_AddTextFontPtrDelegate pImDrawList_AddTextFontPtr = lib.LoadFunction<ImDrawList_AddTextFontPtrDelegate>("ImDrawList_AddTextFontPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddTextFontPtr(ImDrawList* self, ImFont* font, float font_size, Vector2 pos, uint col, byte* text_begin, byte* text_end, float wrap_width, Vector4* cpu_fine_clip_rect) => pImDrawList_AddTextFontPtr(self, font, font_size, pos, col, text_begin, text_end, wrap_width, cpu_fine_clip_rect);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddCircleFilledDelegate(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments);
        private readonly ImDrawList_AddCircleFilledDelegate pImDrawList_AddCircleFilled = lib.LoadFunction<ImDrawList_AddCircleFilledDelegate>("ImDrawList_AddCircleFilled");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddCircleFilled(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments) => pImDrawList_AddCircleFilled(self, centre, radius, col, num_segments);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputFloat2Delegate(byte* label, Vector2* v, byte* format, ImGuiInputTextFlags extra_flags);
        private readonly igInputFloat2Delegate pigInputFloat2 = lib.LoadFunction<igInputFloat2Delegate>("igInputFloat2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputFloat2(byte* label, Vector2* v, byte* format, ImGuiInputTextFlags extra_flags) => pigInputFloat2(label, v, format, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushButtonRepeatDelegate(byte repeat);
        private readonly igPushButtonRepeatDelegate pigPushButtonRepeat = lib.LoadFunction<igPushButtonRepeatDelegate>("igPushButtonRepeat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushButtonRepeat(byte repeat) => pigPushButtonRepeat(repeat);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopItemWidthDelegate();
        private readonly igPopItemWidthDelegate pigPopItemWidth = lib.LoadFunction<igPopItemWidthDelegate>("igPopItemWidth");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopItemWidth() => pigPopItemWidth();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddCircleDelegate(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments, float thickness);
        private readonly ImDrawList_AddCircleDelegate pImDrawList_AddCircle = lib.LoadFunction<ImDrawList_AddCircleDelegate>("ImDrawList_AddCircle");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddCircle(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments, float thickness) => pImDrawList_AddCircle(self, centre, radius, col, num_segments, thickness);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddTriangleFilledDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col);
        private readonly ImDrawList_AddTriangleFilledDelegate pImDrawList_AddTriangleFilled = lib.LoadFunction<ImDrawList_AddTriangleFilledDelegate>("ImDrawList_AddTriangleFilled");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddTriangleFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col) => pImDrawList_AddTriangleFilled(self, a, b, c, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddTriangleDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col, float thickness);
        private readonly ImDrawList_AddTriangleDelegate pImDrawList_AddTriangle = lib.LoadFunction<ImDrawList_AddTriangleDelegate>("ImDrawList_AddTriangle");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddTriangle(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col, float thickness) => pImDrawList_AddTriangle(self, a, b, c, col, thickness);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddQuadFilledDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col);
        private readonly ImDrawList_AddQuadFilledDelegate pImDrawList_AddQuadFilled = lib.LoadFunction<ImDrawList_AddQuadFilledDelegate>("ImDrawList_AddQuadFilled");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddQuadFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col) => pImDrawList_AddQuadFilled(self, a, b, c, d, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetFontSizeDelegate();
        private readonly igGetFontSizeDelegate pigGetFontSize = lib.LoadFunction<igGetFontSizeDelegate>("igGetFontSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetFontSize() => pigGetFontSize();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputDoubleDelegate(byte* label, double* v, double step, double step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        private readonly igInputDoubleDelegate pigInputDouble = lib.LoadFunction<igInputDoubleDelegate>("igInputDouble");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputDouble(byte* label, double* v, double step, double step_fast, byte* format, ImGuiInputTextFlags extra_flags) => pigInputDouble(label, v, step, step_fast, format, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimReserveDelegate(ImDrawList* self, int idx_count, int vtx_count);
        private readonly ImDrawList_PrimReserveDelegate pImDrawList_PrimReserve = lib.LoadFunction<ImDrawList_PrimReserveDelegate>("ImDrawList_PrimReserve");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimReserve(ImDrawList* self, int idx_count, int vtx_count) => pImDrawList_PrimReserve(self, idx_count, vtx_count);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddRectFilledMultiColorDelegate(ImDrawList* self, Vector2 a, Vector2 b, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left);
        private readonly ImDrawList_AddRectFilledMultiColorDelegate pImDrawList_AddRectFilledMultiColor = lib.LoadFunction<ImDrawList_AddRectFilledMultiColorDelegate>("ImDrawList_AddRectFilledMultiColor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddRectFilledMultiColor(ImDrawList* self, Vector2 a, Vector2 b, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left) => pImDrawList_AddRectFilledMultiColor(self, a, b, col_upr_left, col_upr_right, col_bot_right, col_bot_left);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndPopupDelegate();
        private readonly igEndPopupDelegate pigEndPopup = lib.LoadFunction<igEndPopupDelegate>("igEndPopup");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndPopup() => pigEndPopup();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_ClearInputDataDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_ClearInputDataDelegate pImFontAtlas_ClearInputData = lib.LoadFunction<ImFontAtlas_ClearInputDataDelegate>("ImFontAtlas_ClearInputData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_ClearInputData(ImFontAtlas* self) => pImFontAtlas_ClearInputData(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddLineDelegate(ImDrawList* self, Vector2 a, Vector2 b, uint col, float thickness);
        private readonly ImDrawList_AddLineDelegate pImDrawList_AddLine = lib.LoadFunction<ImDrawList_AddLineDelegate>("ImDrawList_AddLine");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddLine(ImDrawList* self, Vector2 a, Vector2 b, uint col, float thickness) => pImDrawList_AddLine(self, a, b, col, thickness);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputTextMultilineDelegate(byte* label, byte* buf, uint buf_size, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);
        private readonly igInputTextMultilineDelegate pigInputTextMultiline = lib.LoadFunction<igInputTextMultilineDelegate>("igInputTextMultiline");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputTextMultiline(byte* label, byte* buf, uint buf_size, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data) => pigInputTextMultiline(label, buf, buf_size, size, flags, callback, user_data);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSelectableDelegate(byte* label, byte selected, ImGuiSelectableFlags flags, Vector2 size);
        private readonly igSelectableDelegate pigSelectable = lib.LoadFunction<igSelectableDelegate>("igSelectable");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSelectable(byte* label, byte selected, ImGuiSelectableFlags flags, Vector2 size) => pigSelectable(label, selected, flags, size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSelectableBoolPtrDelegate(byte* label, byte* p_selected, ImGuiSelectableFlags flags, Vector2 size);
        private readonly igSelectableBoolPtrDelegate pigSelectableBoolPtr = lib.LoadFunction<igSelectableBoolPtrDelegate>("igSelectableBoolPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSelectableBoolPtr(byte* label, byte* p_selected, ImGuiSelectableFlags flags, Vector2 size) => pigSelectableBoolPtr(label, p_selected, flags, size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igListBoxStr_arrDelegate(byte* label, int* current_item, byte** items, int items_count, int height_in_items);
        private readonly igListBoxStr_arrDelegate pigListBoxStr_arr = lib.LoadFunction<igListBoxStr_arrDelegate>("igListBoxStr_arr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igListBoxStr_arr(byte* label, int* current_item, byte** items, int items_count, int height_in_items) => pigListBoxStr_arr(label, current_item, items, items_count, height_in_items);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetCursorPosDelegate();
        private readonly igGetCursorPosDelegate pigGetCursorPos = lib.LoadFunction<igGetCursorPosDelegate>("igGetCursorPos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetCursorPos() => pigGetCursorPos();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 ImDrawList_GetClipRectMinDelegate(ImDrawList* self);
        private readonly ImDrawList_GetClipRectMinDelegate pImDrawList_GetClipRectMin = lib.LoadFunction<ImDrawList_GetClipRectMinDelegate>("ImDrawList_GetClipRectMin");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 ImDrawList_GetClipRectMin(ImDrawList* self) => pImDrawList_GetClipRectMin(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PopTextureIDDelegate(ImDrawList* self);
        private readonly ImDrawList_PopTextureIDDelegate pImDrawList_PopTextureID = lib.LoadFunction<ImDrawList_PopTextureIDDelegate>("ImDrawList_PopTextureID");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PopTextureID(ImDrawList* self) => pImDrawList_PopTextureID(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputFloat4Delegate(byte* label, Vector4* v, byte* format, ImGuiInputTextFlags extra_flags);
        private readonly igInputFloat4Delegate pigInputFloat4 = lib.LoadFunction<igInputFloat4Delegate>("igInputFloat4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputFloat4(byte* label, Vector4* v, byte* format, ImGuiInputTextFlags extra_flags) => pigInputFloat4(label, v, format, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetCursorPosYDelegate(float y);
        private readonly igSetCursorPosYDelegate pigSetCursorPosY = lib.LoadFunction<igSetCursorPosYDelegate>("igSetCursorPosY");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetCursorPosY(float y) => pigSetCursorPosY(y);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* igGetVersionDelegate();
        private readonly igGetVersionDelegate pigGetVersion = lib.LoadFunction<igGetVersionDelegate>("igGetVersion");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* igGetVersion() => pigGetVersion();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndComboDelegate();
        private readonly igEndComboDelegate pigEndCombo = lib.LoadFunction<igEndComboDelegate>("igEndCombo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndCombo() => pigEndCombo();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushIDStrDelegate(byte* str_id);
        private readonly igPushIDStrDelegate pigPushIDStr = lib.LoadFunction<igPushIDStrDelegate>("igPushIDStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushIDStr(byte* str_id) => pigPushIDStr(str_id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushIDRangeDelegate(byte* str_id_begin, byte* str_id_end);
        private readonly igPushIDRangeDelegate pigPushIDRange = lib.LoadFunction<igPushIDRangeDelegate>("igPushIDRange");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushIDRange(byte* str_id_begin, byte* str_id_end) => pigPushIDRange(str_id_begin, str_id_end);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushIDPtrDelegate(void* ptr_id);
        private readonly igPushIDPtrDelegate pigPushIDPtr = lib.LoadFunction<igPushIDPtrDelegate>("igPushIDPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushIDPtr(void* ptr_id) => pigPushIDPtr(ptr_id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushIDIntDelegate(int int_id);
        private readonly igPushIDIntDelegate pigPushIDInt = lib.LoadFunction<igPushIDIntDelegate>("igPushIDInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushIDInt(int int_id) => pigPushIDInt(int_id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ImDrawListDelegate(ImDrawList* self, IntPtr shared_data);
        private readonly ImDrawList_ImDrawListDelegate pImDrawList_ImDrawList = lib.LoadFunction<ImDrawList_ImDrawListDelegate>("ImDrawList_ImDrawList");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_ImDrawList(ImDrawList* self, IntPtr shared_data) => pImDrawList_ImDrawList(self, shared_data);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawCmd_ImDrawCmdDelegate(ImDrawCmd* self);
        private readonly ImDrawCmd_ImDrawCmdDelegate pImDrawCmd_ImDrawCmd = lib.LoadFunction<ImDrawCmd_ImDrawCmdDelegate>("ImDrawCmd_ImDrawCmd");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawCmd_ImDrawCmd(ImDrawCmd* self) => pImDrawCmd_ImDrawCmd(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiListClipper_EndDelegate(ImGuiListClipper* self);
        private readonly ImGuiListClipper_EndDelegate pImGuiListClipper_End = lib.LoadFunction<ImGuiListClipper_EndDelegate>("ImGuiListClipper_End");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiListClipper_End(ImGuiListClipper* self) => pImGuiListClipper_End(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igAlignTextToFramePaddingDelegate();
        private readonly igAlignTextToFramePaddingDelegate pigAlignTextToFramePadding = lib.LoadFunction<igAlignTextToFramePaddingDelegate>("igAlignTextToFramePadding");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igAlignTextToFramePadding() => pigAlignTextToFramePadding();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopStyleColorDelegate(int count);
        private readonly igPopStyleColorDelegate pigPopStyleColor = lib.LoadFunction<igPopStyleColorDelegate>("igPopStyleColor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopStyleColor(int count) => pigPopStyleColor(count);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiListClipper_BeginDelegate(ImGuiListClipper* self, int items_count, float items_height);
        private readonly ImGuiListClipper_BeginDelegate pImGuiListClipper_Begin = lib.LoadFunction<ImGuiListClipper_BeginDelegate>("ImGuiListClipper_Begin");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiListClipper_Begin(ImGuiListClipper* self, int items_count, float items_height) => pImGuiListClipper_Begin(self, items_count, items_height);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTextDelegate(byte* fmt);
        private readonly igTextDelegate pigText = lib.LoadFunction<igTextDelegate>("igText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igText(byte* fmt) => pigText(fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiListClipper_StepDelegate(ImGuiListClipper* self);
        private readonly ImGuiListClipper_StepDelegate pImGuiListClipper_Step = lib.LoadFunction<ImGuiListClipper_StepDelegate>("ImGuiListClipper_Step");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiListClipper_Step(ImGuiListClipper* self) => pImGuiListClipper_Step(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetTextLineHeightWithSpacingDelegate();
        private readonly igGetTextLineHeightWithSpacingDelegate pigGetTextLineHeightWithSpacing = lib.LoadFunction<igGetTextLineHeightWithSpacingDelegate>("igGetTextLineHeightWithSpacing");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetTextLineHeightWithSpacing() => pigGetTextLineHeightWithSpacing();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float* ImGuiStorage_GetFloatRefDelegate(ImGuiStorage* self, uint key, float default_val);
        private readonly ImGuiStorage_GetFloatRefDelegate pImGuiStorage_GetFloatRef = lib.LoadFunction<ImGuiStorage_GetFloatRefDelegate>("ImGuiStorage_GetFloatRef");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float* ImGuiStorage_GetFloatRef(ImGuiStorage* self, uint key, float default_val) => pImGuiStorage_GetFloatRef(self, key, default_val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndTooltipDelegate();
        private readonly igEndTooltipDelegate pigEndTooltip = lib.LoadFunction<igEndTooltipDelegate>("igEndTooltip");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndTooltip() => pigEndTooltip();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiListClipper_ImGuiListClipperDelegate(ImGuiListClipper* self, int items_count, float items_height);
        private readonly ImGuiListClipper_ImGuiListClipperDelegate pImGuiListClipper_ImGuiListClipper = lib.LoadFunction<ImGuiListClipper_ImGuiListClipperDelegate>("ImGuiListClipper_ImGuiListClipper");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiListClipper_ImGuiListClipper(ImGuiListClipper* self, int items_count, float items_height) => pImGuiListClipper_ImGuiListClipper(self, items_count, items_height);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragIntDelegate(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        private readonly igDragIntDelegate pigDragInt = lib.LoadFunction<igDragIntDelegate>("igDragInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragInt(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => pigDragInt(label, v, v_speed, v_min, v_max, format);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderFloatDelegate(byte* label, float* v, float v_min, float v_max, byte* format, float power);
        private readonly igSliderFloatDelegate pigSliderFloat = lib.LoadFunction<igSliderFloatDelegate>("igSliderFloat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderFloat(byte* label, float* v, float v_min, float v_max, byte* format, float power) => pigSliderFloat(label, v, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igColorConvertFloat4ToU32Delegate(Vector4 @in);
        private readonly igColorConvertFloat4ToU32Delegate pigColorConvertFloat4ToU32 = lib.LoadFunction<igColorConvertFloat4ToU32Delegate>("igColorConvertFloat4ToU32");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igColorConvertFloat4ToU32(Vector4 @in) => pigColorConvertFloat4ToU32(@in);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiIO_ClearInputCharactersDelegate(ImGuiIO* self);
        private readonly ImGuiIO_ClearInputCharactersDelegate pImGuiIO_ClearInputCharacters = lib.LoadFunction<ImGuiIO_ClearInputCharactersDelegate>("ImGuiIO_ClearInputCharacters");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiIO_ClearInputCharacters(ImGuiIO* self) => pImGuiIO_ClearInputCharacters(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushClipRectDelegate(Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect);
        private readonly igPushClipRectDelegate pigPushClipRect = lib.LoadFunction<igPushClipRectDelegate>("igPushClipRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect) => pigPushClipRect(clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetColumnWidthDelegate(int column_index, float width);
        private readonly igSetColumnWidthDelegate pigSetColumnWidth = lib.LoadFunction<igSetColumnWidthDelegate>("igSetColumnWidth");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetColumnWidth(int column_index, float width) => pigSetColumnWidth(column_index, width);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiPayload_IsDataTypeDelegate(ImGuiPayload* self, byte* type);
        private readonly ImGuiPayload_IsDataTypeDelegate pImGuiPayload_IsDataType = lib.LoadFunction<ImGuiPayload_IsDataTypeDelegate>("ImGuiPayload_IsDataType");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiPayload_IsDataType(ImGuiPayload* self, byte* type) => pImGuiPayload_IsDataType(self, type);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginMainMenuBarDelegate();
        private readonly igBeginMainMenuBarDelegate pigBeginMainMenuBar = lib.LoadFunction<igBeginMainMenuBarDelegate>("igBeginMainMenuBar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginMainMenuBar() => pigBeginMainMenuBar();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CustomRect_CustomRectDelegate(CustomRect* self);
        private readonly CustomRect_CustomRectDelegate pCustomRect_CustomRect = lib.LoadFunction<CustomRect_CustomRectDelegate>("CustomRect_CustomRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void CustomRect_CustomRect(CustomRect* self) => pCustomRect_CustomRect(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiInputTextCallbackData_HasSelectionDelegate(ImGuiInputTextCallbackData* self);
        private readonly ImGuiInputTextCallbackData_HasSelectionDelegate pImGuiInputTextCallbackData_HasSelection = lib.LoadFunction<ImGuiInputTextCallbackData_HasSelectionDelegate>("ImGuiInputTextCallbackData_HasSelection");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiInputTextCallbackData_HasSelection(ImGuiInputTextCallbackData* self) => pImGuiInputTextCallbackData_HasSelection(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiInputTextCallbackData_InsertCharsDelegate(ImGuiInputTextCallbackData* self, int pos, byte* text, byte* text_end);
        private readonly ImGuiInputTextCallbackData_InsertCharsDelegate pImGuiInputTextCallbackData_InsertChars = lib.LoadFunction<ImGuiInputTextCallbackData_InsertCharsDelegate>("ImGuiInputTextCallbackData_InsertChars");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiInputTextCallbackData_InsertChars(ImGuiInputTextCallbackData* self, int pos, byte* text, byte* text_end) => pImGuiInputTextCallbackData_InsertChars(self, pos, text, text_end);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImFontAtlas_GetMouseCursorTexDataDelegate(ImFontAtlas* self, ImGuiMouseCursor cursor, Vector2* out_offset, Vector2* out_size, Vector2* out_uv_border, Vector2* out_uv_fill);
        private readonly ImFontAtlas_GetMouseCursorTexDataDelegate pImFontAtlas_GetMouseCursorTexData = lib.LoadFunction<ImFontAtlas_GetMouseCursorTexDataDelegate>("ImFontAtlas_GetMouseCursorTexData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImFontAtlas_GetMouseCursorTexData(ImFontAtlas* self, ImGuiMouseCursor cursor, Vector2* out_offset, Vector2* out_size, Vector2* out_uv_border, Vector2* out_uv_fill) => pImFontAtlas_GetMouseCursorTexData(self, cursor, out_offset, out_size, out_uv_border, out_uv_fill);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igVSliderScalarDelegate(byte* label, Vector2 size, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power);
        private readonly igVSliderScalarDelegate pigVSliderScalar = lib.LoadFunction<igVSliderScalarDelegate>("igVSliderScalar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igVSliderScalar(byte* label, Vector2 size, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power) => pigVSliderScalar(label, size, data_type, v, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_SetAllIntDelegate(ImGuiStorage* self, int val);
        private readonly ImGuiStorage_SetAllIntDelegate pImGuiStorage_SetAllInt = lib.LoadFunction<ImGuiStorage_SetAllIntDelegate>("ImGuiStorage_SetAllInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_SetAllInt(ImGuiStorage* self, int val) => pImGuiStorage_SetAllInt(self, val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void** ImGuiStorage_GetVoidPtrRefDelegate(ImGuiStorage* self, uint key, void* default_val);
        private readonly ImGuiStorage_GetVoidPtrRefDelegate pImGuiStorage_GetVoidPtrRef = lib.LoadFunction<ImGuiStorage_GetVoidPtrRefDelegate>("ImGuiStorage_GetVoidPtrRef");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void** ImGuiStorage_GetVoidPtrRef(ImGuiStorage* self, uint key, void* default_val) => pImGuiStorage_GetVoidPtrRef(self, key, default_val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igStyleColorsLightDelegate(ImGuiStyle* dst);
        private readonly igStyleColorsLightDelegate pigStyleColorsLight = lib.LoadFunction<igStyleColorsLightDelegate>("igStyleColorsLight");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igStyleColorsLight(ImGuiStyle* dst) => pigStyleColorsLight(dst);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderFloat3Delegate(byte* label, Vector3* v, float v_min, float v_max, byte* format, float power);
        private readonly igSliderFloat3Delegate pigSliderFloat3 = lib.LoadFunction<igSliderFloat3Delegate>("igSliderFloat3");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderFloat3(byte* label, Vector3* v, float v_min, float v_max, byte* format, float power) => pigSliderFloat3(label, v, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragFloatDelegate(byte* label, float* v, float v_speed, float v_min, float v_max, byte* format, float power);
        private readonly igDragFloatDelegate pigDragFloat = lib.LoadFunction<igDragFloatDelegate>("igDragFloat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragFloat(byte* label, float* v, float v_speed, float v_min, float v_max, byte* format, float power) => pigDragFloat(label, v, v_speed, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImGuiStorage_GetBoolRefDelegate(ImGuiStorage* self, uint key, byte default_val);
        private readonly ImGuiStorage_GetBoolRefDelegate pImGuiStorage_GetBoolRef = lib.LoadFunction<ImGuiStorage_GetBoolRefDelegate>("ImGuiStorage_GetBoolRef");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImGuiStorage_GetBoolRef(ImGuiStorage* self, uint key, byte default_val) => pImGuiStorage_GetBoolRef(self, key, default_val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetWindowHeightDelegate();
        private readonly igGetWindowHeightDelegate pigGetWindowHeight = lib.LoadFunction<igGetWindowHeightDelegate>("igGetWindowHeight");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetWindowHeight() => pigGetWindowHeight();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetMousePosOnOpeningCurrentPopupDelegate();
        private readonly igGetMousePosOnOpeningCurrentPopupDelegate pigGetMousePosOnOpeningCurrentPopup = lib.LoadFunction<igGetMousePosOnOpeningCurrentPopupDelegate>("igGetMousePosOnOpeningCurrentPopup");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetMousePosOnOpeningCurrentPopup() => pigGetMousePosOnOpeningCurrentPopup();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int* ImGuiStorage_GetIntRefDelegate(ImGuiStorage* self, uint key, int default_val);
        private readonly ImGuiStorage_GetIntRefDelegate pImGuiStorage_GetIntRef = lib.LoadFunction<ImGuiStorage_GetIntRefDelegate>("ImGuiStorage_GetIntRef");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int* ImGuiStorage_GetIntRef(ImGuiStorage* self, uint key, int default_val) => pImGuiStorage_GetIntRef(self, key, default_val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igCalcListClippingDelegate(int items_count, float items_height, int* out_items_display_start, int* out_items_display_end);
        private readonly igCalcListClippingDelegate pigCalcListClipping = lib.LoadFunction<igCalcListClippingDelegate>("igCalcListClipping");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igCalcListClipping(int items_count, float items_height, int* out_items_display_start, int* out_items_display_end) => pigCalcListClipping(items_count, items_height, out_items_display_start, out_items_display_end);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_SetVoidPtrDelegate(ImGuiStorage* self, uint key, void* val);
        private readonly ImGuiStorage_SetVoidPtrDelegate pImGuiStorage_SetVoidPtr = lib.LoadFunction<ImGuiStorage_SetVoidPtrDelegate>("ImGuiStorage_SetVoidPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_SetVoidPtr(ImGuiStorage* self, uint key, void* val) => pImGuiStorage_SetVoidPtr(self, key, val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndDragDropSourceDelegate();
        private readonly igEndDragDropSourceDelegate pigEndDragDropSource = lib.LoadFunction<igEndDragDropSourceDelegate>("igEndDragDropSource");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndDragDropSource() => pigEndDragDropSource();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_BuildSortByKeyDelegate(ImGuiStorage* self);
        private readonly ImGuiStorage_BuildSortByKeyDelegate pImGuiStorage_BuildSortByKey = lib.LoadFunction<ImGuiStorage_BuildSortByKeyDelegate>("ImGuiStorage_BuildSortByKey");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_BuildSortByKey(ImGuiStorage* self) => pImGuiStorage_BuildSortByKey(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float ImGuiStorage_GetFloatDelegate(ImGuiStorage* self, uint key, float default_val);
        private readonly ImGuiStorage_GetFloatDelegate pImGuiStorage_GetFloat = lib.LoadFunction<ImGuiStorage_GetFloatDelegate>("ImGuiStorage_GetFloat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float ImGuiStorage_GetFloat(ImGuiStorage* self, uint key, float default_val) => pImGuiStorage_GetFloat(self, key, default_val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_SetBoolDelegate(ImGuiStorage* self, uint key, byte val);
        private readonly ImGuiStorage_SetBoolDelegate pImGuiStorage_SetBool = lib.LoadFunction<ImGuiStorage_SetBoolDelegate>("ImGuiStorage_SetBool");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_SetBool(ImGuiStorage* self, uint key, byte val) => pImGuiStorage_SetBool(self, key, val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiStorage_GetBoolDelegate(ImGuiStorage* self, uint key, byte default_val);
        private readonly ImGuiStorage_GetBoolDelegate pImGuiStorage_GetBool = lib.LoadFunction<ImGuiStorage_GetBoolDelegate>("ImGuiStorage_GetBool");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiStorage_GetBool(ImGuiStorage* self, uint key, byte default_val) => pImGuiStorage_GetBool(self, key, default_val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetFrameHeightWithSpacingDelegate();
        private readonly igGetFrameHeightWithSpacingDelegate pigGetFrameHeightWithSpacing = lib.LoadFunction<igGetFrameHeightWithSpacingDelegate>("igGetFrameHeightWithSpacing");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetFrameHeightWithSpacing() => pigGetFrameHeightWithSpacing();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_SetIntDelegate(ImGuiStorage* self, uint key, int val);
        private readonly ImGuiStorage_SetIntDelegate pImGuiStorage_SetInt = lib.LoadFunction<ImGuiStorage_SetIntDelegate>("ImGuiStorage_SetInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_SetInt(ImGuiStorage* self, uint key, int val) => pImGuiStorage_SetInt(self, key, val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igCloseCurrentPopupDelegate();
        private readonly igCloseCurrentPopupDelegate pigCloseCurrentPopup = lib.LoadFunction<igCloseCurrentPopupDelegate>("igCloseCurrentPopup");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igCloseCurrentPopup() => pigCloseCurrentPopup();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextBuffer_clearDelegate(ImGuiTextBuffer* self);
        private readonly ImGuiTextBuffer_clearDelegate pImGuiTextBuffer_clear = lib.LoadFunction<ImGuiTextBuffer_clearDelegate>("ImGuiTextBuffer_clear");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextBuffer_clear(ImGuiTextBuffer* self) => pImGuiTextBuffer_clear(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igBeginGroupDelegate();
        private readonly igBeginGroupDelegate pigBeginGroup = lib.LoadFunction<igBeginGroupDelegate>("igBeginGroup");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igBeginGroup() => pigBeginGroup();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_ClearDelegate(ImGuiStorage* self);
        private readonly ImGuiStorage_ClearDelegate pImGuiStorage_Clear = lib.LoadFunction<ImGuiStorage_ClearDelegate>("ImGuiStorage_Clear");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStorage_Clear(ImGuiStorage* self) => pImGuiStorage_Clear(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void Pair_PairIntDelegate(Pair* self, uint _key, int _val_i);
        private readonly Pair_PairIntDelegate pPair_PairInt = lib.LoadFunction<Pair_PairIntDelegate>("Pair_PairInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void Pair_PairInt(Pair* self, uint _key, int _val_i) => pPair_PairInt(self, _key, _val_i);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void Pair_PairFloatDelegate(Pair* self, uint _key, float _val_f);
        private readonly Pair_PairFloatDelegate pPair_PairFloat = lib.LoadFunction<Pair_PairFloatDelegate>("Pair_PairFloat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void Pair_PairFloat(Pair* self, uint _key, float _val_f) => pPair_PairFloat(self, _key, _val_f);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void Pair_PairPtrDelegate(Pair* self, uint _key, void* _val_p);
        private readonly Pair_PairPtrDelegate pPair_PairPtr = lib.LoadFunction<Pair_PairPtrDelegate>("Pair_PairPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void Pair_PairPtr(Pair* self, uint _key, void* _val_p) => pPair_PairPtr(self, _key, _val_p);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextBuffer_appendfDelegate(ImGuiTextBuffer* self, byte* fmt);
        private readonly ImGuiTextBuffer_appendfDelegate pImGuiTextBuffer_appendf = lib.LoadFunction<ImGuiTextBuffer_appendfDelegate>("ImGuiTextBuffer_appendf");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextBuffer_appendf(ImGuiTextBuffer* self, byte* fmt) => pImGuiTextBuffer_appendf(self, fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImGuiTextBuffer_c_strDelegate(ImGuiTextBuffer* self);
        private readonly ImGuiTextBuffer_c_strDelegate pImGuiTextBuffer_c_str = lib.LoadFunction<ImGuiTextBuffer_c_strDelegate>("ImGuiTextBuffer_c_str");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImGuiTextBuffer_c_str(ImGuiTextBuffer* self) => pImGuiTextBuffer_c_str(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextBuffer_reserveDelegate(ImGuiTextBuffer* self, int capacity);
        private readonly ImGuiTextBuffer_reserveDelegate pImGuiTextBuffer_reserve = lib.LoadFunction<ImGuiTextBuffer_reserveDelegate>("ImGuiTextBuffer_reserve");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextBuffer_reserve(ImGuiTextBuffer* self, int capacity) => pImGuiTextBuffer_reserve(self, capacity);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiTextBuffer_emptyDelegate(ImGuiTextBuffer* self);
        private readonly ImGuiTextBuffer_emptyDelegate pImGuiTextBuffer_empty = lib.LoadFunction<ImGuiTextBuffer_emptyDelegate>("ImGuiTextBuffer_empty");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiTextBuffer_empty(ImGuiTextBuffer* self) => pImGuiTextBuffer_empty(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderScalarDelegate(byte* label, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power);
        private readonly igSliderScalarDelegate pigSliderScalar = lib.LoadFunction<igSliderScalarDelegate>("igSliderScalar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderScalar(byte* label, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power) => pigSliderScalar(label, data_type, v, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginComboDelegate(byte* label, byte* preview_value, ImGuiComboFlags flags);
        private readonly igBeginComboDelegate pigBeginCombo = lib.LoadFunction<igBeginComboDelegate>("igBeginCombo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginCombo(byte* label, byte* preview_value, ImGuiComboFlags flags) => pigBeginCombo(label, preview_value, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ImGuiTextBuffer_sizeDelegate(ImGuiTextBuffer* self);
        private readonly ImGuiTextBuffer_sizeDelegate pImGuiTextBuffer_size = lib.LoadFunction<ImGuiTextBuffer_sizeDelegate>("ImGuiTextBuffer_size");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int ImGuiTextBuffer_size(ImGuiTextBuffer* self) => pImGuiTextBuffer_size(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginMenuDelegate(byte* label, byte enabled);
        private readonly igBeginMenuDelegate pigBeginMenu = lib.LoadFunction<igBeginMenuDelegate>("igBeginMenu");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginMenu(byte* label, byte enabled) => pigBeginMenu(label, enabled);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemHoveredDelegate(ImGuiHoveredFlags flags);
        private readonly igIsItemHoveredDelegate pigIsItemHovered = lib.LoadFunction<igIsItemHoveredDelegate>("igIsItemHovered");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemHovered(ImGuiHoveredFlags flags) => pigIsItemHovered(flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimWriteVtxDelegate(ImDrawList* self, Vector2 pos, Vector2 uv, uint col);
        private readonly ImDrawList_PrimWriteVtxDelegate pImDrawList_PrimWriteVtx = lib.LoadFunction<ImDrawList_PrimWriteVtxDelegate>("ImDrawList_PrimWriteVtx");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimWriteVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col) => pImDrawList_PrimWriteVtx(self, pos, uv, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igBulletDelegate();
        private readonly igBulletDelegate pigBullet = lib.LoadFunction<igBulletDelegate>("igBullet");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igBullet() => pigBullet();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputTextDelegate(byte* label, byte* buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);
        private readonly igInputTextDelegate pigInputText = lib.LoadFunction<igInputTextDelegate>("igInputText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputText(byte* label, byte* buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data) => pigInputText(label, buf, buf_size, flags, callback, user_data);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputInt3Delegate(byte* label, int* v, ImGuiInputTextFlags extra_flags);
        private readonly igInputInt3Delegate pigInputInt3 = lib.LoadFunction<igInputInt3Delegate>("igInputInt3");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputInt3(byte* label, int* v, ImGuiInputTextFlags extra_flags) => pigInputInt3(label, v, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiIO_ImGuiIODelegate(ImGuiIO* self);
        private readonly ImGuiIO_ImGuiIODelegate pImGuiIO_ImGuiIO = lib.LoadFunction<ImGuiIO_ImGuiIODelegate>("ImGuiIO_ImGuiIO");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiIO_ImGuiIO(ImGuiIO* self) => pImGuiIO_ImGuiIO(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igStyleColorsDarkDelegate(ImGuiStyle* dst);
        private readonly igStyleColorsDarkDelegate pigStyleColorsDark = lib.LoadFunction<igStyleColorsDarkDelegate>("igStyleColorsDark");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igStyleColorsDark(ImGuiStyle* dst) => pigStyleColorsDark(dst);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputIntDelegate(byte* label, int* v, int step, int step_fast, ImGuiInputTextFlags extra_flags);
        private readonly igInputIntDelegate pigInputInt = lib.LoadFunction<igInputIntDelegate>("igInputInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputInt(byte* label, int* v, int step, int step_fast, ImGuiInputTextFlags extra_flags) => pigInputInt(label, v, step, step_fast, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowFontScaleDelegate(float scale);
        private readonly igSetWindowFontScaleDelegate pigSetWindowFontScale = lib.LoadFunction<igSetWindowFontScaleDelegate>("igSetWindowFontScale");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowFontScale(float scale) => pigSetWindowFontScale(scale);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderIntDelegate(byte* label, int* v, int v_min, int v_max, byte* format);
        private readonly igSliderIntDelegate pigSliderInt = lib.LoadFunction<igSliderIntDelegate>("igSliderInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderInt(byte* label, int* v, int v_min, int v_max, byte* format) => pigSliderInt(label, v, v_min, v_max, format);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* TextRange_endDelegate(TextRange* self);
        private readonly TextRange_endDelegate pTextRange_end = lib.LoadFunction<TextRange_endDelegate>("TextRange_end");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* TextRange_end(TextRange* self) => pTextRange_end(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* TextRange_beginDelegate(TextRange* self);
        private readonly TextRange_beginDelegate pTextRange_begin = lib.LoadFunction<TextRange_beginDelegate>("TextRange_begin");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* TextRange_begin(TextRange* self) => pTextRange_begin(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowPosDelegate(Vector2 pos, ImGuiCond cond, Vector2 pivot);
        private readonly igSetNextWindowPosDelegate pigSetNextWindowPos = lib.LoadFunction<igSetNextWindowPosDelegate>("igSetNextWindowPos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowPos(Vector2 pos, ImGuiCond cond, Vector2 pivot) => pigSetNextWindowPos(pos, cond, pivot);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragInt3Delegate(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        private readonly igDragInt3Delegate pigDragInt3 = lib.LoadFunction<igDragInt3Delegate>("igDragInt3");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragInt3(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => pigDragInt3(label, v, v_speed, v_min, v_max, format);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igOpenPopupDelegate(byte* str_id);
        private readonly igOpenPopupDelegate pigOpenPopup = lib.LoadFunction<igOpenPopupDelegate>("igOpenPopup");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igOpenPopup(byte* str_id) => pigOpenPopup(str_id);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TextRange_TextRangeDelegate(TextRange* self);
        private readonly TextRange_TextRangeDelegate pTextRange_TextRange = lib.LoadFunction<TextRange_TextRangeDelegate>("TextRange_TextRange");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void TextRange_TextRange(TextRange* self) => pTextRange_TextRange(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TextRange_TextRangeStrDelegate(TextRange* self, byte* _b, byte* _e);
        private readonly TextRange_TextRangeStrDelegate pTextRange_TextRangeStr = lib.LoadFunction<TextRange_TextRangeStrDelegate>("TextRange_TextRangeStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void TextRange_TextRangeStr(TextRange* self, byte* _b, byte* _e) => pTextRange_TextRangeStr(self, _b, _e);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 ImDrawList_GetClipRectMaxDelegate(ImDrawList* self);
        private readonly ImDrawList_GetClipRectMaxDelegate pImDrawList_GetClipRectMax = lib.LoadFunction<ImDrawList_GetClipRectMaxDelegate>("ImDrawList_GetClipRectMax");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 ImDrawList_GetClipRectMax(ImDrawList* self) => pImDrawList_GetClipRectMax(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igCalcTextSizeDelegate(byte* text, byte* text_end, byte hide_text_after_double_hash, float wrap_width);
        private readonly igCalcTextSizeDelegate pigCalcTextSize = lib.LoadFunction<igCalcTextSizeDelegate>("igCalcTextSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igCalcTextSize(byte* text, byte* text_end, byte hide_text_after_double_hash, float wrap_width) => pigCalcTextSize(text, text_end, hide_text_after_double_hash, wrap_width);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr igGetDrawListSharedDataDelegate();
        private readonly igGetDrawListSharedDataDelegate pigGetDrawListSharedData = lib.LoadFunction<igGetDrawListSharedDataDelegate>("igGetDrawListSharedData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr igGetDrawListSharedData() => pigGetDrawListSharedData();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igColumnsDelegate(int count, byte* id, byte border);
        private readonly igColumnsDelegate pigColumns = lib.LoadFunction<igColumnsDelegate>("igColumns");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igColumns(int count, byte* id, byte border) => pigColumns(count, id, border);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemActiveDelegate();
        private readonly igIsItemActiveDelegate pigIsItemActive = lib.LoadFunction<igIsItemActiveDelegate>("igIsItemActive");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsItemActive() => pigIsItemActive();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextFilter_ImGuiTextFilterDelegate(ImGuiTextFilter* self, byte* default_filter);
        private readonly ImGuiTextFilter_ImGuiTextFilterDelegate pImGuiTextFilter_ImGuiTextFilter = lib.LoadFunction<ImGuiTextFilter_ImGuiTextFilterDelegate>("ImGuiTextFilter_ImGuiTextFilter");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextFilter_ImGuiTextFilter(ImGuiTextFilter* self, byte* default_filter) => pImGuiTextFilter_ImGuiTextFilter(self, default_filter);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiOnceUponAFrame_ImGuiOnceUponAFrameDelegate(ImGuiOnceUponAFrame* self);
        private readonly ImGuiOnceUponAFrame_ImGuiOnceUponAFrameDelegate pImGuiOnceUponAFrame_ImGuiOnceUponAFrame = lib.LoadFunction<ImGuiOnceUponAFrame_ImGuiOnceUponAFrameDelegate>("ImGuiOnceUponAFrame_ImGuiOnceUponAFrame");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiOnceUponAFrame_ImGuiOnceUponAFrame(ImGuiOnceUponAFrame* self) => pImGuiOnceUponAFrame_ImGuiOnceUponAFrame(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginDragDropTargetDelegate();
        private readonly igBeginDragDropTargetDelegate pigBeginDragDropTarget = lib.LoadFunction<igBeginDragDropTargetDelegate>("igBeginDragDropTarget");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginDragDropTarget() => pigBeginDragDropTarget();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte TextRange_emptyDelegate(TextRange* self);
        private readonly TextRange_emptyDelegate pTextRange_empty = lib.LoadFunction<TextRange_emptyDelegate>("TextRange_empty");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte TextRange_empty(TextRange* self) => pTextRange_empty(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiPayload_IsDeliveryDelegate(ImGuiPayload* self);
        private readonly ImGuiPayload_IsDeliveryDelegate pImGuiPayload_IsDelivery = lib.LoadFunction<ImGuiPayload_IsDeliveryDelegate>("ImGuiPayload_IsDelivery");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiPayload_IsDelivery(ImGuiPayload* self) => pImGuiPayload_IsDelivery(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiIO_AddInputCharacterDelegate(ImGuiIO* self, ushort c);
        private readonly ImGuiIO_AddInputCharacterDelegate pImGuiIO_AddInputCharacter = lib.LoadFunction<ImGuiIO_AddInputCharacterDelegate>("ImGuiIO_AddInputCharacter");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiIO_AddInputCharacter(ImGuiIO* self, ushort c) => pImGuiIO_AddInputCharacter(self, c);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddImageRoundedDelegate(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col, float rounding, int rounding_corners);
        private readonly ImDrawList_AddImageRoundedDelegate pImDrawList_AddImageRounded = lib.LoadFunction<ImDrawList_AddImageRoundedDelegate>("ImDrawList_AddImageRounded");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_AddImageRounded(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col, float rounding, int rounding_corners) => pImDrawList_AddImageRounded(self, user_texture_id, a, b, uv_a, uv_b, col, rounding, rounding_corners);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStyle_ImGuiStyleDelegate(ImGuiStyle* self);
        private readonly ImGuiStyle_ImGuiStyleDelegate pImGuiStyle_ImGuiStyle = lib.LoadFunction<ImGuiStyle_ImGuiStyleDelegate>("ImGuiStyle_ImGuiStyle");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStyle_ImGuiStyle(ImGuiStyle* self) => pImGuiStyle_ImGuiStyle(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igColorPicker3Delegate(byte* label, Vector3* col, ImGuiColorEditFlags flags);
        private readonly igColorPicker3Delegate pigColorPicker3 = lib.LoadFunction<igColorPicker3Delegate>("igColorPicker3");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igColorPicker3(byte* label, Vector3* col, ImGuiColorEditFlags flags) => pigColorPicker3(label, col, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetContentRegionMaxDelegate();
        private readonly igGetContentRegionMaxDelegate pigGetContentRegionMax = lib.LoadFunction<igGetContentRegionMaxDelegate>("igGetContentRegionMax");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetContentRegionMax() => pigGetContentRegionMax();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginChildFrameDelegate(uint id, Vector2 size, ImGuiWindowFlags flags);
        private readonly igBeginChildFrameDelegate pigBeginChildFrame = lib.LoadFunction<igBeginChildFrameDelegate>("igBeginChildFrame");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags) => pigBeginChildFrame(id, size, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSaveIniSettingsToDiskDelegate(byte* ini_filename);
        private readonly igSaveIniSettingsToDiskDelegate pigSaveIniSettingsToDisk = lib.LoadFunction<igSaveIniSettingsToDiskDelegate>("igSaveIniSettingsToDisk");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSaveIniSettingsToDisk(byte* ini_filename) => pigSaveIniSettingsToDisk(ini_filename);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_ClearOutputDataDelegate(ImFont* self);
        private readonly ImFont_ClearOutputDataDelegate pImFont_ClearOutputData = lib.LoadFunction<ImFont_ClearOutputDataDelegate>("ImFont_ClearOutputData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFont_ClearOutputData(ImFont* self) => pImFont_ClearOutputData(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* igGetClipboardTextDelegate();
        private readonly igGetClipboardTextDelegate pigGetClipboardText = lib.LoadFunction<igGetClipboardTextDelegate>("igGetClipboardText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* igGetClipboardText() => pigGetClipboardText();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimQuadUVDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);
        private readonly ImDrawList_PrimQuadUVDelegate pImDrawList_PrimQuadUV = lib.LoadFunction<ImDrawList_PrimQuadUVDelegate>("ImDrawList_PrimQuadUV");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimQuadUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col) => pImDrawList_PrimQuadUV(self, a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndDragDropTargetDelegate();
        private readonly igEndDragDropTargetDelegate pigEndDragDropTarget = lib.LoadFunction<igEndDragDropTargetDelegate>("igEndDragDropTarget");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndDragDropTarget() => pigEndDragDropTarget();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesKoreanDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_GetGlyphRangesKoreanDelegate pImFontAtlas_GetGlyphRangesKorean = lib.LoadFunction<ImFontAtlas_GetGlyphRangesKoreanDelegate>("ImFontAtlas_GetGlyphRangesKorean");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ushort* ImFontAtlas_GetGlyphRangesKorean(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesKorean(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int igGetKeyPressedAmountDelegate(int key_index, float repeat_delay, float rate);
        private readonly igGetKeyPressedAmountDelegate pigGetKeyPressedAmount = lib.LoadFunction<igGetKeyPressedAmountDelegate>("igGetKeyPressedAmount");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int igGetKeyPressedAmount(int key_index, float repeat_delay, float rate) => pigGetKeyPressedAmount(key_index, repeat_delay, rate);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_GetTexDataAsRGBA32Delegate(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);
        private readonly ImFontAtlas_GetTexDataAsRGBA32Delegate pImFontAtlas_GetTexDataAsRGBA32 = lib.LoadFunction<ImFontAtlas_GetTexDataAsRGBA32Delegate>("ImFontAtlas_GetTexDataAsRGBA32");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_GetTexDataAsRGBA32(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel) => pImFontAtlas_GetTexDataAsRGBA32(self, out_pixels, out_width, out_height, out_bytes_per_pixel);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igNewFrameDelegate();
        private readonly igNewFrameDelegate pigNewFrame = lib.LoadFunction<igNewFrameDelegate>("igNewFrame");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igNewFrame() => pigNewFrame();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igResetMouseDragDeltaDelegate(int button);
        private readonly igResetMouseDragDeltaDelegate pigResetMouseDragDelta = lib.LoadFunction<igResetMouseDragDeltaDelegate>("igResetMouseDragDelta");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igResetMouseDragDelta(int button) => pigResetMouseDragDelta(button);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetTreeNodeToLabelSpacingDelegate();
        private readonly igGetTreeNodeToLabelSpacingDelegate pigGetTreeNodeToLabelSpacing = lib.LoadFunction<igGetTreeNodeToLabelSpacingDelegate>("igGetTreeNodeToLabelSpacing");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetTreeNodeToLabelSpacing() => pigGetTreeNodeToLabelSpacing();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetMousePosDelegate();
        private readonly igGetMousePosDelegate pigGetMousePos = lib.LoadFunction<igGetMousePosDelegate>("igGetMousePos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetMousePos() => pigGetMousePos();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_AddCharDelegate(GlyphRangesBuilder* self, ushort c);
        private readonly GlyphRangesBuilder_AddCharDelegate pGlyphRangesBuilder_AddChar = lib.LoadFunction<GlyphRangesBuilder_AddCharDelegate>("GlyphRangesBuilder_AddChar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void GlyphRangesBuilder_AddChar(GlyphRangesBuilder* self, ushort c) => pGlyphRangesBuilder_AddChar(self, c);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopIDDelegate();
        private readonly igPopIDDelegate pigPopID = lib.LoadFunction<igPopIDDelegate>("igPopID");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopID() => pigPopID();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseDoubleClickedDelegate(int button);
        private readonly igIsMouseDoubleClickedDelegate pigIsMouseDoubleClicked = lib.LoadFunction<igIsMouseDoubleClickedDelegate>("igIsMouseDoubleClicked");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseDoubleClicked(int button) => pigIsMouseDoubleClicked(button);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igStyleColorsClassicDelegate(ImGuiStyle* dst);
        private readonly igStyleColorsClassicDelegate pigStyleColorsClassic = lib.LoadFunction<igStyleColorsClassicDelegate>("igStyleColorsClassic");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igStyleColorsClassic(ImGuiStyle* dst) => pigStyleColorsClassic(dst);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiTextFilter_IsActiveDelegate(ImGuiTextFilter* self);
        private readonly ImGuiTextFilter_IsActiveDelegate pImGuiTextFilter_IsActive = lib.LoadFunction<ImGuiTextFilter_IsActiveDelegate>("ImGuiTextFilter_IsActive");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiTextFilter_IsActive(ImGuiTextFilter* self) => pImGuiTextFilter_IsActive(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathClearDelegate(ImDrawList* self);
        private readonly ImDrawList_PathClearDelegate pImDrawList_PathClear = lib.LoadFunction<ImDrawList_PathClearDelegate>("ImDrawList_PathClear");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathClear(ImDrawList* self) => pImDrawList_PathClear(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowFocusDelegate();
        private readonly igSetWindowFocusDelegate pigSetWindowFocus = lib.LoadFunction<igSetWindowFocusDelegate>("igSetWindowFocus");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowFocus() => pigSetWindowFocus();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowFocusStrDelegate(byte* name);
        private readonly igSetWindowFocusStrDelegate pigSetWindowFocusStr = lib.LoadFunction<igSetWindowFocusStrDelegate>("igSetWindowFocusStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowFocusStr(byte* name) => pigSetWindowFocusStr(name);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igColorConvertHSVtoRGBDelegate(float h, float s, float v, float* out_r, float* out_g, float* out_b);
        private readonly igColorConvertHSVtoRGBDelegate pigColorConvertHSVtoRGB = lib.LoadFunction<igColorConvertHSVtoRGBDelegate>("igColorConvertHSVtoRGB");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igColorConvertHSVtoRGB(float h, float s, float v, float* out_r, float* out_g, float* out_b) => pigColorConvertHSVtoRGB(h, s, v, out_r, out_g, out_b);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_ImColorDelegate(ImColor* self);
        private readonly ImColor_ImColorDelegate pImColor_ImColor = lib.LoadFunction<ImColor_ImColorDelegate>("ImColor_ImColor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_ImColor(ImColor* self) => pImColor_ImColor(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_ImColorIntDelegate(ImColor* self, int r, int g, int b, int a);
        private readonly ImColor_ImColorIntDelegate pImColor_ImColorInt = lib.LoadFunction<ImColor_ImColorIntDelegate>("ImColor_ImColorInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_ImColorInt(ImColor* self, int r, int g, int b, int a) => pImColor_ImColorInt(self, r, g, b, a);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_ImColorU32Delegate(ImColor* self, uint rgba);
        private readonly ImColor_ImColorU32Delegate pImColor_ImColorU32 = lib.LoadFunction<ImColor_ImColorU32Delegate>("ImColor_ImColorU32");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_ImColorU32(ImColor* self, uint rgba) => pImColor_ImColorU32(self, rgba);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_ImColorFloatDelegate(ImColor* self, float r, float g, float b, float a);
        private readonly ImColor_ImColorFloatDelegate pImColor_ImColorFloat = lib.LoadFunction<ImColor_ImColorFloatDelegate>("ImColor_ImColorFloat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_ImColorFloat(ImColor* self, float r, float g, float b, float a) => pImColor_ImColorFloat(self, r, g, b, a);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_ImColorVec4Delegate(ImColor* self, Vector4 col);
        private readonly ImColor_ImColorVec4Delegate pImColor_ImColorVec4 = lib.LoadFunction<ImColor_ImColorVec4Delegate>("ImColor_ImColorVec4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImColor_ImColorVec4(ImColor* self, Vector4 col) => pImColor_ImColorVec4(self, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igVSliderFloatDelegate(byte* label, Vector2 size, float* v, float v_min, float v_max, byte* format, float power);
        private readonly igVSliderFloatDelegate pigVSliderFloat = lib.LoadFunction<igVSliderFloatDelegate>("igVSliderFloat");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igVSliderFloat(byte* label, Vector2 size, float* v, float v_min, float v_max, byte* format, float power) => pigVSliderFloat(label, size, v, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector4 igColorConvertU32ToFloat4Delegate(uint @in);
        private readonly igColorConvertU32ToFloat4Delegate pigColorConvertU32ToFloat4 = lib.LoadFunction<igColorConvertU32ToFloat4Delegate>("igColorConvertU32ToFloat4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector4 igColorConvertU32ToFloat4(uint @in) => pigColorConvertU32ToFloat4(@in);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopTextWrapPosDelegate();
        private readonly igPopTextWrapPosDelegate pigPopTextWrapPos = lib.LoadFunction<igPopTextWrapPosDelegate>("igPopTextWrapPos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopTextWrapPos() => pigPopTextWrapPos();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextFilter_ClearDelegate(ImGuiTextFilter* self);
        private readonly ImGuiTextFilter_ClearDelegate pImGuiTextFilter_Clear = lib.LoadFunction<ImGuiTextFilter_ClearDelegate>("ImGuiTextFilter_Clear");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextFilter_Clear(ImGuiTextFilter* self) => pImGuiTextFilter_Clear(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImGuiStorage* igGetStateStorageDelegate();
        private readonly igGetStateStorageDelegate pigGetStateStorage = lib.LoadFunction<igGetStateStorageDelegate>("igGetStateStorage");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImGuiStorage* igGetStateStorage() => pigGetStateStorage();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetColumnWidthDelegate(int column_index);
        private readonly igGetColumnWidthDelegate pigGetColumnWidth = lib.LoadFunction<igGetColumnWidthDelegate>("igGetColumnWidth");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetColumnWidth(int column_index) => pigGetColumnWidth(column_index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndMenuBarDelegate();
        private readonly igEndMenuBarDelegate pigEndMenuBar = lib.LoadFunction<igEndMenuBarDelegate>("igEndMenuBar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndMenuBar() => pigEndMenuBar();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetStateStorageDelegate(ImGuiStorage* storage);
        private readonly igSetStateStorageDelegate pigSetStateStorage = lib.LoadFunction<igSetStateStorageDelegate>("igSetStateStorage");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetStateStorage(ImGuiStorage* storage) => pigSetStateStorage(storage);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* igGetStyleColorNameDelegate(ImGuiCol idx);
        private readonly igGetStyleColorNameDelegate pigGetStyleColorName = lib.LoadFunction<igGetStyleColorNameDelegate>("igGetStyleColorName");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* igGetStyleColorName(ImGuiCol idx) => pigGetStyleColorName(idx);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseDraggingDelegate(int button, float lock_threshold);
        private readonly igIsMouseDraggingDelegate pigIsMouseDragging = lib.LoadFunction<igIsMouseDraggingDelegate>("igIsMouseDragging");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseDragging(int button, float lock_threshold) => pigIsMouseDragging(button, lock_threshold);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimWriteIdxDelegate(ImDrawList* self, ushort idx);
        private readonly ImDrawList_PrimWriteIdxDelegate pImDrawList_PrimWriteIdx = lib.LoadFunction<ImDrawList_PrimWriteIdxDelegate>("ImDrawList_PrimWriteIdx");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimWriteIdx(ImDrawList* self, ushort idx) => pImDrawList_PrimWriteIdx(self, idx);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStyle_ScaleAllSizesDelegate(ImGuiStyle* self, float scale_factor);
        private readonly ImGuiStyle_ScaleAllSizesDelegate pImGuiStyle_ScaleAllSizes = lib.LoadFunction<ImGuiStyle_ScaleAllSizesDelegate>("ImGuiStyle_ScaleAllSizes");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiStyle_ScaleAllSizes(ImGuiStyle* self, float scale_factor) => pImGuiStyle_ScaleAllSizes(self, scale_factor);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushStyleColorU32Delegate(ImGuiCol idx, uint col);
        private readonly igPushStyleColorU32Delegate pigPushStyleColorU32 = lib.LoadFunction<igPushStyleColorU32Delegate>("igPushStyleColorU32");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushStyleColorU32(ImGuiCol idx, uint col) => pigPushStyleColorU32(idx, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushStyleColorDelegate(ImGuiCol idx, Vector4 col);
        private readonly igPushStyleColorDelegate pigPushStyleColor = lib.LoadFunction<igPushStyleColorDelegate>("igPushStyleColor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushStyleColor(ImGuiCol idx, Vector4 col) => pigPushStyleColor(idx, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void* igMemAllocDelegate(uint size);
        private readonly igMemAllocDelegate pigMemAlloc = lib.LoadFunction<igMemAllocDelegate>("igMemAlloc");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void* igMemAlloc(uint size) => pigMemAlloc(size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetCurrentContextDelegate(IntPtr ctx);
        private readonly igSetCurrentContextDelegate pigSetCurrentContext = lib.LoadFunction<igSetCurrentContextDelegate>("igSetCurrentContext");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetCurrentContext(IntPtr ctx) => pigSetCurrentContext(ctx);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushItemWidthDelegate(float item_width);
        private readonly igPushItemWidthDelegate pigPushItemWidth = lib.LoadFunction<igPushItemWidthDelegate>("igPushItemWidth");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushItemWidth(float item_width) => pigPushItemWidth(item_width);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsWindowAppearingDelegate();
        private readonly igIsWindowAppearingDelegate pigIsWindowAppearing = lib.LoadFunction<igIsWindowAppearingDelegate>("igIsWindowAppearing");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsWindowAppearing() => pigIsWindowAppearing();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImGuiStyle* igGetStyleDelegate();
        private readonly igGetStyleDelegate pigGetStyle = lib.LoadFunction<igGetStyleDelegate>("igGetStyle");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImGuiStyle* igGetStyle() => pigGetStyle();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetItemAllowOverlapDelegate();
        private readonly igSetItemAllowOverlapDelegate pigSetItemAllowOverlap = lib.LoadFunction<igSetItemAllowOverlapDelegate>("igSetItemAllowOverlap");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetItemAllowOverlap() => pigSetItemAllowOverlap();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndChildDelegate();
        private readonly igEndChildDelegate pigEndChild = lib.LoadFunction<igEndChildDelegate>("igEndChild");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndChild() => pigEndChild();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igCollapsingHeaderDelegate(byte* label, ImGuiTreeNodeFlags flags);
        private readonly igCollapsingHeaderDelegate pigCollapsingHeader = lib.LoadFunction<igCollapsingHeaderDelegate>("igCollapsingHeader");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igCollapsingHeader(byte* label, ImGuiTreeNodeFlags flags) => pigCollapsingHeader(label, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igCollapsingHeaderBoolPtrDelegate(byte* label, byte* p_open, ImGuiTreeNodeFlags flags);
        private readonly igCollapsingHeaderBoolPtrDelegate pigCollapsingHeaderBoolPtr = lib.LoadFunction<igCollapsingHeaderBoolPtrDelegate>("igCollapsingHeaderBoolPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igCollapsingHeaderBoolPtr(byte* label, byte* p_open, ImGuiTreeNodeFlags flags) => pigCollapsingHeaderBoolPtr(label, p_open, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragFloatRange2Delegate(byte* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, byte* format, byte* format_max, float power);
        private readonly igDragFloatRange2Delegate pigDragFloatRange2 = lib.LoadFunction<igDragFloatRange2Delegate>("igDragFloatRange2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragFloatRange2(byte* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, byte* format, byte* format_max, float power) => pigDragFloatRange2(label, v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetMouseCursorDelegate(ImGuiMouseCursor type);
        private readonly igSetMouseCursorDelegate pigSetMouseCursor = lib.LoadFunction<igSetMouseCursorDelegate>("igSetMouseCursor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetMouseCursor(ImGuiMouseCursor type) => pigSetMouseCursor(type);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetWindowContentRegionMaxDelegate();
        private readonly igGetWindowContentRegionMaxDelegate pigGetWindowContentRegionMax = lib.LoadFunction<igGetWindowContentRegionMaxDelegate>("igGetWindowContentRegionMax");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetWindowContentRegionMax() => pigGetWindowContentRegionMax();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputScalarDelegate(byte* label, ImGuiDataType data_type, void* v, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        private readonly igInputScalarDelegate pigInputScalar = lib.LoadFunction<igInputScalarDelegate>("igInputScalar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputScalar(byte* label, ImGuiDataType data_type, void* v, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags) => pigInputScalar(label, data_type, v, step, step_fast, format, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PushClipRectFullScreenDelegate(ImDrawList* self);
        private readonly ImDrawList_PushClipRectFullScreenDelegate pImDrawList_PushClipRectFullScreen = lib.LoadFunction<ImDrawList_PushClipRectFullScreenDelegate>("ImDrawList_PushClipRectFullScreen");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PushClipRectFullScreen(ImDrawList* self) => pImDrawList_PushClipRectFullScreen(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetColorU32Delegate(ImGuiCol idx, float alpha_mul);
        private readonly igGetColorU32Delegate pigGetColorU32 = lib.LoadFunction<igGetColorU32Delegate>("igGetColorU32");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetColorU32(ImGuiCol idx, float alpha_mul) => pigGetColorU32(idx, alpha_mul);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetColorU32Vec4Delegate(Vector4 col);
        private readonly igGetColorU32Vec4Delegate pigGetColorU32Vec4 = lib.LoadFunction<igGetColorU32Vec4Delegate>("igGetColorU32Vec4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetColorU32Vec4(Vector4 col) => pigGetColorU32Vec4(col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetColorU32U32Delegate(uint col);
        private readonly igGetColorU32U32Delegate pigGetColorU32U32 = lib.LoadFunction<igGetColorU32U32Delegate>("igGetColorU32U32");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed uint igGetColorU32U32(uint col) => pigGetColorU32U32(col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate double igGetTimeDelegate();
        private readonly igGetTimeDelegate pigGetTime = lib.LoadFunction<igGetTimeDelegate>("igGetTime");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed double igGetTime() => pigGetTime();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ChannelsMergeDelegate(ImDrawList* self);
        private readonly ImDrawList_ChannelsMergeDelegate pImDrawList_ChannelsMerge = lib.LoadFunction<ImDrawList_ChannelsMergeDelegate>("ImDrawList_ChannelsMerge");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_ChannelsMerge(ImDrawList* self) => pImDrawList_ChannelsMerge(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int igGetColumnIndexDelegate();
        private readonly igGetColumnIndexDelegate pigGetColumnIndex = lib.LoadFunction<igGetColumnIndexDelegate>("igGetColumnIndex");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int igGetColumnIndex() => pigGetColumnIndex();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginPopupContextItemDelegate(byte* str_id, int mouse_button);
        private readonly igBeginPopupContextItemDelegate pigBeginPopupContextItem = lib.LoadFunction<igBeginPopupContextItemDelegate>("igBeginPopupContextItem");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginPopupContextItem(byte* str_id, int mouse_button) => pigBeginPopupContextItem(str_id, mouse_button);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetCursorPosXDelegate(float x);
        private readonly igSetCursorPosXDelegate pigSetCursorPosX = lib.LoadFunction<igSetCursorPosXDelegate>("igSetCursorPosX");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetCursorPosX(float x) => pigSetCursorPosX(x);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetItemRectSizeDelegate();
        private readonly igGetItemRectSizeDelegate pigGetItemRectSize = lib.LoadFunction<igGetItemRectSizeDelegate>("igGetItemRectSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetItemRectSize() => pigGetItemRectSize();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igArrowButtonDelegate(byte* str_id, ImGuiDir dir);
        private readonly igArrowButtonDelegate pigArrowButton = lib.LoadFunction<igArrowButtonDelegate>("igArrowButton");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igArrowButton(byte* str_id, ImGuiDir dir) => pigArrowButton(str_id, dir);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImGuiMouseCursor igGetMouseCursorDelegate();
        private readonly igGetMouseCursorDelegate pigGetMouseCursor = lib.LoadFunction<igGetMouseCursorDelegate>("igGetMouseCursor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImGuiMouseCursor igGetMouseCursor() => pigGetMouseCursor();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushAllowKeyboardFocusDelegate(byte allow_keyboard_focus);
        private readonly igPushAllowKeyboardFocusDelegate pigPushAllowKeyboardFocus = lib.LoadFunction<igPushAllowKeyboardFocusDelegate>("igPushAllowKeyboardFocus");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushAllowKeyboardFocus(byte allow_keyboard_focus) => pigPushAllowKeyboardFocus(allow_keyboard_focus);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetScrollYDelegate();
        private readonly igGetScrollYDelegate pigGetScrollY = lib.LoadFunction<igGetScrollYDelegate>("igGetScrollY");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetScrollY() => pigGetScrollY();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetColumnOffsetDelegate(int column_index, float offset_x);
        private readonly igSetColumnOffsetDelegate pigSetColumnOffset = lib.LoadFunction<igSetColumnOffsetDelegate>("igSetColumnOffset");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetColumnOffset(int column_index, float offset_x) => pigSetColumnOffset(column_index, offset_x);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImGuiTextBuffer_beginDelegate(ImGuiTextBuffer* self);
        private readonly ImGuiTextBuffer_beginDelegate pImGuiTextBuffer_begin = lib.LoadFunction<ImGuiTextBuffer_beginDelegate>("ImGuiTextBuffer_begin");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImGuiTextBuffer_begin(ImGuiTextBuffer* self) => pImGuiTextBuffer_begin(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowPosVec2Delegate(Vector2 pos, ImGuiCond cond);
        private readonly igSetWindowPosVec2Delegate pigSetWindowPosVec2 = lib.LoadFunction<igSetWindowPosVec2Delegate>("igSetWindowPosVec2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowPosVec2(Vector2 pos, ImGuiCond cond) => pigSetWindowPosVec2(pos, cond);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowPosStrDelegate(byte* name, Vector2 pos, ImGuiCond cond);
        private readonly igSetWindowPosStrDelegate pigSetWindowPosStr = lib.LoadFunction<igSetWindowPosStrDelegate>("igSetWindowPosStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetWindowPosStr(byte* name, Vector2 pos, ImGuiCond cond) => pigSetWindowPosStr(name, pos, cond);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetKeyboardFocusHereDelegate(int offset);
        private readonly igSetKeyboardFocusHereDelegate pigSetKeyboardFocusHere = lib.LoadFunction<igSetKeyboardFocusHereDelegate>("igSetKeyboardFocusHere");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetKeyboardFocusHere(int offset) => pigSetKeyboardFocusHere(offset);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetCursorPosYDelegate();
        private readonly igGetCursorPosYDelegate pigGetCursorPosY = lib.LoadFunction<igGetCursorPosYDelegate>("igGetCursorPosY");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetCursorPosY() => pigGetCursorPosY();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ImFontAtlas_AddCustomRectFontGlyphDelegate(ImFontAtlas* self, ImFont* font, ushort id, int width, int height, float advance_x, Vector2 offset);
        private readonly ImFontAtlas_AddCustomRectFontGlyphDelegate pImFontAtlas_AddCustomRectFontGlyph = lib.LoadFunction<ImFontAtlas_AddCustomRectFontGlyphDelegate>("ImFontAtlas_AddCustomRectFontGlyph");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int ImFontAtlas_AddCustomRectFontGlyph(ImFontAtlas* self, ImFont* font, ushort id, int width, int height, float advance_x, Vector2 offset) => pImFontAtlas_AddCustomRectFontGlyph(self, font, id, width, height, advance_x, offset);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndMainMenuBarDelegate();
        private readonly igEndMainMenuBarDelegate pigEndMainMenuBar = lib.LoadFunction<igEndMainMenuBarDelegate>("igEndMainMenuBar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEndMainMenuBar() => pigEndMainMenuBar();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetContentRegionAvailWidthDelegate();
        private readonly igGetContentRegionAvailWidthDelegate pigGetContentRegionAvailWidth = lib.LoadFunction<igGetContentRegionAvailWidthDelegate>("igGetContentRegionAvailWidth");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetContentRegionAvailWidth() => pigGetContentRegionAvailWidth();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsKeyDownDelegate(int user_key_index);
        private readonly igIsKeyDownDelegate pigIsKeyDown = lib.LoadFunction<igIsKeyDownDelegate>("igIsKeyDown");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsKeyDown(int user_key_index) => pigIsKeyDown(user_key_index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseDownDelegate(int button);
        private readonly igIsMouseDownDelegate pigIsMouseDown = lib.LoadFunction<igIsMouseDownDelegate>("igIsMouseDown");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsMouseDown(int button) => pigIsMouseDown(button);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetWindowContentRegionMinDelegate();
        private readonly igGetWindowContentRegionMinDelegate pigGetWindowContentRegionMin = lib.LoadFunction<igGetWindowContentRegionMinDelegate>("igGetWindowContentRegionMin");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetWindowContentRegionMin() => pigGetWindowContentRegionMin();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogButtonsDelegate();
        private readonly igLogButtonsDelegate pigLogButtons = lib.LoadFunction<igLogButtonsDelegate>("igLogButtons");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogButtons() => pigLogButtons();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetWindowContentRegionWidthDelegate();
        private readonly igGetWindowContentRegionWidthDelegate pigGetWindowContentRegionWidth = lib.LoadFunction<igGetWindowContentRegionWidthDelegate>("igGetWindowContentRegionWidth");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetWindowContentRegionWidth() => pigGetWindowContentRegionWidth();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderAngleDelegate(byte* label, float* v_rad, float v_degrees_min, float v_degrees_max);
        private readonly igSliderAngleDelegate pigSliderAngle = lib.LoadFunction<igSliderAngleDelegate>("igSliderAngle");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderAngle(byte* label, float* v_rad, float v_degrees_min, float v_degrees_max) => pigSliderAngle(label, v_rad, v_degrees_min, v_degrees_max);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodeExStrDelegate(byte* label, ImGuiTreeNodeFlags flags);
        private readonly igTreeNodeExStrDelegate pigTreeNodeExStr = lib.LoadFunction<igTreeNodeExStrDelegate>("igTreeNodeExStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodeExStr(byte* label, ImGuiTreeNodeFlags flags) => pigTreeNodeExStr(label, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodeExStrStrDelegate(byte* str_id, ImGuiTreeNodeFlags flags, byte* fmt);
        private readonly igTreeNodeExStrStrDelegate pigTreeNodeExStrStr = lib.LoadFunction<igTreeNodeExStrStrDelegate>("igTreeNodeExStrStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodeExStrStr(byte* str_id, ImGuiTreeNodeFlags flags, byte* fmt) => pigTreeNodeExStrStr(str_id, flags, fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodeExPtrDelegate(void* ptr_id, ImGuiTreeNodeFlags flags, byte* fmt);
        private readonly igTreeNodeExPtrDelegate pigTreeNodeExPtr = lib.LoadFunction<igTreeNodeExPtrDelegate>("igTreeNodeExPtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodeExPtr(void* ptr_id, ImGuiTreeNodeFlags flags, byte* fmt) => pigTreeNodeExPtr(ptr_id, flags, fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetWindowWidthDelegate();
        private readonly igGetWindowWidthDelegate pigGetWindowWidth = lib.LoadFunction<igGetWindowWidthDelegate>("igGetWindowWidth");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetWindowWidth() => pigGetWindowWidth();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushTextWrapPosDelegate(float wrap_pos_x);
        private readonly igPushTextWrapPosDelegate pigPushTextWrapPos = lib.LoadFunction<igPushTextWrapPosDelegate>("igPushTextWrapPos");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushTextWrapPos(float wrap_pos_x) => pigPushTextWrapPos(wrap_pos_x);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ImGuiStorage_GetIntDelegate(ImGuiStorage* self, uint key, int default_val);
        private readonly ImGuiStorage_GetIntDelegate pImGuiStorage_GetInt = lib.LoadFunction<ImGuiStorage_GetIntDelegate>("ImGuiStorage_GetInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed int ImGuiStorage_GetInt(ImGuiStorage* self, uint key, int default_val) => pImGuiStorage_GetInt(self, key, default_val);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderInt3Delegate(byte* label, int* v, int v_min, int v_max, byte* format);
        private readonly igSliderInt3Delegate pigSliderInt3 = lib.LoadFunction<igSliderInt3Delegate>("igSliderInt3");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderInt3(byte* label, int* v, int v_min, int v_max, byte* format) => pigSliderInt3(label, v, v_min, v_max, format);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igShowUserGuideDelegate();
        private readonly igShowUserGuideDelegate pigShowUserGuide = lib.LoadFunction<igShowUserGuideDelegate>("igShowUserGuide");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igShowUserGuide() => pigShowUserGuide();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderScalarNDelegate(byte* label, ImGuiDataType data_type, void* v, int components, void* v_min, void* v_max, byte* format, float power);
        private readonly igSliderScalarNDelegate pigSliderScalarN = lib.LoadFunction<igSliderScalarNDelegate>("igSliderScalarN");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igSliderScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* v_min, void* v_max, byte* format, float power) => pigSliderScalarN(label, data_type, v, components, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImColor ImColor_HSVDelegate(ImColor* self, float h, float s, float v, float a);
        private readonly ImColor_HSVDelegate pImColor_HSV = lib.LoadFunction<ImColor_HSVDelegate>("ImColor_HSV");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed ImColor ImColor_HSV(ImColor* self, float h, float s, float v, float a) => pImColor_HSV(self, h, s, v, a);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathLineToDelegate(ImDrawList* self, Vector2 pos);
        private readonly ImDrawList_PathLineToDelegate pImDrawList_PathLineTo = lib.LoadFunction<ImDrawList_PathLineToDelegate>("ImDrawList_PathLineTo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PathLineTo(ImDrawList* self, Vector2 pos) => pImDrawList_PathLineTo(self, pos);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igImageDelegate(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col);
        private readonly igImageDelegate pigImage = lib.LoadFunction<igImageDelegate>("igImage");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igImage(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col) => pigImage(user_texture_id, size, uv0, uv1, tint_col, border_col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowSizeConstraintsDelegate(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback, void* custom_callback_data);
        private readonly igSetNextWindowSizeConstraintsDelegate pigSetNextWindowSizeConstraints = lib.LoadFunction<igSetNextWindowSizeConstraintsDelegate>("igSetNextWindowSizeConstraints");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback, void* custom_callback_data) => pigSetNextWindowSizeConstraints(size_min, size_max, custom_callback, custom_callback_data);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igDummyDelegate(Vector2 size);
        private readonly igDummyDelegate pigDummy = lib.LoadFunction<igDummyDelegate>("igDummy");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igDummy(Vector2 size) => pigDummy(size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igVSliderIntDelegate(byte* label, Vector2 size, int* v, int v_min, int v_max, byte* format);
        private readonly igVSliderIntDelegate pigVSliderInt = lib.LoadFunction<igVSliderIntDelegate>("igVSliderInt");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igVSliderInt(byte* label, Vector2 size, int* v, int v_min, int v_max, byte* format) => pigVSliderInt(label, size, v, v_min, v_max, format);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextBuffer_ImGuiTextBufferDelegate(ImGuiTextBuffer* self);
        private readonly ImGuiTextBuffer_ImGuiTextBufferDelegate pImGuiTextBuffer_ImGuiTextBuffer = lib.LoadFunction<ImGuiTextBuffer_ImGuiTextBufferDelegate>("ImGuiTextBuffer_ImGuiTextBuffer");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImGuiTextBuffer_ImGuiTextBuffer(ImGuiTextBuffer* self) => pImGuiTextBuffer_ImGuiTextBuffer(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igBulletTextDelegate(byte* fmt);
        private readonly igBulletTextDelegate pigBulletText = lib.LoadFunction<igBulletTextDelegate>("igBulletText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igBulletText(byte* fmt) => pigBulletText(fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igColorEdit4Delegate(byte* label, Vector4* col, ImGuiColorEditFlags flags);
        private readonly igColorEdit4Delegate pigColorEdit4 = lib.LoadFunction<igColorEdit4Delegate>("igColorEdit4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igColorEdit4(byte* label, Vector4* col, ImGuiColorEditFlags flags) => pigColorEdit4(label, col, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igColorPicker4Delegate(byte* label, Vector4* col, ImGuiColorEditFlags flags, float* ref_col);
        private readonly igColorPicker4Delegate pigColorPicker4 = lib.LoadFunction<igColorPicker4Delegate>("igColorPicker4");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igColorPicker4(byte* label, Vector4* col, ImGuiColorEditFlags flags, float* ref_col) => pigColorPicker4(label, col, flags, ref_col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimRectUVDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);
        private readonly ImDrawList_PrimRectUVDelegate pImDrawList_PrimRectUV = lib.LoadFunction<ImDrawList_PrimRectUVDelegate>("ImDrawList_PrimRectUV");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawList_PrimRectUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col) => pImDrawList_PrimRectUV(self, a, b, uv_a, uv_b, col);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInvisibleButtonDelegate(byte* str_id, Vector2 size);
        private readonly igInvisibleButtonDelegate pigInvisibleButton = lib.LoadFunction<igInvisibleButtonDelegate>("igInvisibleButton");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInvisibleButton(byte* str_id, Vector2 size) => pigInvisibleButton(str_id, size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogToClipboardDelegate(int max_depth);
        private readonly igLogToClipboardDelegate pigLogToClipboard = lib.LoadFunction<igLogToClipboardDelegate>("igLogToClipboard");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igLogToClipboard(int max_depth) => pigLogToClipboard(max_depth);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginPopupContextWindowDelegate(byte* str_id, int mouse_button, byte also_over_items);
        private readonly igBeginPopupContextWindowDelegate pigBeginPopupContextWindow = lib.LoadFunction<igBeginPopupContextWindowDelegate>("igBeginPopupContextWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igBeginPopupContextWindow(byte* str_id, int mouse_button, byte also_over_items) => pigBeginPopupContextWindow(str_id, mouse_button, also_over_items);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_ImFontAtlasDelegate(ImFontAtlas* self);
        private readonly ImFontAtlas_ImFontAtlasDelegate pImFontAtlas_ImFontAtlas = lib.LoadFunction<ImFontAtlas_ImFontAtlasDelegate>("ImFontAtlas_ImFontAtlas");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImFontAtlas_ImFontAtlas(ImFontAtlas* self) => pImFontAtlas_ImFontAtlas(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragScalarDelegate(byte* label, ImGuiDataType data_type, void* v, float v_speed, void* v_min, void* v_max, byte* format, float power);
        private readonly igDragScalarDelegate pigDragScalar = lib.LoadFunction<igDragScalarDelegate>("igDragScalar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragScalar(byte* label, ImGuiDataType data_type, void* v, float v_speed, void* v_min, void* v_max, byte* format, float power) => pigDragScalar(label, data_type, v, v_speed, v_min, v_max, format, power);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetItemDefaultFocusDelegate();
        private readonly igSetItemDefaultFocusDelegate pigSetItemDefaultFocus = lib.LoadFunction<igSetItemDefaultFocusDelegate>("igSetItemDefaultFocus");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetItemDefaultFocus() => pigSetItemDefaultFocus();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igCaptureMouseFromAppDelegate(byte capture);
        private readonly igCaptureMouseFromAppDelegate pigCaptureMouseFromApp = lib.LoadFunction<igCaptureMouseFromAppDelegate>("igCaptureMouseFromApp");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igCaptureMouseFromApp(byte capture) => pigCaptureMouseFromApp(capture);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsAnyItemHoveredDelegate();
        private readonly igIsAnyItemHoveredDelegate pigIsAnyItemHovered = lib.LoadFunction<igIsAnyItemHoveredDelegate>("igIsAnyItemHovered");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igIsAnyItemHovered() => pigIsAnyItemHovered();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushFontDelegate(ImFont* font);
        private readonly igPushFontDelegate pigPushFont = lib.LoadFunction<igPushFontDelegate>("igPushFont");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPushFont(ImFont* font) => pigPushFont(font);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputInt2Delegate(byte* label, int* v, ImGuiInputTextFlags extra_flags);
        private readonly igInputInt2Delegate pigInputInt2 = lib.LoadFunction<igInputInt2Delegate>("igInputInt2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputInt2(byte* label, int* v, ImGuiInputTextFlags extra_flags) => pigInputInt2(label, v, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTreePopDelegate();
        private readonly igTreePopDelegate pigTreePop = lib.LoadFunction<igTreePopDelegate>("igTreePop");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igTreePop() => pigTreePop();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndDelegate();
        private readonly igEndDelegate pigEnd = lib.LoadFunction<igEndDelegate>("igEnd");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igEnd() => pigEnd();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawData_ImDrawDataDelegate(ImDrawData* self);
        private readonly ImDrawData_ImDrawDataDelegate pImDrawData_ImDrawData = lib.LoadFunction<ImDrawData_ImDrawDataDelegate>("ImDrawData_ImDrawData");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void ImDrawData_ImDrawData(ImDrawData* self) => pImDrawData_ImDrawData(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igDestroyContextDelegate(IntPtr ctx);
        private readonly igDestroyContextDelegate pigDestroyContext = lib.LoadFunction<igDestroyContextDelegate>("igDestroyContext");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igDestroyContext(IntPtr ctx) => pigDestroyContext(ctx);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImGuiTextBuffer_endDelegate(ImGuiTextBuffer* self);
        private readonly ImGuiTextBuffer_endDelegate pImGuiTextBuffer_end = lib.LoadFunction<ImGuiTextBuffer_endDelegate>("ImGuiTextBuffer_end");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte* ImGuiTextBuffer_end(ImGuiTextBuffer* self) => pImGuiTextBuffer_end(self);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopStyleVarDelegate(int count);
        private readonly igPopStyleVarDelegate pigPopStyleVar = lib.LoadFunction<igPopStyleVarDelegate>("igPopStyleVar");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igPopStyleVar(int count) => pigPopStyleVar(count);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiTextFilter_PassFilterDelegate(ImGuiTextFilter* self, byte* text, byte* text_end);
        private readonly ImGuiTextFilter_PassFilterDelegate pImGuiTextFilter_PassFilter = lib.LoadFunction<ImGuiTextFilter_PassFilterDelegate>("ImGuiTextFilter_PassFilter");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte ImGuiTextFilter_PassFilter(ImGuiTextFilter* self, byte* text, byte* text_end) => pImGuiTextFilter_PassFilter(self, text, text_end);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igShowStyleSelectorDelegate(byte* label);
        private readonly igShowStyleSelectorDelegate pigShowStyleSelector = lib.LoadFunction<igShowStyleSelectorDelegate>("igShowStyleSelector");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igShowStyleSelector(byte* label) => pigShowStyleSelector(label);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputScalarNDelegate(byte* label, ImGuiDataType data_type, void* v, int components, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        private readonly igInputScalarNDelegate pigInputScalarN = lib.LoadFunction<igInputScalarNDelegate>("igInputScalarN");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags) => pigInputScalarN(label, data_type, v, components, step, step_fast, format, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodeStrDelegate(byte* label);
        private readonly igTreeNodeStrDelegate pigTreeNodeStr = lib.LoadFunction<igTreeNodeStrDelegate>("igTreeNodeStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodeStr(byte* label) => pigTreeNodeStr(label);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodeStrStrDelegate(byte* str_id, byte* fmt);
        private readonly igTreeNodeStrStrDelegate pigTreeNodeStrStr = lib.LoadFunction<igTreeNodeStrStrDelegate>("igTreeNodeStrStr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodeStrStr(byte* str_id, byte* fmt) => pigTreeNodeStrStr(str_id, fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodePtrDelegate(void* ptr_id, byte* fmt);
        private readonly igTreeNodePtrDelegate pigTreeNodePtr = lib.LoadFunction<igTreeNodePtrDelegate>("igTreeNodePtr");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igTreeNodePtr(void* ptr_id, byte* fmt) => pigTreeNodePtr(ptr_id, fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetScrollMaxXDelegate();
        private readonly igGetScrollMaxXDelegate pigGetScrollMaxX = lib.LoadFunction<igGetScrollMaxXDelegate>("igGetScrollMaxX");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed float igGetScrollMaxX() => pigGetScrollMaxX();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetTooltipDelegate(byte* fmt);
        private readonly igSetTooltipDelegate pigSetTooltip = lib.LoadFunction<igSetTooltipDelegate>("igSetTooltip");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void igSetTooltip(byte* fmt) => pigSetTooltip(fmt);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetContentRegionAvailDelegate();
        private readonly igGetContentRegionAvailDelegate pigGetContentRegionAvail = lib.LoadFunction<igGetContentRegionAvailDelegate>("igGetContentRegionAvail");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Vector2 igGetContentRegionAvail() => pigGetContentRegionAvail();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputFloat3Delegate(byte* label, Vector3* v, byte* format, ImGuiInputTextFlags extra_flags);
        private readonly igInputFloat3Delegate pigInputFloat3 = lib.LoadFunction<igInputFloat3Delegate>("igInputFloat3");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igInputFloat3(byte* label, Vector3* v, byte* format, ImGuiInputTextFlags extra_flags) => pigInputFloat3(label, v, format, extra_flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragFloat2Delegate(byte* label, Vector2* v, float v_speed, float v_min, float v_max, byte* format, float power);
        private readonly igDragFloat2Delegate pigDragFloat2 = lib.LoadFunction<igDragFloat2Delegate>("igDragFloat2");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed byte igDragFloat2(byte* label, Vector2* v, float v_speed, float v_min, float v_max, byte* format, float power) => pigDragFloat2(label, v, v_speed, v_min, v_max, format, power);
    }
#pragma warning restore 1591
}
