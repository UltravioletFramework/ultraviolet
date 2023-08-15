using System;
using Ultraviolet.Core;

namespace Ultraviolet.TestApplication
{
    partial class UltravioletTestApplicationAdapter
    {
        /// <summary>
        /// Represents an action to perform on a particular frame.
        /// </summary>
        private struct FrameAction
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FrameAction"/> structure.
            /// </summary>
            /// <param name="actionType">The type of frame action which this structure represents.</param>
            /// <param name="actionIndex">The index of the frame, update, or render on which to perform the action.</param>
            /// <param name="action">The action to perform on the specified frame.</param>
            public FrameAction(FrameActionType actionType, Int32 actionIndex, Delegate action)
            {
                Contract.Require(action, nameof(action));

                this.ActionType = actionType;
                this.ActionIndex = actionIndex;
                this.Action = action;
            }

            /// <summary>
            /// Gets the type of frame action which this structure represents.
            /// </summary>
            public FrameActionType ActionType { get; }

            /// <summary>
            /// Gets the index of the frame, update, or render on which to perform the action.
            /// </summary>
            public Int32 ActionIndex { get; }

            /// <summary>
            /// Gets the action to perform on the specified frame.
            /// </summary>
            public Delegate Action { get; }
        }
    }
}
