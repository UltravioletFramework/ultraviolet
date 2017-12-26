using System;

namespace Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Contains methods for coordinating the operations of multiple sprite batches.
    /// </summary>
    internal class SpriteBatchCoordinator : UltravioletResource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpriteBatchCoordinator"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private SpriteBatchCoordinator(UltravioletContext uv)
            : base(uv)
        { }

        /// <summary>
        /// Demands the right to operate in immediate mode.  If the right is denied, an InvalidOperationException is thrown.
        /// </summary>
        public void DemandImmediate()
        {
            if (immediate > 0 || deferred > 0)
                throw new InvalidOperationException(UltravioletStrings.SpriteBatchNestingError);

            immediate++;
        }

        /// <summary>
        /// Demands the right to operate in deferred mode.  If the right is denied, an InvalidOperationException is thrown.
        /// </summary>
        public void DemandDeferred()
        {
            if (immediate > 0)
                throw new InvalidOperationException(UltravioletStrings.SpriteBatchNestingError);

            deferred++;
        }

        /// <summary>
        /// Relinquishes the right to operate in immediate mode.
        /// </summary>
        public void RelinquishImmediate()
        {
            if (--immediate < 0)
                throw new InvalidOperationException(UltravioletStrings.SpriteBatchDemandImbalance);
        }

        /// <summary>
        /// Relinquishes the right to operate in deferred mode.
        /// </summary>
        public void RelinquishDeferred()
        {
            if (--deferred < 0)
                throw new InvalidOperationException(UltravioletStrings.SpriteBatchDemandImbalance);
        }

        /// <summary>
        /// Gets the singleton instance of the <see cref="SpriteBatchCoordinator"/> class.
        /// </summary>
        public static SpriteBatchCoordinator Instance => instance.Value;

        // The coordinator's singleton instance.
        private static UltravioletSingleton<SpriteBatchCoordinator> instance =
            new UltravioletSingleton<SpriteBatchCoordinator>(UltravioletSingletonFlags.DisabledInServiceMode, uv => new SpriteBatchCoordinator(uv));

        // Track how many batches are operating in each mode.
        private Int32 immediate;
        private Int32 deferred;
    }
}
