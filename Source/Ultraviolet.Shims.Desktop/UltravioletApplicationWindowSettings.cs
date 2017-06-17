using System;
using System.Reflection;
using System.Xml.Linq;
using Ultraviolet.Core;
using Ultraviolet.Core.Xml;
using Ultraviolet.Platform;

namespace Ultraviolet
{
    /// <summary>
    /// Represents an Ultraviolet application's internal window settings.
    /// </summary>
    internal class UltravioletApplicationWindowSettings
    {
        /// <summary>
        /// Saves the specified window settings to XML.
        /// </summary>
        /// <param name="settings">The window settings to save.</param>
        /// <returns>An XML element that represents the specified window settings.</returns>
        public static XElement Save(UltravioletApplicationWindowSettings settings)
        {
            Contract.Require(settings, nameof(settings));

            var pos = settings.WindowedPosition;

            return new XElement("Window",
                new XElement("WindowState", settings.WindowState),
                new XElement("WindowMode", settings.WindowMode),
                new XElement("WindowScale", settings.WindowScale),
                new XElement("WindowedPosition", $"{pos.X} {pos.Y} {pos.Width} {pos.Height}"),
                new XElement("GrabsMouseWhenWindowed", settings.GrabsMouseWhenWindowed),
                new XElement("GrabsMouseWhenFullscreenWindowed", settings.GrabsMouseWhenFullscreenWindowed),
                new XElement("GrabsMouseWhenFullscreen", settings.GrabsMouseWhenFullscreen),
                new XElement("SynchronizeWithVerticalRetrace", settings.SynchronizeWithVerticalRetrace),
                settings.FullscreenDisplayMode == null ? null : new XElement("FullscreenDisplayMode",
                    new XElement("Width", settings.FullscreenDisplayMode.Width),
                    new XElement("Height", settings.FullscreenDisplayMode.Height),
                    new XElement("BitsPerPixel", settings.FullscreenDisplayMode.BitsPerPixel),
                    new XElement("RefreshRate", settings.FullscreenDisplayMode.RefreshRate),
                    new XElement("DisplayIndex", settings.FullscreenDisplayMode?.DisplayIndex)
                )
            );
        }

        /// <summary>
        /// Loads window settings from the specified XML element.
        /// </summary>
        /// <param name="xml">The XML element that contains the window settings to load.</param>
        /// <returns>The window settings that were loaded from the specified XML element or <see langword="null"/> if 
        /// settings could not be loaded correctly.</returns>
        public static UltravioletApplicationWindowSettings Load(XElement xml)
        {
            if (xml == null)
                return null;

            try
            {
                var settings = new UltravioletApplicationWindowSettings();

                settings.WindowState = xml.ElementValue<WindowState>("WindowState");
                settings.WindowMode = xml.ElementValue<WindowMode>("WindowMode");
                settings.WindowScale = (Single?)xml.Element("WindowScale") ?? 1f;
                settings.WindowedPosition = xml.ElementValue<Rectangle>("WindowedPosition");
                settings.GrabsMouseWhenWindowed = xml.ElementValue<Boolean>("GrabsMouseWhenWindowed");
                settings.GrabsMouseWhenFullscreenWindowed = xml.ElementValue<Boolean>("GrabsMouseWhenFullscreenWindowed");
                settings.GrabsMouseWhenFullscreen = xml.ElementValue<Boolean>("GrabsMouseWhenFullscreen");
                settings.SynchronizeWithVerticalRetrace = xml.ElementValue<Boolean>("SynchronizeWithVerticalRetrace");

                var fullscreenDisplayMode = xml.Element("FullscreenDisplayMode");
                if (fullscreenDisplayMode != null)
                {
                    var width = fullscreenDisplayMode.ElementValue<Int32>("Width");
                    var height = fullscreenDisplayMode.ElementValue<Int32>("Height");
                    var bitsPerPixel = fullscreenDisplayMode.ElementValue<Int32>("BitsPerPixel");
                    var refreshRate = fullscreenDisplayMode.ElementValue<Int32>("RefreshRate");
                    var displayIndex = fullscreenDisplayMode.ElementValue<Int32>("DisplayIndex");

                    settings.FullscreenDisplayMode = new DisplayMode(width, height, bitsPerPixel, refreshRate, displayIndex);
                }

                return settings;
            }
            catch (FormatException)
            {
                return null;
            }
            catch (TargetInvocationException e)
            {
                if (e.InnerException is FormatException)
                {
                    return null;
                }
                throw;
            }
        }

