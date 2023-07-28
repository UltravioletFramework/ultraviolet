using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using CsvHelper;
using SharpGLTF.Animations;
using Ultraviolet.Core;

namespace Ultraviolet.Tests
{
    /// <summary>
    /// Contains utility methods for testing curves.
    /// </summary>
    public static class CurveTestUtils
    {
        /// <summary>
        /// Reads a curve from the specified CSV file.
        /// </summary>
        public static Curve<TValue, CurveKey<TValue>> ReadCurveFromCsv<TValue>(String path, Ultraviolet.ICurveSampler<TValue, CurveKey<TValue>> sampler)
            where TValue : new()
        {
            using (var file = File.OpenRead(path))
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                var header = csv.HeaderRecord.Select((item, ix) => new { Item = item, Index = ix })
                    .ToDictionary(x => x.Item, x => x.Index);

                var keys = new List<CurveKey<TValue>>();

                while (csv.Read())
                {
                    var keyPosition = csv.GetField<Single>(header["Position"]);
                    var keyValue = (Object)new TValue();
                    ReadCsvFields<TValue>(csv, "Value_", keyValue, header);

                    var key = new CurveKey<TValue>(keyPosition, (TValue)keyValue);
                    keys.Add(key);
                }

                return new Curve<TValue, CurveKey<TValue>>(CurveLoopType.Cycle, CurveLoopType.Cycle, sampler, keys);
            }
        }

