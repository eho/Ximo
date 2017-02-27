using System;
using System.Diagnostics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Ximo.Validation;

namespace Ximo.Extensions
{
    /// <summary>
    ///     Class with <see cref="Action" /> utilities.
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        ///     Runs the action asynchronously in a background thread.
        /// </summary>
        /// <param name="action">The <see cref="Action" /> to be executed asynchronously.</param>
        /// <returns>The started <see cref="Task" /> object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action" /> is <c>null</c>.</exception>
        public static Task RunAsynchronously([NotNull] this Action action)
        {
            Check.NotNull(action, nameof(action));
            return Task.Factory.StartNew(action);
        }

        /// <summary>
        ///     Executes the specified <see cref="Action" /> and measures the time of execution.
        /// </summary>
        /// <param name="action">The <see cref="Action" /> to be executed and measured.</param>
        /// <returns>A <see cref="TimeSpan" /> object representing the time elapsed for execution.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="action" /> is <c>null</c>. </exception>
        public static TimeSpan ExecuteAndTime([NotNull] this Action action)
        {
            Check.NotNull(action, nameof(action));

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            action.Invoke();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}