using System;
using Ultraviolet.OpenGL.Bindings;

namespace Ultraviolet.OpenGL.Graphics.Caching
{
    /// <summary>
    /// Represents the cached value of a capability which is enabled by glEnable().
    /// </summary>
    internal unsafe struct CachedCapability
    {
        private CachedCapability(Boolean value)
        {
            this.value = value;
        }

        public static implicit operator CachedCapability(Boolean value) => new CachedCapability(value);

        public static explicit operator Boolean(CachedCapability capability) => capability.value;

        public static CachedCapability FromDevice(UInt32 cap)
        {
            var enabled = GL.IsEnabled(cap);
            GL.ThrowIfError();

            return enabled;
        }

        public static Boolean TryUpdate(UInt32 cap, ref CachedCapability current, Boolean desired)
        {
            if (current.value == desired)
                return false;

            current = desired;
            GL.Enable(cap, desired);
            GL.ThrowIfError();

            return true;
        }

        private readonly Boolean value;
    }
}
