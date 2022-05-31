using System;
using System.Collections.Generic;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents the source code for an <see cref="Effect"/> object.
    /// </summary>
    public abstract class EffectSource
    {
        /// <summary>
        /// Compiles the source code into an <see cref="Effect"/> object.
        /// </summary>
        /// <param name="externs">The collection of extern values to provide to the effect compiler.</param>
        /// <returns>The compiled <see cref="Effect"/> instance.</returns>
        public abstract Effect Compile(Dictionary<String, String> externs = null);
    }
}
