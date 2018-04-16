using System;
using System.Runtime.InteropServices;
using System.Security;
using Ultraviolet.Core;
using Ultraviolet.Core.Native;

namespace Ultraviolet.SDL2.Native
{
#pragma warning disable 1591
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate Int32 SDL_EventFilter(IntPtr userdata, SDL_Event* @event);

    /// <summary>
    /// Contains bindings for native SDL2 function calls.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public unsafe static partial class SDLNative
    {
        // NOTE: The #ifdefs everywhere are necessary because I haven't yet found a way to make
        // the new dynamic loader work on mobile platforms, particularly Android, where dlopen()
        // sometimes maps the same library to multiple address spaces for reasons that I haven't
        // yet been able to discern. My hope is that if the proposed .NET Standard API for dynamic
        // library loading ever makes it to Xamarin Android/iOS, we can standardize all supported
        // platforms on a single declaration type. For now, though, this nonsense seems necessary.

#if ANDROID
        const String LIBRARY = "SDL2";
#elif IOS
        const String LIBRARY = "__Internal";
#else
        private static readonly NativeLibrary lib = new NativeLibrary(
            UltravioletPlatformInfo.CurrentPlatform == UltravioletPlatform.Windows ? "SDL2" : "libSDL2");
#endif
                
#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetError", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GetError_Impl();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GetErrorDelegate();
        private static readonly SDL_GetErrorDelegate pSDL_GetError = lib.LoadFunction<SDL_GetErrorDelegate>("SDL_GetError");
        private static IntPtr SDL_GetError_Impl() => pSDL_GetError();
#endif
        public static String SDL_GetError() => Marshal.PtrToStringAnsi(SDL_GetError_Impl());

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_ClearError", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ClearError();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_ClearErrorDelegate();
        private static readonly SDL_ClearErrorDelegate pSDL_ClearError = lib.LoadFunction<SDL_ClearErrorDelegate>("SDL_ClearError");
        public static void SDL_ClearError() => pSDL_ClearError();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_Init", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_Init(SDL_Init flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_InitDelegate(SDL_Init flags);
        private static readonly SDL_InitDelegate pSDL_Init = lib.LoadFunction<SDL_InitDelegate>("SDL_Init");
        public static Int32 SDL_Init(SDL_Init flags) => pSDL_Init(flags);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_Quit", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Quit();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_QuitDelegate();
        private static readonly SDL_QuitDelegate pSDL_Quit = lib.LoadFunction<SDL_QuitDelegate>("SDL_Quit");
        public static void SDL_Quit() => pSDL_Quit();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_PumpEvents", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PumpEvents();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_PumpEventsDelegate();
        private static readonly SDL_PumpEventsDelegate pSDL_PumpEvents = lib.LoadFunction<SDL_PumpEventsDelegate>("SDL_PumpEvents");
        public static void SDL_PumpEvents() => pSDL_PumpEvents();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_PollEvent", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_PollEvent(out SDL_Event @event);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_PollEventDelegate(out SDL_Event @event);
        private static readonly SDL_PollEventDelegate pSDL_PollEvent = lib.LoadFunction<SDL_PollEventDelegate>("SDL_PollEvent");
        public static Int32 SDL_PollEvent(out SDL_Event @event) => pSDL_PollEvent(out @event);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetEventFilter", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetEventFilter(IntPtr filter, IntPtr userdata);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetEventFilterDelegate(IntPtr filter, IntPtr userdata);
        private static readonly SDL_SetEventFilterDelegate pSDL_SetEventFilter = lib.LoadFunction<SDL_SetEventFilterDelegate>("SDL_SetEventFilter");
        public static void SDL_SetEventFilter(IntPtr filter, IntPtr userdata) => pSDL_SetEventFilter(filter, userdata);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_CreateWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateWindow([MarshalAs(UnmanagedType.LPStr)] String title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_CreateWindowDelegate([MarshalAs(UnmanagedType.LPStr)] String title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags);
        private static readonly SDL_CreateWindowDelegate pSDL_CreateWindow = lib.LoadFunction<SDL_CreateWindowDelegate>("SDL_CreateWindow");
        public static IntPtr SDL_CreateWindow(String title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags) => pSDL_CreateWindow(title, x, y, w, h, flags);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_CreateWindowFrom", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateWindowFrom(IntPtr data);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_CreateWindowFromDelegate(IntPtr data);
        private static readonly SDL_CreateWindowFromDelegate pSDL_CreateWindowFrom = lib.LoadFunction<SDL_CreateWindowFromDelegate>("SDL_CreateWindowFrom");
        public static IntPtr SDL_CreateWindowFrom(IntPtr data) => pSDL_CreateWindowFrom(data);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_DestroyWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyWindow(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_DestroyWindowDelegate(IntPtr window);
        private static readonly SDL_DestroyWindowDelegate pSDL_DestroyWindow = lib.LoadFunction<SDL_DestroyWindowDelegate>("SDL_DestroyWindow");
        public static void SDL_DestroyWindow(IntPtr window) => pSDL_DestroyWindow(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowID", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_GetWindowID(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UInt32 SDL_GetWindowIDDelegate(IntPtr window);
        private static readonly SDL_GetWindowIDDelegate pSDL_GetWindowID = lib.LoadFunction<SDL_GetWindowIDDelegate>("SDL_GetWindowID");
        public static UInt32 SDL_GetWindowID(IntPtr window) => pSDL_GetWindowID(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GetWindowTitle_Impl(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GetWindowTitleDelegate(IntPtr window);
        private static readonly SDL_GetWindowTitleDelegate pSDL_GetWindowTitle = lib.LoadFunction<SDL_GetWindowTitleDelegate>("SDL_GetWindowTitle");
        private static IntPtr SDL_GetWindowTitle_Impl(IntPtr window) => pSDL_GetWindowTitle(window);
#endif
        public static String SDL_GetWindowTitle(IntPtr window) => Marshal.PtrToStringAnsi(SDL_GetWindowTitle_Impl(window));

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowTitle", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowTitle(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] String title);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowTitleDelegate(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] String title);
        private static readonly SDL_SetWindowTitleDelegate pSDL_SetWindowTitle = lib.LoadFunction<SDL_SetWindowTitleDelegate>("SDL_SetWindowTitle");
        public static void SDL_SetWindowTitle(IntPtr window, String title) => pSDL_SetWindowTitle(window, title);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowIcon", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowIcon(IntPtr window, IntPtr icon);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowIconDelegate(IntPtr window, IntPtr icon);
        private static readonly SDL_SetWindowIconDelegate pSDL_SetWindowIcon = lib.LoadFunction<SDL_SetWindowIconDelegate>("SDL_SetWindowIcon");
        public static void SDL_SetWindowIcon(IntPtr window, IntPtr icon) => pSDL_SetWindowIcon(window, icon);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowPosition", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowPosition(IntPtr window, out Int32 x, out Int32 y);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GetWindowPositionDelegate(IntPtr window, out Int32 x, out Int32 y);
        private static readonly SDL_GetWindowPositionDelegate pSDL_GetWindowPosition = lib.LoadFunction<SDL_GetWindowPositionDelegate>("SDL_GetWindowPosition");
        public static void SDL_GetWindowPosition(IntPtr window, out Int32 x, out Int32 y) => pSDL_GetWindowPosition(window, out x, out y);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowPosition", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowPosition(IntPtr window, Int32 x, Int32 y);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowPositionDelegate(IntPtr window, Int32 x, Int32 y);
        private static readonly SDL_SetWindowPositionDelegate pSDL_SetWindowPosition = lib.LoadFunction<SDL_SetWindowPositionDelegate>("SDL_SetWindowPosition");
        public static void SDL_SetWindowPosition(IntPtr window, Int32 x, Int32 y) => pSDL_SetWindowPosition(window, x, y);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowSize(IntPtr window, out Int32 w, out Int32 h);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GetWindowSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private static readonly SDL_GetWindowSizeDelegate pSDL_GetWindowSize = lib.LoadFunction<SDL_GetWindowSizeDelegate>("SDL_GetWindowSize");
        public static void SDL_GetWindowSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GetWindowSize(window, out w, out h);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowSize(IntPtr window, Int32 w, Int32 h);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowSizeDelegate(IntPtr window, Int32 w, Int32 h);
        private static readonly SDL_SetWindowSizeDelegate pSDL_SetWindowSize = lib.LoadFunction<SDL_SetWindowSizeDelegate>("SDL_SetWindowSize");
        public static void SDL_SetWindowSize(IntPtr window, Int32 w, Int32 h) => pSDL_SetWindowSize(window, w, h);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowMinimumSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowMinimumSize(IntPtr window, out Int32 w, out Int32 h);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GetWindowMinimumSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private static readonly SDL_GetWindowMinimumSizeDelegate pSDL_GetWindowMinimumSize = lib.LoadFunction<SDL_GetWindowMinimumSizeDelegate>("SDL_GetWindowMinimumSize");
        public static void SDL_GetWindowMinimumSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GetWindowMinimumSize(window, out w, out h);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowMinimumSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowMinimumSize(IntPtr window, Int32 w, Int32 h);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowMinimumSizeDelegate(IntPtr window, Int32 w, Int32 h);
        private static readonly SDL_SetWindowMinimumSizeDelegate pSDL_SetWindowMinimumSize = lib.LoadFunction<SDL_SetWindowMinimumSizeDelegate>("SDL_SetWindowMinimumSize");
        public static void SDL_SetWindowMinimumSize(IntPtr window, Int32 w, Int32 h) => pSDL_SetWindowMinimumSize(window, w, h);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowMaximumSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowMaximumSize(IntPtr window, out Int32 w, out Int32 h);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GetWindowMaximumSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private static readonly SDL_GetWindowMaximumSizeDelegate pSDL_GetWindowMaximumSize = lib.LoadFunction<SDL_GetWindowMaximumSizeDelegate>("SDL_GetWindowMaximumSize");
        public static void SDL_GetWindowMaximumSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GetWindowMaximumSize(window, out w, out h);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowMaximumSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowMaximumSize(IntPtr window, Int32 w, Int32 h);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowMaximumSizeDelegate(IntPtr window, Int32 w, Int32 h);
        private static readonly SDL_SetWindowMaximumSizeDelegate pSDL_SetWindowMaximumSize = lib.LoadFunction<SDL_SetWindowMaximumSizeDelegate>("SDL_SetWindowMaximumSize");
        public static void SDL_SetWindowMaximumSize(IntPtr window, Int32 w, Int32 h) => pSDL_SetWindowMaximumSize(window, w, h);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowGrab", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GetWindowGrab(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GetWindowGrabDelegate(IntPtr window);
        private static readonly SDL_GetWindowGrabDelegate pSDL_GetWindowGrab = lib.LoadFunction<SDL_GetWindowGrabDelegate>("SDL_GetWindowGrab");
        public static void SDL_GetWindowGrab(IntPtr window) => pSDL_GetWindowGrab(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowGrab", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowGrab(IntPtr window, Boolean grabbed);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowGrabDelegate(IntPtr window, Boolean grabbed);
        private static readonly SDL_SetWindowGrabDelegate pSDL_SetWindowGrab = lib.LoadFunction<SDL_SetWindowGrabDelegate>("SDL_SetWindowGrab");
        public static void SDL_SetWindowGrab(IntPtr window, Boolean grabbed) => pSDL_SetWindowGrab(window, grabbed);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowBordered", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowBordered(IntPtr window, Boolean bordered);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetWindowBorderedDelegate(IntPtr window, Boolean bordered);
        private static readonly SDL_SetWindowBorderedDelegate pSDL_SetWindowBordered = lib.LoadFunction<SDL_SetWindowBorderedDelegate>("SDL_SetWindowBordered");
        public static void SDL_SetWindowBordered(IntPtr window, Boolean bordered) => pSDL_SetWindowBordered(window, bordered);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowFullscreen", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_SetWindowFullscreen(IntPtr window, UInt32 flags);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SetWindowFullscreenDelegate(IntPtr window, UInt32 flags);
        private static readonly SDL_SetWindowFullscreenDelegate pSDL_SetWindowFullscreen = lib.LoadFunction<SDL_SetWindowFullscreenDelegate>("SDL_SetWindowFullscreen");
        public static Int32 SDL_SetWindowFullscreen(IntPtr window, UInt32 flags) => pSDL_SetWindowFullscreen(window, flags);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_SetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SetWindowDisplayModeDelegate(IntPtr window, SDL_DisplayMode* mode);
        private static readonly SDL_SetWindowDisplayModeDelegate pSDL_SetWindowDisplayMode = lib.LoadFunction<SDL_SetWindowDisplayModeDelegate>("SDL_SetWindowDisplayMode");
        public static Int32 SDL_SetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode) => pSDL_SetWindowDisplayMode(window, mode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetWindowDisplayModeDelegate(IntPtr window, SDL_DisplayMode* mode);
        private static readonly SDL_GetWindowDisplayModeDelegate pSDL_GetWindowDisplayMode = lib.LoadFunction<SDL_GetWindowDisplayModeDelegate>("SDL_GetWindowDisplayMode");
        public static Int32 SDL_GetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode) => pSDL_GetWindowDisplayMode(window, mode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowDisplayIndex", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetWindowDisplayIndex(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetWindowDisplayIndexDelegate(IntPtr window);
        private static readonly SDL_GetWindowDisplayIndexDelegate pSDL_GetWindowDisplayIndex = lib.LoadFunction<SDL_GetWindowDisplayIndexDelegate>("SDL_GetWindowDisplayIndex");
        public static Int32 SDL_GetWindowDisplayIndex(IntPtr window) => pSDL_GetWindowDisplayIndex(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowFlags", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_WindowFlags SDL_GetWindowFlags(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_WindowFlags SDL_GetWindowFlagsDelegate(IntPtr window);
        private static readonly SDL_GetWindowFlagsDelegate pSDL_GetWindowFlags = lib.LoadFunction<SDL_GetWindowFlagsDelegate>("SDL_GetWindowFlags");
        public static SDL_WindowFlags SDL_GetWindowFlags(IntPtr window) => pSDL_GetWindowFlags(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_ShowWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ShowWindow(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_ShowWindowDelegate(IntPtr window);
        private static readonly SDL_ShowWindowDelegate pSDL_ShowWindow = lib.LoadFunction<SDL_ShowWindowDelegate>("SDL_ShowWindow");
        public static void SDL_ShowWindow(IntPtr window) => pSDL_ShowWindow(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_HideWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_HideWindow(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_HideWindowDelegate(IntPtr window);
        private static readonly SDL_HideWindowDelegate pSDL_HideWindow = lib.LoadFunction<SDL_HideWindowDelegate>("SDL_HideWindow");
        public static void SDL_HideWindow(IntPtr window) => pSDL_HideWindow(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_MaximizeWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MaximizeWindow(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_MaximizeWindowDelegate(IntPtr window);
        private static readonly SDL_MaximizeWindowDelegate pSDL_MaximizeWindow = lib.LoadFunction<SDL_MaximizeWindowDelegate>("SDL_MaximizeWindow");
        public static void SDL_MaximizeWindow(IntPtr window) => pSDL_MaximizeWindow(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_MinimizeWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_MinimizeWindow(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_MinimizeWindowDelegate(IntPtr window);
        private static readonly SDL_MinimizeWindowDelegate pSDL_MinimizeWindow = lib.LoadFunction<SDL_MinimizeWindowDelegate>("SDL_MinimizeWindow");
        public static void SDL_MinimizeWindow(IntPtr window) => pSDL_MinimizeWindow(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_RestoreWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RestoreWindow(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_RestoreWindowDelegate(IntPtr window);
        private static readonly SDL_RestoreWindowDelegate pSDL_RestoreWindow = lib.LoadFunction<SDL_RestoreWindowDelegate>("SDL_RestoreWindow");
        public static void SDL_RestoreWindow(IntPtr window) => pSDL_RestoreWindow(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowWMInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean SDL_GetWindowWMInfo(IntPtr window, SDL_SysWMinfo* info);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_GetWindowWMInfoDelegate(IntPtr window, SDL_SysWMinfo* info);
        private static readonly SDL_GetWindowWMInfoDelegate pSDL_GetWindowWMInfo = lib.LoadFunction<SDL_GetWindowWMInfoDelegate>("SDL_GetWindowWMInfo");
        public static Boolean SDL_GetWindowWMInfo(IntPtr window, SDL_SysWMinfo* info) => pSDL_GetWindowWMInfo(window, info);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_RWFromFile", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromFile([MarshalAs(UnmanagedType.LPStr)] String file, [MarshalAs(UnmanagedType.LPStr)] String mode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_RWFromFileDelegate([MarshalAs(UnmanagedType.LPStr)] String file, [MarshalAs(UnmanagedType.LPStr)] String mode);
        private static readonly SDL_RWFromFileDelegate pSDL_RWFromFile = lib.LoadFunction<SDL_RWFromFileDelegate>("SDL_RWFromFile");
        public static IntPtr SDL_RWFromFile(String file, String mode) => pSDL_RWFromFile(file, mode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_RWFromMem", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_RWFromMem(IntPtr mem, Int32 size);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_RWFromMemDelegate(IntPtr mem, Int32 size);
        private static readonly SDL_RWFromMemDelegate pSDL_RWFromMem = lib.LoadFunction<SDL_RWFromMemDelegate>("SDL_RWFromMem");
        public static IntPtr SDL_RWFromMem(IntPtr mem, Int32 size) => pSDL_RWFromMem(mem, size);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_AllocRW", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_AllocRW();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_AllocRWDelegate();
        private static readonly SDL_AllocRWDelegate pSDL_AllocRW = lib.LoadFunction<SDL_AllocRWDelegate>("SDL_AllocRW");
        public static IntPtr SDL_AllocRW() => pSDL_AllocRW();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_FreeRW", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeRW(IntPtr area);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_FreeRWDelegate(IntPtr area);
        private static readonly SDL_FreeRWDelegate pSDL_FreeRW = lib.LoadFunction<SDL_FreeRWDelegate>("SDL_FreeRW");
        public static void SDL_FreeRW(IntPtr area) => pSDL_FreeRW(area);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_LoadBMP_RW", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Surface* SDL_LoadBMP_RW(IntPtr src, Int32 freesrc);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Surface* SDL_LoadBMP_RWDelegate(IntPtr src, Int32 freesrc);
        private static readonly SDL_LoadBMP_RWDelegate pSDL_LoadBMP_RW = lib.LoadFunction<SDL_LoadBMP_RWDelegate>("SDL_LoadBMP_RW");
        public static SDL_Surface* SDL_LoadBMP_RW(IntPtr src, Int32 freesrc) => pSDL_LoadBMP_RW(src, freesrc);
#endif
        public static SDL_Surface* SDL_LoadBMP(String file) => SDL_LoadBMP_RW(SDL_RWFromFile(file, "r"), 1);

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SaveBMP_RW", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_SaveBMP_RW(SDL_Surface* surface, IntPtr dst, Int32 freedst);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SaveBMP_RWDelegate(SDL_Surface* surface, IntPtr dst, Int32 freedst);
        private static readonly SDL_SaveBMP_RWDelegate pSDL_SaveBMP_RW = lib.LoadFunction<SDL_SaveBMP_RWDelegate>("SDL_SaveBMP_RW");
        public static Int32 SDL_SaveBMP_RW(SDL_Surface* surface, IntPtr dst, Int32 freedst) => pSDL_SaveBMP_RW(surface, dst, freedst);
#endif
        public static Int32 SDL_SaveBMP(SDL_Surface* surface, String file) => SDL_SaveBMP_RW(surface, SDL_RWFromFile(file, "wb"), 1);

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetMouseState", CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 SDL_GetMouseState(out Int32 x, out Int32 y);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate UInt32 SDL_GetMouseStateDelegate(out Int32 x, out Int32 y);
        private static readonly SDL_GetMouseStateDelegate pSDL_GetMouseState = lib.LoadFunction<SDL_GetMouseStateDelegate>("SDL_GetMouseState");
        public static UInt32 SDL_GetMouseState(out Int32 x, out Int32 y) => pSDL_GetMouseState(out x, out y);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetKeyboardState", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetKeyboardState(out Int32 numkeys);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GetKeyboardStateDelegate(out Int32 numkeys);
        private static readonly SDL_GetKeyboardStateDelegate pSDL_GetKeyboardState = lib.LoadFunction<SDL_GetKeyboardStateDelegate>("SDL_GetKeyboardState");
        public static IntPtr SDL_GetKeyboardState(out Int32 numkeys) => pSDL_GetKeyboardState(out numkeys);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetScancodeFromKey", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Scancode SDL_GetScancodeFromKey(SDL_Keycode keycode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Scancode SDL_GetScancodeFromKeyDelegate(SDL_Keycode keycode);
        private static readonly SDL_GetScancodeFromKeyDelegate pSDL_GetScancodeFromKey = lib.LoadFunction<SDL_GetScancodeFromKeyDelegate>("SDL_GetScancodeFromKey");
        public static SDL_Scancode SDL_GetScancodeFromKey(SDL_Keycode keycode) => pSDL_GetScancodeFromKey(keycode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetModState", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Keymod SDL_GetModState();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Keymod SDL_GetModStateDelegate();
        private static readonly SDL_GetModStateDelegate pSDL_GetModState = lib.LoadFunction<SDL_GetModStateDelegate>("SDL_GetModState");
        public static SDL_Keymod SDL_GetModState() => pSDL_GetModState();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetHint", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean SDL_SetHint([MarshalAs(UnmanagedType.LPStr)] String name, [MarshalAs(UnmanagedType.LPStr)] String value);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_SetHintDelegate([MarshalAs(UnmanagedType.LPStr)] String name, [MarshalAs(UnmanagedType.LPStr)] String value);
        private static readonly SDL_SetHintDelegate pSDL_SetHint = lib.LoadFunction<SDL_SetHintDelegate>("SDL_SetHint");
        public static Boolean SDL_SetHint(String name, String value) => pSDL_SetHint(name, value);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_CreateRGBSurface", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Surface* SDL_CreateRGBSurface(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 Amask);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Surface* SDL_CreateRGBSurfaceDelegate(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 Amask);
        private static readonly SDL_CreateRGBSurfaceDelegate pSDL_CreateRGBSurface = lib.LoadFunction<SDL_CreateRGBSurfaceDelegate>("SDL_CreateRGBSurface");
        public static SDL_Surface* SDL_CreateRGBSurface(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 Amask) => pSDL_CreateRGBSurface(flags, width, height, depth, Rmask, Gmask, Bmask, Amask);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_FreeSurface", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeSurface(SDL_Surface* surface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_FreeSurfaceDelegate(SDL_Surface* surface);
        private static readonly SDL_FreeSurfaceDelegate pSDL_FreeSurface = lib.LoadFunction<SDL_FreeSurfaceDelegate>("SDL_FreeSurface");
        public static void SDL_FreeSurface(SDL_Surface* surface) => pSDL_FreeSurface(surface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_LockSurface", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_LockSurface(SDL_Surface* surface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_LockSurfaceDelegate(SDL_Surface* surface);
        private static readonly SDL_LockSurfaceDelegate pSDL_LockSurface = lib.LoadFunction<SDL_LockSurfaceDelegate>("SDL_LockSurface");
        public static Int32 SDL_LockSurface(SDL_Surface* surface) => pSDL_LockSurface(surface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_UnlockSurface", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_UnlockSurface(SDL_Surface* surface);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_UnlockSurfaceDelegate(SDL_Surface* surface);
        private static readonly SDL_UnlockSurfaceDelegate pSDL_UnlockSurface = lib.LoadFunction<SDL_UnlockSurfaceDelegate>("SDL_UnlockSurface");
        public static void SDL_UnlockSurface(SDL_Surface* surface) => pSDL_UnlockSurface(surface);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_UpperBlit", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_BlitSurface(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_UpperBlitDelegate(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect);
        private static readonly SDL_UpperBlitDelegate p_SDL_UpperBlit = lib.LoadFunction<SDL_UpperBlitDelegate>("SDL_UpperBlit");
        public static Int32 SDL_BlitSurface(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect) => p_SDL_UpperBlit(src, srcrect, dst, dstrect);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_UpperBlitScaled", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_BlitScaled(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_UpperBlitScaledDelegate(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect);
        private static readonly SDL_UpperBlitScaledDelegate pSDL_UpperBlitScaled = lib.LoadFunction<SDL_UpperBlitScaledDelegate>("SDL_UpperBlitScaled");
        public static Int32 SDL_BlitScaled(SDL_Surface* src, SDL_Rect* srcrect, SDL_Surface* dst, SDL_Rect* dstrect) => pSDL_UpperBlitScaled(src, srcrect, dst, dstrect);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetSurfaceBlendMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_SetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode blendMode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SetSurfaceBlendModeDelegate(SDL_Surface* surface, SDL_BlendMode blendMode);
        private static readonly SDL_SetSurfaceBlendModeDelegate pSDL_SetSurfaceBlendMode = lib.LoadFunction<SDL_SetSurfaceBlendModeDelegate>("SDL_SetSurfaceBlendMode");
        public static Int32 SDL_SetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode blendMode) => pSDL_SetSurfaceBlendMode(surface, blendMode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetSurfaceBlendMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode* blendMode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetSurfaceBlendModeDelegate(SDL_Surface* surface, SDL_BlendMode* blendMode);
        private static readonly SDL_GetSurfaceBlendModeDelegate pSDL_GetSurfaceBlendMode = lib.LoadFunction<SDL_GetSurfaceBlendModeDelegate>("SDL_GetSurfaceBlendMode");
        public static Int32 SDL_GetSurfaceBlendMode(SDL_Surface* surface, SDL_BlendMode* blendMode) => pSDL_GetSurfaceBlendMode(surface, blendMode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_FillRect", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_FillRect(SDL_Surface* surface, SDL_Rect* rect, UInt32 color);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_FillRectDelegate(SDL_Surface* surface, SDL_Rect* rect, UInt32 color);
        private static readonly SDL_FillRectDelegate pSDL_FillRect = lib.LoadFunction<SDL_FillRectDelegate>("SDL_FillRect");
        public static Int32 SDL_FillRect(SDL_Surface* surface, SDL_Rect* rect, UInt32 color) => pSDL_FillRect(surface, rect, color);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_FillRects", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_FillRects(SDL_Surface* dst, SDL_Rect* rects, Int32 count, UInt32 colors);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_FillRectsDelegate(SDL_Surface* dst, SDL_Rect* rects, Int32 count, UInt32 colors);
        private static readonly SDL_FillRectsDelegate pSDL_FillRects = lib.LoadFunction<SDL_FillRectsDelegate>("SDL_FillRects");
        public static Int32 SDL_FillRects(SDL_Surface* dst, SDL_Rect* rects, Int32 count, UInt32 colors) => pSDL_FillRects(dst, rects, count, colors);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_CreateColorCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Cursor* SDL_CreateColorCursor(SDL_Surface* surface, Int32 hot_x, Int32 hot_y);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Cursor* SDL_CreateColorCursorDelegate(SDL_Surface* surface, Int32 hot_x, Int32 hot_y);
        private static readonly SDL_CreateColorCursorDelegate pSDL_CreateColorCursor = lib.LoadFunction<SDL_CreateColorCursorDelegate>("SDL_CreateColorCursor");
        public static SDL_Cursor* SDL_CreateColorCursor(SDL_Surface* surface, Int32 hot_x, Int32 hot_y) => pSDL_CreateColorCursor(surface, hot_x, hot_y);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_FreeCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_FreeCursor(SDL_Cursor* cursor);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_FreeCursorDelegate(SDL_Cursor* cursor);
        private static readonly SDL_FreeCursorDelegate pSDL_FreeCursor = lib.LoadFunction<SDL_FreeCursorDelegate>("SDL_FreeCursor");
        public static void SDL_FreeCursor(SDL_Cursor* cursor) => pSDL_FreeCursor(cursor);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_ShowCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_ShowCursor(Int32 toggle);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_ShowCursorDelegate(Int32 toggle);
        private static readonly SDL_ShowCursorDelegate pSDL_ShowCursor = lib.LoadFunction<SDL_ShowCursorDelegate>("SDL_ShowCursor");
        public static Int32 SDL_ShowCursor(Int32 toggle) => pSDL_ShowCursor(toggle);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Cursor* SDL_GetCursor();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Cursor* SDL_GetCursorDelegate();
        private static readonly SDL_GetCursorDelegate pSDL_GetCursor = lib.LoadFunction<SDL_GetCursorDelegate>("SDL_GetCursor");
        public static SDL_Cursor* SDL_GetCursor() => pSDL_GetCursor();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetCursor(SDL_Cursor* cursor);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetCursorDelegate(SDL_Cursor* cursor);
        private static readonly SDL_SetCursorDelegate pSDL_SetCursor = lib.LoadFunction<SDL_SetCursorDelegate>("SDL_SetCursor");
        public static void SDL_SetCursor(SDL_Cursor* cursor) => pSDL_SetCursor(cursor);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetDefaultCursor", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Cursor* SDL_GetDefaultCursor();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Cursor* SDL_GetDefaultCursorDelegate();
        private static readonly SDL_GetDefaultCursorDelegate pSDL_GetDefaultCursor = lib.LoadFunction<SDL_GetDefaultCursorDelegate>("SDL_GetDefaultCursor");
        public static SDL_Cursor* SDL_GetDefaultCursor() => pSDL_GetDefaultCursor();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetNumVideoDisplays", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetNumVideoDisplays();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetNumVideoDisplaysDelegate();
        private static readonly SDL_GetNumVideoDisplaysDelegate pSDL_GetNumVideoDisplays = lib.LoadFunction<SDL_GetNumVideoDisplaysDelegate>("SDL_GetNumVideoDisplays");
        public static Int32 SDL_GetNumVideoDisplays() => pSDL_GetNumVideoDisplays();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetDisplayName", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GetDisplayName_Impl(Int32 displayIndex);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GetDisplayNameDelegate(Int32 displayIndex);
        private static readonly SDL_GetDisplayNameDelegate pSDL_GetDisplayName = lib.LoadFunction<SDL_GetDisplayNameDelegate>("SDL_GetDisplayName");
        private static IntPtr SDL_GetDisplayName_Impl(Int32 displayIndex) => pSDL_GetDisplayName(displayIndex);
#endif
        public static String SDL_GetDisplayName(Int32 displayIndex) => Marshal.PtrToStringAnsi(SDL_GetDisplayName_Impl(displayIndex));

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetDisplayBounds", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetDisplayBounds(Int32 displayIndex, SDL_Rect* rect);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetDisplayBoundsDelegate(Int32 displayIndex, SDL_Rect* rect);
        private static readonly SDL_GetDisplayBoundsDelegate pSDL_GetDisplayBounds = lib.LoadFunction<SDL_GetDisplayBoundsDelegate>("SDL_GetDisplayBounds");
        public static Int32 SDL_GetDisplayBounds(Int32 displayIndex, SDL_Rect* rect) => pSDL_GetDisplayBounds(displayIndex, rect);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetNumDisplayModes", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetNumDisplayModes(Int32 displayIndex);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetNumDisplayModesDelegate(Int32 displayIndex);
        private static readonly SDL_GetNumDisplayModesDelegate pSDL_GetNumDisplayModes = lib.LoadFunction<SDL_GetNumDisplayModesDelegate>("SDL_GetNumDisplayModes");
        public static Int32 SDL_GetNumDisplayModes(Int32 displayIndex) => pSDL_GetNumDisplayModes(displayIndex);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetDisplayMode(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetDisplayModeDelegate(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode);
        private static readonly SDL_GetDisplayModeDelegate pSDL_GetDisplayMode = lib.LoadFunction<SDL_GetDisplayModeDelegate>("SDL_GetDisplayMode");
        public static Int32 SDL_GetDisplayMode(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode) => pSDL_GetDisplayMode(displayIndex, modeIndex, mode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetCurrentDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetCurrentDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetCurrentDisplayModeDelegate(Int32 displayIndex, SDL_DisplayMode* mode);
        private static readonly SDL_GetCurrentDisplayModeDelegate pSDL_GetCurrentDisplayMode = lib.LoadFunction<SDL_GetCurrentDisplayModeDelegate>("SDL_GetCurrentDisplayMode");
        public static Int32 SDL_GetCurrentDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode) => pSDL_GetCurrentDisplayMode(displayIndex, mode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetDesktopDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetDesktopDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetDesktopDisplayModeDelegate(Int32 displayIndex, SDL_DisplayMode* mode);
        private static readonly SDL_GetDesktopDisplayModeDelegate pSDL_GetDesktopDisplayMode = lib.LoadFunction<SDL_GetDesktopDisplayModeDelegate>("SDL_GetDesktopDisplayMode");
        public static Int32 SDL_GetDesktopDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode) => pSDL_GetDesktopDisplayMode(displayIndex, mode);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetClosestDisplayMode", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_DisplayMode* SDL_GetClosestDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_DisplayMode* SDL_GetClosestDisplayModeDelegate(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest);
        private static readonly SDL_GetClosestDisplayModeDelegate pSDL_GetClosestDisplayMode = lib.LoadFunction<SDL_GetClosestDisplayModeDelegate>("SDL_GetClosestDisplayMode");
        public static SDL_DisplayMode* SDL_GetClosestDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest) => pSDL_GetClosestDisplayMode(displayIndex, mode, closest);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_PixelFormatEnumToMasks", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean SDL_PixelFormatEnumToMasks(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_PixelFormatEnumToMasksDelegate(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask);
        private static readonly SDL_PixelFormatEnumToMasksDelegate pSDL_PixelFormatEnumToMasks = lib.LoadFunction<SDL_PixelFormatEnumToMasksDelegate>("SDL_PixelFormatEnumToMasks");
        public static Boolean SDL_PixelFormatEnumToMasks(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask) => pSDL_PixelFormatEnumToMasks(format, bpp, Rmask, Gmask, Bmask, Amask);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GL_GetProcAddress", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GL_GetProcAddress([MarshalAs(UnmanagedType.LPStr)] String proc);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GL_GetProcAddressDelegate([MarshalAs(UnmanagedType.LPStr)] String proc);
        private static readonly SDL_GL_GetProcAddressDelegate pSDL_GL_GetProcAddress = lib.LoadFunction<SDL_GL_GetProcAddressDelegate>("SDL_GL_GetProcAddress");
        public static IntPtr SDL_GL_GetProcAddress(String proc) => pSDL_GL_GetProcAddress(proc);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GL_CreateContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GL_CreateContext(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GL_CreateContextDelegate(IntPtr window);
        private static readonly SDL_GL_CreateContextDelegate pSDL_GL_CreateContext = lib.LoadFunction<SDL_GL_CreateContextDelegate>("SDL_GL_CreateContext");
        public static IntPtr SDL_GL_CreateContext(IntPtr window) => pSDL_GL_CreateContext(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GL_DeleteContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_DeleteContext(IntPtr context);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GL_DeleteContextDelegate(IntPtr context);
        private static readonly SDL_GL_DeleteContextDelegate pSDL_GL_DeleteContext = lib.LoadFunction<SDL_GL_DeleteContextDelegate>("SDL_GL_DeleteContext");
        public static void SDL_GL_DeleteContext(IntPtr context) => pSDL_GL_DeleteContext(context);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GL_GetCurrentContext", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GL_GetCurrentContext();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GL_GetCurrentContextDelegate();
        private static readonly SDL_GL_GetCurrentContextDelegate pSDL_GL_GetCurrentContext = lib.LoadFunction<SDL_GL_GetCurrentContextDelegate>("SDL_GL_GetCurrentContext");
        public static IntPtr SDL_GL_GetCurrentContext() => pSDL_GL_GetCurrentContext();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GL_MakeCurrent", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GL_MakeCurrent(IntPtr window, IntPtr context);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GL_MakeCurrentDelegate(IntPtr window, IntPtr context);
        private static readonly SDL_GL_MakeCurrentDelegate pSDL_GL_MakeCurrent = lib.LoadFunction<SDL_GL_MakeCurrentDelegate>("SDL_GL_MakeCurrent");
        public static Int32 SDL_GL_MakeCurrent(IntPtr window, IntPtr context) => pSDL_GL_MakeCurrent(window, context);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GL_SetAttribute", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GL_SetAttribute(SDL_GLattr attr, Int32 value);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GL_SetAttributeDelegate(SDL_GLattr attr, Int32 value);
        private static readonly SDL_GL_SetAttributeDelegate pSDL_GL_SetAttribute = lib.LoadFunction<SDL_GL_SetAttributeDelegate>("SDL_GL_SetAttribute");
        public static Int32 SDL_GL_SetAttribute(SDL_GLattr attr, Int32 value) => pSDL_GL_SetAttribute(attr, value);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GL_GetAttribute", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GL_GetAttribute(SDL_GLattr attr, Int32* value);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GL_GetAttributeDelegate(SDL_GLattr attr, Int32* value);
        private static readonly SDL_GL_GetAttributeDelegate pSDL_GL_GetAttribute = lib.LoadFunction<SDL_GL_GetAttributeDelegate>("SDL_GL_GetAttribute");
        public static Int32 SDL_GL_GetAttribute(SDL_GLattr attr, Int32* value) => pSDL_GL_GetAttribute(attr, value);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GL_SwapWindow", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_SwapWindow(IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GL_SwapWindowDelegate(IntPtr window);
        private static readonly SDL_GL_SwapWindowDelegate pSDL_GL_SwapWindow = lib.LoadFunction<SDL_GL_SwapWindowDelegate>("SDL_GL_SwapWindow");
        public static void SDL_GL_SwapWindow(IntPtr window) => pSDL_GL_SwapWindow(window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GL_SetSwapInterval", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GL_SetSwapInterval(Int32 interval);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GL_SetSwapIntervalDelegate(Int32 interval);
        private static readonly SDL_GL_SetSwapIntervalDelegate pSDL_GL_SetSwapInterval = lib.LoadFunction<SDL_GL_SetSwapIntervalDelegate>("SDL_GL_SetSwapInterval");
        public static Int32 SDL_GL_SetSwapInterval(Int32 interval) => pSDL_GL_SetSwapInterval(interval);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GL_GetDrawableSize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GL_GetDrawableSize(IntPtr window, out Int32 w, out Int32 h);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GL_GetDrawableSizeDelegate(IntPtr window, out Int32 w, out Int32 h);
        private static readonly SDL_GL_GetDrawableSizeDelegate pSDL_GL_GetDrawableSize = lib.LoadFunction<SDL_GL_GetDrawableSizeDelegate>("SDL_GL_GetDrawableSize");
        public static void SDL_GL_GetDrawableSize(IntPtr window, out Int32 w, out Int32 h) => pSDL_GL_GetDrawableSize(window, out w, out h);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_NumJoysticks", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_NumJoysticks();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_NumJoysticksDelegate();
        private static readonly SDL_NumJoysticksDelegate pSDL_NumJoysticks = lib.LoadFunction<SDL_NumJoysticksDelegate>("SDL_NumJoysticks");
        public static Int32 SDL_NumJoysticks() => pSDL_NumJoysticks();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_IsGameController", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean SDL_IsGameController(Int32 joystick_index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_IsGameControllerDelegate(Int32 joystick_index);
        private static readonly SDL_IsGameControllerDelegate pSDL_IsGameController = lib.LoadFunction<SDL_IsGameControllerDelegate>("SDL_IsGameController");
        public static Boolean SDL_IsGameController(Int32 joystick_index) => pSDL_IsGameController(joystick_index);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GameControllerOpen", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GameControllerOpen(Int32 index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GameControllerOpenDelegate(Int32 index);
        private static readonly SDL_GameControllerOpenDelegate pSDL_GameControllerOpen = lib.LoadFunction<SDL_GameControllerOpenDelegate>("SDL_GameControllerOpen");
        public static IntPtr SDL_GameControllerOpen(Int32 index) => pSDL_GameControllerOpen(index);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GameControllerClose", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_GameControllerClose(IntPtr gamecontroller);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_GameControllerCloseDelegate(IntPtr gamecontroller);
        private static readonly SDL_GameControllerCloseDelegate pSDL_GameControllerClose = lib.LoadFunction<SDL_GameControllerCloseDelegate>("SDL_GameControllerClose");
        public static void SDL_GameControllerClose(IntPtr gamecontroller) => pSDL_GameControllerClose(gamecontroller);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GameControllerNameForIndex", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GameControllerNameForIndex_Impl(Int32 joystick_index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GameControllerNameForIndexDelegate(Int32 joystick_index);
        private static readonly SDL_GameControllerNameForIndexDelegate pSDL_GameControllerNameForIndex = lib.LoadFunction<SDL_GameControllerNameForIndexDelegate>("SDL_GameControllerNameForIndex");
        private static IntPtr SDL_GameControllerNameForIndex_Impl(Int32 joystick_index) => pSDL_GameControllerNameForIndex(joystick_index);
#endif
        public static String SDL_GameControllerNameForIndex(Int32 joystick_index) => Marshal.PtrToStringAnsi(SDL_GameControllerNameForIndex_Impl(joystick_index));

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GameControllerGetButton", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean SDL_GameControllerGetButton(IntPtr gamecontroller, SDL_GameControllerButton button);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_GameControllerGetButtonDelegate(IntPtr gamecontroller, SDL_GameControllerButton button);
        private static readonly SDL_GameControllerGetButtonDelegate pSDL_GameControllerGetButton = lib.LoadFunction<SDL_GameControllerGetButtonDelegate>("SDL_GameControllerGetButton");
        public static Boolean SDL_GameControllerGetButton(IntPtr gamecontroller, SDL_GameControllerButton button) => pSDL_GameControllerGetButton(gamecontroller, button);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GameControllerGetJoystick", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GameControllerGetJoystick(IntPtr gamecontroller);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GameControllerGetJoystickDelegate(IntPtr gamecontroller);
        private static readonly SDL_GameControllerGetJoystickDelegate pSDL_GameControllerGetJoystick = lib.LoadFunction<SDL_GameControllerGetJoystickDelegate>("SDL_GameControllerGetJoystick");
        public static IntPtr SDL_GameControllerGetJoystick(IntPtr gamecontroller) => pSDL_GameControllerGetJoystick(gamecontroller);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_JoystickInstanceID", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_JoystickInstanceID(IntPtr joystick);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_JoystickInstanceIDDelegate(IntPtr joystick);
        private static readonly SDL_JoystickInstanceIDDelegate pSDL_JoystickInstanceID = lib.LoadFunction<SDL_JoystickInstanceIDDelegate>("SDL_JoystickInstanceID");
        public static Int32 SDL_JoystickInstanceID(IntPtr joystick) => pSDL_JoystickInstanceID(joystick);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetNumTouchDevices", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetNumTouchDevices();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetNumTouchDevicesDelegate();
        private static readonly SDL_GetNumTouchDevicesDelegate pSDL_GetNumTouchDevices = lib.LoadFunction<SDL_GetNumTouchDevicesDelegate>("SDL_GetNumTouchDevices");
        public static Int32 SDL_GetNumTouchDevices() => pSDL_GetNumTouchDevices();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetTouchDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int64 SDL_GetTouchDevice(Int32 index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int64 SDL_GetTouchDeviceDelegate(Int32 index);
        private static readonly SDL_GetTouchDeviceDelegate pSDL_GetTouchDevice = lib.LoadFunction<SDL_GetTouchDeviceDelegate>("SDL_GetTouchDevice");
        public static Int64 SDL_GetTouchDevice(Int32 index) => pSDL_GetTouchDevice(index);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetNumTouchFingers", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetNumTouchFingers(Int64 touchID);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetNumTouchFingersDelegate(Int64 touchID);
        private static readonly SDL_GetNumTouchFingersDelegate pSDL_GetNumTouchFingers = lib.LoadFunction<SDL_GetNumTouchFingersDelegate>("SDL_GetNumTouchFingers");
        public static Int32 SDL_GetNumTouchFingers(Int64 touchID) => pSDL_GetNumTouchFingers(touchID);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetTouchFinger", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_Finger* SDL_GetTouchFinger(Int64 touchID, Int32 index);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_Finger* SDL_GetTouchFingerDelegate(Int64 touchID, Int32 index);
        private static readonly SDL_GetTouchFingerDelegate pSDL_GetTouchFinger = lib.LoadFunction<SDL_GetTouchFingerDelegate>("SDL_GetTouchFinger");
        public static SDL_Finger* SDL_GetTouchFinger(Int64 touchID, Int32 index) => pSDL_GetTouchFinger(touchID, index);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_RecordGesture", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_RecordGesture(Int64 touchID);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_RecordGestureDelegate(Int64 touchID);
        private static readonly SDL_RecordGestureDelegate pSDL_RecordGesture = lib.LoadFunction<SDL_RecordGestureDelegate>("SDL_RecordGesture");
        public static Int32 SDL_RecordGesture(Int64 touchID) => pSDL_RecordGesture(touchID);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SaveAllDollarTemplates", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_SaveAllDollarTemplates(IntPtr dst);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SaveAllDollarTemplatesDelegate(IntPtr dst);
        private static readonly SDL_SaveAllDollarTemplatesDelegate pSDL_SaveAllDollarTemplates = lib.LoadFunction<SDL_SaveAllDollarTemplatesDelegate>("SDL_SaveAllDollarTemplates");
        public static Int32 SDL_SaveAllDollarTemplates(IntPtr dst) => pSDL_SaveAllDollarTemplates(dst);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SaveDollarTemplate", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_SaveDollarTemplate(Int64 gestureID, IntPtr dst);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SaveDollarTemplateDelegate(Int64 gestureID, IntPtr dst);
        private static readonly SDL_SaveDollarTemplateDelegate pSDL_SaveDollarTemplate = lib.LoadFunction<SDL_SaveDollarTemplateDelegate>("SDL_SaveDollarTemplate");
        public static Int32 SDL_SaveDollarTemplate(Int64 gestureID, IntPtr dst) => pSDL_SaveDollarTemplate(gestureID, dst);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_LoadDollarTemplates", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_LoadDollarTemplates(Int64 touchID, IntPtr src);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_LoadDollarTemplatesDelegate(Int64 touchID, IntPtr src);
        private static readonly SDL_LoadDollarTemplatesDelegate pSDL_LoadDollarTemplates = lib.LoadFunction<SDL_LoadDollarTemplatesDelegate>("SDL_LoadDollarTemplates");
        public static Int32 SDL_LoadDollarTemplates(Int64 touchID, IntPtr src) => pSDL_LoadDollarTemplates(touchID, src);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_StartTextInput", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_StartTextInput();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_StartTextInputDelegate();
        private static readonly SDL_StartTextInputDelegate pSDL_StartTextInput = lib.LoadFunction<SDL_StartTextInputDelegate>("SDL_StartTextInput");
        public static void SDL_StartTextInput() => pSDL_StartTextInput();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_StopTextInput", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_StopTextInput();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_StopTextInputDelegate();
        private static readonly SDL_StopTextInputDelegate pSDL_StopTextInput = lib.LoadFunction<SDL_StopTextInputDelegate>("SDL_StopTextInput");
        public static void SDL_StopTextInput() => pSDL_StopTextInput();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetTextInputRect", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetTextInputRect(SDL_Rect* rect);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetTextInputRectDelegate(SDL_Rect* rect);
        private static readonly SDL_SetTextInputRectDelegate pSDL_SetTextInputRect = lib.LoadFunction<SDL_SetTextInputRectDelegate>("SDL_SetTextInputRect");
        public static void SDL_SetTextInputRect(SDL_Rect* rect) => pSDL_SetTextInputRect(rect);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_HasClipboardText", CallingConvention = CallingConvention.Cdecl)]
        public static extern Boolean SDL_HasClipboardText();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Boolean SDL_HasClipboardTextDelegate();
        private static readonly SDL_HasClipboardTextDelegate pSDL_HasClipboardText = lib.LoadFunction<SDL_HasClipboardTextDelegate>("SDL_HasClipboardText");
        public static Boolean SDL_HasClipboardText() => pSDL_HasClipboardText();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetClipboardText", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_GetClipboardText();
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GetClipboardTextDelegate();
        private static readonly SDL_GetClipboardTextDelegate pSDL_GetClipboardText = lib.LoadFunction<SDL_GetClipboardTextDelegate>("SDL_GetClipboardText");
        public static IntPtr SDL_GetClipboardText() => pSDL_GetClipboardText();
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetClipboardText", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetClipboardText([MarshalAs(UnmanagedType.LPStr)] String text);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_SetClipboardTextDelegate([MarshalAs(UnmanagedType.LPStr)] String text);
        private static readonly SDL_SetClipboardTextDelegate pSDL_SetClipboardText = lib.LoadFunction<SDL_SetClipboardTextDelegate>("SDL_SetClipboardText");
        public static void SDL_SetClipboardText(String text) => pSDL_SetClipboardText(text);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetPowerInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_PowerState SDL_GetPowerInfo(int* secs, int* pct);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate SDL_PowerState SDL_GetPowerInfoDelegate(int* secs, int* pct);
        private static readonly SDL_GetPowerInfoDelegate pSDL_GetPowerInfo = lib.LoadFunction<SDL_GetPowerInfoDelegate>("SDL_GetPowerInfo");
        public static SDL_PowerState SDL_GetPowerInfo(int* secs, int* pct) => pSDL_GetPowerInfo(secs, pct);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_ShowSimpleMessageBox", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_ShowSimpleMessageBox(UInt32 flags, [MarshalAs(UnmanagedType.LPStr)] String title, [MarshalAs(UnmanagedType.LPStr)] String message, IntPtr window);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_ShowSimpleMessageBoxDelegate(UInt32 flags, [MarshalAs(UnmanagedType.LPStr)] String title, [MarshalAs(UnmanagedType.LPStr)] String message, IntPtr window);
        private static readonly SDL_ShowSimpleMessageBoxDelegate pSDL_ShowSimpleMessageBox = lib.LoadFunction<SDL_ShowSimpleMessageBoxDelegate>("SDL_ShowSimpleMessageBox");
        public static Int32 SDL_ShowSimpleMessageBox(UInt32 flags, String title, String message, IntPtr window) => pSDL_ShowSimpleMessageBox(flags, title, message, window);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_SetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_SetWindowOpacity(IntPtr window, Single opacity);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_SetWindowOpacityDelegate(IntPtr window, Single opacity);
        private static readonly SDL_SetWindowOpacityDelegate pSDL_SetWindowOpacity = lib.LoadFunction<SDL_SetWindowOpacityDelegate>("SDL_SetWindowOpacity");
        public static Int32 SDL_SetWindowOpacity(IntPtr window, Single opacity) => pSDL_SetWindowOpacity(window, opacity);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetWindowOpacity", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetWindowOpacity(IntPtr window, Single* opacity);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetWindowOpacityDelegate(IntPtr window, Single* opacity);
        private static readonly SDL_GetWindowOpacityDelegate pSDL_GetWindowOpacity = lib.LoadFunction<SDL_GetWindowOpacityDelegate>("SDL_GetWindowOpacity");
        public static Int32 SDL_GetWindowOpacity(IntPtr window, Single* opacity) => pSDL_GetWindowOpacity(window, opacity);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GameControllerAddMapping", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GameControllerAddMapping([MarshalAs(UnmanagedType.LPStr)] String mappingString);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GameControllerAddMappingDelegate([MarshalAs(UnmanagedType.LPStr)] String mappingString);
        private static readonly SDL_GameControllerAddMappingDelegate pSDL_GameControllerAddMapping = lib.LoadFunction<SDL_GameControllerAddMappingDelegate>("SDL_GameControllerAddMapping");
        public static Int32 SDL_GameControllerAddMapping([MarshalAs(UnmanagedType.LPStr)] String mappingString) => pSDL_GameControllerAddMapping(mappingString);
#endif
        
#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GameControllerAddMappingsFromRW", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GameControllerAddMappingsFromRW(IntPtr rw, Int32 freerw);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GameControllerAddMappingsFromRWDelegate(IntPtr rw, Int32 freerw);
        private static readonly SDL_GameControllerAddMappingsFromRWDelegate pSDL_GameControllerAddMappingsFromRW = lib.LoadFunction<SDL_GameControllerAddMappingsFromRWDelegate>("SDL_GameControllerAddMappingsFromRW");
        public static Int32 SDL_GameControllerAddMappingsFromRW(IntPtr rw, Int32 freerw) => pSDL_GameControllerAddMappingsFromRW(rw, freerw);
#endif
        public static Int32 SDL_GameControllerAddMappingsFromFile(String file) => SDL_GameControllerAddMappingsFromRW(SDL_RWFromFile(file, "rb"), 1);

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GameControllerMapping", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GameControllerMapping_Impl(IntPtr gamecontroller);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GameControllerMappingDelegate(IntPtr gamecontroller);
        private static readonly SDL_GameControllerMappingDelegate pSDL_GameControllerMapping = lib.LoadFunction<SDL_GameControllerMappingDelegate>("SDL_GameControllerMapping");
        private static IntPtr SDL_GameControllerMapping_Impl(IntPtr gamecontroller) => pSDL_GameControllerMapping(gamecontroller);
#endif      
        public static String SDL_GameControllerMapping(IntPtr gamecontroller) => Marshal.PtrToStringAnsi(SDL_GameControllerMapping_Impl(gamecontroller));

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GameControllerMappingForGUID", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr SDL_GameControllerMappingForGUID_Impl(Guid guid);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr SDL_GameControllerMappingForGUIDDelegate(Guid guid);
        private static readonly SDL_GameControllerMappingForGUIDDelegate pSDL_GameControllerMappingForGUID = lib.LoadFunction<SDL_GameControllerMappingForGUIDDelegate>("SDL_GameControllerMappingForGUID");
        private static IntPtr SDL_GameControllerMappingForGUID_Impl(Guid guid) => pSDL_GameControllerMappingForGUID(guid);
#endif
        public static String SDL_GameControllerMappingForGUID(Guid guid) => Marshal.PtrToStringAnsi(SDL_GameControllerMappingForGUID_Impl(SDL_JoystickGUID.Marshal(guid)));

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_JoystickGetGUID", CallingConvention = CallingConvention.Cdecl)]
        private static extern Guid SDL_JoystickGetGUID_Impl(IntPtr joystick);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Guid SDL_JoystickGetGUIDDelegate(IntPtr joystick);
        private static readonly SDL_JoystickGetGUIDDelegate pSDL_JoystickGetGUID = lib.LoadFunction<SDL_JoystickGetGUIDDelegate>("SDL_JoystickGetGUID");
        private static Guid SDL_JoystickGetGUID_Impl(IntPtr joystick) => pSDL_JoystickGetGUID(joystick);
#endif
        public static Guid SDL_JoystickGetGUID(IntPtr joystick) => SDL_JoystickGUID.Marshal(SDL_JoystickGetGUID_Impl(joystick));

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_JoystickGetGUIDFromString", CallingConvention = CallingConvention.Cdecl)]
        private static extern Guid SDL_JoystickGetGUIDFromString_Impl([MarshalAs(UnmanagedType.LPStr)] String pchGUID);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Guid SDL_JoystickGetGUIDFromStringDelegate([MarshalAs(UnmanagedType.LPStr)] String pchGUID);
        private static readonly SDL_JoystickGetGUIDFromStringDelegate pSDL_JoystickGetGUIDFromString = lib.LoadFunction<SDL_JoystickGetGUIDFromStringDelegate>("SDL_JoystickGetGUIDFromString");
        private static Guid SDL_JoystickGetGUIDFromString_Impl([MarshalAs(UnmanagedType.LPStr)] String pchGUID) => pSDL_JoystickGetGUIDFromString(pchGUID);
#endif
        public static Guid SDL_JoystickGetGUIDFromString(String pchGUID) => SDL_JoystickGUID.Marshal(SDL_JoystickGetGUIDFromString_Impl(pchGUID));

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_GetDisplayDPI", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 SDL_GetDisplayDPI(Int32 displayIndex, Single* ddpi, Single* hdpi, Single *vdpi);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate Int32 SDL_GetDisplayDPIDelegate(Int32 displayIndex, Single* ddpi, Single* hdpi, Single *vdpi);
        private static readonly SDL_GetDisplayDPIDelegate pSDL_GetDisplayDPI = lib.LoadFunction<SDL_GetDisplayDPIDelegate>("SDL_GetDisplayDPI");
        public static Int32 SDL_GetDisplayDPI(Int32 displayIndex, Single* ddpi, Single* hdpi, Single *vdpi) => pSDL_GetDisplayDPI(displayIndex, ddpi, hdpi, vdpi);
#endif

#if ANDROID || IOS
        [DllImport(LIBRARY, EntryPoint="SDL_free", CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_free(IntPtr mem);
#else
        [MonoNativeFunctionWrapper]
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void SDL_freeDelegate(IntPtr mem);
        private static readonly SDL_freeDelegate pSDL_free = lib.LoadFunction<SDL_freeDelegate>("SDL_free");
        public static void SDL_free(IntPtr mem) => pSDL_free(mem);
#endif
    }
#pragma warning restore 1591
}