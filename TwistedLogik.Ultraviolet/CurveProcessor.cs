using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using TwistedLogik.Nucleus.Xml;
using TwistedLogik.Ultraviolet.Content;

namespace TwistedLogik.Ultraviolet
{
    /// <summary>
    /// Represents a content processor which processes XNA-formatted 
    /// curve definition XML files into instances of the Curve class.
    /// </summary>
    [ContentProcessor]
    internal sealed class CurveProcessor : ContentProcessor<XDocument, Curve>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete)
        {
            var curve = Process(manager, metadata, input);

            writer.Write((int)curve.PreLoop);
            writer.Write((int)curve.PostLoop);

            writer.Write(curve.Keys.Count);
            foreach (var key in curve.Keys)
            {
                writer.Write(key.Position);
                writer.Write(key.Value);
                writer.Write(key.TangentIn);
                writer.Write(key.TangentOut);
                writer.Write((int)key.Continuity);
            }
        }

        /// <inheritdoc/>
        public override Curve ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var preLoop  = (CurveLoopType)reader.ReadInt32();
            var postLoop = (CurveLoopType)reader.ReadInt32();

            var keyCount = reader.ReadInt32();
            var keyCollection = new List<CurveKey>();
            for (int i = 0; i < keyCount; i++)
            {
                var keyPosition   = reader.ReadSingle();
                var keyValue      = reader.ReadSingle();
                var keyTangentIn  = reader.ReadSingle();
                var keyTangentOut = reader.ReadSingle();
                var keyContinuity = (CurveContinuity)reader.ReadInt32();
                keyCollection.Add(new CurveKey(keyPosition, keyValue, keyTangentIn, keyTangentOut, keyContinuity));
            }

            return new Curve(preLoop, postLoop, keyCollection);
        }

        /// <inheritdoc/>
        public override Curve Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var asset = input.Root.Element("Asset");
            if (asset == null || asset.AttributeValueString("Type") != "Framework:Curve")
                throw new InvalidDataException(UltravioletStrings.InvalidCurveData);

            var preLoop  = asset.ElementValue<CurveLoopType>("PreLoop");
            var postLoop = asset.ElementValue<CurveLoopType>("PostLoop");

            const Int32 ComponentsPerKey = 5;

            var keysString     = asset.ElementValueString("Keys");
            var keysComponents = keysString.Split(' ');
            if (keysComponents.Length % ComponentsPerKey != 0)
                throw new InvalidDataException(UltravioletStrings.InvalidCurveData);

            var curveKeyCollection = new List<CurveKey>();
            for (int i = 0; i < keysComponents.Length; i += ComponentsPerKey)
            {
                var position   = Single.Parse(keysComponents[i + 0]);
                var value      = Single.Parse(keysComponents[i + 1]);
                var tangentIn  = Single.Parse(keysComponents[i + 2]);
                var tangentOut = Single.Parse(keysComponents[i + 3]);
                var continuity = (CurveContinuity)Enum.Parse(typeof(CurveContinuity), keysComponents[i + 4]);
                curveKeyCollection.Add(new CurveKey(position, value, tangentIn, tangentOut, continuity));
            }

            return new Curve(preLoop, postLoop, curveKeyCollection);
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing
        {
            get { return true; }
        }
    }
}
