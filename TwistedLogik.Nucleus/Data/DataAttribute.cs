using System;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents a hierarchical data definition attribute which can be queried by the Nucleus Object Loader.
    /// </summary>
    public abstract class DataAttribute
    {
        /// <summary>
        /// Gets the attribute's name.
        /// </summary>
        public abstract String Name
        {
            get;
        }

        /// <summary>
        /// Gets the attribute's value.
        /// </summary>
        public abstract String Value
        {
            get;
        }
    }
}
