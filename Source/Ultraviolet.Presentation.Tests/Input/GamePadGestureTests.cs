using System.Runtime.CompilerServices;
using NUnit.Framework;
using Ultraviolet.Input;
using Ultraviolet.Presentation.Input;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Presentation.Tests.Input
{
    [TestFixture]
    public class GamePadGestureTests : UltravioletTestFramework
    {
        [Test]
        public void GamePadGesture_TryParse_SucceedsForValidStrings()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(GamePadGesture);
            var result = GamePadGesture.TryParse("LeftStick", out gesture);

            TheResultingValue(result).ShouldBe(true);
            TheResultingValue(gesture.Button).ShouldBe(GamePadButton.LeftStick);
            TheResultingValue(gesture.PlayerIndex).ShouldBe(GamePadGesture.AnyPlayerIndex);
        }

        [Test]
        public void GamePadGesture_TryParse_SucceedsForValidStrings_WithExplicitAnyPlayerIndex()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(GamePadGesture);
            var result = GamePadGesture.TryParse("ANY:LeftStick", out gesture);

            TheResultingValue(result).ShouldBe(true);
            TheResultingValue(gesture.Button).ShouldBe(GamePadButton.LeftStick);
            TheResultingValue(gesture.PlayerIndex).ShouldBe(GamePadGesture.AnyPlayerIndex);
        }

        [Test]
        public void GamePadGesture_TryParse_SucceedsForValidStrings_WithNumericPlayerIndex()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(GamePadGesture);
            var result = GamePadGesture.TryParse("P1:LeftStick", out gesture);

            TheResultingValue(result).ShouldBe(true);
            TheResultingValue(gesture.Button).ShouldBe(GamePadButton.LeftStick);
            TheResultingValue(gesture.PlayerIndex).ShouldBe(1);
        }

        [Test]
        public void GamePadGesture_TryParse_FailsForInvalidStrings()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(GamePadGesture);
            var result = GamePadGesture.TryParse("asdfasdfas", out gesture);

            TheResultingValue(result).ShouldBe(false);
            TheResultingObject(gesture).ShouldBeNull();
        }

        [Test]
        public void GamePadGesture_TryParse_FailsForInvalidStringsWithNegativePlayerIndices()
        {
            RuntimeHelpers.RunClassConstructor(typeof(UltravioletStrings).TypeHandle);

            var gesture = default(GamePadGesture);
            var result = GamePadGesture.TryParse("P-1:LeftStick", out gesture);

            TheResultingValue(result).ShouldBe(false);
            TheResultingObject(gesture).ShouldBeNull();
        }
    }
}
