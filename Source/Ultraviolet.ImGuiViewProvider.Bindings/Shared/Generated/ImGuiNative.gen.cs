using System;
using System.Runtime.InteropServices;
using System.Security;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.ImGuiViewProvider.Bindings
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    public static unsafe partial class ImGuiNative
    {
#if ANDROID
        const String LIBRARY = "cimgui";
#elif IOS
        const String LIBRARY = "__Internal";
#else
        private static readonly NativeLibrary lib = new NativeLibrary (
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ? "cimgui" : "libcimgui");
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetFrameHeight();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetFrameHeightDelegate();
        private static readonly igGetFrameHeightDelegate pigGetFrameHeight = lib.LoadFunction<igGetFrameHeightDelegate>("igGetFrameHeight");
        public static float igGetFrameHeight() => pigGetFrameHeight();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr igCreateContext(ImFontAtlas* shared_font_atlas);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr igCreateContextDelegate(ImFontAtlas* shared_font_atlas);
        private static readonly igCreateContextDelegate pigCreateContext = lib.LoadFunction<igCreateContextDelegate>("igCreateContext");
        public static IntPtr igCreateContext(ImFontAtlas* shared_font_atlas) => pigCreateContext(shared_font_atlas);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igTextUnformatted(byte* text, byte* text_end);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTextUnformattedDelegate(byte* text, byte* text_end);
        private static readonly igTextUnformattedDelegate pigTextUnformatted = lib.LoadFunction<igTextUnformattedDelegate>("igTextUnformatted");
        public static void igTextUnformatted(byte* text, byte* text_end) => pigTextUnformatted(text, text_end);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPopFont();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopFontDelegate();
        private static readonly igPopFontDelegate pigPopFont = lib.LoadFunction<igPopFontDelegate>("igPopFont");
        public static void igPopFont() => pigPopFont();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igCombo(byte* label, int* current_item, byte** items, int items_count, int popup_max_height_in_items);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igComboDelegate(byte* label, int* current_item, byte** items, int items_count, int popup_max_height_in_items);
        private static readonly igComboDelegate pigCombo = lib.LoadFunction<igComboDelegate>("igCombo");
        public static byte igCombo(byte* label, int* current_item, byte** items, int items_count, int popup_max_height_in_items) => pigCombo(label, current_item, items, items_count, popup_max_height_in_items);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igComboStr(byte* label, int* current_item, byte* items_separated_by_zeros, int popup_max_height_in_items);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igComboStrDelegate(byte* label, int* current_item, byte* items_separated_by_zeros, int popup_max_height_in_items);
        private static readonly igComboStrDelegate pigComboStr = lib.LoadFunction<igComboStrDelegate>("igComboStr");
        public static byte igComboStr(byte* label, int* current_item, byte* items_separated_by_zeros, int popup_max_height_in_items) => pigComboStr(label, current_item, items_separated_by_zeros, popup_max_height_in_items);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igCaptureKeyboardFromApp(byte capture);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igCaptureKeyboardFromAppDelegate(byte capture);
        private static readonly igCaptureKeyboardFromAppDelegate pigCaptureKeyboardFromApp = lib.LoadFunction<igCaptureKeyboardFromAppDelegate>("igCaptureKeyboardFromApp");
        public static void igCaptureKeyboardFromApp(byte capture) => pigCaptureKeyboardFromApp(capture);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsWindowFocused(ImGuiFocusedFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsWindowFocusedDelegate(ImGuiFocusedFlags flags);
        private static readonly igIsWindowFocusedDelegate pigIsWindowFocused = lib.LoadFunction<igIsWindowFocusedDelegate>("igIsWindowFocused");
        public static byte igIsWindowFocused(ImGuiFocusedFlags flags) => pigIsWindowFocused(flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igRender();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igRenderDelegate();
        private static readonly igRenderDelegate pigRender = lib.LoadFunction<igRenderDelegate>("igRender");
        public static void igRender() => pigRender();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_ChannelsSetCurrent(ImDrawList* self, int channel_index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ChannelsSetCurrentDelegate(ImDrawList* self, int channel_index);
        private static readonly ImDrawList_ChannelsSetCurrentDelegate pImDrawList_ChannelsSetCurrent = lib.LoadFunction<ImDrawList_ChannelsSetCurrentDelegate>("ImDrawList_ChannelsSetCurrent");
        public static void ImDrawList_ChannelsSetCurrent(ImDrawList* self, int channel_index) => pImDrawList_ChannelsSetCurrent(self, channel_index);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragFloat4(byte* label, Vector4* v, float v_speed, float v_min, float v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragFloat4Delegate(byte* label, Vector4* v, float v_speed, float v_min, float v_max, byte* format, float power);
        private static readonly igDragFloat4Delegate pigDragFloat4 = lib.LoadFunction<igDragFloat4Delegate>("igDragFloat4");
        public static byte igDragFloat4(byte* label, Vector4* v, float v_speed, float v_min, float v_max, byte* format, float power) => pigDragFloat4(label, v, v_speed, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_ChannelsSplit(ImDrawList* self, int channels_count);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ChannelsSplitDelegate(ImDrawList* self, int channels_count);
        private static readonly ImDrawList_ChannelsSplitDelegate pImDrawList_ChannelsSplit = lib.LoadFunction<ImDrawList_ChannelsSplitDelegate>("ImDrawList_ChannelsSplit");
        public static void ImDrawList_ChannelsSplit(ImDrawList* self, int channels_count) => pImDrawList_ChannelsSplit(self, channels_count);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsMousePosValid(Vector2* mouse_pos);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMousePosValidDelegate(Vector2* mouse_pos);
        private static readonly igIsMousePosValidDelegate pigIsMousePosValid = lib.LoadFunction<igIsMousePosValidDelegate>("igIsMousePosValid");
        public static byte igIsMousePosValid(Vector2* mouse_pos) => pigIsMousePosValid(mouse_pos);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetCursorScreenPos_nonUDT2")]
        public static extern Vector2 igGetCursorScreenPos();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetCursorScreenPosDelegate();
        private static readonly igGetCursorScreenPosDelegate pigGetCursorScreenPos = lib.LoadFunction<igGetCursorScreenPosDelegate>("igGetCursorScreenPos");
        public static Vector2 igGetCursorScreenPos() => pigGetCursorScreenPos();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDebugCheckVersionAndDataLayout(byte* version_str, uint sz_io, uint sz_style, uint sz_vec2, uint sz_vec4, uint sz_drawvert);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDebugCheckVersionAndDataLayoutDelegate(byte* version_str, uint sz_io, uint sz_style, uint sz_vec2, uint sz_vec4, uint sz_drawvert);
        private static readonly igDebugCheckVersionAndDataLayoutDelegate pigDebugCheckVersionAndDataLayout = lib.LoadFunction<igDebugCheckVersionAndDataLayoutDelegate>("igDebugCheckVersionAndDataLayout");
        public static byte igDebugCheckVersionAndDataLayout(byte* version_str, uint sz_io, uint sz_style, uint sz_vec2, uint sz_vec4, uint sz_drawvert) => pigDebugCheckVersionAndDataLayout(version_str, sz_io, sz_style, sz_vec2, sz_vec4, sz_drawvert);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetScrollHere(float center_y_ratio);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetScrollHereDelegate(float center_y_ratio);
        private static readonly igSetScrollHereDelegate pigSetScrollHere = lib.LoadFunction<igSetScrollHereDelegate>("igSetScrollHere");
        public static void igSetScrollHere(float center_y_ratio) => pigSetScrollHere(center_y_ratio);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetScrollY(float scroll_y);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetScrollYDelegate(float scroll_y);
        private static readonly igSetScrollYDelegate pigSetScrollY = lib.LoadFunction<igSetScrollYDelegate>("igSetScrollY");
        public static void igSetScrollY(float scroll_y) => pigSetScrollY(scroll_y);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetColorEditOptions(ImGuiColorEditFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetColorEditOptionsDelegate(ImGuiColorEditFlags flags);
        private static readonly igSetColorEditOptionsDelegate pigSetColorEditOptions = lib.LoadFunction<igSetColorEditOptionsDelegate>("igSetColorEditOptions");
        public static void igSetColorEditOptions(ImGuiColorEditFlags flags) => pigSetColorEditOptions(flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetScrollFromPosY(float pos_y, float center_y_ratio);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetScrollFromPosYDelegate(float pos_y, float center_y_ratio);
        private static readonly igSetScrollFromPosYDelegate pigSetScrollFromPosY = lib.LoadFunction<igSetScrollFromPosYDelegate>("igSetScrollFromPosY");
        public static void igSetScrollFromPosY(float pos_y, float center_y_ratio) => pigSetScrollFromPosY(pos_y, center_y_ratio);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern Vector4* igGetStyleColorVec4(ImGuiCol idx);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector4* igGetStyleColorVec4Delegate(ImGuiCol idx);
        private static readonly igGetStyleColorVec4Delegate pigGetStyleColorVec4 = lib.LoadFunction<igGetStyleColorVec4Delegate>("igGetStyleColorVec4");
        public static Vector4* igGetStyleColorVec4(ImGuiCol idx) => pigGetStyleColorVec4(idx);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsMouseHoveringRect(Vector2 r_min, Vector2 r_max, byte clip);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseHoveringRectDelegate(Vector2 r_min, Vector2 r_max, byte clip);
        private static readonly igIsMouseHoveringRectDelegate pigIsMouseHoveringRect = lib.LoadFunction<igIsMouseHoveringRectDelegate>("igIsMouseHoveringRect");
        public static byte igIsMouseHoveringRect(Vector2 r_min, Vector2 r_max, byte clip) => pigIsMouseHoveringRect(r_min, r_max, clip);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImVec4_ImVec4(Vector4* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImVec4_ImVec4Delegate(Vector4* self);
        private static readonly ImVec4_ImVec4Delegate pImVec4_ImVec4 = lib.LoadFunction<ImVec4_ImVec4Delegate>("ImVec4_ImVec4");
        public static void ImVec4_ImVec4(Vector4* self) => pImVec4_ImVec4(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImVec4_ImVec4Float(Vector4* self, float _x, float _y, float _z, float _w);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImVec4_ImVec4FloatDelegate(Vector4* self, float _x, float _y, float _z, float _w);
        private static readonly ImVec4_ImVec4FloatDelegate pImVec4_ImVec4Float = lib.LoadFunction<ImVec4_ImVec4FloatDelegate>("ImVec4_ImVec4Float");
        public static void ImVec4_ImVec4Float(Vector4* self, float _x, float _y, float _z, float _w) => pImVec4_ImVec4Float(self, _x, _y, _z, _w);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImColor_SetHSV(ImColor* self, float h, float s, float v, float a);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_SetHSVDelegate(ImColor* self, float h, float s, float v, float a);
        private static readonly ImColor_SetHSVDelegate pImColor_SetHSV = lib.LoadFunction<ImColor_SetHSVDelegate>("ImColor_SetHSV");
        public static void ImColor_SetHSV(ImColor* self, float h, float s, float v, float a) => pImColor_SetHSV(self, h, s, v, a);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragFloat3(byte* label, Vector3* v, float v_speed, float v_min, float v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragFloat3Delegate(byte* label, Vector3* v, float v_speed, float v_min, float v_max, byte* format, float power);
        private static readonly igDragFloat3Delegate pigDragFloat3 = lib.LoadFunction<igDragFloat3Delegate>("igDragFloat3");
        public static byte igDragFloat3(byte* label, Vector3* v, float v_speed, float v_min, float v_max, byte* format, float power) => pigDragFloat3(label, v, v_speed, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddPolyline(ImDrawList* self, Vector2* points, int num_points, uint col, byte closed, float thickness);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddPolylineDelegate(ImDrawList* self, Vector2* points, int num_points, uint col, byte closed, float thickness);
        private static readonly ImDrawList_AddPolylineDelegate pImDrawList_AddPolyline = lib.LoadFunction<ImDrawList_AddPolylineDelegate>("ImDrawList_AddPolyline");
        public static void ImDrawList_AddPolyline(ImDrawList* self, Vector2* points, int num_points, uint col, byte closed, float thickness) => pImDrawList_AddPolyline(self, points, num_points, col, closed, thickness);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igValueBool(byte* prefix, byte b);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igValueBoolDelegate(byte* prefix, byte b);
        private static readonly igValueBoolDelegate pigValueBool = lib.LoadFunction<igValueBoolDelegate>("igValueBool");
        public static void igValueBool(byte* prefix, byte b) => pigValueBool(prefix, b);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igValueInt(byte* prefix, int v);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igValueIntDelegate(byte* prefix, int v);
        private static readonly igValueIntDelegate pigValueInt = lib.LoadFunction<igValueIntDelegate>("igValueInt");
        public static void igValueInt(byte* prefix, int v) => pigValueInt(prefix, v);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igValueUint(byte* prefix, uint v);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igValueUintDelegate(byte* prefix, uint v);
        private static readonly igValueUintDelegate pigValueUint = lib.LoadFunction<igValueUintDelegate>("igValueUint");
        public static void igValueUint(byte* prefix, uint v) => pigValueUint(prefix, v);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igValueFloat(byte* prefix, float v, byte* float_format);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igValueFloatDelegate(byte* prefix, float v, byte* float_format);
        private static readonly igValueFloatDelegate pigValueFloat = lib.LoadFunction<igValueFloatDelegate>("igValueFloat");
        public static void igValueFloat(byte* prefix, float v, byte* float_format) => pigValueFloat(prefix, v, float_format);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiTextFilter_Build(ImGuiTextFilter* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextFilter_BuildDelegate(ImGuiTextFilter* self);
        private static readonly ImGuiTextFilter_BuildDelegate pImGuiTextFilter_Build = lib.LoadFunction<ImGuiTextFilter_BuildDelegate>("ImGuiTextFilter_Build");
        public static void ImGuiTextFilter_Build(ImGuiTextFilter* self) => pImGuiTextFilter_Build(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetItemRectMax_nonUDT2")]
        public static extern Vector2 igGetItemRectMax();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetItemRectMaxDelegate();
        private static readonly igGetItemRectMaxDelegate pigGetItemRectMax = lib.LoadFunction<igGetItemRectMaxDelegate>("igGetItemRectMax");
        public static Vector2 igGetItemRectMax() => pigGetItemRectMax();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsItemDeactivated();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemDeactivatedDelegate();
        private static readonly igIsItemDeactivatedDelegate pigIsItemDeactivated = lib.LoadFunction<igIsItemDeactivatedDelegate>("igIsItemDeactivated");
        public static byte igIsItemDeactivated() => pigIsItemDeactivated();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushStyleVarFloat(ImGuiStyleVar idx, float val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushStyleVarFloatDelegate(ImGuiStyleVar idx, float val);
        private static readonly igPushStyleVarFloatDelegate pigPushStyleVarFloat = lib.LoadFunction<igPushStyleVarFloatDelegate>("igPushStyleVarFloat");
        public static void igPushStyleVarFloat(ImGuiStyleVar idx, float val) => pigPushStyleVarFloat(idx, val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushStyleVarVec2(ImGuiStyleVar idx, Vector2 val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushStyleVarVec2Delegate(ImGuiStyleVar idx, Vector2 val);
        private static readonly igPushStyleVarVec2Delegate pigPushStyleVarVec2 = lib.LoadFunction<igPushStyleVarVec2Delegate>("igPushStyleVarVec2");
        public static void igPushStyleVarVec2(ImGuiStyleVar idx, Vector2 val) => pigPushStyleVarVec2(idx, val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* igSaveIniSettingsToMemory(uint* out_ini_size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* igSaveIniSettingsToMemoryDelegate(uint* out_ini_size);
        private static readonly igSaveIniSettingsToMemoryDelegate pigSaveIniSettingsToMemory = lib.LoadFunction<igSaveIniSettingsToMemoryDelegate>("igSaveIniSettingsToMemory");
        public static byte* igSaveIniSettingsToMemory(uint* out_ini_size) => pigSaveIniSettingsToMemory(out_ini_size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragIntRange2(byte* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, byte* format, byte* format_max);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragIntRange2Delegate(byte* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, byte* format, byte* format_max);
        private static readonly igDragIntRange2Delegate pigDragIntRange2 = lib.LoadFunction<igDragIntRange2Delegate>("igDragIntRange2");
        public static byte igDragIntRange2(byte* label, int* v_current_min, int* v_current_max, float v_speed, int v_min, int v_max, byte* format, byte* format_max) => pigDragIntRange2(label, v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igUnindent(float indent_w);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igUnindentDelegate(float indent_w);
        private static readonly igUnindentDelegate pigUnindent = lib.LoadFunction<igUnindentDelegate>("igUnindent");
        public static void igUnindent(float indent_w) => pigUnindent(indent_w);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImFont* ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(ImFontAtlas* self, byte* compressed_font_data_base85, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontFromMemoryCompressedBase85TTFDelegate(ImFontAtlas* self, byte* compressed_font_data_base85, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        private static readonly ImFontAtlas_AddFontFromMemoryCompressedBase85TTFDelegate pImFontAtlas_AddFontFromMemoryCompressedBase85TTF = lib.LoadFunction<ImFontAtlas_AddFontFromMemoryCompressedBase85TTFDelegate>("ImFontAtlas_AddFontFromMemoryCompressedBase85TTF");
        public static ImFont* ImFontAtlas_AddFontFromMemoryCompressedBase85TTF(ImFontAtlas* self, byte* compressed_font_data_base85, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => pImFontAtlas_AddFontFromMemoryCompressedBase85TTF(self, compressed_font_data_base85, size_pixels, font_cfg, glyph_ranges);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPopAllowKeyboardFocus();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopAllowKeyboardFocusDelegate();
        private static readonly igPopAllowKeyboardFocusDelegate pigPopAllowKeyboardFocus = lib.LoadFunction<igPopAllowKeyboardFocusDelegate>("igPopAllowKeyboardFocus");
        public static void igPopAllowKeyboardFocus() => pigPopAllowKeyboardFocus();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igLoadIniSettingsFromDisk(byte* ini_filename);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLoadIniSettingsFromDiskDelegate(byte* ini_filename);
        private static readonly igLoadIniSettingsFromDiskDelegate pigLoadIniSettingsFromDisk = lib.LoadFunction<igLoadIniSettingsFromDiskDelegate>("igLoadIniSettingsFromDisk");
        public static void igLoadIniSettingsFromDisk(byte* ini_filename) => pigLoadIniSettingsFromDisk(ini_filename);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetCursorStartPos_nonUDT2")]
        public static extern Vector2 igGetCursorStartPos();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetCursorStartPosDelegate();
        private static readonly igGetCursorStartPosDelegate pigGetCursorStartPos = lib.LoadFunction<igGetCursorStartPosDelegate>("igGetCursorStartPos");
        public static Vector2 igGetCursorStartPos() => pigGetCursorStartPos();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetCursorScreenPos(Vector2 screen_pos);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetCursorScreenPosDelegate(Vector2 screen_pos);
        private static readonly igSetCursorScreenPosDelegate pigSetCursorScreenPos = lib.LoadFunction<igSetCursorScreenPosDelegate>("igSetCursorScreenPos");
        public static void igSetCursorScreenPos(Vector2 screen_pos) => pigSetCursorScreenPos(screen_pos);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputInt4(byte* label, int* v, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputInt4Delegate(byte* label, int* v, ImGuiInputTextFlags extra_flags);
        private static readonly igInputInt4Delegate pigInputInt4 = lib.LoadFunction<igInputInt4Delegate>("igInputInt4");
        public static byte igInputInt4(byte* label, int* v, ImGuiInputTextFlags extra_flags) => pigInputInt4(label, v, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFont_AddRemapChar(ImFont* self, ushort dst, ushort src, byte overwrite_dst);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_AddRemapCharDelegate(ImFont* self, ushort dst, ushort src, byte overwrite_dst);
        private static readonly ImFont_AddRemapCharDelegate pImFont_AddRemapChar = lib.LoadFunction<ImFont_AddRemapCharDelegate>("ImFont_AddRemapChar");
        public static void ImFont_AddRemapChar(ImFont* self, ushort dst, ushort src, byte overwrite_dst) => pImFont_AddRemapChar(self, dst, src, overwrite_dst);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFont_AddGlyph(ImFont* self, ushort c, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1, float advance_x);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_AddGlyphDelegate(ImFont* self, ushort c, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1, float advance_x);
        private static readonly ImFont_AddGlyphDelegate pImFont_AddGlyph = lib.LoadFunction<ImFont_AddGlyphDelegate>("ImFont_AddGlyph");
        public static void ImFont_AddGlyph(ImFont* self, ushort c, float x0, float y0, float x1, float y1, float u0, float v0, float u1, float v1, float advance_x) => pImFont_AddGlyph(self, c, x0, y0, x1, y1, u0, v0, u1, v1, advance_x);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsRectVisible(Vector2 size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsRectVisibleDelegate(Vector2 size);
        private static readonly igIsRectVisibleDelegate pigIsRectVisible = lib.LoadFunction<igIsRectVisibleDelegate>("igIsRectVisible");
        public static byte igIsRectVisible(Vector2 size) => pigIsRectVisible(size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsRectVisibleVec2(Vector2 rect_min, Vector2 rect_max);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsRectVisibleVec2Delegate(Vector2 rect_min, Vector2 rect_max);
        private static readonly igIsRectVisibleVec2Delegate pigIsRectVisibleVec2 = lib.LoadFunction<igIsRectVisibleVec2Delegate>("igIsRectVisibleVec2");
        public static byte igIsRectVisibleVec2(Vector2 rect_min, Vector2 rect_max) => pigIsRectVisibleVec2(rect_min, rect_max);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFont_GrowIndex(ImFont* self, int new_size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_GrowIndexDelegate(ImFont* self, int new_size);
        private static readonly ImFont_GrowIndexDelegate pImFont_GrowIndex = lib.LoadFunction<ImFont_GrowIndexDelegate>("ImFont_GrowIndex");
        public static void ImFont_GrowIndex(ImFont* self, int new_size) => pImFont_GrowIndex(self, new_size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImFontAtlas_Build(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImFontAtlas_BuildDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_BuildDelegate pImFontAtlas_Build = lib.LoadFunction<ImFontAtlas_BuildDelegate>("ImFontAtlas_Build");
        public static byte ImFontAtlas_Build(ImFontAtlas* self) => pImFontAtlas_Build(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igLabelText(byte* label, byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLabelTextDelegate(byte* label, byte* fmt);
        private static readonly igLabelTextDelegate pigLabelText = lib.LoadFunction<igLabelTextDelegate>("igLabelText");
        public static void igLabelText(byte* label, byte* fmt) => pigLabelText(label, fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFont_RenderText(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, byte* text_begin, byte* text_end, float wrap_width, byte cpu_fine_clip);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_RenderTextDelegate(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, byte* text_begin, byte* text_end, float wrap_width, byte cpu_fine_clip);
        private static readonly ImFont_RenderTextDelegate pImFont_RenderText = lib.LoadFunction<ImFont_RenderTextDelegate>("ImFont_RenderText");
        public static void ImFont_RenderText(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, Vector4 clip_rect, byte* text_begin, byte* text_end, float wrap_width, byte cpu_fine_clip) => pImFont_RenderText(self, draw_list, size, pos, col, clip_rect, text_begin, text_end, wrap_width, cpu_fine_clip);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igLogFinish();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogFinishDelegate();
        private static readonly igLogFinishDelegate pigLogFinish = lib.LoadFunction<igLogFinishDelegate>("igLogFinish");
        public static void igLogFinish() => pigLogFinish();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsKeyPressed(int user_key_index, byte repeat);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsKeyPressedDelegate(int user_key_index, byte repeat);
        private static readonly igIsKeyPressedDelegate pigIsKeyPressed = lib.LoadFunction<igIsKeyPressedDelegate>("igIsKeyPressed");
        public static byte igIsKeyPressed(int user_key_index, byte repeat) => pigIsKeyPressed(user_key_index, repeat);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetColumnOffset(int column_index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetColumnOffsetDelegate(int column_index);
        private static readonly igGetColumnOffsetDelegate pigGetColumnOffset = lib.LoadFunction<igGetColumnOffsetDelegate>("igGetColumnOffset");
        public static float igGetColumnOffset(int column_index) => pigGetColumnOffset(column_index);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PopClipRect(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PopClipRectDelegate(ImDrawList* self);
        private static readonly ImDrawList_PopClipRectDelegate pImDrawList_PopClipRect = lib.LoadFunction<ImDrawList_PopClipRectDelegate>("ImDrawList_PopClipRect");
        public static void ImDrawList_PopClipRect(ImDrawList* self) => pImDrawList_PopClipRect(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImFontGlyph* ImFont_FindGlyphNoFallback(ImFont* self, ushort c);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFontGlyph* ImFont_FindGlyphNoFallbackDelegate(ImFont* self, ushort c);
        private static readonly ImFont_FindGlyphNoFallbackDelegate pImFont_FindGlyphNoFallback = lib.LoadFunction<ImFont_FindGlyphNoFallbackDelegate>("ImFont_FindGlyphNoFallback");
        public static ImFontGlyph* ImFont_FindGlyphNoFallback(ImFont* self, ushort c) => pImFont_FindGlyphNoFallback(self, c);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetNextWindowCollapsed(byte collapsed, ImGuiCond cond);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowCollapsedDelegate(byte collapsed, ImGuiCond cond);
        private static readonly igSetNextWindowCollapsedDelegate pigSetNextWindowCollapsed = lib.LoadFunction<igSetNextWindowCollapsedDelegate>("igSetNextWindowCollapsed");
        public static void igSetNextWindowCollapsed(byte collapsed, ImGuiCond cond) => pigSetNextWindowCollapsed(collapsed, cond);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr igGetCurrentContext();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr igGetCurrentContextDelegate();
        private static readonly igGetCurrentContextDelegate pigGetCurrentContext = lib.LoadFunction<igGetCurrentContextDelegate>("igGetCurrentContext");
        public static IntPtr igGetCurrentContext() => pigGetCurrentContext();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSmallButton(byte* label);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSmallButtonDelegate(byte* label);
        private static readonly igSmallButtonDelegate pigSmallButton = lib.LoadFunction<igSmallButtonDelegate>("igSmallButton");
        public static byte igSmallButton(byte* label) => pigSmallButton(label);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igOpenPopupOnItemClick(byte* str_id, int mouse_button);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igOpenPopupOnItemClickDelegate(byte* str_id, int mouse_button);
        private static readonly igOpenPopupOnItemClickDelegate pigOpenPopupOnItemClick = lib.LoadFunction<igOpenPopupOnItemClickDelegate>("igOpenPopupOnItemClick");
        public static byte igOpenPopupOnItemClick(byte* str_id, int mouse_button) => pigOpenPopupOnItemClick(str_id, mouse_button);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsAnyMouseDown();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsAnyMouseDownDelegate();
        private static readonly igIsAnyMouseDownDelegate pigIsAnyMouseDown = lib.LoadFunction<igIsAnyMouseDownDelegate>("igIsAnyMouseDown");
        public static byte igIsAnyMouseDown() => pigIsAnyMouseDown();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* ImFont_CalcWordWrapPositionA(ImFont* self, float scale, byte* text, byte* text_end, float wrap_width);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImFont_CalcWordWrapPositionADelegate(ImFont* self, float scale, byte* text, byte* text_end, float wrap_width);
        private static readonly ImFont_CalcWordWrapPositionADelegate pImFont_CalcWordWrapPositionA = lib.LoadFunction<ImFont_CalcWordWrapPositionADelegate>("ImFont_CalcWordWrapPositionA");
        public static byte* ImFont_CalcWordWrapPositionA(ImFont* self, float scale, byte* text, byte* text_end, float wrap_width) => pImFont_CalcWordWrapPositionA(self, scale, text, text_end, wrap_width);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImFont_CalcTextSizeA_nonUDT2")]
        public static extern Vector2 ImFont_CalcTextSizeA(ImFont* self, float size, float max_width, float wrap_width, byte* text_begin, byte* text_end, byte** remaining);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 ImFont_CalcTextSizeADelegate(ImFont* self, float size, float max_width, float wrap_width, byte* text_begin, byte* text_end, byte** remaining);
        private static readonly ImFont_CalcTextSizeADelegate pImFont_CalcTextSizeA = lib.LoadFunction<ImFont_CalcTextSizeADelegate>("ImFont_CalcTextSizeA");
        public static Vector2 ImFont_CalcTextSizeA(ImFont* self, float size, float max_width, float wrap_width, byte* text_begin, byte* text_end, byte** remaining) => pImFont_CalcTextSizeA(self, size, max_width, wrap_width, text_begin, text_end, remaining);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GlyphRangesBuilder_SetBit(GlyphRangesBuilder* self, int n);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_SetBitDelegate(GlyphRangesBuilder* self, int n);
        private static readonly GlyphRangesBuilder_SetBitDelegate pGlyphRangesBuilder_SetBit = lib.LoadFunction<GlyphRangesBuilder_SetBitDelegate>("GlyphRangesBuilder_SetBit");
        public static void GlyphRangesBuilder_SetBit(GlyphRangesBuilder* self, int n) => pGlyphRangesBuilder_SetBit(self, n);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImFont_IsLoaded(ImFont* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImFont_IsLoadedDelegate(ImFont* self);
        private static readonly ImFont_IsLoadedDelegate pImFont_IsLoaded = lib.LoadFunction<ImFont_IsLoadedDelegate>("ImFont_IsLoaded");
        public static byte ImFont_IsLoaded(ImFont* self) => pImFont_IsLoaded(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float ImFont_GetCharAdvance(ImFont* self, ushort c);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float ImFont_GetCharAdvanceDelegate(ImFont* self, ushort c);
        private static readonly ImFont_GetCharAdvanceDelegate pImFont_GetCharAdvance = lib.LoadFunction<ImFont_GetCharAdvanceDelegate>("ImFont_GetCharAdvance");
        public static float ImFont_GetCharAdvance(ImFont* self, ushort c) => pImFont_GetCharAdvance(self, c);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igImageButton(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, int frame_padding, Vector4 bg_col, Vector4 tint_col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igImageButtonDelegate(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, int frame_padding, Vector4 bg_col, Vector4 tint_col);
        private static readonly igImageButtonDelegate pigImageButton = lib.LoadFunction<igImageButtonDelegate>("igImageButton");
        public static byte igImageButton(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, int frame_padding, Vector4 bg_col, Vector4 tint_col) => pigImageButton(user_texture_id, size, uv0, uv1, frame_padding, bg_col, tint_col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFont_SetFallbackChar(ImFont* self, ushort c);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_SetFallbackCharDelegate(ImFont* self, ushort c);
        private static readonly ImFont_SetFallbackCharDelegate pImFont_SetFallbackChar = lib.LoadFunction<ImFont_SetFallbackCharDelegate>("ImFont_SetFallbackChar");
        public static void ImFont_SetFallbackChar(ImFont* self, ushort c) => pImFont_SetFallbackChar(self, c);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndFrame();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndFrameDelegate();
        private static readonly igEndFrameDelegate pigEndFrame = lib.LoadFunction<igEndFrameDelegate>("igEndFrame");
        public static void igEndFrame() => pigEndFrame();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderFloat2(byte* label, Vector2* v, float v_min, float v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderFloat2Delegate(byte* label, Vector2* v, float v_min, float v_max, byte* format, float power);
        private static readonly igSliderFloat2Delegate pigSliderFloat2 = lib.LoadFunction<igSliderFloat2Delegate>("igSliderFloat2");
        public static byte igSliderFloat2(byte* label, Vector2* v, float v_min, float v_max, byte* format, float power) => pigSliderFloat2(label, v, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFont_RenderChar(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, ushort c);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_RenderCharDelegate(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, ushort c);
        private static readonly ImFont_RenderCharDelegate pImFont_RenderChar = lib.LoadFunction<ImFont_RenderCharDelegate>("ImFont_RenderChar");
        public static void ImFont_RenderChar(ImFont* self, ImDrawList* draw_list, float size, Vector2 pos, uint col, ushort c) => pImFont_RenderChar(self, draw_list, size, pos, col, c);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igRadioButtonBool(byte* label, byte active);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igRadioButtonBoolDelegate(byte* label, byte active);
        private static readonly igRadioButtonBoolDelegate pigRadioButtonBool = lib.LoadFunction<igRadioButtonBoolDelegate>("igRadioButtonBool");
        public static byte igRadioButtonBool(byte* label, byte active) => pigRadioButtonBool(label, active);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igRadioButtonIntPtr(byte* label, int* v, int v_button);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igRadioButtonIntPtrDelegate(byte* label, int* v, int v_button);
        private static readonly igRadioButtonIntPtrDelegate pigRadioButtonIntPtr = lib.LoadFunction<igRadioButtonIntPtrDelegate>("igRadioButtonIntPtr");
        public static byte igRadioButtonIntPtr(byte* label, int* v, int v_button) => pigRadioButtonIntPtr(label, v, v_button);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PushClipRect(ImDrawList* self, Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PushClipRectDelegate(ImDrawList* self, Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect);
        private static readonly ImDrawList_PushClipRectDelegate pImDrawList_PushClipRect = lib.LoadFunction<ImDrawList_PushClipRectDelegate>("ImDrawList_PushClipRect");
        public static void ImDrawList_PushClipRect(ImDrawList* self, Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect) => pImDrawList_PushClipRect(self, clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImFontGlyph* ImFont_FindGlyph(ImFont* self, ushort c);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFontGlyph* ImFont_FindGlyphDelegate(ImFont* self, ushort c);
        private static readonly ImFont_FindGlyphDelegate pImFont_FindGlyph = lib.LoadFunction<ImFont_FindGlyphDelegate>("ImFont_FindGlyph");
        public static ImFontGlyph* ImFont_FindGlyph(ImFont* self, ushort c) => pImFont_FindGlyph(self, c);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsItemDeactivatedAfterEdit();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemDeactivatedAfterEditDelegate();
        private static readonly igIsItemDeactivatedAfterEditDelegate pigIsItemDeactivatedAfterEdit = lib.LoadFunction<igIsItemDeactivatedAfterEditDelegate>("igIsItemDeactivatedAfterEdit");
        public static byte igIsItemDeactivatedAfterEdit() => pigIsItemDeactivatedAfterEdit();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImDrawList* igGetWindowDrawList();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImDrawList* igGetWindowDrawListDelegate();
        private static readonly igGetWindowDrawListDelegate pigGetWindowDrawList = lib.LoadFunction<igGetWindowDrawListDelegate>("igGetWindowDrawList");
        public static ImDrawList* igGetWindowDrawList() => pigGetWindowDrawList();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImFont* ImFontAtlas_AddFont(ImFontAtlas* self, ImFontConfig* font_cfg);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontDelegate(ImFontAtlas* self, ImFontConfig* font_cfg);
        private static readonly ImFontAtlas_AddFontDelegate pImFontAtlas_AddFont = lib.LoadFunction<ImFontAtlas_AddFontDelegate>("ImFontAtlas_AddFont");
        public static ImFont* ImFontAtlas_AddFont(ImFontAtlas* self, ImFontConfig* font_cfg) => pImFontAtlas_AddFont(self, font_cfg);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PathBezierCurveTo(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, int num_segments);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathBezierCurveToDelegate(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, int num_segments);
        private static readonly ImDrawList_PathBezierCurveToDelegate pImDrawList_PathBezierCurveTo = lib.LoadFunction<ImDrawList_PathBezierCurveToDelegate>("ImDrawList_PathBezierCurveTo");
        public static void ImDrawList_PathBezierCurveTo(ImDrawList* self, Vector2 p1, Vector2 p2, Vector2 p3, int num_segments) => pImDrawList_PathBezierCurveTo(self, p1, p2, p3, num_segments);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiPayload_Clear(ImGuiPayload* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiPayload_ClearDelegate(ImGuiPayload* self);
        private static readonly ImGuiPayload_ClearDelegate pImGuiPayload_Clear = lib.LoadFunction<ImGuiPayload_ClearDelegate>("ImGuiPayload_Clear");
        public static void ImGuiPayload_Clear(ImGuiPayload* self) => pImGuiPayload_Clear(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igNewLine();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igNewLineDelegate();
        private static readonly igNewLineDelegate pigNewLine = lib.LoadFunction<igNewLineDelegate>("igNewLine");
        public static void igNewLine() => pigNewLine();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsItemFocused();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemFocusedDelegate();
        private static readonly igIsItemFocusedDelegate pigIsItemFocused = lib.LoadFunction<igIsItemFocusedDelegate>("igIsItemFocused");
        public static byte igIsItemFocused() => pigIsItemFocused();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igLoadIniSettingsFromMemory(byte* ini_data, uint ini_size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLoadIniSettingsFromMemoryDelegate(byte* ini_data, uint ini_size);
        private static readonly igLoadIniSettingsFromMemoryDelegate pigLoadIniSettingsFromMemory = lib.LoadFunction<igLoadIniSettingsFromMemoryDelegate>("igLoadIniSettingsFromMemory");
        public static void igLoadIniSettingsFromMemory(byte* ini_data, uint ini_size) => pigLoadIniSettingsFromMemory(ini_data, ini_size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderInt2(byte* label, int* v, int v_min, int v_max, byte* format);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderInt2Delegate(byte* label, int* v, int v_min, int v_max, byte* format);
        private static readonly igSliderInt2Delegate pigSliderInt2 = lib.LoadFunction<igSliderInt2Delegate>("igSliderInt2");
        public static byte igSliderInt2(byte* label, int* v, int v_min, int v_max, byte* format) => pigSliderInt2(label, v, v_min, v_max, format);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetWindowSizeVec2(Vector2 size, ImGuiCond cond);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowSizeVec2Delegate(Vector2 size, ImGuiCond cond);
        private static readonly igSetWindowSizeVec2Delegate pigSetWindowSizeVec2 = lib.LoadFunction<igSetWindowSizeVec2Delegate>("igSetWindowSizeVec2");
        public static void igSetWindowSizeVec2(Vector2 size, ImGuiCond cond) => pigSetWindowSizeVec2(size, cond);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetWindowSizeStr(byte* name, Vector2 size, ImGuiCond cond);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowSizeStrDelegate(byte* name, Vector2 size, ImGuiCond cond);
        private static readonly igSetWindowSizeStrDelegate pigSetWindowSizeStr = lib.LoadFunction<igSetWindowSizeStrDelegate>("igSetWindowSizeStr");
        public static void igSetWindowSizeStr(byte* name, Vector2 size, ImGuiCond cond) => pigSetWindowSizeStr(name, size, cond);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputFloat(byte* label, float* v, float step, float step_fast, byte* format, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputFloatDelegate(byte* label, float* v, float step, float step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        private static readonly igInputFloatDelegate pigInputFloat = lib.LoadFunction<igInputFloatDelegate>("igInputFloat");
        public static byte igInputFloat(byte* label, float* v, float step, float step_fast, byte* format, ImGuiInputTextFlags extra_flags) => pigInputFloat(label, v, step, step_fast, format, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFont_ImFont(ImFont* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_ImFontDelegate(ImFont* self);
        private static readonly ImFont_ImFontDelegate pImFont_ImFont = lib.LoadFunction<ImFont_ImFontDelegate>("ImFont_ImFont");
        public static void ImFont_ImFont(ImFont* self) => pImFont_ImFont(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiStorage_SetFloat(ImGuiStorage* self, uint key, float val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_SetFloatDelegate(ImGuiStorage* self, uint key, float val);
        private static readonly ImGuiStorage_SetFloatDelegate pImGuiStorage_SetFloat = lib.LoadFunction<ImGuiStorage_SetFloatDelegate>("ImGuiStorage_SetFloat");
        public static void ImGuiStorage_SetFloat(ImGuiStorage* self, uint key, float val) => pImGuiStorage_SetFloat(self, key, val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igColorConvertRGBtoHSV(float r, float g, float b, float* out_h, float* out_s, float* out_v);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igColorConvertRGBtoHSVDelegate(float r, float g, float b, float* out_h, float* out_s, float* out_v);
        private static readonly igColorConvertRGBtoHSVDelegate pigColorConvertRGBtoHSV = lib.LoadFunction<igColorConvertRGBtoHSVDelegate>("igColorConvertRGBtoHSV");
        public static void igColorConvertRGBtoHSV(float r, float g, float b, float* out_h, float* out_s, float* out_v) => pigColorConvertRGBtoHSV(r, g, b, out_h, out_s, out_v);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginMenuBar();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginMenuBarDelegate();
        private static readonly igBeginMenuBarDelegate pigBeginMenuBar = lib.LoadFunction<igBeginMenuBarDelegate>("igBeginMenuBar");
        public static byte igBeginMenuBar() => pigBeginMenuBar();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsPopupOpen(byte* str_id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsPopupOpenDelegate(byte* str_id);
        private static readonly igIsPopupOpenDelegate pigIsPopupOpen = lib.LoadFunction<igIsPopupOpenDelegate>("igIsPopupOpen");
        public static byte igIsPopupOpen(byte* str_id) => pigIsPopupOpen(str_id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsItemVisible();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemVisibleDelegate();
        private static readonly igIsItemVisibleDelegate pigIsItemVisible = lib.LoadFunction<igIsItemVisibleDelegate>("igIsItemVisible");
        public static byte igIsItemVisible() => pigIsItemVisible();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFontAtlas_CalcCustomRectUV(ImFontAtlas* self, CustomRect* rect, Vector2* out_uv_min, Vector2* out_uv_max);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_CalcCustomRectUVDelegate(ImFontAtlas* self, CustomRect* rect, Vector2* out_uv_min, Vector2* out_uv_max);
        private static readonly ImFontAtlas_CalcCustomRectUVDelegate pImFontAtlas_CalcCustomRectUV = lib.LoadFunction<ImFontAtlas_CalcCustomRectUVDelegate>("ImFontAtlas_CalcCustomRectUV");
        public static void ImFontAtlas_CalcCustomRectUV(ImFontAtlas* self, CustomRect* rect, Vector2* out_uv_min, Vector2* out_uv_max) => pImFontAtlas_CalcCustomRectUV(self, rect, out_uv_min, out_uv_max);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern CustomRect* ImFontAtlas_GetCustomRectByIndex(ImFontAtlas* self, int index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate CustomRect* ImFontAtlas_GetCustomRectByIndexDelegate(ImFontAtlas* self, int index);
        private static readonly ImFontAtlas_GetCustomRectByIndexDelegate pImFontAtlas_GetCustomRectByIndex = lib.LoadFunction<ImFontAtlas_GetCustomRectByIndexDelegate>("ImFontAtlas_GetCustomRectByIndex");
        public static CustomRect* ImFontAtlas_GetCustomRectByIndex(ImFontAtlas* self, int index) => pImFontAtlas_GetCustomRectByIndex(self, index);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GlyphRangesBuilder_AddText(GlyphRangesBuilder* self, byte* text, byte* text_end);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_AddTextDelegate(GlyphRangesBuilder* self, byte* text, byte* text_end);
        private static readonly GlyphRangesBuilder_AddTextDelegate pGlyphRangesBuilder_AddText = lib.LoadFunction<GlyphRangesBuilder_AddTextDelegate>("GlyphRangesBuilder_AddText");
        public static void GlyphRangesBuilder_AddText(GlyphRangesBuilder* self, byte* text, byte* text_end) => pGlyphRangesBuilder_AddText(self, text, text_end);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_UpdateTextureID(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_UpdateTextureIDDelegate(ImDrawList* self);
        private static readonly ImDrawList_UpdateTextureIDDelegate pImDrawList_UpdateTextureID = lib.LoadFunction<ImDrawList_UpdateTextureIDDelegate>("ImDrawList_UpdateTextureID");
        public static void ImDrawList_UpdateTextureID(ImDrawList* self) => pImDrawList_UpdateTextureID(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetNextWindowSize(Vector2 size, ImGuiCond cond);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowSizeDelegate(Vector2 size, ImGuiCond cond);
        private static readonly igSetNextWindowSizeDelegate pigSetNextWindowSize = lib.LoadFunction<igSetNextWindowSizeDelegate>("igSetNextWindowSize");
        public static void igSetNextWindowSize(Vector2 size, ImGuiCond cond) => pigSetNextWindowSize(size, cond);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ImFontAtlas_AddCustomRectRegular(ImFontAtlas* self, uint id, int width, int height);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ImFontAtlas_AddCustomRectRegularDelegate(ImFontAtlas* self, uint id, int width, int height);
        private static readonly ImFontAtlas_AddCustomRectRegularDelegate pImFontAtlas_AddCustomRectRegular = lib.LoadFunction<ImFontAtlas_AddCustomRectRegularDelegate>("ImFontAtlas_AddCustomRectRegular");
        public static int ImFontAtlas_AddCustomRectRegular(ImFontAtlas* self, uint id, int width, int height) => pImFontAtlas_AddCustomRectRegular(self, id, width, height);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetWindowCollapsedBool(byte collapsed, ImGuiCond cond);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowCollapsedBoolDelegate(byte collapsed, ImGuiCond cond);
        private static readonly igSetWindowCollapsedBoolDelegate pigSetWindowCollapsedBool = lib.LoadFunction<igSetWindowCollapsedBoolDelegate>("igSetWindowCollapsedBool");
        public static void igSetWindowCollapsedBool(byte collapsed, ImGuiCond cond) => pigSetWindowCollapsedBool(collapsed, cond);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetWindowCollapsedStr(byte* name, byte collapsed, ImGuiCond cond);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowCollapsedStrDelegate(byte* name, byte collapsed, ImGuiCond cond);
        private static readonly igSetWindowCollapsedStrDelegate pigSetWindowCollapsedStr = lib.LoadFunction<igSetWindowCollapsedStrDelegate>("igSetWindowCollapsedStr");
        public static void igSetWindowCollapsedStr(byte* name, byte collapsed, ImGuiCond cond) => pigSetWindowCollapsedStr(name, collapsed, cond);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetMouseDragDelta_nonUDT2")]
        public static extern Vector2 igGetMouseDragDelta(int button, float lock_threshold);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetMouseDragDeltaDelegate(int button, float lock_threshold);
        private static readonly igGetMouseDragDeltaDelegate pigGetMouseDragDelta = lib.LoadFunction<igGetMouseDragDeltaDelegate>("igGetMouseDragDelta");
        public static Vector2 igGetMouseDragDelta(int button, float lock_threshold) => pigGetMouseDragDelta(button, lock_threshold);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiPayload* igAcceptDragDropPayload(byte* type, ImGuiDragDropFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImGuiPayload* igAcceptDragDropPayloadDelegate(byte* type, ImGuiDragDropFlags flags);
        private static readonly igAcceptDragDropPayloadDelegate pigAcceptDragDropPayload = lib.LoadFunction<igAcceptDragDropPayloadDelegate>("igAcceptDragDropPayload");
        public static ImGuiPayload* igAcceptDragDropPayload(byte* type, ImGuiDragDropFlags flags) => pigAcceptDragDropPayload(type, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginDragDropSource(ImGuiDragDropFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginDragDropSourceDelegate(ImGuiDragDropFlags flags);
        private static readonly igBeginDragDropSourceDelegate pigBeginDragDropSource = lib.LoadFunction<igBeginDragDropSourceDelegate>("igBeginDragDropSource");
        public static byte igBeginDragDropSource(ImGuiDragDropFlags flags) => pigBeginDragDropSource(flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte CustomRect_IsPacked(CustomRect* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte CustomRect_IsPackedDelegate(CustomRect* self);
        private static readonly CustomRect_IsPackedDelegate pCustomRect_IsPacked = lib.LoadFunction<CustomRect_IsPackedDelegate>("CustomRect_IsPacked");
        public static byte CustomRect_IsPacked(CustomRect* self) => pCustomRect_IsPacked(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPlotLines(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPlotLinesDelegate(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
        private static readonly igPlotLinesDelegate pigPlotLines = lib.LoadFunction<igPlotLinesDelegate>("igPlotLines");
        public static void igPlotLines(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride) => pigPlotLines(label, values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImFontAtlas_IsBuilt(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImFontAtlas_IsBuiltDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_IsBuiltDelegate pImFontAtlas_IsBuilt = lib.LoadFunction<ImFontAtlas_IsBuiltDelegate>("ImFontAtlas_IsBuilt");
        public static byte ImFontAtlas_IsBuilt(ImFontAtlas* self) => pImFontAtlas_IsBuilt(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImVec2_ImVec2(Vector2* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImVec2_ImVec2Delegate(Vector2* self);
        private static readonly ImVec2_ImVec2Delegate pImVec2_ImVec2 = lib.LoadFunction<ImVec2_ImVec2Delegate>("ImVec2_ImVec2");
        public static void ImVec2_ImVec2(Vector2* self) => pImVec2_ImVec2(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImVec2_ImVec2Float(Vector2* self, float _x, float _y);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImVec2_ImVec2FloatDelegate(Vector2* self, float _x, float _y);
        private static readonly ImVec2_ImVec2FloatDelegate pImVec2_ImVec2Float = lib.LoadFunction<ImVec2_ImVec2FloatDelegate>("ImVec2_ImVec2Float");
        public static void ImVec2_ImVec2Float(Vector2* self, float _x, float _y) => pImVec2_ImVec2Float(self, _x, _y);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiPayload_ImGuiPayload(ImGuiPayload* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiPayload_ImGuiPayloadDelegate(ImGuiPayload* self);
        private static readonly ImGuiPayload_ImGuiPayloadDelegate pImGuiPayload_ImGuiPayload = lib.LoadFunction<ImGuiPayload_ImGuiPayloadDelegate>("ImGuiPayload_ImGuiPayload");
        public static void ImGuiPayload_ImGuiPayload(ImGuiPayload* self) => pImGuiPayload_ImGuiPayload(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_Clear(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ClearDelegate(ImDrawList* self);
        private static readonly ImDrawList_ClearDelegate pImDrawList_Clear = lib.LoadFunction<ImDrawList_ClearDelegate>("ImDrawList_Clear");
        public static void ImDrawList_Clear(ImDrawList* self) => pImDrawList_Clear(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GlyphRangesBuilder_AddRanges(GlyphRangesBuilder* self, ushort* ranges);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_AddRangesDelegate(GlyphRangesBuilder* self, ushort* ranges);
        private static readonly GlyphRangesBuilder_AddRangesDelegate pGlyphRangesBuilder_AddRanges = lib.LoadFunction<GlyphRangesBuilder_AddRangesDelegate>("GlyphRangesBuilder_AddRanges");
        public static void GlyphRangesBuilder_AddRanges(GlyphRangesBuilder* self, ushort* ranges) => pGlyphRangesBuilder_AddRanges(self, ranges);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern int igGetFrameCount();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int igGetFrameCountDelegate();
        private static readonly igGetFrameCountDelegate pigGetFrameCount = lib.LoadFunction<igGetFrameCountDelegate>("igGetFrameCount");
        public static int igGetFrameCount() => pigGetFrameCount();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* ImFont_GetDebugName(ImFont* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImFont_GetDebugNameDelegate(ImFont* self);
        private static readonly ImFont_GetDebugNameDelegate pImFont_GetDebugName = lib.LoadFunction<ImFont_GetDebugNameDelegate>("ImFont_GetDebugName");
        public static byte* ImFont_GetDebugName(ImFont* self) => pImFont_GetDebugName(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igListBoxFooter();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igListBoxFooterDelegate();
        private static readonly igListBoxFooterDelegate pigListBoxFooter = lib.LoadFunction<igListBoxFooterDelegate>("igListBoxFooter");
        public static void igListBoxFooter() => pigListBoxFooter();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPopClipRect();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopClipRectDelegate();
        private static readonly igPopClipRectDelegate pigPopClipRect = lib.LoadFunction<igPopClipRectDelegate>("igPopClipRect");
        public static void igPopClipRect() => pigPopClipRect();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddBezierCurve(ImDrawList* self, Vector2 pos0, Vector2 cp0, Vector2 cp1, Vector2 pos1, uint col, float thickness, int num_segments);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddBezierCurveDelegate(ImDrawList* self, Vector2 pos0, Vector2 cp0, Vector2 cp1, Vector2 pos1, uint col, float thickness, int num_segments);
        private static readonly ImDrawList_AddBezierCurveDelegate pImDrawList_AddBezierCurve = lib.LoadFunction<ImDrawList_AddBezierCurveDelegate>("ImDrawList_AddBezierCurve");
        public static void ImDrawList_AddBezierCurve(ImDrawList* self, Vector2 pos0, Vector2 cp0, Vector2 cp1, Vector2 pos1, uint col, float thickness, int num_segments) => pImDrawList_AddBezierCurve(self, pos0, cp0, cp1, pos1, col, thickness, num_segments);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GlyphRangesBuilder_GlyphRangesBuilder(GlyphRangesBuilder* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_GlyphRangesBuilderDelegate(GlyphRangesBuilder* self);
        private static readonly GlyphRangesBuilder_GlyphRangesBuilderDelegate pGlyphRangesBuilder_GlyphRangesBuilder = lib.LoadFunction<GlyphRangesBuilder_GlyphRangesBuilderDelegate>("GlyphRangesBuilder_GlyphRangesBuilder");
        public static void GlyphRangesBuilder_GlyphRangesBuilder(GlyphRangesBuilder* self) => pGlyphRangesBuilder_GlyphRangesBuilder(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetWindowSize_nonUDT2")]
        public static extern Vector2 igGetWindowSize();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetWindowSizeDelegate();
        private static readonly igGetWindowSizeDelegate pigGetWindowSize = lib.LoadFunction<igGetWindowSizeDelegate>("igGetWindowSize");
        public static Vector2 igGetWindowSize() => pigGetWindowSize();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort* ImFontAtlas_GetGlyphRangesThai(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesThaiDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_GetGlyphRangesThaiDelegate pImFontAtlas_GetGlyphRangesThai = lib.LoadFunction<ImFontAtlas_GetGlyphRangesThaiDelegate>("ImFontAtlas_GetGlyphRangesThai");
        public static ushort* ImFontAtlas_GetGlyphRangesThai(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesThai(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igCheckboxFlags(byte* label, uint* flags, uint flags_value);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igCheckboxFlagsDelegate(byte* label, uint* flags, uint flags_value);
        private static readonly igCheckboxFlagsDelegate pigCheckboxFlags = lib.LoadFunction<igCheckboxFlagsDelegate>("igCheckboxFlags");
        public static byte igCheckboxFlags(byte* label, uint* flags, uint flags_value) => pigCheckboxFlags(label, flags, flags_value);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort* ImFontAtlas_GetGlyphRangesCyrillic(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesCyrillicDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_GetGlyphRangesCyrillicDelegate pImFontAtlas_GetGlyphRangesCyrillic = lib.LoadFunction<ImFontAtlas_GetGlyphRangesCyrillicDelegate>("ImFontAtlas_GetGlyphRangesCyrillic");
        public static ushort* ImFontAtlas_GetGlyphRangesCyrillic(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesCyrillic(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsWindowHovered(ImGuiHoveredFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsWindowHoveredDelegate(ImGuiHoveredFlags flags);
        private static readonly igIsWindowHoveredDelegate pigIsWindowHovered = lib.LoadFunction<igIsWindowHoveredDelegate>("igIsWindowHovered");
        public static byte igIsWindowHovered(ImGuiHoveredFlags flags) => pigIsWindowHovered(flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommonDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_GetGlyphRangesChineseSimplifiedCommonDelegate pImFontAtlas_GetGlyphRangesChineseSimplifiedCommon = lib.LoadFunction<ImFontAtlas_GetGlyphRangesChineseSimplifiedCommonDelegate>("ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon");
        public static ushort* ImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesChineseSimplifiedCommon(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPlotHistogramFloatPtr(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPlotHistogramFloatPtrDelegate(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride);
        private static readonly igPlotHistogramFloatPtrDelegate pigPlotHistogramFloatPtr = lib.LoadFunction<igPlotHistogramFloatPtrDelegate>("igPlotHistogramFloatPtr");
        public static void igPlotHistogramFloatPtr(byte* label, float* values, int values_count, int values_offset, byte* overlay_text, float scale_min, float scale_max, Vector2 graph_size, int stride) => pigPlotHistogramFloatPtr(label, values, values_count, values_offset, overlay_text, scale_min, scale_max, graph_size, stride);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginPopupContextVoid(byte* str_id, int mouse_button);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginPopupContextVoidDelegate(byte* str_id, int mouse_button);
        private static readonly igBeginPopupContextVoidDelegate pigBeginPopupContextVoid = lib.LoadFunction<igBeginPopupContextVoidDelegate>("igBeginPopupContextVoid");
        public static byte igBeginPopupContextVoid(byte* str_id, int mouse_button) => pigBeginPopupContextVoid(str_id, mouse_button);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort* ImFontAtlas_GetGlyphRangesChineseFull(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesChineseFullDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_GetGlyphRangesChineseFullDelegate pImFontAtlas_GetGlyphRangesChineseFull = lib.LoadFunction<ImFontAtlas_GetGlyphRangesChineseFullDelegate>("ImFontAtlas_GetGlyphRangesChineseFull");
        public static ushort* ImFontAtlas_GetGlyphRangesChineseFull(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesChineseFull(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igShowStyleEditor(ImGuiStyle* @ref);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igShowStyleEditorDelegate(ImGuiStyle* @ref);
        private static readonly igShowStyleEditorDelegate pigShowStyleEditor = lib.LoadFunction<igShowStyleEditorDelegate>("igShowStyleEditor");
        public static void igShowStyleEditor(ImGuiStyle* @ref) => pigShowStyleEditor(@ref);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igCheckbox(byte* label, byte* v);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igCheckboxDelegate(byte* label, byte* v);
        private static readonly igCheckboxDelegate pigCheckbox = lib.LoadFunction<igCheckboxDelegate>("igCheckbox");
        public static byte igCheckbox(byte* label, byte* v) => pigCheckbox(label, v);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetWindowPos_nonUDT2")]
        public static extern Vector2 igGetWindowPos();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetWindowPosDelegate();
        private static readonly igGetWindowPosDelegate pigGetWindowPos = lib.LoadFunction<igGetWindowPosDelegate>("igGetWindowPos");
        public static Vector2 igGetWindowPos() => pigGetWindowPos();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiInputTextCallbackData_ImGuiInputTextCallbackData(ImGuiInputTextCallbackData* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiInputTextCallbackData_ImGuiInputTextCallbackDataDelegate(ImGuiInputTextCallbackData* self);
        private static readonly ImGuiInputTextCallbackData_ImGuiInputTextCallbackDataDelegate pImGuiInputTextCallbackData_ImGuiInputTextCallbackData = lib.LoadFunction<ImGuiInputTextCallbackData_ImGuiInputTextCallbackDataDelegate>("ImGuiInputTextCallbackData_ImGuiInputTextCallbackData");
        public static void ImGuiInputTextCallbackData_ImGuiInputTextCallbackData(ImGuiInputTextCallbackData* self) => pImGuiInputTextCallbackData_ImGuiInputTextCallbackData(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetNextWindowContentSize(Vector2 size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowContentSizeDelegate(Vector2 size);
        private static readonly igSetNextWindowContentSizeDelegate pigSetNextWindowContentSize = lib.LoadFunction<igSetNextWindowContentSizeDelegate>("igSetNextWindowContentSize");
        public static void igSetNextWindowContentSize(Vector2 size) => pigSetNextWindowContentSize(size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igTextColored(Vector4 col, byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTextColoredDelegate(Vector4 col, byte* fmt);
        private static readonly igTextColoredDelegate pigTextColored = lib.LoadFunction<igTextColoredDelegate>("igTextColored");
        public static void igTextColored(Vector4 col, byte* fmt) => pigTextColored(col, fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igLogToFile(int max_depth, byte* filename);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogToFileDelegate(int max_depth, byte* filename);
        private static readonly igLogToFileDelegate pigLogToFile = lib.LoadFunction<igLogToFileDelegate>("igLogToFile");
        public static void igLogToFile(int max_depth, byte* filename) => pigLogToFile(max_depth, filename);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igButton(byte* label, Vector2 size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igButtonDelegate(byte* label, Vector2 size);
        private static readonly igButtonDelegate pigButton = lib.LoadFunction<igButtonDelegate>("igButton");
        public static byte igButton(byte* label, Vector2 size) => pigButton(label, size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsItemEdited();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemEditedDelegate();
        private static readonly igIsItemEditedDelegate pigIsItemEdited = lib.LoadFunction<igIsItemEditedDelegate>("igIsItemEdited");
        public static byte igIsItemEdited() => pigIsItemEdited();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PushTextureID(ImDrawList* self, IntPtr texture_id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PushTextureIDDelegate(ImDrawList* self, IntPtr texture_id);
        private static readonly ImDrawList_PushTextureIDDelegate pImDrawList_PushTextureID = lib.LoadFunction<ImDrawList_PushTextureIDDelegate>("ImDrawList_PushTextureID");
        public static void ImDrawList_PushTextureID(ImDrawList* self, IntPtr texture_id) => pImDrawList_PushTextureID(self, texture_id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igTreeAdvanceToLabelPos();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTreeAdvanceToLabelPosDelegate();
        private static readonly igTreeAdvanceToLabelPosDelegate pigTreeAdvanceToLabelPos = lib.LoadFunction<igTreeAdvanceToLabelPosDelegate>("igTreeAdvanceToLabelPos");
        public static void igTreeAdvanceToLabelPos() => pigTreeAdvanceToLabelPos();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiInputTextCallbackData_DeleteChars(ImGuiInputTextCallbackData* self, int pos, int bytes_count);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiInputTextCallbackData_DeleteCharsDelegate(ImGuiInputTextCallbackData* self, int pos, int bytes_count);
        private static readonly ImGuiInputTextCallbackData_DeleteCharsDelegate pImGuiInputTextCallbackData_DeleteChars = lib.LoadFunction<ImGuiInputTextCallbackData_DeleteCharsDelegate>("ImGuiInputTextCallbackData_DeleteChars");
        public static void ImGuiInputTextCallbackData_DeleteChars(ImGuiInputTextCallbackData* self, int pos, int bytes_count) => pImGuiInputTextCallbackData_DeleteChars(self, pos, bytes_count);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragInt2(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragInt2Delegate(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        private static readonly igDragInt2Delegate pigDragInt2 = lib.LoadFunction<igDragInt2Delegate>("igDragInt2");
        public static byte igDragInt2(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => pigDragInt2(label, v, v_speed, v_min, v_max, format);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort* ImFontAtlas_GetGlyphRangesDefault(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesDefaultDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_GetGlyphRangesDefaultDelegate pImFontAtlas_GetGlyphRangesDefault = lib.LoadFunction<ImFontAtlas_GetGlyphRangesDefaultDelegate>("ImFontAtlas_GetGlyphRangesDefault");
        public static ushort* ImFontAtlas_GetGlyphRangesDefault(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesDefault(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsAnyItemActive();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsAnyItemActiveDelegate();
        private static readonly igIsAnyItemActiveDelegate pigIsAnyItemActive = lib.LoadFunction<igIsAnyItemActiveDelegate>("igIsAnyItemActive");
        public static byte igIsAnyItemActive() => pigIsAnyItemActive();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFontAtlas_SetTexID(ImFontAtlas* self, IntPtr id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_SetTexIDDelegate(ImFontAtlas* self, IntPtr id);
        private static readonly ImFontAtlas_SetTexIDDelegate pImFontAtlas_SetTexID = lib.LoadFunction<ImFontAtlas_SetTexIDDelegate>("ImFontAtlas_SetTexID");
        public static void ImFontAtlas_SetTexID(ImFontAtlas* self, IntPtr id) => pImFontAtlas_SetTexID(self, id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igMenuItemBool(byte* label, byte* shortcut, byte selected, byte enabled);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igMenuItemBoolDelegate(byte* label, byte* shortcut, byte selected, byte enabled);
        private static readonly igMenuItemBoolDelegate pigMenuItemBool = lib.LoadFunction<igMenuItemBoolDelegate>("igMenuItemBool");
        public static byte igMenuItemBool(byte* label, byte* shortcut, byte selected, byte enabled) => pigMenuItemBool(label, shortcut, selected, enabled);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igMenuItemBoolPtr(byte* label, byte* shortcut, byte* p_selected, byte enabled);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igMenuItemBoolPtrDelegate(byte* label, byte* shortcut, byte* p_selected, byte enabled);
        private static readonly igMenuItemBoolPtrDelegate pigMenuItemBoolPtr = lib.LoadFunction<igMenuItemBoolPtrDelegate>("igMenuItemBoolPtr");
        public static byte igMenuItemBoolPtr(byte* label, byte* shortcut, byte* p_selected, byte enabled) => pigMenuItemBoolPtr(label, shortcut, p_selected, enabled);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderFloat4(byte* label, Vector4* v, float v_min, float v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderFloat4Delegate(byte* label, Vector4* v, float v_min, float v_max, byte* format, float power);
        private static readonly igSliderFloat4Delegate pigSliderFloat4 = lib.LoadFunction<igSliderFloat4Delegate>("igSliderFloat4");
        public static byte igSliderFloat4(byte* label, Vector4* v, float v_min, float v_max, byte* format, float power) => pigSliderFloat4(label, v, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetCursorPosX();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetCursorPosXDelegate();
        private static readonly igGetCursorPosXDelegate pigGetCursorPosX = lib.LoadFunction<igGetCursorPosXDelegate>("igGetCursorPosX");
        public static float igGetCursorPosX() => pigGetCursorPosX();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFontAtlas_ClearTexData(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_ClearTexDataDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_ClearTexDataDelegate pImFontAtlas_ClearTexData = lib.LoadFunction<ImFontAtlas_ClearTexDataDelegate>("ImFontAtlas_ClearTexData");
        public static void ImFontAtlas_ClearTexData(ImFontAtlas* self) => pImFontAtlas_ClearTexData(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFontAtlas_ClearFonts(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_ClearFontsDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_ClearFontsDelegate pImFontAtlas_ClearFonts = lib.LoadFunction<ImFontAtlas_ClearFontsDelegate>("ImFontAtlas_ClearFonts");
        public static void ImFontAtlas_ClearFonts(ImFontAtlas* self) => pImFontAtlas_ClearFonts(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern int igGetColumnsCount();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int igGetColumnsCountDelegate();
        private static readonly igGetColumnsCountDelegate pigGetColumnsCount = lib.LoadFunction<igGetColumnsCountDelegate>("igGetColumnsCount");
        public static int igGetColumnsCount() => pigGetColumnsCount();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPopButtonRepeat();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopButtonRepeatDelegate();
        private static readonly igPopButtonRepeatDelegate pigPopButtonRepeat = lib.LoadFunction<igPopButtonRepeatDelegate>("igPopButtonRepeat");
        public static void igPopButtonRepeat() => pigPopButtonRepeat();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragScalarN(byte* label, ImGuiDataType data_type, void* v, int components, float v_speed, void* v_min, void* v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragScalarNDelegate(byte* label, ImGuiDataType data_type, void* v, int components, float v_speed, void* v_min, void* v_max, byte* format, float power);
        private static readonly igDragScalarNDelegate pigDragScalarN = lib.LoadFunction<igDragScalarNDelegate>("igDragScalarN");
        public static byte igDragScalarN(byte* label, ImGuiDataType data_type, void* v, int components, float v_speed, void* v_min, void* v_max, byte* format, float power) => pigDragScalarN(label, data_type, v, components, v_speed, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImGuiPayload_IsPreview(ImGuiPayload* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiPayload_IsPreviewDelegate(ImGuiPayload* self);
        private static readonly ImGuiPayload_IsPreviewDelegate pImGuiPayload_IsPreview = lib.LoadFunction<ImGuiPayload_IsPreviewDelegate>("ImGuiPayload_IsPreview");
        public static byte ImGuiPayload_IsPreview(ImGuiPayload* self) => pImGuiPayload_IsPreview(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSpacing();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSpacingDelegate();
        private static readonly igSpacingDelegate pigSpacing = lib.LoadFunction<igSpacingDelegate>("igSpacing");
        public static void igSpacing() => pigSpacing();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFontAtlas_Clear(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_ClearDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_ClearDelegate pImFontAtlas_Clear = lib.LoadFunction<ImFontAtlas_ClearDelegate>("ImFontAtlas_Clear");
        public static void ImFontAtlas_Clear(ImFontAtlas* self) => pImFontAtlas_Clear(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsAnyItemFocused();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsAnyItemFocusedDelegate();
        private static readonly igIsAnyItemFocusedDelegate pigIsAnyItemFocused = lib.LoadFunction<igIsAnyItemFocusedDelegate>("igIsAnyItemFocused");
        public static byte igIsAnyItemFocused() => pigIsAnyItemFocused();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddRectFilled(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddRectFilledDelegate(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags);
        private static readonly ImDrawList_AddRectFilledDelegate pImDrawList_AddRectFilled = lib.LoadFunction<ImDrawList_AddRectFilledDelegate>("ImDrawList_AddRectFilled");
        public static void ImDrawList_AddRectFilled(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags) => pImDrawList_AddRectFilled(self, a, b, col, rounding, rounding_corners_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImFont* ImFontAtlas_AddFontFromMemoryCompressedTTF(ImFontAtlas* self, void* compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontFromMemoryCompressedTTFDelegate(ImFontAtlas* self, void* compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        private static readonly ImFontAtlas_AddFontFromMemoryCompressedTTFDelegate pImFontAtlas_AddFontFromMemoryCompressedTTF = lib.LoadFunction<ImFontAtlas_AddFontFromMemoryCompressedTTFDelegate>("ImFontAtlas_AddFontFromMemoryCompressedTTF");
        public static ImFont* ImFontAtlas_AddFontFromMemoryCompressedTTF(ImFontAtlas* self, void* compressed_font_data, int compressed_font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => pImFontAtlas_AddFontFromMemoryCompressedTTF(self, compressed_font_data, compressed_font_size, size_pixels, font_cfg, glyph_ranges);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igMemFree(void* ptr);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igMemFreeDelegate(void* ptr);
        private static readonly igMemFreeDelegate pigMemFree = lib.LoadFunction<igMemFreeDelegate>("igMemFree");
        public static void igMemFree(void* ptr) => pigMemFree(ptr);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetFontTexUvWhitePixel_nonUDT2")]
        public static extern Vector2 igGetFontTexUvWhitePixel();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetFontTexUvWhitePixelDelegate();
        private static readonly igGetFontTexUvWhitePixelDelegate pigGetFontTexUvWhitePixel = lib.LoadFunction<igGetFontTexUvWhitePixelDelegate>("igGetFontTexUvWhitePixel");
        public static Vector2 igGetFontTexUvWhitePixel() => pigGetFontTexUvWhitePixel();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddDrawCmd(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddDrawCmdDelegate(ImDrawList* self);
        private static readonly ImDrawList_AddDrawCmdDelegate pImDrawList_AddDrawCmd = lib.LoadFunction<ImDrawList_AddDrawCmdDelegate>("ImDrawList_AddDrawCmd");
        public static void ImDrawList_AddDrawCmd(ImDrawList* self) => pImDrawList_AddDrawCmd(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsItemClicked(int mouse_button);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemClickedDelegate(int mouse_button);
        private static readonly igIsItemClickedDelegate pigIsItemClicked = lib.LoadFunction<igIsItemClickedDelegate>("igIsItemClicked");
        public static byte igIsItemClicked(int mouse_button) => pigIsItemClicked(mouse_button);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImFont* ImFontAtlas_AddFontFromMemoryTTF(ImFontAtlas* self, void* font_data, int font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontFromMemoryTTFDelegate(ImFontAtlas* self, void* font_data, int font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        private static readonly ImFontAtlas_AddFontFromMemoryTTFDelegate pImFontAtlas_AddFontFromMemoryTTF = lib.LoadFunction<ImFontAtlas_AddFontFromMemoryTTFDelegate>("ImFontAtlas_AddFontFromMemoryTTF");
        public static ImFont* ImFontAtlas_AddFontFromMemoryTTF(ImFontAtlas* self, void* font_data, int font_size, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => pImFontAtlas_AddFontFromMemoryTTF(self, font_data, font_size, size_pixels, font_cfg, glyph_ranges);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImFont* ImFontAtlas_AddFontFromFileTTF(ImFontAtlas* self, byte* filename, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontFromFileTTFDelegate(ImFontAtlas* self, byte* filename, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges);
        private static readonly ImFontAtlas_AddFontFromFileTTFDelegate pImFontAtlas_AddFontFromFileTTF = lib.LoadFunction<ImFontAtlas_AddFontFromFileTTFDelegate>("ImFontAtlas_AddFontFromFileTTF");
        public static ImFont* ImFontAtlas_AddFontFromFileTTF(ImFontAtlas* self, byte* filename, float size_pixels, ImFontConfig* font_cfg, ushort* glyph_ranges) => pImFontAtlas_AddFontFromFileTTF(self, filename, size_pixels, font_cfg, glyph_ranges);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igProgressBar(float fraction, Vector2 size_arg, byte* overlay);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igProgressBarDelegate(float fraction, Vector2 size_arg, byte* overlay);
        private static readonly igProgressBarDelegate pigProgressBar = lib.LoadFunction<igProgressBarDelegate>("igProgressBar");
        public static void igProgressBar(float fraction, Vector2 size_arg, byte* overlay) => pigProgressBar(fraction, size_arg, overlay);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImFont* ImFontAtlas_AddFontDefault(ImFontAtlas* self, ImFontConfig* font_cfg);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* ImFontAtlas_AddFontDefaultDelegate(ImFontAtlas* self, ImFontConfig* font_cfg);
        private static readonly ImFontAtlas_AddFontDefaultDelegate pImFontAtlas_AddFontDefault = lib.LoadFunction<ImFontAtlas_AddFontDefaultDelegate>("ImFontAtlas_AddFontDefault");
        public static ImFont* ImFontAtlas_AddFontDefault(ImFontAtlas* self, ImFontConfig* font_cfg) => pImFontAtlas_AddFontDefault(self, font_cfg);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetNextWindowBgAlpha(float alpha);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowBgAlphaDelegate(float alpha);
        private static readonly igSetNextWindowBgAlphaDelegate pigSetNextWindowBgAlpha = lib.LoadFunction<igSetNextWindowBgAlphaDelegate>("igSetNextWindowBgAlpha");
        public static void igSetNextWindowBgAlpha(float alpha) => pigSetNextWindowBgAlpha(alpha);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginPopup(byte* str_id, ImGuiWindowFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginPopupDelegate(byte* str_id, ImGuiWindowFlags flags);
        private static readonly igBeginPopupDelegate pigBeginPopup = lib.LoadFunction<igBeginPopupDelegate>("igBeginPopup");
        public static byte igBeginPopup(byte* str_id, ImGuiWindowFlags flags) => pigBeginPopup(str_id, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFont_BuildLookupTable(ImFont* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_BuildLookupTableDelegate(ImFont* self);
        private static readonly ImFont_BuildLookupTableDelegate pImFont_BuildLookupTable = lib.LoadFunction<ImFont_BuildLookupTableDelegate>("ImFont_BuildLookupTable");
        public static void ImFont_BuildLookupTable(ImFont* self) => pImFont_BuildLookupTable(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetScrollX();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetScrollXDelegate();
        private static readonly igGetScrollXDelegate pigGetScrollX = lib.LoadFunction<igGetScrollXDelegate>("igGetScrollX");
        public static float igGetScrollX() => pigGetScrollX();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern int igGetKeyIndex(ImGuiKey imgui_key);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int igGetKeyIndexDelegate(ImGuiKey imgui_key);
        private static readonly igGetKeyIndexDelegate pigGetKeyIndex = lib.LoadFunction<igGetKeyIndexDelegate>("igGetKeyIndex");
        public static int igGetKeyIndex(ImGuiKey imgui_key) => pigGetKeyIndex(imgui_key);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImDrawList* igGetOverlayDrawList();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImDrawList* igGetOverlayDrawListDelegate();
        private static readonly igGetOverlayDrawListDelegate pigGetOverlayDrawList = lib.LoadFunction<igGetOverlayDrawListDelegate>("igGetOverlayDrawList");
        public static ImDrawList* igGetOverlayDrawList() => pigGetOverlayDrawList();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint igGetIDStr(byte* str_id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetIDStrDelegate(byte* str_id);
        private static readonly igGetIDStrDelegate pigGetIDStr = lib.LoadFunction<igGetIDStrDelegate>("igGetIDStr");
        public static uint igGetIDStr(byte* str_id) => pigGetIDStr(str_id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint igGetIDRange(byte* str_id_begin, byte* str_id_end);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetIDRangeDelegate(byte* str_id_begin, byte* str_id_end);
        private static readonly igGetIDRangeDelegate pigGetIDRange = lib.LoadFunction<igGetIDRangeDelegate>("igGetIDRange");
        public static uint igGetIDRange(byte* str_id_begin, byte* str_id_end) => pigGetIDRange(str_id_begin, str_id_end);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint igGetIDPtr(void* ptr_id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetIDPtrDelegate(void* ptr_id);
        private static readonly igGetIDPtrDelegate pigGetIDPtr = lib.LoadFunction<igGetIDPtrDelegate>("igGetIDPtr");
        public static uint igGetIDPtr(void* ptr_id) => pigGetIDPtr(ptr_id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort* ImFontAtlas_GetGlyphRangesJapanese(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesJapaneseDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_GetGlyphRangesJapaneseDelegate pImFontAtlas_GetGlyphRangesJapanese = lib.LoadFunction<ImFontAtlas_GetGlyphRangesJapaneseDelegate>("ImFontAtlas_GetGlyphRangesJapanese");
        public static ushort* ImFontAtlas_GetGlyphRangesJapanese(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesJapanese(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igListBoxHeaderVec2(byte* label, Vector2 size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igListBoxHeaderVec2Delegate(byte* label, Vector2 size);
        private static readonly igListBoxHeaderVec2Delegate pigListBoxHeaderVec2 = lib.LoadFunction<igListBoxHeaderVec2Delegate>("igListBoxHeaderVec2");
        public static byte igListBoxHeaderVec2(byte* label, Vector2 size) => pigListBoxHeaderVec2(label, size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igListBoxHeaderInt(byte* label, int items_count, int height_in_items);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igListBoxHeaderIntDelegate(byte* label, int items_count, int height_in_items);
        private static readonly igListBoxHeaderIntDelegate pigListBoxHeaderInt = lib.LoadFunction<igListBoxHeaderIntDelegate>("igListBoxHeaderInt");
        public static byte igListBoxHeaderInt(byte* label, int items_count, int height_in_items) => pigListBoxHeaderInt(label, items_count, height_in_items);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFontConfig_ImFontConfig(ImFontConfig* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontConfig_ImFontConfigDelegate(ImFontConfig* self);
        private static readonly ImFontConfig_ImFontConfigDelegate pImFontConfig_ImFontConfig = lib.LoadFunction<ImFontConfig_ImFontConfigDelegate>("ImFontConfig_ImFontConfig");
        public static void ImFontConfig_ImFontConfig(ImFontConfig* self) => pImFontConfig_ImFontConfig(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsMouseReleased(int button);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseReleasedDelegate(int button);
        private static readonly igIsMouseReleasedDelegate pigIsMouseReleased = lib.LoadFunction<igIsMouseReleasedDelegate>("igIsMouseReleased");
        public static byte igIsMouseReleased(int button) => pigIsMouseReleased(button);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawData_ScaleClipRects(ImDrawData* self, Vector2 sc);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawData_ScaleClipRectsDelegate(ImDrawData* self, Vector2 sc);
        private static readonly ImDrawData_ScaleClipRectsDelegate pImDrawData_ScaleClipRects = lib.LoadFunction<ImDrawData_ScaleClipRectsDelegate>("ImDrawData_ScaleClipRects");
        public static void ImDrawData_ScaleClipRects(ImDrawData* self, Vector2 sc) => pImDrawData_ScaleClipRects(self, sc);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetItemRectMin_nonUDT2")]
        public static extern Vector2 igGetItemRectMin();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetItemRectMinDelegate();
        private static readonly igGetItemRectMinDelegate pigGetItemRectMin = lib.LoadFunction<igGetItemRectMinDelegate>("igGetItemRectMin");
        public static Vector2 igGetItemRectMin() => pigGetItemRectMin();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawData_DeIndexAllBuffers(ImDrawData* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawData_DeIndexAllBuffersDelegate(ImDrawData* self);
        private static readonly ImDrawData_DeIndexAllBuffersDelegate pImDrawData_DeIndexAllBuffers = lib.LoadFunction<ImDrawData_DeIndexAllBuffersDelegate>("ImDrawData_DeIndexAllBuffers");
        public static void ImDrawData_DeIndexAllBuffers(ImDrawData* self) => pImDrawData_DeIndexAllBuffers(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igLogText(byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogTextDelegate(byte* fmt);
        private static readonly igLogTextDelegate pigLogText = lib.LoadFunction<igLogTextDelegate>("igLogText");
        public static void igLogText(byte* fmt) => pigLogText(fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawData_Clear(ImDrawData* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawData_ClearDelegate(ImDrawData* self);
        private static readonly ImDrawData_ClearDelegate pImDrawData_Clear = lib.LoadFunction<ImDrawData_ClearDelegate>("ImDrawData_Clear");
        public static void ImDrawData_Clear(ImDrawData* self) => pImDrawData_Clear(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void* ImGuiStorage_GetVoidPtr(ImGuiStorage* self, uint key);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void* ImGuiStorage_GetVoidPtrDelegate(ImGuiStorage* self, uint key);
        private static readonly ImGuiStorage_GetVoidPtrDelegate pImGuiStorage_GetVoidPtr = lib.LoadFunction<ImGuiStorage_GetVoidPtrDelegate>("ImGuiStorage_GetVoidPtr");
        public static void* ImGuiStorage_GetVoidPtr(ImGuiStorage* self, uint key) => pImGuiStorage_GetVoidPtr(self, key);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igTextWrapped(byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTextWrappedDelegate(byte* fmt);
        private static readonly igTextWrappedDelegate pigTextWrapped = lib.LoadFunction<igTextWrappedDelegate>("igTextWrapped");
        public static void igTextWrapped(byte* fmt) => pigTextWrapped(fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_UpdateClipRect(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_UpdateClipRectDelegate(ImDrawList* self);
        private static readonly ImDrawList_UpdateClipRectDelegate pImDrawList_UpdateClipRect = lib.LoadFunction<ImDrawList_UpdateClipRectDelegate>("ImDrawList_UpdateClipRect");
        public static void ImDrawList_UpdateClipRect(ImDrawList* self) => pImDrawList_UpdateClipRect(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PrimVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimVtxDelegate(ImDrawList* self, Vector2 pos, Vector2 uv, uint col);
        private static readonly ImDrawList_PrimVtxDelegate pImDrawList_PrimVtx = lib.LoadFunction<ImDrawList_PrimVtxDelegate>("ImDrawList_PrimVtx");
        public static void ImDrawList_PrimVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col) => pImDrawList_PrimVtx(self, pos, uv, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndGroup();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndGroupDelegate();
        private static readonly igEndGroupDelegate pigEndGroup = lib.LoadFunction<igEndGroupDelegate>("igEndGroup");
        public static void igEndGroup() => pigEndGroup();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImFont* igGetFont();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImFont* igGetFontDelegate();
        private static readonly igGetFontDelegate pigGetFont = lib.LoadFunction<igGetFontDelegate>("igGetFont");
        public static ImFont* igGetFont() => pigGetFont();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igTreePushStr(byte* str_id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTreePushStrDelegate(byte* str_id);
        private static readonly igTreePushStrDelegate pigTreePushStr = lib.LoadFunction<igTreePushStrDelegate>("igTreePushStr");
        public static void igTreePushStr(byte* str_id) => pigTreePushStr(str_id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igTreePushPtr(void* ptr_id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTreePushPtrDelegate(void* ptr_id);
        private static readonly igTreePushPtrDelegate pigTreePushPtr = lib.LoadFunction<igTreePushPtrDelegate>("igTreePushPtr");
        public static void igTreePushPtr(void* ptr_id) => pigTreePushPtr(ptr_id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igTextDisabled(byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTextDisabledDelegate(byte* fmt);
        private static readonly igTextDisabledDelegate pigTextDisabled = lib.LoadFunction<igTextDisabledDelegate>("igTextDisabled");
        public static void igTextDisabled(byte* fmt) => pigTextDisabled(fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PrimRect(ImDrawList* self, Vector2 a, Vector2 b, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimRectDelegate(ImDrawList* self, Vector2 a, Vector2 b, uint col);
        private static readonly ImDrawList_PrimRectDelegate pImDrawList_PrimRect = lib.LoadFunction<ImDrawList_PrimRectDelegate>("ImDrawList_PrimRect");
        public static void ImDrawList_PrimRect(ImDrawList* self, Vector2 a, Vector2 b, uint col) => pImDrawList_PrimRect(self, a, b, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddQuad(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col, float thickness);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddQuadDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col, float thickness);
        private static readonly ImDrawList_AddQuadDelegate pImDrawList_AddQuad = lib.LoadFunction<ImDrawList_AddQuadDelegate>("ImDrawList_AddQuad");
        public static void ImDrawList_AddQuad(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col, float thickness) => pImDrawList_AddQuad(self, a, b, c, d, col, thickness);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_ClearFreeMemory(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ClearFreeMemoryDelegate(ImDrawList* self);
        private static readonly ImDrawList_ClearFreeMemoryDelegate pImDrawList_ClearFreeMemory = lib.LoadFunction<ImDrawList_ClearFreeMemoryDelegate>("ImDrawList_ClearFreeMemory");
        public static void ImDrawList_ClearFreeMemory(ImDrawList* self) => pImDrawList_ClearFreeMemory(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetNextTreeNodeOpen(byte is_open, ImGuiCond cond);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextTreeNodeOpenDelegate(byte is_open, ImGuiCond cond);
        private static readonly igSetNextTreeNodeOpenDelegate pigSetNextTreeNodeOpen = lib.LoadFunction<igSetNextTreeNodeOpenDelegate>("igSetNextTreeNodeOpen");
        public static void igSetNextTreeNodeOpen(byte is_open, ImGuiCond cond) => pigSetNextTreeNodeOpen(is_open, cond);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igLogToTTY(int max_depth);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogToTTYDelegate(int max_depth);
        private static readonly igLogToTTYDelegate pigLogToTTY = lib.LoadFunction<igLogToTTYDelegate>("igLogToTTY");
        public static void igLogToTTY(int max_depth) => pigLogToTTY(max_depth);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GlyphRangesBuilder_BuildRanges(GlyphRangesBuilder* self, ImVector* out_ranges);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_BuildRangesDelegate(GlyphRangesBuilder* self, ImVector* out_ranges);
        private static readonly GlyphRangesBuilder_BuildRangesDelegate pGlyphRangesBuilder_BuildRanges = lib.LoadFunction<GlyphRangesBuilder_BuildRangesDelegate>("GlyphRangesBuilder_BuildRanges");
        public static void GlyphRangesBuilder_BuildRanges(GlyphRangesBuilder* self, ImVector* out_ranges) => pGlyphRangesBuilder_BuildRanges(self, out_ranges);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImDrawList* ImDrawList_CloneOutput(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImDrawList* ImDrawList_CloneOutputDelegate(ImDrawList* self);
        private static readonly ImDrawList_CloneOutputDelegate pImDrawList_CloneOutput = lib.LoadFunction<ImDrawList_CloneOutputDelegate>("ImDrawList_CloneOutput");
        public static ImDrawList* ImDrawList_CloneOutput(ImDrawList* self) => pImDrawList_CloneOutput(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiIO* igGetIO();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImGuiIO* igGetIODelegate();
        private static readonly igGetIODelegate pigGetIO = lib.LoadFunction<igGetIODelegate>("igGetIO");
        public static ImGuiIO* igGetIO() => pigGetIO();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragInt4(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragInt4Delegate(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        private static readonly igDragInt4Delegate pigDragInt4 = lib.LoadFunction<igDragInt4Delegate>("igDragInt4");
        public static byte igDragInt4(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => pigDragInt4(label, v, v_speed, v_min, v_max, format);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igNextColumn();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igNextColumnDelegate();
        private static readonly igNextColumnDelegate pigNextColumn = lib.LoadFunction<igNextColumnDelegate>("igNextColumn");
        public static void igNextColumn() => pigNextColumn();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddRect(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags, float thickness);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddRectDelegate(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags, float thickness);
        private static readonly ImDrawList_AddRectDelegate pImDrawList_AddRect = lib.LoadFunction<ImDrawList_AddRectDelegate>("ImDrawList_AddRect");
        public static void ImDrawList_AddRect(ImDrawList* self, Vector2 a, Vector2 b, uint col, float rounding, int rounding_corners_flags, float thickness) => pImDrawList_AddRect(self, a, b, col, rounding, rounding_corners_flags, thickness);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TextRange_split(TextRange* self, byte separator, ImVector* @out);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TextRange_splitDelegate(TextRange* self, byte separator, ImVector* @out);
        private static readonly TextRange_splitDelegate pTextRange_split = lib.LoadFunction<TextRange_splitDelegate>("TextRange_split");
        public static void TextRange_split(TextRange* self, byte separator, ImVector* @out) => pTextRange_split(self, separator, @out);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetCursorPos(Vector2 local_pos);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetCursorPosDelegate(Vector2 local_pos);
        private static readonly igSetCursorPosDelegate pigSetCursorPos = lib.LoadFunction<igSetCursorPosDelegate>("igSetCursorPos");
        public static void igSetCursorPos(Vector2 local_pos) => pigSetCursorPos(local_pos);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginPopupModal(byte* name, byte* p_open, ImGuiWindowFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginPopupModalDelegate(byte* name, byte* p_open, ImGuiWindowFlags flags);
        private static readonly igBeginPopupModalDelegate pigBeginPopupModal = lib.LoadFunction<igBeginPopupModalDelegate>("igBeginPopupModal");
        public static byte igBeginPopupModal(byte* name, byte* p_open, ImGuiWindowFlags flags) => pigBeginPopupModal(name, p_open, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderInt4(byte* label, int* v, int v_min, int v_max, byte* format);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderInt4Delegate(byte* label, int* v, int v_min, int v_max, byte* format);
        private static readonly igSliderInt4Delegate pigSliderInt4 = lib.LoadFunction<igSliderInt4Delegate>("igSliderInt4");
        public static byte igSliderInt4(byte* label, int* v, int v_min, int v_max, byte* format) => pigSliderInt4(label, v, v_min, v_max, format);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddCallback(ImDrawList* self, IntPtr callback, void* callback_data);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddCallbackDelegate(ImDrawList* self, IntPtr callback, void* callback_data);
        private static readonly ImDrawList_AddCallbackDelegate pImDrawList_AddCallback = lib.LoadFunction<ImDrawList_AddCallbackDelegate>("ImDrawList_AddCallback");
        public static void ImDrawList_AddCallback(ImDrawList* self, IntPtr callback, void* callback_data) => pImDrawList_AddCallback(self, callback, callback_data);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igShowMetricsWindow(byte* p_open);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igShowMetricsWindowDelegate(byte* p_open);
        private static readonly igShowMetricsWindowDelegate pigShowMetricsWindow = lib.LoadFunction<igShowMetricsWindowDelegate>("igShowMetricsWindow");
        public static void igShowMetricsWindow(byte* p_open) => pigShowMetricsWindow(p_open);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetScrollMaxY();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetScrollMaxYDelegate();
        private static readonly igGetScrollMaxYDelegate pigGetScrollMaxY = lib.LoadFunction<igGetScrollMaxYDelegate>("igGetScrollMaxY");
        public static float igGetScrollMaxY() => pigGetScrollMaxY();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igBeginTooltip();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igBeginTooltipDelegate();
        private static readonly igBeginTooltipDelegate pigBeginTooltip = lib.LoadFunction<igBeginTooltipDelegate>("igBeginTooltip");
        public static void igBeginTooltip() => pigBeginTooltip();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetScrollX(float scroll_x);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetScrollXDelegate(float scroll_x);
        private static readonly igSetScrollXDelegate pigSetScrollX = lib.LoadFunction<igSetScrollXDelegate>("igSetScrollX");
        public static void igSetScrollX(float scroll_x) => pigSetScrollX(scroll_x);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImDrawData* igGetDrawData();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImDrawData* igGetDrawDataDelegate();
        private static readonly igGetDrawDataDelegate pigGetDrawData = lib.LoadFunction<igGetDrawDataDelegate>("igGetDrawData");
        public static ImDrawData* igGetDrawData() => pigGetDrawData();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetTextLineHeight();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetTextLineHeightDelegate();
        private static readonly igGetTextLineHeightDelegate pigGetTextLineHeight = lib.LoadFunction<igGetTextLineHeightDelegate>("igGetTextLineHeight");
        public static float igGetTextLineHeight() => pigGetTextLineHeight();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSeparator();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSeparatorDelegate();
        private static readonly igSeparatorDelegate pigSeparator = lib.LoadFunction<igSeparatorDelegate>("igSeparator");
        public static void igSeparator() => pigSeparator();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginChild(byte* str_id, Vector2 size, byte border, ImGuiWindowFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginChildDelegate(byte* str_id, Vector2 size, byte border, ImGuiWindowFlags flags);
        private static readonly igBeginChildDelegate pigBeginChild = lib.LoadFunction<igBeginChildDelegate>("igBeginChild");
        public static byte igBeginChild(byte* str_id, Vector2 size, byte border, ImGuiWindowFlags flags) => pigBeginChild(str_id, size, border, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginChildID(uint id, Vector2 size, byte border, ImGuiWindowFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginChildIDDelegate(uint id, Vector2 size, byte border, ImGuiWindowFlags flags);
        private static readonly igBeginChildIDDelegate pigBeginChildID = lib.LoadFunction<igBeginChildIDDelegate>("igBeginChildID");
        public static byte igBeginChildID(uint id, Vector2 size, byte border, ImGuiWindowFlags flags) => pigBeginChildID(id, size, border, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PathRect(ImDrawList* self, Vector2 rect_min, Vector2 rect_max, float rounding, int rounding_corners_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathRectDelegate(ImDrawList* self, Vector2 rect_min, Vector2 rect_max, float rounding, int rounding_corners_flags);
        private static readonly ImDrawList_PathRectDelegate pImDrawList_PathRect = lib.LoadFunction<ImDrawList_PathRectDelegate>("ImDrawList_PathRect");
        public static void ImDrawList_PathRect(ImDrawList* self, Vector2 rect_min, Vector2 rect_max, float rounding, int rounding_corners_flags) => pImDrawList_PathRect(self, rect_min, rect_max, rounding, rounding_corners_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsMouseClicked(int button, byte repeat);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseClickedDelegate(int button, byte repeat);
        private static readonly igIsMouseClickedDelegate pigIsMouseClicked = lib.LoadFunction<igIsMouseClickedDelegate>("igIsMouseClicked");
        public static byte igIsMouseClicked(int button, byte repeat) => pigIsMouseClicked(button, repeat);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igCalcItemWidth();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igCalcItemWidthDelegate();
        private static readonly igCalcItemWidthDelegate pigCalcItemWidth = lib.LoadFunction<igCalcItemWidthDelegate>("igCalcItemWidth");
        public static float igCalcItemWidth() => pigCalcItemWidth();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PathArcToFast(ImDrawList* self, Vector2 centre, float radius, int a_min_of_12, int a_max_of_12);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathArcToFastDelegate(ImDrawList* self, Vector2 centre, float radius, int a_min_of_12, int a_max_of_12);
        private static readonly ImDrawList_PathArcToFastDelegate pImDrawList_PathArcToFast = lib.LoadFunction<ImDrawList_PathArcToFastDelegate>("ImDrawList_PathArcToFast");
        public static void ImDrawList_PathArcToFast(ImDrawList* self, Vector2 centre, float radius, int a_min_of_12, int a_max_of_12) => pImDrawList_PathArcToFast(self, centre, radius, a_min_of_12, a_max_of_12);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndChildFrame();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndChildFrameDelegate();
        private static readonly igEndChildFrameDelegate pigEndChildFrame = lib.LoadFunction<igEndChildFrameDelegate>("igEndChildFrame");
        public static void igEndChildFrame() => pigEndChildFrame();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igIndent(float indent_w);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igIndentDelegate(float indent_w);
        private static readonly igIndentDelegate pigIndent = lib.LoadFunction<igIndentDelegate>("igIndent");
        public static void igIndent(float indent_w) => pigIndent(indent_w);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSetDragDropPayload(byte* type, void* data, uint size, ImGuiCond cond);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSetDragDropPayloadDelegate(byte* type, void* data, uint size, ImGuiCond cond);
        private static readonly igSetDragDropPayloadDelegate pigSetDragDropPayload = lib.LoadFunction<igSetDragDropPayloadDelegate>("igSetDragDropPayload");
        public static byte igSetDragDropPayload(byte* type, void* data, uint size, ImGuiCond cond) => pigSetDragDropPayload(type, data, size, cond);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte GlyphRangesBuilder_GetBit(GlyphRangesBuilder* self, int n);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte GlyphRangesBuilder_GetBitDelegate(GlyphRangesBuilder* self, int n);
        private static readonly GlyphRangesBuilder_GetBitDelegate pGlyphRangesBuilder_GetBit = lib.LoadFunction<GlyphRangesBuilder_GetBitDelegate>("GlyphRangesBuilder_GetBit");
        public static byte GlyphRangesBuilder_GetBit(GlyphRangesBuilder* self, int n) => pGlyphRangesBuilder_GetBit(self, n);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImGuiTextFilter_Draw(ImGuiTextFilter* self, byte* label, float width);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiTextFilter_DrawDelegate(ImGuiTextFilter* self, byte* label, float width);
        private static readonly ImGuiTextFilter_DrawDelegate pImGuiTextFilter_Draw = lib.LoadFunction<ImGuiTextFilter_DrawDelegate>("ImGuiTextFilter_Draw");
        public static byte ImGuiTextFilter_Draw(ImGuiTextFilter* self, byte* label, float width) => pImGuiTextFilter_Draw(self, label, width);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igShowDemoWindow(byte* p_open);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igShowDemoWindowDelegate(byte* p_open);
        private static readonly igShowDemoWindowDelegate pigShowDemoWindow = lib.LoadFunction<igShowDemoWindowDelegate>("igShowDemoWindow");
        public static void igShowDemoWindow(byte* p_open) => pigShowDemoWindow(p_open);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PathStroke(ImDrawList* self, uint col, byte closed, float thickness);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathStrokeDelegate(ImDrawList* self, uint col, byte closed, float thickness);
        private static readonly ImDrawList_PathStrokeDelegate pImDrawList_PathStroke = lib.LoadFunction<ImDrawList_PathStrokeDelegate>("ImDrawList_PathStroke");
        public static void ImDrawList_PathStroke(ImDrawList* self, uint col, byte closed, float thickness) => pImDrawList_PathStroke(self, col, closed, thickness);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PathFillConvex(ImDrawList* self, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathFillConvexDelegate(ImDrawList* self, uint col);
        private static readonly ImDrawList_PathFillConvexDelegate pImDrawList_PathFillConvex = lib.LoadFunction<ImDrawList_PathFillConvexDelegate>("ImDrawList_PathFillConvex");
        public static void ImDrawList_PathFillConvex(ImDrawList* self, uint col) => pImDrawList_PathFillConvex(self, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PathLineToMergeDuplicate(ImDrawList* self, Vector2 pos);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathLineToMergeDuplicateDelegate(ImDrawList* self, Vector2 pos);
        private static readonly ImDrawList_PathLineToMergeDuplicateDelegate pImDrawList_PathLineToMergeDuplicate = lib.LoadFunction<ImDrawList_PathLineToMergeDuplicateDelegate>("ImDrawList_PathLineToMergeDuplicate");
        public static void ImDrawList_PathLineToMergeDuplicate(ImDrawList* self, Vector2 pos) => pImDrawList_PathLineToMergeDuplicate(self, pos);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndMenu();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndMenuDelegate();
        private static readonly igEndMenuDelegate pigEndMenu = lib.LoadFunction<igEndMenuDelegate>("igEndMenu");
        public static void igEndMenu() => pigEndMenu();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igColorButton(byte* desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igColorButtonDelegate(byte* desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size);
        private static readonly igColorButtonDelegate pigColorButton = lib.LoadFunction<igColorButtonDelegate>("igColorButton");
        public static byte igColorButton(byte* desc_id, Vector4 col, ImGuiColorEditFlags flags, Vector2 size) => pigColorButton(desc_id, col, flags, size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFontAtlas_GetTexDataAsAlpha8(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_GetTexDataAsAlpha8Delegate(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);
        private static readonly ImFontAtlas_GetTexDataAsAlpha8Delegate pImFontAtlas_GetTexDataAsAlpha8 = lib.LoadFunction<ImFontAtlas_GetTexDataAsAlpha8Delegate>("ImFontAtlas_GetTexDataAsAlpha8");
        public static void ImFontAtlas_GetTexDataAsAlpha8(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel) => pImFontAtlas_GetTexDataAsAlpha8(self, out_pixels, out_width, out_height, out_bytes_per_pixel);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsKeyReleased(int user_key_index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsKeyReleasedDelegate(int user_key_index);
        private static readonly igIsKeyReleasedDelegate pigIsKeyReleased = lib.LoadFunction<igIsKeyReleasedDelegate>("igIsKeyReleased");
        public static byte igIsKeyReleased(int user_key_index) => pigIsKeyReleased(user_key_index);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetClipboardText(byte* text);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetClipboardTextDelegate(byte* text);
        private static readonly igSetClipboardTextDelegate pigSetClipboardText = lib.LoadFunction<igSetClipboardTextDelegate>("igSetClipboardText");
        public static void igSetClipboardText(byte* text) => pigSetClipboardText(text);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PathArcTo(ImDrawList* self, Vector2 centre, float radius, float a_min, float a_max, int num_segments);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathArcToDelegate(ImDrawList* self, Vector2 centre, float radius, float a_min, float a_max, int num_segments);
        private static readonly ImDrawList_PathArcToDelegate pImDrawList_PathArcTo = lib.LoadFunction<ImDrawList_PathArcToDelegate>("ImDrawList_PathArcTo");
        public static void ImDrawList_PathArcTo(ImDrawList* self, Vector2 centre, float radius, float a_min, float a_max, int num_segments) => pImDrawList_PathArcTo(self, centre, radius, a_min, a_max, num_segments);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddConvexPolyFilled(ImDrawList* self, Vector2* points, int num_points, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddConvexPolyFilledDelegate(ImDrawList* self, Vector2* points, int num_points, uint col);
        private static readonly ImDrawList_AddConvexPolyFilledDelegate pImDrawList_AddConvexPolyFilled = lib.LoadFunction<ImDrawList_AddConvexPolyFilledDelegate>("ImDrawList_AddConvexPolyFilled");
        public static void ImDrawList_AddConvexPolyFilled(ImDrawList* self, Vector2* points, int num_points, uint col) => pImDrawList_AddConvexPolyFilled(self, points, num_points, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsWindowCollapsed();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsWindowCollapsedDelegate();
        private static readonly igIsWindowCollapsedDelegate pigIsWindowCollapsed = lib.LoadFunction<igIsWindowCollapsedDelegate>("igIsWindowCollapsed");
        public static byte igIsWindowCollapsed() => pigIsWindowCollapsed();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igShowFontSelector(byte* label);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igShowFontSelectorDelegate(byte* label);
        private static readonly igShowFontSelectorDelegate pigShowFontSelector = lib.LoadFunction<igShowFontSelectorDelegate>("igShowFontSelector");
        public static void igShowFontSelector(byte* label) => pigShowFontSelector(label);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddImageQuad(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddImageQuadDelegate(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);
        private static readonly ImDrawList_AddImageQuadDelegate pImDrawList_AddImageQuad = lib.LoadFunction<ImDrawList_AddImageQuadDelegate>("ImDrawList_AddImageQuad");
        public static void ImDrawList_AddImageQuad(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col) => pImDrawList_AddImageQuad(self, user_texture_id, a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetNextWindowFocus();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowFocusDelegate();
        private static readonly igSetNextWindowFocusDelegate pigSetNextWindowFocus = lib.LoadFunction<igSetNextWindowFocusDelegate>("igSetNextWindowFocus");
        public static void igSetNextWindowFocus() => pigSetNextWindowFocus();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSameLine(float pos_x, float spacing_w);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSameLineDelegate(float pos_x, float spacing_w);
        private static readonly igSameLineDelegate pigSameLine = lib.LoadFunction<igSameLineDelegate>("igSameLine");
        public static void igSameLine(float pos_x, float spacing_w) => pigSameLine(pos_x, spacing_w);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBegin(byte* name, byte* p_open, ImGuiWindowFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginDelegate(byte* name, byte* p_open, ImGuiWindowFlags flags);
        private static readonly igBeginDelegate pigBegin = lib.LoadFunction<igBeginDelegate>("igBegin");
        public static byte igBegin(byte* name, byte* p_open, ImGuiWindowFlags flags) => pigBegin(name, p_open, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igColorEdit3(byte* label, Vector3* col, ImGuiColorEditFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igColorEdit3Delegate(byte* label, Vector3* col, ImGuiColorEditFlags flags);
        private static readonly igColorEdit3Delegate pigColorEdit3 = lib.LoadFunction<igColorEdit3Delegate>("igColorEdit3");
        public static byte igColorEdit3(byte* label, Vector3* col, ImGuiColorEditFlags flags) => pigColorEdit3(label, col, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddImage(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddImageDelegate(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);
        private static readonly ImDrawList_AddImageDelegate pImDrawList_AddImage = lib.LoadFunction<ImDrawList_AddImageDelegate>("ImDrawList_AddImage");
        public static void ImDrawList_AddImage(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col) => pImDrawList_AddImage(self, user_texture_id, a, b, uv_a, uv_b, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiIO_AddInputCharactersUTF8(ImGuiIO* self, byte* utf8_chars);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiIO_AddInputCharactersUTF8Delegate(ImGuiIO* self, byte* utf8_chars);
        private static readonly ImGuiIO_AddInputCharactersUTF8Delegate pImGuiIO_AddInputCharactersUTF8 = lib.LoadFunction<ImGuiIO_AddInputCharactersUTF8Delegate>("ImGuiIO_AddInputCharactersUTF8");
        public static void ImGuiIO_AddInputCharactersUTF8(ImGuiIO* self, byte* utf8_chars) => pImGuiIO_AddInputCharactersUTF8(self, utf8_chars);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddText(ImDrawList* self, Vector2 pos, uint col, byte* text_begin, byte* text_end);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddTextDelegate(ImDrawList* self, Vector2 pos, uint col, byte* text_begin, byte* text_end);
        private static readonly ImDrawList_AddTextDelegate pImDrawList_AddText = lib.LoadFunction<ImDrawList_AddTextDelegate>("ImDrawList_AddText");
        public static void ImDrawList_AddText(ImDrawList* self, Vector2 pos, uint col, byte* text_begin, byte* text_end) => pImDrawList_AddText(self, pos, col, text_begin, text_end);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddTextFontPtr(ImDrawList* self, ImFont* font, float font_size, Vector2 pos, uint col, byte* text_begin, byte* text_end, float wrap_width, Vector4* cpu_fine_clip_rect);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddTextFontPtrDelegate(ImDrawList* self, ImFont* font, float font_size, Vector2 pos, uint col, byte* text_begin, byte* text_end, float wrap_width, Vector4* cpu_fine_clip_rect);
        private static readonly ImDrawList_AddTextFontPtrDelegate pImDrawList_AddTextFontPtr = lib.LoadFunction<ImDrawList_AddTextFontPtrDelegate>("ImDrawList_AddTextFontPtr");
        public static void ImDrawList_AddTextFontPtr(ImDrawList* self, ImFont* font, float font_size, Vector2 pos, uint col, byte* text_begin, byte* text_end, float wrap_width, Vector4* cpu_fine_clip_rect) => pImDrawList_AddTextFontPtr(self, font, font_size, pos, col, text_begin, text_end, wrap_width, cpu_fine_clip_rect);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddCircleFilled(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddCircleFilledDelegate(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments);
        private static readonly ImDrawList_AddCircleFilledDelegate pImDrawList_AddCircleFilled = lib.LoadFunction<ImDrawList_AddCircleFilledDelegate>("ImDrawList_AddCircleFilled");
        public static void ImDrawList_AddCircleFilled(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments) => pImDrawList_AddCircleFilled(self, centre, radius, col, num_segments);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputFloat2(byte* label, Vector2* v, byte* format, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputFloat2Delegate(byte* label, Vector2* v, byte* format, ImGuiInputTextFlags extra_flags);
        private static readonly igInputFloat2Delegate pigInputFloat2 = lib.LoadFunction<igInputFloat2Delegate>("igInputFloat2");
        public static byte igInputFloat2(byte* label, Vector2* v, byte* format, ImGuiInputTextFlags extra_flags) => pigInputFloat2(label, v, format, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushButtonRepeat(byte repeat);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushButtonRepeatDelegate(byte repeat);
        private static readonly igPushButtonRepeatDelegate pigPushButtonRepeat = lib.LoadFunction<igPushButtonRepeatDelegate>("igPushButtonRepeat");
        public static void igPushButtonRepeat(byte repeat) => pigPushButtonRepeat(repeat);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPopItemWidth();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopItemWidthDelegate();
        private static readonly igPopItemWidthDelegate pigPopItemWidth = lib.LoadFunction<igPopItemWidthDelegate>("igPopItemWidth");
        public static void igPopItemWidth() => pigPopItemWidth();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddCircle(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments, float thickness);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddCircleDelegate(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments, float thickness);
        private static readonly ImDrawList_AddCircleDelegate pImDrawList_AddCircle = lib.LoadFunction<ImDrawList_AddCircleDelegate>("ImDrawList_AddCircle");
        public static void ImDrawList_AddCircle(ImDrawList* self, Vector2 centre, float radius, uint col, int num_segments, float thickness) => pImDrawList_AddCircle(self, centre, radius, col, num_segments, thickness);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddTriangleFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddTriangleFilledDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col);
        private static readonly ImDrawList_AddTriangleFilledDelegate pImDrawList_AddTriangleFilled = lib.LoadFunction<ImDrawList_AddTriangleFilledDelegate>("ImDrawList_AddTriangleFilled");
        public static void ImDrawList_AddTriangleFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col) => pImDrawList_AddTriangleFilled(self, a, b, c, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddTriangle(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col, float thickness);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddTriangleDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col, float thickness);
        private static readonly ImDrawList_AddTriangleDelegate pImDrawList_AddTriangle = lib.LoadFunction<ImDrawList_AddTriangleDelegate>("ImDrawList_AddTriangle");
        public static void ImDrawList_AddTriangle(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, uint col, float thickness) => pImDrawList_AddTriangle(self, a, b, c, col, thickness);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddQuadFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddQuadFilledDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col);
        private static readonly ImDrawList_AddQuadFilledDelegate pImDrawList_AddQuadFilled = lib.LoadFunction<ImDrawList_AddQuadFilledDelegate>("ImDrawList_AddQuadFilled");
        public static void ImDrawList_AddQuadFilled(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, uint col) => pImDrawList_AddQuadFilled(self, a, b, c, d, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetFontSize();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetFontSizeDelegate();
        private static readonly igGetFontSizeDelegate pigGetFontSize = lib.LoadFunction<igGetFontSizeDelegate>("igGetFontSize");
        public static float igGetFontSize() => pigGetFontSize();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputDouble(byte* label, double* v, double step, double step_fast, byte* format, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputDoubleDelegate(byte* label, double* v, double step, double step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        private static readonly igInputDoubleDelegate pigInputDouble = lib.LoadFunction<igInputDoubleDelegate>("igInputDouble");
        public static byte igInputDouble(byte* label, double* v, double step, double step_fast, byte* format, ImGuiInputTextFlags extra_flags) => pigInputDouble(label, v, step, step_fast, format, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PrimReserve(ImDrawList* self, int idx_count, int vtx_count);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimReserveDelegate(ImDrawList* self, int idx_count, int vtx_count);
        private static readonly ImDrawList_PrimReserveDelegate pImDrawList_PrimReserve = lib.LoadFunction<ImDrawList_PrimReserveDelegate>("ImDrawList_PrimReserve");
        public static void ImDrawList_PrimReserve(ImDrawList* self, int idx_count, int vtx_count) => pImDrawList_PrimReserve(self, idx_count, vtx_count);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddRectFilledMultiColor(ImDrawList* self, Vector2 a, Vector2 b, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddRectFilledMultiColorDelegate(ImDrawList* self, Vector2 a, Vector2 b, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left);
        private static readonly ImDrawList_AddRectFilledMultiColorDelegate pImDrawList_AddRectFilledMultiColor = lib.LoadFunction<ImDrawList_AddRectFilledMultiColorDelegate>("ImDrawList_AddRectFilledMultiColor");
        public static void ImDrawList_AddRectFilledMultiColor(ImDrawList* self, Vector2 a, Vector2 b, uint col_upr_left, uint col_upr_right, uint col_bot_right, uint col_bot_left) => pImDrawList_AddRectFilledMultiColor(self, a, b, col_upr_left, col_upr_right, col_bot_right, col_bot_left);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndPopup();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndPopupDelegate();
        private static readonly igEndPopupDelegate pigEndPopup = lib.LoadFunction<igEndPopupDelegate>("igEndPopup");
        public static void igEndPopup() => pigEndPopup();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFontAtlas_ClearInputData(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_ClearInputDataDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_ClearInputDataDelegate pImFontAtlas_ClearInputData = lib.LoadFunction<ImFontAtlas_ClearInputDataDelegate>("ImFontAtlas_ClearInputData");
        public static void ImFontAtlas_ClearInputData(ImFontAtlas* self) => pImFontAtlas_ClearInputData(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddLine(ImDrawList* self, Vector2 a, Vector2 b, uint col, float thickness);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddLineDelegate(ImDrawList* self, Vector2 a, Vector2 b, uint col, float thickness);
        private static readonly ImDrawList_AddLineDelegate pImDrawList_AddLine = lib.LoadFunction<ImDrawList_AddLineDelegate>("ImDrawList_AddLine");
        public static void ImDrawList_AddLine(ImDrawList* self, Vector2 a, Vector2 b, uint col, float thickness) => pImDrawList_AddLine(self, a, b, col, thickness);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputTextMultiline(byte* label, byte* buf, uint buf_size, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputTextMultilineDelegate(byte* label, byte* buf, uint buf_size, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);
        private static readonly igInputTextMultilineDelegate pigInputTextMultiline = lib.LoadFunction<igInputTextMultilineDelegate>("igInputTextMultiline");
        public static byte igInputTextMultiline(byte* label, byte* buf, uint buf_size, Vector2 size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data) => pigInputTextMultiline(label, buf, buf_size, size, flags, callback, user_data);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSelectable(byte* label, byte selected, ImGuiSelectableFlags flags, Vector2 size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSelectableDelegate(byte* label, byte selected, ImGuiSelectableFlags flags, Vector2 size);
        private static readonly igSelectableDelegate pigSelectable = lib.LoadFunction<igSelectableDelegate>("igSelectable");
        public static byte igSelectable(byte* label, byte selected, ImGuiSelectableFlags flags, Vector2 size) => pigSelectable(label, selected, flags, size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSelectableBoolPtr(byte* label, byte* p_selected, ImGuiSelectableFlags flags, Vector2 size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSelectableBoolPtrDelegate(byte* label, byte* p_selected, ImGuiSelectableFlags flags, Vector2 size);
        private static readonly igSelectableBoolPtrDelegate pigSelectableBoolPtr = lib.LoadFunction<igSelectableBoolPtrDelegate>("igSelectableBoolPtr");
        public static byte igSelectableBoolPtr(byte* label, byte* p_selected, ImGuiSelectableFlags flags, Vector2 size) => pigSelectableBoolPtr(label, p_selected, flags, size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igListBoxStr_arr(byte* label, int* current_item, byte** items, int items_count, int height_in_items);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igListBoxStr_arrDelegate(byte* label, int* current_item, byte** items, int items_count, int height_in_items);
        private static readonly igListBoxStr_arrDelegate pigListBoxStr_arr = lib.LoadFunction<igListBoxStr_arrDelegate>("igListBoxStr_arr");
        public static byte igListBoxStr_arr(byte* label, int* current_item, byte** items, int items_count, int height_in_items) => pigListBoxStr_arr(label, current_item, items, items_count, height_in_items);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetCursorPos_nonUDT2")]
        public static extern Vector2 igGetCursorPos();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetCursorPosDelegate();
        private static readonly igGetCursorPosDelegate pigGetCursorPos = lib.LoadFunction<igGetCursorPosDelegate>("igGetCursorPos");
        public static Vector2 igGetCursorPos() => pigGetCursorPos();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImDrawList_GetClipRectMin_nonUDT2")]
        public static extern Vector2 ImDrawList_GetClipRectMin(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 ImDrawList_GetClipRectMinDelegate(ImDrawList* self);
        private static readonly ImDrawList_GetClipRectMinDelegate pImDrawList_GetClipRectMin = lib.LoadFunction<ImDrawList_GetClipRectMinDelegate>("ImDrawList_GetClipRectMin");
        public static Vector2 ImDrawList_GetClipRectMin(ImDrawList* self) => pImDrawList_GetClipRectMin(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PopTextureID(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PopTextureIDDelegate(ImDrawList* self);
        private static readonly ImDrawList_PopTextureIDDelegate pImDrawList_PopTextureID = lib.LoadFunction<ImDrawList_PopTextureIDDelegate>("ImDrawList_PopTextureID");
        public static void ImDrawList_PopTextureID(ImDrawList* self) => pImDrawList_PopTextureID(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputFloat4(byte* label, Vector4* v, byte* format, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputFloat4Delegate(byte* label, Vector4* v, byte* format, ImGuiInputTextFlags extra_flags);
        private static readonly igInputFloat4Delegate pigInputFloat4 = lib.LoadFunction<igInputFloat4Delegate>("igInputFloat4");
        public static byte igInputFloat4(byte* label, Vector4* v, byte* format, ImGuiInputTextFlags extra_flags) => pigInputFloat4(label, v, format, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetCursorPosY(float y);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetCursorPosYDelegate(float y);
        private static readonly igSetCursorPosYDelegate pigSetCursorPosY = lib.LoadFunction<igSetCursorPosYDelegate>("igSetCursorPosY");
        public static void igSetCursorPosY(float y) => pigSetCursorPosY(y);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* igGetVersion();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* igGetVersionDelegate();
        private static readonly igGetVersionDelegate pigGetVersion = lib.LoadFunction<igGetVersionDelegate>("igGetVersion");
        public static byte* igGetVersion() => pigGetVersion();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndCombo();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndComboDelegate();
        private static readonly igEndComboDelegate pigEndCombo = lib.LoadFunction<igEndComboDelegate>("igEndCombo");
        public static void igEndCombo() => pigEndCombo();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushIDStr(byte* str_id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushIDStrDelegate(byte* str_id);
        private static readonly igPushIDStrDelegate pigPushIDStr = lib.LoadFunction<igPushIDStrDelegate>("igPushIDStr");
        public static void igPushIDStr(byte* str_id) => pigPushIDStr(str_id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushIDRange(byte* str_id_begin, byte* str_id_end);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushIDRangeDelegate(byte* str_id_begin, byte* str_id_end);
        private static readonly igPushIDRangeDelegate pigPushIDRange = lib.LoadFunction<igPushIDRangeDelegate>("igPushIDRange");
        public static void igPushIDRange(byte* str_id_begin, byte* str_id_end) => pigPushIDRange(str_id_begin, str_id_end);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushIDPtr(void* ptr_id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushIDPtrDelegate(void* ptr_id);
        private static readonly igPushIDPtrDelegate pigPushIDPtr = lib.LoadFunction<igPushIDPtrDelegate>("igPushIDPtr");
        public static void igPushIDPtr(void* ptr_id) => pigPushIDPtr(ptr_id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushIDInt(int int_id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushIDIntDelegate(int int_id);
        private static readonly igPushIDIntDelegate pigPushIDInt = lib.LoadFunction<igPushIDIntDelegate>("igPushIDInt");
        public static void igPushIDInt(int int_id) => pigPushIDInt(int_id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_ImDrawList(ImDrawList* self, IntPtr shared_data);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ImDrawListDelegate(ImDrawList* self, IntPtr shared_data);
        private static readonly ImDrawList_ImDrawListDelegate pImDrawList_ImDrawList = lib.LoadFunction<ImDrawList_ImDrawListDelegate>("ImDrawList_ImDrawList");
        public static void ImDrawList_ImDrawList(ImDrawList* self, IntPtr shared_data) => pImDrawList_ImDrawList(self, shared_data);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawCmd_ImDrawCmd(ImDrawCmd* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawCmd_ImDrawCmdDelegate(ImDrawCmd* self);
        private static readonly ImDrawCmd_ImDrawCmdDelegate pImDrawCmd_ImDrawCmd = lib.LoadFunction<ImDrawCmd_ImDrawCmdDelegate>("ImDrawCmd_ImDrawCmd");
        public static void ImDrawCmd_ImDrawCmd(ImDrawCmd* self) => pImDrawCmd_ImDrawCmd(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiListClipper_End(ImGuiListClipper* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiListClipper_EndDelegate(ImGuiListClipper* self);
        private static readonly ImGuiListClipper_EndDelegate pImGuiListClipper_End = lib.LoadFunction<ImGuiListClipper_EndDelegate>("ImGuiListClipper_End");
        public static void ImGuiListClipper_End(ImGuiListClipper* self) => pImGuiListClipper_End(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igAlignTextToFramePadding();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igAlignTextToFramePaddingDelegate();
        private static readonly igAlignTextToFramePaddingDelegate pigAlignTextToFramePadding = lib.LoadFunction<igAlignTextToFramePaddingDelegate>("igAlignTextToFramePadding");
        public static void igAlignTextToFramePadding() => pigAlignTextToFramePadding();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPopStyleColor(int count);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopStyleColorDelegate(int count);
        private static readonly igPopStyleColorDelegate pigPopStyleColor = lib.LoadFunction<igPopStyleColorDelegate>("igPopStyleColor");
        public static void igPopStyleColor(int count) => pigPopStyleColor(count);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiListClipper_Begin(ImGuiListClipper* self, int items_count, float items_height);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiListClipper_BeginDelegate(ImGuiListClipper* self, int items_count, float items_height);
        private static readonly ImGuiListClipper_BeginDelegate pImGuiListClipper_Begin = lib.LoadFunction<ImGuiListClipper_BeginDelegate>("ImGuiListClipper_Begin");
        public static void ImGuiListClipper_Begin(ImGuiListClipper* self, int items_count, float items_height) => pImGuiListClipper_Begin(self, items_count, items_height);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igText(byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTextDelegate(byte* fmt);
        private static readonly igTextDelegate pigText = lib.LoadFunction<igTextDelegate>("igText");
        public static void igText(byte* fmt) => pigText(fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImGuiListClipper_Step(ImGuiListClipper* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiListClipper_StepDelegate(ImGuiListClipper* self);
        private static readonly ImGuiListClipper_StepDelegate pImGuiListClipper_Step = lib.LoadFunction<ImGuiListClipper_StepDelegate>("ImGuiListClipper_Step");
        public static byte ImGuiListClipper_Step(ImGuiListClipper* self) => pImGuiListClipper_Step(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetTextLineHeightWithSpacing();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetTextLineHeightWithSpacingDelegate();
        private static readonly igGetTextLineHeightWithSpacingDelegate pigGetTextLineHeightWithSpacing = lib.LoadFunction<igGetTextLineHeightWithSpacingDelegate>("igGetTextLineHeightWithSpacing");
        public static float igGetTextLineHeightWithSpacing() => pigGetTextLineHeightWithSpacing();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float* ImGuiStorage_GetFloatRef(ImGuiStorage* self, uint key, float default_val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float* ImGuiStorage_GetFloatRefDelegate(ImGuiStorage* self, uint key, float default_val);
        private static readonly ImGuiStorage_GetFloatRefDelegate pImGuiStorage_GetFloatRef = lib.LoadFunction<ImGuiStorage_GetFloatRefDelegate>("ImGuiStorage_GetFloatRef");
        public static float* ImGuiStorage_GetFloatRef(ImGuiStorage* self, uint key, float default_val) => pImGuiStorage_GetFloatRef(self, key, default_val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndTooltip();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndTooltipDelegate();
        private static readonly igEndTooltipDelegate pigEndTooltip = lib.LoadFunction<igEndTooltipDelegate>("igEndTooltip");
        public static void igEndTooltip() => pigEndTooltip();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiListClipper_ImGuiListClipper(ImGuiListClipper* self, int items_count, float items_height);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiListClipper_ImGuiListClipperDelegate(ImGuiListClipper* self, int items_count, float items_height);
        private static readonly ImGuiListClipper_ImGuiListClipperDelegate pImGuiListClipper_ImGuiListClipper = lib.LoadFunction<ImGuiListClipper_ImGuiListClipperDelegate>("ImGuiListClipper_ImGuiListClipper");
        public static void ImGuiListClipper_ImGuiListClipper(ImGuiListClipper* self, int items_count, float items_height) => pImGuiListClipper_ImGuiListClipper(self, items_count, items_height);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragInt(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragIntDelegate(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        private static readonly igDragIntDelegate pigDragInt = lib.LoadFunction<igDragIntDelegate>("igDragInt");
        public static byte igDragInt(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => pigDragInt(label, v, v_speed, v_min, v_max, format);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderFloat(byte* label, float* v, float v_min, float v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderFloatDelegate(byte* label, float* v, float v_min, float v_max, byte* format, float power);
        private static readonly igSliderFloatDelegate pigSliderFloat = lib.LoadFunction<igSliderFloatDelegate>("igSliderFloat");
        public static byte igSliderFloat(byte* label, float* v, float v_min, float v_max, byte* format, float power) => pigSliderFloat(label, v, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint igColorConvertFloat4ToU32(Vector4 @in);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igColorConvertFloat4ToU32Delegate(Vector4 @in);
        private static readonly igColorConvertFloat4ToU32Delegate pigColorConvertFloat4ToU32 = lib.LoadFunction<igColorConvertFloat4ToU32Delegate>("igColorConvertFloat4ToU32");
        public static uint igColorConvertFloat4ToU32(Vector4 @in) => pigColorConvertFloat4ToU32(@in);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiIO_ClearInputCharacters(ImGuiIO* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiIO_ClearInputCharactersDelegate(ImGuiIO* self);
        private static readonly ImGuiIO_ClearInputCharactersDelegate pImGuiIO_ClearInputCharacters = lib.LoadFunction<ImGuiIO_ClearInputCharactersDelegate>("ImGuiIO_ClearInputCharacters");
        public static void ImGuiIO_ClearInputCharacters(ImGuiIO* self) => pImGuiIO_ClearInputCharacters(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushClipRectDelegate(Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect);
        private static readonly igPushClipRectDelegate pigPushClipRect = lib.LoadFunction<igPushClipRectDelegate>("igPushClipRect");
        public static void igPushClipRect(Vector2 clip_rect_min, Vector2 clip_rect_max, byte intersect_with_current_clip_rect) => pigPushClipRect(clip_rect_min, clip_rect_max, intersect_with_current_clip_rect);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetColumnWidth(int column_index, float width);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetColumnWidthDelegate(int column_index, float width);
        private static readonly igSetColumnWidthDelegate pigSetColumnWidth = lib.LoadFunction<igSetColumnWidthDelegate>("igSetColumnWidth");
        public static void igSetColumnWidth(int column_index, float width) => pigSetColumnWidth(column_index, width);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImGuiPayload_IsDataType(ImGuiPayload* self, byte* type);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiPayload_IsDataTypeDelegate(ImGuiPayload* self, byte* type);
        private static readonly ImGuiPayload_IsDataTypeDelegate pImGuiPayload_IsDataType = lib.LoadFunction<ImGuiPayload_IsDataTypeDelegate>("ImGuiPayload_IsDataType");
        public static byte ImGuiPayload_IsDataType(ImGuiPayload* self, byte* type) => pImGuiPayload_IsDataType(self, type);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginMainMenuBar();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginMainMenuBarDelegate();
        private static readonly igBeginMainMenuBarDelegate pigBeginMainMenuBar = lib.LoadFunction<igBeginMainMenuBarDelegate>("igBeginMainMenuBar");
        public static byte igBeginMainMenuBar() => pigBeginMainMenuBar();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void CustomRect_CustomRect(CustomRect* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CustomRect_CustomRectDelegate(CustomRect* self);
        private static readonly CustomRect_CustomRectDelegate pCustomRect_CustomRect = lib.LoadFunction<CustomRect_CustomRectDelegate>("CustomRect_CustomRect");
        public static void CustomRect_CustomRect(CustomRect* self) => pCustomRect_CustomRect(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImGuiInputTextCallbackData_HasSelection(ImGuiInputTextCallbackData* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiInputTextCallbackData_HasSelectionDelegate(ImGuiInputTextCallbackData* self);
        private static readonly ImGuiInputTextCallbackData_HasSelectionDelegate pImGuiInputTextCallbackData_HasSelection = lib.LoadFunction<ImGuiInputTextCallbackData_HasSelectionDelegate>("ImGuiInputTextCallbackData_HasSelection");
        public static byte ImGuiInputTextCallbackData_HasSelection(ImGuiInputTextCallbackData* self) => pImGuiInputTextCallbackData_HasSelection(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiInputTextCallbackData_InsertChars(ImGuiInputTextCallbackData* self, int pos, byte* text, byte* text_end);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiInputTextCallbackData_InsertCharsDelegate(ImGuiInputTextCallbackData* self, int pos, byte* text, byte* text_end);
        private static readonly ImGuiInputTextCallbackData_InsertCharsDelegate pImGuiInputTextCallbackData_InsertChars = lib.LoadFunction<ImGuiInputTextCallbackData_InsertCharsDelegate>("ImGuiInputTextCallbackData_InsertChars");
        public static void ImGuiInputTextCallbackData_InsertChars(ImGuiInputTextCallbackData* self, int pos, byte* text, byte* text_end) => pImGuiInputTextCallbackData_InsertChars(self, pos, text, text_end);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImFontAtlas_GetMouseCursorTexData(ImFontAtlas* self, ImGuiMouseCursor cursor, Vector2* out_offset, Vector2* out_size, Vector2* out_uv_border, Vector2* out_uv_fill);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImFontAtlas_GetMouseCursorTexDataDelegate(ImFontAtlas* self, ImGuiMouseCursor cursor, Vector2* out_offset, Vector2* out_size, Vector2* out_uv_border, Vector2* out_uv_fill);
        private static readonly ImFontAtlas_GetMouseCursorTexDataDelegate pImFontAtlas_GetMouseCursorTexData = lib.LoadFunction<ImFontAtlas_GetMouseCursorTexDataDelegate>("ImFontAtlas_GetMouseCursorTexData");
        public static byte ImFontAtlas_GetMouseCursorTexData(ImFontAtlas* self, ImGuiMouseCursor cursor, Vector2* out_offset, Vector2* out_size, Vector2* out_uv_border, Vector2* out_uv_fill) => pImFontAtlas_GetMouseCursorTexData(self, cursor, out_offset, out_size, out_uv_border, out_uv_fill);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igVSliderScalar(byte* label, Vector2 size, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igVSliderScalarDelegate(byte* label, Vector2 size, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power);
        private static readonly igVSliderScalarDelegate pigVSliderScalar = lib.LoadFunction<igVSliderScalarDelegate>("igVSliderScalar");
        public static byte igVSliderScalar(byte* label, Vector2 size, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power) => pigVSliderScalar(label, size, data_type, v, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiStorage_SetAllInt(ImGuiStorage* self, int val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_SetAllIntDelegate(ImGuiStorage* self, int val);
        private static readonly ImGuiStorage_SetAllIntDelegate pImGuiStorage_SetAllInt = lib.LoadFunction<ImGuiStorage_SetAllIntDelegate>("ImGuiStorage_SetAllInt");
        public static void ImGuiStorage_SetAllInt(ImGuiStorage* self, int val) => pImGuiStorage_SetAllInt(self, val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void** ImGuiStorage_GetVoidPtrRef(ImGuiStorage* self, uint key, void* default_val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void** ImGuiStorage_GetVoidPtrRefDelegate(ImGuiStorage* self, uint key, void* default_val);
        private static readonly ImGuiStorage_GetVoidPtrRefDelegate pImGuiStorage_GetVoidPtrRef = lib.LoadFunction<ImGuiStorage_GetVoidPtrRefDelegate>("ImGuiStorage_GetVoidPtrRef");
        public static void** ImGuiStorage_GetVoidPtrRef(ImGuiStorage* self, uint key, void* default_val) => pImGuiStorage_GetVoidPtrRef(self, key, default_val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igStyleColorsLight(ImGuiStyle* dst);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igStyleColorsLightDelegate(ImGuiStyle* dst);
        private static readonly igStyleColorsLightDelegate pigStyleColorsLight = lib.LoadFunction<igStyleColorsLightDelegate>("igStyleColorsLight");
        public static void igStyleColorsLight(ImGuiStyle* dst) => pigStyleColorsLight(dst);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderFloat3(byte* label, Vector3* v, float v_min, float v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderFloat3Delegate(byte* label, Vector3* v, float v_min, float v_max, byte* format, float power);
        private static readonly igSliderFloat3Delegate pigSliderFloat3 = lib.LoadFunction<igSliderFloat3Delegate>("igSliderFloat3");
        public static byte igSliderFloat3(byte* label, Vector3* v, float v_min, float v_max, byte* format, float power) => pigSliderFloat3(label, v, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragFloat(byte* label, float* v, float v_speed, float v_min, float v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragFloatDelegate(byte* label, float* v, float v_speed, float v_min, float v_max, byte* format, float power);
        private static readonly igDragFloatDelegate pigDragFloat = lib.LoadFunction<igDragFloatDelegate>("igDragFloat");
        public static byte igDragFloat(byte* label, float* v, float v_speed, float v_min, float v_max, byte* format, float power) => pigDragFloat(label, v, v_speed, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* ImGuiStorage_GetBoolRef(ImGuiStorage* self, uint key, byte default_val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImGuiStorage_GetBoolRefDelegate(ImGuiStorage* self, uint key, byte default_val);
        private static readonly ImGuiStorage_GetBoolRefDelegate pImGuiStorage_GetBoolRef = lib.LoadFunction<ImGuiStorage_GetBoolRefDelegate>("ImGuiStorage_GetBoolRef");
        public static byte* ImGuiStorage_GetBoolRef(ImGuiStorage* self, uint key, byte default_val) => pImGuiStorage_GetBoolRef(self, key, default_val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetWindowHeight();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetWindowHeightDelegate();
        private static readonly igGetWindowHeightDelegate pigGetWindowHeight = lib.LoadFunction<igGetWindowHeightDelegate>("igGetWindowHeight");
        public static float igGetWindowHeight() => pigGetWindowHeight();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetMousePosOnOpeningCurrentPopup_nonUDT2")]
        public static extern Vector2 igGetMousePosOnOpeningCurrentPopup();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetMousePosOnOpeningCurrentPopupDelegate();
        private static readonly igGetMousePosOnOpeningCurrentPopupDelegate pigGetMousePosOnOpeningCurrentPopup = lib.LoadFunction<igGetMousePosOnOpeningCurrentPopupDelegate>("igGetMousePosOnOpeningCurrentPopup");
        public static Vector2 igGetMousePosOnOpeningCurrentPopup() => pigGetMousePosOnOpeningCurrentPopup();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern int* ImGuiStorage_GetIntRef(ImGuiStorage* self, uint key, int default_val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int* ImGuiStorage_GetIntRefDelegate(ImGuiStorage* self, uint key, int default_val);
        private static readonly ImGuiStorage_GetIntRefDelegate pImGuiStorage_GetIntRef = lib.LoadFunction<ImGuiStorage_GetIntRefDelegate>("ImGuiStorage_GetIntRef");
        public static int* ImGuiStorage_GetIntRef(ImGuiStorage* self, uint key, int default_val) => pImGuiStorage_GetIntRef(self, key, default_val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igCalcListClipping(int items_count, float items_height, int* out_items_display_start, int* out_items_display_end);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igCalcListClippingDelegate(int items_count, float items_height, int* out_items_display_start, int* out_items_display_end);
        private static readonly igCalcListClippingDelegate pigCalcListClipping = lib.LoadFunction<igCalcListClippingDelegate>("igCalcListClipping");
        public static void igCalcListClipping(int items_count, float items_height, int* out_items_display_start, int* out_items_display_end) => pigCalcListClipping(items_count, items_height, out_items_display_start, out_items_display_end);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiStorage_SetVoidPtr(ImGuiStorage* self, uint key, void* val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_SetVoidPtrDelegate(ImGuiStorage* self, uint key, void* val);
        private static readonly ImGuiStorage_SetVoidPtrDelegate pImGuiStorage_SetVoidPtr = lib.LoadFunction<ImGuiStorage_SetVoidPtrDelegate>("ImGuiStorage_SetVoidPtr");
        public static void ImGuiStorage_SetVoidPtr(ImGuiStorage* self, uint key, void* val) => pImGuiStorage_SetVoidPtr(self, key, val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndDragDropSource();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndDragDropSourceDelegate();
        private static readonly igEndDragDropSourceDelegate pigEndDragDropSource = lib.LoadFunction<igEndDragDropSourceDelegate>("igEndDragDropSource");
        public static void igEndDragDropSource() => pigEndDragDropSource();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiStorage_BuildSortByKey(ImGuiStorage* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_BuildSortByKeyDelegate(ImGuiStorage* self);
        private static readonly ImGuiStorage_BuildSortByKeyDelegate pImGuiStorage_BuildSortByKey = lib.LoadFunction<ImGuiStorage_BuildSortByKeyDelegate>("ImGuiStorage_BuildSortByKey");
        public static void ImGuiStorage_BuildSortByKey(ImGuiStorage* self) => pImGuiStorage_BuildSortByKey(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float ImGuiStorage_GetFloat(ImGuiStorage* self, uint key, float default_val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float ImGuiStorage_GetFloatDelegate(ImGuiStorage* self, uint key, float default_val);
        private static readonly ImGuiStorage_GetFloatDelegate pImGuiStorage_GetFloat = lib.LoadFunction<ImGuiStorage_GetFloatDelegate>("ImGuiStorage_GetFloat");
        public static float ImGuiStorage_GetFloat(ImGuiStorage* self, uint key, float default_val) => pImGuiStorage_GetFloat(self, key, default_val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiStorage_SetBool(ImGuiStorage* self, uint key, byte val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_SetBoolDelegate(ImGuiStorage* self, uint key, byte val);
        private static readonly ImGuiStorage_SetBoolDelegate pImGuiStorage_SetBool = lib.LoadFunction<ImGuiStorage_SetBoolDelegate>("ImGuiStorage_SetBool");
        public static void ImGuiStorage_SetBool(ImGuiStorage* self, uint key, byte val) => pImGuiStorage_SetBool(self, key, val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImGuiStorage_GetBool(ImGuiStorage* self, uint key, byte default_val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiStorage_GetBoolDelegate(ImGuiStorage* self, uint key, byte default_val);
        private static readonly ImGuiStorage_GetBoolDelegate pImGuiStorage_GetBool = lib.LoadFunction<ImGuiStorage_GetBoolDelegate>("ImGuiStorage_GetBool");
        public static byte ImGuiStorage_GetBool(ImGuiStorage* self, uint key, byte default_val) => pImGuiStorage_GetBool(self, key, default_val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetFrameHeightWithSpacing();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetFrameHeightWithSpacingDelegate();
        private static readonly igGetFrameHeightWithSpacingDelegate pigGetFrameHeightWithSpacing = lib.LoadFunction<igGetFrameHeightWithSpacingDelegate>("igGetFrameHeightWithSpacing");
        public static float igGetFrameHeightWithSpacing() => pigGetFrameHeightWithSpacing();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiStorage_SetInt(ImGuiStorage* self, uint key, int val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_SetIntDelegate(ImGuiStorage* self, uint key, int val);
        private static readonly ImGuiStorage_SetIntDelegate pImGuiStorage_SetInt = lib.LoadFunction<ImGuiStorage_SetIntDelegate>("ImGuiStorage_SetInt");
        public static void ImGuiStorage_SetInt(ImGuiStorage* self, uint key, int val) => pImGuiStorage_SetInt(self, key, val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igCloseCurrentPopup();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igCloseCurrentPopupDelegate();
        private static readonly igCloseCurrentPopupDelegate pigCloseCurrentPopup = lib.LoadFunction<igCloseCurrentPopupDelegate>("igCloseCurrentPopup");
        public static void igCloseCurrentPopup() => pigCloseCurrentPopup();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiTextBuffer_clear(ImGuiTextBuffer* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextBuffer_clearDelegate(ImGuiTextBuffer* self);
        private static readonly ImGuiTextBuffer_clearDelegate pImGuiTextBuffer_clear = lib.LoadFunction<ImGuiTextBuffer_clearDelegate>("ImGuiTextBuffer_clear");
        public static void ImGuiTextBuffer_clear(ImGuiTextBuffer* self) => pImGuiTextBuffer_clear(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igBeginGroup();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igBeginGroupDelegate();
        private static readonly igBeginGroupDelegate pigBeginGroup = lib.LoadFunction<igBeginGroupDelegate>("igBeginGroup");
        public static void igBeginGroup() => pigBeginGroup();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiStorage_Clear(ImGuiStorage* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStorage_ClearDelegate(ImGuiStorage* self);
        private static readonly ImGuiStorage_ClearDelegate pImGuiStorage_Clear = lib.LoadFunction<ImGuiStorage_ClearDelegate>("ImGuiStorage_Clear");
        public static void ImGuiStorage_Clear(ImGuiStorage* self) => pImGuiStorage_Clear(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Pair_PairInt(Pair* self, uint _key, int _val_i);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void Pair_PairIntDelegate(Pair* self, uint _key, int _val_i);
        private static readonly Pair_PairIntDelegate pPair_PairInt = lib.LoadFunction<Pair_PairIntDelegate>("Pair_PairInt");
        public static void Pair_PairInt(Pair* self, uint _key, int _val_i) => pPair_PairInt(self, _key, _val_i);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Pair_PairFloat(Pair* self, uint _key, float _val_f);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void Pair_PairFloatDelegate(Pair* self, uint _key, float _val_f);
        private static readonly Pair_PairFloatDelegate pPair_PairFloat = lib.LoadFunction<Pair_PairFloatDelegate>("Pair_PairFloat");
        public static void Pair_PairFloat(Pair* self, uint _key, float _val_f) => pPair_PairFloat(self, _key, _val_f);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void Pair_PairPtr(Pair* self, uint _key, void* _val_p);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void Pair_PairPtrDelegate(Pair* self, uint _key, void* _val_p);
        private static readonly Pair_PairPtrDelegate pPair_PairPtr = lib.LoadFunction<Pair_PairPtrDelegate>("Pair_PairPtr");
        public static void Pair_PairPtr(Pair* self, uint _key, void* _val_p) => pPair_PairPtr(self, _key, _val_p);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiTextBuffer_appendf(ImGuiTextBuffer* self, byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextBuffer_appendfDelegate(ImGuiTextBuffer* self, byte* fmt);
        private static readonly ImGuiTextBuffer_appendfDelegate pImGuiTextBuffer_appendf = lib.LoadFunction<ImGuiTextBuffer_appendfDelegate>("ImGuiTextBuffer_appendf");
        public static void ImGuiTextBuffer_appendf(ImGuiTextBuffer* self, byte* fmt) => pImGuiTextBuffer_appendf(self, fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* ImGuiTextBuffer_c_str(ImGuiTextBuffer* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImGuiTextBuffer_c_strDelegate(ImGuiTextBuffer* self);
        private static readonly ImGuiTextBuffer_c_strDelegate pImGuiTextBuffer_c_str = lib.LoadFunction<ImGuiTextBuffer_c_strDelegate>("ImGuiTextBuffer_c_str");
        public static byte* ImGuiTextBuffer_c_str(ImGuiTextBuffer* self) => pImGuiTextBuffer_c_str(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiTextBuffer_reserve(ImGuiTextBuffer* self, int capacity);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextBuffer_reserveDelegate(ImGuiTextBuffer* self, int capacity);
        private static readonly ImGuiTextBuffer_reserveDelegate pImGuiTextBuffer_reserve = lib.LoadFunction<ImGuiTextBuffer_reserveDelegate>("ImGuiTextBuffer_reserve");
        public static void ImGuiTextBuffer_reserve(ImGuiTextBuffer* self, int capacity) => pImGuiTextBuffer_reserve(self, capacity);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImGuiTextBuffer_empty(ImGuiTextBuffer* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiTextBuffer_emptyDelegate(ImGuiTextBuffer* self);
        private static readonly ImGuiTextBuffer_emptyDelegate pImGuiTextBuffer_empty = lib.LoadFunction<ImGuiTextBuffer_emptyDelegate>("ImGuiTextBuffer_empty");
        public static byte ImGuiTextBuffer_empty(ImGuiTextBuffer* self) => pImGuiTextBuffer_empty(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderScalar(byte* label, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderScalarDelegate(byte* label, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power);
        private static readonly igSliderScalarDelegate pigSliderScalar = lib.LoadFunction<igSliderScalarDelegate>("igSliderScalar");
        public static byte igSliderScalar(byte* label, ImGuiDataType data_type, void* v, void* v_min, void* v_max, byte* format, float power) => pigSliderScalar(label, data_type, v, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginCombo(byte* label, byte* preview_value, ImGuiComboFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginComboDelegate(byte* label, byte* preview_value, ImGuiComboFlags flags);
        private static readonly igBeginComboDelegate pigBeginCombo = lib.LoadFunction<igBeginComboDelegate>("igBeginCombo");
        public static byte igBeginCombo(byte* label, byte* preview_value, ImGuiComboFlags flags) => pigBeginCombo(label, preview_value, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ImGuiTextBuffer_size(ImGuiTextBuffer* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ImGuiTextBuffer_sizeDelegate(ImGuiTextBuffer* self);
        private static readonly ImGuiTextBuffer_sizeDelegate pImGuiTextBuffer_size = lib.LoadFunction<ImGuiTextBuffer_sizeDelegate>("ImGuiTextBuffer_size");
        public static int ImGuiTextBuffer_size(ImGuiTextBuffer* self) => pImGuiTextBuffer_size(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginMenu(byte* label, byte enabled);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginMenuDelegate(byte* label, byte enabled);
        private static readonly igBeginMenuDelegate pigBeginMenu = lib.LoadFunction<igBeginMenuDelegate>("igBeginMenu");
        public static byte igBeginMenu(byte* label, byte enabled) => pigBeginMenu(label, enabled);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsItemHovered(ImGuiHoveredFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemHoveredDelegate(ImGuiHoveredFlags flags);
        private static readonly igIsItemHoveredDelegate pigIsItemHovered = lib.LoadFunction<igIsItemHoveredDelegate>("igIsItemHovered");
        public static byte igIsItemHovered(ImGuiHoveredFlags flags) => pigIsItemHovered(flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PrimWriteVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimWriteVtxDelegate(ImDrawList* self, Vector2 pos, Vector2 uv, uint col);
        private static readonly ImDrawList_PrimWriteVtxDelegate pImDrawList_PrimWriteVtx = lib.LoadFunction<ImDrawList_PrimWriteVtxDelegate>("ImDrawList_PrimWriteVtx");
        public static void ImDrawList_PrimWriteVtx(ImDrawList* self, Vector2 pos, Vector2 uv, uint col) => pImDrawList_PrimWriteVtx(self, pos, uv, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igBullet();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igBulletDelegate();
        private static readonly igBulletDelegate pigBullet = lib.LoadFunction<igBulletDelegate>("igBullet");
        public static void igBullet() => pigBullet();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputText(byte* label, byte* buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputTextDelegate(byte* label, byte* buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data);
        private static readonly igInputTextDelegate pigInputText = lib.LoadFunction<igInputTextDelegate>("igInputText");
        public static byte igInputText(byte* label, byte* buf, uint buf_size, ImGuiInputTextFlags flags, ImGuiInputTextCallback callback, void* user_data) => pigInputText(label, buf, buf_size, flags, callback, user_data);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputInt3(byte* label, int* v, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputInt3Delegate(byte* label, int* v, ImGuiInputTextFlags extra_flags);
        private static readonly igInputInt3Delegate pigInputInt3 = lib.LoadFunction<igInputInt3Delegate>("igInputInt3");
        public static byte igInputInt3(byte* label, int* v, ImGuiInputTextFlags extra_flags) => pigInputInt3(label, v, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiIO_ImGuiIO(ImGuiIO* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiIO_ImGuiIODelegate(ImGuiIO* self);
        private static readonly ImGuiIO_ImGuiIODelegate pImGuiIO_ImGuiIO = lib.LoadFunction<ImGuiIO_ImGuiIODelegate>("ImGuiIO_ImGuiIO");
        public static void ImGuiIO_ImGuiIO(ImGuiIO* self) => pImGuiIO_ImGuiIO(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igStyleColorsDark(ImGuiStyle* dst);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igStyleColorsDarkDelegate(ImGuiStyle* dst);
        private static readonly igStyleColorsDarkDelegate pigStyleColorsDark = lib.LoadFunction<igStyleColorsDarkDelegate>("igStyleColorsDark");
        public static void igStyleColorsDark(ImGuiStyle* dst) => pigStyleColorsDark(dst);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputInt(byte* label, int* v, int step, int step_fast, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputIntDelegate(byte* label, int* v, int step, int step_fast, ImGuiInputTextFlags extra_flags);
        private static readonly igInputIntDelegate pigInputInt = lib.LoadFunction<igInputIntDelegate>("igInputInt");
        public static byte igInputInt(byte* label, int* v, int step, int step_fast, ImGuiInputTextFlags extra_flags) => pigInputInt(label, v, step, step_fast, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetWindowFontScale(float scale);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowFontScaleDelegate(float scale);
        private static readonly igSetWindowFontScaleDelegate pigSetWindowFontScale = lib.LoadFunction<igSetWindowFontScaleDelegate>("igSetWindowFontScale");
        public static void igSetWindowFontScale(float scale) => pigSetWindowFontScale(scale);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderInt(byte* label, int* v, int v_min, int v_max, byte* format);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderIntDelegate(byte* label, int* v, int v_min, int v_max, byte* format);
        private static readonly igSliderIntDelegate pigSliderInt = lib.LoadFunction<igSliderIntDelegate>("igSliderInt");
        public static byte igSliderInt(byte* label, int* v, int v_min, int v_max, byte* format) => pigSliderInt(label, v, v_min, v_max, format);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* TextRange_end(TextRange* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* TextRange_endDelegate(TextRange* self);
        private static readonly TextRange_endDelegate pTextRange_end = lib.LoadFunction<TextRange_endDelegate>("TextRange_end");
        public static byte* TextRange_end(TextRange* self) => pTextRange_end(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* TextRange_begin(TextRange* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* TextRange_beginDelegate(TextRange* self);
        private static readonly TextRange_beginDelegate pTextRange_begin = lib.LoadFunction<TextRange_beginDelegate>("TextRange_begin");
        public static byte* TextRange_begin(TextRange* self) => pTextRange_begin(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetNextWindowPos(Vector2 pos, ImGuiCond cond, Vector2 pivot);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowPosDelegate(Vector2 pos, ImGuiCond cond, Vector2 pivot);
        private static readonly igSetNextWindowPosDelegate pigSetNextWindowPos = lib.LoadFunction<igSetNextWindowPosDelegate>("igSetNextWindowPos");
        public static void igSetNextWindowPos(Vector2 pos, ImGuiCond cond, Vector2 pivot) => pigSetNextWindowPos(pos, cond, pivot);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragInt3(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragInt3Delegate(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format);
        private static readonly igDragInt3Delegate pigDragInt3 = lib.LoadFunction<igDragInt3Delegate>("igDragInt3");
        public static byte igDragInt3(byte* label, int* v, float v_speed, int v_min, int v_max, byte* format) => pigDragInt3(label, v, v_speed, v_min, v_max, format);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igOpenPopup(byte* str_id);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igOpenPopupDelegate(byte* str_id);
        private static readonly igOpenPopupDelegate pigOpenPopup = lib.LoadFunction<igOpenPopupDelegate>("igOpenPopup");
        public static void igOpenPopup(byte* str_id) => pigOpenPopup(str_id);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TextRange_TextRange(TextRange* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TextRange_TextRangeDelegate(TextRange* self);
        private static readonly TextRange_TextRangeDelegate pTextRange_TextRange = lib.LoadFunction<TextRange_TextRangeDelegate>("TextRange_TextRange");
        public static void TextRange_TextRange(TextRange* self) => pTextRange_TextRange(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TextRange_TextRangeStr(TextRange* self, byte* _b, byte* _e);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void TextRange_TextRangeStrDelegate(TextRange* self, byte* _b, byte* _e);
        private static readonly TextRange_TextRangeStrDelegate pTextRange_TextRangeStr = lib.LoadFunction<TextRange_TextRangeStrDelegate>("TextRange_TextRangeStr");
        public static void TextRange_TextRangeStr(TextRange* self, byte* _b, byte* _e) => pTextRange_TextRangeStr(self, _b, _e);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImDrawList_GetClipRectMax_nonUDT2")]
        public static extern Vector2 ImDrawList_GetClipRectMax(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 ImDrawList_GetClipRectMaxDelegate(ImDrawList* self);
        private static readonly ImDrawList_GetClipRectMaxDelegate pImDrawList_GetClipRectMax = lib.LoadFunction<ImDrawList_GetClipRectMaxDelegate>("ImDrawList_GetClipRectMax");
        public static Vector2 ImDrawList_GetClipRectMax(ImDrawList* self) => pImDrawList_GetClipRectMax(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igCalcTextSize_nonUDT2")]
        public static extern Vector2 igCalcTextSize(byte* text, byte* text_end, byte hide_text_after_double_hash, float wrap_width);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igCalcTextSizeDelegate(byte* text, byte* text_end, byte hide_text_after_double_hash, float wrap_width);
        private static readonly igCalcTextSizeDelegate pigCalcTextSize = lib.LoadFunction<igCalcTextSizeDelegate>("igCalcTextSize");
        public static Vector2 igCalcTextSize(byte* text, byte* text_end, byte hide_text_after_double_hash, float wrap_width) => pigCalcTextSize(text, text_end, hide_text_after_double_hash, wrap_width);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr igGetDrawListSharedData();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr igGetDrawListSharedDataDelegate();
        private static readonly igGetDrawListSharedDataDelegate pigGetDrawListSharedData = lib.LoadFunction<igGetDrawListSharedDataDelegate>("igGetDrawListSharedData");
        public static IntPtr igGetDrawListSharedData() => pigGetDrawListSharedData();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igColumns(int count, byte* id, byte border);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igColumnsDelegate(int count, byte* id, byte border);
        private static readonly igColumnsDelegate pigColumns = lib.LoadFunction<igColumnsDelegate>("igColumns");
        public static void igColumns(int count, byte* id, byte border) => pigColumns(count, id, border);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsItemActive();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsItemActiveDelegate();
        private static readonly igIsItemActiveDelegate pigIsItemActive = lib.LoadFunction<igIsItemActiveDelegate>("igIsItemActive");
        public static byte igIsItemActive() => pigIsItemActive();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiTextFilter_ImGuiTextFilter(ImGuiTextFilter* self, byte* default_filter);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextFilter_ImGuiTextFilterDelegate(ImGuiTextFilter* self, byte* default_filter);
        private static readonly ImGuiTextFilter_ImGuiTextFilterDelegate pImGuiTextFilter_ImGuiTextFilter = lib.LoadFunction<ImGuiTextFilter_ImGuiTextFilterDelegate>("ImGuiTextFilter_ImGuiTextFilter");
        public static void ImGuiTextFilter_ImGuiTextFilter(ImGuiTextFilter* self, byte* default_filter) => pImGuiTextFilter_ImGuiTextFilter(self, default_filter);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiOnceUponAFrame_ImGuiOnceUponAFrame(ImGuiOnceUponAFrame* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiOnceUponAFrame_ImGuiOnceUponAFrameDelegate(ImGuiOnceUponAFrame* self);
        private static readonly ImGuiOnceUponAFrame_ImGuiOnceUponAFrameDelegate pImGuiOnceUponAFrame_ImGuiOnceUponAFrame = lib.LoadFunction<ImGuiOnceUponAFrame_ImGuiOnceUponAFrameDelegate>("ImGuiOnceUponAFrame_ImGuiOnceUponAFrame");
        public static void ImGuiOnceUponAFrame_ImGuiOnceUponAFrame(ImGuiOnceUponAFrame* self) => pImGuiOnceUponAFrame_ImGuiOnceUponAFrame(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginDragDropTarget();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginDragDropTargetDelegate();
        private static readonly igBeginDragDropTargetDelegate pigBeginDragDropTarget = lib.LoadFunction<igBeginDragDropTargetDelegate>("igBeginDragDropTarget");
        public static byte igBeginDragDropTarget() => pigBeginDragDropTarget();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte TextRange_empty(TextRange* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte TextRange_emptyDelegate(TextRange* self);
        private static readonly TextRange_emptyDelegate pTextRange_empty = lib.LoadFunction<TextRange_emptyDelegate>("TextRange_empty");
        public static byte TextRange_empty(TextRange* self) => pTextRange_empty(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImGuiPayload_IsDelivery(ImGuiPayload* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiPayload_IsDeliveryDelegate(ImGuiPayload* self);
        private static readonly ImGuiPayload_IsDeliveryDelegate pImGuiPayload_IsDelivery = lib.LoadFunction<ImGuiPayload_IsDeliveryDelegate>("ImGuiPayload_IsDelivery");
        public static byte ImGuiPayload_IsDelivery(ImGuiPayload* self) => pImGuiPayload_IsDelivery(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiIO_AddInputCharacter(ImGuiIO* self, ushort c);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiIO_AddInputCharacterDelegate(ImGuiIO* self, ushort c);
        private static readonly ImGuiIO_AddInputCharacterDelegate pImGuiIO_AddInputCharacter = lib.LoadFunction<ImGuiIO_AddInputCharacterDelegate>("ImGuiIO_AddInputCharacter");
        public static void ImGuiIO_AddInputCharacter(ImGuiIO* self, ushort c) => pImGuiIO_AddInputCharacter(self, c);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_AddImageRounded(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col, float rounding, int rounding_corners);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_AddImageRoundedDelegate(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col, float rounding, int rounding_corners);
        private static readonly ImDrawList_AddImageRoundedDelegate pImDrawList_AddImageRounded = lib.LoadFunction<ImDrawList_AddImageRoundedDelegate>("ImDrawList_AddImageRounded");
        public static void ImDrawList_AddImageRounded(ImDrawList* self, IntPtr user_texture_id, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col, float rounding, int rounding_corners) => pImDrawList_AddImageRounded(self, user_texture_id, a, b, uv_a, uv_b, col, rounding, rounding_corners);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiStyle_ImGuiStyle(ImGuiStyle* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStyle_ImGuiStyleDelegate(ImGuiStyle* self);
        private static readonly ImGuiStyle_ImGuiStyleDelegate pImGuiStyle_ImGuiStyle = lib.LoadFunction<ImGuiStyle_ImGuiStyleDelegate>("ImGuiStyle_ImGuiStyle");
        public static void ImGuiStyle_ImGuiStyle(ImGuiStyle* self) => pImGuiStyle_ImGuiStyle(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igColorPicker3(byte* label, Vector3* col, ImGuiColorEditFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igColorPicker3Delegate(byte* label, Vector3* col, ImGuiColorEditFlags flags);
        private static readonly igColorPicker3Delegate pigColorPicker3 = lib.LoadFunction<igColorPicker3Delegate>("igColorPicker3");
        public static byte igColorPicker3(byte* label, Vector3* col, ImGuiColorEditFlags flags) => pigColorPicker3(label, col, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetContentRegionMax_nonUDT2")]
        public static extern Vector2 igGetContentRegionMax();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetContentRegionMaxDelegate();
        private static readonly igGetContentRegionMaxDelegate pigGetContentRegionMax = lib.LoadFunction<igGetContentRegionMaxDelegate>("igGetContentRegionMax");
        public static Vector2 igGetContentRegionMax() => pigGetContentRegionMax();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginChildFrameDelegate(uint id, Vector2 size, ImGuiWindowFlags flags);
        private static readonly igBeginChildFrameDelegate pigBeginChildFrame = lib.LoadFunction<igBeginChildFrameDelegate>("igBeginChildFrame");
        public static byte igBeginChildFrame(uint id, Vector2 size, ImGuiWindowFlags flags) => pigBeginChildFrame(id, size, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSaveIniSettingsToDisk(byte* ini_filename);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSaveIniSettingsToDiskDelegate(byte* ini_filename);
        private static readonly igSaveIniSettingsToDiskDelegate pigSaveIniSettingsToDisk = lib.LoadFunction<igSaveIniSettingsToDiskDelegate>("igSaveIniSettingsToDisk");
        public static void igSaveIniSettingsToDisk(byte* ini_filename) => pigSaveIniSettingsToDisk(ini_filename);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFont_ClearOutputData(ImFont* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFont_ClearOutputDataDelegate(ImFont* self);
        private static readonly ImFont_ClearOutputDataDelegate pImFont_ClearOutputData = lib.LoadFunction<ImFont_ClearOutputDataDelegate>("ImFont_ClearOutputData");
        public static void ImFont_ClearOutputData(ImFont* self) => pImFont_ClearOutputData(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* igGetClipboardText();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* igGetClipboardTextDelegate();
        private static readonly igGetClipboardTextDelegate pigGetClipboardText = lib.LoadFunction<igGetClipboardTextDelegate>("igGetClipboardText");
        public static byte* igGetClipboardText() => pigGetClipboardText();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PrimQuadUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimQuadUVDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col);
        private static readonly ImDrawList_PrimQuadUVDelegate pImDrawList_PrimQuadUV = lib.LoadFunction<ImDrawList_PrimQuadUVDelegate>("ImDrawList_PrimQuadUV");
        public static void ImDrawList_PrimQuadUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Vector2 uv_a, Vector2 uv_b, Vector2 uv_c, Vector2 uv_d, uint col) => pImDrawList_PrimQuadUV(self, a, b, c, d, uv_a, uv_b, uv_c, uv_d, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndDragDropTarget();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndDragDropTargetDelegate();
        private static readonly igEndDragDropTargetDelegate pigEndDragDropTarget = lib.LoadFunction<igEndDragDropTargetDelegate>("igEndDragDropTarget");
        public static void igEndDragDropTarget() => pigEndDragDropTarget();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ushort* ImFontAtlas_GetGlyphRangesKorean(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ushort* ImFontAtlas_GetGlyphRangesKoreanDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_GetGlyphRangesKoreanDelegate pImFontAtlas_GetGlyphRangesKorean = lib.LoadFunction<ImFontAtlas_GetGlyphRangesKoreanDelegate>("ImFontAtlas_GetGlyphRangesKorean");
        public static ushort* ImFontAtlas_GetGlyphRangesKorean(ImFontAtlas* self) => pImFontAtlas_GetGlyphRangesKorean(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern int igGetKeyPressedAmount(int key_index, float repeat_delay, float rate);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int igGetKeyPressedAmountDelegate(int key_index, float repeat_delay, float rate);
        private static readonly igGetKeyPressedAmountDelegate pigGetKeyPressedAmount = lib.LoadFunction<igGetKeyPressedAmountDelegate>("igGetKeyPressedAmount");
        public static int igGetKeyPressedAmount(int key_index, float repeat_delay, float rate) => pigGetKeyPressedAmount(key_index, repeat_delay, rate);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFontAtlas_GetTexDataAsRGBA32(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_GetTexDataAsRGBA32Delegate(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel);
        private static readonly ImFontAtlas_GetTexDataAsRGBA32Delegate pImFontAtlas_GetTexDataAsRGBA32 = lib.LoadFunction<ImFontAtlas_GetTexDataAsRGBA32Delegate>("ImFontAtlas_GetTexDataAsRGBA32");
        public static void ImFontAtlas_GetTexDataAsRGBA32(ImFontAtlas* self, byte** out_pixels, int* out_width, int* out_height, int* out_bytes_per_pixel) => pImFontAtlas_GetTexDataAsRGBA32(self, out_pixels, out_width, out_height, out_bytes_per_pixel);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igNewFrame();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igNewFrameDelegate();
        private static readonly igNewFrameDelegate pigNewFrame = lib.LoadFunction<igNewFrameDelegate>("igNewFrame");
        public static void igNewFrame() => pigNewFrame();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igResetMouseDragDelta(int button);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igResetMouseDragDeltaDelegate(int button);
        private static readonly igResetMouseDragDeltaDelegate pigResetMouseDragDelta = lib.LoadFunction<igResetMouseDragDeltaDelegate>("igResetMouseDragDelta");
        public static void igResetMouseDragDelta(int button) => pigResetMouseDragDelta(button);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetTreeNodeToLabelSpacing();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetTreeNodeToLabelSpacingDelegate();
        private static readonly igGetTreeNodeToLabelSpacingDelegate pigGetTreeNodeToLabelSpacing = lib.LoadFunction<igGetTreeNodeToLabelSpacingDelegate>("igGetTreeNodeToLabelSpacing");
        public static float igGetTreeNodeToLabelSpacing() => pigGetTreeNodeToLabelSpacing();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetMousePos_nonUDT2")]
        public static extern Vector2 igGetMousePos();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetMousePosDelegate();
        private static readonly igGetMousePosDelegate pigGetMousePos = lib.LoadFunction<igGetMousePosDelegate>("igGetMousePos");
        public static Vector2 igGetMousePos() => pigGetMousePos();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void GlyphRangesBuilder_AddChar(GlyphRangesBuilder* self, ushort c);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void GlyphRangesBuilder_AddCharDelegate(GlyphRangesBuilder* self, ushort c);
        private static readonly GlyphRangesBuilder_AddCharDelegate pGlyphRangesBuilder_AddChar = lib.LoadFunction<GlyphRangesBuilder_AddCharDelegate>("GlyphRangesBuilder_AddChar");
        public static void GlyphRangesBuilder_AddChar(GlyphRangesBuilder* self, ushort c) => pGlyphRangesBuilder_AddChar(self, c);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPopID();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopIDDelegate();
        private static readonly igPopIDDelegate pigPopID = lib.LoadFunction<igPopIDDelegate>("igPopID");
        public static void igPopID() => pigPopID();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsMouseDoubleClicked(int button);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseDoubleClickedDelegate(int button);
        private static readonly igIsMouseDoubleClickedDelegate pigIsMouseDoubleClicked = lib.LoadFunction<igIsMouseDoubleClickedDelegate>("igIsMouseDoubleClicked");
        public static byte igIsMouseDoubleClicked(int button) => pigIsMouseDoubleClicked(button);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igStyleColorsClassic(ImGuiStyle* dst);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igStyleColorsClassicDelegate(ImGuiStyle* dst);
        private static readonly igStyleColorsClassicDelegate pigStyleColorsClassic = lib.LoadFunction<igStyleColorsClassicDelegate>("igStyleColorsClassic");
        public static void igStyleColorsClassic(ImGuiStyle* dst) => pigStyleColorsClassic(dst);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImGuiTextFilter_IsActive(ImGuiTextFilter* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiTextFilter_IsActiveDelegate(ImGuiTextFilter* self);
        private static readonly ImGuiTextFilter_IsActiveDelegate pImGuiTextFilter_IsActive = lib.LoadFunction<ImGuiTextFilter_IsActiveDelegate>("ImGuiTextFilter_IsActive");
        public static byte ImGuiTextFilter_IsActive(ImGuiTextFilter* self) => pImGuiTextFilter_IsActive(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PathClear(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathClearDelegate(ImDrawList* self);
        private static readonly ImDrawList_PathClearDelegate pImDrawList_PathClear = lib.LoadFunction<ImDrawList_PathClearDelegate>("ImDrawList_PathClear");
        public static void ImDrawList_PathClear(ImDrawList* self) => pImDrawList_PathClear(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetWindowFocus();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowFocusDelegate();
        private static readonly igSetWindowFocusDelegate pigSetWindowFocus = lib.LoadFunction<igSetWindowFocusDelegate>("igSetWindowFocus");
        public static void igSetWindowFocus() => pigSetWindowFocus();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetWindowFocusStr(byte* name);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowFocusStrDelegate(byte* name);
        private static readonly igSetWindowFocusStrDelegate pigSetWindowFocusStr = lib.LoadFunction<igSetWindowFocusStrDelegate>("igSetWindowFocusStr");
        public static void igSetWindowFocusStr(byte* name) => pigSetWindowFocusStr(name);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igColorConvertHSVtoRGB(float h, float s, float v, float* out_r, float* out_g, float* out_b);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igColorConvertHSVtoRGBDelegate(float h, float s, float v, float* out_r, float* out_g, float* out_b);
        private static readonly igColorConvertHSVtoRGBDelegate pigColorConvertHSVtoRGB = lib.LoadFunction<igColorConvertHSVtoRGBDelegate>("igColorConvertHSVtoRGB");
        public static void igColorConvertHSVtoRGB(float h, float s, float v, float* out_r, float* out_g, float* out_b) => pigColorConvertHSVtoRGB(h, s, v, out_r, out_g, out_b);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImColor_ImColor(ImColor* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_ImColorDelegate(ImColor* self);
        private static readonly ImColor_ImColorDelegate pImColor_ImColor = lib.LoadFunction<ImColor_ImColorDelegate>("ImColor_ImColor");
        public static void ImColor_ImColor(ImColor* self) => pImColor_ImColor(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImColor_ImColorInt(ImColor* self, int r, int g, int b, int a);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_ImColorIntDelegate(ImColor* self, int r, int g, int b, int a);
        private static readonly ImColor_ImColorIntDelegate pImColor_ImColorInt = lib.LoadFunction<ImColor_ImColorIntDelegate>("ImColor_ImColorInt");
        public static void ImColor_ImColorInt(ImColor* self, int r, int g, int b, int a) => pImColor_ImColorInt(self, r, g, b, a);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImColor_ImColorU32(ImColor* self, uint rgba);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_ImColorU32Delegate(ImColor* self, uint rgba);
        private static readonly ImColor_ImColorU32Delegate pImColor_ImColorU32 = lib.LoadFunction<ImColor_ImColorU32Delegate>("ImColor_ImColorU32");
        public static void ImColor_ImColorU32(ImColor* self, uint rgba) => pImColor_ImColorU32(self, rgba);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImColor_ImColorFloat(ImColor* self, float r, float g, float b, float a);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_ImColorFloatDelegate(ImColor* self, float r, float g, float b, float a);
        private static readonly ImColor_ImColorFloatDelegate pImColor_ImColorFloat = lib.LoadFunction<ImColor_ImColorFloatDelegate>("ImColor_ImColorFloat");
        public static void ImColor_ImColorFloat(ImColor* self, float r, float g, float b, float a) => pImColor_ImColorFloat(self, r, g, b, a);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImColor_ImColorVec4(ImColor* self, Vector4 col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImColor_ImColorVec4Delegate(ImColor* self, Vector4 col);
        private static readonly ImColor_ImColorVec4Delegate pImColor_ImColorVec4 = lib.LoadFunction<ImColor_ImColorVec4Delegate>("ImColor_ImColorVec4");
        public static void ImColor_ImColorVec4(ImColor* self, Vector4 col) => pImColor_ImColorVec4(self, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igVSliderFloat(byte* label, Vector2 size, float* v, float v_min, float v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igVSliderFloatDelegate(byte* label, Vector2 size, float* v, float v_min, float v_max, byte* format, float power);
        private static readonly igVSliderFloatDelegate pigVSliderFloat = lib.LoadFunction<igVSliderFloatDelegate>("igVSliderFloat");
        public static byte igVSliderFloat(byte* label, Vector2 size, float* v, float v_min, float v_max, byte* format, float power) => pigVSliderFloat(label, size, v, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igColorConvertU32ToFloat4_nonUDT2")]
        public static extern Vector4 igColorConvertU32ToFloat4(uint @in);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector4 igColorConvertU32ToFloat4Delegate(uint @in);
        private static readonly igColorConvertU32ToFloat4Delegate pigColorConvertU32ToFloat4 = lib.LoadFunction<igColorConvertU32ToFloat4Delegate>("igColorConvertU32ToFloat4");
        public static Vector4 igColorConvertU32ToFloat4(uint @in) => pigColorConvertU32ToFloat4(@in);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPopTextWrapPos();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopTextWrapPosDelegate();
        private static readonly igPopTextWrapPosDelegate pigPopTextWrapPos = lib.LoadFunction<igPopTextWrapPosDelegate>("igPopTextWrapPos");
        public static void igPopTextWrapPos() => pigPopTextWrapPos();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiTextFilter_Clear(ImGuiTextFilter* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextFilter_ClearDelegate(ImGuiTextFilter* self);
        private static readonly ImGuiTextFilter_ClearDelegate pImGuiTextFilter_Clear = lib.LoadFunction<ImGuiTextFilter_ClearDelegate>("ImGuiTextFilter_Clear");
        public static void ImGuiTextFilter_Clear(ImGuiTextFilter* self) => pImGuiTextFilter_Clear(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiStorage* igGetStateStorage();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImGuiStorage* igGetStateStorageDelegate();
        private static readonly igGetStateStorageDelegate pigGetStateStorage = lib.LoadFunction<igGetStateStorageDelegate>("igGetStateStorage");
        public static ImGuiStorage* igGetStateStorage() => pigGetStateStorage();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetColumnWidth(int column_index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetColumnWidthDelegate(int column_index);
        private static readonly igGetColumnWidthDelegate pigGetColumnWidth = lib.LoadFunction<igGetColumnWidthDelegate>("igGetColumnWidth");
        public static float igGetColumnWidth(int column_index) => pigGetColumnWidth(column_index);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndMenuBar();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndMenuBarDelegate();
        private static readonly igEndMenuBarDelegate pigEndMenuBar = lib.LoadFunction<igEndMenuBarDelegate>("igEndMenuBar");
        public static void igEndMenuBar() => pigEndMenuBar();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetStateStorage(ImGuiStorage* storage);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetStateStorageDelegate(ImGuiStorage* storage);
        private static readonly igSetStateStorageDelegate pigSetStateStorage = lib.LoadFunction<igSetStateStorageDelegate>("igSetStateStorage");
        public static void igSetStateStorage(ImGuiStorage* storage) => pigSetStateStorage(storage);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* igGetStyleColorName(ImGuiCol idx);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* igGetStyleColorNameDelegate(ImGuiCol idx);
        private static readonly igGetStyleColorNameDelegate pigGetStyleColorName = lib.LoadFunction<igGetStyleColorNameDelegate>("igGetStyleColorName");
        public static byte* igGetStyleColorName(ImGuiCol idx) => pigGetStyleColorName(idx);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsMouseDragging(int button, float lock_threshold);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseDraggingDelegate(int button, float lock_threshold);
        private static readonly igIsMouseDraggingDelegate pigIsMouseDragging = lib.LoadFunction<igIsMouseDraggingDelegate>("igIsMouseDragging");
        public static byte igIsMouseDragging(int button, float lock_threshold) => pigIsMouseDragging(button, lock_threshold);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PrimWriteIdx(ImDrawList* self, ushort idx);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimWriteIdxDelegate(ImDrawList* self, ushort idx);
        private static readonly ImDrawList_PrimWriteIdxDelegate pImDrawList_PrimWriteIdx = lib.LoadFunction<ImDrawList_PrimWriteIdxDelegate>("ImDrawList_PrimWriteIdx");
        public static void ImDrawList_PrimWriteIdx(ImDrawList* self, ushort idx) => pImDrawList_PrimWriteIdx(self, idx);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiStyle_ScaleAllSizes(ImGuiStyle* self, float scale_factor);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiStyle_ScaleAllSizesDelegate(ImGuiStyle* self, float scale_factor);
        private static readonly ImGuiStyle_ScaleAllSizesDelegate pImGuiStyle_ScaleAllSizes = lib.LoadFunction<ImGuiStyle_ScaleAllSizesDelegate>("ImGuiStyle_ScaleAllSizes");
        public static void ImGuiStyle_ScaleAllSizes(ImGuiStyle* self, float scale_factor) => pImGuiStyle_ScaleAllSizes(self, scale_factor);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushStyleColorU32(ImGuiCol idx, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushStyleColorU32Delegate(ImGuiCol idx, uint col);
        private static readonly igPushStyleColorU32Delegate pigPushStyleColorU32 = lib.LoadFunction<igPushStyleColorU32Delegate>("igPushStyleColorU32");
        public static void igPushStyleColorU32(ImGuiCol idx, uint col) => pigPushStyleColorU32(idx, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushStyleColor(ImGuiCol idx, Vector4 col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushStyleColorDelegate(ImGuiCol idx, Vector4 col);
        private static readonly igPushStyleColorDelegate pigPushStyleColor = lib.LoadFunction<igPushStyleColorDelegate>("igPushStyleColor");
        public static void igPushStyleColor(ImGuiCol idx, Vector4 col) => pigPushStyleColor(idx, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void* igMemAlloc(uint size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void* igMemAllocDelegate(uint size);
        private static readonly igMemAllocDelegate pigMemAlloc = lib.LoadFunction<igMemAllocDelegate>("igMemAlloc");
        public static void* igMemAlloc(uint size) => pigMemAlloc(size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetCurrentContext(IntPtr ctx);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetCurrentContextDelegate(IntPtr ctx);
        private static readonly igSetCurrentContextDelegate pigSetCurrentContext = lib.LoadFunction<igSetCurrentContextDelegate>("igSetCurrentContext");
        public static void igSetCurrentContext(IntPtr ctx) => pigSetCurrentContext(ctx);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushItemWidth(float item_width);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushItemWidthDelegate(float item_width);
        private static readonly igPushItemWidthDelegate pigPushItemWidth = lib.LoadFunction<igPushItemWidthDelegate>("igPushItemWidth");
        public static void igPushItemWidth(float item_width) => pigPushItemWidth(item_width);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsWindowAppearing();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsWindowAppearingDelegate();
        private static readonly igIsWindowAppearingDelegate pigIsWindowAppearing = lib.LoadFunction<igIsWindowAppearingDelegate>("igIsWindowAppearing");
        public static byte igIsWindowAppearing() => pigIsWindowAppearing();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiStyle* igGetStyle();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImGuiStyle* igGetStyleDelegate();
        private static readonly igGetStyleDelegate pigGetStyle = lib.LoadFunction<igGetStyleDelegate>("igGetStyle");
        public static ImGuiStyle* igGetStyle() => pigGetStyle();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetItemAllowOverlap();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetItemAllowOverlapDelegate();
        private static readonly igSetItemAllowOverlapDelegate pigSetItemAllowOverlap = lib.LoadFunction<igSetItemAllowOverlapDelegate>("igSetItemAllowOverlap");
        public static void igSetItemAllowOverlap() => pigSetItemAllowOverlap();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndChild();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndChildDelegate();
        private static readonly igEndChildDelegate pigEndChild = lib.LoadFunction<igEndChildDelegate>("igEndChild");
        public static void igEndChild() => pigEndChild();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igCollapsingHeader(byte* label, ImGuiTreeNodeFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igCollapsingHeaderDelegate(byte* label, ImGuiTreeNodeFlags flags);
        private static readonly igCollapsingHeaderDelegate pigCollapsingHeader = lib.LoadFunction<igCollapsingHeaderDelegate>("igCollapsingHeader");
        public static byte igCollapsingHeader(byte* label, ImGuiTreeNodeFlags flags) => pigCollapsingHeader(label, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igCollapsingHeaderBoolPtr(byte* label, byte* p_open, ImGuiTreeNodeFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igCollapsingHeaderBoolPtrDelegate(byte* label, byte* p_open, ImGuiTreeNodeFlags flags);
        private static readonly igCollapsingHeaderBoolPtrDelegate pigCollapsingHeaderBoolPtr = lib.LoadFunction<igCollapsingHeaderBoolPtrDelegate>("igCollapsingHeaderBoolPtr");
        public static byte igCollapsingHeaderBoolPtr(byte* label, byte* p_open, ImGuiTreeNodeFlags flags) => pigCollapsingHeaderBoolPtr(label, p_open, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragFloatRange2(byte* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, byte* format, byte* format_max, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragFloatRange2Delegate(byte* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, byte* format, byte* format_max, float power);
        private static readonly igDragFloatRange2Delegate pigDragFloatRange2 = lib.LoadFunction<igDragFloatRange2Delegate>("igDragFloatRange2");
        public static byte igDragFloatRange2(byte* label, float* v_current_min, float* v_current_max, float v_speed, float v_min, float v_max, byte* format, byte* format_max, float power) => pigDragFloatRange2(label, v_current_min, v_current_max, v_speed, v_min, v_max, format, format_max, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetMouseCursor(ImGuiMouseCursor type);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetMouseCursorDelegate(ImGuiMouseCursor type);
        private static readonly igSetMouseCursorDelegate pigSetMouseCursor = lib.LoadFunction<igSetMouseCursorDelegate>("igSetMouseCursor");
        public static void igSetMouseCursor(ImGuiMouseCursor type) => pigSetMouseCursor(type);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetWindowContentRegionMax_nonUDT2")]
        public static extern Vector2 igGetWindowContentRegionMax();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetWindowContentRegionMaxDelegate();
        private static readonly igGetWindowContentRegionMaxDelegate pigGetWindowContentRegionMax = lib.LoadFunction<igGetWindowContentRegionMaxDelegate>("igGetWindowContentRegionMax");
        public static Vector2 igGetWindowContentRegionMax() => pigGetWindowContentRegionMax();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputScalar(byte* label, ImGuiDataType data_type, void* v, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputScalarDelegate(byte* label, ImGuiDataType data_type, void* v, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        private static readonly igInputScalarDelegate pigInputScalar = lib.LoadFunction<igInputScalarDelegate>("igInputScalar");
        public static byte igInputScalar(byte* label, ImGuiDataType data_type, void* v, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags) => pigInputScalar(label, data_type, v, step, step_fast, format, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PushClipRectFullScreen(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PushClipRectFullScreenDelegate(ImDrawList* self);
        private static readonly ImDrawList_PushClipRectFullScreenDelegate pImDrawList_PushClipRectFullScreen = lib.LoadFunction<ImDrawList_PushClipRectFullScreenDelegate>("ImDrawList_PushClipRectFullScreen");
        public static void ImDrawList_PushClipRectFullScreen(ImDrawList* self) => pImDrawList_PushClipRectFullScreen(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint igGetColorU32(ImGuiCol idx, float alpha_mul);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetColorU32Delegate(ImGuiCol idx, float alpha_mul);
        private static readonly igGetColorU32Delegate pigGetColorU32 = lib.LoadFunction<igGetColorU32Delegate>("igGetColorU32");
        public static uint igGetColorU32(ImGuiCol idx, float alpha_mul) => pigGetColorU32(idx, alpha_mul);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint igGetColorU32Vec4(Vector4 col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetColorU32Vec4Delegate(Vector4 col);
        private static readonly igGetColorU32Vec4Delegate pigGetColorU32Vec4 = lib.LoadFunction<igGetColorU32Vec4Delegate>("igGetColorU32Vec4");
        public static uint igGetColorU32Vec4(Vector4 col) => pigGetColorU32Vec4(col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern uint igGetColorU32U32(uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint igGetColorU32U32Delegate(uint col);
        private static readonly igGetColorU32U32Delegate pigGetColorU32U32 = lib.LoadFunction<igGetColorU32U32Delegate>("igGetColorU32U32");
        public static uint igGetColorU32U32(uint col) => pigGetColorU32U32(col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern double igGetTime();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate double igGetTimeDelegate();
        private static readonly igGetTimeDelegate pigGetTime = lib.LoadFunction<igGetTimeDelegate>("igGetTime");
        public static double igGetTime() => pigGetTime();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_ChannelsMerge(ImDrawList* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_ChannelsMergeDelegate(ImDrawList* self);
        private static readonly ImDrawList_ChannelsMergeDelegate pImDrawList_ChannelsMerge = lib.LoadFunction<ImDrawList_ChannelsMergeDelegate>("ImDrawList_ChannelsMerge");
        public static void ImDrawList_ChannelsMerge(ImDrawList* self) => pImDrawList_ChannelsMerge(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern int igGetColumnIndex();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int igGetColumnIndexDelegate();
        private static readonly igGetColumnIndexDelegate pigGetColumnIndex = lib.LoadFunction<igGetColumnIndexDelegate>("igGetColumnIndex");
        public static int igGetColumnIndex() => pigGetColumnIndex();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginPopupContextItem(byte* str_id, int mouse_button);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginPopupContextItemDelegate(byte* str_id, int mouse_button);
        private static readonly igBeginPopupContextItemDelegate pigBeginPopupContextItem = lib.LoadFunction<igBeginPopupContextItemDelegate>("igBeginPopupContextItem");
        public static byte igBeginPopupContextItem(byte* str_id, int mouse_button) => pigBeginPopupContextItem(str_id, mouse_button);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetCursorPosX(float x);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetCursorPosXDelegate(float x);
        private static readonly igSetCursorPosXDelegate pigSetCursorPosX = lib.LoadFunction<igSetCursorPosXDelegate>("igSetCursorPosX");
        public static void igSetCursorPosX(float x) => pigSetCursorPosX(x);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetItemRectSize_nonUDT2")]
        public static extern Vector2 igGetItemRectSize();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetItemRectSizeDelegate();
        private static readonly igGetItemRectSizeDelegate pigGetItemRectSize = lib.LoadFunction<igGetItemRectSizeDelegate>("igGetItemRectSize");
        public static Vector2 igGetItemRectSize() => pigGetItemRectSize();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igArrowButton(byte* str_id, ImGuiDir dir);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igArrowButtonDelegate(byte* str_id, ImGuiDir dir);
        private static readonly igArrowButtonDelegate pigArrowButton = lib.LoadFunction<igArrowButtonDelegate>("igArrowButton");
        public static byte igArrowButton(byte* str_id, ImGuiDir dir) => pigArrowButton(str_id, dir);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern ImGuiMouseCursor igGetMouseCursor();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImGuiMouseCursor igGetMouseCursorDelegate();
        private static readonly igGetMouseCursorDelegate pigGetMouseCursor = lib.LoadFunction<igGetMouseCursorDelegate>("igGetMouseCursor");
        public static ImGuiMouseCursor igGetMouseCursor() => pigGetMouseCursor();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushAllowKeyboardFocus(byte allow_keyboard_focus);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushAllowKeyboardFocusDelegate(byte allow_keyboard_focus);
        private static readonly igPushAllowKeyboardFocusDelegate pigPushAllowKeyboardFocus = lib.LoadFunction<igPushAllowKeyboardFocusDelegate>("igPushAllowKeyboardFocus");
        public static void igPushAllowKeyboardFocus(byte allow_keyboard_focus) => pigPushAllowKeyboardFocus(allow_keyboard_focus);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetScrollY();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetScrollYDelegate();
        private static readonly igGetScrollYDelegate pigGetScrollY = lib.LoadFunction<igGetScrollYDelegate>("igGetScrollY");
        public static float igGetScrollY() => pigGetScrollY();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetColumnOffset(int column_index, float offset_x);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetColumnOffsetDelegate(int column_index, float offset_x);
        private static readonly igSetColumnOffsetDelegate pigSetColumnOffset = lib.LoadFunction<igSetColumnOffsetDelegate>("igSetColumnOffset");
        public static void igSetColumnOffset(int column_index, float offset_x) => pigSetColumnOffset(column_index, offset_x);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* ImGuiTextBuffer_begin(ImGuiTextBuffer* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImGuiTextBuffer_beginDelegate(ImGuiTextBuffer* self);
        private static readonly ImGuiTextBuffer_beginDelegate pImGuiTextBuffer_begin = lib.LoadFunction<ImGuiTextBuffer_beginDelegate>("ImGuiTextBuffer_begin");
        public static byte* ImGuiTextBuffer_begin(ImGuiTextBuffer* self) => pImGuiTextBuffer_begin(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetWindowPosVec2(Vector2 pos, ImGuiCond cond);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowPosVec2Delegate(Vector2 pos, ImGuiCond cond);
        private static readonly igSetWindowPosVec2Delegate pigSetWindowPosVec2 = lib.LoadFunction<igSetWindowPosVec2Delegate>("igSetWindowPosVec2");
        public static void igSetWindowPosVec2(Vector2 pos, ImGuiCond cond) => pigSetWindowPosVec2(pos, cond);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetWindowPosStr(byte* name, Vector2 pos, ImGuiCond cond);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetWindowPosStrDelegate(byte* name, Vector2 pos, ImGuiCond cond);
        private static readonly igSetWindowPosStrDelegate pigSetWindowPosStr = lib.LoadFunction<igSetWindowPosStrDelegate>("igSetWindowPosStr");
        public static void igSetWindowPosStr(byte* name, Vector2 pos, ImGuiCond cond) => pigSetWindowPosStr(name, pos, cond);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetKeyboardFocusHere(int offset);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetKeyboardFocusHereDelegate(int offset);
        private static readonly igSetKeyboardFocusHereDelegate pigSetKeyboardFocusHere = lib.LoadFunction<igSetKeyboardFocusHereDelegate>("igSetKeyboardFocusHere");
        public static void igSetKeyboardFocusHere(int offset) => pigSetKeyboardFocusHere(offset);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetCursorPosY();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetCursorPosYDelegate();
        private static readonly igGetCursorPosYDelegate pigGetCursorPosY = lib.LoadFunction<igGetCursorPosYDelegate>("igGetCursorPosY");
        public static float igGetCursorPosY() => pigGetCursorPosY();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ImFontAtlas_AddCustomRectFontGlyph(ImFontAtlas* self, ImFont* font, ushort id, int width, int height, float advance_x, Vector2 offset);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ImFontAtlas_AddCustomRectFontGlyphDelegate(ImFontAtlas* self, ImFont* font, ushort id, int width, int height, float advance_x, Vector2 offset);
        private static readonly ImFontAtlas_AddCustomRectFontGlyphDelegate pImFontAtlas_AddCustomRectFontGlyph = lib.LoadFunction<ImFontAtlas_AddCustomRectFontGlyphDelegate>("ImFontAtlas_AddCustomRectFontGlyph");
        public static int ImFontAtlas_AddCustomRectFontGlyph(ImFontAtlas* self, ImFont* font, ushort id, int width, int height, float advance_x, Vector2 offset) => pImFontAtlas_AddCustomRectFontGlyph(self, font, id, width, height, advance_x, offset);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEndMainMenuBar();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndMainMenuBarDelegate();
        private static readonly igEndMainMenuBarDelegate pigEndMainMenuBar = lib.LoadFunction<igEndMainMenuBarDelegate>("igEndMainMenuBar");
        public static void igEndMainMenuBar() => pigEndMainMenuBar();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetContentRegionAvailWidth();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetContentRegionAvailWidthDelegate();
        private static readonly igGetContentRegionAvailWidthDelegate pigGetContentRegionAvailWidth = lib.LoadFunction<igGetContentRegionAvailWidthDelegate>("igGetContentRegionAvailWidth");
        public static float igGetContentRegionAvailWidth() => pigGetContentRegionAvailWidth();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsKeyDown(int user_key_index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsKeyDownDelegate(int user_key_index);
        private static readonly igIsKeyDownDelegate pigIsKeyDown = lib.LoadFunction<igIsKeyDownDelegate>("igIsKeyDown");
        public static byte igIsKeyDown(int user_key_index) => pigIsKeyDown(user_key_index);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsMouseDown(int button);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsMouseDownDelegate(int button);
        private static readonly igIsMouseDownDelegate pigIsMouseDown = lib.LoadFunction<igIsMouseDownDelegate>("igIsMouseDown");
        public static byte igIsMouseDown(int button) => pigIsMouseDown(button);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetWindowContentRegionMin_nonUDT2")]
        public static extern Vector2 igGetWindowContentRegionMin();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetWindowContentRegionMinDelegate();
        private static readonly igGetWindowContentRegionMinDelegate pigGetWindowContentRegionMin = lib.LoadFunction<igGetWindowContentRegionMinDelegate>("igGetWindowContentRegionMin");
        public static Vector2 igGetWindowContentRegionMin() => pigGetWindowContentRegionMin();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igLogButtons();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogButtonsDelegate();
        private static readonly igLogButtonsDelegate pigLogButtons = lib.LoadFunction<igLogButtonsDelegate>("igLogButtons");
        public static void igLogButtons() => pigLogButtons();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetWindowContentRegionWidth();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetWindowContentRegionWidthDelegate();
        private static readonly igGetWindowContentRegionWidthDelegate pigGetWindowContentRegionWidth = lib.LoadFunction<igGetWindowContentRegionWidthDelegate>("igGetWindowContentRegionWidth");
        public static float igGetWindowContentRegionWidth() => pigGetWindowContentRegionWidth();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderAngle(byte* label, float* v_rad, float v_degrees_min, float v_degrees_max);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderAngleDelegate(byte* label, float* v_rad, float v_degrees_min, float v_degrees_max);
        private static readonly igSliderAngleDelegate pigSliderAngle = lib.LoadFunction<igSliderAngleDelegate>("igSliderAngle");
        public static byte igSliderAngle(byte* label, float* v_rad, float v_degrees_min, float v_degrees_max) => pigSliderAngle(label, v_rad, v_degrees_min, v_degrees_max);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igTreeNodeExStr(byte* label, ImGuiTreeNodeFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodeExStrDelegate(byte* label, ImGuiTreeNodeFlags flags);
        private static readonly igTreeNodeExStrDelegate pigTreeNodeExStr = lib.LoadFunction<igTreeNodeExStrDelegate>("igTreeNodeExStr");
        public static byte igTreeNodeExStr(byte* label, ImGuiTreeNodeFlags flags) => pigTreeNodeExStr(label, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igTreeNodeExStrStr(byte* str_id, ImGuiTreeNodeFlags flags, byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodeExStrStrDelegate(byte* str_id, ImGuiTreeNodeFlags flags, byte* fmt);
        private static readonly igTreeNodeExStrStrDelegate pigTreeNodeExStrStr = lib.LoadFunction<igTreeNodeExStrStrDelegate>("igTreeNodeExStrStr");
        public static byte igTreeNodeExStrStr(byte* str_id, ImGuiTreeNodeFlags flags, byte* fmt) => pigTreeNodeExStrStr(str_id, flags, fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igTreeNodeExPtr(void* ptr_id, ImGuiTreeNodeFlags flags, byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodeExPtrDelegate(void* ptr_id, ImGuiTreeNodeFlags flags, byte* fmt);
        private static readonly igTreeNodeExPtrDelegate pigTreeNodeExPtr = lib.LoadFunction<igTreeNodeExPtrDelegate>("igTreeNodeExPtr");
        public static byte igTreeNodeExPtr(void* ptr_id, ImGuiTreeNodeFlags flags, byte* fmt) => pigTreeNodeExPtr(ptr_id, flags, fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetWindowWidth();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetWindowWidthDelegate();
        private static readonly igGetWindowWidthDelegate pigGetWindowWidth = lib.LoadFunction<igGetWindowWidthDelegate>("igGetWindowWidth");
        public static float igGetWindowWidth() => pigGetWindowWidth();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushTextWrapPos(float wrap_pos_x);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushTextWrapPosDelegate(float wrap_pos_x);
        private static readonly igPushTextWrapPosDelegate pigPushTextWrapPos = lib.LoadFunction<igPushTextWrapPosDelegate>("igPushTextWrapPos");
        public static void igPushTextWrapPos(float wrap_pos_x) => pigPushTextWrapPos(wrap_pos_x);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern int ImGuiStorage_GetInt(ImGuiStorage* self, uint key, int default_val);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int ImGuiStorage_GetIntDelegate(ImGuiStorage* self, uint key, int default_val);
        private static readonly ImGuiStorage_GetIntDelegate pImGuiStorage_GetInt = lib.LoadFunction<ImGuiStorage_GetIntDelegate>("ImGuiStorage_GetInt");
        public static int ImGuiStorage_GetInt(ImGuiStorage* self, uint key, int default_val) => pImGuiStorage_GetInt(self, key, default_val);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderInt3(byte* label, int* v, int v_min, int v_max, byte* format);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderInt3Delegate(byte* label, int* v, int v_min, int v_max, byte* format);
        private static readonly igSliderInt3Delegate pigSliderInt3 = lib.LoadFunction<igSliderInt3Delegate>("igSliderInt3");
        public static byte igSliderInt3(byte* label, int* v, int v_min, int v_max, byte* format) => pigSliderInt3(label, v, v_min, v_max, format);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igShowUserGuide();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igShowUserGuideDelegate();
        private static readonly igShowUserGuideDelegate pigShowUserGuide = lib.LoadFunction<igShowUserGuideDelegate>("igShowUserGuide");
        public static void igShowUserGuide() => pigShowUserGuide();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igSliderScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* v_min, void* v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igSliderScalarNDelegate(byte* label, ImGuiDataType data_type, void* v, int components, void* v_min, void* v_max, byte* format, float power);
        private static readonly igSliderScalarNDelegate pigSliderScalarN = lib.LoadFunction<igSliderScalarNDelegate>("igSliderScalarN");
        public static byte igSliderScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* v_min, void* v_max, byte* format, float power) => pigSliderScalarN(label, data_type, v, components, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ImColor_HSV_nonUDT2")]
        public static extern ImColor ImColor_HSV(ImColor* self, float h, float s, float v, float a);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate ImColor ImColor_HSVDelegate(ImColor* self, float h, float s, float v, float a);
        private static readonly ImColor_HSVDelegate pImColor_HSV = lib.LoadFunction<ImColor_HSVDelegate>("ImColor_HSV");
        public static ImColor ImColor_HSV(ImColor* self, float h, float s, float v, float a) => pImColor_HSV(self, h, s, v, a);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PathLineTo(ImDrawList* self, Vector2 pos);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PathLineToDelegate(ImDrawList* self, Vector2 pos);
        private static readonly ImDrawList_PathLineToDelegate pImDrawList_PathLineTo = lib.LoadFunction<ImDrawList_PathLineToDelegate>("ImDrawList_PathLineTo");
        public static void ImDrawList_PathLineTo(ImDrawList* self, Vector2 pos) => pImDrawList_PathLineTo(self, pos);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igImage(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igImageDelegate(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col);
        private static readonly igImageDelegate pigImage = lib.LoadFunction<igImageDelegate>("igImage");
        public static void igImage(IntPtr user_texture_id, Vector2 size, Vector2 uv0, Vector2 uv1, Vector4 tint_col, Vector4 border_col) => pigImage(user_texture_id, size, uv0, uv1, tint_col, border_col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback, void* custom_callback_data);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetNextWindowSizeConstraintsDelegate(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback, void* custom_callback_data);
        private static readonly igSetNextWindowSizeConstraintsDelegate pigSetNextWindowSizeConstraints = lib.LoadFunction<igSetNextWindowSizeConstraintsDelegate>("igSetNextWindowSizeConstraints");
        public static void igSetNextWindowSizeConstraints(Vector2 size_min, Vector2 size_max, ImGuiSizeCallback custom_callback, void* custom_callback_data) => pigSetNextWindowSizeConstraints(size_min, size_max, custom_callback, custom_callback_data);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igDummy(Vector2 size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igDummyDelegate(Vector2 size);
        private static readonly igDummyDelegate pigDummy = lib.LoadFunction<igDummyDelegate>("igDummy");
        public static void igDummy(Vector2 size) => pigDummy(size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igVSliderInt(byte* label, Vector2 size, int* v, int v_min, int v_max, byte* format);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igVSliderIntDelegate(byte* label, Vector2 size, int* v, int v_min, int v_max, byte* format);
        private static readonly igVSliderIntDelegate pigVSliderInt = lib.LoadFunction<igVSliderIntDelegate>("igVSliderInt");
        public static byte igVSliderInt(byte* label, Vector2 size, int* v, int v_min, int v_max, byte* format) => pigVSliderInt(label, size, v, v_min, v_max, format);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImGuiTextBuffer_ImGuiTextBuffer(ImGuiTextBuffer* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImGuiTextBuffer_ImGuiTextBufferDelegate(ImGuiTextBuffer* self);
        private static readonly ImGuiTextBuffer_ImGuiTextBufferDelegate pImGuiTextBuffer_ImGuiTextBuffer = lib.LoadFunction<ImGuiTextBuffer_ImGuiTextBufferDelegate>("ImGuiTextBuffer_ImGuiTextBuffer");
        public static void ImGuiTextBuffer_ImGuiTextBuffer(ImGuiTextBuffer* self) => pImGuiTextBuffer_ImGuiTextBuffer(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igBulletText(byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igBulletTextDelegate(byte* fmt);
        private static readonly igBulletTextDelegate pigBulletText = lib.LoadFunction<igBulletTextDelegate>("igBulletText");
        public static void igBulletText(byte* fmt) => pigBulletText(fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igColorEdit4(byte* label, Vector4* col, ImGuiColorEditFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igColorEdit4Delegate(byte* label, Vector4* col, ImGuiColorEditFlags flags);
        private static readonly igColorEdit4Delegate pigColorEdit4 = lib.LoadFunction<igColorEdit4Delegate>("igColorEdit4");
        public static byte igColorEdit4(byte* label, Vector4* col, ImGuiColorEditFlags flags) => pigColorEdit4(label, col, flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igColorPicker4(byte* label, Vector4* col, ImGuiColorEditFlags flags, float* ref_col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igColorPicker4Delegate(byte* label, Vector4* col, ImGuiColorEditFlags flags, float* ref_col);
        private static readonly igColorPicker4Delegate pigColorPicker4 = lib.LoadFunction<igColorPicker4Delegate>("igColorPicker4");
        public static byte igColorPicker4(byte* label, Vector4* col, ImGuiColorEditFlags flags, float* ref_col) => pigColorPicker4(label, col, flags, ref_col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawList_PrimRectUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawList_PrimRectUVDelegate(ImDrawList* self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col);
        private static readonly ImDrawList_PrimRectUVDelegate pImDrawList_PrimRectUV = lib.LoadFunction<ImDrawList_PrimRectUVDelegate>("ImDrawList_PrimRectUV");
        public static void ImDrawList_PrimRectUV(ImDrawList* self, Vector2 a, Vector2 b, Vector2 uv_a, Vector2 uv_b, uint col) => pImDrawList_PrimRectUV(self, a, b, uv_a, uv_b, col);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInvisibleButton(byte* str_id, Vector2 size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInvisibleButtonDelegate(byte* str_id, Vector2 size);
        private static readonly igInvisibleButtonDelegate pigInvisibleButton = lib.LoadFunction<igInvisibleButtonDelegate>("igInvisibleButton");
        public static byte igInvisibleButton(byte* str_id, Vector2 size) => pigInvisibleButton(str_id, size);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igLogToClipboard(int max_depth);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igLogToClipboardDelegate(int max_depth);
        private static readonly igLogToClipboardDelegate pigLogToClipboard = lib.LoadFunction<igLogToClipboardDelegate>("igLogToClipboard");
        public static void igLogToClipboard(int max_depth) => pigLogToClipboard(max_depth);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igBeginPopupContextWindow(byte* str_id, int mouse_button, byte also_over_items);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igBeginPopupContextWindowDelegate(byte* str_id, int mouse_button, byte also_over_items);
        private static readonly igBeginPopupContextWindowDelegate pigBeginPopupContextWindow = lib.LoadFunction<igBeginPopupContextWindowDelegate>("igBeginPopupContextWindow");
        public static byte igBeginPopupContextWindow(byte* str_id, int mouse_button, byte also_over_items) => pigBeginPopupContextWindow(str_id, mouse_button, also_over_items);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImFontAtlas_ImFontAtlas(ImFontAtlas* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImFontAtlas_ImFontAtlasDelegate(ImFontAtlas* self);
        private static readonly ImFontAtlas_ImFontAtlasDelegate pImFontAtlas_ImFontAtlas = lib.LoadFunction<ImFontAtlas_ImFontAtlasDelegate>("ImFontAtlas_ImFontAtlas");
        public static void ImFontAtlas_ImFontAtlas(ImFontAtlas* self) => pImFontAtlas_ImFontAtlas(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragScalar(byte* label, ImGuiDataType data_type, void* v, float v_speed, void* v_min, void* v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragScalarDelegate(byte* label, ImGuiDataType data_type, void* v, float v_speed, void* v_min, void* v_max, byte* format, float power);
        private static readonly igDragScalarDelegate pigDragScalar = lib.LoadFunction<igDragScalarDelegate>("igDragScalar");
        public static byte igDragScalar(byte* label, ImGuiDataType data_type, void* v, float v_speed, void* v_min, void* v_max, byte* format, float power) => pigDragScalar(label, data_type, v, v_speed, v_min, v_max, format, power);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetItemDefaultFocus();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetItemDefaultFocusDelegate();
        private static readonly igSetItemDefaultFocusDelegate pigSetItemDefaultFocus = lib.LoadFunction<igSetItemDefaultFocusDelegate>("igSetItemDefaultFocus");
        public static void igSetItemDefaultFocus() => pigSetItemDefaultFocus();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igCaptureMouseFromApp(byte capture);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igCaptureMouseFromAppDelegate(byte capture);
        private static readonly igCaptureMouseFromAppDelegate pigCaptureMouseFromApp = lib.LoadFunction<igCaptureMouseFromAppDelegate>("igCaptureMouseFromApp");
        public static void igCaptureMouseFromApp(byte capture) => pigCaptureMouseFromApp(capture);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igIsAnyItemHovered();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igIsAnyItemHoveredDelegate();
        private static readonly igIsAnyItemHoveredDelegate pigIsAnyItemHovered = lib.LoadFunction<igIsAnyItemHoveredDelegate>("igIsAnyItemHovered");
        public static byte igIsAnyItemHovered() => pigIsAnyItemHovered();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPushFont(ImFont* font);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPushFontDelegate(ImFont* font);
        private static readonly igPushFontDelegate pigPushFont = lib.LoadFunction<igPushFontDelegate>("igPushFont");
        public static void igPushFont(ImFont* font) => pigPushFont(font);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputInt2(byte* label, int* v, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputInt2Delegate(byte* label, int* v, ImGuiInputTextFlags extra_flags);
        private static readonly igInputInt2Delegate pigInputInt2 = lib.LoadFunction<igInputInt2Delegate>("igInputInt2");
        public static byte igInputInt2(byte* label, int* v, ImGuiInputTextFlags extra_flags) => pigInputInt2(label, v, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igTreePop();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igTreePopDelegate();
        private static readonly igTreePopDelegate pigTreePop = lib.LoadFunction<igTreePopDelegate>("igTreePop");
        public static void igTreePop() => pigTreePop();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igEnd();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igEndDelegate();
        private static readonly igEndDelegate pigEnd = lib.LoadFunction<igEndDelegate>("igEnd");
        public static void igEnd() => pigEnd();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void ImDrawData_ImDrawData(ImDrawData* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void ImDrawData_ImDrawDataDelegate(ImDrawData* self);
        private static readonly ImDrawData_ImDrawDataDelegate pImDrawData_ImDrawData = lib.LoadFunction<ImDrawData_ImDrawDataDelegate>("ImDrawData_ImDrawData");
        public static void ImDrawData_ImDrawData(ImDrawData* self) => pImDrawData_ImDrawData(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igDestroyContext(IntPtr ctx);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igDestroyContextDelegate(IntPtr ctx);
        private static readonly igDestroyContextDelegate pigDestroyContext = lib.LoadFunction<igDestroyContextDelegate>("igDestroyContext");
        public static void igDestroyContext(IntPtr ctx) => pigDestroyContext(ctx);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte* ImGuiTextBuffer_end(ImGuiTextBuffer* self);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte* ImGuiTextBuffer_endDelegate(ImGuiTextBuffer* self);
        private static readonly ImGuiTextBuffer_endDelegate pImGuiTextBuffer_end = lib.LoadFunction<ImGuiTextBuffer_endDelegate>("ImGuiTextBuffer_end");
        public static byte* ImGuiTextBuffer_end(ImGuiTextBuffer* self) => pImGuiTextBuffer_end(self);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igPopStyleVar(int count);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igPopStyleVarDelegate(int count);
        private static readonly igPopStyleVarDelegate pigPopStyleVar = lib.LoadFunction<igPopStyleVarDelegate>("igPopStyleVar");
        public static void igPopStyleVar(int count) => pigPopStyleVar(count);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte ImGuiTextFilter_PassFilter(ImGuiTextFilter* self, byte* text, byte* text_end);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte ImGuiTextFilter_PassFilterDelegate(ImGuiTextFilter* self, byte* text, byte* text_end);
        private static readonly ImGuiTextFilter_PassFilterDelegate pImGuiTextFilter_PassFilter = lib.LoadFunction<ImGuiTextFilter_PassFilterDelegate>("ImGuiTextFilter_PassFilter");
        public static byte ImGuiTextFilter_PassFilter(ImGuiTextFilter* self, byte* text, byte* text_end) => pImGuiTextFilter_PassFilter(self, text, text_end);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igShowStyleSelector(byte* label);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igShowStyleSelectorDelegate(byte* label);
        private static readonly igShowStyleSelectorDelegate pigShowStyleSelector = lib.LoadFunction<igShowStyleSelectorDelegate>("igShowStyleSelector");
        public static byte igShowStyleSelector(byte* label) => pigShowStyleSelector(label);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputScalarNDelegate(byte* label, ImGuiDataType data_type, void* v, int components, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags);
        private static readonly igInputScalarNDelegate pigInputScalarN = lib.LoadFunction<igInputScalarNDelegate>("igInputScalarN");
        public static byte igInputScalarN(byte* label, ImGuiDataType data_type, void* v, int components, void* step, void* step_fast, byte* format, ImGuiInputTextFlags extra_flags) => pigInputScalarN(label, data_type, v, components, step, step_fast, format, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igTreeNodeStr(byte* label);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodeStrDelegate(byte* label);
        private static readonly igTreeNodeStrDelegate pigTreeNodeStr = lib.LoadFunction<igTreeNodeStrDelegate>("igTreeNodeStr");
        public static byte igTreeNodeStr(byte* label) => pigTreeNodeStr(label);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igTreeNodeStrStr(byte* str_id, byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodeStrStrDelegate(byte* str_id, byte* fmt);
        private static readonly igTreeNodeStrStrDelegate pigTreeNodeStrStr = lib.LoadFunction<igTreeNodeStrStrDelegate>("igTreeNodeStrStr");
        public static byte igTreeNodeStrStr(byte* str_id, byte* fmt) => pigTreeNodeStrStr(str_id, fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igTreeNodePtr(void* ptr_id, byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igTreeNodePtrDelegate(void* ptr_id, byte* fmt);
        private static readonly igTreeNodePtrDelegate pigTreeNodePtr = lib.LoadFunction<igTreeNodePtrDelegate>("igTreeNodePtr");
        public static byte igTreeNodePtr(void* ptr_id, byte* fmt) => pigTreeNodePtr(ptr_id, fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern float igGetScrollMaxX();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate float igGetScrollMaxXDelegate();
        private static readonly igGetScrollMaxXDelegate pigGetScrollMaxX = lib.LoadFunction<igGetScrollMaxXDelegate>("igGetScrollMaxX");
        public static float igGetScrollMaxX() => pigGetScrollMaxX();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern void igSetTooltip(byte* fmt);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void igSetTooltipDelegate(byte* fmt);
        private static readonly igSetTooltipDelegate pigSetTooltip = lib.LoadFunction<igSetTooltipDelegate>("igSetTooltip");
        public static void igSetTooltip(byte* fmt) => pigSetTooltip(fmt);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl, EntryPoint = "igGetContentRegionAvail_nonUDT2")]
        public static extern Vector2 igGetContentRegionAvail();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Vector2 igGetContentRegionAvailDelegate();
        private static readonly igGetContentRegionAvailDelegate pigGetContentRegionAvail = lib.LoadFunction<igGetContentRegionAvailDelegate>("igGetContentRegionAvail");
        public static Vector2 igGetContentRegionAvail() => pigGetContentRegionAvail();
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igInputFloat3(byte* label, Vector3* v, byte* format, ImGuiInputTextFlags extra_flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igInputFloat3Delegate(byte* label, Vector3* v, byte* format, ImGuiInputTextFlags extra_flags);
        private static readonly igInputFloat3Delegate pigInputFloat3 = lib.LoadFunction<igInputFloat3Delegate>("igInputFloat3");
        public static byte igInputFloat3(byte* label, Vector3* v, byte* format, ImGuiInputTextFlags extra_flags) => pigInputFloat3(label, v, format, extra_flags);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, CallingConvention = CallingConvention.Cdecl)]
        public static extern byte igDragFloat2(byte* label, Vector2* v, float v_speed, float v_min, float v_max, byte* format, float power);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate byte igDragFloat2Delegate(byte* label, Vector2* v, float v_speed, float v_min, float v_max, byte* format, float power);
        private static readonly igDragFloat2Delegate pigDragFloat2 = lib.LoadFunction<igDragFloat2Delegate>("igDragFloat2");
        public static byte igDragFloat2(byte* label, Vector2* v, float v_speed, float v_min, float v_max, byte* format, float power) => pigDragFloat2(label, v, v_speed, v_min, v_max, format, power);
#endif
        
    }
#pragma warning restore 1591
}
