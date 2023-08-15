namespace Ultraviolet.TestApplication
{
    partial class UltravioletTestApplicationAdapter
    {
        /// <summary>
        /// Represents the types of frame actions which can be included in a test.
        /// </summary>
        private enum FrameActionType
        {
            /// <summary>
            /// Occurs at the start of a frame.
            /// </summary>
            FrameStart,

            /// <summary>
            /// Occurs at the start of Render().
            /// </summary>
            Render,

            /// <summary>
            /// Occurs at the start of Update().
            /// </summary>
            Update,
        }
    }
}