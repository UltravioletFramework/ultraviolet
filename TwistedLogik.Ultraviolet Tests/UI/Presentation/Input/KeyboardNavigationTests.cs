using Microsoft.VisualStudio.TestTools.UnitTesting;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Tests.UI.Presentation.Screens;
using TwistedLogik.Ultraviolet.UI.Presentation.Controls;

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
        public void UPF_KeyNav_TabIsSuppressed_WhenKeyDownIsHandled()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_SuppressTab(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WhenKeyboardNavigationModeIsCycle()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Cycle(content))
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
                .OnFrame(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WhenKeyboardNavigationModeIsOnce()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Once(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WhenKeyboardNavigationModeIsNone()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_None(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WhenKeyboardNavigationModeIsContained()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Contained(content))
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
                .OnFrame(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WithTabIndices()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_TabIndices(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrame(12, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(13, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WithTabIndices_AndKeyboardNavigationModeIsLocal()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_TabIndicesLocal(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrame(12, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrame(13, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
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
        
        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_ShiftTabIsSuppressed_WhenKeyDownIsHandled()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_SuppressTab(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WhenKeyboardNavigationModeIsCycle()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Cycle(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WhenKeyboardNavigationModeIsOnce()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Once(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WhenKeyboardNavigationModeIsNone()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_None(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WhenKeyboardNavigationModeIsContained()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Contained(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WithTabIndices()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_TabIndices(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(12, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(13, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .RunAllFrameActions();
        }

        [TestMethod]
        [TestCategory("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WithTabIndices_AndKeyboardNavigationModeIsLocal()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_TabIndicesLocal(content))
                .OnFrame(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrame(12, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrame(13, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .RunAllFrameActions();
        }
    }
}
