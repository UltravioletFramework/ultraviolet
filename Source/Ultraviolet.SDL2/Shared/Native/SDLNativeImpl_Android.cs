using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class SDLNativeImpl_Android : SDLNativeImpl
    {
        [DllImport("SDL2", EntryPoint = "SDL_GetError", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetError();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IntPtr SDL_GetError_Raw() => INTERNAL_SDL_GetError();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GetError() => Marshal.PtrToStringAnsi(SDL_GetError_Raw());
        
        [DllImport("SDL2", EntryPoint = "SDL_ClearError", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_ClearError();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_ClearError() => INTERNAL_SDL_ClearError();
        
        [DllImport("SDL2", EntryPoint = "SDL_Init", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_Init(SDL_Init flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_Init(SDL_Init flags) => INTERNAL_SDL_Init(flags);
        
        [DllImport("SDL2", EntryPoint = "SDL_Quit", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_Quit();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_Quit() => INTERNAL_SDL_Quit();
        
        [DllImport("SDL2", EntryPoint = "SDL_PumpEvents", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_PumpEvents();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_PumpEvents() => INTERNAL_SDL_PumpEvents();
        
        [DllImport("SDL2", EntryPoint = "SDL_PollEvent", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_PollEvent(out SDL_Event @event);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_PollEvent(out SDL_Event @event) => INTERNAL_SDL_PollEvent(out @event);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetEventFilter", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetEventFilter(IntPtr filter, IntPtr userdata);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetEventFilter(IntPtr filter, IntPtr userdata) => INTERNAL_SDL_SetEventFilter(filter, userdata);
        
        [DllImport("SDL2", EntryPoint = "SDL_CreateWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_CreateWindow([MarshalAs(UnmanagedType.LPStr)] String title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_CreateWindow(String title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags) => INTERNAL_SDL_CreateWindow(title, x, y, w, h, flags);
        
        [DllImport("SDL2", EntryPoint = "SDL_CreateWindowFrom", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_CreateWindowFrom(IntPtr data);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_CreateWindowFrom(IntPtr data) => INTERNAL_SDL_CreateWindowFrom(data);
        
        [DllImport("SDL2", EntryPoint = "SDL_DestroyWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_DestroyWindow(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_DestroyWindow(IntPtr window) => INTERNAL_SDL_DestroyWindow(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowID", CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 INTERNAL_SDL_GetWindowID(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 SDL_GetWindowID(IntPtr window) => INTERNAL_SDL_GetWindowID(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetWindowTitle(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IntPtr SDL_GetWindowTitle_Raw(IntPtr window) => INTERNAL_SDL_GetWindowTitle(window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GetWindowTitle(IntPtr window) => Marshal.PtrToStringAnsi(SDL_GetWindowTitle_Raw(window));
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetWindowTitle(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] String title);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowTitle(IntPtr window, String title) => INTERNAL_SDL_SetWindowTitle(window, title);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowIcon", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetWindowIcon(IntPtr window, IntPtr icon);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowIcon(IntPtr window, IntPtr icon) => INTERNAL_SDL_SetWindowIcon(window, icon);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowPosition", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_GetWindowPosition(IntPtr window, out Int32 x, out Int32 y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GetWindowPosition(IntPtr window, out Int32 x, out Int32 y) => INTERNAL_SDL_GetWindowPosition(window, out x, out y);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowPosition", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetWindowPosition(IntPtr window, Int32 x, Int32 y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowPosition(IntPtr window, Int32 x, Int32 y) => INTERNAL_SDL_SetWindowPosition(window, x, y);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_GetWindowSize(IntPtr window, out Int32 w, out Int32 h);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GetWindowSize(IntPtr window, out Int32 w, out Int32 h) => INTERNAL_SDL_GetWindowSize(window, out w, out h);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetWindowSize(IntPtr window, Int32 w, Int32 h);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowSize(IntPtr window, Int32 w, Int32 h) => INTERNAL_SDL_SetWindowSize(window, w, h);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowMinimumSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_GetWindowMinimumSize(IntPtr window, out Int32 w, out Int32 h);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GetWindowMinimumSize(IntPtr window, out Int32 w, out Int32 h) => INTERNAL_SDL_GetWindowMinimumSize(window, out w, out h);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowMinimumSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetWindowMinimumSize(IntPtr window, Int32 w, Int32 h);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowMinimumSize(IntPtr window, Int32 w, Int32 h) => INTERNAL_SDL_SetWindowMinimumSize(window, w, h);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowMaximumSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_GetWindowMaximumSize(IntPtr window, out Int32 w, out Int32 h);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GetWindowMaximumSize(IntPtr window, out Int32 w, out Int32 h) => INTERNAL_SDL_GetWindowMaximumSize(window, out w, out h);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowMaximumSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetWindowMaximumSize(IntPtr window, Int32 w, Int32 h);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowMaximumSize(IntPtr window, Int32 w, Int32 h) => INTERNAL_SDL_SetWindowMaximumSize(window, w, h);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowGrab", CallingConvention = CallingConvention.Cdecl)]
        private static extern Boolean INTERNAL_SDL_GetWindowGrab(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_GetWindowGrab(IntPtr window) => INTERNAL_SDL_GetWindowGrab(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowGrab", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetWindowGrab(IntPtr window, Boolean grabbed);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowGrab(IntPtr window, Boolean grabbed) => INTERNAL_SDL_SetWindowGrab(window, grabbed);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowBordered", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_SetWindowBordered(IntPtr window, Boolean bordered);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetWindowBordered(IntPtr window, Boolean bordered) => INTERNAL_SDL_SetWindowBordered(window, bordered);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowFullscreen", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_SetWindowFullscreen(IntPtr window, UInt32 flags);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetWindowFullscreen(IntPtr window, UInt32 flags) => INTERNAL_SDL_SetWindowFullscreen(window, flags);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_SetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode) => INTERNAL_SDL_SetWindowDisplayMode(window, mode);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode) => INTERNAL_SDL_GetWindowDisplayMode(window, mode);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowDisplayIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetWindowDisplayIndex(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetWindowDisplayIndex(IntPtr window) => INTERNAL_SDL_GetWindowDisplayIndex(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowFlags", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_WindowFlags INTERNAL_SDL_GetWindowFlags(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_WindowFlags SDL_GetWindowFlags(IntPtr window) => INTERNAL_SDL_GetWindowFlags(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_ShowWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_ShowWindow(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_ShowWindow(IntPtr window) => INTERNAL_SDL_ShowWindow(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_HideWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_HideWindow(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_HideWindow(IntPtr window) => INTERNAL_SDL_HideWindow(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_MaximizeWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_MaximizeWindow(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_MaximizeWindow(IntPtr window) => INTERNAL_SDL_MaximizeWindow(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_MinimizeWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_MinimizeWindow(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_MinimizeWindow(IntPtr window) => INTERNAL_SDL_MinimizeWindow(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_RestoreWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_RestoreWindow(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_RestoreWindow(IntPtr window) => INTERNAL_SDL_RestoreWindow(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowWMInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern Boolean INTERNAL_SDL_GetWindowWMInfo(IntPtr window, SDL_SysWMinfo* info);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_GetWindowWMInfo(IntPtr window, SDL_SysWMinfo* info) => INTERNAL_SDL_GetWindowWMInfo(window, info);
        
        [DllImport("SDL2", EntryPoint = "SDL_RWFromFile", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_RWFromFile([MarshalAs(UnmanagedType.LPStr)] String file, [MarshalAs(UnmanagedType.LPStr)] String mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_RWFromFile(String file, String mode) => INTERNAL_SDL_RWFromFile(file, mode);
        
        [DllImport("SDL2", EntryPoint = "SDL_RWFromMem", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_RWFromMem(IntPtr mem, Int32 size);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_RWFromMem(IntPtr mem, Int32 size) => INTERNAL_SDL_RWFromMem(mem, size);
        
        [DllImport("SDL2", EntryPoint = "SDL_AllocRW", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_AllocRW();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_AllocRW() => INTERNAL_SDL_AllocRW();
        
        [DllImport("SDL2", EntryPoint = "SDL_FreeRW", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_FreeRW(IntPtr area);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_FreeRW(IntPtr area) => INTERNAL_SDL_FreeRW(area);
        
        [DllImport("SDL2", EntryPoint = "SDL_LoadBMP_RW", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_Surface* INTERNAL_SDL_LoadBMP_RW(IntPtr src, Int32 freesrc);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Surface* SDL_LoadBMP_RW(IntPtr src, Int32 freesrc) => INTERNAL_SDL_LoadBMP_RW(src, freesrc);
        
        [DllImport("SDL2", EntryPoint = "SDL_SaveBMP_RW", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_SaveBMP_RW(SDL_Surface* surface, IntPtr dst, Int32 freedst);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SaveBMP_RW(SDL_Surface* surface, IntPtr dst, Int32 freedst) => INTERNAL_SDL_SaveBMP_RW(surface, dst, freedst);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetMouseState", CallingConvention = CallingConvention.Cdecl)]
        private static extern UInt32 INTERNAL_SDL_GetMouseState(out Int32 x, out Int32 y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 SDL_GetMouseState(out Int32 x, out Int32 y) => INTERNAL_SDL_GetMouseState(out x, out y);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetKeyboardState", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetKeyboardState(out Int32 numkeys);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GetKeyboardState(out Int32 numkeys) => INTERNAL_SDL_GetKeyboardState(out numkeys);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetScancodeFromKey", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_Scancode INTERNAL_SDL_GetScancodeFromKey(SDL_Keycode keycode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Scancode SDL_GetScancodeFromKey(SDL_Keycode keycode) => INTERNAL_SDL_GetScancodeFromKey(keycode);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetModState", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_Keymod INTERNAL_SDL_GetModState();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Keymod SDL_GetModState() => INTERNAL_SDL_GetModState();
        
        [DllImport("SDL2", EntryPoint = "SDL_SetHint", CallingConvention = CallingConvention.Cdecl)]
        private static extern Boolean INTERNAL_SDL_SetHint([MarshalAs(UnmanagedType.LPStr)] String name, [MarshalAs(UnmanagedType.LPStr)] String value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_SetHint(String name, String value) => INTERNAL_SDL_SetHint(name, value);
        
        [DllImport("SDL2", EntryPoint = "SDL_CreateRGBSurface", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_Surface* INTERNAL_SDL_CreateRGBSurface(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 AMask);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Surface* SDL_CreateRGBSurface(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 AMask) => INTERNAL_SDL_CreateRGBSurface(flags, width, height, depth, Rmask, Gmask, Bmask, AMask);
        
        [DllImport("SDL2", EntryPoint = "SDL_FreeSurface", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_FreeSurface(SDL_Surface* surface);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_FreeSurface(SDL_Surface* surface) => INTERNAL_SDL_FreeSurface(surface);
        
        [DllImport("SDL2", EntryPoint = "SDL_LockSurface", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_LockSurface(SDL_Surface* surface);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_LockSurface(SDL_Surface* surface) => INTERNAL_SDL_LockSurface(surface);
        
        [DllImport("SDL2", EntryPoint = "SDL_UnlockSurface", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_UnlockSurface(SDL_Surface* surface);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_UnlockSurface(SDL_Surface* surface) => INTERNAL_SDL_UnlockSurface(surface);
        
        [DllImport("SDL2", EntryPoint = "SDL_UpperBlit", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_BlitSurface(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_BlitSurface(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect) => INTERNAL_SDL_BlitSurface(src, srcrect, dst, dstrect);
        
        [DllImport("SDL2", EntryPoint = "SDL_UpperBlitScaled", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_BlitScaled(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_BlitScaled(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect) => INTERNAL_SDL_BlitScaled(src, srcrect, dst, dstrect);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetSurfaceBlendMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_SetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode blendMode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode blendMode) => INTERNAL_SDL_SetSurfaceBlendMode(surface, blendMode);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetSurfaceBlendMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode* blendMode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode* blendMode) => INTERNAL_SDL_GetSurfaceBlendMode(surface, blendMode);
        
        [DllImport("SDL2", EntryPoint = "SDL_FillRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_FillRect(SDL_Surface* surface, SDL_Rect* rect, UInt32 color);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_FillRect(SDL_Surface* surface, SDL_Rect* rect, UInt32 color) => INTERNAL_SDL_FillRect(surface, rect, color);
        
        [DllImport("SDL2", EntryPoint = "SDL_FillRects", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_FillRects(SDL_Surface* dst, SDL_Rect* rects, Int32 count, UInt32 colors);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_FillRects(SDL_Surface* dst, SDL_Rect* rects, Int32 count, UInt32 colors) => INTERNAL_SDL_FillRects(dst, rects, count, colors);
        
        [DllImport("SDL2", EntryPoint = "SDL_CreateColorCursor", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_Cursor* INTERNAL_SDL_CreateColorCursor(SDL_Surface* surface, Int32 hot_x, Int32 hot_y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Cursor* SDL_CreateColorCursor(SDL_Surface* surface, Int32 hot_x, Int32 hot_y) => INTERNAL_SDL_CreateColorCursor(surface, hot_x, hot_y);
        
        [DllImport("SDL2", EntryPoint = "SDL_FreeCursor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_FreeCursor(SDL_Cursor* cursor);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_FreeCursor(SDL_Cursor* cursor) => INTERNAL_SDL_FreeCursor(cursor);
        
        [DllImport("SDL2", EntryPoint = "SDL_ShowCursor", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_ShowCursor(Int32 toggle);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_ShowCursor(Int32 toggle) => INTERNAL_SDL_ShowCursor(toggle);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetCursor", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_Cursor* INTERNAL_SDL_GetCursor();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Cursor* SDL_GetCursor() => INTERNAL_SDL_GetCursor();
        
        [DllImport("SDL2", EntryPoint = "SDL_SetCursor", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetCursor(SDL_Cursor* cursor);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetCursor(SDL_Cursor* cursor) => INTERNAL_SDL_SetCursor(cursor);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetDefaultCursor", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_Cursor* INTERNAL_SDL_GetDefaultCursor();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Cursor* SDL_GetDefaultCursor() => INTERNAL_SDL_GetDefaultCursor();
        
        [DllImport("SDL2", EntryPoint = "SDL_GetNumVideoDisplays", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetNumVideoDisplays();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetNumVideoDisplays() => INTERNAL_SDL_GetNumVideoDisplays();
        
        [DllImport("SDL2", EntryPoint = "SDL_GetDisplayName", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetDisplayName(Int32 displayIndex);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IntPtr SDL_GetDisplayName_Raw(Int32 displayIndex) => INTERNAL_SDL_GetDisplayName(displayIndex);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GetDisplayName(Int32 displayIndex) => Marshal.PtrToStringAnsi(SDL_GetDisplayName_Raw(displayIndex));
        
        [DllImport("SDL2", EntryPoint = "SDL_GetDisplayBounds", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetDisplayBounds(Int32 displayIndex, SDL_Rect* rect);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetDisplayBounds(Int32 displayIndex, SDL_Rect* rect) => INTERNAL_SDL_GetDisplayBounds(displayIndex, rect);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetNumDisplayModes", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetNumDisplayModes(Int32 displayIndex);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetNumDisplayModes(Int32 displayIndex) => INTERNAL_SDL_GetNumDisplayModes(displayIndex);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetDisplayMode(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetDisplayMode(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode) => INTERNAL_SDL_GetDisplayMode(displayIndex, modeIndex, mode);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetCurrentDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetCurrentDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetCurrentDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode) => INTERNAL_SDL_GetCurrentDisplayMode(displayIndex, mode);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetDesktopDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetDesktopDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetDesktopDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode) => INTERNAL_SDL_GetDesktopDisplayMode(displayIndex, mode);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetClosestDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_DisplayMode* INTERNAL_SDL_GetClosestDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_DisplayMode* SDL_GetClosestDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest) => INTERNAL_SDL_GetClosestDisplayMode(displayIndex, mode, closest);
        
        [DllImport("SDL2", EntryPoint = "SDL_PixelFormatEnumToMasks", CallingConvention = CallingConvention.Cdecl)]
        private static extern Boolean INTERNAL_SDL_PixelFormatEnumToMasks(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_PixelFormatEnumToMasks(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask) => INTERNAL_SDL_PixelFormatEnumToMasks(format, bpp, Rmask, Gmask, Bmask, Amask);
        
        [DllImport("SDL2", EntryPoint = "SDL_GL_GetProcAddress", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GL_GetProcAddress([MarshalAs(UnmanagedType.LPStr)] String proc);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GL_GetProcAddress(String proc) => INTERNAL_SDL_GL_GetProcAddress(proc);
        
        [DllImport("SDL2", EntryPoint = "SDL_GL_CreateContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GL_CreateContext(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GL_CreateContext(IntPtr window) => INTERNAL_SDL_GL_CreateContext(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_GL_DeleteContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_GL_DeleteContext(IntPtr context);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GL_DeleteContext(IntPtr context) => INTERNAL_SDL_GL_DeleteContext(context);
        
        [DllImport("SDL2", EntryPoint = "SDL_GL_GetCurrentContext", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GL_GetCurrentContext(IntPtr context);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GL_GetCurrentContext(IntPtr context) => INTERNAL_SDL_GL_GetCurrentContext(context);
        
        [DllImport("SDL2", EntryPoint = "SDL_GL_MakeCurrent", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GL_MakeCurrent(IntPtr window, IntPtr context);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GL_MakeCurrent(IntPtr window, IntPtr context) => INTERNAL_SDL_GL_MakeCurrent(window, context);
        
        [DllImport("SDL2", EntryPoint = "SDL_GL_SetAttribute", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GL_SetAttribute(SDL_GLattr attr, Int32 value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GL_SetAttribute(SDL_GLattr attr, Int32 value) => INTERNAL_SDL_GL_SetAttribute(attr, value);
        
        [DllImport("SDL2", EntryPoint = "SDL_GL_GetAttribute", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GL_GetAttribute(SDL_GLattr attr, Int32* value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GL_GetAttribute(SDL_GLattr attr, Int32* value) => INTERNAL_SDL_GL_GetAttribute(attr, value);
        
        [DllImport("SDL2", EntryPoint = "SDL_GL_SwapWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_GL_SwapWindow(IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GL_SwapWindow(IntPtr window) => INTERNAL_SDL_GL_SwapWindow(window);
        
        [DllImport("SDL2", EntryPoint = "SDL_GL_SetSwapInterval", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GL_SetSwapInterval(Int32 interval);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GL_SetSwapInterval(Int32 interval) => INTERNAL_SDL_GL_SetSwapInterval(interval);
        
        [DllImport("SDL2", EntryPoint = "SDL_GL_GetDrawableSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_GL_GetDrawableSize(IntPtr window, out Int32 w, out Int32 h);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GL_GetDrawableSize(IntPtr window, out Int32 w, out Int32 h) => INTERNAL_SDL_GL_GetDrawableSize(window, out w, out h);
        
        [DllImport("SDL2", EntryPoint = "SDL_NumJoysticks", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_NumJoysticks();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_NumJoysticks() => INTERNAL_SDL_NumJoysticks();
        
        [DllImport("SDL2", EntryPoint = "SDL_IsGameController", CallingConvention = CallingConvention.Cdecl)]
        private static extern Boolean INTERNAL_SDL_IsGameController(Int32 joystick_index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_IsGameController(Int32 joystick_index) => INTERNAL_SDL_IsGameController(joystick_index);
        
        [DllImport("SDL2", EntryPoint = "SDL_GameControllerOpen", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GameControllerOpen(Int32 index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GameControllerOpen(Int32 index) => INTERNAL_SDL_GameControllerOpen(index);
        
        [DllImport("SDL2", EntryPoint = "SDL_GameControllerClose", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_GameControllerClose(IntPtr gamecontroller);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GameControllerClose(IntPtr gamecontroller) => INTERNAL_SDL_GameControllerClose(gamecontroller);
        
        [DllImport("SDL2", EntryPoint = "SDL_GameControllerNameForIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GameControllerNameForIndex(Int32 joystick_index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IntPtr SDL_GameControllerNameForIndex_Raw(Int32 joystick_index) => INTERNAL_SDL_GameControllerNameForIndex(joystick_index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GameControllerNameForIndex(Int32 joystick_index) => Marshal.PtrToStringAnsi(SDL_GameControllerNameForIndex_Raw(joystick_index));
        
        [DllImport("SDL2", EntryPoint = "SDL_GameControllerGetButton", CallingConvention = CallingConvention.Cdecl)]
        private static extern Boolean INTERNAL_SDL_GameControllerGetButton(IntPtr gamecontroller, SDL_GameControllerButton button);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_GameControllerGetButton(IntPtr gamecontroller, SDL_GameControllerButton button) => INTERNAL_SDL_GameControllerGetButton(gamecontroller, button);
        
        [DllImport("SDL2", EntryPoint = "SDL_GameControllerGetJoystick", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GameControllerGetJoystick(IntPtr gamecontroller);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GameControllerGetJoystick(IntPtr gamecontroller) => INTERNAL_SDL_GameControllerGetJoystick(gamecontroller);
        
        [DllImport("SDL2", EntryPoint = "SDL_JoystickInstanceID", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_JoystickInstanceID(IntPtr joystick);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_JoystickInstanceID(IntPtr joystick) => INTERNAL_SDL_JoystickInstanceID(joystick);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetNumTouchDevices", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetNumTouchDevices();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetNumTouchDevices() => INTERNAL_SDL_GetNumTouchDevices();
        
        [DllImport("SDL2", EntryPoint = "SDL_GetTouchDevice", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int64 INTERNAL_SDL_GetTouchDevice(Int32 index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int64 SDL_GetTouchDevice(Int32 index) => INTERNAL_SDL_GetTouchDevice(index);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetNumTouchFingers", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetNumTouchFingers(Int64 touchID);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetNumTouchFingers(Int64 touchID) => INTERNAL_SDL_GetNumTouchFingers(touchID);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetTouchFinger", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_Finger* INTERNAL_SDL_GetTouchFinger(Int64 touchID, Int32 index);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Finger* SDL_GetTouchFinger(Int64 touchID, Int32 index) => INTERNAL_SDL_GetTouchFinger(touchID, index);
        
        [DllImport("SDL2", EntryPoint = "SDL_RecordGesture", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_RecordGesture(Int64 touchID);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_RecordGesture(Int64 touchID) => INTERNAL_SDL_RecordGesture(touchID);
        
        [DllImport("SDL2", EntryPoint = "SDL_SaveAllDollarTemplates", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_SaveAllDollarTemplates(IntPtr dst);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SaveAllDollarTemplates(IntPtr dst) => INTERNAL_SDL_SaveAllDollarTemplates(dst);
        
        [DllImport("SDL2", EntryPoint = "SDL_SaveDollarTemplate", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_SaveDollarTemplate(Int64 gestureID, IntPtr dst);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SaveDollarTemplate(Int64 gestureID, IntPtr dst) => INTERNAL_SDL_SaveDollarTemplate(gestureID, dst);
        
        [DllImport("SDL2", EntryPoint = "SDL_LoadDollarTemplates", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_LoadDollarTemplates(Int64 touchID, IntPtr src);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_LoadDollarTemplates(Int64 touchID, IntPtr src) => INTERNAL_SDL_LoadDollarTemplates(touchID, src);
        
        [DllImport("SDL2", EntryPoint = "SDL_StartTextInput", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_StartTextInput();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_StartTextInput() => INTERNAL_SDL_StartTextInput();
        
        [DllImport("SDL2", EntryPoint = "SDL_StopTextInput", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_StopTextInput();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_StopTextInput() => INTERNAL_SDL_StopTextInput();
        
        [DllImport("SDL2", EntryPoint = "SDL_SetTextInputRect", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetTextInputRect(SDL_Rect* rect);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetTextInputRect(SDL_Rect* rect) => INTERNAL_SDL_SetTextInputRect(rect);
        
        [DllImport("SDL2", EntryPoint = "SDL_HasClipboardText", CallingConvention = CallingConvention.Cdecl)]
        private static extern Boolean INTERNAL_SDL_HasClipboardText();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_HasClipboardText() => INTERNAL_SDL_HasClipboardText();
        
        [DllImport("SDL2", EntryPoint = "SDL_GetClipboardText", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetClipboardText();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GetClipboardText() => INTERNAL_SDL_GetClipboardText();
        
        [DllImport("SDL2", EntryPoint = "SDL_SetClipboardText", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_SetClipboardText(IntPtr text);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetClipboardText(IntPtr text) => INTERNAL_SDL_SetClipboardText(text);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetPowerInfo", CallingConvention = CallingConvention.Cdecl)]
        private static extern SDL_PowerState INTERNAL_SDL_GetPowerInfo(Int32* secs, Int32* pct);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_PowerState SDL_GetPowerInfo(Int32* secs, Int32* pct) => INTERNAL_SDL_GetPowerInfo(secs, pct);
        
        [DllImport("SDL2", EntryPoint = "SDL_ShowSimpleMessageBox", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_ShowSimpleMessageBox(UInt32 flags, [MarshalAs(UnmanagedType.LPStr)] String title, [MarshalAs(UnmanagedType.LPStr)] String message, IntPtr window);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_ShowSimpleMessageBox(UInt32 flags, String title, String message, IntPtr window) => INTERNAL_SDL_ShowSimpleMessageBox(flags, title, message, window);
        
        [DllImport("SDL2", EntryPoint = "SDL_SetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_SetWindowOpacity(IntPtr window, Single opacity);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetWindowOpacity(IntPtr window, Single opacity) => INTERNAL_SDL_SetWindowOpacity(window, opacity);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetWindowOpacity(IntPtr window, Single* opacity);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetWindowOpacity(IntPtr window, Single* opacity) => INTERNAL_SDL_GetWindowOpacity(window, opacity);
        
        [DllImport("SDL2", EntryPoint = "SDL_GameControllerAddMapping", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GameControllerAddMapping([MarshalAs(UnmanagedType.LPStr)] String mappingString);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GameControllerAddMapping(String mappingString) => INTERNAL_SDL_GameControllerAddMapping(mappingString);
        
        [DllImport("SDL2", EntryPoint = "SDL_GameControllerAddMappingsFromRW", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GameControllerAddMappingsFromRW(IntPtr rw, Int32 freerw);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GameControllerAddMappingsFromRW(IntPtr rw, Int32 freerw) => INTERNAL_SDL_GameControllerAddMappingsFromRW(rw, freerw);
        
        [DllImport("SDL2", EntryPoint = "SDL_GameControllerMapping", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GameControllerMapping(IntPtr gamecontroller);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IntPtr SDL_GameControllerMapping_Raw(IntPtr gamecontroller) => INTERNAL_SDL_GameControllerMapping(gamecontroller);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GameControllerMapping(IntPtr gamecontroller) => Marshal.PtrToStringAnsi(SDL_GameControllerMapping_Raw(gamecontroller));
        
        [DllImport("SDL2", EntryPoint = "SDL_GameControllerMappingForGUID", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GameControllerMappingForGUID(Guid guid);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private IntPtr SDL_GameControllerMappingForGUID_Raw(Guid guid) => INTERNAL_SDL_GameControllerMappingForGUID(guid);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GameControllerMappingForGUID(Guid guid) => Marshal.PtrToStringAnsi(SDL_GameControllerMappingForGUID_Raw(guid));
        
        [DllImport("SDL2", EntryPoint = "SDL_JoystickGetGUID", CallingConvention = CallingConvention.Cdecl)]
        private static extern Guid INTERNAL_SDL_JoystickGetGUID([MarshalAs(UnmanagedType.LPStr)] String pchGUID);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Guid SDL_JoystickGetGUID(String pchGUID) => INTERNAL_SDL_JoystickGetGUID(pchGUID);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetDisplayDPI", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_GetDisplayDPI(Int32 displayIndex, Single* ddpi, Single* hdpi, Single* vdpi);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetDisplayDPI(Int32 displayIndex, Single* ddpi, Single* hdpi, Single* vdpi) => INTERNAL_SDL_GetDisplayDPI(displayIndex, ddpi, hdpi, vdpi);
        
        [DllImport("SDL2", EntryPoint = "SDL_free", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_free(IntPtr mem);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_free(IntPtr mem) => INTERNAL_SDL_free(mem);
        
        [DllImport("SDL2", EntryPoint = "SDL_GetRelativeMouseMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern Boolean INTERNAL_SDL_GetRelativeMouseMode();
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_GetRelativeMouseMode() => INTERNAL_SDL_GetRelativeMouseMode();
        
        [DllImport("SDL2", EntryPoint = "SDL_SetRelativeMouseMode", CallingConvention = CallingConvention.Cdecl)]
        private static extern Int32 INTERNAL_SDL_SetRelativeMouseMode(Boolean enabled);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetRelativeMouseMode(Boolean enabled) => INTERNAL_SDL_SetRelativeMouseMode(enabled);
        
        [DllImport("SDL2", EntryPoint = "SDL_WarpMouseInWindow", CallingConvention = CallingConvention.Cdecl)]
        private static extern void INTERNAL_SDL_WarpMouseInWindow(IntPtr window, Int32 x, Int32 y);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_WarpMouseInWindow(IntPtr window, Int32 x, Int32 y) => INTERNAL_SDL_WarpMouseInWindow(window, x, y);
    }
#pragma warning restore 1591
}
