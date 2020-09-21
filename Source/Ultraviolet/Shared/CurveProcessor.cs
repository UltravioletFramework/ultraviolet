using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Ultraviolet.Content;
using Ultraviolet.Core.Xml;
using CubicSplineSingleCurve = Ultraviolet.Curve<System.Single, Ultraviolet.CubicSplineCurveKey<System.Single>>;

namespace Ultraviolet
{
    /// <summary>
    /// Represents a content processor which processes XNA-formatted 
    /// curve definition XML files into instances of the Curve class.
    /// </summary>
    [ContentProcessor]
    internal sealed class CurveProcessor : ContentProcessor<XDocument, CubicSplineSingleCurve>
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
                var keyContinuity = record.SamplerOverride == null ? (Int32)CurveContinuity.CubicSpline :
                    GetCurveContinuityFromSampler(record.SamplerOverride);

                writer.Write(key.Position);
                writer.Write(key.Value);
                writer.Write(key.TangentIn);
                writer.Write(key.TangentOut);
                writer.Write((Int32)keyContinuity);
            }
        }

        /// <inheritdoc/>
        public override CubicSplineSingleCurve ImportPreprocessed(ContentManager manager, IContentProcessorMetadata metadata, BinaryReader reader)
        {
            var preLoop  = (CurveLoopType)reader.ReadInt32();
            var postLoop = (CurveLoopType)reader.ReadInt32();

            var keyCount = reader.ReadInt32();
            var keyContinuities = new List<CurveContinuity>();
            var keyCollection = new List<CubicSplineCurveKey<Single>>();
            for (int i = 0; i < keyCount; i++)
            {
                var keyPosition = reader.ReadSingle();
                var keyValue = reader.ReadSingle();
                var keyTangentIn = reader.ReadSingle();
                var keyTangentOut = reader.ReadSingle();
                var keyContinuity = (CurveContinuity)reader.ReadInt32();
                keyCollection.Add(new CubicSplineCurveKey<Single>(keyPosition, keyValue, keyTangentIn, keyTangentOut));
                keyContinuities.Add(keyContinuity);
            }

            return SingleCurve.CubicSpline(preLoop, postLoop, keyCollection);
        }

        /// <inheritdoc/>
        public override CubicSplineSingleCurve Process(ContentManager manager, IContentProcessorMetadata metadata, XDocument input)
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

            var curveKeyCollection = new List<CubicSplineCurveKey<Single>>();
            var curveKeyContinuities = new List<CurveContinuity>();
            for (int i = 0; i < keysComponents.Length; i += ComponentsPerKey)
            {
                var position   = Single.Parse(keysComponents[i + 0]);
                var value      = Single.Parse(keysComponents[i + 1]);
                var tangentIn  = Single.Parse(keysComponents[i + 2]);
                var tangentOut = Single.Parse(keysComponents[i + 3]);
                var continuity = (CurveContinuity)Enum.Parse(typeof(CurveContinuity), keysComponents[i + 4]);
                curveKeyCollection.Add(new CubicSplineCurveKey<Single>(position, value, tangentIn, tangentOut));
                curveKeyContinuities.Add(continuity);
            }

            var curve = SingleCurve.CubicSpline(preLoop, postLoop, curveKeyCollection);
            for (int i = 0; i < curveKeyContinuities.Count; i++)
            {
                var curveKeyContinuity = curveKeyContinuities[i];
                if (curveKeyContinuity == CurveContinuity.CubicSpline)
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
        private static CurveContinuity GetCurveContinuityFromSampler(ICurveSampler<Single, CubicSplineCurveKey<Single>> sampler)
        {
            if (sampler == SingleCurveCubicSplineSampler.Instance)
                return CurveContinuity.CubicSpline;

            if (sampler == SingleCurveStepSampler.Instance)
                return CurveContinuity.Step;

            if (sampler == SingleCurveLinearSampler.Instance)
                return CurveContinuity.Linear;

            throw new ArgumentOutOfRangeException(nameof(sampler));
        }

        /// <summary>
        /// Converts a <see cref="CurveContinuity"/> value to a sampler.
        /// </summary>
        private static ICurveSampler<Single, CubicSplineCurveKey<Single>> GetSamplerFromCurveContinuity(CurveContinuity curveContinuity)
        {
            switch (curveContinuity)
            {
                case CurveContinuity.CubicSpline:
                    return SingleCurveCubicSplineSampler.Instance;

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
