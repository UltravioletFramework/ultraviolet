using System;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Graphics.Graphics2D;
using TwistedLogik.Ultraviolet.Platform;

namespace TwistedLogik.Ultraviolet.UI
{
    /// <summary>
    /// Represents the method that is called when a UI layout is loaded.
    /// </summary>
    /// <param name="layout">The <see cref="IUILayout"/> that raised the event.</param>
    public delegate void UILayoutEventHandler(IUILayout layout);

    /// <summary>
    /// Represents the method that is called when a layout prints a debug message.
    /// </summary>
    /// <param name="layout">The <see cref="IUILayout"/> that raised the event.</param>
    /// <param name="message">The message that was printed.</param>
    /// <param name="source">The name of the script that caused the message, if available.</param>
    /// <param name="line">The line of code that caused the message, if available.</param>
    public delegate void UILayoutMessageEventHandler(IUILayout layout, String message, String source, Int32 line);

    /// <summary>
    /// Represents a user interface layout.
    /// </summary>
    public interface IUILayout : IDisposable
    {
        /// <summary>
        /// Loads the specified layout.
        /// </summary>
        /// <param name="content">A <see cref="ContentManager"/> with which to load layout resources.</param>
        /// <param name="layout">The asset path of the layout to load.</param>
        void LoadLayout(ContentManager content, String layout);

        /// <summary>
        /// Loads the specified layout.
        /// </summary>
        /// <param name="content">A <see cref="ContentManager"/> with which to load layout resources.</param>
        /// <param name="definition">A <see cref="UIPanelDefinition"/> containing the layout information to load.</param>
        void LoadLayout(ContentManager content, UIPanelDefinition definition);

        /// <summary>
        /// Updates the layout's state.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Update()"/>.</param>
        void Update(UltravioletTime time);

        /// <summary>
        /// Draw the layout.
        /// </summary>
        /// <param name="time">Time elapsed since the last call to <see cref="UltravioletContext.Draw()"/>.</param>
        /// <param name="spriteBatch">The <see cref="SpriteBatch"/> with which to draw the layout.</param>
        /// <param name="color">The <see cref="Color"/> with which to tint the layout.</param>
        void Draw(UltravioletTime time, SpriteBatch spriteBatch, Color color);

        /// <summary>
        /// Grants input focus to the layout.
        /// </summary>
        void Focus();

        /// <summary>
        /// Removes input focus from the layout.
        /// </summary>
        void Blur();

        /// <summary>
        /// Registers a static API method.
        /// </summary>
        /// <typeparam name="T">The type that defines the method to register.</typeparam>
        /// <param name="method">The name of the method to register.</param>
        void RegisterApiMethod<T>(String method);

        /// <summary>
        /// Registers a static API method.
        /// </summary>
        /// <typeparam name="T">The type that defines the method to register.</typeparam>
        /// <param name="method">The name of the method to register.</param>
        /// <param name="name">The name under which the method can be invoked by a layout script.</param>
        void RegisterApiMethod<T>(String method, String name);

        /// <summary>
        /// Registers an API method.
        /// </summary>
        /// <typeparam name="T">The type that defines the method to register.</typeparam>
        /// <param name="method">The name of the method to register.</param>
        /// <param name="target">The object on which the method will be invoked.</param>
        void RegisterApiMethod<T>(String method, T target) where T : class;

        /// <summary>
        /// Registers an API method.
        /// </summary>
        /// <typeparam name="T">The type that defines the method to register.</typeparam>
        /// <param name="method">The name of the method to register.</param>
        /// <param name="name">The name under which the method can be invoked by a layout script.</param>
        /// <param name="target">The object on which the method will be invoked.</param>
        void RegisterApiMethod<T>(String method, String name, T target) where T : class;

        /// <summary>
        /// Gets a delegate that represents the specified layout script.
        /// </summary>
        /// <typeparam name="T">The type of delegate to create for the specified script.</typeparam>
        /// <param name="script">The name of the script for which to retrieve a delegate.</param>
        /// <returns>A delegate that represents the specified layout script.</returns>
        T GetScriptDelegate<T>(String script);

        /// <summary>
        /// Gets a <see cref="UILayoutState"/> value that represents the layout's current state.
        /// </summary>
        UILayoutState State { get; }

        /// <summary>
        /// Gets or sets the layout's position.
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the x-coordinate of the layout's position.
        /// </summary>
        Int32 X { get; set; }

        /// <summary>
        /// Gets or sets the y-coordinate of the layout's position.
        /// </summary>
        Int32 Y { get; set; }

        /// <summary>
        /// Gets or sets the layout's size.
        /// </summary>
        Size2 Size { get; set; }

        /// <summary>
        /// Gets or sets the layout's width in pixels.
        /// </summary>
        Int32 Width { get; set; }

        /// <summary>
        /// Gets or sets the layout's height in pixels.
        /// </summary>
        Int32 Height { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the layout is interactive.  Non-interactive
        /// layouts will not receive input events.
        /// </summary>
        Boolean Interactive { get; set; }

        /// <summary>
        /// Gets a value indicating whether the layout has input focus.
        /// </summary>
        Boolean Focused { get; }

        /// <summary>
        /// Gets or sets the window to which the layout is drawn.
        /// </summary>
        /// <remarks>If the <c>Window</c> property is <c>null</c>, the layout will be drawn to the primary window.</remarks>
        IUltravioletWindow Window { get; set; }

        /// <summary>
        /// Occurs when the layout is initialized.
        /// </summary>
        event UILayoutEventHandler Initialized;

        /// <summary>
        /// Occurs when the layout begins loading.
        /// </summary>
        event UILayoutEventHandler Loading;

        /// <summary>
        /// Occurs when the layout is ready for interaction.
        /// </summary>
        event UILayoutEventHandler Ready;

        /// <summary>
        /// Occurs when a layout script writes a debug message.
        /// </summary>
        event UILayoutMessageEventHandler ScriptMessage;
    }
}
