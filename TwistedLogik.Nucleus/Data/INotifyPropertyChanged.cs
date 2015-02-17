using System;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents the method that is called when an instance of the <see cref="INotifyPropertyChanged"/> interface is changed.
    /// </summary>
    /// <param name="propertyName">The name of the property that was changed. If all of the object's properties have
    /// changed, this value can be either <see cref="String.Empty"/> or <c>null</c>.</param>
    public delegate void NotifyChangedEventHandler(String propertyName);

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
