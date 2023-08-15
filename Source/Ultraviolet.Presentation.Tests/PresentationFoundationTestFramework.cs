using System;
using System.IO;
using Ultraviolet.Content;
using Ultraviolet.Core.TestFramework;
using Ultraviolet.Presentation.Styles;
using Ultraviolet.TestApplication;
using Ultraviolet.TestFramework;
using Ultraviolet.UI;

namespace Ultraviolet.Presentation.Tests
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
        protected T GetElementWithFocus<T>(IUltravioletTestApplicationAdapter app) where T : UIElement
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
        protected ObjectResult<UIElement> TheElementWithFocus(IUltravioletTestApplicationAdapter app)
        {
            return TheResultingObject(GetElementWithFocus<UIElement>(app));
        }

        /// <summary>
        /// Wraps the element with keyboard focus for evaluation.
        /// </summary>
        /// <param name="app">The test application.</param>
        /// <returns>The wrapped element.</returns>
        protected ObjectResult<T> TheElementWithFocus<T>(IUltravioletTestApplicationAdapter app) where T : UIElement
        {
            return TheResultingObject(GetElementWithFocus<T>(app));
        }

        /// <summary>
        /// Initializes a test application which displays the specified Presentation Foundation view.
        /// </summary>
        protected IUltravioletTestApplicationAdapter GivenAPresentationFoundationTestFor<T>(Func<ContentManager, T> ctor) where T : UIScreen
        {
            var globalStyleSheet = default(GlobalStyleSheet);
            var screen = default(UIScreen);

            return GivenAnUltravioletApplication()
                .WithPresentationFoundationConfigured()
                .WithInitialization(uv =>
                {
                    var upf = uv.GetUI().GetPresentationFoundation();
                    upf.CompileExpressions(Path.Combine("Resources", "Content"), CompileExpressionsFlags.GenerateInMemory | CompileExpressionsFlags.WorkInTemporaryDirectory);
                    upf.LoadCompiledExpressions();
                })
                .WithContent(content =>
                {
                    content.Ultraviolet.GetContent().Manifests.Load(Path.Combine("Resources", "Content", "Manifests", "Global.manifest"));

                    globalStyleSheet = GlobalStyleSheet.Create();
                    globalStyleSheet.Append(content, "UI/DefaultUIStyles");

                    content.Ultraviolet.GetUI().GetPresentationFoundation().SetGlobalStyleSheet(globalStyleSheet);

                    screen = ctor(content);
                    content.Ultraviolet.GetUI().GetScreens().Open(screen);
                })
                .WithDispose(() =>
                {
                    screen?.Dispose();
                    globalStyleSheet?.Dispose();
                });
        }
    }
}
