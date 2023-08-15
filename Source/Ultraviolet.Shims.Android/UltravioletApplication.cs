using Android.App;
using Android.OS;
using Android.Text;
using Org.Libsdl.App;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Core.Messages;
using Ultraviolet.Graphics;
using Ultraviolet.Input;
using Ultraviolet.Messages;
using Ultraviolet.Platform;
using Ultraviolet.SDL2;
using Ultraviolet.Shims.Android.Graphics;
using Ultraviolet.Shims.Android.Input;
using Ultraviolet.Shims.Android.Platform;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an <see cref="Activity"/> which hosts and runs an Ultraviolet application.
    /// </summary>
    public abstract class UltravioletApplication : SDLActivity,
        IMessageSubscriber<UltravioletMessageID>,
        IUltravioletComponent,
        IUltravioletHost,
        IUltravioletApplicationAdapterHost,
        IDisposable
    {
        /// <summary>
        /// Initializes the <see cref="UltravioletApplication"/> type.
        /// </summary>
        static UltravioletApplication()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.N)
            {
                var dataDir = Android.App.Application.Context.ApplicationContext.DataDir.AbsolutePath;
                Directory.SetCurrentDirectory(dataDir);
            }
            else
            {
                var pkgManager = Android.App.Application.Context.PackageManager;
                var pkgName = Android.App.Application.Context.PackageName;
                var pkgInfo = pkgManager.GetPackageInfo(pkgName, 0);
                var dataDir = pkgInfo.ApplicationInfo.DataDir;
                Directory.SetCurrentDirectory(dataDir);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UltravioletApplication"/> class.
        /// </summary>
        /// <param name="developerName">The name of the company or developer who built this application.</param>
        /// <param name="applicationName">The name of the application </param>
        protected UltravioletApplication(String developerName, String applicationName)
        {
            Contract.RequireNotEmpty(developerName, nameof(developerName));
            Contract.RequireNotEmpty(applicationName, nameof(applicationName));

            PreserveApplicationSettings = true;

            this.DeveloperName = developerName;
            this.ApplicationName = applicationName;

            applicationAdapter = OnCreatingApplicationAdapter();
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

        /// <inheritdoc/>
        public override void OnConfigurationChanged(Android.Content.Res.Configuration newConfig)
        {
            if (created && Ultraviolet != null && !Ultraviolet.Disposed)
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
        /// Runs the Ultraviolet application.
        /// </summary>
        public void Run()
        {
            Contract.EnsureNotDisposed(this, disposed);

            var configuration = new SDL2UltravioletConfiguration();

            OnConfiguring(configuration);
            applicationAdapter.Configure(configuration);

            OnInitializing();

            CreateUltravioletContext(configuration, (context, factory) =>
            {
                factory.SetFactoryMethod<SurfaceSourceFactory>((stream) => new AndroidSurfaceSource(stream));
                factory.SetFactoryMethod<SurfaceSaverFactory>(() => new AndroidSurfaceSaver());
                factory.SetFactoryMethod<IconLoaderFactory>(() => new AndroidIconLoader());
                factory.SetFactoryMethod<FileSystemServiceFactory>(() => new FileSystemService());
                factory.SetFactoryMethod<ScreenRotationServiceFactory>((display) => new AndroidScreenRotationService(display));
                factory.SetFactoryMethod<ScreenDensityServiceFactory>((display) => new AndroidScreenDensityService(display));
                factory.SetFactoryMethod<AssemblyLoaderServiceFactory>(() => new AndroidAssemblyLoaderService());

                var softwareKeyboardService = new AndroidSoftwareKeyboardService();
                factory.SetFactoryMethod<SoftwareKeyboardServiceFactory>(() => softwareKeyboardService);
            });

            OnInitialized();

            WarnIfFileSystemSourceIsMissing();

            LoadSettings();

            OnLoadingContent();

            running = true;
            while (running)
            {
                if (IsSuspended)
                {
                    timingLogic.RunOneTickSuspended();
                }
                else
                {
                    timingLogic.RunOneTick();
                }
                Thread.Yield();
            }

            timingLogic.Cleanup();

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

        /// <inheritdoc/>
        public String DeveloperName { get; }

        /// <inheritdoc/>
        public String ApplicationName { get; }

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
                if (timingLogic != null)
                {
                    timingLogic.IsFixedTimeStep = value;
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
                if (timingLogic != null)
                {
                    timingLogic.TargetElapsedTime = value;
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
                if (timingLogic != null)
                {
                    timingLogic.InactiveSleepTime = value;
                }
            }
        }

        /// <summary>
        /// Gets the current instance of the <see cref="UltravioletApplication"/> class.
        /// </summary>
        internal static UltravioletApplication Instance
        {
            get { return MSingleton as UltravioletApplication; }
        }

        /// <summary>
        /// Gets or sets the activity's current keyboard type.
        /// </summary>
        internal InputTypes KeyboardInputType
        {
            get { return (InputTypes)MCurrentInputType; }
            set { MCurrentInputType = (Int32)value; }
        }

        /// <summary>
        /// Called when the application is creating its Ultraviolet context.
        /// </summary>
        /// <param name="configuration">The configuration to supply to the context</param>
        /// <param name="factoryInitializer">A factory which is executed when the context is being initialized.</param>
        /// <returns>The Ultraviolet context.</returns>
        protected UltravioletContext OnCreatingUltravioletContext(UltravioletConfiguration configuration, Action<UltravioletContext, UltravioletFactory> factoryInitializer)
        {
            PopulateConfiguration(configuration);
            return new SDL2UltravioletContext(this, (SDL2UltravioletConfiguration)configuration, factoryInitializer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        protected virtual void OnConfiguring(UltravioletConfiguration configuration)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The host</returns>
        protected abstract UltravioletApplicationAdapter OnCreatingApplicationAdapter();

        /// <inheritdoc/>
        protected override void OnCreate(Bundle savedInstanceState)
        {
            var uv = UltravioletContext.RequestCurrent();
            if (uv != null && !uv.Disposed)
            {
                /*TODO
                var data = uv.Messages.CreateMessageData<AndroidLifecycleMessageData>();
                data.Activity = this;
                uv.Messages.Publish(UltravioletMessages.AndroidActivityCreate, data);
                */
            }
            base.OnCreate(savedInstanceState);
        }

        /// <inheritdoc/>
        protected override void OnStart()
        {
            var uv = UltravioletContext.RequestCurrent();
            if (uv != null && !uv.Disposed)
            {
                /*TODO
                var data = uv.Messages.CreateMessageData<AndroidLifecycleMessageData>();
                data.Activity = this;
                uv.Messages.Publish(UltravioletMessages.AndroidActivityStart, data);
                */
            }
            base.OnStart();
        }

        /// <inheritdoc/>
        protected override void OnResume()
        {
            var uv = UltravioletContext.RequestCurrent();
            if (uv != null && !uv.Disposed)
            {
                /*TODO
                var data = uv.Messages.CreateMessageData<AndroidLifecycleMessageData>();
                data.Activity = this;
                uv.Messages.Publish(UltravioletMessages.AndroidActivityResume, data);
                */
            }
            base.OnResume();
        }

        /// <inheritdoc/>
        protected override void OnPause()
        {
            var uv = UltravioletContext.RequestCurrent();
            if (uv != null && !uv.Disposed)
            {
                /*TODO
                var data = uv.Messages.CreateMessageData<AndroidLifecycleMessageData>();
                data.Activity = this;
                uv.Messages.Publish(UltravioletMessages.AndroidActivityPause, data);
                */
            }
            base.OnPause();
        }

        /// <inheritdoc/>
        protected override void OnStop()
        {
            var uv = UltravioletContext.RequestCurrent();
            if (uv != null && !uv.Disposed)
            {
                /* TODO
                var data = uv.Messages.CreateMessageData<AndroidLifecycleMessageData>();
                data.Activity = this;
                uv.Messages.Publish(UltravioletMessages.AndroidActivityStop, data);
                */
            }
            base.OnStop();
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            var uv = UltravioletContext.RequestCurrent();
            if (uv != null && !uv.Disposed)
            {
                /*TODO
                var data = uv.Messages.CreateMessageData<AndroidLifecycleMessageData>();
                data.Activity = this;
                uv.Messages.Publish(UltravioletMessages.AndroidActivityDestroy, data);
                */
            }
            base.OnDestroy();
        }

        /// <inheritdoc/>
        protected override void OnRestart()
        {
            var uv = UltravioletContext.RequestCurrent();
            if (uv != null && !uv.Disposed)
            {
                /*TODO
                var data = uv.Messages.CreateMessageData<AndroidLifecycleMessageData>();
                data.Activity = this;
                uv.Messages.Publish(UltravioletMessages.AndroidActivityRestart, data);
                */
            }
            base.OnRestart();
        }

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

                        timingLogic = null;
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
            applicationAdapter.Initializing();
        }

        /// <summary>
        /// Called after the application has been initialized.
        /// </summary>
        protected virtual void OnInitialized()
        {
            applicationAdapter.Initialized();
        }

        /// <summary>
        /// Called when the application is loading content.
        /// </summary>
        protected virtual void OnLoadingContent()
        {
            applicationAdapter.LoadingContent();
        }

        /// <summary>
        /// Called when the application state is being updated.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        protected virtual void OnUpdating(UltravioletTime time)
        {
            applicationAdapter.Updating(time);
        }

        /// <summary>
        /// Called when the scene is being drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        protected virtual void OnDrawing(UltravioletTime time)
        {
            applicationAdapter.Drawing(time);
        }

        /// <summary>
        /// Called when one of the application's windows is about to be drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="window">The window that is about to be drawn.</param>
        protected virtual void OnWindowDrawing(UltravioletTime time, IUltravioletWindow window)
        {
            applicationAdapter.WindowDrawing(time, window);
        }

        /// <summary>
        /// Called after one of the application's windows has been drawn.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        /// <param name="window">The window that was just drawn.</param>
        protected virtual void OnWindowDrawn(UltravioletTime time, IUltravioletWindow window)
        {
            applicationAdapter.WindowDrawn(time, window);
        }

        /// <summary>
        /// Called when the application is about to be suspended.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected internal virtual void OnSuspending()
        {
            applicationAdapter.Suspending();
        }

        /// <summary>
        /// Called when the application has been suspended.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected internal virtual void OnSuspended()
        {
            SaveSettings();
            applicationAdapter.Suspended();
        }

        /// <summary>
        /// Called when the application is about to be resumed.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected internal virtual void OnResuming()
        {
            applicationAdapter.Resuming();
        }

        /// <summary>
        /// Called when the application has been resumed.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected internal virtual void OnResumed()
        {
            applicationAdapter.Resumed();
        }

        /// <summary>
        /// Called when the operating system is attempting to reclaim memory.
        /// </summary>
        /// <remarks>When implementing this method, be aware that it can potentially be called
        /// from a thread other than the main Ultraviolet thread.</remarks>
        protected internal virtual void OnReclaimingMemory()
        {
            applicationAdapter.ReclaimingMemory();
        }

        /// <summary>
        /// Called when the application is being shut down.
        /// </summary>
        protected virtual void OnShutdown()
        {
            applicationAdapter.Shutdown();
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
                timingLogic?.ResetElapsed();

                lock (stateSyncObject)
                    suspended = false;

                OnResumed();
            }
            else if (type == UltravioletMessages.LowMemory)
            {
                OnReclaimingMemory();
            }
        }

        /// <summary>
        /// Creates the timing logic for this host process.
        /// </summary>
        protected virtual IUltravioletTimingLogic CreateTimingLogic()
        {
            var timingLogic = new UltravioletTimingLogic(this);
            timingLogic.IsFixedTimeStep = this.IsFixedTimeStep;
            timingLogic.TargetElapsedTime = this.TargetElapsedTime;
            timingLogic.InactiveSleepTime = this.InactiveSleepTime;
            return timingLogic;
        }

        /// <inheritdoc/>
        protected sealed override void OnUltravioletRun()
        {
            Run();

            SafeDispose.DisposeRef(ref uv);
            if (finished)
            {
                Finish();
            }
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
        /// Uses a file source which is appropriate to the current platform.
        /// </summary>
        /// <returns><see langword="true"/> if a platform-specific file source was used; otherwise, <see langword="false"/>.</returns>
        protected Boolean UsePlatformSpecificFileSource()
        {
            FileSystemService.Source = new AndroidAssetFileSource(Assets, GetType().Assembly);
            return true;
        }

        /// <summary>
        /// Sets the file system source to an archive file loaded from a manifest resource stream,
        /// if the specified manifest resource exists.
        /// </summary>
        /// <param name="name">The name of the manifest resource being loaded as the file system source.</param>
        /// <returns><see langword="true"/> if the file source was set; otherwise, <see langword="false"/>.</returns>
        protected Boolean SetFileSourceFromManifestIfExists(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            var asm = GetType().Assembly;
            if (asm.GetManifestResourceNames().Contains(name))
            {
                FileSystemService.Source = ContentArchive.FromArchiveFile(() =>
                {
                    return asm.GetManifestResourceStream(name);
                });
                return true;
            }

            return false;
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

        /// <inheritdoc/>
        public String GetLocalApplicationSettingsDirectory()
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), DeveloperName, ApplicationName);
            Directory.CreateDirectory(path);
            return path;
        }

        /// <inheritdoc/>
        public String GetRoamingApplicationSettingsDirectory()
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), DeveloperName, ApplicationName);
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
        /// <param name="configuration">The configuration to supply to the context</param>
        /// <param name="factoryInitializer">A delegate which is executed when the factory is being initialized.</param>
        /// </summary>
        private void CreateUltravioletContext(SDL2UltravioletConfiguration configuration, Action<UltravioletContext, UltravioletFactory> factoryInitializer)
        {
            LoadSettings();

            uv = UltravioletContext.EnsureSuccessfulCreation(OnCreatingUltravioletContext, configuration, factoryInitializer);
            if (uv == null)
                throw new InvalidOperationException(UltravioletStrings.ContextNotCreated);

            if (this.settings != null)
            {
                this.settings.Apply(uv);
            }

            this.timingLogic = CreateTimingLogic();
            if (this.timingLogic == null)
                throw new InvalidOperationException(UltravioletStrings.InvalidTimingLogic);

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
                    var settings = UltravioletApplicationSettings.Load(path);
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

                this.settings = UltravioletApplicationSettings.FromCurrentSettings(Ultraviolet);
                UltravioletApplicationSettings.Save(path, settings);
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
        /// Writes a warning to the debug output if no file system source has been specified.
        /// </summary>
        private void WarnIfFileSystemSourceIsMissing()
        {
            if (FileSystemService.Source == null)
            {
                System.Diagnostics.Debug.WriteLine("WARNING: No file system source has been specified.");
            }
        }

        public UltravioletApplicationAdapter ApplicationAdapter => applicationAdapter;

        // Property values.
        private UltravioletContext uv;

        // State values.
        private readonly Object stateSyncObject = new Object();
        private IUltravioletTimingLogic timingLogic;
        private Boolean created;
        private Boolean running;
        private Boolean finished;
        private Boolean suspended;
        private Boolean disposed;
        private IUltravioletWindow primary;

        // The application's tick state.
        private Boolean isFixedTimeStep = UltravioletTimingLogic.DefaultIsFixedTimeStep;
        private TimeSpan targetElapsedTime = UltravioletTimingLogic.DefaultTargetElapsedTime;
        private TimeSpan inactiveSleepTime = UltravioletTimingLogic.DefaultInactiveSleepTime;

        // The application's settings.
        private UltravioletApplicationSettings settings;

        private UltravioletApplicationAdapter applicationAdapter = null;
    }
}