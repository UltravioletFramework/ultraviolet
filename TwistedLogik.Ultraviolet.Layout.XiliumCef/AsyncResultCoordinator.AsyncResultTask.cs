using System;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    partial class AsyncResultCoordinator
    {
        /// <summary>
        /// Represents a task which is awaiting the result of an asynchronous JavaScript call.
        /// </summary>
        private struct AsyncResultTask
        {
            /// <summary>
            /// Initializes a new instance of the AsyncResultTask structure.
            /// </summary>
            /// <param name="taskID">The task's unique identifier.</param>
            /// <param name="taskCompletionSource">The task's completion source.</param>
            /// <param name="taskType">The task's expected result type.</param>
            public AsyncResultTask(Int64 taskID, Object taskCompletionSource, Type taskType)
            {
                this.TaskID = taskID;
                this.TaskCompletionSource = taskCompletionSource;
                this.TaskResultType = taskType;
            }

            /// <summary>
            /// The task's unique identifier.
            /// </summary>
            public readonly Int64 TaskID;

            /// <summary>
            /// The task's completion source.
            /// </summary>
            public readonly Object TaskCompletionSource;

            /// <summary>
            /// The task's expected result type.
            /// </summary>
            public readonly Type TaskResultType;
        }
    }
}
