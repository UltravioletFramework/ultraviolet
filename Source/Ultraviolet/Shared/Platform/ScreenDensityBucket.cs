
namespace Ultraviolet.Platform
{
    /// <summary>
    /// Represents the buckets into which Ultraviolet classifies the pixel density of a display device.
    /// </summary>
    public enum ScreenDensityBucket
    {
        /// <summary>
        /// Standard desktop DPI (72 dpi).
        /// </summary>
        Desktop,

        /// <summary>
        /// Low mobile DPI (~120 dpi).
        /// </summary>
        Low,

        /// <summary>
        /// macOS Retina DPI (~144 dpi).
        /// </summary>
        Retina,

        /// <summary>
        /// Medium mobile DPI (~160 dpi).
        /// </summary>
        Medium,

        /// <summary>
        /// macOS Retina HD DPI (~216 DPI).
        /// </summary>
        RetinaHD,

        /// <summary>
        /// High mobile DPI (~240 dpi).
        /// </summary>
        High,

        /// <summary>
        /// Extra high mobile DPI (~320 dpi).
        /// </summary>
        ExtraHigh,

        /// <summary>
        /// Extra extra high mobile DPI (~480 dpi).
        /// </summary>
        ExtraExtraHigh,

        /// <summary>
        /// Extra extra extra high mobile DPI (~640 dpi).
        /// </summary>
        ExtraExtraExtraHigh,
    }
}
