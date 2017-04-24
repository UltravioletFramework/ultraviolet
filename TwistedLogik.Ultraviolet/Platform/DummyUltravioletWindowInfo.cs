using System;
using System.Collections;
using System.Collections.Generic;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents a dummy implementation of <see cref="IUltravioletWindowInfo"/>.
    /// </summary>
    public sealed class DummyUltravioletWindowInfo : IUltravioletWindowInfo
    {
        /// <inheritdoc/>
        public void DesignatePrimary(IUltravioletWindow window)
        {
        }

        /// <inheritdoc/>
        public IUltravioletWindow GetByID(Int32 id)
        {
            return null;
        }

        /// <inheritdoc/>
        public IUltravioletWindow GetPrimary()
        {
            return null;
        }

        /// <inheritdoc/>
        public IUltravioletWindow GetCurrent()
        {
            return null;
        }

        /// <inheritdoc/>
        public IUltravioletWindow Create(String caption, Int32 x, Int32 y, Int32 width, Int32 height, WindowFlags flags = WindowFlags.None)
        {
            return null;
        }

        /// <inheritdoc/>
        public IUltravioletWindow CreateFromNativePointer(IntPtr ptr)
        {
            return null;
        }

        /// <inheritdoc/>
        public Boolean Destroy(IUltravioletWindow window)
        {
            return true;
        }

        /// <inheritdoc/>
        public Boolean DestroyByID(Int32 id)
        {
            return true;
        }

        /// <inheritdoc/>
        IEnumerator<IUltravioletWindow> IEnumerable<IUltravioletWindow>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public List<IUltravioletWindow>.Enumerator GetEnumerator()
        {
            return windows.GetEnumerator();
        }

        /// <inheritdoc/>
        public event UltravioletWindowInfoEventHandler WindowCreated
        {
            add { }
            remove { }
        }

        /// <inheritdoc/>
        public event UltravioletWindowInfoEventHandler WindowDestroyed
        {
            add { }
            remove { }
        }

        /// <inheritdoc/>
        public event UltravioletWindowInfoEventHandler PrimaryWindowChanging
        {
            add { }
            remove { }
        }

        /// <inheritdoc/>
        public event UltravioletWindowInfoEventHandler PrimaryWindowChanged
        {
            add { }
            remove { }
        }

        /// <inheritdoc/>
        public event UltravioletWindowInfoEventHandler CurrentWindowChanging
        {
            add { }
            remove { }
        }

        /// <inheritdoc/>
        public event UltravioletWindowInfoEventHandler CurrentWindowChanged
        {
            add { }
            remove { }
        }

        // State values.
        private readonly List<IUltravioletWindow> windows = new List<IUltravioletWindow>();
    }
}
