using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using OpenGLES;
using UIKit;

namespace TwistedLogik.Ultraviolet.iOS.Bindings
{
    // @interface SDLUIKitDelegate : NSObject <UIApplicationDelegate>
    [BaseType(typeof(UIApplicationDelegate))]
    interface SDLUIKitDelegate
    {
        // +(id)sharedAppDelegate;
        [Static]
        [Export("sharedAppDelegate")]
        NSObject SharedAppDelegate { get; }

        // +(NSString *)getAppDelegateClassName;
        [Static]
        [Export("getAppDelegateClassName")]
        string AppDelegateClassName { get; }

        // -(void)hideLaunchScreen;
        [Export("hideLaunchScreen")]
        void HideLaunchScreen();
    }

    // @interface SDL_uikitviewcontroller : UIViewController <UITextFieldDelegate>
    [BaseType(typeof(UIViewController))]
    interface SDL_uikitviewcontroller : IUITextFieldDelegate
    {
        // -(void)showKeyboard;
        [Export("showKeyboard")]
        void ShowKeyboard();

        // -(void)hideKeyboard;
        [Export("hideKeyboard")]
        void HideKeyboard();
    }
}