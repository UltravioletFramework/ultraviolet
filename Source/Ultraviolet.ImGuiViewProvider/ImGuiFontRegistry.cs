using System;
using System.Runtime.InteropServices;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.ImGuiViewProvider.Bindings;

namespace Ultraviolet.ImGuiViewProvider
{
    /// <summary>
    /// Represents an ImGui view's registry of loaded fonts.
    /// </summary>
    public sealed class ImGuiFontRegistry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImGuiFontRegistry"/> class.
        /// </summary>
        /// <param name="imGuiContext">The ImGui context associated with this object.</param>
        internal ImGuiFontRegistry(IntPtr imGuiContext)
        {
            this.imGuiContext = imGuiContext;
        }
        
        /// <summary>
        /// Registers the default ImGui font for use.
        /// </summary>
        /// <returns>An <see cref="ImFontPtr"/> structure which represents the registered font.</returns>
        public ImFontPtr RegisterDefault()
        {
            if (locked)
                throw new InvalidOperationException(ImGuiStrings.RegistryIsLocked);

            if (ImGui.GetCurrentContext() != imGuiContext)
                throw new InvalidOperationException(ImGuiStrings.ImGuiContextIsNotCurrent);

            return ImGui.GetIO().Fonts.AddFontDefault();
        }

        /// <summary>
        /// Registers the specified TTF file for use.
        /// </summary>
        /// <returns>An <see cref="ImFontPtr"/> structure which represents the registered font.</returns>
        public ImFontPtr RegisterFromFileTTF(String filename, Single sizeInPixels)
        {
            Contract.RequireNotEmpty(filename, nameof(filename));

            if (locked)
                throw new InvalidOperationException(ImGuiStrings.RegistryIsLocked);

            if (ImGui.GetCurrentContext() != imGuiContext)
                throw new InvalidOperationException(ImGuiStrings.ImGuiContextIsNotCurrent);

            return ImGui.GetIO().Fonts.AddFontFromFileTTF(filename, sizeInPixels);
        }

        /// <summary>
        /// Registers the specified TTF asset for use.
        /// </summary>
        /// <returns>An <see cref="ImFontPtr"/> structure which represents the registered font.</returns>
        public ImFontPtr RegisterFromAssetTTF(ContentManager content, String asset, Single sizeInPixels)
        {
            Contract.Require(content, nameof(content));
            Contract.RequireNotEmpty(asset, nameof(asset));

            if (locked)
                throw new InvalidOperationException(ImGuiStrings.RegistryIsLocked);

            if (ImGui.GetCurrentContext() != imGuiContext)
                throw new InvalidOperationException(ImGuiStrings.ImGuiContextIsNotCurrent);

            var data = content.Import<Byte[]>(asset, true);
            var native = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, native, data.Length);

            unsafe
            {
                return ImGui.GetIO().Fonts.AddFontFromMemoryTTF(native, data.Length, sizeInPixels);
            }
        }

        /// <summary>
        /// Registers the specified TTF asset for use.
        /// </summary>
        /// <returns>An <see cref="ImFontPtr"/> structure which represents the registered font.</returns>
        public ImFontPtr RegisterFromAssetTTF(ContentManager content, AssetID asset, Single sizeInPixels)
        {
            Contract.Require(content, nameof(content));

            if (locked)
                throw new InvalidOperationException(ImGuiStrings.RegistryIsLocked);

            if (ImGui.GetCurrentContext() != imGuiContext)
                throw new InvalidOperationException(ImGuiStrings.ImGuiContextIsNotCurrent);

            var data = content.Import<Byte[]>(asset, true);
            var native = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, native, data.Length);

            unsafe
            {
                return ImGui.GetIO().Fonts.AddFontFromMemoryTTF(native, data.Length, sizeInPixels);
            }
        }

        /// <summary>
        /// Locks the registry, preventing further changes.
        /// </summary>
        internal void Lock()
        {
            this.locked = true;
        }

        // State values.
        private readonly IntPtr imGuiContext;
        private Boolean locked;
    }
}
