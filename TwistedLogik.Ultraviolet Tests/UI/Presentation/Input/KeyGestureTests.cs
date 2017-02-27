using System.Runtime.CompilerServices;
using NUnit.Framework;
using TwistedLogik.Ultraviolet.Input;
using TwistedLogik.Ultraviolet.Testing;
using TwistedLogik.Ultraviolet.UI.Presentation.Input;

namespace TwistedLogik.Ultraviolet.Tests.UI.Presentation.Input
{
    [TestFixture]
    public class KeyGestureTests : UltravioletTestFramework
    {
        [Test]
        public void KeyGesture_TryParse_SucceedsForValidStrings()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(KeyGesture);
            var result = KeyGesture.TryParse("X", out gesture);

            TheResultingValue(result).ShouldBe(true);
            TheResultingValue(gesture.Key).ShouldBe(Key.X);
            TheResultingValue(gesture.Modifiers).ShouldBe(ModifierKeys.None);
            TheResultingString(gesture.DisplayString).ShouldBe("X");
        }

        [Test]
        public void KeyGesture_TryParse_SucceedsForValidStrings_WithModifierKeys()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(KeyGesture);
            var result = KeyGesture.TryParse("Ctrl+Alt+X", out gesture);

            TheResultingValue(result).ShouldBe(true);
            TheResultingValue(gesture.Key).ShouldBe(Key.X);
            TheResultingValue(gesture.Modifiers).ShouldBe(ModifierKeys.Control | ModifierKeys.Alt);
            TheResultingString(gesture.DisplayString).ShouldBe("Ctrl+Alt+X");
        }

        [Test]
        public void KeyGesture_TryParse_FailsForInvalidStrings()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(KeyGesture);
            var result = KeyGesture.TryParse("asdfasdfas", out gesture);

            TheResultingValue(result).ShouldBe(false);
            TheResultingObject(gesture).ShouldBeNull();
        }

        [Test]
        public void KeyGesture_TryParse_FailsForInvalidStringsWithRepeatedModifiers()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(KeyGesture);
            var result = KeyGesture.TryParse("Ctrl+Ctrl+X", out gesture);

            TheResultingValue(result).ShouldBe(false);
            TheResultingObject(gesture).ShouldBeNull();
        }
    }
}
