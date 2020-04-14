using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Ultraviolet.Core;

namespace Ultraviolet.Windows.Forms
{
    /// <summary>
    /// Represents the primary Form for a Windows Forms application using the Ultraviolet engine.
    /// </summary>
    public partial class UltravioletForm : Form, IUltravioletHost
    {
        /// <summary>
        /// Contains native methods used by the host.
        /// </summary>
        private static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Message
            {
                public IntPtr hWnd;
                public uint Msg;
                public IntPtr wParam;
                public IntPtr lParam;
                public uint Time;
                public System.Drawing.Point Point;
            }

            [DllImport("User32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern Boolean PeekMessage(out Message message, IntPtr hWnd, uint filterMin, uint filterMax, uint flags);
        }

        /// <summary>
        /// Initializes a new instance of the UltravioletForm class.
        /// </summary>
        public UltravioletForm()
        {
            InitializeComponent();
            InitializeUltraviolet();
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        void IUltravioletHost.Exit()
        {
            Close();
        }

        /// <summary>
        /// Gets the Ultraviolet context.
        /// </summary>
        public UltravioletContext Ultraviolet
        {
            get
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                return uv;
            }
        }

        /// <inheritdoc/>
        public String DeveloperName { get; set; }

        /// <inheritdoc/>
        public String ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the application's primary window is synchronized
        /// to the vertical retrace when rendering (i.e., whether vsync is enabled).
        /// </summary>
        public Boolean SynchronizeWithVerticalRetrace
        {
            get
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                if (!DesignMode)
                {
                    var primary = Ultraviolet.GetPlatform().Windows.GetPrimary();
                    if (primary == null)
                        throw new InvalidOperationException(UltravioletStrings.NoPrimaryWindow);

                    return primary.SynchronizeWithVerticalRetrace;
                }
                return false;
            }
            set
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                if (!DesignMode)
                {
                    var primary = Ultraviolet.GetPlatform().Windows.GetPrimary();
                    if (primary == null)
                        throw new InvalidOperationException(UltravioletStrings.NoPrimaryWindow);

                    primary.SynchronizeWithVerticalRetrace = value;
                }
            }
        }

        /// <inheritdoc/>
        public Boolean IsActive
        {
            get
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                return this.WindowState != FormWindowState.Minimized && ContainsFocus;
            }
        }

        /// <inheritdoc/>
        public Boolean IsSuspended
        {
            get
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                return false;
            }
        }

        /// <inheritdoc/>
        public Boolean IsFixedTimeStep
        {
            get
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                return this.isFixedTimeStep;
            }
            set
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

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
                Contract.EnsureNotDisposed(this, IsDisposed);

                return this.targetElapsedTime;
            }
            set
            {
                Contract.EnsureNotDisposed(this, IsDisposed);
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
                Contract.EnsureNotDisposed(this, IsDisposed);

                return this.inactiveSleepTime;
            }
            set
            {
                Contract.EnsureNotDisposed(this, IsDisposed);

                this.inactiveSleepTime = value;
                if (timingLogic != null)
                {
                    timingLogic.InactiveSleepTime = value;
                }
            }
        }

        /// <summary>
        /// Called when the application is creating its Ultraviolet context.
        /// </summary>
        /// <returns>The application's Ultraviolet context.</returns>
        protected virtual UltravioletContext OnCreatingUltravioletContext()
        {
            return null;
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
        /// Occurs when the Ultraviolet context is updating its state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        protected virtual void OnUpdating(UltravioletTime time)
        {

        }

        /// <summary>
        /// Called when the application is being shut down.
        /// </summary>
        protected virtual void OnShutdown()
        {

        }

        /// <summary>
        /// Raises the Closing event.
        /// </summary>
        /// <param name="e">A CancelEventArgs that contains the event data.</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (!DesignMode)
            {
                Application.Idle -= Application_Idle;

                timingLogic.Cleanup();

                if (uv != null)
                    uv.WaitForPendingTasks(true);
            }
            base.OnClosing(e);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(components);
                SafeDispose.Dispose(uv);

                timingLogic = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Initializes the Ultraviolet engine.
        /// </summary>
        private void InitializeUltraviolet()
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            OnInitializing();

            uv = OnCreatingUltravioletContext();
            if (uv == null)
                throw new InvalidOperationException(UltravioletStrings.ContextNotCreated);

            this.timingLogic = CreateTimingLogic();
            if (this.timingLogic == null)
                throw new InvalidOperationException(UltravioletStrings.InvalidTimingLogic);

            uv.Updating += uv_Updating;
            uv.Shutdown += uv_Shutdown;

            Application.Idle += Application_Idle;

            OnInitialized();

            OnLoadingContent();
        }

        /// <summary>
        /// Creates the Ultraviolet host core for this host process.
        /// </summary>
        protected virtual IUltravioletTimingLogic CreateTimingLogic()
        {
            var timingLogic = new UltravioletTimingLogic(this);
            timingLogic.IsFixedTimeStep = this.IsFixedTimeStep;
            timingLogic.TargetElapsedTime = this.TargetElapsedTime;
            timingLogic.InactiveSleepTime = this.InactiveSleepTime;
            return timingLogic;
        }

        /// <summary>
        /// Handles the application's Idle event.
        /// </summary>
        /// <param name="sender">The object that raised the event.</param>
        /// <param name="e">An EventArgs that contains the event data.</param>
        private void Application_Idle(Object sender, EventArgs e)
        {
            NativeMethods.Message message;
            while (!NativeMethods.PeekMessage(out message, IntPtr.Zero, 0, 0, 0))
            {
                if (!Ultraviolet.Disposed)
                {
                    Tick();
                }
            }
        }

        /// <summary>
        /// Processes one application tick.
        /// </summary>
        private void Tick()
        {
            timingLogic.RunOneTick();
        }

        /// <summary>
        /// Handles the Ultraviolet context's Updating event.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="time">Time elapsed since the last call to Update.</param>
        private void uv_Updating(UltravioletContext uv, UltravioletTime time)
        {
            OnUpdating(time);
        }

        /// <summary>
        /// Called when the application is being shut down.
        /// </summary>
        private void uv_Shutdown(UltravioletContext uv)
        {
            OnShutdown();
        }

        // The Ultraviolet context.
        private UltravioletContext uv;
        private IUltravioletTimingLogic timingLogic;

        // The application's tick state.
        private Boolean isFixedTimeStep = UltravioletTimingLogic.DefaultIsFixedTimeStep;
        private TimeSpan targetElapsedTime = UltravioletTimingLogic.DefaultTargetElapsedTime;
        private TimeSpan inactiveSleepTime = UltravioletTimingLogic.DefaultInactiveSleepTime;
    }
}
