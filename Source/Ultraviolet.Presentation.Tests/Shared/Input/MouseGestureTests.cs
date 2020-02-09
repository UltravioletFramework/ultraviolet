using System.Runtime.CompilerServices;
using NUnit.Framework;
using Ultraviolet.Presentation.Input;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Presentation.Tests.Input
{
    [TestFixture]
    public class MouseGestureTests : UltravioletTestFramework
    {
        [Test]
        public void MouseGesture_TryParse_SucceedsForValidStrings()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(MouseGesture);
            var result = MouseGesture.TryParse("MiddleDoubleClick", out gesture);

            TheResultingValue(result).ShouldBe(true);
            TheResultingValue(gesture.MouseAction).ShouldBe(MouseAction.MiddleDoubleClick);
            TheResultingValue(gesture.Modifiers).ShouldBe(ModifierKeys.None);
        }

        [Test]
        public void MouseGesture_TryParse_SucceedsForValidStrings_WithModifierKeys()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(MouseGesture);
            var result = MouseGesture.TryParse("Ctrl+Alt+MiddleDoubleClick", out gesture);

            TheResultingValue(result).ShouldBe(true);
            TheResultingValue(gesture.MouseAction).ShouldBe(MouseAction.MiddleDoubleClick);
            TheResultingValue(gesture.Modifiers).ShouldBe(ModifierKeys.Control | ModifierKeys.Alt);
        }

        [Test]
        public void MouseGesture_TryParse_FailsForInvalidStrings()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(MouseGesture);
            var result = MouseGesture.TryParse("asdfasdfas", out gesture);

            TheResultingValue(result).ShouldBe(false);
            TheResultingObject(gesture).ShouldBeNull();
        }

        [Test]
        public void MouseGesture_TryParse_FailsForInvalidStringsWithRepeatedModifiers()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(MouseGesture);
            var result = MouseGesture.TryParse("Ctrl+Ctrl+LeftClick", out gesture);

            TheResultingValue(result).ShouldBe(false);
            TheResultingObject(gesture).ShouldBeNull();
        }
    }
}
