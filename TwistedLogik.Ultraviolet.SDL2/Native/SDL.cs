using System;
using System.Runtime.InteropServices;
using System.Security;
using TwistedLogik.Nucleus;

#pragma warning disable 1591

namespace TwistedLogik.Ultraviolet.SDL2.Native
{
    /// <summary>
    /// Contains bindings for native SDL2 function calls.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public unsafe static class SDL
    {
#if IOS
        const String LibraryPath = "__Internal";
#else
        const String LibraryPath = "SDL2";
#endif

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int32 EventFilter(IntPtr userdata, SDL_Event* @event);

        static SDL()
        {
            LibraryLoader.Load("SDL2");
        }

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetError")]
        private static extern IntPtr GetError_Impl();

        public static String GetError()
        {
            return Marshal.PtrToStringAnsi(GetError_Impl());
        }

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ClearError")]
        public static extern void ClearError();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_Init")]
        public static extern Int32 Init(SDL_Init flags);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_Quit")]
        public static extern void Quit();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_PumpEvents")]
        public static extern void PumpEvents();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_PollEvent")]
        public static extern Int32 PollEvent(out SDL_Event @event);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetEventFilter")]
        public static extern void SetEventFilter(IntPtr filter, IntPtr userdata);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateWindow", BestFitMapping = false)]
        public static extern IntPtr CreateWindow([MarshalAs(UnmanagedType.LPStr)] String title, Int32 x, Int32 y, Int32 w, Int32 h, SDL_WindowFlags flags);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateWindowFrom")]
        public static extern IntPtr CreateWindowFrom(IntPtr data);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_DestroyWindow")]
        public static extern void DestroyWindow(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowID")]
        public static extern UInt32 GetWindowID(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowTitle")]
        public static extern IntPtr GetWindowTitle_Impl(IntPtr window);

        public static String GetWindowTitle(IntPtr window)
        {
            return Marshal.PtrToStringAnsi(GetWindowTitle_Impl(window));
        }

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowTitle", BestFitMapping = false)]
        public static extern void SetWindowTitle(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] String title);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowIcon")]
        public static extern void SetWindowIcon(IntPtr window, IntPtr icon);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowPosition")]
        public static extern void GetWindowPosition(IntPtr window, out Int32 x, out Int32 y);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowPosition")]
        public static extern void SetWindowPosition(IntPtr window, Int32 x, Int32 y);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowSize")]
        public static extern void GetWindowSize(IntPtr window, out Int32 w, out Int32 h);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowSize")]
        public static extern void SetWindowSize(IntPtr window, Int32 w, Int32 h);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowMinimumSize")]
        public static extern void GetWindowMinimumSize(IntPtr window, out Int32 w, out Int32 h);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowMinimumSize")]
        public static extern void SetWindowMinimumSize(IntPtr window, Int32 w, Int32 h);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowMaximumSize")]
        public static extern void GetWindowMaximumSize(IntPtr window, out Int32 w, out Int32 h);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowMaximumSize")]
        public static extern void SetWindowMaximumSize(IntPtr window, Int32 w, Int32 h);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowGrab")]
        public static extern bool GetWindowGrab(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowGrab")]
        public static extern void SetWindowGrab(IntPtr window, Boolean grabbed);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowBordered")]
        public static extern void SetWindowBordered(IntPtr window, Boolean bordered);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowFullscreen")]
        public static extern Int32 SetWindowFullscreen(IntPtr window, UInt32 flags);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetWindowDisplayMode")]
        public static extern Int32 SetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowDisplayMode")]
        public static extern Int32 GetWindowDisplayMode(IntPtr window, SDL_DisplayMode* mode);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowDisplayIndex")]
        public static extern Int32 GetWindowDisplayIndex(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowFlags")]
        public static extern SDL_WindowFlags GetWindowFlags(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ShowWindow")]
        public static extern void ShowWindow(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_HideWindow")]
        public static extern void HideWindow(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_MaximizeWindow")]
        public static extern void MaximizeWindow(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_MinimizeWindow")]
        public static extern void MinimizeWindow(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RestoreWindow")]
        public static extern void RestoreWindow(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetWindowWMInfo")]
        public static extern bool GetWindowWMInfo(IntPtr window, SDL_SysWMinfo* info);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RWFromFile", BestFitMapping = false)]
        public static extern IntPtr RWFromFile([MarshalAs(UnmanagedType.LPStr)] String file, [MarshalAs(UnmanagedType.LPStr)] String mode);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RWFromMem")]
        public static extern IntPtr RWFromMem(IntPtr mem, Int32 size);
        
        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_AllocRW")]
        public static extern IntPtr AllocRW();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_FreeRW")]
        public static extern void FreeRW(IntPtr area);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadBMP_RW")]
        public static extern SDL_Surface_Native* LoadBMP_RW(IntPtr src, Int32 freesrc);

        public static SDL_Surface_Native* LoadBMP(String file)
        {
            return LoadBMP_RW(RWFromFile(file, "r"), 1);
        }

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SaveBMP_RW")]
        public static extern Int32 SaveBMP_RW(SDL_Surface_Native* surface, IntPtr dst, Int32 freedst);

        public static Int32 SaveBMP(SDL_Surface_Native* surface, String file)
        {
            return SaveBMP_RW(surface, RWFromFile(file, "wb"), 1);
        }

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetMouseState")]
        public static extern UInt32 GetMouseState(out Int32 x, out Int32 y);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetKeyboardState")]
        public static extern IntPtr GetKeyboardState(out Int32 numkeys);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetScancodeFromKey")]
        public static extern SDL_Scancode GetScancodeFromKey(SDL_Keycode keycode);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetModState")]
        public static extern SDL_Keymod GetModState();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetHint", BestFitMapping = false)]
        public static extern Boolean SetHint([MarshalAs(UnmanagedType.LPStr)] String name, [MarshalAs(UnmanagedType.LPStr)] String value);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateRGBSurface")]
        public static extern SDL_Surface_Native* CreateRGBSurface(UInt32 flags, Int32 width, Int32 height, Int32 depth, UInt32 Rmask, UInt32 Gmask, UInt32 Bmask, UInt32 Amask);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_FreeSurface")]
        public static extern void FreeSurface(SDL_Surface_Native* surface);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LockSurface")]
        public static extern Int32 LockSurface(SDL_Surface_Native* surface);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UnlockSurface")]
        public static extern void UnlockSurface(SDL_Surface_Native* surface);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UpperBlit")]
        private static extern Int32 UpperBlit(SDL_Surface_Native* src, SDL_Rect* srcrect, SDL_Surface_Native* dst, SDL_Rect* dstrect);

        public static Int32 BlitSurface(SDL_Surface_Native* src, SDL_Rect* srcrect, SDL_Surface_Native* dst, SDL_Rect* dstrect)
        {
            return UpperBlit(src, srcrect, dst, dstrect);
        }

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UpperBlitScaled")]
        private static extern Int32 UpperBlitScaled(SDL_Surface_Native* src, SDL_Rect* srcrect, SDL_Surface_Native* dst, SDL_Rect* dstrect);

        public static Int32 BlitScaled(SDL_Surface_Native* src, SDL_Rect* srcrect, SDL_Surface_Native* dst, SDL_Rect* dstrect)
        {
            return UpperBlitScaled(src, srcrect, dst, dstrect);
        }

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetSurfaceBlendMode")]
        public static extern Int32 SetSurfaceBlendMode(SDL_Surface_Native* surface, SDL_BlendMode blendMode);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetSurfaceBlendMode")]
        public static extern Int32 GetSurfaceBlendMode(SDL_Surface_Native* surface, SDL_BlendMode* blendMode);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_CreateColorCursor")]
        public static extern SDL_Cursor* CreateColorCursor(SDL_Surface_Native* surface, Int32 hot_x, Int32 hot_y);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_FreeCursor")]
        public static extern void FreeCursor(SDL_Cursor* cursor);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ShowCursor")]
        public static extern Int32 ShowCursor(Int32 toggle);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetCursor")]
        public static extern SDL_Cursor* GetCursor();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetCursor")]
        public static extern void SetCursor(SDL_Cursor* cursor);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetDefaultCursor")]
        public static extern SDL_Cursor* GetDefaultCursor();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetNumVideoDisplays")]
        public static extern Int32 GetNumVideoDisplays();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetDisplayName")]
        private static extern IntPtr GetDisplayName_Impl(Int32 displayIndex);

        public static String GetDisplayName(Int32 displayIndex)
        {
            return Marshal.PtrToStringAnsi(GetDisplayName_Impl(displayIndex));
        }

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetDisplayBounds")]
        public static extern Int32 GetDisplayBounds(Int32 displayIndex, SDL_Rect* rect);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetNumDisplayModes")]
        public static extern Int32 GetNumDisplayModes(Int32 displayIndex);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetDisplayMode")]
        public static extern Int32 GetDisplayMode(Int32 displayIndex, Int32 modeIndex, SDL_DisplayMode* mode);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetCurrentDisplayMode")]
        public static extern Int32 GetCurrentDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetDesktopDisplayMode")]
        public static extern Int32 GetDesktopDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetClosestDisplayMode")]
        public static extern SDL_DisplayMode* GetClosestDisplayMode(Int32 displayIndex, SDL_DisplayMode* mode, SDL_DisplayMode* closest);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_PixelFormatEnumToMasks")]
        public static extern Boolean PixelFormatEnumToMasks(UInt32 format, Int32* bpp, UInt32* Rmask, UInt32* Gmask, UInt32* Bmask, UInt32* Amask);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_GetProcAddress", CharSet = CharSet.Ansi, BestFitMapping = false)]
        public static extern IntPtr GL_GetProcAddress([MarshalAs(UnmanagedType.LPStr)] String proc);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_CreateContext")]
        public static extern IntPtr GL_CreateContext(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_DeleteContext")]
        public static extern void GL_DeleteContext(IntPtr context);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_GetCurrentContext")]
        public static extern IntPtr GL_GetCurrentContext();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_MakeCurrent")]
        public static extern Int32 GL_MakeCurrent(IntPtr window, IntPtr context);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_SetAttribute")]
        public static extern Int32 GL_SetAttribute(SDL_GLattr attr, Int32 value);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_GetAttribute")]
        public static extern Int32 GL_GetAttribute(SDL_GLattr attr, Int32* value);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_SwapWindow")]
        public static extern void GL_SwapWindow(IntPtr window);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_SetSwapInterval")]
        public static extern Int32 GL_SetSwapInterval(Int32 interval);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GL_GetDrawableSize")]
        public static extern void GL_GetDrawableSize(IntPtr window, out Int32 w, out Int32 h);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_NumJoysticks")]
        public static extern Int32 NumJoysticks();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_IsGameController")]
        public static extern Boolean IsGameController(Int32 joystick_index);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerOpen")]
        public static extern IntPtr GameControllerOpen(Int32 index);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerClose")]
        public static extern IntPtr GameControllerClose(IntPtr gamecontroller);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerNameForIndex")]
        private static extern IntPtr GameControllerNameForIndex_Impl(Int32 joystick_index);

        public static String GameControllerNameForIndex(Int32 joystick_index)
        {
            return Marshal.PtrToStringAnsi(GameControllerNameForIndex_Impl(joystick_index));
        }

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetButton")]
        public static extern Boolean GameControllerGetButton(IntPtr gamecontroller, SDL_GameControllerButton button);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GameControllerGetJoystick")]
        public static extern IntPtr GameControllerGetJoystick(IntPtr gamecontroller);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_JoystickInstanceID")]
        public static extern Int32 JoystickInstanceID(IntPtr joystick);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetNumTouchDevices")]
        public static extern Int32 GetNumTouchDevices();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetTouchDevice")]
        public static extern Int64 GetTouchDevice(Int32 index);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetNumTouchFingers")]
        public static extern Int32 GetNumTouchFingers(Int64 touchID);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetTouchFinger")]
        public static extern SDL_Finger* GetTouchFinger(Int64 touchID, Int32 index);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_RecordGesture")]
        public static extern Int32 RecordGesture(Int64 touchID);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SaveAllDollarTemplates")]
        public static extern Int32 SaveAllDollarTemplates(IntPtr dst);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SaveDollarTemplate")]
        public static extern Int32 SaveDollarTemplates(Int64 gestureID, IntPtr dst);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_LoadDollarTemplates")]
        public static extern Int32 LoadDollarTemplates(Int64 touchID, IntPtr src);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_StartTextInput")]
        public static extern void StartTextInput();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_StopTextInput")]
        public static extern void StopTextInput();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetTextInputRect")]
        public static extern void SetTextInputRect(SDL_Rect* rect);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_HasClipboardText")]
        public static extern Boolean HasClipboardText();

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetClipboardText")]
        private static extern IntPtr GetClipboardText_Impl();

        public static String GetClipboardText()
        {
            var ptr = GetClipboardText_Impl();
            return (ptr == IntPtr.Zero) ? null : Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_SetClipboardText")]
        public static extern void SetClipboardText([MarshalAs(UnmanagedType.LPStr)] String text);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_GetPowerInfo")]
        public static extern SDL_PowerState GetPowerInfo(int* secs, int* pct);

        [DllImport(LibraryPath, CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_ShowSimpleMessageBox")]
        public static extern Int32 ShowSimpleMessageBox(UInt32 flags, [MarshalAs(UnmanagedType.LPStr)] String title, [MarshalAs(UnmanagedType.LPStr)] String message, IntPtr window);
    }
}
