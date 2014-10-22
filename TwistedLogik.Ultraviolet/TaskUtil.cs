using System.Threading.Tasks;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Contains utility methods for creating Task objects.
    /// </summary>
    internal static class TaskUtil
    {
        /// <summary>
        /// Creates a Task object which encapsulates the specified result value.
        /// </summary>
        /// <typeparam name="T">The type of value returned from the task.</typeparam>
        /// <param name="value">The result value.</param>
        /// <returns>The Task that was created.</returns>
        public static Task<T> FromResult<T>(T value)
        {
            var task = new Task<T>(() => value);
            task.RunSynchronously();
            return task;
        }
    }
}
