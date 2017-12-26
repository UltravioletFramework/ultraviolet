using System;
using Ultraviolet.Core;

namespace Ultraviolet.TestFramework
{
    partial class UltravioletTestApplication
    {
        /// <summary>
        /// Represents an action to perform on a particular frame.
        /// </summary>
        private struct FrameAction
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FrameAction"/> structure.
            /// </summary>
            /// <param name="frame">The index of the frame on which to perform the action.</param>
            /// <param name="action">The action to perform on the specified frame.</param>
            public FrameAction(Int32 frame, Action<IUltravioletTestApplication> action)
            {
                Contract.Require(action, nameof(action));

                this.frame = frame;
                this.action = action;
            }

            /// <summary>
            /// Gets the index of the frame on which to perform the action.
            /// </summary>
            public Int32 Frame
            {
                get { return frame; }
            }

            /// <summary>
            /// Gets the action to perform on the specified frame.
            /// </summary>
            public Action<IUltravioletTestApplication> Action
            {
                get { return action; }
            }

            // Property values.
            private readonly Int32 frame;
            private readonly Action<IUltravioletTestApplication> action;
        }
    }
}
