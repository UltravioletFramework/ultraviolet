using System;

namespace TwistedLogik.Nucleus.Data
{
    /// <summary>
    /// Represents the method that is called when an instance of the INotifyPropertyChanged interface is changed.
    /// </summary>
    /// <param name="instance">The instance that was changed.</param>
    /// <param name="property">The name of the property that was changed.</param>
    public delegate void NotifyPropertyChangedEventHandler(Object instance, String property);

    /// <summary>
    /// Represents an object which raises an event when one of its property values changes.
    /// </summary>
    public interface INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when one of the object's property values changes.
        /// </summary>
        event NotifyPropertyChangedEventHandler PropertyChanged;
    }
}
