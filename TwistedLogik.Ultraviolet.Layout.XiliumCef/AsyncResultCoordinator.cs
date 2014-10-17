using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Contains methods for coordinating the asynchronous retrieval of return values from layout scripts.
    /// </summary>
    internal static partial class AsyncResultCoordinator
    {
        /// <summary>
        /// Creates a new task completion source for coordinating async script results.
        /// </summary>
        /// <param name="taskID">The identifier of the task that was created.</param>
        /// <returns>The task completion source that was created.</returns>
        public static TaskCompletionSource<T> GetTaskCompletionSource<T>(out Int64 taskID)
        {
            taskID = Interlocked.Increment(ref AsyncResultCoordinator.taskID);

            var tcs = new TaskCompletionSource<T>();
            lock (tasks)
            {
                tasks[taskID] = new AsyncResultTask(taskID, tcs, typeof(T));
            }
            return tcs;
        }

        /// <summary>
        /// Sets the result value for the specified task.
        /// </summary>
        /// <param name="taskID">The identifier of the task for which to set a result value.</param>
        /// <param name="value">The value to set as the task result.</param>
        public static void SetTaskResult(Int64 taskID, JToken value)
        {
            AsyncResultTask task;
            lock (tasks)
            {
                if (!tasks.TryGetValue(taskID, out task))
                    throw new ArgumentException(XiliumStrings.UnrecognizedTaskID);

                tasks.Remove(taskID);
            }

            var result = value.ToObject(task.TaskResultType);

            var miSetResult = task.TaskCompletionSource.GetType().GetMethod("SetResult");
            miSetResult.Invoke(task.TaskCompletionSource, new[] { result });
        }

        // The next available task identifier.
        private static Int64 taskID = 0;

        // A dictionary associating task IDs with their completion sources.
        private static readonly Dictionary<Int64, AsyncResultTask> tasks = 
            new Dictionary<Int64, AsyncResultTask>();
    }
}