        /// <summary>
        /// Creates a set of window settings from the current application state.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        /// <returns>The window settings which were retrieved.</returns>
        public static UltravioletApplicationWindowSettings FromCurrentSettings(UltravioletContext uv)
        {
            Contract.Require(uv, nameof(uv));

            var primary = uv.GetPlatform().Windows.GetPrimary();
            if (primary == null)
                return null;

            var settings = new UltravioletApplicationWindowSettings();

            settings.WindowState = primary.GetWindowState();
            settings.WindowMode = primary.GetWindowMode();
            settings.WindowScale = primary.WindowScale;
            settings.WindowedPosition = new Rectangle(primary.WindowedPosition, primary.WindowedClientSize);
            settings.FullscreenDisplayMode = primary.GetFullscreenDisplayMode();
            settings.GrabsMouseWhenWindowed = primary.GrabsMouseWhenWindowed;
            settings.GrabsMouseWhenFullscreenWindowed = primary.GrabsMouseWhenFullscreenWindowed;
            settings.GrabsMouseWhenFullscreen = primary.GrabsMouseWhenFullscreen;
            settings.SynchronizeWithVerticalRetrace = primary.SynchronizeWithVerticalRetrace;

            return settings;
        }

        /// <summary>
        /// Applies the specified settings.
        /// </summary>
        /// <param name="uv">The Ultraviolet context.</param>
        public void Apply(UltravioletContext uv)
        {
            var primary = uv.GetPlatform().Windows.GetPrimary();
            if (primary == null)
                return;

            primary.SetWindowState(WindowState);
            primary.SetWindowMode(WindowMode);
            primary.SetWindowBounds(WindowedPosition, WindowScale);
            primary.GrabsMouseWhenWindowed = GrabsMouseWhenWindowed;
            primary.GrabsMouseWhenFullscreenWindowed = GrabsMouseWhenFullscreenWindowed;
            primary.GrabsMouseWhenFullscreen = GrabsMouseWhenFullscreen;
            primary.SynchronizeWithVerticalRetrace = SynchronizeWithVerticalRetrace;

            if (FullscreenDisplayMode != null)
            {
                primary.SetFullscreenDisplayMode(FullscreenDisplayMode);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the primary window grabs the mouse when it enters windowed mode.
        /// </summary>
        public Boolean GrabsMouseWhenWindowed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the primary window grabs the mouse when it enters fullscreen windowed mode.
        /// </summary>
        public Boolean GrabsMouseWhenFullscreenWindowed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the primary window grabs the mouse when it enters fullscreen mode.
        /// </summary>
        public Boolean GrabsMouseWhenFullscreen
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the primary window is synchronized with the vertical retrace.
        /// </summary>
        public Boolean SynchronizeWithVerticalRetrace
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the primary window's window state.
        /// </summary>
        public WindowState WindowState
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the primary window's window mode.
        /// </summary>
        public WindowMode WindowMode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the scaling factor which is applied to the window.
        /// </summary>
        public Single WindowScale
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the primary window's position and client size.
        /// </summary>
        public Rectangle WindowedPosition
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the primary window's fullscreen display mode.
        /// </summary>
        public DisplayMode FullscreenDisplayMode
        {
            get;
            private set;
        }
    }
}