using System;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents the method that is called when an instance of the <see cref="INotifyPropertyChanged"/> interface is changed.
    /// </summary>
    /// <param name="instance">The instance that was changed.</param>
    public delegate void NotifyChangedEventHandler(Object instance);

    /// <summary>
    /// Represents an object which raises an event when one of its property values changes.
    /// </summary>
    public interface INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when one of the object's property values changes.
        /// </summary>
        event NotifyChangedEventHandler Changed;
    }
}
