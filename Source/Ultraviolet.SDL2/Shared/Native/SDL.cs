using System;
using System.Runtime.InteropServices;
using System.Security;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

#pragma warning disable 1591

namespace Ultraviolet.SDL2.Native
{
    /// <summary>
    /// Contains bindings for native SDL2 function calls.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public unsafe static class SDL
    {
        private static readonly NativeLibrary lib = new NativeLibrary(
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.iOS ? "__Internal" :
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ? "SDL2.dll" : "libSDL2.so");
        
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 EventFilter(IntPtr userdata, SDL_Event* @event);

        public const UInt32 SDL_WINDOWPOS_CENTERED_MASK = 0x2FFF0000u;
        public const UInt32 SDL_WINDOWPOS_UNDEFINED_MASK = 0x1FFF0000u;

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GetErrorDelegate();
        private static readonly SDL_GetErrorDelegate pSDL_GetError = lib.LoadFunction<SDL_GetErrorDelegate>("SDL_GetError");
        public static String GetError() => Marshal.PtrToStringAnsi(pSDL_GetError());

        [MonoNativeFunctionWrapper]
        private delegate void SDL_ClearErrorDelegate();
        private static readonly SDL_ClearErrorDelegate pSDL_ClearError = lib.LoadFunction<SDL_ClearErrorDelegate>("SDL_ClearError");
        public static void ClearError() => pSDL_ClearError();

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_InitDelegate(SDL_Init flags);
        private static readonly SDL_InitDelegate pSDL_Init = lib.LoadFunction<SDL_InitDelegate>("SDL_Init");
        public static Int32 Init(SDL_Init flags) => pSDL_Init(flags);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_QuitDelegate();
        private static readonly SDL_QuitDelegate pSDL_Quit = lib.LoadFunction<SDL_QuitDelegate>("SDL_Quit");
        public static void Quit() => pSDL_Quit();

        [MonoNativeFunctionWrapper]
        private delegate void SDL_PumpEventsDelegate();
        private static readonly SDL_PumpEventsDelegate pSDL_PumpEvents = lib.LoadFunction<SDL_PumpEventsDelegate>("SDL_PumpEvents");
        public static void PumpEvents() => pSDL_PumpEvents();

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_PollEventDelegate(out SDL_Event @event);
        private static readonly SDL_PollEventDelegate pSDL_PollEvent = lib.LoadFunction<SDL_PollEventDelegate>("SDL_PollEvent");
        public static Int32 PollEvent(out SDL_Event @event) => pSDL_PollEvent(out @event);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetEventFilterDelegate(IntPtr filter, IntPtr userdata);
        private static readonly SDL_SetEventFilterDelegate pSDL_SetEventFilterDelegate = lib.LoadFunction<SDL_SetEventFilterDelegate>("SDL_SetEventFilter");
        public static void SetEventFilter(IntPtr filter, IntPtr userdata) => pSDL_SetEventFilterDelegate(filter, userdata);

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_CreateWindowDelegate(IntPtr title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags);
        private static readonly SDL_CreateWindowDelegate pSDL_CreateWindow = lib.LoadFunction<SDL_CreateWindowDelegate>("SDL_CreateWindow");
        public static IntPtr CreateWindow(String title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags)
        {
            var pTitle = IntPtr.Zero;
            try
            {
                pTitle = Marshal.StringToHGlobalAnsi(title);
                return pSDL_CreateWindow(pTitle, x, y, w, h, flags);
            }
            finally
            {
                if (pTitle != IntPtr.Zero)
                    Marshal.FreeHGlobal(pTitle);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_CreateWindowFromDelegate(IntPtr data);
        private static readonly SDL_CreateWindowFromDelegate pSDL_CreateWindowFrom = lib.LoadFunction<SDL_CreateWindowFromDelegate>("SDL_CreateWindowFrom");
        public static IntPtr CreateWindowFrom(IntPtr data) => pSDL_CreateWindowFrom(data);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_DestroyWindowDelegate(IntPtr window);
        private static readonly SDL_DestroyWindowDelegate pSDL_DestroyWindow = lib.LoadFunction<SDL_DestroyWindowDelegate>("SDL_DestroyWindow");
        public static void DestroyWindow(IntPtr window) => pSDL_DestroyWindow(window);

        [MonoNativeFunctionWrapper]
        private delegate UInt32 SDL_GetWindowIDDelegate(IntPtr window);
        private static readonly SDL_GetWindowIDDelegate pSDL_GetWindowID = lib.LoadFunction<SDL_GetWindowIDDelegate>("SDL_GetWindowID");
        public static UInt32 GetWindowID(IntPtr window) => pSDL_GetWindowID(window);

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GetWindowTitleDelegate(IntPtr window);
        private static readonly SDL_GetWindowTitleDelegate pSDL_GetWindowTitle = lib.LoadFunction<SDL_GetWindowTitleDelegate>("SDL_GetWindowTitle");
        public static String GetWindowTitle(IntPtr window) => Marshal.PtrToStringAnsi(pSDL_GetWindowTitle(window));

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetWindowTitleDelegate(IntPtr window, IntPtr title);
        private static readonly SDL_SetWindowTitleDelegate pSDL_SetWindowTitle = lib.LoadFunction<SDL_SetWindowTitleDelegate>("SDL_SetWindowTitle");
        public static void SetWindowTitle(IntPtr window, String title)
        {
            var pTitle = IntPtr.Zero;
            try
            {
                pTitle = Marshal.StringToHGlobalAnsi(title);
                pSDL_SetWindowTitle(window, pTitle);
            }
            finally
            {
                if (pTitle != IntPtr.Zero)
                    Marshal.FreeHGlobal(pTitle);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetWindowIconDelegate(IntPtr window, IntPtr icon);
        private static readonly SDL_SetWindowIconDelegate pSDL_SetWindowIcon = lib.LoadFunction<SDL_SetWindowIconDelegate>("SDL_SetWindowIcon");
        public static void SetWindowIcon(IntPtr window, IntPtr icon) => pSDL_SetWindowIcon(window, icon);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_GetWindowPositionDelegate(IntPtr window, out Int32 x, out Int32 y);
        private static readonly SDL_GetWindowPositionDelegate pSDL_GetWindowPosition = lib.LoadFunction<SDL_GetWindowPositionDelegate>("SDL_GetWindowPosition");
        public static void GetWindowPosition(IntPtr window, out Int32 x, out Int32 y) => pSDL_GetWindowPosition(window, out x, out y);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetWindowPositionDelegate(IntPtr window, Int32 x, Int32 y);
        private static readonly SDL_SetWindowPositionDelegate pSDL_SetWindowPosition = lib.LoadFunction<SDL_SetWindowPositionDelegate>("SDL_SetWindowPosition");
        public static void SetWindowPosition(IntPtr window, Int32 x, Int32 y) => pSDL_SetWindowPosition(window, x, y);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_GetWindowSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private static readonly SDL_GetWindowSizeDelegate pSDL_GetWindowSize = lib.LoadFunction<SDL_GetWindowSizeDelegate>("SDL_GetWindowSize");
        public static void GetWindowSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GetWindowSize(window, out w, out h);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetWindowSizeDelegate(IntPtr window, Int32 w, Int32 h);
        private static readonly SDL_SetWindowSizeDelegate pSDL_SetWindowSize = lib.LoadFunction<SDL_SetWindowSizeDelegate>("SDL_SetWindowSize");
        public static void SetWindowSize(IntPtr window, Int32 w, Int32 h) => pSDL_SetWindowSize(window, w, h);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_GetWindowMinimumSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private static readonly SDL_GetWindowMinimumSizeDelegate pSDL_GetWindowMinimumSize = lib.LoadFunction<SDL_GetWindowMinimumSizeDelegate>("SDL_GetWindowMinimumSize");
        public static void GetWindowMinimumSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GetWindowMinimumSize(window, out w, out h);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetWindowMinimumSizeDelegate(IntPtr window, Int32 w, Int32 h);
        private static readonly SDL_SetWindowMinimumSizeDelegate pSDL_SetWindowMinimumSize = lib.LoadFunction<SDL_SetWindowMinimumSizeDelegate>("SDL_SetWindowMinimumSize");
        public static void SetWindowMinimumSize(IntPtr window, Int32 w, Int32 h) => pSDL_SetWindowMinimumSize(window, w, h);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_GetWindowMaximumSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private static readonly SDL_GetWindowMaximumSizeDelegate pSDL_GetWindowMaximumSize = lib.LoadFunction<SDL_GetWindowMaximumSizeDelegate>("SDL_GetWindowMaximumSize");
        public static void GetWindowMaximumSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GetWindowMaximumSize(window, out w, out h);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetWindowMaximumSizeDelegate(IntPtr window, Int32 w, Int32 h);
        private static readonly SDL_SetWindowMaximumSizeDelegate pSDL_SetWindowMaximumSize = lib.LoadFunction<SDL_SetWindowMaximumSizeDelegate>("SDL_SetWindowMaximumSize");
        public static void SetWindowMaximumSize(IntPtr window, Int32 w, Int32 h) => pSDL_SetWindowMaximumSize(window, w, h);

        [MonoNativeFunctionWrapper]
        private delegate Boolean SDL_GetWindowGrabDelegate(IntPtr window);
        private static readonly SDL_GetWindowGrabDelegate pSDL_GetWindowGrab = lib.LoadFunction<SDL_GetWindowGrabDelegate>("SDL_GetWindowGrab");
        public static void GetWindowGrab(IntPtr window) => pSDL_GetWindowGrab(window);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetWindowGrabDelegate(IntPtr window, Boolean grabbed);
        private static readonly SDL_SetWindowGrabDelegate pSDL_SetWindowGrab = lib.LoadFunction<SDL_SetWindowGrabDelegate>("SDL_SetWindowGrab");
        public static void SetWindowGrab(IntPtr window, Boolean grabbed) => pSDL_SetWindowGrab(window, grabbed);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetWindowBorderedDelegate(IntPtr window, Boolean bordered);
        private static readonly SDL_SetWindowBorderedDelegate pSDL_SetWindowBordered = lib.LoadFunction<SDL_SetWindowBorderedDelegate>("SDL_SetWindowBordered");
        public static void SetWindowBordered(IntPtr window, Boolean bordered) => pSDL_SetWindowBordered(window, bordered);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_SetWindowFullscreenDelegate(IntPtr window, UInt32 flags);
        private static readonly SDL_SetWindowFullscreenDelegate pSDL_SetWindowFullscreen = lib.LoadFunction<SDL_SetWindowFullscreenDelegate>("SDL_SetWindowFullscreen");
        public static Int32 SetWindowFullscreen(IntPtr window, UInt32 flags) => pSDL_SetWindowFullscreen(window, flags);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_SetWindowDisplayModeDelegate(IntPtr window, SDL_DisplayMode* mode);
        private static readonly SDL_SetWindowDisplayModeDelegate pSDL_SetWindowDisplayMode = lib.LoadFunction<SDL_SetWindowDisplayModeDelegate>("SDL_SetWindowDisplayMode");
        public static Int32 SetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode) => pSDL_SetWindowDisplayMode(window, mode);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetWindowDisplayModeDelegate(IntPtr window, SDL_DisplayMode* mode);
        private static readonly SDL_GetWindowDisplayModeDelegate pSDL_GetWindowDisplayMode = lib.LoadFunction<SDL_GetWindowDisplayModeDelegate>("SDL_GetWindowDisplayMode");
        public static Int32 GetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode) => pSDL_GetWindowDisplayMode(window, mode);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetWindowDisplayIndexDelegate(IntPtr window);
        private static readonly SDL_GetWindowDisplayIndexDelegate pSDL_GetWindowDisplayModeIndex = lib.LoadFunction<SDL_GetWindowDisplayIndexDelegate>("SDL_GetWindowDisplayIndex");
        public static Int32 GetWindowDisplayIndex(IntPtr window) => pSDL_GetWindowDisplayModeIndex(window);

        [MonoNativeFunctionWrapper]
        private delegate SDL_WindowFlags SDL_GetWindowFlagsDelegate(IntPtr window);
        private static readonly SDL_GetWindowFlagsDelegate pSDL_GetWindowFlags = lib.LoadFunction<SDL_GetWindowFlagsDelegate>("SDL_GetWindowFlags");
        public static SDL_WindowFlags GetWindowFlags(IntPtr window) => pSDL_GetWindowFlags(window);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_ShowWindowDelegate(IntPtr window);
        private static readonly SDL_ShowWindowDelegate pSDL_ShowWindow = lib.LoadFunction<SDL_ShowWindowDelegate>("SDL_ShowWindow");
        public static void ShowWindow(IntPtr window) => pSDL_ShowWindow(window);
        
        [MonoNativeFunctionWrapper]
        private delegate void SDL_HideWindowDelegate(IntPtr window);
        private static readonly SDL_HideWindowDelegate pSDL_HideWindow = lib.LoadFunction<SDL_HideWindowDelegate>("SDL_HideWindow");
        public static void HideWindow(IntPtr window) => pSDL_HideWindow(window);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_MaximizeWindowDelegate(IntPtr window);
        private static readonly SDL_MaximizeWindowDelegate pSDL_MaximizeWindow = lib.LoadFunction<SDL_MaximizeWindowDelegate>("SDL_MaximizeWindow");
        public static void MaximizeWindow(IntPtr window) => pSDL_MaximizeWindow(window);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_MinimizeWindowDelegate(IntPtr window);
        private static readonly SDL_MinimizeWindowDelegate pSDL_MinimizeWindow = lib.LoadFunction<SDL_MinimizeWindowDelegate>("SDL_MinimizeWindow");
        public static void MinimizeWindow(IntPtr window) => pSDL_MinimizeWindow(window);
        
        [MonoNativeFunctionWrapper]
        private delegate void SDL_RestoreWindowDelegate(IntPtr window);
        private static readonly SDL_RestoreWindowDelegate pSDL_RestoreWindow = lib.LoadFunction<SDL_RestoreWindowDelegate>("SDL_RestoreWindow");
        public static void RestoreWindow(IntPtr window) => pSDL_RestoreWindow(window);

        [MonoNativeFunctionWrapper]
        private delegate Boolean SDL_GetWindowWMInfoDelegate(IntPtr window, SDL_SysWMinfo* info);
        private static readonly SDL_GetWindowWMInfoDelegate pSDL_GetWindowWMInfo = lib.LoadFunction<SDL_GetWindowWMInfoDelegate>("SDL_GetWindowWMInfo");
        public static Boolean GetWindowWMInfo(IntPtr window, SDL_SysWMinfo* info) => pSDL_GetWindowWMInfo(window, info);

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_RWFromFileDelegate(IntPtr file, IntPtr mode);
        private static readonly SDL_RWFromFileDelegate pSDL_RWFromFile = lib.LoadFunction<SDL_RWFromFileDelegate>("SDL_RWFromFile");
        public static IntPtr RWFromFile(String file, String mode)
        {
            var pFile = IntPtr.Zero;
            var pMode = IntPtr.Zero;
            try
            {
                pFile = Marshal.StringToHGlobalAnsi(file);
                pMode = Marshal.StringToHGlobalAnsi(mode);
                return pSDL_RWFromFile(pFile, pMode);
            }
            finally
            {
                if (pFile != IntPtr.Zero)
                    Marshal.FreeHGlobal(pFile);
                if (pMode != IntPtr.Zero)
                    Marshal.FreeHGlobal(pMode);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_RWFromMemDelegate(IntPtr mem, Int32 size);
        private static readonly SDL_RWFromMemDelegate pSDL_RWFromMem = lib.LoadFunction<SDL_RWFromMemDelegate>("SDL_RWFromMem");
        public static IntPtr SDL_RWFromMem(IntPtr mem, Int32 size) => pSDL_RWFromMem(mem, size);
        
        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_AllocRWDelegate();
        private static readonly SDL_AllocRWDelegate pSDL_AllocRW = lib.LoadFunction<SDL_AllocRWDelegate>("SDL_AllocRW");
        public static IntPtr AllocRW() => pSDL_AllocRW();

        [MonoNativeFunctionWrapper]
        private delegate void SDL_FreeRWDelegate(IntPtr area);
        private static readonly SDL_FreeRWDelegate pSDL_FreeRW = lib.LoadFunction<SDL_FreeRWDelegate>("SDL_FreeRW");
        public static void FreeRW(IntPtr area) => pSDL_FreeRW(area);

        [MonoNativeFunctionWrapper]
        private delegate SDL_Surface* SDL_LoadBMP_RWDelegate(IntPtr src, Int32 freesrc);
        private static readonly SDL_LoadBMP_RWDelegate pSDL_LoadBMP_RW = lib.LoadFunction<SDL_LoadBMP_RWDelegate>("SDL_LoadBMP_RW");
        public static SDL_Surface* LoadBMP_RW(IntPtr src, Int32 freesrc) => pSDL_LoadBMP_RW(src, freesrc);
        public static SDL_Surface* LoadBMP(String file) => LoadBMP_RW(RWFromFile(file, "r"), 1);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_SaveBMP_RWDelegate(SDL_Surface* surface, IntPtr dst, Int32 freedst);
        private static readonly SDL_SaveBMP_RWDelegate pSDL_SaveBMP_RW = lib.LoadFunction<SDL_SaveBMP_RWDelegate>("SDL_SaveBMP_RW");
        public static Int32 SaveBMP_RW(SDL_Surface* surface, IntPtr dst, Int32 freedst) => pSDL_SaveBMP_RW(surface, dst, freedst);
        public static Int32 SaveBMP(SDL_Surface* surface, String file) => SaveBMP_RW(surface, RWFromFile(file, "wb"), 1);

        [MonoNativeFunctionWrapper]
        private delegate UInt32 SDL_GetMouseStateDelegate(out Int32 x, out Int32 y);
        private static readonly SDL_GetMouseStateDelegate pSDL_GetMouseState = lib.LoadFunction<SDL_GetMouseStateDelegate>("SDL_GetMouseState");
        public static UInt32 GetMouseState(out Int32 x, out Int32 y) => pSDL_GetMouseState(out x, out y);

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GetKeyboardStateDelegate(out Int32 numkeys);
        private static readonly SDL_GetKeyboardStateDelegate pSDL_GetKeyboardState = lib.LoadFunction<SDL_GetKeyboardStateDelegate>("SDL_GetKeyboardState");
        public static IntPtr GetKeyboardState(out Int32 numkeys) => pSDL_GetKeyboardState(out numkeys);

        [MonoNativeFunctionWrapper]
        private delegate SDL_Scancode SDL_GetScancodeFromKeyDelegate(SDL_Keycode keycode);
        private static readonly SDL_GetScancodeFromKeyDelegate pSDL_GetScancodeFromKey = lib.LoadFunction<SDL_GetScancodeFromKeyDelegate>("SDL_GetScancodeFromKey");
        public static SDL_Scancode GetScancodeFromKey(SDL_Keycode keycode) => pSDL_GetScancodeFromKey(keycode);

        [MonoNativeFunctionWrapper]
        private delegate SDL_Keymod SDL_GetModStateDelegate();
        private static readonly SDL_GetModStateDelegate pSDL_GetModState = lib.LoadFunction<SDL_GetModStateDelegate>("SDL_GetModState");
        public static SDL_Keymod GetModState() => pSDL_GetModState();

        [MonoNativeFunctionWrapper]
        private delegate Boolean SDL_SetHintDelegate(IntPtr name, IntPtr value);
        private static readonly SDL_SetHintDelegate pSDL_SetHint = lib.LoadFunction<SDL_SetHintDelegate>("SDL_SetHint");
        public static Boolean SetHint(String name, String value)
        {
            var pName = IntPtr.Zero;
            var pValue = IntPtr.Zero;
            try
            {
                pName = Marshal.StringToHGlobalAnsi(name);
                pValue = Marshal.StringToHGlobalAnsi(value);
                return pSDL_SetHint(pName, pValue);
            }
            finally
            {
                if (pName != IntPtr.Zero)
                    Marshal.FreeHGlobal(pName);
                if (pValue != IntPtr.Zero)
                    Marshal.FreeHGlobal(pValue);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate SDL_Surface* SDL_CreateRGBSurfaceDelegate(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 Amask);
        private static readonly SDL_CreateRGBSurfaceDelegate pSDL_CreateRGBSurface = lib.LoadFunction<SDL_CreateRGBSurfaceDelegate>("SDL_CreateRGBSurface");
        public static SDL_Surface* CreateRGBSurface(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 Amask) => pSDL_CreateRGBSurface(flags, width, height, depth, Rmask, Gmask, Bmask, Amask);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_FreeSurfaceDelegate(SDL_Surface* surface);
        private static readonly SDL_FreeSurfaceDelegate pSDL_FreeSurface = lib.LoadFunction<SDL_FreeSurfaceDelegate>("SDL_FreeSurface");
        public static void FreeSurface(SDL_Surface* surface) => pSDL_FreeSurface(surface);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_LockSurfaceDelegate(SDL_Surface* surface);
        private static readonly SDL_LockSurfaceDelegate pSDL_LockSurface = lib.LoadFunction<SDL_LockSurfaceDelegate>("SDL_LockSurface");
        public static Int32 LockSurface(SDL_Surface* surface) => pSDL_LockSurface(surface);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_UnlockSurfaceDelegate(SDL_Surface* surface);
        private static readonly SDL_UnlockSurfaceDelegate pSDL_UnlockSurface = lib.LoadFunction<SDL_UnlockSurfaceDelegate>("SDL_UnlockSurface");
        public static void UnlockSurface(SDL_Surface* surface) => pSDL_UnlockSurface(surface);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_UpperBlitDelegate(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect);
        private static readonly SDL_UpperBlitDelegate pSDL_UpperBlit = lib.LoadFunction<SDL_UpperBlitDelegate>("SDL_UpperBlit");
        public static Int32 BlitSurface(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect) => pSDL_UpperBlit(src, srcrect, dst, dstrect);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_UpperBlitScaledDelegate(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect);
        private static readonly SDL_UpperBlitScaledDelegate pSDL_UpperBlitScaled = lib.LoadFunction<SDL_UpperBlitScaledDelegate>("SDL_UpperBlitScaled");
        public static Int32 BlitScaled(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect) => pSDL_UpperBlitScaled(src, srcrect, dst, dstrect);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_SetSurfaceBlendModeDelegate(SDL_Surface* surface, SDL_BlendMode blendMode);
        private static readonly SDL_SetSurfaceBlendModeDelegate pSDL_SetSurfaceBlendMode = lib.LoadFunction<SDL_SetSurfaceBlendModeDelegate>("SDL_SetSurfaceBlendMode");
        public static Int32 SetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode blendMode) => pSDL_SetSurfaceBlendMode(surface, blendMode);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetSurfaceBlendModeDelegate(SDL_Surface* surface, SDL_BlendMode* blendMode);
        private static readonly SDL_GetSurfaceBlendModeDelegate pSDL_GetSurfaceBlendMode = lib.LoadFunction<SDL_GetSurfaceBlendModeDelegate>("SDL_GetSurfaceBlendMode");
        public static Int32 GetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode* blendMode) => pSDL_GetSurfaceBlendMode(surface, blendMode);

        [MonoNativeFunctionWrapper]
        private delegate SDL_Cursor* SDL_CreateColorCursorDelegate(SDL_Surface* surface, Int32 hot_x, Int32 hot_y);
        private static readonly SDL_CreateColorCursorDelegate pSDL_CreateColorCursor = lib.LoadFunction<SDL_CreateColorCursorDelegate>("SDL_CreateColorCursor");
        public static SDL_Cursor* CreateColorCursor(SDL_Surface* surface, Int32 hot_x, Int32 hot_y) => pSDL_CreateColorCursor(surface, hot_x, hot_y);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_FreeCursorDelegate(SDL_Cursor* cursor);
        private static readonly SDL_FreeCursorDelegate pSDL_FreeCursor = lib.LoadFunction<SDL_FreeCursorDelegate>("SDL_FreeCursor");
        public static void FreeCursor(SDL_Cursor* cursor) => pSDL_FreeCursor(cursor);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_ShowCursorDelegate(Int32 toggle);
        private static readonly SDL_ShowCursorDelegate pSDL_ShowCursor = lib.LoadFunction<SDL_ShowCursorDelegate>("SDL_ShowCursor");
        public static Int32 ShowCursor(Int32 toggle) => pSDL_ShowCursor(toggle);

        [MonoNativeFunctionWrapper]
        private delegate SDL_Cursor* SDL_GetCursorDelegate();
        private static readonly SDL_GetCursorDelegate pSDL_GetCursor = lib.LoadFunction<SDL_GetCursorDelegate>("SDL_GetCursor");
        public static SDL_Cursor* GetCursor() => pSDL_GetCursor();

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetCursorDelegate(SDL_Cursor* cursor);
        private static readonly SDL_SetCursorDelegate pSDL_SetCursor = lib.LoadFunction<SDL_SetCursorDelegate>("SDL_SetCursor");
        public static void SetCursor(SDL_Cursor* cursor) => pSDL_SetCursor(cursor);

        [MonoNativeFunctionWrapper]
        private delegate SDL_Cursor* SDL_GetDefaultCursorDelegate();
        private static readonly SDL_GetDefaultCursorDelegate pSDL_GetDefaultCursor = lib.LoadFunction<SDL_GetDefaultCursorDelegate>("SDL_GetDefaultCursor");
        public static SDL_Cursor* GetDefaultCursor() => pSDL_GetDefaultCursor();

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetNumVideoDisplaysDelegate();
        private static readonly SDL_GetNumVideoDisplaysDelegate pSDL_GetNumVideoDisplays = lib.LoadFunction<SDL_GetNumVideoDisplaysDelegate>("SDL_GetNumVideoDisplays");
        public static Int32 GetNumVideoDisplays() => pSDL_GetNumVideoDisplays();

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GetDisplayNameDelegate(Int32 displayIndex);
        private static readonly SDL_GetDisplayNameDelegate pSDL_GetDisplayName = lib.LoadFunction<SDL_GetDisplayNameDelegate>("SDL_GetDisplayName");
        public static String GetDisplayName(Int32 displayIndex) => Marshal.PtrToStringAnsi(pSDL_GetDisplayName(displayIndex));

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetDisplayBoundsDelegate(Int32 displayIndex, SDL_Rect* rect);
        private static readonly SDL_GetDisplayBoundsDelegate pSDL_GetDisplayBounds = lib.LoadFunction<SDL_GetDisplayBoundsDelegate>("SDL_GetDisplayBounds");
        public static Int32 GetDisplayBounds(Int32 displayIndex, SDL_Rect* rect) => pSDL_GetDisplayBounds(displayIndex, rect);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetNumDisplayModesDelegate(Int32 displayIndex);
        private static readonly SDL_GetNumDisplayModesDelegate pSDL_GetNumDisplayModes = lib.LoadFunction<SDL_GetNumDisplayModesDelegate>("SDL_GetNumDisplayModes");
        public static Int32 GetNumDisplayModes(Int32 displayIndex) => pSDL_GetNumDisplayModes(displayIndex);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetDisplayModeDelegate(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode);
        private static readonly SDL_GetDisplayModeDelegate pSDL_GetDisplayMode = lib.LoadFunction<SDL_GetDisplayModeDelegate>("SDL_GetDisplayMode");
        public static Int32 GetDisplayMode(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode) => pSDL_GetDisplayMode(displayIndex, modeIndex, mode);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetCurrentDisplayModeDelegate(Int32 displayIndex, SDL_DisplayMode* mode);
        private static readonly SDL_GetCurrentDisplayModeDelegate pSDL_GetCurrentDisplayMode = lib.LoadFunction<SDL_GetCurrentDisplayModeDelegate>("SDL_GetCurrentDisplayMode");
        public static Int32 GetCurrentDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode) => pSDL_GetCurrentDisplayMode(displayIndex, mode);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetDesktopDisplayModeDelegate(Int32 displayIndex, SDL_DisplayMode* mode);
        private static readonly SDL_GetDesktopDisplayModeDelegate pSDL_GetDesktopDisplayMode = lib.LoadFunction<SDL_GetDesktopDisplayModeDelegate>("SDL_GetDesktopDisplayMode");
        public static Int32 GetDesktopDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode) => pSDL_GetDesktopDisplayMode(displayIndex, mode);

        [MonoNativeFunctionWrapper]
        private delegate SDL_DisplayMode* SDL_GetClosestDisplayModeDelegate(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest);
        private static readonly SDL_GetClosestDisplayModeDelegate pSDL_GetClosestDisplayMode = lib.LoadFunction<SDL_GetClosestDisplayModeDelegate>("SDL_GetClosestDisplayMode");
        public static SDL_DisplayMode* GetClosestDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest) => pSDL_GetClosestDisplayMode(displayIndex, mode, closest);

        [MonoNativeFunctionWrapper]
        private delegate Boolean SDL_PixelFormatEnumToMasksDelegate(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask);
        private static readonly SDL_PixelFormatEnumToMasksDelegate pSDL_PixelFormatEnumToMasks = lib.LoadFunction<SDL_PixelFormatEnumToMasksDelegate>("SDL_PixelFormatEnumToMasks");
        public static Boolean PixelFormatEnumToMasks(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask) => pSDL_PixelFormatEnumToMasks(format, bpp, Rmask, Gmask, Bmask, Amask);

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GL_GetProcAddressDelegate(IntPtr proc);
        private static readonly SDL_GL_GetProcAddressDelegate pSDL_GL_GetProcAddress = lib.LoadFunction<SDL_GL_GetProcAddressDelegate>("SDL_GL_GetProcAddress");
        public static IntPtr GL_GetProcAddress(String proc)
        {
            var pProc = IntPtr.Zero;
            try
            {
                pProc = Marshal.StringToHGlobalAnsi(proc);
                return pSDL_GL_GetProcAddress(pProc);
            }
            finally
            {
                if (pProc != IntPtr.Zero)
                    Marshal.FreeHGlobal(pProc);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GL_CreateContextDelegate(IntPtr window);
        private static readonly SDL_GL_CreateContextDelegate pSDL_GL_CreateContext = lib.LoadFunction<SDL_GL_CreateContextDelegate>("SDL_GL_CreateContext");
        public static IntPtr GL_CreateContext(IntPtr window) => pSDL_GL_CreateContext(window);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_GL_DeleteContextDelegate(IntPtr context);
        private static readonly SDL_GL_DeleteContextDelegate pSDL_GL_DeleteContext = lib.LoadFunction<SDL_GL_DeleteContextDelegate>("SDL_GL_DeleteContext");
        public static void GL_DeleteContext(IntPtr context) => pSDL_GL_DeleteContext(context);

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GL_GetCurrentContextDelegate();
        private static readonly SDL_GL_GetCurrentContextDelegate pSDL_GL_GetCurrentContext = lib.LoadFunction<SDL_GL_GetCurrentContextDelegate>("SDL_GL_GetCurrentContext");
        public static IntPtr GL_GetCurrentContext() => pSDL_GL_GetCurrentContext();

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GL_MakeCurrentDelegate(IntPtr window, IntPtr context);
        private static readonly SDL_GL_MakeCurrentDelegate pSDL_GL_MakeCurrent = lib.LoadFunction<SDL_GL_MakeCurrentDelegate>("SDL_GL_MakeCurrent");
        public static Int32 GL_MakeCurrent(IntPtr window, IntPtr context) => pSDL_GL_MakeCurrent(window, context);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GL_SetAttributeDelegate(SDL_GLattr attr, Int32 value);
        private static readonly SDL_GL_SetAttributeDelegate pSDL_GL_SetAttribute = lib.LoadFunction<SDL_GL_SetAttributeDelegate>("SDL_GL_SetAttribute");
        public static Int32 GL_SetAttribute(SDL_GLattr attr, Int32 value) => pSDL_GL_SetAttribute(attr, value);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GL_GetAttributeDelegate(SDL_GLattr attr, Int32* value);
        private static readonly SDL_GL_GetAttributeDelegate pSDL_GL_GetAttribute = lib.LoadFunction<SDL_GL_GetAttributeDelegate>("SDL_GL_GetAttribute");
        public static Int32 GL_GetAttribute(SDL_GLattr attr, Int32* value) => pSDL_GL_GetAttribute(attr, value);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_GL_SwapWindowDelegate(IntPtr window);
        private static readonly SDL_GL_SwapWindowDelegate pSDL_GL_SwapWindow = lib.LoadFunction<SDL_GL_SwapWindowDelegate>("SDL_GL_SwapWindow");
        public static void GL_SwapWindow(IntPtr window) => pSDL_GL_SwapWindow(window);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GL_SetSwapIntervalDelegate(Int32 interval);
        private static readonly SDL_GL_SetSwapIntervalDelegate pSDL_GL_SetSwapInterval = lib.LoadFunction<SDL_GL_SetSwapIntervalDelegate>("SDL_GL_SetSwapInterval");
        public static Int32 GL_SetSwapInterval(Int32 interval) => pSDL_GL_SetSwapInterval(interval);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_GL_GetDrawableSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private static readonly SDL_GL_GetDrawableSizeDelegate pSDL_GL_GetDrawableSize = lib.LoadFunction<SDL_GL_GetDrawableSizeDelegate>("SDL_GL_GetDrawableSize");
        public static void GL_GetDrawableSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GL_GetDrawableSize(window, out w, out h);
        public static void GetDrawableSize(IntPtr window, Boolean opengl, out Int32 w, out Int32 h)
        {
            if (opengl)
            {
                pSDL_GL_GetDrawableSize(window, out w, out h);
            }
            else
            {
                pSDL_GetWindowSize(window, out w, out h);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_NumJoysticksDelegate();
        private static readonly SDL_NumJoysticksDelegate pSDL_NumJoysticks = lib.LoadFunction<SDL_NumJoysticksDelegate>("SDL_NumJoysticks");
        public static Int32 NumJoysticks() => pSDL_NumJoysticks();

        [MonoNativeFunctionWrapper]
        private delegate Boolean SDL_IsGameControllerDelegate(Int32 joystick_index);
        private static readonly SDL_IsGameControllerDelegate pSDL_IsGameController = lib.LoadFunction<SDL_IsGameControllerDelegate>("SDL_IsGameController");
        public static Boolean IsGameController(Int32 joystick_index) => pSDL_IsGameController(joystick_index);

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GameControllerOpenDelegate(Int32 index);
        private static readonly SDL_GameControllerOpenDelegate pSDL_GameControllerOpen = lib.LoadFunction<SDL_GameControllerOpenDelegate>("SDL_GameControllerOpen");
        public static IntPtr GameControllerOpen(Int32 index) => pSDL_GameControllerOpen(index);

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GameControllerCloseDelegate(IntPtr gamecontroller);
        private static readonly SDL_GameControllerCloseDelegate pSDL_GameControllerClose = lib.LoadFunction<SDL_GameControllerCloseDelegate>("SDL_GameControllerClose");
        public static IntPtr GameControllerClose(IntPtr gamecontroller) => pSDL_GameControllerClose(gamecontroller);

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GameControllerNameForIndexDelegate(Int32 joystick_index);
        private static readonly SDL_GameControllerNameForIndexDelegate pSDL_GameControllerNameForIndex = lib.LoadFunction<SDL_GameControllerNameForIndexDelegate>("SDL_GameControllerNameForIndex");
        public static String GameControllerNameForIndex(Int32 joystick_index) => Marshal.PtrToStringAnsi(pSDL_GameControllerNameForIndex(joystick_index));

        [MonoNativeFunctionWrapper]
        private delegate Boolean SDL_GameControllerGetButtonDelegate(IntPtr gamecontroller, SDL_GameControllerButton button);
        private static readonly SDL_GameControllerGetButtonDelegate pSDL_GameControllerGetButton = lib.LoadFunction<SDL_GameControllerGetButtonDelegate>("SDL_GameControllerGetButton");
        public static Boolean GameControllerGetButton(IntPtr gamecontroller, SDL_GameControllerButton button) => pSDL_GameControllerGetButton(gamecontroller, button);

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GameControllerGetJoystickDelegate(IntPtr gamecontroller);
        private static readonly SDL_GameControllerGetJoystickDelegate pSDL_GameControllerGetJoystick = lib.LoadFunction<SDL_GameControllerGetJoystickDelegate>("SDL_GameControllerGetJoystick");
        public static IntPtr GameControllerGetJoystick(IntPtr gamecontroller) => pSDL_GameControllerGetJoystick(gamecontroller);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_JoystickInstanceIDDelegate(IntPtr joystick);
        private static readonly SDL_JoystickInstanceIDDelegate pSDL_JoystickInstanceID = lib.LoadFunction<SDL_JoystickInstanceIDDelegate>("SDL_JoystickInstanceID");
        public static Int32 JoystickInstanceID(IntPtr joystick) => pSDL_JoystickInstanceID(joystick);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetNumTouchDevicesDelegate();
        private static readonly SDL_GetNumTouchDevicesDelegate pSDL_GetNumTouchDevices = lib.LoadFunction<SDL_GetNumTouchDevicesDelegate>("SDL_GetNumTouchDevices");
        public static Int32 GetNumTouchDevices() => pSDL_GetNumTouchDevices();

        [MonoNativeFunctionWrapper]
        private delegate Int64 SDL_GetTouchDeviceDelegate(Int32 index);
        private static readonly SDL_GetTouchDeviceDelegate pSDL_GetTouchDevice = lib.LoadFunction<SDL_GetTouchDeviceDelegate>("SDL_GetTouchDevice");
        public static Int64 GetTouchDevice(Int32 index) => pSDL_GetTouchDevice(index);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetNumTouchFingersDelegate(Int64 touchID);
        private static readonly SDL_GetNumTouchFingersDelegate pSDL_GetNumTouchFingers = lib.LoadFunction<SDL_GetNumTouchFingersDelegate>("SDL_GetNumTouchFingers");
        public static Int32 GetNumTouchFingers(Int64 touchID) => pSDL_GetNumTouchFingers(touchID);

        [MonoNativeFunctionWrapper]
        private delegate SDL_Finger* SDL_GetTouchFingerDelegate(Int64 touchID, Int32 index);
        private static readonly SDL_GetTouchFingerDelegate pSDL_GetTouchFinger = lib.LoadFunction<SDL_GetTouchFingerDelegate>("SDL_GetTouchFinger");
        public static SDL_Finger* GetTouchFinger(Int64 touchID, Int32 index) => pSDL_GetTouchFinger(touchID, index);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_RecordGestureDelegate(Int64 touchID);
        private static readonly SDL_RecordGestureDelegate pSDL_RecordGesture = lib.LoadFunction<SDL_RecordGestureDelegate>("SDL_RecordGesture");
        public static Int32 RecordGesture(Int64 touchID) => pSDL_RecordGesture(touchID);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_SaveAllDollarTemplatesDelegate(IntPtr dst);
        private static readonly SDL_SaveAllDollarTemplatesDelegate pSDL_SaveAllDollarTemplates = lib.LoadFunction<SDL_SaveAllDollarTemplatesDelegate>("SDL_SaveAllDollarTemplates");
        public static Int32 SaveAllDollarTemplates(IntPtr dst) => pSDL_SaveAllDollarTemplates(dst);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_SaveDollarTemplateDelegate(Int64 gestureID, IntPtr dst);
        private static readonly SDL_SaveDollarTemplateDelegate pSDL_SaveDollarTemplate = lib.LoadFunction<SDL_SaveDollarTemplateDelegate>("SDL_SaveDollarTemplate");
        public static Int32 SaveDollarTemplate(Int64 gestureID, IntPtr dst) => pSDL_SaveDollarTemplate(gestureID, dst);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_LoadDollarTemplatesDelegate(Int64 touchID, IntPtr src);
        private static readonly SDL_LoadDollarTemplatesDelegate pSDL_LoadDollarTemplates = lib.LoadFunction<SDL_LoadDollarTemplatesDelegate>("SDL_LoadDollarTemplates");
        public static Int32 LoadDollarTemplates(Int64 touchID, IntPtr src) => pSDL_LoadDollarTemplates(touchID, src);

        [MonoNativeFunctionWrapper]
        private delegate void SDL_StartTextInputDelegate();
        private static readonly SDL_StartTextInputDelegate pSDL_StartTextInput = lib.LoadFunction<SDL_StartTextInputDelegate>("SDL_StartTextInput");
        public static void StartTextInput() => pSDL_StartTextInput();

        [MonoNativeFunctionWrapper]
        private delegate void SDL_StopTextInputDelegate();
        private static readonly SDL_StopTextInputDelegate pSDL_StopTextInput = lib.LoadFunction<SDL_StopTextInputDelegate>("SDL_StopTextInput");
        public static void StopTextInput() => pSDL_StopTextInput();

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetTextInputRectDelegate(SDL_Rect* rect);
        private static readonly SDL_SetTextInputRectDelegate pSDL_SetTextInputRect = lib.LoadFunction<SDL_SetTextInputRectDelegate>("SDL_SetTextInputRect");
        public static void SetTextInputRect(SDL_Rect* rect) => pSDL_SetTextInputRect(rect);

        [MonoNativeFunctionWrapper]
        private delegate Boolean SDL_HasClipboardTextDelegate();
        private static readonly SDL_HasClipboardTextDelegate pSDL_HasClipboardText = lib.LoadFunction<SDL_HasClipboardTextDelegate>("SDL_HasClipboardText");
        public static Boolean HasClipboardText() => pSDL_HasClipboardText();

        [MonoNativeFunctionWrapper]
        private delegate IntPtr SDL_GetClipboardTextDelegate();
        private static readonly SDL_GetClipboardTextDelegate pSDL_GetClipboardText = lib.LoadFunction<SDL_GetClipboardTextDelegate>("SDL_GetClipboardText");
        public static String GetClipboardText()
        {
            var ptr = pSDL_GetClipboardText();
            return (ptr == IntPtr.Zero) ? null : Marshal.PtrToStringAnsi(ptr);
        }

        [MonoNativeFunctionWrapper]
        private delegate void SDL_SetClipboardTextDelegate(IntPtr text);
        private static readonly SDL_SetClipboardTextDelegate pSDL_SetClipboardText = lib.LoadFunction<SDL_SetClipboardTextDelegate>("SDL_SetClipboardText");
        public static void SetClipboardText(String text)
        {
            var pText = IntPtr.Zero;
            try
            {
                pText = Marshal.StringToHGlobalAnsi(text);
                pSDL_SetClipboardText(pText);
            }
            finally
            {
                if (pText != IntPtr.Zero)
                    Marshal.FreeHGlobal(pText);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate SDL_PowerState SDL_GetPowerInfoDelegate(int* secs, int* pct);
        private static readonly SDL_GetPowerInfoDelegate pSDL_GetPowerInfo = lib.LoadFunction<SDL_GetPowerInfoDelegate>("SDL_GetPowerInfo");
        public static SDL_PowerState GetPowerInfo(int* secs, int* pct) => pSDL_GetPowerInfo(secs, pct);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_ShowSimpleMessageBoxDelegate(UInt32 flags, IntPtr title, IntPtr message, IntPtr window);
        private static readonly SDL_ShowSimpleMessageBoxDelegate pSDL_ShowSimpleMessageBox = lib.LoadFunction<SDL_ShowSimpleMessageBoxDelegate>("SDL_ShowSimpleMessageBox");
        public static Int32 ShowSimpleMessageBox(UInt32 flags, String title, String message, IntPtr window)
        {
            var pTitle = IntPtr.Zero;
            var pMessage = IntPtr.Zero;
            try
            {
                pTitle = Marshal.StringToHGlobalAnsi(title);
                pMessage = Marshal.StringToHGlobalAnsi(message);
                return pSDL_ShowSimpleMessageBox(flags, pTitle, pMessage, window);
            }
            finally
            {
                if (pTitle != IntPtr.Zero)
                    Marshal.FreeHGlobal(pTitle);
                if (pMessage != IntPtr.Zero)
                    Marshal.FreeHGlobal(pMessage);
            }
        }

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_SetWindowOpacityDelegate(IntPtr window, Single opacity);
        private static readonly SDL_SetWindowOpacityDelegate pSDL_SetWindowOpacity = lib.LoadFunction<SDL_SetWindowOpacityDelegate>("SDL_SetWindowOpacity");
        public static Int32 SetWindowOpacity(IntPtr window, Single opacity) => pSDL_SetWindowOpacity(window, opacity);

        [MonoNativeFunctionWrapper]
        private delegate Int32 SDL_GetWindowOpacityDelegate(IntPtr window, Single* opacity);
        private static readonly SDL_GetWindowOpacityDelegate pSDL_GetWindowOpacity = lib.LoadFunction<SDL_GetWindowOpacityDelegate>("SDL_GetWindowOpacity");
        public static Int32 GetWindowOpacity(IntPtr window, Single* opacity) => pSDL_GetWindowOpacity(window, opacity);
    }
}
