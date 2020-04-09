using System;
using System.IO;
using System.Reflection;
using Android.App;

namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents an implementation of the <see cref="AssemblyLoaderService"/> class specific to the Android platform.
    /// </summary>
    public sealed class AndroidAssemblyLoaderService : AssemblyLoaderService
    {
        /// <inheritdoc/>
        public override Assembly Load(String name)
        {
            name = name.Replace('\\', '/');

            var context = Application.Context;

            using (var srcStream = context.Assets.Open(name))
            using (var dstStream = new MemoryStream())
            {
                srcStream.CopyTo(dstStream);
                return Assembly.Load(dstStream.ToArray());
            }
        }
    }
}