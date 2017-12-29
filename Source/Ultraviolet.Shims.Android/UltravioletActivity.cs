using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Xml;
using Android.Text;
using Org.Libsdl.App;
using Ultraviolet.Shims.Android.Input;
using Ultraviolet.Shims.Android.Platform;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.Messages;
using Ultraviolet.Platform;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="Activity"/> which hosts and runs an Ultraviolet application.
    /// </summary>
    [CLSCompliant(false)]
    public abstract class UltravioletActivity : SDLActivity,
        IMessageSubscriber<UltravioletMessageID>,
        IUltravioletComponent,
        IUltravioletHost,
        IDisposable
    {
        /// <summary>
        /// Contains native methods used by the <see cref="UltravioletActivity"/> class.
        /// </summary>
        private static class Native
        {
            [DllImport("SDL2", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SDL_UV_SetMainProc")]
            public static extern void UVSetMainProc(IntPtr proc);
        }

        /// <summary>
        /// Initializes the <see cref="UltravioletActivity"/> type.
        /// </summary>
        static UltravioletActivity()
        {
            var dataDir = Android.App.Application.Context.ApplicationContext.DataDir.AbsolutePath;
            Directory.SetCurrentDirectory(dataDir);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletActivity"/> class.
        /// </summary>
        /// <param name="company">The name of the company that produced the application.</param>
        /// <param name="application">The name of the application </param>
        protected UltravioletActivity(String company, String application)
        {
            Contract.RequireNotEmpty(company, nameof(company));
            Contract.RequireNotEmpty(application, nameof(application));

            PreserveApplicationSettings = true;

            this.company = company;
            this.application = application;

            this.sdlMainProc = new Func<Int32>(SDLMainProc);
            Native.UVSetMainProc(Marshal.GetFunctionPointerForDelegate(this.sdlMainProc));
        }

        /// <summary>
        /// Receives a message that has been published to a queue.
        /// </summary>
        /// <param name="type">The type of message that was received.</param>
        /// <param name="data">The data for the message that was received.</param>
        void IMessageSubscriber<UltravioletMessageID>.ReceiveMessage(UltravioletMessageID type, MessageData data)
        {
            OnReceivedMessage(type, data);
        }

        /// <summary>
        /// Runs the Ultraviolet application.
        /// </summary>
        public void Run()
        {
            Contract.EnsureNotDisposed(this, disposed);

            OnInitializing();

            CreateUltravioletContext();

            OnInitialized();

            WarnIfFileSystemSourceIsMissing();

            LoadSettings();

            OnLoadingContent();

            running = true;
            while (running)
            {
                if (IsSuspended)
                {
                    hostcore.RunOneTickSuspended();
                }
                else
                {
                    hostcore.RunOneTick();
                }
                Thread.Yield();
            }

            uv.WaitForPendingTasks(true);
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        public void Exit()
        {
            Contract.EnsureNotDisposed(this, disposed);

            running = false;
            finished = true;
        }

        /// <summary>
        /// Gets or sets the activity's current keyboard type.
        /// </summary>
        internal InputTypes KeyboardInputType
        {
            get { return (InputTypes)MCurrentInputType; }
            set { MCurrentInputType = (Int32)value; }
        }

        /// <inheritdoc/>
        public override void OnConfigurationChanged(global::Android.Content.Res.Configuration newConfig)
        {
            if (Ultraviolet != null && !Ultraviolet.Disposed)
            {
                var display = Ultraviolet.GetPlatform().Displays[0];
                var rotation = (ScreenRotation)WindowManager.DefaultDisplay.Rotation;

                if (rotation != display.Rotation)
                {
                    AndroidScreenRotationService.UpdateScreenRotation(rotation);

                    var messageData = Ultraviolet.Messages.CreateMessageData<OrientationChangedMessageData>();
                    messageData.Display = display;
                    Ultraviolet.Messages.Publish(UltravioletMessages.OrientationChanged, messageData);
                }
            }

            base.OnConfigurationChanged(newConfig);
        }

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);
                Contract.Ensure(created, UltravioletStrings.ContextMissing);

                return uv;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the application's primary window is synchronized
        /// to the vertical retrace when rendering (i.e., whether vsync is enabled).
        /// </summary>
        public Boolean SynchronizeWithVerticalRetrace
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                if (primary == null)
                    throw new InvalidOperationException(UltravioletStrings.NoPrimaryWindow);

                return primary.SynchronizeWithVerticalRetrace;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                if (primary == null)
                    throw new InvalidOperationException(UltravioletStrings.NoPrimaryWindow);

                primary.SynchronizeWithVerticalRetrace = value;
            }
        }

        /// <inheritdoc/>
        public Boolean IsActive
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                lock (stateSyncObject)
                    return !suspended;
            }
        }

        /// <inheritdoc/>
        public Boolean IsSuspended
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                lock (stateSyncObject)
                    return suspended;
            }
        }

        /// <inheritdoc/>
        public Boolean IsFixedTimeStep
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return this.isFixedTimeStep;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                this.isFixedTimeStep = value;
                if (hostcore != null)
                {
                    hostcore.IsFixedTimeStep = value;
                }
            }
        }

        /// <inheritdoc/>
        public TimeSpan TargetElapsedTime
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return this.targetElapsedTime;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);
                Contract.EnsureRange(value.TotalMilliseconds >= 0, nameof(value));

                this.targetElapsedTime = value;
                if (hostcore != null)
                {
                    hostcore.TargetElapsedTime = value;
                }
            }
        }

        /// <inheritdoc/>
        public TimeSpan InactiveSleepTime
        {
            get
            {
                Contract.EnsureNotDisposed(this, disposed);

                return this.inactiveSleepTime;
            }
            set
            {
                Contract.EnsureNotDisposed(this, disposed);

                this.inactiveSleepTime = value;
                if (hostcore != null)
                {
                    hostcore.InactiveSleepTime = value;
                }
            }
        }

        /// <summary>
        /// Called when the application is creating its Ultraviolet context.
        /// </summary>
        /// <returns>The Ultraviolet context.</returns>
        protected abstract UltravioletContext OnCreatingUltravioletContext();

        /// <summary>
        /// Releases resources associated with the object.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> if the object is being disposed; <see langword="false"/> if the object is being finalized.</param>
        protected override void Dispose(Boolean disposing)
        {
            lock (stateSyncObject)
            {
                if (!disposed)
                {
                    if (disposing && uv != null)
                    {
                        uv.Messages.Unsubscribe(this);

                        if (primary != null)
                        {
                            primary.Drawing -= uv_Drawing;
                            primary = null;
                        }

                        uv.Dispose();

                        uv.Updating -= uv_Updating;
                        uv.Shutdown -= uv_Shutdown;
                        uv.WindowDrawing -= uv_WindowDrawing;
                        uv.WindowDrawn -= uv_WindowDrawn;

                        hostcore = null;
                    }
                    disposed = true;
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Called when the application is initializing.
        /// </summary>
        protected virtual void OnInitializing()
        {

        }

        /// <summary>
        /// Called after the application has been initialized.
        /// </summary>
        protected virtual void OnInitialized()
        {

        }

        /// <summary>
        /// Called when the application is loading content.
        /// </summary>
        protected virtual void OnLoadingContent()
        {

        }

        /// <summary>
        /// Called when the application state is being updated.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected virtual void OnUpdating(UltravioletTime time)
        {

        }

        /// <summary>
        /// Called when the scene is being drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        protected virtual void OnDrawing(UltravioletTime time)
        {

        }

        /// <summary>
        /// Called when one of the application's windows is about to be drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="window">The window that is about to be drawn.</param>
        protected virtual void OnWindowDrawing(UltravioletTime time, IUltravioletWindow window)
        {

        }

        /// <summary>
        /// Called after one of the application's windows has been drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="window">The window that was just drawn.</param>
        protected virtual void OnWindowDrawn(UltravioletTime time, IUltravioletWindow window)
        {

        }

        /// <summary>
        /// Called when the application is about to be suspended.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected internal virtual void OnSuspending()
        {
        }

        /// <summary>
        /// Called when the application has been suspended.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected internal virtual void OnSuspended()
        {
            SaveSettings();
        }

        /// <summary>
        /// Called when the application is about to be resumed.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected internal virtual void OnResuming()
        {

        }

        /// <summary>
        /// Called when the application has been resumed.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected internal virtual void OnResumed()
        {

        }

        /// <summary>
        /// Called when the operating system is attempting to reclaim memory.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected internal virtual void OnReclaimingMemory()
        {

        }

        /// <summary>
        /// Called when the application is being shut down.
        /// </summary>
        protected virtual void OnShutdown()
        {

        }

        /// <summary>
        /// Occurs when the context receives a message from its queue.
        /// </summary>
        /// <param name="type">The message type.</param>
        /// <param name="data">The message data.</param>
        protected virtual void OnReceivedMessage(UltravioletMessageID type, MessageData data)
        {
            if (type == UltravioletMessages.ApplicationTerminating || type == UltravioletMessages.Quit)
            {
                running = false;
            }
            else if (type == UltravioletMessages.ApplicationSuspending)
            {
                OnSuspending();

                lock (stateSyncObject)
                    suspended = true;
            }
            else if (type == UltravioletMessages.ApplicationSuspended)
            {
                OnSuspended();
            }
            else if (type == UltravioletMessages.ApplicationResuming)
            {
                OnResuming();
            }
            else if (type == UltravioletMessages.ApplicationResumed)
            {
                hostcore?.ResetElapsed();

                lock (stateSyncObject)
                    suspended = false;

                OnResumed();
            }
            else if (type == UltravioletMessages.LowMemory)
            {
                OnReclaimingMemory();
            }
        }

        /// <inheritdoc/>
        protected override void OnCreate(global::Android.OS.Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            AndroidScreenDensityService.Activity = this;
            AndroidSoftwareKeyboardService.Activity = this;
        }

        /// <summary>
        /// Ensures that the assembly which contains the specified type is linked on platforms
        /// which require ahead-of-time compilation.
        /// </summary>
        /// <typeparam name="T">One of the types defined by the assembly to link.</typeparam>
        protected void EnsureAssemblyIsLinked<T>()
        {
            Console.WriteLine("Touching '" + typeof(T).Assembly.FullName + "' to ensure linkage...");
        }

        /// <summary>
        /// Sets the file system source to an archive file loaded from a manifest resource stream,
        /// if the specified manifest resource exists.
        /// </summary>
        /// <param name="name">The name of the manifest resource being loaded as the file system source.</param>
        protected void SetFileSourceFromManifestIfExists(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            var asm = GetType().Assembly;
            if (asm.GetManifestResourceNames().Contains(name))
            {
                FileSystemService.Source = ContentArchive.FromArchiveFile(() =>
                {
                    return asm.GetManifestResourceStream(name);
                });
            }
        }

        /// <summary>
        /// Sets the file system source to an archive file loaded from a manifest resource stream.
        /// </summary>
        /// <param name="name">The name of the manifest resource being loaded as the file system source.</param>
        protected void SetFileSourceFromManifest(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            var asm = GetType().Assembly;
            if (!asm.GetManifestResourceNames().Contains(name))
                throw new FileNotFoundException(name);

            FileSystemService.Source = ContentArchive.FromArchiveFile(() =>
            {
                return asm.GetManifestResourceStream(name);
            });
        }

        /// <summary>
        /// Populates the specified Ultraviolet configuration with the application's initial values.
        /// </summary>
        /// <param name="configuration">The <see cref="UltravioletConfiguration"/> to populate.</param>
        protected void PopulateConfiguration(UltravioletConfiguration configuration)
        {
            Contract.Require(configuration, nameof(configuration));
        }

        /// <summary>
        /// Gets the directory that contains the application's local configuration files.
        /// If the directory does not already exist, it will be created.
        /// </summary>
        /// <returns>The directory that contains the application's local configuration files.</returns>
        protected String GetLocalApplicationSettingsDirectory()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), company, application);
            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Gets the directory that contains the application's roaming configuration files.
        /// If the directory does not already exist, it will be created.
        /// </summary>
        /// <returns>The directory that contains the application's roaming configuration files.</returns>
        protected String GetRoamingApplicationSettingsDirectory()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), company, application);
            Directory.CreateDirectory(path);
            return path;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the application's internal framework settings
        /// should be preserved between instances.
        /// </summary>
        protected Boolean PreserveApplicationSettings
        {
            get;
            set;
        }

        /// <summary>
        /// Creates the application's Ultraviolet context.
        /// </summary>
        private void CreateUltravioletContext()
        {
            LoadSettings();
            
            uv = UltravioletContext.EnsureSuccessfulCreation(OnCreatingUltravioletContext);
            if (uv == null)
                throw new InvalidOperationException(UltravioletStrings.ContextNotCreated);

            if (this.settings != null)
            {
                this.settings.Apply(uv);
            }

            CreateUltravioletHostCore();

            this.uv.Messages.Subscribe(this,
                UltravioletMessages.ApplicationTerminating,
                UltravioletMessages.ApplicationSuspending,
                UltravioletMessages.ApplicationSuspended,
                UltravioletMessages.ApplicationResuming,
                UltravioletMessages.ApplicationResumed,
                UltravioletMessages.LowMemory,
                UltravioletMessages.Quit);
            this.uv.Updating += uv_Updating;
            this.uv.Shutdown += uv_Shutdown;
            this.uv.WindowDrawing += uv_WindowDrawing;
            this.uv.WindowDrawn += uv_WindowDrawn;

            this.uv.GetPlatform().Windows.PrimaryWindowChanging += uv_PrimaryWindowChanging;
            this.uv.GetPlatform().Windows.PrimaryWindowChanged += uv_PrimaryWindowChanged;
            HookPrimaryWindowEvents();

            this.created = true;
        }

        /// <summary>
        /// Creates the Ultraviolet host core for this host process.
        /// </summary>
        private void CreateUltravioletHostCore()
        {
            hostcore = new UltravioletHostCore(this);
            hostcore.IsFixedTimeStep = this.IsFixedTimeStep;
            hostcore.TargetElapsedTime = this.TargetElapsedTime;
            hostcore.InactiveSleepTime = this.InactiveSleepTime;
        }

        /// <summary>
        /// Hooks into the primary window's events.
        /// </summary>
        private void HookPrimaryWindowEvents()
        {
            if (primary != null)
            {
                primary.Drawing -= uv_Drawing;
            }

            primary = uv.GetPlatform().Windows.GetPrimary();

            if (primary != null)
            {
                primary.Drawing += uv_Drawing;
            }
        }

        /// <summary>
        /// Loads the application's settings.
        /// </summary>
        private void LoadSettings()
        {
            lock (stateSyncObject)
            {
                if (!PreserveApplicationSettings)
                    return;

                var directory = GetLocalApplicationSettingsDirectory();
                var path = Path.Combine(directory, "UltravioletSettings.xml");

                try
                {
                    var settings = UltravioletActivitySettings.Load(path);
                    if (settings == null)
                        return;

                    this.settings = settings;
                }
                catch (FileNotFoundException) { }
                catch (DirectoryNotFoundException) { }
                catch (XmlException) { }
            }
        }

        /// <summary>
        /// Saves the application's settings.
        /// </summary>
        private void SaveSettings()
        {
            lock (stateSyncObject)
            {
                if (!PreserveApplicationSettings)
                    return;

                var path = Path.Combine(GetLocalApplicationSettingsDirectory(), "UltravioletSettings.xml");

                this.settings = UltravioletActivitySettings.FromCurrentSettings(Ultraviolet);
                UltravioletActivitySettings.Save(path, settings);
            }
        }

        /// <summary>
        /// Handles the Ultraviolet window manager's PrimaryWindowChanging event.
        /// </summary>
        /// <param name="window">The primary window.</param>
        private void uv_PrimaryWindowChanging(IUltravioletWindow window)
        {
            SaveSettings();
        }

        /// <summary>
        /// Handles the Ultraviolet window manager's PrimaryWindowChanged event.
        /// </summary>
        /// <param name="window">The primary window.</param>
        private void uv_PrimaryWindowChanged(IUltravioletWindow window)
        {
            HookPrimaryWindowEvents();
        }

        /// <summary>
        /// Handles the Ultraviolet window's Drawing event.
        /// </summary>
        /// <param name="window">The window being drawn.</param>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        private void uv_Drawing(IUltravioletWindow window, UltravioletTime time)
        {
            OnDrawing(time);
        }

        /// <summary>
        /// Handles the Ultraviolet context's Updating event.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        private void uv_Updating(UltravioletContext uv, UltravioletTime time)
        {
            OnUpdating(time);
        }

        /// <summary>
        /// Handles the Ultraviolet context's Shutdown event.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private void uv_Shutdown(UltravioletContext uv)
        {
            OnShutdown();
        }

        /// <summary>
        /// Handles the Ultraviolet context's <see cref="UltravioletContext.WindowDrawing"/> event.
        /// </summary>
        private void uv_WindowDrawing(UltravioletContext uv, UltravioletTime time, IUltravioletWindow window)
        {
            OnWindowDrawing(time, window);
        }

        /// <summary>
        /// Handles the Ultraviolet context's <see cref="UltravioletContext.WindowDrawn"/> event.
        /// </summary>
        private void uv_WindowDrawn(UltravioletContext uv, UltravioletTime time, IUltravioletWindow window)
        {
            OnWindowDrawn(time, window);
        }

        /// <summary>
        /// The procedure invoked by SDL_main.
        /// </summary>
        /// <returns>The program's status code on exit.</returns>
        private Int32 SDLMainProc()
        {
            Run();

            SafeDispose.DisposeRef(ref uv);
            if (finished)
            {
                Finish();
            }

            return 0;
        }

        /// <summary>
        /// Writes a warning to the debug output if no file system source has been specified.
        /// </summary>
        private void WarnIfFileSystemSourceIsMissing()
        {
            if (FileSystemService.Source == null)
            {
                System.Diagnostics.Debug.WriteLine("WARNING: No file system source has been specified.");
            }
        }

        // Property values.
        private UltravioletContext uv;

        // State values.
        private readonly Object stateSyncObject = new Object();
        private readonly Func<Int32> sdlMainProc;
        private UltravioletHostCore hostcore;
        private Boolean created;
        private Boolean running;
        private Boolean finished;
        private Boolean suspended;
        private Boolean disposed;
        private IUltravioletWindow primary;

        // The application's tick state.
        private Boolean isFixedTimeStep = UltravioletHostCore.DefaultIsFixedTimeStep;
        private TimeSpan targetElapsedTime = UltravioletHostCore.DefaultTargetElapsedTime;
        private TimeSpan inactiveSleepTime = UltravioletHostCore.DefaultInactiveSleepTime;

        // The application's name.
        private readonly String company;
        private readonly String application;

        // The application's settings.
        private UltravioletActivitySettings settings;
    }
}