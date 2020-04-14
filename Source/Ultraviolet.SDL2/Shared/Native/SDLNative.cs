using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    public static unsafe partial class SDLNative
    {
        private static readonly SDLNativeImpl impl;
        
        static SDLNative()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Android:
                    impl = new SDLNativeImpl_Android();
                    break;
                    
                default:
                    impl = new SDLNativeImpl_Default();
                    break;
            }
        }
        
        public const Int32 SDL_QUERY = -1;
        public const Int32 SDL_DISABLE = 0;
        public const Int32 SDL_ENABLE = 1;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static String SDL_GetError() => impl.SDL_GetError();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_ClearError() => impl.SDL_ClearError();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_Init(SDL_Init flags) => impl.SDL_Init(flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_Quit() => impl.SDL_Quit();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_PumpEvents() => impl.SDL_PumpEvents();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_PollEvent(out SDL_Event @event) => impl.SDL_PollEvent(out @event);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetEventFilter(IntPtr filter, IntPtr userdata) => impl.SDL_SetEventFilter(filter, userdata);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_CreateWindow(String title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags) => impl.SDL_CreateWindow(title, x, y, w, h, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_CreateWindowFrom(IntPtr data) => impl.SDL_CreateWindowFrom(data);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_DestroyWindow(IntPtr window) => impl.SDL_DestroyWindow(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 SDL_GetWindowID(IntPtr window) => impl.SDL_GetWindowID(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static String SDL_GetWindowTitle(IntPtr window) => impl.SDL_GetWindowTitle(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetWindowTitle(IntPtr window, String title) => impl.SDL_SetWindowTitle(window, title);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetWindowIcon(IntPtr window, IntPtr icon) => impl.SDL_SetWindowIcon(window, icon);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_GetWindowPosition(IntPtr window, out Int32 x, out Int32 y) => impl.SDL_GetWindowPosition(window, out x, out y);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetWindowPosition(IntPtr window, Int32 x, Int32 y) => impl.SDL_SetWindowPosition(window, x, y);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_GetWindowSize(IntPtr window, out Int32 w, out Int32 h) => impl.SDL_GetWindowSize(window, out w, out h);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetWindowSize(IntPtr window, Int32 w, Int32 h) => impl.SDL_SetWindowSize(window, w, h);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_GetWindowMinimumSize(IntPtr window, out Int32 w, out Int32 h) => impl.SDL_GetWindowMinimumSize(window, out w, out h);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetWindowMinimumSize(IntPtr window, Int32 w, Int32 h) => impl.SDL_SetWindowMinimumSize(window, w, h);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_GetWindowMaximumSize(IntPtr window, out Int32 w, out Int32 h) => impl.SDL_GetWindowMaximumSize(window, out w, out h);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetWindowMaximumSize(IntPtr window, Int32 w, Int32 h) => impl.SDL_SetWindowMaximumSize(window, w, h);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean SDL_GetWindowGrab(IntPtr window) => impl.SDL_GetWindowGrab(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetWindowGrab(IntPtr window, Boolean grabbed) => impl.SDL_SetWindowGrab(window, grabbed);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_SetWindowBordered(IntPtr window, Boolean bordered) => impl.SDL_SetWindowBordered(window, bordered);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_SetWindowFullscreen(IntPtr window, UInt32 flags) => impl.SDL_SetWindowFullscreen(window, flags);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_SetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode) => impl.SDL_SetWindowDisplayMode(window, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode) => impl.SDL_GetWindowDisplayMode(window, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetWindowDisplayIndex(IntPtr window) => impl.SDL_GetWindowDisplayIndex(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_WindowFlags SDL_GetWindowFlags(IntPtr window) => impl.SDL_GetWindowFlags(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_ShowWindow(IntPtr window) => impl.SDL_ShowWindow(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_HideWindow(IntPtr window) => impl.SDL_HideWindow(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_MaximizeWindow(IntPtr window) => impl.SDL_MaximizeWindow(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_MinimizeWindow(IntPtr window) => impl.SDL_MinimizeWindow(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_RestoreWindow(IntPtr window) => impl.SDL_RestoreWindow(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean SDL_GetWindowWMInfo(IntPtr window, SDL_SysWMinfo* info) => impl.SDL_GetWindowWMInfo(window, info);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_RWFromFile(String file, String mode) => impl.SDL_RWFromFile(file, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_RWFromMem(IntPtr mem, Int32 size) => impl.SDL_RWFromMem(mem, size);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_AllocRW() => impl.SDL_AllocRW();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_FreeRW(IntPtr area) => impl.SDL_FreeRW(area);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_Surface* SDL_LoadBMP_RW(IntPtr src, Int32 freesrc) => impl.SDL_LoadBMP_RW(src, freesrc);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_SaveBMP_RW(SDL_Surface* surface, IntPtr dst, Int32 freedst) => impl.SDL_SaveBMP_RW(surface, dst, freedst);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UInt32 SDL_GetMouseState(out Int32 x, out Int32 y) => impl.SDL_GetMouseState(out x, out y);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_GetKeyboardState(out Int32 numkeys) => impl.SDL_GetKeyboardState(out numkeys);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_Scancode SDL_GetScancodeFromKey(SDL_Keycode keycode) => impl.SDL_GetScancodeFromKey(keycode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_Keymod SDL_GetModState() => impl.SDL_GetModState();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean SDL_SetHint(String name, String value) => impl.SDL_SetHint(name, value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_Surface* SDL_CreateRGBSurface(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 AMask) => impl.SDL_CreateRGBSurface(flags, width, height, depth, Rmask, Gmask, Bmask, AMask);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_FreeSurface(SDL_Surface* surface) => impl.SDL_FreeSurface(surface);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_LockSurface(SDL_Surface* surface) => impl.SDL_LockSurface(surface);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_UnlockSurface(SDL_Surface* surface) => impl.SDL_UnlockSurface(surface);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_BlitSurface(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect) => impl.SDL_BlitSurface(src, srcrect, dst, dstrect);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_BlitScaled(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect) => impl.SDL_BlitScaled(src, srcrect, dst, dstrect);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_SetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode blendMode) => impl.SDL_SetSurfaceBlendMode(surface, blendMode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode* blendMode) => impl.SDL_GetSurfaceBlendMode(surface, blendMode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_FillRect(SDL_Surface* surface, SDL_Rect* rect, UInt32 color) => impl.SDL_FillRect(surface, rect, color);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_FillRects(SDL_Surface* dst, SDL_Rect* rects, Int32 count, UInt32 colors) => impl.SDL_FillRects(dst, rects, count, colors);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_Cursor* SDL_CreateColorCursor(SDL_Surface* surface, Int32 hot_x, Int32 hot_y) => impl.SDL_CreateColorCursor(surface, hot_x, hot_y);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_FreeCursor(SDL_Cursor* cursor) => impl.SDL_FreeCursor(cursor);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_ShowCursor(Int32 toggle) => impl.SDL_ShowCursor(toggle);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_Cursor* SDL_GetCursor() => impl.SDL_GetCursor();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetCursor(SDL_Cursor* cursor) => impl.SDL_SetCursor(cursor);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_Cursor* SDL_GetDefaultCursor() => impl.SDL_GetDefaultCursor();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetNumVideoDisplays() => impl.SDL_GetNumVideoDisplays();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static String SDL_GetDisplayName(Int32 displayIndex) => impl.SDL_GetDisplayName(displayIndex);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetDisplayBounds(Int32 displayIndex, SDL_Rect* rect) => impl.SDL_GetDisplayBounds(displayIndex, rect);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetNumDisplayModes(Int32 displayIndex) => impl.SDL_GetNumDisplayModes(displayIndex);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetDisplayMode(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode) => impl.SDL_GetDisplayMode(displayIndex, modeIndex, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetCurrentDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode) => impl.SDL_GetCurrentDisplayMode(displayIndex, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetDesktopDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode) => impl.SDL_GetDesktopDisplayMode(displayIndex, mode);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_DisplayMode* SDL_GetClosestDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest) => impl.SDL_GetClosestDisplayMode(displayIndex, mode, closest);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean SDL_PixelFormatEnumToMasks(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask) => impl.SDL_PixelFormatEnumToMasks(format, bpp, Rmask, Gmask, Bmask, Amask);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_GL_GetProcAddress(String proc) => impl.SDL_GL_GetProcAddress(proc);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_GL_CreateContext(IntPtr window) => impl.SDL_GL_CreateContext(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_GL_DeleteContext(IntPtr context) => impl.SDL_GL_DeleteContext(context);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_GL_GetCurrentContext(IntPtr context) => impl.SDL_GL_GetCurrentContext(context);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GL_MakeCurrent(IntPtr window, IntPtr context) => impl.SDL_GL_MakeCurrent(window, context);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GL_SetAttribute(SDL_GLattr attr, Int32 value) => impl.SDL_GL_SetAttribute(attr, value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GL_GetAttribute(SDL_GLattr attr, Int32* value) => impl.SDL_GL_GetAttribute(attr, value);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_GL_SwapWindow(IntPtr window) => impl.SDL_GL_SwapWindow(window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GL_SetSwapInterval(Int32 interval) => impl.SDL_GL_SetSwapInterval(interval);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_GL_GetDrawableSize(IntPtr window, out Int32 w, out Int32 h) => impl.SDL_GL_GetDrawableSize(window, out w, out h);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_NumJoysticks() => impl.SDL_NumJoysticks();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean SDL_IsGameController(Int32 joystick_index) => impl.SDL_IsGameController(joystick_index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_GameControllerOpen(Int32 index) => impl.SDL_GameControllerOpen(index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_GameControllerClose(IntPtr gamecontroller) => impl.SDL_GameControllerClose(gamecontroller);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static String SDL_GameControllerNameForIndex(Int32 joystick_index) => impl.SDL_GameControllerNameForIndex(joystick_index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean SDL_GameControllerGetButton(IntPtr gamecontroller, SDL_GameControllerButton button) => impl.SDL_GameControllerGetButton(gamecontroller, button);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_GameControllerGetJoystick(IntPtr gamecontroller) => impl.SDL_GameControllerGetJoystick(gamecontroller);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_JoystickInstanceID(IntPtr joystick) => impl.SDL_JoystickInstanceID(joystick);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetNumTouchDevices() => impl.SDL_GetNumTouchDevices();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 SDL_GetTouchDevice(Int32 index) => impl.SDL_GetTouchDevice(index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetNumTouchFingers(Int64 touchID) => impl.SDL_GetNumTouchFingers(touchID);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_Finger* SDL_GetTouchFinger(Int64 touchID, Int32 index) => impl.SDL_GetTouchFinger(touchID, index);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_RecordGesture(Int64 touchID) => impl.SDL_RecordGesture(touchID);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_SaveAllDollarTemplates(IntPtr dst) => impl.SDL_SaveAllDollarTemplates(dst);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_SaveDollarTemplate(Int64 gestureID, IntPtr dst) => impl.SDL_SaveDollarTemplate(gestureID, dst);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_LoadDollarTemplates(Int64 touchID, IntPtr src) => impl.SDL_LoadDollarTemplates(touchID, src);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_StartTextInput() => impl.SDL_StartTextInput();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_StopTextInput() => impl.SDL_StopTextInput();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetTextInputRect(SDL_Rect* rect) => impl.SDL_SetTextInputRect(rect);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean SDL_HasClipboardText() => impl.SDL_HasClipboardText();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr SDL_GetClipboardText() => impl.SDL_GetClipboardText();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_SetClipboardText(IntPtr text) => impl.SDL_SetClipboardText(text);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SDL_PowerState SDL_GetPowerInfo(Int32* secs, Int32* pct) => impl.SDL_GetPowerInfo(secs, pct);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_ShowSimpleMessageBox(UInt32 flags, String title, String message, IntPtr window) => impl.SDL_ShowSimpleMessageBox(flags, title, message, window);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_SetWindowOpacity(IntPtr window, Single opacity) => impl.SDL_SetWindowOpacity(window, opacity);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetWindowOpacity(IntPtr window, Single* opacity) => impl.SDL_GetWindowOpacity(window, opacity);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GameControllerAddMapping(String mappingString) => impl.SDL_GameControllerAddMapping(mappingString);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GameControllerAddMappingsFromRW(IntPtr rw, Int32 freerw) => impl.SDL_GameControllerAddMappingsFromRW(rw, freerw);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static String SDL_GameControllerMapping(IntPtr gamecontroller) => impl.SDL_GameControllerMapping(gamecontroller);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static String SDL_GameControllerMappingForGUID(Guid guid) => impl.SDL_GameControllerMappingForGUID(guid);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid SDL_JoystickGetGUID(String pchGUID) => impl.SDL_JoystickGetGUID(pchGUID);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_GetDisplayDPI(Int32 displayIndex, Single* ddpi, Single* hdpi, Single* vdpi) => impl.SDL_GetDisplayDPI(displayIndex, ddpi, hdpi, vdpi);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_free(IntPtr mem) => impl.SDL_free(mem);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Boolean SDL_GetRelativeMouseMode() => impl.SDL_GetRelativeMouseMode();
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 SDL_SetRelativeMouseMode(Boolean enabled) => impl.SDL_SetRelativeMouseMode(enabled);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SDL_WarpMouseInWindow(IntPtr window, Int32 x, Int32 y) => impl.SDL_WarpMouseInWindow(window, x, y);
    }
#pragma warning restore 1591
}
