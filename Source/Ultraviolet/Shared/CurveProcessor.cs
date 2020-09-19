using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Ultraviolet.Content;
using Ultraviolet.Core.Xml;
using SmoothSingleCurve = Ultraviolet.Curve<System.Single, Ultraviolet.SmoothCurveKey<System.Single>>;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a content processor which processes XNA-formatted 
    /// curve definition XML files into instances of the Curve class.
    /// </summary>
    [ContentProcessor]
    internal sealed class CurveProcessor : ContentProcessor<XDocument, SmoothSingleCurve>
    {
        /// <inheritdoc/>
        public override void ExportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryWriter writer, XDocument input, Boolean delete)
        {
            var curve = Process(manager, metadata, input);

            writer.Write((Int32)curve.PreLoop);
            writer.Write((Int32)curve.PostLoop);

            writer.Write(curve.Keys.Count);
            foreach (var record in curve.Keys)
            {
                var key = record.Key;
                var keyContinuity = record.SamplerOverride == null ? (Int32)CurveContinuity.Smooth :
                    GetCurveContinuityFromSampler(record.SamplerOverride);

                writer.Write(key.Position);
                writer.Write(key.Value);
                writer.Write(key.TangentIn);
                writer.Write(key.TangentOut);
                writer.Write((Int32)keyContinuity);
            }
        }

        /// <inheritdoc/>
        public override SmoothSingleCurve ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var preLoop  = (CurveLoopType)reader.ReadInt32();
            var postLoop = (CurveLoopType)reader.ReadInt32();

            var keyCount = reader.ReadInt32();
            var keyContinuities = new List<CurveContinuity>();
            var keyCollection = new List<SmoothCurveKey<Single>>();
            for (int i = 0; i < keyCount; i++)
            {
                var keyPosition = reader.ReadSingle();
                var keyValue = reader.ReadSingle();
                var keyTangentIn = reader.ReadSingle();
                var keyTangentOut = reader.ReadSingle();
                var keyContinuity = (CurveContinuity)reader.ReadInt32();
                keyCollection.Add(new SmoothCurveKey<Single>(keyPosition, keyValue, keyTangentIn, keyTangentOut));
                keyContinuities.Add(keyContinuity);
            }

            return SingleCurve.Smooth(preLoop, postLoop, keyCollection);
        }

        /// <inheritdoc/>
        public override SmoothSingleCurve Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
        {
            var asset = input.Root.Element("Asset");
            if (asset == null || asset.AttributeValueString("Type") != "Framework:Curve")
                throw new InvalidDataException(UltravioletStrings.InvalidCurveData);

            var preLoop  = asset.ElementValue<CurveLoopType>("PreLoop");
            var postLoop = asset.ElementValue<CurveLoopType>("PostLoop");

            const Int32 ComponentsPerKey = 5;

            var keysString     = asset.ElementValueString("Keys");
            var keysComponents = keysString.Split((Char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (keysComponents.Length % ComponentsPerKey != 0)
                throw new InvalidDataException(UltravioletStrings.InvalidCurveData);

            var curveKeyCollection = new List<SmoothCurveKey<Single>>();
            var curveKeyContinuities = new List<CurveContinuity>();
            for (int i = 0; i < keysComponents.Length; i += ComponentsPerKey)
            {
                var position   = Single.Parse(keysComponents[i + 0]);
                var value      = Single.Parse(keysComponents[i + 1]);
                var tangentIn  = Single.Parse(keysComponents[i + 2]);
                var tangentOut = Single.Parse(keysComponents[i + 3]);
                var continuity = (CurveContinuity)Enum.Parse(typeof(CurveContinuity), keysComponents[i + 4]);
                curveKeyCollection.Add(new SmoothCurveKey<Single>(position, value, tangentIn, tangentOut));
                curveKeyContinuities.Add(continuity);
            }

            var curve = SingleCurve.Smooth(preLoop, postLoop, curveKeyCollection);
            for (int i = 0; i < curveKeyContinuities.Count; i++)
            {
                var curveKeyContinuity = curveKeyContinuities[i];
                if (curveKeyContinuity == CurveContinuity.Smooth)
                    continue;

                curve.Keys.OverrideKeySampler(i, GetSamplerFromCurveContinuity(curveKeyContinuity));
            }

            return curve;
        }

        /// <inheritdoc/>
        public override Boolean SupportsPreprocessing => true;

        /// <summary>
        /// Converts a sampler to a <see cref="CurveContinuity"/> value.
        /// </summary>
        private static CurveContinuity GetCurveContinuityFromSampler(ICurveSampler<Single, SmoothCurveKey<Single>> sampler)
        {
            if (sampler == SingleCurveSmoothSampler.Instance)
                return CurveContinuity.Smooth;

            if (sampler == SingleCurveStepSampler.Instance)
                return CurveContinuity.Step;

            if (sampler == SingleCurveLinearSampler.Instance)
                return CurveContinuity.Linear;

            throw new ArgumentOutOfRangeException(nameof(sampler));
        }

        /// <summary>
        /// Converts a <see cref="CurveContinuity"/> value to a sampler.
        /// </summary>
        private static ICurveSampler<Single, SmoothCurveKey<Single>> GetSamplerFromCurveContinuity(CurveContinuity curveContinuity)
        {
            switch (curveContinuity)
            {
                case CurveContinuity.Smooth:
                    return SingleCurveSmoothSampler.Instance;

                case CurveContinuity.Step:
                    return SingleCurveStepSampler.Instance;

                case CurveContinuity.Linear:
                    return SingleCurveLinearSampler.Instance;

                default:
                    throw new ArgumentOutOfRangeException(nameof(curveContinuity));
            }
        }
    }
}
