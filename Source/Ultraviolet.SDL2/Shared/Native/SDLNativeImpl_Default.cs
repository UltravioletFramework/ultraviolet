using System;
using System.Security;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class SDLNativeImpl_Default : SDLNativeImpl
    {
        private static readonly NativeLibrary lib;
        
        static SDLNativeImpl_Default()
        {
            switch (UltravioletPlatformInfo.CurrentPlatform)
            {
                case UltravioletPlatform.Linux:
                    lib = new NativeLibrary("libSDL2");
                    break;
                case UltravioletPlatform.macOS:
                    lib = new NativeLibrary("libSDL2");
                    break;
                default:
                    lib = new NativeLibrary("SDL2");
                    break;
            }
        }
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GetError_RawDelegate();
        private readonly SDL_GetError_RawDelegate pSDL_GetError_Raw = lib.LoadFunction<SDL_GetError_RawDelegate>("SDL_GetError");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GetError() => Marshal.PtrToStringAnsi(pSDL_GetError_Raw());
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_ClearErrorDelegate();
        private readonly SDL_ClearErrorDelegate pSDL_ClearError = lib.LoadFunction<SDL_ClearErrorDelegate>("SDL_ClearError");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_ClearError() => pSDL_ClearError();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_InitDelegate(SDL_Init flags);
        private readonly SDL_InitDelegate pSDL_Init = lib.LoadFunction<SDL_InitDelegate>("SDL_Init");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_Init(SDL_Init flags) => pSDL_Init(flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_QuitDelegate();
        private readonly SDL_QuitDelegate pSDL_Quit = lib.LoadFunction<SDL_QuitDelegate>("SDL_Quit");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_Quit() => pSDL_Quit();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_PumpEventsDelegate();
        private readonly SDL_PumpEventsDelegate pSDL_PumpEvents = lib.LoadFunction<SDL_PumpEventsDelegate>("SDL_PumpEvents");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_PumpEvents() => pSDL_PumpEvents();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_PollEventDelegate(out SDL_Event @event);
        private readonly SDL_PollEventDelegate pSDL_PollEvent = lib.LoadFunction<SDL_PollEventDelegate>("SDL_PollEvent");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_PollEvent(out SDL_Event @event) => pSDL_PollEvent(out @event);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetEventFilterDelegate(IntPtr filter, IntPtr userdata);
        private readonly SDL_SetEventFilterDelegate pSDL_SetEventFilter = lib.LoadFunction<SDL_SetEventFilterDelegate>("SDL_SetEventFilter");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetEventFilter(IntPtr filter, IntPtr userdata) => pSDL_SetEventFilter(filter, userdata);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_CreateWindowDelegate(String title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags);
        private readonly SDL_CreateWindowDelegate pSDL_CreateWindow = lib.LoadFunction<SDL_CreateWindowDelegate>("SDL_CreateWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_CreateWindow(String title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags) => pSDL_CreateWindow(title, x, y, w, h, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_CreateWindowFromDelegate(IntPtr data);
        private readonly SDL_CreateWindowFromDelegate pSDL_CreateWindowFrom = lib.LoadFunction<SDL_CreateWindowFromDelegate>("SDL_CreateWindowFrom");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_CreateWindowFrom(IntPtr data) => pSDL_CreateWindowFrom(data);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_DestroyWindowDelegate(IntPtr window);
        private readonly SDL_DestroyWindowDelegate pSDL_DestroyWindow = lib.LoadFunction<SDL_DestroyWindowDelegate>("SDL_DestroyWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_DestroyWindow(IntPtr window) => pSDL_DestroyWindow(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UInt32 SDL_GetWindowIDDelegate(IntPtr window);
        private readonly SDL_GetWindowIDDelegate pSDL_GetWindowID = lib.LoadFunction<SDL_GetWindowIDDelegate>("SDL_GetWindowID");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 SDL_GetWindowID(IntPtr window) => pSDL_GetWindowID(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GetWindowTitle_RawDelegate(IntPtr window);
        private readonly SDL_GetWindowTitle_RawDelegate pSDL_GetWindowTitle_Raw = lib.LoadFunction<SDL_GetWindowTitle_RawDelegate>("SDL_GetWindowTitle");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GetWindowTitle(IntPtr window) => Marshal.PtrToStringAnsi(pSDL_GetWindowTitle_Raw(window));
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowTitleDelegate(IntPtr window, String title);
        private readonly SDL_SetWindowTitleDelegate pSDL_SetWindowTitle = lib.LoadFunction<SDL_SetWindowTitleDelegate>("SDL_SetWindowTitle");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowTitle(IntPtr window, String title) => pSDL_SetWindowTitle(window, title);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowIconDelegate(IntPtr window, IntPtr icon);
        private readonly SDL_SetWindowIconDelegate pSDL_SetWindowIcon = lib.LoadFunction<SDL_SetWindowIconDelegate>("SDL_SetWindowIcon");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowIcon(IntPtr window, IntPtr icon) => pSDL_SetWindowIcon(window, icon);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GetWindowPositionDelegate(IntPtr window, out Int32 x, out Int32 y);
        private readonly SDL_GetWindowPositionDelegate pSDL_GetWindowPosition = lib.LoadFunction<SDL_GetWindowPositionDelegate>("SDL_GetWindowPosition");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GetWindowPosition(IntPtr window, out Int32 x, out Int32 y) => pSDL_GetWindowPosition(window, out x, out y);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowPositionDelegate(IntPtr window, Int32 x, Int32 y);
        private readonly SDL_SetWindowPositionDelegate pSDL_SetWindowPosition = lib.LoadFunction<SDL_SetWindowPositionDelegate>("SDL_SetWindowPosition");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowPosition(IntPtr window, Int32 x, Int32 y) => pSDL_SetWindowPosition(window, x, y);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GetWindowSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private readonly SDL_GetWindowSizeDelegate pSDL_GetWindowSize = lib.LoadFunction<SDL_GetWindowSizeDelegate>("SDL_GetWindowSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GetWindowSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GetWindowSize(window, out w, out h);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowSizeDelegate(IntPtr window, Int32 w, Int32 h);
        private readonly SDL_SetWindowSizeDelegate pSDL_SetWindowSize = lib.LoadFunction<SDL_SetWindowSizeDelegate>("SDL_SetWindowSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowSize(IntPtr window, Int32 w, Int32 h) => pSDL_SetWindowSize(window, w, h);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GetWindowMinimumSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private readonly SDL_GetWindowMinimumSizeDelegate pSDL_GetWindowMinimumSize = lib.LoadFunction<SDL_GetWindowMinimumSizeDelegate>("SDL_GetWindowMinimumSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GetWindowMinimumSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GetWindowMinimumSize(window, out w, out h);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowMinimumSizeDelegate(IntPtr window, Int32 w, Int32 h);
        private readonly SDL_SetWindowMinimumSizeDelegate pSDL_SetWindowMinimumSize = lib.LoadFunction<SDL_SetWindowMinimumSizeDelegate>("SDL_SetWindowMinimumSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowMinimumSize(IntPtr window, Int32 w, Int32 h) => pSDL_SetWindowMinimumSize(window, w, h);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GetWindowMaximumSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private readonly SDL_GetWindowMaximumSizeDelegate pSDL_GetWindowMaximumSize = lib.LoadFunction<SDL_GetWindowMaximumSizeDelegate>("SDL_GetWindowMaximumSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GetWindowMaximumSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GetWindowMaximumSize(window, out w, out h);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowMaximumSizeDelegate(IntPtr window, Int32 w, Int32 h);
        private readonly SDL_SetWindowMaximumSizeDelegate pSDL_SetWindowMaximumSize = lib.LoadFunction<SDL_SetWindowMaximumSizeDelegate>("SDL_SetWindowMaximumSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowMaximumSize(IntPtr window, Int32 w, Int32 h) => pSDL_SetWindowMaximumSize(window, w, h);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_GetWindowGrabDelegate(IntPtr window);
        private readonly SDL_GetWindowGrabDelegate pSDL_GetWindowGrab = lib.LoadFunction<SDL_GetWindowGrabDelegate>("SDL_GetWindowGrab");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_GetWindowGrab(IntPtr window) => pSDL_GetWindowGrab(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowGrabDelegate(IntPtr window, Boolean grabbed);
        private readonly SDL_SetWindowGrabDelegate pSDL_SetWindowGrab = lib.LoadFunction<SDL_SetWindowGrabDelegate>("SDL_SetWindowGrab");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetWindowGrab(IntPtr window, Boolean grabbed) => pSDL_SetWindowGrab(window, grabbed);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SetWindowBorderedDelegate(IntPtr window, Boolean bordered);
        private readonly SDL_SetWindowBorderedDelegate pSDL_SetWindowBordered = lib.LoadFunction<SDL_SetWindowBorderedDelegate>("SDL_SetWindowBordered");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetWindowBordered(IntPtr window, Boolean bordered) => pSDL_SetWindowBordered(window, bordered);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SetWindowFullscreenDelegate(IntPtr window, UInt32 flags);
        private readonly SDL_SetWindowFullscreenDelegate pSDL_SetWindowFullscreen = lib.LoadFunction<SDL_SetWindowFullscreenDelegate>("SDL_SetWindowFullscreen");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetWindowFullscreen(IntPtr window, UInt32 flags) => pSDL_SetWindowFullscreen(window, flags);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SetWindowDisplayModeDelegate(IntPtr window, SDL_DisplayMode* mode);
        private readonly SDL_SetWindowDisplayModeDelegate pSDL_SetWindowDisplayMode = lib.LoadFunction<SDL_SetWindowDisplayModeDelegate>("SDL_SetWindowDisplayMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode) => pSDL_SetWindowDisplayMode(window, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetWindowDisplayModeDelegate(IntPtr window, SDL_DisplayMode* mode);
        private readonly SDL_GetWindowDisplayModeDelegate pSDL_GetWindowDisplayMode = lib.LoadFunction<SDL_GetWindowDisplayModeDelegate>("SDL_GetWindowDisplayMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode) => pSDL_GetWindowDisplayMode(window, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetWindowDisplayIndexDelegate(IntPtr window);
        private readonly SDL_GetWindowDisplayIndexDelegate pSDL_GetWindowDisplayIndex = lib.LoadFunction<SDL_GetWindowDisplayIndexDelegate>("SDL_GetWindowDisplayIndex");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetWindowDisplayIndex(IntPtr window) => pSDL_GetWindowDisplayIndex(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_WindowFlags SDL_GetWindowFlagsDelegate(IntPtr window);
        private readonly SDL_GetWindowFlagsDelegate pSDL_GetWindowFlags = lib.LoadFunction<SDL_GetWindowFlagsDelegate>("SDL_GetWindowFlags");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_WindowFlags SDL_GetWindowFlags(IntPtr window) => pSDL_GetWindowFlags(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_ShowWindowDelegate(IntPtr window);
        private readonly SDL_ShowWindowDelegate pSDL_ShowWindow = lib.LoadFunction<SDL_ShowWindowDelegate>("SDL_ShowWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_ShowWindow(IntPtr window) => pSDL_ShowWindow(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_HideWindowDelegate(IntPtr window);
        private readonly SDL_HideWindowDelegate pSDL_HideWindow = lib.LoadFunction<SDL_HideWindowDelegate>("SDL_HideWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_HideWindow(IntPtr window) => pSDL_HideWindow(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_MaximizeWindowDelegate(IntPtr window);
        private readonly SDL_MaximizeWindowDelegate pSDL_MaximizeWindow = lib.LoadFunction<SDL_MaximizeWindowDelegate>("SDL_MaximizeWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_MaximizeWindow(IntPtr window) => pSDL_MaximizeWindow(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_MinimizeWindowDelegate(IntPtr window);
        private readonly SDL_MinimizeWindowDelegate pSDL_MinimizeWindow = lib.LoadFunction<SDL_MinimizeWindowDelegate>("SDL_MinimizeWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_MinimizeWindow(IntPtr window) => pSDL_MinimizeWindow(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_RestoreWindowDelegate(IntPtr window);
        private readonly SDL_RestoreWindowDelegate pSDL_RestoreWindow = lib.LoadFunction<SDL_RestoreWindowDelegate>("SDL_RestoreWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_RestoreWindow(IntPtr window) => pSDL_RestoreWindow(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_GetWindowWMInfoDelegate(IntPtr window, SDL_SysWMinfo* info);
        private readonly SDL_GetWindowWMInfoDelegate pSDL_GetWindowWMInfo = lib.LoadFunction<SDL_GetWindowWMInfoDelegate>("SDL_GetWindowWMInfo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_GetWindowWMInfo(IntPtr window, SDL_SysWMinfo* info) => pSDL_GetWindowWMInfo(window, info);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_RWFromFileDelegate(String file, String mode);
        private readonly SDL_RWFromFileDelegate pSDL_RWFromFile = lib.LoadFunction<SDL_RWFromFileDelegate>("SDL_RWFromFile");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_RWFromFile(String file, String mode) => pSDL_RWFromFile(file, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_RWFromMemDelegate(IntPtr mem, Int32 size);
        private readonly SDL_RWFromMemDelegate pSDL_RWFromMem = lib.LoadFunction<SDL_RWFromMemDelegate>("SDL_RWFromMem");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_RWFromMem(IntPtr mem, Int32 size) => pSDL_RWFromMem(mem, size);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_AllocRWDelegate();
        private readonly SDL_AllocRWDelegate pSDL_AllocRW = lib.LoadFunction<SDL_AllocRWDelegate>("SDL_AllocRW");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_AllocRW() => pSDL_AllocRW();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_FreeRWDelegate(IntPtr area);
        private readonly SDL_FreeRWDelegate pSDL_FreeRW = lib.LoadFunction<SDL_FreeRWDelegate>("SDL_FreeRW");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_FreeRW(IntPtr area) => pSDL_FreeRW(area);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Surface* SDL_LoadBMP_RWDelegate(IntPtr src, Int32 freesrc);
        private readonly SDL_LoadBMP_RWDelegate pSDL_LoadBMP_RW = lib.LoadFunction<SDL_LoadBMP_RWDelegate>("SDL_LoadBMP_RW");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Surface* SDL_LoadBMP_RW(IntPtr src, Int32 freesrc) => pSDL_LoadBMP_RW(src, freesrc);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SaveBMP_RWDelegate(SDL_Surface* surface, IntPtr dst, Int32 freedst);
        private readonly SDL_SaveBMP_RWDelegate pSDL_SaveBMP_RW = lib.LoadFunction<SDL_SaveBMP_RWDelegate>("SDL_SaveBMP_RW");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SaveBMP_RW(SDL_Surface* surface, IntPtr dst, Int32 freedst) => pSDL_SaveBMP_RW(surface, dst, freedst);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UInt32 SDL_GetMouseStateDelegate(out Int32 x, out Int32 y);
        private readonly SDL_GetMouseStateDelegate pSDL_GetMouseState = lib.LoadFunction<SDL_GetMouseStateDelegate>("SDL_GetMouseState");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed UInt32 SDL_GetMouseState(out Int32 x, out Int32 y) => pSDL_GetMouseState(out x, out y);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GetKeyboardStateDelegate(out Int32 numkeys);
        private readonly SDL_GetKeyboardStateDelegate pSDL_GetKeyboardState = lib.LoadFunction<SDL_GetKeyboardStateDelegate>("SDL_GetKeyboardState");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GetKeyboardState(out Int32 numkeys) => pSDL_GetKeyboardState(out numkeys);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Scancode SDL_GetScancodeFromKeyDelegate(SDL_Keycode keycode);
        private readonly SDL_GetScancodeFromKeyDelegate pSDL_GetScancodeFromKey = lib.LoadFunction<SDL_GetScancodeFromKeyDelegate>("SDL_GetScancodeFromKey");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Scancode SDL_GetScancodeFromKey(SDL_Keycode keycode) => pSDL_GetScancodeFromKey(keycode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Keymod SDL_GetModStateDelegate();
        private readonly SDL_GetModStateDelegate pSDL_GetModState = lib.LoadFunction<SDL_GetModStateDelegate>("SDL_GetModState");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Keymod SDL_GetModState() => pSDL_GetModState();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_SetHintDelegate(String name, String value);
        private readonly SDL_SetHintDelegate pSDL_SetHint = lib.LoadFunction<SDL_SetHintDelegate>("SDL_SetHint");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_SetHint(String name, String value) => pSDL_SetHint(name, value);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Surface* SDL_CreateRGBSurfaceDelegate(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 AMask);
        private readonly SDL_CreateRGBSurfaceDelegate pSDL_CreateRGBSurface = lib.LoadFunction<SDL_CreateRGBSurfaceDelegate>("SDL_CreateRGBSurface");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Surface* SDL_CreateRGBSurface(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 AMask) => pSDL_CreateRGBSurface(flags, width, height, depth, Rmask, Gmask, Bmask, AMask);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_FreeSurfaceDelegate(SDL_Surface* surface);
        private readonly SDL_FreeSurfaceDelegate pSDL_FreeSurface = lib.LoadFunction<SDL_FreeSurfaceDelegate>("SDL_FreeSurface");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_FreeSurface(SDL_Surface* surface) => pSDL_FreeSurface(surface);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_LockSurfaceDelegate(SDL_Surface* surface);
        private readonly SDL_LockSurfaceDelegate pSDL_LockSurface = lib.LoadFunction<SDL_LockSurfaceDelegate>("SDL_LockSurface");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_LockSurface(SDL_Surface* surface) => pSDL_LockSurface(surface);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_UnlockSurfaceDelegate(SDL_Surface* surface);
        private readonly SDL_UnlockSurfaceDelegate pSDL_UnlockSurface = lib.LoadFunction<SDL_UnlockSurfaceDelegate>("SDL_UnlockSurface");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_UnlockSurface(SDL_Surface* surface) => pSDL_UnlockSurface(surface);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_BlitSurfaceDelegate(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect);
        private readonly SDL_BlitSurfaceDelegate pSDL_BlitSurface = lib.LoadFunction<SDL_BlitSurfaceDelegate>("SDL_UpperBlit");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_BlitSurface(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect) => pSDL_BlitSurface(src, srcrect, dst, dstrect);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_BlitScaledDelegate(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect);
        private readonly SDL_BlitScaledDelegate pSDL_BlitScaled = lib.LoadFunction<SDL_BlitScaledDelegate>("SDL_UpperBlitScaled");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_BlitScaled(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect) => pSDL_BlitScaled(src, srcrect, dst, dstrect);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SetSurfaceBlendModeDelegate(SDL_Surface* surface, SDL_BlendMode blendMode);
        private readonly SDL_SetSurfaceBlendModeDelegate pSDL_SetSurfaceBlendMode = lib.LoadFunction<SDL_SetSurfaceBlendModeDelegate>("SDL_SetSurfaceBlendMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode blendMode) => pSDL_SetSurfaceBlendMode(surface, blendMode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetSurfaceBlendModeDelegate(SDL_Surface* surface, SDL_BlendMode* blendMode);
        private readonly SDL_GetSurfaceBlendModeDelegate pSDL_GetSurfaceBlendMode = lib.LoadFunction<SDL_GetSurfaceBlendModeDelegate>("SDL_GetSurfaceBlendMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode* blendMode) => pSDL_GetSurfaceBlendMode(surface, blendMode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_FillRectDelegate(SDL_Surface* surface, SDL_Rect* rect, UInt32 color);
        private readonly SDL_FillRectDelegate pSDL_FillRect = lib.LoadFunction<SDL_FillRectDelegate>("SDL_FillRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_FillRect(SDL_Surface* surface, SDL_Rect* rect, UInt32 color) => pSDL_FillRect(surface, rect, color);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_FillRectsDelegate(SDL_Surface* dst, SDL_Rect* rects, Int32 count, UInt32 colors);
        private readonly SDL_FillRectsDelegate pSDL_FillRects = lib.LoadFunction<SDL_FillRectsDelegate>("SDL_FillRects");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_FillRects(SDL_Surface* dst, SDL_Rect* rects, Int32 count, UInt32 colors) => pSDL_FillRects(dst, rects, count, colors);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Cursor* SDL_CreateColorCursorDelegate(SDL_Surface* surface, Int32 hot_x, Int32 hot_y);
        private readonly SDL_CreateColorCursorDelegate pSDL_CreateColorCursor = lib.LoadFunction<SDL_CreateColorCursorDelegate>("SDL_CreateColorCursor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Cursor* SDL_CreateColorCursor(SDL_Surface* surface, Int32 hot_x, Int32 hot_y) => pSDL_CreateColorCursor(surface, hot_x, hot_y);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_FreeCursorDelegate(SDL_Cursor* cursor);
        private readonly SDL_FreeCursorDelegate pSDL_FreeCursor = lib.LoadFunction<SDL_FreeCursorDelegate>("SDL_FreeCursor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_FreeCursor(SDL_Cursor* cursor) => pSDL_FreeCursor(cursor);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_ShowCursorDelegate(Int32 toggle);
        private readonly SDL_ShowCursorDelegate pSDL_ShowCursor = lib.LoadFunction<SDL_ShowCursorDelegate>("SDL_ShowCursor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_ShowCursor(Int32 toggle) => pSDL_ShowCursor(toggle);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Cursor* SDL_GetCursorDelegate();
        private readonly SDL_GetCursorDelegate pSDL_GetCursor = lib.LoadFunction<SDL_GetCursorDelegate>("SDL_GetCursor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Cursor* SDL_GetCursor() => pSDL_GetCursor();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetCursorDelegate(SDL_Cursor* cursor);
        private readonly SDL_SetCursorDelegate pSDL_SetCursor = lib.LoadFunction<SDL_SetCursorDelegate>("SDL_SetCursor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetCursor(SDL_Cursor* cursor) => pSDL_SetCursor(cursor);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Cursor* SDL_GetDefaultCursorDelegate();
        private readonly SDL_GetDefaultCursorDelegate pSDL_GetDefaultCursor = lib.LoadFunction<SDL_GetDefaultCursorDelegate>("SDL_GetDefaultCursor");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Cursor* SDL_GetDefaultCursor() => pSDL_GetDefaultCursor();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetNumVideoDisplaysDelegate();
        private readonly SDL_GetNumVideoDisplaysDelegate pSDL_GetNumVideoDisplays = lib.LoadFunction<SDL_GetNumVideoDisplaysDelegate>("SDL_GetNumVideoDisplays");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetNumVideoDisplays() => pSDL_GetNumVideoDisplays();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GetDisplayName_RawDelegate(Int32 displayIndex);
        private readonly SDL_GetDisplayName_RawDelegate pSDL_GetDisplayName_Raw = lib.LoadFunction<SDL_GetDisplayName_RawDelegate>("SDL_GetDisplayName");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GetDisplayName(Int32 displayIndex) => Marshal.PtrToStringAnsi(pSDL_GetDisplayName_Raw(displayIndex));
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetDisplayBoundsDelegate(Int32 displayIndex, SDL_Rect* rect);
        private readonly SDL_GetDisplayBoundsDelegate pSDL_GetDisplayBounds = lib.LoadFunction<SDL_GetDisplayBoundsDelegate>("SDL_GetDisplayBounds");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetDisplayBounds(Int32 displayIndex, SDL_Rect* rect) => pSDL_GetDisplayBounds(displayIndex, rect);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetNumDisplayModesDelegate(Int32 displayIndex);
        private readonly SDL_GetNumDisplayModesDelegate pSDL_GetNumDisplayModes = lib.LoadFunction<SDL_GetNumDisplayModesDelegate>("SDL_GetNumDisplayModes");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetNumDisplayModes(Int32 displayIndex) => pSDL_GetNumDisplayModes(displayIndex);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetDisplayModeDelegate(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode);
        private readonly SDL_GetDisplayModeDelegate pSDL_GetDisplayMode = lib.LoadFunction<SDL_GetDisplayModeDelegate>("SDL_GetDisplayMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetDisplayMode(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode) => pSDL_GetDisplayMode(displayIndex, modeIndex, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetCurrentDisplayModeDelegate(Int32 displayIndex, SDL_DisplayMode* mode);
        private readonly SDL_GetCurrentDisplayModeDelegate pSDL_GetCurrentDisplayMode = lib.LoadFunction<SDL_GetCurrentDisplayModeDelegate>("SDL_GetCurrentDisplayMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetCurrentDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode) => pSDL_GetCurrentDisplayMode(displayIndex, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetDesktopDisplayModeDelegate(Int32 displayIndex, SDL_DisplayMode* mode);
        private readonly SDL_GetDesktopDisplayModeDelegate pSDL_GetDesktopDisplayMode = lib.LoadFunction<SDL_GetDesktopDisplayModeDelegate>("SDL_GetDesktopDisplayMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetDesktopDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode) => pSDL_GetDesktopDisplayMode(displayIndex, mode);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_DisplayMode* SDL_GetClosestDisplayModeDelegate(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest);
        private readonly SDL_GetClosestDisplayModeDelegate pSDL_GetClosestDisplayMode = lib.LoadFunction<SDL_GetClosestDisplayModeDelegate>("SDL_GetClosestDisplayMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_DisplayMode* SDL_GetClosestDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest) => pSDL_GetClosestDisplayMode(displayIndex, mode, closest);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_PixelFormatEnumToMasksDelegate(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask);
        private readonly SDL_PixelFormatEnumToMasksDelegate pSDL_PixelFormatEnumToMasks = lib.LoadFunction<SDL_PixelFormatEnumToMasksDelegate>("SDL_PixelFormatEnumToMasks");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_PixelFormatEnumToMasks(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask) => pSDL_PixelFormatEnumToMasks(format, bpp, Rmask, Gmask, Bmask, Amask);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GL_GetProcAddressDelegate(String proc);
        private readonly SDL_GL_GetProcAddressDelegate pSDL_GL_GetProcAddress = lib.LoadFunction<SDL_GL_GetProcAddressDelegate>("SDL_GL_GetProcAddress");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GL_GetProcAddress(String proc) => pSDL_GL_GetProcAddress(proc);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GL_CreateContextDelegate(IntPtr window);
        private readonly SDL_GL_CreateContextDelegate pSDL_GL_CreateContext = lib.LoadFunction<SDL_GL_CreateContextDelegate>("SDL_GL_CreateContext");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GL_CreateContext(IntPtr window) => pSDL_GL_CreateContext(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GL_DeleteContextDelegate(IntPtr context);
        private readonly SDL_GL_DeleteContextDelegate pSDL_GL_DeleteContext = lib.LoadFunction<SDL_GL_DeleteContextDelegate>("SDL_GL_DeleteContext");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GL_DeleteContext(IntPtr context) => pSDL_GL_DeleteContext(context);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GL_GetCurrentContextDelegate(IntPtr context);
        private readonly SDL_GL_GetCurrentContextDelegate pSDL_GL_GetCurrentContext = lib.LoadFunction<SDL_GL_GetCurrentContextDelegate>("SDL_GL_GetCurrentContext");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GL_GetCurrentContext(IntPtr context) => pSDL_GL_GetCurrentContext(context);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GL_MakeCurrentDelegate(IntPtr window, IntPtr context);
        private readonly SDL_GL_MakeCurrentDelegate pSDL_GL_MakeCurrent = lib.LoadFunction<SDL_GL_MakeCurrentDelegate>("SDL_GL_MakeCurrent");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GL_MakeCurrent(IntPtr window, IntPtr context) => pSDL_GL_MakeCurrent(window, context);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GL_SetAttributeDelegate(SDL_GLattr attr, Int32 value);
        private readonly SDL_GL_SetAttributeDelegate pSDL_GL_SetAttribute = lib.LoadFunction<SDL_GL_SetAttributeDelegate>("SDL_GL_SetAttribute");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GL_SetAttribute(SDL_GLattr attr, Int32 value) => pSDL_GL_SetAttribute(attr, value);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GL_GetAttributeDelegate(SDL_GLattr attr, Int32* value);
        private readonly SDL_GL_GetAttributeDelegate pSDL_GL_GetAttribute = lib.LoadFunction<SDL_GL_GetAttributeDelegate>("SDL_GL_GetAttribute");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GL_GetAttribute(SDL_GLattr attr, Int32* value) => pSDL_GL_GetAttribute(attr, value);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GL_SwapWindowDelegate(IntPtr window);
        private readonly SDL_GL_SwapWindowDelegate pSDL_GL_SwapWindow = lib.LoadFunction<SDL_GL_SwapWindowDelegate>("SDL_GL_SwapWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GL_SwapWindow(IntPtr window) => pSDL_GL_SwapWindow(window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GL_SetSwapIntervalDelegate(Int32 interval);
        private readonly SDL_GL_SetSwapIntervalDelegate pSDL_GL_SetSwapInterval = lib.LoadFunction<SDL_GL_SetSwapIntervalDelegate>("SDL_GL_SetSwapInterval");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GL_SetSwapInterval(Int32 interval) => pSDL_GL_SetSwapInterval(interval);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GL_GetDrawableSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private readonly SDL_GL_GetDrawableSizeDelegate pSDL_GL_GetDrawableSize = lib.LoadFunction<SDL_GL_GetDrawableSizeDelegate>("SDL_GL_GetDrawableSize");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GL_GetDrawableSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GL_GetDrawableSize(window, out w, out h);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_NumJoysticksDelegate();
        private readonly SDL_NumJoysticksDelegate pSDL_NumJoysticks = lib.LoadFunction<SDL_NumJoysticksDelegate>("SDL_NumJoysticks");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_NumJoysticks() => pSDL_NumJoysticks();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_IsGameControllerDelegate(Int32 joystick_index);
        private readonly SDL_IsGameControllerDelegate pSDL_IsGameController = lib.LoadFunction<SDL_IsGameControllerDelegate>("SDL_IsGameController");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_IsGameController(Int32 joystick_index) => pSDL_IsGameController(joystick_index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GameControllerOpenDelegate(Int32 index);
        private readonly SDL_GameControllerOpenDelegate pSDL_GameControllerOpen = lib.LoadFunction<SDL_GameControllerOpenDelegate>("SDL_GameControllerOpen");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GameControllerOpen(Int32 index) => pSDL_GameControllerOpen(index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GameControllerCloseDelegate(IntPtr gamecontroller);
        private readonly SDL_GameControllerCloseDelegate pSDL_GameControllerClose = lib.LoadFunction<SDL_GameControllerCloseDelegate>("SDL_GameControllerClose");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_GameControllerClose(IntPtr gamecontroller) => pSDL_GameControllerClose(gamecontroller);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GameControllerNameForIndex_RawDelegate(Int32 joystick_index);
        private readonly SDL_GameControllerNameForIndex_RawDelegate pSDL_GameControllerNameForIndex_Raw = lib.LoadFunction<SDL_GameControllerNameForIndex_RawDelegate>("SDL_GameControllerNameForIndex");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GameControllerNameForIndex(Int32 joystick_index) => Marshal.PtrToStringAnsi(pSDL_GameControllerNameForIndex_Raw(joystick_index));
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_GameControllerGetButtonDelegate(IntPtr gamecontroller, SDL_GameControllerButton button);
        private readonly SDL_GameControllerGetButtonDelegate pSDL_GameControllerGetButton = lib.LoadFunction<SDL_GameControllerGetButtonDelegate>("SDL_GameControllerGetButton");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_GameControllerGetButton(IntPtr gamecontroller, SDL_GameControllerButton button) => pSDL_GameControllerGetButton(gamecontroller, button);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GameControllerGetJoystickDelegate(IntPtr gamecontroller);
        private readonly SDL_GameControllerGetJoystickDelegate pSDL_GameControllerGetJoystick = lib.LoadFunction<SDL_GameControllerGetJoystickDelegate>("SDL_GameControllerGetJoystick");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GameControllerGetJoystick(IntPtr gamecontroller) => pSDL_GameControllerGetJoystick(gamecontroller);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_JoystickInstanceIDDelegate(IntPtr joystick);
        private readonly SDL_JoystickInstanceIDDelegate pSDL_JoystickInstanceID = lib.LoadFunction<SDL_JoystickInstanceIDDelegate>("SDL_JoystickInstanceID");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_JoystickInstanceID(IntPtr joystick) => pSDL_JoystickInstanceID(joystick);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetNumTouchDevicesDelegate();
        private readonly SDL_GetNumTouchDevicesDelegate pSDL_GetNumTouchDevices = lib.LoadFunction<SDL_GetNumTouchDevicesDelegate>("SDL_GetNumTouchDevices");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetNumTouchDevices() => pSDL_GetNumTouchDevices();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int64 SDL_GetTouchDeviceDelegate(Int32 index);
        private readonly SDL_GetTouchDeviceDelegate pSDL_GetTouchDevice = lib.LoadFunction<SDL_GetTouchDeviceDelegate>("SDL_GetTouchDevice");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int64 SDL_GetTouchDevice(Int32 index) => pSDL_GetTouchDevice(index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetNumTouchFingersDelegate(Int64 touchID);
        private readonly SDL_GetNumTouchFingersDelegate pSDL_GetNumTouchFingers = lib.LoadFunction<SDL_GetNumTouchFingersDelegate>("SDL_GetNumTouchFingers");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetNumTouchFingers(Int64 touchID) => pSDL_GetNumTouchFingers(touchID);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Finger* SDL_GetTouchFingerDelegate(Int64 touchID, Int32 index);
        private readonly SDL_GetTouchFingerDelegate pSDL_GetTouchFinger = lib.LoadFunction<SDL_GetTouchFingerDelegate>("SDL_GetTouchFinger");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_Finger* SDL_GetTouchFinger(Int64 touchID, Int32 index) => pSDL_GetTouchFinger(touchID, index);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_RecordGestureDelegate(Int64 touchID);
        private readonly SDL_RecordGestureDelegate pSDL_RecordGesture = lib.LoadFunction<SDL_RecordGestureDelegate>("SDL_RecordGesture");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_RecordGesture(Int64 touchID) => pSDL_RecordGesture(touchID);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SaveAllDollarTemplatesDelegate(IntPtr dst);
        private readonly SDL_SaveAllDollarTemplatesDelegate pSDL_SaveAllDollarTemplates = lib.LoadFunction<SDL_SaveAllDollarTemplatesDelegate>("SDL_SaveAllDollarTemplates");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SaveAllDollarTemplates(IntPtr dst) => pSDL_SaveAllDollarTemplates(dst);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SaveDollarTemplateDelegate(Int64 gestureID, IntPtr dst);
        private readonly SDL_SaveDollarTemplateDelegate pSDL_SaveDollarTemplate = lib.LoadFunction<SDL_SaveDollarTemplateDelegate>("SDL_SaveDollarTemplate");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SaveDollarTemplate(Int64 gestureID, IntPtr dst) => pSDL_SaveDollarTemplate(gestureID, dst);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_LoadDollarTemplatesDelegate(Int64 touchID, IntPtr src);
        private readonly SDL_LoadDollarTemplatesDelegate pSDL_LoadDollarTemplates = lib.LoadFunction<SDL_LoadDollarTemplatesDelegate>("SDL_LoadDollarTemplates");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_LoadDollarTemplates(Int64 touchID, IntPtr src) => pSDL_LoadDollarTemplates(touchID, src);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_StartTextInputDelegate();
        private readonly SDL_StartTextInputDelegate pSDL_StartTextInput = lib.LoadFunction<SDL_StartTextInputDelegate>("SDL_StartTextInput");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_StartTextInput() => pSDL_StartTextInput();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_StopTextInputDelegate();
        private readonly SDL_StopTextInputDelegate pSDL_StopTextInput = lib.LoadFunction<SDL_StopTextInputDelegate>("SDL_StopTextInput");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_StopTextInput() => pSDL_StopTextInput();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetTextInputRectDelegate(SDL_Rect* rect);
        private readonly SDL_SetTextInputRectDelegate pSDL_SetTextInputRect = lib.LoadFunction<SDL_SetTextInputRectDelegate>("SDL_SetTextInputRect");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetTextInputRect(SDL_Rect* rect) => pSDL_SetTextInputRect(rect);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_HasClipboardTextDelegate();
        private readonly SDL_HasClipboardTextDelegate pSDL_HasClipboardText = lib.LoadFunction<SDL_HasClipboardTextDelegate>("SDL_HasClipboardText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_HasClipboardText() => pSDL_HasClipboardText();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GetClipboardTextDelegate();
        private readonly SDL_GetClipboardTextDelegate pSDL_GetClipboardText = lib.LoadFunction<SDL_GetClipboardTextDelegate>("SDL_GetClipboardText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed IntPtr SDL_GetClipboardText() => pSDL_GetClipboardText();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetClipboardTextDelegate(IntPtr text);
        private readonly SDL_SetClipboardTextDelegate pSDL_SetClipboardText = lib.LoadFunction<SDL_SetClipboardTextDelegate>("SDL_SetClipboardText");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_SetClipboardText(IntPtr text) => pSDL_SetClipboardText(text);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_PowerState SDL_GetPowerInfoDelegate(Int32* secs, Int32* pct);
        private readonly SDL_GetPowerInfoDelegate pSDL_GetPowerInfo = lib.LoadFunction<SDL_GetPowerInfoDelegate>("SDL_GetPowerInfo");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed SDL_PowerState SDL_GetPowerInfo(Int32* secs, Int32* pct) => pSDL_GetPowerInfo(secs, pct);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_ShowSimpleMessageBoxDelegate(UInt32 flags, String title, String message, IntPtr window);
        private readonly SDL_ShowSimpleMessageBoxDelegate pSDL_ShowSimpleMessageBox = lib.LoadFunction<SDL_ShowSimpleMessageBoxDelegate>("SDL_ShowSimpleMessageBox");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_ShowSimpleMessageBox(UInt32 flags, String title, String message, IntPtr window) => pSDL_ShowSimpleMessageBox(flags, title, message, window);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SetWindowOpacityDelegate(IntPtr window, Single opacity);
        private readonly SDL_SetWindowOpacityDelegate pSDL_SetWindowOpacity = lib.LoadFunction<SDL_SetWindowOpacityDelegate>("SDL_SetWindowOpacity");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetWindowOpacity(IntPtr window, Single opacity) => pSDL_SetWindowOpacity(window, opacity);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetWindowOpacityDelegate(IntPtr window, Single* opacity);
        private readonly SDL_GetWindowOpacityDelegate pSDL_GetWindowOpacity = lib.LoadFunction<SDL_GetWindowOpacityDelegate>("SDL_GetWindowOpacity");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetWindowOpacity(IntPtr window, Single* opacity) => pSDL_GetWindowOpacity(window, opacity);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GameControllerAddMappingDelegate(String mappingString);
        private readonly SDL_GameControllerAddMappingDelegate pSDL_GameControllerAddMapping = lib.LoadFunction<SDL_GameControllerAddMappingDelegate>("SDL_GameControllerAddMapping");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GameControllerAddMapping(String mappingString) => pSDL_GameControllerAddMapping(mappingString);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GameControllerAddMappingsFromRWDelegate(IntPtr rw, Int32 freerw);
        private readonly SDL_GameControllerAddMappingsFromRWDelegate pSDL_GameControllerAddMappingsFromRW = lib.LoadFunction<SDL_GameControllerAddMappingsFromRWDelegate>("SDL_GameControllerAddMappingsFromRW");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GameControllerAddMappingsFromRW(IntPtr rw, Int32 freerw) => pSDL_GameControllerAddMappingsFromRW(rw, freerw);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GameControllerMapping_RawDelegate(IntPtr gamecontroller);
        private readonly SDL_GameControllerMapping_RawDelegate pSDL_GameControllerMapping_Raw = lib.LoadFunction<SDL_GameControllerMapping_RawDelegate>("SDL_GameControllerMapping");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GameControllerMapping(IntPtr gamecontroller) => Marshal.PtrToStringAnsi(pSDL_GameControllerMapping_Raw(gamecontroller));
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GameControllerMappingForGUID_RawDelegate(Guid guid);
        private readonly SDL_GameControllerMappingForGUID_RawDelegate pSDL_GameControllerMappingForGUID_Raw = lib.LoadFunction<SDL_GameControllerMappingForGUID_RawDelegate>("SDL_GameControllerMappingForGUID");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed String SDL_GameControllerMappingForGUID(Guid guid) => Marshal.PtrToStringAnsi(pSDL_GameControllerMappingForGUID_Raw(guid));
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Guid SDL_JoystickGetGUIDDelegate(String pchGUID);
        private readonly SDL_JoystickGetGUIDDelegate pSDL_JoystickGetGUID = lib.LoadFunction<SDL_JoystickGetGUIDDelegate>("SDL_JoystickGetGUID");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Guid SDL_JoystickGetGUID(String pchGUID) => pSDL_JoystickGetGUID(pchGUID);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetDisplayDPIDelegate(Int32 displayIndex, Single* ddpi, Single* hdpi, Single* vdpi);
        private readonly SDL_GetDisplayDPIDelegate pSDL_GetDisplayDPI = lib.LoadFunction<SDL_GetDisplayDPIDelegate>("SDL_GetDisplayDPI");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_GetDisplayDPI(Int32 displayIndex, Single* ddpi, Single* hdpi, Single* vdpi) => pSDL_GetDisplayDPI(displayIndex, ddpi, hdpi, vdpi);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_freeDelegate(IntPtr mem);
        private readonly SDL_freeDelegate pSDL_free = lib.LoadFunction<SDL_freeDelegate>("SDL_free");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_free(IntPtr mem) => pSDL_free(mem);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_GetRelativeMouseModeDelegate();
        private readonly SDL_GetRelativeMouseModeDelegate pSDL_GetRelativeMouseMode = lib.LoadFunction<SDL_GetRelativeMouseModeDelegate>("SDL_GetRelativeMouseMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Boolean SDL_GetRelativeMouseMode() => pSDL_GetRelativeMouseMode();
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SetRelativeMouseModeDelegate(Boolean enabled);
        private readonly SDL_SetRelativeMouseModeDelegate pSDL_SetRelativeMouseMode = lib.LoadFunction<SDL_SetRelativeMouseModeDelegate>("SDL_SetRelativeMouseMode");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed Int32 SDL_SetRelativeMouseMode(Boolean enabled) => pSDL_SetRelativeMouseMode(enabled);
        
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_WarpMouseInWindowDelegate(IntPtr window, Int32 x, Int32 y);
        private readonly SDL_WarpMouseInWindowDelegate pSDL_WarpMouseInWindow = lib.LoadFunction<SDL_WarpMouseInWindowDelegate>("SDL_WarpMouseInWindow");
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override sealed void SDL_WarpMouseInWindow(IntPtr window, Int32 x, Int32 y) => pSDL_WarpMouseInWindow(window, x, y);
    }
#pragma warning restore 1591
}
