using System;
using System.Runtime.CompilerServices;
using Ultraviolet.Core;
using Ultraviolet.Presentation.Animations;
using Ultraviolet.Presentation.Input;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents the state of the Ultraviolet Presentation Foundation.
    /// </summary>
    public sealed partial class PresentationFoundation : UltravioletResource
    {
        /// <summary>
        /// Initializes the <see cref="PresentationFoundation"/> type.
        /// </summary>
        static PresentationFoundation() => CommandManager.RegisterValueResolvers();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="PresentationFoundation"/> class.
        /// </summary>
        private PresentationFoundation(UltravioletContext uv)
            : base(uv)
        {
            RuntimeHelpers.RunClassConstructor(typeof(Tweening).TypeHandle);
            RuntimeHelpers.RunClassConstructor(typeof(SimpleClockPool).TypeHandle);
            RuntimeHelpers.RunClassConstructor(typeof(StoryboardClockPool).TypeHandle);

            RegisterCoreTypes();

            this.outOfBandRenderer = uv.IsRunningInServiceMode ? null : new OutOfBandRenderer(uv);

            this.styleQueue = new LayoutQueue(InvalidateStyle, false);
            this.measureQueue = new LayoutQueue(InvalidateMeasure);
            this.arrangeQueue = new LayoutQueue(InvalidateArrange);          
        }

        /// <summary>
        /// Modifies the specified <see cref="UltravioletConfiguration"/> instance so that the Ultraviolet
        /// Presentation Foundation will be registered as the context's view provider.
        /// </summary>
        /// <param name="ultravioletConfig">The <see cref="UltravioletConfiguration"/> instance to modify.</param>
        /// <param name="presentationConfig">Configuration settings for the Ultraviolet Presentation Foundation.</param>
        public static void Configure(UltravioletConfiguration ultravioletConfig, PresentationFoundationConfiguration presentationConfig = null)
        {
            Contract.Require(ultravioletConfig, nameof(ultravioletConfig));

            ultravioletConfig.ViewProviderConfiguration = presentationConfig;
        }
        
        /// <summary>
        /// Gets the singleton instance of the Presentation Foundation.
        /// </summary>
        internal static PresentationFoundation Instance => instance;

        /// <summary>
        /// Gets the identifier of the current digest cycle.
        /// </summary>
        internal Int64 DigestCycleID => digestCycleID;

        /// <summary>
        /// Gets the identifier of the last digest cycle during which a layout occurred.
        /// </summary>
        internal Int64 DigestCycleIDOfLastLayout => digestCycleIDOfLastLayout;

        /// <summary>
        /// Gets the renderer which is used to draw elements out-of-band.
        /// </summary>
        internal OutOfBandRenderer OutOfBandRenderer => outOfBandRenderer;

        /// <inheritdoc/>
        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                SafeDispose.Dispose(outOfBandRenderer);
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Called when the Ultraviolet context blah blah blah
        /// </summary>
        /// <param name="uv"></param>
        private void OnFrameStart(UltravioletContext uv)
        {
            PerformanceStats.OnFrameStart();
        }

        /// <summary>
        /// Called when the Ultraviolet context is about to update its subsystems.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        private void OnUpdatingSubsystems(UltravioletContext uv, UltravioletTime time)
        {
            digestCycleID++;
        }

        /// <summary>
        /// Called when the Ultraviolet UI subsystem is being updated.
        /// </summary>
        /// <param name="subsystem">The Ultraviolet subsystem.</param>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update(UltravioletTime)"/>.</param>
        private void OnUpdatingUI(IUltravioletSubsystem subsystem, UltravioletTime time)
        {
            PerformanceStats.BeginUpdate();

            PerformLayout();

            if (OutOfBandRenderer != null)
                OutOfBandRenderer.Update();

            PerformanceStats.EndUpdate();
        }

        /// <summary>
        /// Called when the Ultraviolet context is about to draw a frame.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw(UltravioletTime)"/>.</param>
        private void OnDrawing(UltravioletContext uv, UltravioletTime time)
        {
            if (OutOfBandRenderer != null)
                OutOfBandRenderer.DrawRenderTargets(time);
        }
        
        // The singleton instance of the Ultraviolet Presentation Foundation.
        private static readonly UltravioletSingleton<PresentationFoundation> instance =
            new UltravioletSingleton<PresentationFoundation>(uv =>
            {
                var instance = new PresentationFoundation(uv);
                uv.FrameStart += instance.OnFrameStart;
                uv.UpdatingSubsystems += instance.OnUpdatingSubsystems;
                uv.GetUI().Updating += instance.OnUpdatingUI;
                uv.Drawing += instance.OnDrawing;
                return instance;
            });
        
        // The out-of-band element renderer.
        private readonly OutOfBandRenderer outOfBandRenderer;

        // The identifier of the current digest cycle.
        private Int64 digestCycleID = 1;
        private Int64 digestCycleIDOfLastLayout;
    }
}
