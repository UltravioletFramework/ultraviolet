using System;
using Ultraviolet.Core;

namespace Ultraviolet.Graphics
{
    /// <summary>
    /// Represents a region of the screen in which rendering takes place.
    /// </summary>
    [Serializable]
    public partial struct Viewport : IEquatable<Viewport>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> structure.
        /// </summary>
        /// <param name="x">The x-coordinate of the upper-left corner of the viewport on the render target surface.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the viewport on the render target surface.</param>
        /// <param name="width">The width of the viewport in pixels.</param>
        /// <param name="height">The height of the viewport in pixels.</param>
        public Viewport(Int32 x, Int32 y, Int32 width, Int32 height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.MinDepth = 0f;
            this.MaxDepth = 1f;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Viewport"/> structure.
        /// </summary>
        /// <param name="bounds">A rectangle which describes the viewport's boundary.</param>
        public Viewport(Rectangle bounds)
        {
            this.X = bounds.X;
            this.Y = bounds.Y;
            this.Width = bounds.Width;
            this.Height = bounds.Height;
            this.MinDepth = 0f;
            this.MaxDepth = 1f;
        }

        /// <inheritdoc/>
        public override String ToString() => $"{{X:{X} Y:{Y} Width:{Width} Height:{Height}}}";
        
        /// <summary>
        /// Projects a <see cref="Vector3"/> from world space into screen space.
        /// </summary>
        /// <param name="source">The vector to project.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="world">The world matrix.</param>
        /// <returns>The projected vector.</returns>
        public Vector3 Project(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            Project(ref source, ref projection, ref view, ref world, out var result);
            return result;
        }

        /// <summary>
        /// Projects a <see cref="Vector3"/> from world space into screen space.
        /// </summary>
        /// <param name="source">The vector to project.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="world">The world matrix.</param>
        /// <param name="result">The projected vector.</param>
        public void Project(ref Vector3 source, ref Matrix projection, ref Matrix view, ref Matrix world, out Vector3 result)
        {
            Matrix.Multiply(ref world, ref view, out var matrix);
            Matrix.Multiply(ref matrix, ref projection, out matrix);

            Vector3.Transform(ref source, ref matrix, out result);

            var a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
            if (!MathUtil.AreApproximatelyEqual(a, 1.0f))
            {
                result.X /= a;
                result.Y /= a;
                result.Z /= a;
            }

            result.X = (((result.X + 1f) * 0.5f) * Width) + X;
            result.Y = (((-result.Y + 1f) * 0.5f) * Height) + Y;
            result.Z = (result.Z * (MaxDepth - MinDepth)) + MinDepth;
        }

        /// <summary>
        /// Unprojects a <see cref="Vector3"/> from screen space into world space.
        /// </summary>
        /// <param name="source">The vector to project.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="world">The world matrix.</param>
        /// <returns>The projected vector.</returns>
        public Vector3 Unproject(Vector3 source, Matrix projection, Matrix view, Matrix world)
        {
            Unproject(ref source, ref projection, ref view, ref world, out var result);
            return result;
        }

        /// <summary>
        /// Unprojects a <see cref="Vector3"/> from screen space into world space.
        /// </summary>
        /// <param name="source">The vector to project.</param>
        /// <param name="projection">The projection matrix.</param>
        /// <param name="view">The view matrix.</param>
        /// <param name="world">The world matrix.</param>
        /// <param name="result">The projected vector.</param>
        public void Unproject(ref Vector3 source, ref Matrix projection, ref Matrix view, ref Matrix world, out Vector3 result)
        {
            Matrix.Multiply(ref world, ref view, out var matrix);
            Matrix.Multiply(ref matrix, ref projection, out matrix);
            Matrix.Invert(ref matrix, out matrix);

            source.X = (((source.X - X) / Width) * 2f) - 1f;
            source.Y = -((((source.Y - Y) / Height) * 2f) - 1f);
            source.Z = (source.Z - MinDepth) / (MaxDepth - MinDepth);

            Vector3.Transform(ref source, ref matrix, out var vector);

            var a = (((source.X * matrix.M14) + (source.Y * matrix.M24)) + (source.Z * matrix.M34)) + matrix.M44;
            if (!MathUtil.AreApproximatelyEqual(a, 1.0f))
            {
                vector.X /= a;
                vector.Y /= a;
                vector.Z /= a;
            }

            result = vector;
        }

        /// <summary>
        /// Gets or sets the viewport's boundary.
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(X, Y, Width, Height);
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Gets the viewport's aspect ratio.
        /// </summary>
        public Single AspectRatio
        {
            get
            {
                if (Height != 0 && Width != 0)
                {
                    return Width / (Single)Height;
                }
                return 0f;
            }
        }

        /// <summary>
        /// The x-coordinate of the upper-left corner of the viewport on the render target surface.
        /// </summary>
        public Int32 X;

        /// <summary>
        /// The y-coordinate of the upper-left corner of the viewport on the render target surface.
        /// </summary>
        public Int32 Y;

        /// <summary>
        /// The width of the viewport in pixels.
        /// </summary>
        public Int32 Width;

        /// <summary>
        /// The height of the viewport in pixels.
        /// </summary>
        public Int32 Height;

        /// <summary>
        /// The minimum depth of the clip volume.
        /// </summary>
        public Single MinDepth;

        /// <summary>
        /// The maximum depth of the clip volume.
        /// </summary>
        public Single MaxDepth;        
    }
}
