using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TwistedLogik.Nucleus;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Platform;
using TwistedLogik.Ultraviolet.UI;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents a UI layout using XiliumCef.
    /// </summary>
    public sealed partial class XiliumCefUILayout : UltravioletResource, IUILayout
    {
        /// <summary>
        /// Initializes the XiliumCefUILayout type.
        /// </summary>
        static XiliumCefUILayout()
        {
            miBuildScriptArgument = typeof(XiliumCefUILayout).GetMethod("BuildScriptArgument", BindingFlags.NonPublic | BindingFlags.Static,
                null, new [] { typeof(Object) }, null);

            miStringFormat = typeof(String).GetMethod("Format", BindingFlags.Public | BindingFlags.Static,
                null, new[] { typeof(String), typeof(Object[]) }, null);

            miExecuteJavaScript = typeof(XiliumCefUILayout).GetMethod("ExecuteJavaScript", BindingFlags.NonPublic | BindingFlags.Instance,
                null, new Type[] { typeof(String) }, null);
        }

        /// <summary>
        /// Initializes a new instance of the XiliumCefLayout class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="x">The x-coordinate of the layout's initial position.</param>
        /// <param name="y">The y-coordinate of the layout's initial position.</param>
        /// <param name="width">The layout's initial width in pixels.</param>
        /// <param name="height">The layout's initial height in pixels.</param>
        public XiliumCefUILayout(UltravioletContext uv, Int32 x, Int32 y, Int32 width, Int32 height)
            : base(uv)
        {
            this.cefClient = new XiliumCefClient(this, x, y, width, height);
            this.cefClient.Initialized += (client) => 
            { 
                this.state = UILayoutState.Initialized;
                this.OnInitialized();
            };
            this.cefClient.LoadEnd += (client) => 
            { 
                this.state = UILayoutState.Ready;
                this.OnReady();
            };

            this.interactive = true;

            HookMouseEvents();
            HookKeyboardEvents();
        }

        /// <summary>
        /// Handles a console message from the layout's browser.
        /// </summary>
        /// <param name="message">The console message.</param>
        /// <param name="source">The message source.</param>
        /// <param name="line">The line of code which gave rise to the message.</param>
        public void HandleConsoleMessage(String message, String source, Int32 line)
        {
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(String.Format("{0} [{1}:{2}]", message, source ?? "unknown", line));
            }
            OnScriptMessage(message, source, line);
        }

        /// <summary>
        /// Loads the specified layout.
        /// </summary>
        /// <param name="content">A content manager with which to load layout resources.</param>
        /// <param name="layout">The asset path of the layout to load.</param>
        public void LoadLayout(ContentManager content, String layout)
        {
            Contract.Require(content, "content");
            Contract.RequireNotEmpty(layout, "layout");
            Contract.EnsureNotDisposed(this, Disposed);

            if (state == UILayoutState.Initializing)
                throw new InvalidOperationException(XiliumStrings.LayoutNotInitialized);

            this.state = UILayoutState.Loading;
            this.OnLoading();

            this.cefClient.LoadLayout(content, layout);
        }

        /// <summary>
        /// Loads the specified layout.
        /// </summary>
        /// <param name="content">A content manager with which to load layout resources.</param>
        /// <param name="definition">A UI panel definition containing the layout information to load.</param>
        public void LoadLayout(ContentManager content, UIPanelDefinition definition)
        {
            Contract.Require(content, "content");
            Contract.Require(definition, "definition");
            Contract.EnsureNotDisposed(this, Disposed);

            if (state == UILayoutState.Initializing)
                throw new InvalidOperationException(XiliumStrings.LayoutNotInitialized);

            this.state = UILayoutState.Loading;
            this.OnLoading();

            this.cefClient.LoadLayout(content, definition);
        }

        /// <summary>
        /// Updates the layout's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        public void Update(UltravioletTime time)
        {
            Contract.EnsureNotDisposed(this, Disposed);
        }

        /// <summary>
        /// Draw the layout.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to draw.</param>
        /// <param name="spriteBatch">The sprite batch with which to draw the layout.</param>
        /// <param name="color">The color with which to tint the layout.</param>
        public void Draw(UltravioletTime time, SpriteBatch spriteBatch, Color color)
        {
            Contract.Require(spriteBatch, "spriteBatch");
            Contract.EnsureNotDisposed(this, Disposed);

            cefClient.Draw(spriteBatch, color);
        }

        /// <summary>
        /// Grants input focus to the layout.
        /// </summary>
        public void Focus()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            focused = true;
            if (cefClient.BrowserHost != null)
            {
                cefClient.BrowserHost.SendFocusEvent(true);
            }
        }

        /// <summary>
        /// Removes input focus from the layout.
        /// </summary>
        public void Blur()
        {
            Contract.EnsureNotDisposed(this, Disposed);

            focused = false;
            if (cefClient.BrowserHost != null)
            {
                cefClient.BrowserHost.SendFocusEvent(false);
            }
        }

        /// <summary>
        /// Registers a static API method.
        /// </summary>
        /// <param name="method">The name of the method to register.</param>
        public void RegisterApiMethod<T>(String method)
        {
            RegisterApiMethod<T>(method, null);
        }

        /// <summary>
        /// Registers a static API method.
        /// </summary>
        /// <param name="method">The name of the method to register.</param>
        /// <param name="name">The name under which the method can be invoked by a layout script.</param>
        public void RegisterApiMethod<T>(String method, String name)
        {
            Contract.RequireNotEmpty(method, "method");
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == UILayoutState.Initializing)
                throw new InvalidOperationException(XiliumStrings.LayoutNotInitialized);

            var registry = ApiMethodRegistryManager.GetByBrowserID(cefClient.Browser.Identifier);
            registry.RegisterApiMethod<T>(method, name);
        }

        /// <summary>
        /// Registers an API method.
        /// </summary>
        /// <param name="method">The name of the method to register.</param>
        /// <param name="target">The object on whic hthe method will be invoked.</param>
        public void RegisterApiMethod<T>(String method, T target) where T : class
        {
            RegisterApiMethod(method, null, target);
        }

        /// <summary>
        /// Registers an API method.
        /// </summary>
        /// <param name="method">The name of the method to register.</param>
        /// <param name="name">The name under which the method can be invoked by a layout script.</param>
        /// <param name="target">The object on whic hthe method will be invoked.</param>
        public void RegisterApiMethod<T>(String method, String name, T target) where T : class
        {
            Contract.RequireNotEmpty(method, "method");
            Contract.EnsureNotDisposed(this, Disposed);

            if (State == UILayoutState.Initializing)
                throw new InvalidOperationException(XiliumStrings.LayoutNotInitialized);

            var registry = ApiMethodRegistryManager.GetByBrowserID(cefClient.Browser.Identifier);
            registry.RegisterApiMethod<T>(method, name, target);
        }

        /// <summary>
        /// Gets a delegate that represents the specified layout script.
        /// </summary>
        /// <param name="script">The name of the script for which to retrieve a delegate.</param>
        /// <returns>A delegate that represents the specified layout script.</returns>
        public T GetScriptDelegate<T>(String script)
        {
            Contract.RequireNotEmpty(script, "script");
            Contract.EnsureNotDisposed(this, Disposed);

            if (State != UILayoutState.Ready)
                throw new InvalidOperationException(XiliumStrings.LayoutNotReady);

            if(!typeof(T).IsSubclassOf(typeof(Delegate)))
                throw new Exception(XiliumStrings.NotADelegate);

            // Get the parameter list for the requested delegate.
            var delegateInvoke = typeof(T).GetMethod("Invoke");
            var delegateParams = delegateInvoke.GetParameters();

            // Determine whether the delegate returns a Task<T>.  If it does, prepare to
            // handle asynchronous return values.
            var returnSync = true;
            var returnTaskType = delegateInvoke.ReturnType;
            var returnType = typeof(void);
            var returnCompletionSourceType = typeof(void);
            if (returnTaskType != typeof(void))
            {
                if (returnTaskType != typeof(Task) && !returnTaskType.IsSubclassOf(typeof(Task)))
                    throw new InvalidOperationException(XiliumStrings.ScriptMustReturnVoidOrTask);
                if (!returnTaskType.IsGenericType)
                    throw new InvalidOperationException(XiliumStrings.ScriptMustReturnVoidOrTask);

                returnSync = false;
                returnType = returnTaskType.GetGenericArguments().Single();
                returnCompletionSourceType = typeof(TaskCompletionSource<>).MakeGenericType(returnType);                
            }

            // Construct an expressoin tree which invokes the JavaScript function represented by the delegate.
            var signature = BuildScriptSignature(script, delegateParams.Length + (returnSync ? 0 : 1));

            var expReturnLabel   = Expression.Label(returnTaskType);
            var expVarLayout     = Expression.Variable(typeof(XiliumCefUILayout), "layout");
            var expVarTaskSource = Expression.Variable(returnCompletionSourceType, "taskSource");
            var expVarTaskID     = Expression.Variable(typeof(Int64), "taskID");
            var expVarSignature  = Expression.Variable(typeof(String), "signature");
            var expVarParameters = Expression.Variable(typeof(Object[]), "parameters");
            var expVarInvocation = Expression.Variable(typeof(String), "invocation");

            var miGetTaskCompletionSource = typeof(AsyncResultCoordinator).GetMethod("GetTaskCompletionSource", 
                BindingFlags.Public | BindingFlags.Static).MakeGenericMethod(returnType);

            var expParameters = delegateParams.Select(x => Expression.Parameter(x.ParameterType, x.Name)).ToList();
            var expParametersAsObject = expParameters.Select(x => Expression.Call(miBuildScriptArgument, Expression.Convert(x, typeof(Object)))).ToList();
            var expLambda = Expression.Lambda<T>(
                Expression.Block(
                    new ParameterExpression[] { expVarLayout, expVarTaskSource, expVarTaskID, expVarSignature, expVarParameters, expVarInvocation },
                    returnSync ? (Expression)Expression.Empty() : Expression.Assign(
                        expVarTaskSource,
                        Expression.Call(miGetTaskCompletionSource, expVarTaskID)
                    ),
                    Expression.Assign(
                        expVarSignature,
                        Expression.Constant(signature, typeof(String))
                    ),
                    Expression.Assign(
                        expVarParameters,
                        Expression.NewArrayInit(typeof(Object), returnSync ? expParametersAsObject : 
                            Enumerable.Union(new Expression [] { Expression.Convert(expVarTaskID, typeof(Object)) }, expParametersAsObject))
                    ),
                    Expression.Assign(
                        expVarInvocation,
                        Expression.Call(miStringFormat, expVarSignature, expVarParameters)
                    ),
                    Expression.Assign(
                        expVarLayout,
                        Expression.Constant(this)
                    ),
                    Expression.Call(expVarLayout, miExecuteJavaScript, expVarInvocation),
                    returnSync ? (Expression)Expression.Empty() : Expression.Return(expReturnLabel, Expression.Property(expVarTaskSource, "Task")),
                    returnSync ? (Expression)Expression.Empty() : Expression.Label(expReturnLabel, Expression.Default(returnTaskType))
                ), expParameters);

            return expLambda.Compile();
        }
        
        /// <summary>
        /// Gets the layout's current state.
        /// </summary>
        public UILayoutState State
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return state;
            }
        }

        /// <summary>
        /// Gets or sets the layout's position.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.cefClient.Position;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                this.cefClient.Position = value;
            }
        }

        /// <summary>
        /// Gets or sets the x-coordinate of the layout's position.
        /// </summary>
        public Int32 X
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.cefClient.X;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                this.cefClient.X = value;
            }
        }

        /// <summary>
        /// Gets or sets the y-coordinate of the layout's position.
        /// </summary>
        public Int32 Y
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.cefClient.Y;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                this.cefClient.Y = value;
            }
        }

        /// <summary>
        /// Gets or sets the layout's size.
        /// </summary>
        public Size2 Size
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.cefClient.Size;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                this.cefClient.Size = value;
            }
        }

        /// <summary>
        /// Gets or sets the layout's width in pixels.
        /// </summary>
        public Int32 Width
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.cefClient.Width;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                this.cefClient.Width = value;
            }
        }

        /// <summary>
        /// Gets or sets the layout's height in pixels.
        /// </summary>
        public Int32 Height
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return this.cefClient.Height;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                this.cefClient.Height = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the layout is interactive.  Non-interactive
        /// layouts will not receive input events.
        /// </summary>
        public Boolean Interactive
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return interactive;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                interactive = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the layout has input focus.
        /// </summary>
        public Boolean Focused
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return focused;
            }
        }

        /// <summary>
        /// Gets or sets the window that is associated with the layout.
        /// </summary>
        public IUltravioletWindow Window
        {
            get
            {
                Contract.EnsureNotDisposed(this, Disposed);

                return window;
            }
            set
            {
                Contract.EnsureNotDisposed(this, Disposed);

                window = value;
            }
        }

        /// <summary>
        /// Occurs when the layout has been initialized.
        /// </summary>
        public event UILayoutEventHandler Initialized;

        /// <summary>
        /// Occurs when the layout begins loading.
        /// </summary>
        public event UILayoutEventHandler Loading;

        /// <summary>
        /// Occurs when the layout is ready for interaction.
        /// </summary>
        public event UILayoutEventHandler Ready;

        /// <summary>
        /// Occurs when a layout script writes a debug message.
        /// </summary>
        public event UILayoutMessageEventHandler ScriptMessage;

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing">true if the object is being disposed; false if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (!Ultraviolet.Disposed)
                {
                    UnhookMouseEvents();
                    UnhookKeyboardEvents();
                }
                SafeDispose.Dispose(cefClient);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Builds the specified script function's signature string.
        /// </summary>
        /// <param name="script">The name of the script function.</param>
        /// <param name="paramcount">The number of parameters in the script's signature.</param>
        /// <returns>The specified script function's signature string.</returns>
        private static String BuildScriptSignature(String script, Int32 paramcount)
        {
            var signature = new StringBuilder();

            signature.AppendFormat("api.{0}(", script);
            for (int i = 0; i < paramcount; i++)
            {
                if (i > 0)
                {
                    signature.Append(", ");
                }
                signature.AppendFormat("{{{0}}}", i);
            }
            signature.AppendFormat(");");

            return signature.ToString();
        }

        /// <summary>
        /// Builds a script argument from the specified object.
        /// </summary>
        private static Object BuildScriptArgument(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Gets the mouse's current coordinates relative to the layout.
        /// </summary>
        /// <returns>The mouse's current coordinates relative to the layout.</returns>
        private Vector2 GetMouseCoordinates()
        {
            if (Ultraviolet.GetInput().IsMouseSupported())
            {
                var mouse = Ultraviolet.GetInput().GetMouse();
                return new Vector2(mouse.X - X, mouse.Y - Y);
            }
            return Vector2.Zero;
        }

        /// <summary>
        /// Gets the current mouse modifiers.
        /// </summary>
        private CefEventFlags GetMouseModifiers()
        {
            var mod = CefEventFlags.None;

            if (!Ultraviolet.GetInput().IsMouseSupported())
                return mod;

            var device = Ultraviolet.GetInput().GetMouse();

            if (device.IsButtonDown(MouseButton.Left))
                mod |= CefEventFlags.LeftMouseButton;

            if (device.IsButtonDown(MouseButton.Right))
                mod |= CefEventFlags.RightMouseButton;

            if (device.IsButtonDown(MouseButton.Middle))
                mod |= CefEventFlags.MiddleMouseButton;

            return mod;
        }

        /// <summary>
        /// Gets the current keyboard modifiers.
        /// </summary>
        private CefEventFlags GetKeyboardModifiers()
        {
            var mod = CefEventFlags.None;

            if (!Ultraviolet.GetInput().IsKeyboardSupported())
                return mod;

            var device = Ultraviolet.GetInput().GetKeyboard();

            if (device.IsAltDown)
                mod |= CefEventFlags.AltDown;

            if (device.IsControlDown)
                mod |= CefEventFlags.ControlDown;

            if (device.IsShiftDown)
                mod |= CefEventFlags.ShiftDown;

            return mod;
        }

        /// <summary>
        /// Maps the specified Ultraviolet Key value to a Win32 virtual-key code.
        /// </summary>
        /// <param name="key">The Ultraviolet Key value to map.</param>
        /// <returns>The mapped key value.</returns>
        private Int32 MapUltravioletKey(Key key)
        {
            Int32 vk;
            KeyMap.TryGetValue((int)key, out vk);
            return vk;
        }

        /// <summary>
        /// Hooks into the keyboard's input events.
        /// </summary>
        private void HookKeyboardEvents()
        {
            if (!Ultraviolet.GetInput().IsKeyboardSupported())
                return;

            var keyboard = Ultraviolet.GetInput().GetKeyboard();

            keyboard.KeyPressed  += keyboard_KeyPressed;
            keyboard.KeyReleased += keyboard_KeyReleased;
            keyboard.TextInput   += keyboard_TextInput;
        }

        /// <summary>
        /// Hooks into the mouse's input events.
        /// </summary>
        private void HookMouseEvents()
        {
            if (!Ultraviolet.GetInput().IsMouseSupported())
                return;

            var mouse = Ultraviolet.GetInput().GetMouse();

            mouse.ButtonPressed  += mouse_ButtonPressed;
            mouse.ButtonReleased += mouse_ButtonReleased;
            mouse.Moved          += mouse_Moved;
            mouse.WheelScrolled  += mouse_WheelScrolled;
        }

        /// <summary>
        /// Unhooks from the keyboard's input events.
        /// </summary>
        private void UnhookKeyboardEvents()
        {
            if (!Ultraviolet.GetInput().IsKeyboardSupported())
                return;

            var keyboard = Ultraviolet.GetInput().GetKeyboard();

            keyboard.KeyPressed  -= keyboard_KeyPressed;
            keyboard.KeyReleased -= keyboard_KeyReleased;
            keyboard.TextInput   -= keyboard_TextInput;
        }

        /// <summary>
        /// Unhooks from the mouse's input events.
        /// </summary>
        private void UnhookMouseEvents()
        {
            if (!Ultraviolet.GetInput().IsMouseSupported())
                return;

            var mouse = Ultraviolet.GetInput().GetMouse();

            mouse.ButtonPressed  -= mouse_ButtonPressed;
            mouse.ButtonReleased -= mouse_ButtonReleased;
            mouse.Moved          -= mouse_Moved;
            mouse.WheelScrolled  -= mouse_WheelScrolled;
        }
        
        /// <summary>
        /// Executes the specified JavaScript code.
        /// </summary>
        /// <param name="invocation">The JavaScript code to execute.</param>
        private void ExecuteJavaScript(String invocation)
        {
            cefClient.Browser.GetMainFrame().ExecuteJavaScript(invocation, String.Empty, 0);
        }

        /// <summary>
        /// Raises the Initialized event.
        /// </summary>
        private void OnInitialized()
        {
            var temp = Initialized;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the Loading event.
        /// </summary>
        private void OnLoading()
        {
            var temp = Loading;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the Ready event.
        /// </summary>
        private void OnReady()
        {
            var temp = Ready;
            if (temp != null)
            {
                temp(this);
            }
        }

        /// <summary>
        /// Raises the ScriptMessage event.
        /// </summary>
        /// <param name="message">The message that was printed.</param>
        /// <param name="source">The name of the script that caused the message, if available.</param>
        /// <param name="line">The line of code that caused the message, if available.</param>
        private void OnScriptMessage(String message, String source, Int32 line)
        {
            var temp = ScriptMessage;
            if (temp != null)
            {
                temp(this, message, source, line);
            }
        }

        /// <summary>
        /// Gets the layout's associated window, or the primary window if the layout
        /// has no associated window.
        /// </summary>
        private IUltravioletWindow GetAssociatedWindow()
        {
            return window ?? Ultraviolet.GetPlatform().Windows.GetPrimary();
        }

        /// <summary>
        /// Handles the keyboard's KeyReleased event.
        /// </summary>
        private void keyboard_KeyReleased(IUltravioletWindow window, KeyboardDevice device, Key key)
        {
            if (!interactive || window != GetAssociatedWindow())
                return;

            var evt = new CefKeyEvent()
            {
                EventType = CefKeyEventType.KeyUp,
                WindowsKeyCode = MapUltravioletKey(key),
                NativeKeyCode = 0,
                IsSystemKey = false,
                Modifiers = GetKeyboardModifiers(),
            };
            cefClient.BrowserHost.SendKeyEvent(evt);
        }

        /// <summary>
        /// Handles the keyboard's KeyPressed event.
        /// </summary>
        private void keyboard_KeyPressed(IUltravioletWindow window, KeyboardDevice device, Key key, Boolean ctrl, Boolean alt, Boolean shift, Boolean repeat)
        {
            if (!interactive || window != GetAssociatedWindow())
                return;

            var evt = new CefKeyEvent()
            {
                EventType = CefKeyEventType.RawKeyDown,
                WindowsKeyCode = MapUltravioletKey(key),
                NativeKeyCode = 0,
                IsSystemKey = false,
                Modifiers = GetKeyboardModifiers(),
            };
            cefClient.BrowserHost.SendKeyEvent(evt);
        }

        /// <summary>
        /// Handles the keyboard's TextInput event.
        /// </summary>
        private void keyboard_TextInput(IUltravioletWindow window, KeyboardDevice device)
        {
            if (!interactive || window != GetAssociatedWindow())
                return;

            device.GetTextInput(textInputBuffer, false);

            var mods = GetKeyboardModifiers();

            for (int i = 0; i < textInputBuffer.Length; i++)
            {
                var evt = new CefKeyEvent()
                {
                    EventType = CefKeyEventType.Char,
                    WindowsKeyCode = (int)textInputBuffer[i],
                    Modifiers = mods,
                };
                cefClient.BrowserHost.SendKeyEvent(evt);
            }
        }

        /// <summary>
        /// Handles the mouse's ButtonPressed event.
        /// </summary>
        private void mouse_ButtonPressed(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (!interactive || window != GetAssociatedWindow())
                return;

            var pos = GetMouseCoordinates();
            var evt = new CefMouseEvent((int)pos.X, (int)pos.Y, GetMouseModifiers());
            switch (button)
            {
                case MouseButton.Left:
                    cefClient.BrowserHost.SendMouseClickEvent(evt, CefMouseButtonType.Left, false, 1);
                    break;

                case MouseButton.Middle:
                    cefClient.BrowserHost.SendMouseClickEvent(evt, CefMouseButtonType.Middle, false, 1);
                    break;

                case MouseButton.Right:
                    cefClient.BrowserHost.SendMouseClickEvent(evt, CefMouseButtonType.Right, false, 1);
                    break;
            }
        }

        /// <summary>
        /// Handles the mouse's ButtonReleased event.
        /// </summary>
        private void mouse_ButtonReleased(IUltravioletWindow window, MouseDevice device, MouseButton button)
        {
            if (!interactive || window != GetAssociatedWindow())
                return;

            var pos = GetMouseCoordinates();
            var evt = new CefMouseEvent((int)pos.X, (int)pos.Y, GetMouseModifiers());
            switch (button)
            {
                case MouseButton.Left:
                    cefClient.BrowserHost.SendMouseClickEvent(evt, CefMouseButtonType.Left, true, 1);
                    break;

                case MouseButton.Middle:
                    cefClient.BrowserHost.SendMouseClickEvent(evt, CefMouseButtonType.Middle, true, 1);
                    break;

                case MouseButton.Right:
                    cefClient.BrowserHost.SendMouseClickEvent(evt, CefMouseButtonType.Right, true, 1);
                    break;
            }
        }

        /// <summary>
        /// Handles the mouse's Moved event.
        /// </summary>
        private void mouse_Moved(IUltravioletWindow window, MouseDevice device, Int32 x, Int32 y, Int32 dx, Int32 dy)
        {
            if (!interactive || window != GetAssociatedWindow())
                return;

            var layoutArea = new Rectangle(0, 0, Width, Height);
            var layoutContainedMouse = layoutArea.Contains(new Vector2(mouseX, mouseY));
         
            mouseX = x - X;
            mouseY = y - Y;

            var layoutContainsMouse = layoutArea.Contains(new Vector2(mouseX, mouseY));

            var evt = new CefMouseEvent(x, y, GetMouseModifiers());
            cefClient.BrowserHost.SendMouseMoveEvent(evt, layoutContainedMouse && !layoutContainsMouse);
        }

        /// <summary>
        /// Handles the mouse's WheelScrolled event.
        /// </summary>
        private void mouse_WheelScrolled(IUltravioletWindow window, MouseDevice device, Int32 x, Int32 y)
        {
            if (!interactive || window != GetAssociatedWindow())
                return;

            const Int32 WHEEL_DELTA = 120;

            var pos = GetMouseCoordinates();
            var evt = new CefMouseEvent((int)pos.X, (int)pos.Y, GetMouseModifiers());
            cefClient.BrowserHost.SendMouseWheelEvent(evt, -x * WHEEL_DELTA, y * WHEEL_DELTA);
        }

        // The CEF client object.
        private readonly XiliumCefClient cefClient;
        private UILayoutState state = UILayoutState.Initializing;

        // Input state variables.
        private readonly StringBuilder textInputBuffer = new StringBuilder();
        private Int32 mouseX;
        private Int32 mouseY;
        private Boolean interactive;
        private Boolean focused;
        private IUltravioletWindow window;

        // Scripting variables.
        private static readonly MethodInfo miBuildScriptArgument;
        private static readonly MethodInfo miStringFormat;
        private static readonly MethodInfo miExecuteJavaScript;
    }
}
