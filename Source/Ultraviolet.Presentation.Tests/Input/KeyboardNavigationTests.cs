using NUnit.Framework;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls;
using Ultraviolet.Presentation.Tests.Screens;

namespace Ultraviolet.Presentation.Tests.Input
{
    [TestFixture]
    public class KeyboardNavigationTests : PresentationFoundationTestFramework
    {
        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_TabMovesToNextElement()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Simple(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_TabIsSuppressed_WhenKeyDownIsHandled()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_SuppressTab(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WhenKeyboardNavigationModeIsCycle()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Cycle(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WhenKeyboardNavigationModeIsOnce()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Once(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WhenKeyboardNavigationModeIsNone()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_None(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WhenKeyboardNavigationModeIsContained()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Contained(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WithTabIndices()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_TabIndices(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(12, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(13, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_TabNavigatesCorrectly_WithTabIndices_AndKeyboardNavigationModeIsLocal()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_TabIndicesLocal(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(12, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, false))
                .OnFrameStart(13, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_ShiftTabMovesToPreviousElement()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Simple(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .RunAllFrameActions();
        }
        
        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_ShiftTabIsSuppressed_WhenKeyDownIsHandled()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_SuppressTab(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WhenKeyboardNavigationModeIsCycle()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Cycle(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WhenKeyboardNavigationModeIsOnce()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Once(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WhenKeyboardNavigationModeIsNone()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_None(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WhenKeyboardNavigationModeIsContained()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_Contained(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WithTabIndices()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_TabIndices(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(12, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(13, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_KeyNav_ShiftTabNavigatesCorrectly_WithTabIndices_AndKeyboardNavigationModeIsLocal()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_KeyNav_TabIndicesLocal(content))
                .OnFrameStart(1, app => TheElementWithFocus(app).ShouldBeNull())
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(3, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(5, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb4"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(7, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb2"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(9, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb1"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(11, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb5"))
                .OnFrameStart(12, app => app.SpoofKeyPress(Scancode.Tab, Key.Tab, false, false, true))
                .OnFrameStart(13, app => TheElementWithFocus<TextBox>(app).ShouldSatisfyTheCondition(x => x.Name == "tb3"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_DirNav_ArrowsNavigateCorrectly_WhenKeyboardNavigationModeIsContinue()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_DirNav_Continue(content))
                .OnFrameStart(1, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"))
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Down, Key.Down, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn4"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrameStart(9, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrameStart(11, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn2"))
                .OnFrameStart(12, app => app.SpoofKeyPress(Scancode.Down, Key.Down, false, false, false))
                .OnFrameStart(13, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrameStart(14, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(15, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn5"))
                .OnFrameStart(16, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(17, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnR"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_DirNav_ArrowsNavigateCorrectly_WhenKeyboardNavigationModeIsCycle()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_DirNav_Cycle(content))
                .OnFrameStart(1, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"))
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn5"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(9, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(11, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrameStart(12, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrameStart(13, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn2"))
                .OnFrameStart(14, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrameStart(15, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn4"))
                .OnFrameStart(16, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrameStart(17, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_DirNav_ArrowsNavigateCorrectly_WhenKeyboardNavigationModeIsContained()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_DirNav_Contained(content))
                .OnFrameStart(1, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Left, Key.Left, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(5, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn5"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Left, Key.Left, false, false, false))
                .OnFrameStart(9, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrameStart(10, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrameStart(11, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrameStart(12, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn2"))
                .OnFrameStart(13, app => app.SpoofKeyPress(Scancode.Down, Key.Down, false, false, false))
                .OnFrameStart(14, app => app.SpoofKeyPress(Scancode.Down, Key.Down, false, false, false))
                .OnFrameStart(15, app => app.SpoofKeyPress(Scancode.Down, Key.Down, false, false, false))
                .OnFrameStart(16, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn4"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_DirNav_ArrowsNavigateCorrectly_WhenKeyboardNavigationModeIsOnce()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_DirNav_Once(content))
                .OnFrameStart(1, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"))
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnR"))
                .OnFrameStart(6, app => app.SpoofKeyPress(Scancode.Left, Key.Left, false, false, false))
                .OnFrameStart(7, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn5"))
                .OnFrameStart(8, app => app.SpoofKeyPress(Scancode.Left, Key.Left, false, false, false))
                .OnFrameStart(9, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"));
        }

        [Test]
        [Category("UPF")]
        public void UPF_DirNav_ArrowsNavigateCorrectly_WhenKeyboardNavigationModeIsNone()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_DirNav_None(content))
                .OnFrameStart(1, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"))
                .OnFrameStart(2, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrameStart(3, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnR"))
                .OnFrameStart(4, app => app.SpoofKeyPress(Scancode.Left, Key.Left, false, false, false))
                .OnFrameStart(5, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"));
        }
    }
}
