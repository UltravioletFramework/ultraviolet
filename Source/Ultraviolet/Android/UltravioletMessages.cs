namespace Ultraviolet
{
    static partial class UltravioletMessages
    {
        /// <summary>
        /// An event which is raised when OnCreate() is called for the main Activity.
        /// </summary>
        public static readonly UltravioletMessageID AndroidActivityCreate = UltravioletMessageID.Acquire(nameof(AndroidActivityCreate));

        /// <summary>
        /// An event which is raised when OnStart() is called for the main Activity.
        /// </summary>
        public static readonly UltravioletMessageID AndroidActivityStart = UltravioletMessageID.Acquire(nameof(AndroidActivityStart));

        /// <summary>
        /// An event which is raised when OnResume() is called for the main Activity.
        /// </summary>
        public static readonly UltravioletMessageID AndroidActivityResume = UltravioletMessageID.Acquire(nameof(AndroidActivityResume));

        /// <summary>
        /// An event which is raised when OnPause() is called for the main Activity.
        /// </summary>
        public static readonly UltravioletMessageID AndroidActivityPause = UltravioletMessageID.Acquire(nameof(AndroidActivityPause));

        /// <summary>
        /// An event which is raised when OnStop() is called for the main Activity.
        /// </summary>
        public static readonly UltravioletMessageID AndroidActivityStop = UltravioletMessageID.Acquire(nameof(AndroidActivityStop));

        /// <summary>
        /// An event which is raised when OnDestroy() is called for the main Activity.
        /// </summary>
        public static readonly UltravioletMessageID AndroidActivityDestroy = UltravioletMessageID.Acquire(nameof(AndroidActivityDestroy));

        /// <summary>
        /// An event which is raised when OnRestart() is called for the main Activity.
        /// </summary>
        public static readonly UltravioletMessageID AndroidActivityRestart = UltravioletMessageID.Acquire(nameof(AndroidActivityRestart));
    }
}