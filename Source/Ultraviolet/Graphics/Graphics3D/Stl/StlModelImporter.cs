using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ultraviolet.Content;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Imports .stl model files.
    /// </summary>
    [ContentImporter(".stl")]
    public sealed class StlModelImporter : ContentImporter<StlModelDescription>
    {
        /// <summary>
        /// An array of file extensions supported by this importer 
        /// </summary>
        public static String[] SupportedExtensions { get; } = new string[] { ".stl" };

        /// <inheritdoc/>
        public override StlModelDescription Import(IContentImporterMetadata metadata, Stream stream)
        {
            var header = new Byte[5];
            stream.Read(header, 0, header.Length);
            if (header[0] == (Byte)'s' &&
                header[1] == (Byte)'o' &&
                header[2] == (Byte)'l' &&
                header[3] == (Byte)'i' &&
                header[4] == (Byte)'d')
            {
                return ImportAscii(metadata, stream);
            }
            else
            {
                return ImportBinary(metadata, stream);
            }
        }

        /// <summary>
        /// Imports an ASCII-format STL file.
        /// </summary>
        private StlModelDescription ImportAscii(IContentImporterMetadata metadata, Stream stream)
        {
            using (var reader = new StreamReader(stream, Encoding.ASCII, false, 1024, true))
            {
                // Read the model's name.
                var name = reader.ReadLine().Trim();

                // Read the model's facets.
                var triangles = new List<StlModelTriangleDescription>();
                while (!reader.EndOfStream)
                {
                    String Advance(StreamReader r) => r.ReadLine().Trim();

                    var line = Advance(reader);
                    if (Accept(ref line, "endsolid"))
                    {
                        var endsolidName = line.Trim();
                        if (!endsolidName.Equals(name, StringComparison.Ordinal))
                            throw new InvalidDataException(UltravioletStrings.MalformedContentFile);

                        break;
                    }

                    ExpectCoordinates(ref line, "facet normal", out var normal);

                    line = Advance(reader);
                    Expect(ref line, "outer loop");

                    line = Advance(reader);
                    ExpectCoordinates(ref line, "vertex", out var v1);

                    line = Advance(reader);
                    ExpectCoordinates(ref line, "vertex", out var v2);

                    line = Advance(reader);
                    ExpectCoordinates(ref line, "vertex", out var v3);

                    line = Advance(reader);
                    Expect(ref line, "endloop");

                    line = Advance(reader);
                    Expect(ref line, "endfacet");

                    triangles.Add(new StlModelTriangleDescription(normal, v1, v2, v3));
                }

                return new StlModelDescription { Name = name, Triangles = triangles };
            }
        }

        /// <summary>
        /// Optionally accepts the specified text.
        /// </summary>
        private Boolean Accept(ref String line, String text)
        {
            if (line.StartsWith(text, StringComparison.Ordinal))
            {
                line = line.Substring(text.Length);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Expects the specified line of text, throwing an exception if it is not found.
        /// </summary>
        private void Expect(ref String line, String text)
        {
            if (!line.Equals(text, StringComparison.Ordinal))
                throw new InvalidDataException(UltravioletStrings.MalformedContentFile);

            line = String.Empty;
        }

        /// <summary>
        /// Expects a set of coordinates with the specified prefix, throwing an exception if it is not found.
        /// </summary>
        private void ExpectCoordinates(ref String line, String prefix, out Vector3 coordinates)
        {
            if (!line.StartsWith(prefix, StringComparison.Ordinal))
                throw new InvalidDataException(UltravioletStrings.MalformedContentFile);

            line = line.Substring(prefix.Length).Trim();

            var parts = line.ToString().Split(' ');
            if (parts.Length != 3)
                throw new InvalidDataException(UltravioletStrings.MalformedContentFile);

            if (!Single.TryParse(parts[0], out var c1) ||
                !Single.TryParse(parts[1], out var c2) ||
                !Single.TryParse(parts[2], out var c3))
            {
                throw new InvalidDataException(UltravioletStrings.MalformedContentFile);
            }

            coordinates = new Vector3(c1, c2, c3);
        }

        /// <summary>
        /// Imports a binary-format STL file.
        /// </summary>
        private StlModelDescription ImportBinary(IContentImporterMetadata metadata, Stream stream)
        {
            // Skip the rest of the header.
            stream.Seek(75, SeekOrigin.Current);

            // Read triangles.
            using (var reader = new BinaryReader(stream, Encoding.ASCII, true))
            {
                var triangleCount = reader.ReadUInt32();
                var triangles = new StlModelTriangleDescription[triangleCount];

                for (var i = 0u; i < triangleCount; i++)
                {
                    var nx = reader.ReadSingle();
                    var ny = reader.ReadSingle();
                    var nz = reader.ReadSingle();
                    var normal = Vector3.Normalize(new Vector3(nx, ny, nz));

                    var v1x = reader.ReadSingle();
                    var v1y = reader.ReadSingle();
                    var v1z = reader.ReadSingle();
                    var v1 = new Vector3(v1x, v1y, v1z);

                    var v2x = reader.ReadSingle();
                    var v2y = reader.ReadSingle();
                    var v2z = reader.ReadSingle();
                    var v2 = new Vector3(v2x, v2y, v2z);

                    var v3x = reader.ReadSingle();
                    var v3y = reader.ReadSingle();
                    var v3z = reader.ReadSingle();
                    var v3 = new Vector3(v3x, v3y, v3z);

                    var attributeByteCount = reader.ReadUInt16();
                    if (attributeByteCount > 0)
                        throw new NotSupportedException(UltravioletStrings.MalformedContentFile);

                    triangles[i] = new StlModelTriangleDescription(normal, v1, v2, v3);
                }

                return new StlModelDescription { Triangles = triangles };
            }
        }
    }
}
