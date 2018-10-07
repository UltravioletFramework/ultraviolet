using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;

namespace Ultraviolet.OpenGL.Graphics
{
    /// <summary>
    /// Represents the OpenGL implementation of the <see cref="EffectSource"/> class.
    /// </summary>
    public class OpenGLEffectSource : EffectSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenGLEffectSource"/> class.
        /// </summary>
        /// <param name="assetType">The asset type that was loaded.</param>
        /// <param name="manager">The content manager with which the asset was loaded.</param>
        /// <param name="input">The intermediate form of the asset that was loaded.</param>
        /// <param name="metadata">The processor metadata for the asset that was loaded.</param>
        internal OpenGLEffectSource(OpenGLEffectSourceAssetType assetType, ContentManager manager, IContentProcessorMetadata metadata, Object input)
        {
            this.assetType = assetType;
            this.manager = manager;
            this.input = input;
            this.metadata = metadata;
        }

        /// <inheritdoc/>
        public override Effect Compile(Dictionary<String, String> externs)
        {
            Contract.EnsureNotDisposed(manager, manager.Disposed);

            switch (assetType)
            {
                case OpenGLEffectSourceAssetType.JObject:
                    {
                        var processor = new OpenGLEffectProcessorFromJObject() { Externs = externs };
                        return processor.Process(manager, metadata, (JObject)input);
                    }

                case OpenGLEffectSourceAssetType.ShaderSource:
                    {
                        var processor = new OpenGLEffectProcessorFromShaderSource() { Externs = externs };
                        return processor.Process(manager, metadata, (String)input);
                    }

                case OpenGLEffectSourceAssetType.XDocument:
                    {
                        var processor = new OpenGLEffectProcessorFromXDocument() { Externs = externs };
                        return processor.Process(manager, metadata, (XDocument)input);
                    }

                default:
                    throw new InvalidOperationException();
            }
        }

        // Asset data.
        private readonly OpenGLEffectSourceAssetType assetType;
        private readonly ContentManager manager;
        private readonly IContentProcessorMetadata metadata;
        private readonly Object input;
    }
}
