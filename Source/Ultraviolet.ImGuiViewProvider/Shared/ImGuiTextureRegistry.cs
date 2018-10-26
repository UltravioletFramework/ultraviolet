using System;
using System.Collections.Generic;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.ImGuiViewProvider
{
    /// <summary>
    /// Represents an ImGui view's registry of loaded textures.
    /// </summary>
    public sealed class ImGuiTextureRegistry
    {
        /// <summary>
        /// Gets the texture with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the texture to retrieve.</param>
        /// <returns>The texture with the specified identifier, or <see langword="null"/> if no such texture exists.</returns>
        public Texture2D Get(Int32 id)
        {
            registry.TryGetValue(id, out var texture);
            return texture;
        }

        /// <summary>
        /// Registers the specified texture.
        /// </summary>
        /// <param name="texture">The texture to register.</param>
        /// <returns>The identifier which was assigned to the specified texture.</returns>
        public Int32 Register(Texture2D texture)
        {
            Contract.Require(texture, nameof(texture));

            var id = GetNextTextureID();
            registry.Add(id, texture);
            return id;
        }

        /// <summary>
        /// Registers the specified texture.
        /// </summary>
        /// <param name="content">The content manager with which to load the texture.</param>
        /// <param name="asset">The asset that represents the texture.</param>
        /// <returns>The identifier which was assigned to the specified texture.</returns>
        public Int32 Register(ContentManager content, String asset)
        {
            Contract.Require(content, nameof(content));
            Contract.RequireNotEmpty(asset, nameof(asset));

            var id = GetNextTextureID();
            var texture = content.Load<Texture2D>(asset);
            registry.Add(id, texture);
            return id;
        }

        /// <summary>
        /// Registers the specified texture.
        /// </summary>
        /// <param name="content">The content manager with which to load the texture.</param>
        /// <param name="asset">The asset that represents the texture.</param>
        /// <returns>The identifier which was assigned to the specified texture.</returns>
        public Int32 Register(ContentManager content, AssetID asset)
        {
            Contract.Require(content, nameof(content));

            var id = GetNextTextureID();
            var texture = content.Load<Texture2D>(asset);
            registry.Add(id, texture);
            return id;
        }

        /// <summary>
        /// Unregisters the texture with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the texture to unregister.</param>
        /// <returns><see langword="true"/> if the texture was unregistered; otherwise, <see langword="false"/>.</returns>
        public Boolean Unregister(Int32 id)
        {
            return registry.Remove(id);
        }

        /// <summary>
        /// Gets the next available texture identifier.
        /// </summary>
        /// <returns>The texture identifier which was found.</returns>
        private Int32 GetNextTextureID()
        {
            void Advance(ref Int32 value) => value = (value == Int32.MaxValue) ? 1 : value + 1;

            var attempts = 0;
            while (registry.ContainsKey(nextTextureID))
            {
                Advance(ref nextTextureID);

                if (++attempts == Int32.MaxValue)
                    throw new InvalidOperationException();
            }

            var id = nextTextureID;
            Advance(ref nextTextureID);
            return id;
        }

        // Texture registry.
        private readonly Dictionary<Int32, Texture2D> registry = new Dictionary<int, Texture2D>();
        private Int32 nextTextureID = 1;
    }
}
