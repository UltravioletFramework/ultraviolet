using System;
using Ultraviolet.Platform;

namespace Ultraviolet.Graphics.Graphics3D
{
    /// <summary>
    /// Represents a camera with perspective.
    /// </summary>
    public sealed class PerspectiveCamera : Camera
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PerspectiveCamera"/> class.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        private PerspectiveCamera(UltravioletContext uv)
            : base(uv)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="PerspectiveCamera"/> class.
        /// </summary>
        /// <returns>The instance of <see cref="PerspectiveCamera"/> that was created.</returns>
        public static PerspectiveCamera Create() => new PerspectiveCamera(UltravioletContext.DemandCurrent());

        /// <inheritdoc/>
        public override void Update(IUltravioletWindow window = null)
        {
            var win = window ?? Ultraviolet.GetPlatform().Windows.GetCurrent() ?? Ultraviolet.GetPlatform().Windows.GetPrimary();
            var aspectRatio = win.DrawableSize.Width / (Single)win.DrawableSize.Height;

            view = Matrix.CreateLookAt(Position, Target, Up);
            proj = Matrix.CreatePerspectiveFieldOfView(FieldOfView, aspectRatio, NearPlaneDistance, FarPlaneDistance);
            Matrix.Multiply(ref view, ref proj, out viewproj);
        }

        /// <inheritdoc/>
        public override void GetViewMatrix(out Matrix matrix) => matrix = view;

        /// <inheritdoc/>
        public override void GetProjectionMatrix(out Matrix matrix) => matrix = proj;

        /// <inheritdoc/>
        public override void GetViewProjectionMatrix(out Matrix matrix) => matrix = viewproj;

        /// <summary>
        /// Gets the camera's field of view in radians.
        /// </summary>
        public Single FieldOfView { get; } = (Single)Math.PI / 4f;

        /// <summary>
        /// Gets or sets the distance to the near plane.
        /// </summary>
        public Single NearPlaneDistance { get; set; } = 1f;

        /// <summary>
        /// Gets or sets the distance to the far plane.
        /// </summary>
        public Single FarPlaneDistance { get; set; } = 1000f;

        /// <summary>
        /// Gets or sets the camera's position in 3D space.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the point in 3D space at which the camera is targeted.
        /// </summary>
        public Vector3 Target { get; set; }

        /// <summary>
        /// Gets or sets the vector that denotes which direction is "up" for this camera.
        /// </summary>
        public Vector3 Up { get; set; } = Vector3.Up;

        // Calculated matrices.
        private Matrix view;
        private Matrix proj;
        private Matrix viewproj;
    }
}