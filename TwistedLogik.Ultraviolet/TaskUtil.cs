using System;
using System.Threading.Tasks;

namespace Ultraviolet
{
    /// <summary>
    /// Contains utility methods for creating Task objects.
    /// </summary>
    internal static class TaskUtil
    {
        /// <summary>
        /// Executes the specified action and creates a <see cref="Task"/> which returns
        /// the Boolean value <see langword="true"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns>The <see cref="Task"/> that was created.</returns>
        public static Task FromAction(Action action)
        {
            action();

            var task = new Task<Boolean>(() => true);
            task.RunSynchronously();
            return task;
        }

        /// <summary>
        /// Executes the specified action and creates a <see cref="Task"/> which returns
        /// the Boolean value <see langword="true"/>.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="state">A state value to pass to the executed action.</param>
        /// <returns>The <see cref="Task"/> that was created.</returns>
        public static Task FromAction(Action<Object> action, Object state)
        {
            action(state);

            var task = new Task<Boolean>(() => true);
            task.RunSynchronously();
            return task;
        }

        /// <summary>
        /// Creates a <see cref="Task"/> object which returns the specified result value.
        /// </summary>
        /// <typeparam name="T">The type of value returned from the task.</typeparam>
        /// <param name="value">The result value.</param>
        /// <returns>The <see cref="Task"/> that was created.</returns>
        public static Task<T> FromResult<T>(T value)
        {
            var task = new Task<T>(() => value);
            task.RunSynchronously();
            return task;
        }        
    }
}
