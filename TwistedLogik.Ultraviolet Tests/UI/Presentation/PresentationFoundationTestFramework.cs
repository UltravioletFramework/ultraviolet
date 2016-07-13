using System;
using System.IO;
using TwistedLogik.Nucleus.Testing;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Testing;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation
{
    /// <summary>
    /// Represents the base class for tests which require the Presentation Foundation.
    /// </summary>
    public class PresentationFoundationTestFramework : UltravioletApplicationTestFramework
    {
        /// <summary>
        /// Gets the element which currently has focus.
        /// </summary>
        /// <typeparam name="T">The type of element which is expected to have focus.</typeparam>
        /// <param name="app">The Ultraviolet test application.</param>
        /// <returns>The element which currently has focus.</returns>
        protected T GetElementWithFocus<T>(IUltravioletTestApplication app) where T : UIElement
        {
            var screen = app.Ultraviolet.GetUI().GetScreens().Peek();
            if (screen == null)
                return null;

            var view = screen.View as PresentationFoundationView;
            if (view == null)
                return null;

            return view.ElementWithFocus as T;
        }

        /// <summary>
        /// Wraps the element with keyboard focus for evaluation.
        /// </summary>
        /// <param name="app">The test application.</param>
        /// <returns>The wrapped element.</returns>
        protected ObjectResult<UIElement> TheElementWithFocus(IUltravioletTestApplication app)
        {
            return TheResultingObject(GetElementWithFocus<UIElement>(app));
        }

        /// <summary>
        /// Wraps the element with keyboard focus for evaluation.
        /// </summary>
        /// <param name="app">The test application.</param>
        /// <returns>The wrapped element.</returns>
        protected ObjectResult<T> TheElementWithFocus<T>(IUltravioletTestApplication app) where T : UIElement
        {
            return TheResultingObject(GetElementWithFocus<T>(app));
        }

        /// <summary>
        /// Initializes a test application which displays the specified Presentation Foundation view.
        /// </summary>
        protected IUltravioletTestApplication GivenAPresentationFoundationTestFor<T>(Func<ContentManager, T> ctor) where T : UIScreen
        {
            return GivenAnUltravioletApplication()
                .WithPresentationFoundationConfigured()
                .WithInitialization(uv =>
                {
                    var upf = uv.GetUI().GetPresentationFoundation();
                    upf.CompileExpressions("Content", CompileExpressionsFlags.GenerateInMemory);
                    upf.LoadCompiledExpressions();
                })
                .WithContent(content =>
                {
                    content.Ultraviolet.GetContent().Manifests.Load(Path.Combine("Content", "Manifests", "Global.manifest"));

                    var globalStyleSheet = content.Load<UvssDocument>(@"UI\DefaultUIStyles");
                    content.Ultraviolet.GetUI().GetPresentationFoundation().SetGlobalStyleSheet(globalStyleSheet);

                    var screen = ctor(content);
                    content.Ultraviolet.GetUI().GetScreens().Open(screen);
                });
        }
    }
}
