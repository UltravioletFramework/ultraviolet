using System;
using NUnit.Framework;
using Ultraviolet.Presentation;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Presentation.Tests
{
    [TestFixture]
    public class VersionedStringSourceTests : UltravioletTestFramework
    {
        [Test]
        public void VersionedStringSource_IsInvalid_WhenNoSourceIsSpecified()
        {
            var source = new VersionedStringSource();

            TheResultingValue(source.IsValid).ShouldBe(false);
            TheResultingValue(source.IsSourcedFromString).ShouldBe(false);
            TheResultingValue(source.IsSourcedFromStringBuilder).ShouldBe(false);
        }

        [Test]
        public void VersionedStringSource_IsValid_WhenStringSourceIsSpecified()
        {
            var data = "Hello, world!";
            var source = new VersionedStringSource(data);

            TheResultingValue(source.IsValid).ShouldBe(true);
            TheResultingValue(source.IsSourcedFromString).ShouldBe(true);
            TheResultingValue(source.IsSourcedFromStringBuilder).ShouldBe(false);
        }

        [Test]
        public void VersionedStringSource_IsValid_WhenStringBuilderSourceIsSpecified()
        {
            var data = new VersionedStringBuilder("Hello, world!");
            var source = new VersionedStringSource(data);

            TheResultingValue(source.IsValid).ShouldBe(true);
            TheResultingValue(source.IsSourcedFromString).ShouldBe(false);
            TheResultingValue(source.IsSourcedFromStringBuilder).ShouldBe(true);
        }

        [Test]
        public void VersionedStringSource_ProducesCorrectString_WhenToStringIsCalled_AndSourceIsInvalid()
        {
            var source = new VersionedStringSource();

            TheResultingString(source.ToString()).ShouldBe(null);
        }

        [Test]
        public void VersionedStringSource_ProducesCorrectString_WhenToStringIsCalled_AndSourceIsString()
        {
            var data = "Hello, world!";
            var source = new VersionedStringSource(data);

            TheResultingString(source.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringSource_ProducesCorrectString_WhenToStringIsCalled_AndSourceIsStringBuilder()
        {
            var data = new VersionedStringBuilder("Hello, world!");
            var source = new VersionedStringSource(data);

            TheResultingString(source.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringSource_ExplicitlyConvertsToString_WhenSourceIsString()
        {
            var data = "Hello, world!";
            var source = new VersionedStringSource(data);

            TheResultingString((String)source).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringSource_CannotConvertToString_WhenSourceIsNotString()
        {
            var data = new VersionedStringBuilder("Hello, world!");
            var source = new VersionedStringSource(data);

            Assert.That(() => TheResultingString((String)source).ShouldBe("Hello, world!"),
                Throws.TypeOf<InvalidCastException>());
        }

        [Test]
        public void VersionedStringSource_ExplicitlyConvertsToStringBuilder_WhenSourceIsStringBuilder()
        {
            var data = new VersionedStringBuilder("Hello, world!");
            var source = new VersionedStringSource(data);

            TheResultingObject((VersionedStringBuilder)source)
                .ShouldSatisfyTheCondition(x => x.ToString() == "Hello, world!");
        }

        [Test]
        public void VersionedStringSource_CannotConvertToStringBuilder_WhenSourceIsNotStringBuilder()
        {
            var data = "Hello, world!";
            var source = new VersionedStringSource(data);

            Assert.That(() =>
            {
                TheResultingObject((VersionedStringBuilder)source)
                    .ShouldSatisfyTheCondition(x => x.ToString() == "Hello, world!");
            },
            Throws.TypeOf<InvalidCastException>());
        }
    }
}
