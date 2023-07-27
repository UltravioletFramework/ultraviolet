using System;
using System.Diagnostics;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents a value associated with an OpenGL context that is cached by the OpenGL implementation
    /// of Ultraviolet in order to avoid costly calls to glGet() functions.
    /// </summary>
    internal class OpenGLStateInteger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLStateInteger"/> class.
        /// </summary>
        /// <param name="name">The human-readable name of this value.</param>
        /// <param name="pname">The property name of this value when retrieved via glGet().</param>
        /// <param name="initial">The initial value of the integer.</param>
        public OpenGLStateInteger(String name, UInt32 pname, Int32 initial = 0)
        {
            this.name  = name;
            this.pname = pname;
            this.initial = initial;
            this.value = initial;
        }

        /// <summary>
        /// Implicitly converts a cached value to an integer.
        /// </summary>
        /// <param name="value">The cached value to convert.</param>
        /// <returns>The converted integer.</returns>
        public static implicit operator Int32(OpenGLStateInteger value)
        {
            return value.value;
        }

        /// <summary>
        /// Implicitly converts a cached value to an integer.
        /// </summary>
        /// <param name="value">The cached value to convert.</param>
        /// <returns>The converted integer.</returns>
        public static implicit operator UInt32(OpenGLStateInteger value)
        {
            return (UInt32)value.value;
        }

        /// <summary>
        /// Resets the state of the cached integer.
        /// </summary>
        public void Reset()
        {
            this.value = initial;
        }

        /// <summary>
        /// Verifies that the cached value matches the value that is currently
        /// set on the current OpenGL context.
        /// </summary>
        [Conditional("VERIFY_OPENGL_CACHE")]
        public void Verify()
        {
            var valueOnContext = GL.GetInteger(pname);
            if (valueOnContext != value)
            {
                throw new InvalidOperationException(OpenGLStrings.StaleOpenGLCache.Format(name));
            }
        }

        /// <summary>
        /// Updates the cached value.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <returns>The old value.</returns>
        public Int32 Update(Int32 value)
        {
            var old = this.value;
            this.value = value;
            return old;
        }

        /// <summary>
        /// Updates the cached value.
        /// </summary>
        /// <param name="value">The new value.</param>
        /// <returns>The old value.</returns>
        public UInt32 Update(UInt32 value)
        {
            var old = (uint)this.value;
            this.value = (int)value;
            return old;
        }

        /// <summary>
        /// Converts the object to a human-readable string.
        /// </summary>
        /// <returns>A human-readable string that represents the object.</returns>
        public override String ToString()
        {
            return String.Format("{0} = {1}", Name, value);
        }

        /// <summary>
        /// Gets the value's human-readable name.
        /// </summary>
        public String Name
        {
            get { return name; }
        }

        // State values.
        private readonly String name;
        private readonly UInt32 pname;
        private Int32 value;
        private Int32 initial;
    }
}
