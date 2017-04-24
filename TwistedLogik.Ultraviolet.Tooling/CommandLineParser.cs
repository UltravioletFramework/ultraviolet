using System;
using System.Collections.Generic;
using Ultraviolet.Core;
using Ultraviolet.Core.Data;

namespace Ultraviolet.Tooling
{
    /// <summary>
    /// Contains methods for parsing command line arguments.
    /// </summary>
    public sealed class CommandLineParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineParser"/> class.
        /// </summary>
        /// <param name="args">The command line arguments to parse.</param>
        public CommandLineParser(IEnumerable<String> args)
        {
            Contract.Require(args, nameof(args));

            foreach (var arg in args)
            {
                if (!arg.StartsWith("-"))
                    continue;

                var ixDelimiter = arg.IndexOf(":");
                if (ixDelimiter >= 0)
                {
                    if (ixDelimiter == 0)
                    {
                        throw new InvalidCommandLineException();
                    }

                    var key = arg.Substring(1, ixDelimiter - 1);
                    var value = arg.Substring(ixDelimiter + 1);

                    arguments[key.ToLowerInvariant()] = value;
                }
                else
                {
                    var key = arg.Substring(1);

                    arguments[key.ToLowerInvariant()] = null;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified argument value represents a parameter (i.e. -foo:value).
        /// </summary>
        /// <param name="arg">The argument value to evaluate.</param>
        /// <returns><see langword="true"/> if the specified argument value represents a parameter; otherwise, <see langword="false"/>.</returns>
        public Boolean IsParameter(String arg)
        {
            Contract.RequireNotEmpty(arg, nameof(arg));

            return arg.StartsWith("-");
        }

        /// <summary>
        /// Gets a value indicating whether the command line arguments include an argument with the specified name.
        /// </summary>
        /// <param name="name">The name of the argument to evaluate.</param>
        /// <returns><see langword="true"/> if the command line arguments include an argument with the specified name; otherwise, <see langword="false"/>.</returns>
        public Boolean HasArgument(String name)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            return arguments.ContainsKey(name.ToLowerInvariant());
        }

        /// <summary>
        /// Gets the value of the specified argument.
        /// </summary>
        /// <param name="name">The name of the argument for which to retrieve a value.</param>
        /// <param name="value">The value of the specified argument.</param>
        /// <returns><see langword="true"/> if the specified argument exists and has a value; otherwise, <see langword="false"/>.</returns>
        public Boolean TryGetArgument(String name, out String value)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            String strval;
            if (!arguments.TryGetValue(name.ToLowerInvariant(), out strval))
            {
                value = null;
                return false;
            }
            if (strval == null)
            {
                value = null;
                return false;
            }
            value = strval;
            return true;
        }

        /// <summary>
        /// Gets the value of the specified argument.
        /// </summary>
        /// <param name="name">The name of the argument for which to retrieve a value.</param>
        /// <param name="value">The value of the specified argument.</param>
        /// <returns><see langword="true"/> if the specified argument exists and has a value; otherwise, <see langword="false"/>.</returns>
        public Boolean TryGetArgument<T>(String name, out T value)
        {
            Contract.RequireNotEmpty(name, nameof(name));

            String strval;
            if (!arguments.TryGetValue(name.ToLowerInvariant(), out strval))
            {
                value = default(T);
                return false;
            }
            if (strval == null)
            {
                value = default(T);
                return false;
            }
            value = (T)ObjectResolver.FromString(strval, typeof(T));
            return true;
        }

        /// <summary>
        /// Gets the value of the specified argument.
        /// </summary>
        /// <typeparam name="T">The type to which to convert the argument value.</typeparam>
        /// <param name="name">The name of the argument for which to retrieve a value.</param>
        /// <returns>The value of the specified argument.</returns>
        public T GetArgument<T>(String name)
        {
            T value;
            if (!TryGetArgument<T>(name, out value))
            {
                throw new InvalidCommandLineException();
            }
            return value;
        }

        /// <summary>
        /// Gets the value of the specified argument. If the specified argument does not exist or does not
        /// have a value, a default value is returned instead.
        /// </summary>
        /// <typeparam name="T">The type to which to convert the argument value.</typeparam>
        /// <param name="name">The name of the argument for which to retrieve a value.</param>
        /// <param name="defaultValue">The default value to return if the argument value cannot be retrieved.</param>
        /// <returns>The value of the specified argument.</returns>
        public T GetArgumentOrDefault<T>(String name, T defaultValue = default(T))
        {
            T value;
            if (!TryGetArgument<T>(name, out value))
            {
                return defaultValue;
            }
            return value;
        }

        // A dictionary associating arguments with their values.
        private readonly Dictionary<String, String> arguments = 
            new Dictionary<String, String>();
    }
}