        /// <summary>
        /// Reads a curve from the specified CSV file.
        /// </summary>
        public static Curve<TValue, CubicSplineCurveKey<TValue>> ReadCurveFromCsv<TValue>(String path, Ultraviolet.ICurveSampler<TValue, CubicSplineCurveKey<TValue>> sampler)
            where TValue : new()
        {
            using (var file = File.OpenRead(path))
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                var header = csv.HeaderRecord.Select((item, ix) => new { Item = item, Index = ix })
                    .ToDictionary(x => x.Item, x => x.Index);

                var keys = new List<CubicSplineCurveKey<TValue>>();

                while (csv.Read())
                {
                    var keyPosition = csv.GetField<Single>(header["Position"]);
                    var keyValue = (Object)new TValue();
                    ReadCsvFields<TValue>(csv, "Value_", keyValue, header);

                    var keyTangentIn = (Object)new TValue();
                    ReadCsvFields<TValue>(csv, "TangentIn_", keyTangentIn, header);

                    var keyTangentOut = (Object)new TValue();
                    ReadCsvFields<TValue>(csv, "TangentOut_", keyTangentOut, header);

                    var key = new CubicSplineCurveKey<TValue>(keyPosition, (TValue)keyValue, (TValue)keyTangentIn, (TValue)keyTangentOut);
                    keys.Add(key);
                }

                return new Curve<TValue, CubicSplineCurveKey<TValue>>(CurveLoopType.Cycle, CurveLoopType.Cycle, sampler, keys);
            }
        }

        /// <summary>
        /// Reads a collection of samples from the specified CSV file.
        /// </summary>
        public static IEnumerable<(Single Position, TValue Value)> ReadSamplesFromCsv<TValue>(String path)
            where TValue : new()
        {
            using (var file = File.OpenRead(path))
            using (var reader = new StreamReader(file))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                var header = csv.HeaderRecord.Select((item, ix) => new { Item = item, Index = ix })
                    .ToDictionary(x => x.Item, x => x.Index);

                var samples = new List<(Single Position, TValue Value)>();

                while (csv.Read())
                {
                    var keyPosition = csv.GetField<Single>(header["Position"]);
                    var keyValue = (Object)new TValue();
                    ReadCsvFields<TValue>(csv, null, keyValue, header);

                    samples.Add((keyPosition, (TValue)keyValue));
                }

                return samples;
            }
        }

        /// <summary>
        /// Reads the CSV fields for the specified value.
        /// </summary>
        private static void ReadCsvFields<TValue>(CsvReader csv, String prefix, Object value, Dictionary<String, Int32> header)
        {
            var fields = typeof(TValue).GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (header.TryGetValue($"{prefix}{field.Name}", out var fieldIndex))
                {
                    var fieldValue = csv.GetField(field.FieldType, fieldIndex);
                    field.SetValue(value, fieldValue);
                }
            }
        }

        /// <summary>
        /// Writes the specified curve's data files to the specified directory.
        /// </summary>
        /// <param name="dirpath">The path to the directory in which to write the curve's files.</param>
        /// <param name="basename">The base name given to the curve's data files.</param>
        /// <param name="curve">The curve to write.</param>
        /// <param name="sampleCount">The number of samples to write.</param>
        public static void WriteToCsv<TValue, TKey>(String dirpath, String basename, Curve<TValue, TKey> curve, Int32 sampleCount)
            where TKey : CurveKey<TValue>
        {
            Contract.Require(dirpath, nameof(dirpath));
            Contract.Require(basename, nameof(basename));
            Contract.Require(curve, nameof(curve));
            Contract.EnsureRange(sampleCount > 0, nameof(sampleCount));

            var keyFilePath = Path.Combine(dirpath, $"{basename}_keys.csv");
            using (var keyFileStream = File.OpenWrite(keyFilePath))
            {
                WriteKeysToCsv(keyFileStream, curve);
            }

            var sampleFilePath = Path.Combine(dirpath, $"{basename}_samples.csv");
            using (var sampleFileStream = File.OpenWrite(sampleFilePath))
            {
                WriteSamplesToCsv(sampleFileStream, curve, sampleCount);
            }
        }

        /// <summary>
        /// Writes the specified curve's data files to the specified directory using SharpGLTF to perform sampling.
        /// </summary>
        /// <param name="dirpath">The path to the directory in which to write the curve's files.</param>
        /// <param name="basename">The base name given to the curve's data files.</param>
        /// <param name="curve">The curve to write.</param>
        /// <param name="sampleCount">The number of samples to write.</param>
        public static void WriteToCsvUsingSharpGltf<TKey>(String dirpath, String basename, Curve<Single, TKey> curve, Int32 sampleCount)
            where TKey : CurveKey<Single>
        {
            Contract.Require(dirpath, nameof(dirpath));
            Contract.Require(basename, nameof(basename));
            Contract.Require(curve, nameof(curve));
            Contract.EnsureRange(sampleCount > 0, nameof(sampleCount));

            var keyFilePath = Path.Combine(dirpath, $"{basename}_keys.csv");
            using (var keyFileStream = File.OpenWrite(keyFilePath))
            {
                WriteKeysToCsv(keyFileStream, curve);
            }

            var sampleFilePath = Path.Combine(dirpath, $"{basename}_samples.csv");
            using (var sampleFileStream = File.OpenWrite(sampleFilePath))
            {
                WriteSamplesToCsvUsingSharpGltf(sampleFileStream, curve, sampleCount);
            }
        }

        /// <summary>
        /// Writes the specified curve's data files to the specified directory using SharpGLTF to perform sampling.
        /// </summary>
        /// <param name="dirpath">The path to the directory in which to write the curve's files.</param>
        /// <param name="basename">The base name given to the curve's data files.</param>
        /// <param name="curve">The curve to write.</param>
        /// <param name="sampleCount">The number of samples to write.</param>
        public static void WriteToCsvUsingSharpGltf<TKey>(String dirpath, String basename, Curve<Vector3, TKey> curve, Int32 sampleCount)
            where TKey : CurveKey<Vector3>
        {
            Contract.Require(dirpath, nameof(dirpath));
            Contract.Require(basename, nameof(basename));
            Contract.Require(curve, nameof(curve));
            Contract.EnsureRange(sampleCount > 0, nameof(sampleCount));

            var keyFilePath = Path.Combine(dirpath, $"{basename}_keys.csv");
            using (var keyFileStream = File.OpenWrite(keyFilePath))
            {
                WriteKeysToCsv(keyFileStream, curve);
            }

            var sampleFilePath = Path.Combine(dirpath, $"{basename}_samples.csv");
            using (var sampleFileStream = File.OpenWrite(sampleFilePath))
            {
                WriteSamplesToCsvUsingSharpGltf(sampleFileStream, curve, sampleCount);
            }
        }
        
        /// <summary>
        /// Writes the specified curve's data files to the specified directory using SharpGLTF to perform sampling.
        /// </summary>
        /// <param name="dirpath">The path to the directory in which to write the curve's files.</param>
        /// <param name="basename">The base name given to the curve's data files.</param>
        /// <param name="curve">The curve to write.</param>
        /// <param name="sampleCount">The number of samples to write.</param>
        public static void WriteToCsvUsingSharpGltf<TKey>(String dirpath, String basename, Curve<Quaternion, TKey> curve, Int32 sampleCount)
            where TKey : CurveKey<Quaternion>
        {
            Contract.Require(dirpath, nameof(dirpath));
            Contract.Require(basename, nameof(basename));
            Contract.Require(curve, nameof(curve));
            Contract.EnsureRange(sampleCount > 0, nameof(sampleCount));

            var keyFilePath = Path.Combine(dirpath, $"{basename}_keys.csv");
            using (var keyFileStream = File.OpenWrite(keyFilePath))
            {
                WriteKeysToCsv(keyFileStream, curve);
            }

            var sampleFilePath = Path.Combine(dirpath, $"{basename}_samples.csv");
            using (var sampleFileStream = File.OpenWrite(sampleFilePath))
            {
                WriteSamplesToCsvUsingSharpGltf(sampleFileStream, curve, sampleCount);
            }
        }

        /// <summary>
        /// Writes the specified curve's keys to a CSV file.
        /// </summary>
        /// <param name="output">The stream to which to write the CSV data.</param>
        /// <param name="curve">The curve for which to write keys.</param>
        public static void WriteKeysToCsv<TValue, TKey>(Stream output, Curve<TValue, TKey> curve)
            where TKey : CurveKey<TValue>
        {
            Contract.Require(output, nameof(output));
            Contract.Require(curve, nameof(curve));

            using (var writer = new StreamWriter(output))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteField("Key");
                csv.WriteField("Position");
                WriteCsvFieldNames(csv, "Value_", typeof(TValue));

                if (typeof(TKey) == typeof(CubicSplineCurveKey<TValue>))
                {
                    WriteCsvFieldNames(csv, "TangentIn_", typeof(TValue));
                    WriteCsvFieldNames(csv, "TangentOut_", typeof(TValue));
                }

                csv.NextRecord();

                var keys = curve.Keys.Select(x => x.Key).ToList();
                for (var i = 0; i < keys.Count; i++)
                {
                    var key = keys[i];

                    csv.WriteField(i);
                    csv.WriteField(key.Position);
                    WriteCsvFields(csv, key.Value);

                    if (key is CubicSplineCurveKey<TValue> cubicSplineKey)
                    {
                        WriteCsvFields(csv, cubicSplineKey.TangentIn);
                        WriteCsvFields(csv, cubicSplineKey.TangentOut);
                    }

                    csv.NextRecord();
                }
            }
        }

        /// <summary>
        /// Writes samples from the specified curve to a CSV file.
        /// </summary>
        /// <param name="output">The stream to which to write the CSV data.</param>
        /// <param name="curve">The curve for which to write samples.</param>
        /// <param name="sampleCount">The number of samples to write.</param>
        public static void WriteSamplesToCsv<TValue>(Stream output, Curve<TValue> curve, Int32 sampleCount)
        {
            Contract.Require(output, nameof(output));
            Contract.Require(curve, nameof(curve));
            Contract.EnsureRange(sampleCount > 0, nameof(sampleCount));

            using (var writer = new StreamWriter(output))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                WriteCsvHeader<TValue>(csv);

                var delta = curve.Length / sampleCount;
                var position = curve.StartPosition;
                for (var i = 0; i < sampleCount; i++)
                {
                    var value = curve.Evaluate(position, default);
                    WriteCsvRecord(csv, position, value);

                    position += delta;
                }
            }
        }

        /// <summary>
        /// Writes samples from the specified curve to a CSV file using SharpGLTF to perform sampling.
        /// </summary>
        /// <param name="output">The stream to which to write the CSV data.</param>
        /// <param name="curve">The curve for which to write samples.</param>
        /// <param name="sampleCount">The number of samples to write.</param>
        public static void WriteSamplesToCsvUsingSharpGltf<TKey>(Stream output, Curve<Single, TKey> curve, Int32 sampleCount)
            where TKey : CurveKey<Single>
        {
            Contract.Require(output, nameof(output));
            Contract.Require(curve, nameof(curve));
            Contract.EnsureRange(sampleCount > 0, nameof(sampleCount));

            var sharpGltfSampler = CreateSharpGltfCurveSampler(curve);

            using (var writer = new StreamWriter(output))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                WriteCsvHeader<Single>(csv);

                var delta = curve.Length / sampleCount;
                var position = curve.StartPosition;
                for (var i = 0; i < sampleCount; i++)
                {
                    var value = sharpGltfSampler.GetPoint(position)[0];
                    WriteCsvRecord(csv, position, value);

                    position += delta;
                }
            }
        }

        /// <summary>
        /// Writes samples from the specified curve to a CSV file using SharpGLTF to perform sampling.
        /// </summary>
        /// <param name="output">The stream to which to write the CSV data.</param>
        /// <param name="curve">The curve for which to write samples.</param>
        /// <param name="sampleCount">The number of samples to write.</param>
        public static void WriteSamplesToCsvUsingSharpGltf<TKey>(Stream output, Curve<Vector3, TKey> curve, Int32 sampleCount)
            where TKey : CurveKey<Vector3>
        {
            Contract.Require(output, nameof(output));
            Contract.Require(curve, nameof(curve));
            Contract.EnsureRange(sampleCount > 0, nameof(sampleCount));

            var sharpGltfSampler = CreateSharpGltfCurveSampler(curve);

            using (var writer = new StreamWriter(output))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                WriteCsvHeader<Vector3>(csv);

                var delta = curve.Length / sampleCount;
                var position = curve.StartPosition;
                for (var i = 0; i < sampleCount; i++)
                {
                    var value = sharpGltfSampler.GetPoint(position);
                    WriteCsvRecord(csv, position, value);

                    position += delta;
                }
            }
        }
        
        /// <summary>
        /// Writes samples from the specified curve to a CSV file using SharpGLTF to perform sampling.
        /// </summary>
        /// <param name="output">The stream to which to write the CSV data.</param>
        /// <param name="curve">The curve for which to write samples.</param>
        /// <param name="sampleCount">The number of samples to write.</param>
        public static void WriteSamplesToCsvUsingSharpGltf<TKey>(Stream output, Curve<Quaternion, TKey> curve, Int32 sampleCount)
            where TKey : CurveKey<Quaternion>
        {
            Contract.Require(output, nameof(output));
            Contract.Require(curve, nameof(curve));
            Contract.EnsureRange(sampleCount > 0, nameof(sampleCount));

            var sharpGltfSampler = CreateSharpGltfCurveSampler(curve);

            using (var writer = new StreamWriter(output))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                WriteCsvHeader<Quaternion>(csv);

                var delta = curve.Length / sampleCount;
                var position = curve.StartPosition;
                for (var i = 0; i < sampleCount; i++)
                {
                    var value = sharpGltfSampler.GetPoint(position);
                    WriteCsvRecord(csv, position, value);

                    position += delta;
                }
            }
        }

        /// <summary>
        /// Writes the CSV header for the specified type.
        /// </summary>
        private static void WriteCsvHeader<TValue>(CsvWriter csv)
        {
            csv.WriteField("Position");

            WriteCsvFieldNames(csv, null, typeof(TValue));

            csv.NextRecord();
        }

        /// <summary>
        /// Writes the CSV record for the specified value.
        /// </summary>
        private static void WriteCsvRecord<TValue>(CsvWriter csv, Single position, TValue record)
        {
            csv.WriteField(position);

            WriteCsvFields(csv, record);

            csv.NextRecord();
        }

        /// <summary>
        /// Writes the CSV field names for the specified type.
        /// </summary>
        private static void WriteCsvFieldNames(CsvWriter csv, String prefix, Type type)
        {
            if (type.IsPrimitive)
            {
                csv.WriteField($"{prefix}Value");
            }
            else
            {
                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    csv.WriteField($"{prefix}{field.Name}");
                }
            }
        }

        /// <summary>
        /// Writes the CSV fields for the specified value.
        /// </summary>
        private static void WriteCsvFields<TValue>(CsvWriter csv, TValue value)
        {
            if (typeof(TValue).IsPrimitive)
            {
                csv.WriteField(value);
            }
            else
            {
                var fields = typeof(TValue).GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    csv.WriteField(field.GetValue(value));
                }
            }
        }

        /// <summary>
        /// Creates a SharpGLTF curve sampler for the specified Ultraviolet curve.
        /// </summary>
        private static SharpGLTF.Animations.ICurveSampler<Single[]> CreateSharpGltfCurveSampler<TKey>(Curve<Single, TKey> curve)
            where TKey : CurveKey<Single>
        {
            if (typeof(TKey) == typeof(CubicSplineCurveKey<Single>))
            {
                var gltfKeys = curve.Keys.Select(x => x.Key).Cast<CubicSplineCurveKey<Single>>()
                    .Select(x => (x.Position, (new[] { x.TangentIn }, new[] { x.Value }, new[] { x.TangentOut }))).ToList();

                return gltfKeys.CreateSampler();
            }
            else
            {
                var gltfKeys = curve.Keys
                    .Select(x => (x.Key.Position, (new[] { x.Key.Value }))).ToList();

                var isLinear = curve.Sampler == SingleCurveLinearSampler.Instance;
                return gltfKeys.CreateSampler(isLinear: isLinear);
            }
        }

        /// <summary>
        /// Creates a SharpGLTF curve sampler for the specified Ultraviolet curve.
        /// </summary>
        private static SharpGLTF.Animations.ICurveSampler<System.Numerics.Vector3> CreateSharpGltfCurveSampler<TKey>(Curve<Vector3, TKey> curve)
            where TKey : CurveKey<Vector3>
        {
            if (typeof(TKey) == typeof(CubicSplineCurveKey<Vector3>))
            {
                var gltfKeys = curve.Keys.Select(x => x.Key).Cast<CubicSplineCurveKey<Vector3>>()
                    .Select(x => (x.Position, ((System.Numerics.Vector3)x.TangentIn, (System.Numerics.Vector3)x.Value, (System.Numerics.Vector3)x.TangentOut))).ToList();

                return gltfKeys.CreateSampler();
            }
            else
            {
                var gltfKeys = curve.Keys
                    .Select(x => (x.Key.Position, (System.Numerics.Vector3)x.Key.Value)).ToList();

                var isLinear = curve.Sampler == Vector3CurveLinearSampler.Instance;
                return gltfKeys.CreateSampler(isLinear: isLinear);
            }
        }

        /// <summary>
        /// Creates a SharpGLTF curve sampler for the specified Ultraviolet curve.
        /// </summary>
        private static SharpGLTF.Animations.ICurveSampler<System.Numerics.Quaternion> CreateSharpGltfCurveSampler<TKey>(Curve<Quaternion, TKey> curve)
            where TKey : CurveKey<Quaternion>
        {
            if (typeof(TKey) == typeof(CubicSplineCurveKey<Quaternion>))
            {
                var gltfKeys = curve.Keys.Select(x => x.Key).Cast<CubicSplineCurveKey<Quaternion>>()
                    .Select(x => (x.Position, ((System.Numerics.Quaternion)x.TangentIn, (System.Numerics.Quaternion)x.Value, (System.Numerics.Quaternion)x.TangentOut))).ToList();

                return gltfKeys.CreateSampler();
            }
            else
            {
                var gltfKeys = curve.Keys
                    .Select(x => (x.Key.Position, (System.Numerics.Quaternion)x.Key.Value)).ToList();

                var isLinear = curve.Sampler == QuaternionCurveLinearSampler.Instance;
                return gltfKeys.CreateSampler(isLinear: isLinear);
            }
        }
    }
}
