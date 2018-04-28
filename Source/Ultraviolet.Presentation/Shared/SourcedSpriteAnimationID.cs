using System;
using Newtonsoft.Json;
using Ultraviolet.Core.Data;
using Ultraviolet.Graphics.Graphics2D;

namespace Ultraviolet.Presentation
{
    /// <summary>
    /// Represents an animation identifier which is flagged as being either globally- or locally-sourced.
    /// </summary>
    [JsonConverter(typeof(ObjectResolverJsonConverter))]
    public partial struct SourcedSpriteAnimationID : IEquatable<SourcedSpriteAnimationID>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourcedSpriteAnimationID"/> class.
        /// </summary>
        /// <param name="spriteAnimationID">The sprite animation's identifier.</param>
        /// <param name="spriteSource">The sprite asset's source.</param>
        public SourcedSpriteAnimationID(SpriteAnimationID spriteAnimationID, AssetSource spriteSource)
        {
            this.spriteAnimationID = spriteAnimationID;
            this.spriteSource = spriteSource;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{SpriteAnimationID} {SpriteSource.ToString().ToLowerInvariant()}";

        /// <summary>
        /// Gets the sprite animation's identifier.
        /// </summary>
        public SpriteAnimationID SpriteAnimationID
        {
            get { return spriteAnimationID; }
        }

        /// <summary>
        /// Gets the sprite's source.
        /// </summary>
        public AssetSource SpriteSource
        {
            get { return spriteSource; }
        }
        
        // Property values.
        private readonly SpriteAnimationID spriteAnimationID;
        private readonly AssetSource spriteSource;
    }
}
