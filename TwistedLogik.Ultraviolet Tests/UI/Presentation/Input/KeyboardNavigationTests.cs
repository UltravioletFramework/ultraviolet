using System;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Testing;
using TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens;
using TwistedLogik.Ultraviolet.UI;
using TwistedLogik.Ultraviolet.UI.Presentation;
using TwistedLogik.Ultraviolet.UI.Presentation.Styles;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Input
{
    [TestClass]
    public class KeyboardNavigationTests : PresentationFoundationTestFramework
    {
        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            StandardInitialization(testContext);
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_TabMovesToNextElement()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Simple(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_ShiftTabMovesToPreviousElement()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Simple(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .RunAllFrameActions();
        }        
    }
}
