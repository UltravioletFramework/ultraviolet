using NUnit.Framework;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Controls;
using Ultraviolet.Tests.UI.Presentation.Screens;

namespace Ultraviolet.Tests.UI.Presentation.Input
{
    [TestFixture]
    public class KeyboardNavigationTests : PresentationFoundationTestFramework
    {
        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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
        
        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
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

        [Test]
        [Category("UPF")]
        public void UPF_DirNav_ArrowsNavigateCorrectly_WhenKeyboardNavigationModeIsContinue()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_DirNav_Continue(content))
                .OnFrame(1, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"))
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(5, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Down, Key.Down, false, false, false))
                .OnFrame(7, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn4"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrame(9, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrame(11, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn2"))
                .OnFrame(12, app => app.SpoofKeyPress(Scancode.Down, Key.Down, false, false, false))
                .OnFrame(13, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrame(14, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(15, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn5"))
                .OnFrame(16, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(17, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnR"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_DirNav_ArrowsNavigateCorrectly_WhenKeyboardNavigationModeIsCycle()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_DirNav_Cycle(content))
                .OnFrame(1, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"))
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(5, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(7, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn5"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(9, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(11, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrame(12, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrame(13, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn2"))
                .OnFrame(14, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrame(15, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn4"))
                .OnFrame(16, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrame(17, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_DirNav_ArrowsNavigateCorrectly_WhenKeyboardNavigationModeIsContained()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_DirNav_Contained(content))
                .OnFrame(1, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Left, Key.Left, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(5, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(7, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn5"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Left, Key.Left, false, false, false))
                .OnFrame(9, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn3"))
                .OnFrame(10, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrame(11, app => app.SpoofKeyPress(Scancode.Up, Key.Up, false, false, false))
                .OnFrame(12, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn2"))
                .OnFrame(13, app => app.SpoofKeyPress(Scancode.Down, Key.Down, false, false, false))
                .OnFrame(14, app => app.SpoofKeyPress(Scancode.Down, Key.Down, false, false, false))
                .OnFrame(15, app => app.SpoofKeyPress(Scancode.Down, Key.Down, false, false, false))
                .OnFrame(16, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn4"))
                .RunAllFrameActions();
        }

        [Test]
        [Category("UPF")]
        public void UPF_DirNav_ArrowsNavigateCorrectly_WhenKeyboardNavigationModeIsOnce()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_DirNav_Once(content))
                .OnFrame(1, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"))
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn1"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(5, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnR"))
                .OnFrame(6, app => app.SpoofKeyPress(Scancode.Left, Key.Left, false, false, false))
                .OnFrame(7, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btn5"))
                .OnFrame(8, app => app.SpoofKeyPress(Scancode.Left, Key.Left, false, false, false))
                .OnFrame(9, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"));
        }

        [Test]
        [Category("UPF")]
        public void UPF_DirNav_ArrowsNavigateCorrectly_WhenKeyboardNavigationModeIsNone()
        {
            GivenAPresentationFoundationTestFor(content => new UPF_DirNav_None(content))
                .OnFrame(1, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"))
                .OnFrame(2, app => app.SpoofKeyPress(Scancode.Right, Key.Right, false, false, false))
                .OnFrame(3, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnR"))
                .OnFrame(4, app => app.SpoofKeyPress(Scancode.Left, Key.Left, false, false, false))
                .OnFrame(5, app => TheElementWithFocus<Button>(app).ShouldSatisfyTheCondition(x => x.Name == "btnL"));
        }
    }
}
