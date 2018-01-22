using System;

namespace Ultraviolet.Presentation.Uvml
{
    /// <summary>
    /// Represents an uninitialized object instance produced by a UVML template.
    /// </summary>
    internal sealed class UvmlTemplateInstance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UvmlTemplateInstance"/> class.
        /// </summary>
        /// <param name="instance">The object instance which this object represents.</param>
        /// <param name="initializer">A delegate which initializes the specified object instance.</param>
        public UvmlTemplateInstance(Object instance, Action initializer)
        {
            this.instance = instance;
            this.initializer = initializer;
        }

        /// <summary>
        /// Finalizes the object and returns the result.
        /// </summary>
        /// <returns>The finalized object instance.</returns>
        public Object FinalizeInstance()
        {
            if (finalized)
                throw new InvalidOperationException();

            initializer();
            finalized = true;

            return instance;
        }

        // State values.
        private readonly Object instance;
        private readonly Action initializer;
        private Boolean finalized;
    }
}
