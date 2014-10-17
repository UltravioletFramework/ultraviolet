using System;

namespace TwistedLogik.Ultraviolet.Graphics.Graphics2D
{
    /// <summary>
    /// Contains methods for coordinating the operations of multiple sprite batches.
    /// </summary>
    internal static class SpriteBatchCoordinator
    {
        /// <summary>
        /// Demands the right to operate in immediate mode.  If the right is denied, an InvalidOperationException is thrown.
        /// </summary>
        public static void DemandImmediate()
        {
            if (immediate > 0 || deferred > 0)
                throw new InvalidOperationException(UltravioletStrings.SpriteBatchNestingError);

            immediate++;
        }

        /// <summary>
        /// Demands the right to operate in deferred mode.  If the right is denied, an InvalidOperationException is thrown.
        /// </summary>
        public static void DemandDeferred()
        {
            if (immediate > 0)
                throw new InvalidOperationException(UltravioletStrings.SpriteBatchNestingError);

            deferred++;
        }

        /// <summary>
        /// Relinquishes the right to operate in immediate mode.
        /// </summary>
        public static void RelinquishImmediate()
        {
            if (--immediate < 0)
                throw new InvalidOperationException(UltravioletStrings.SpriteBatchDemandImbalance);
        }

        /// <summary>
        /// Relinquishes the right to operate in deferred mode.
        /// </summary>
        public static void RelinquishDeferred()
        {
            if (--deferred < 0)
                throw new InvalidOperationException(UltravioletStrings.SpriteBatchDemandImbalance);
        }

        // Track how many batches are operating in each mode.
        private static Int32 immediate;
        private static Int32 deferred;
    }
}
