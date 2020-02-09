using System;
using System.Text;
using NUnit.Framework;
using Ultraviolet.Presentation;
using Ultraviolet.TestFramework;

namespace Ultraviolet.Presentation.Tests
{
    [TestFixture]
    public class VersionedStringBuilderTests : UltravioletTestFramework
    {
        [Test]
        public void VersionedStringBuilder_IsInitializedToSpecifiedString()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            TheResultingValue(builder.Version).ShouldBe(0);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenClearIsCalled()
        {
            var builder = new VersionedStringBuilder("Hello, world!");
            
            builder.Clear();

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe(String.Empty);
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenAppendStringIsCalled()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var value = " Goodbye, world!";
            builder.Append(value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world! Goodbye, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsNotIncremented_WhenAppendStringIsCalled_AndValueIsNull()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            builder.Append((String)null);

            TheResultingValue(builder.Version).ShouldBe(0);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenAppendStringBuilderIsCalled()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var value = new StringBuilder(" Goodbye, world!");
            builder.Append(value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world! Goodbye, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsNotIncremented_WhenAppendStringBuilderIsCalled_AndValueIsNull()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            builder.Append((StringBuilder)null);

            TheResultingValue(builder.Version).ShouldBe(0);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenAppendVersionedStringBuilderIsCalled()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var value = new VersionedStringBuilder(" Goodbye, world!");
            builder.Append(value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world! Goodbye, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsNotIncremented_WhenAppendVersionedStringBuilderIsCalled_AndValueIsNull()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            builder.Append((VersionedStringBuilder)null);

            TheResultingValue(builder.Version).ShouldBe(0);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenAppendVersionedStringSourceIsCalled_WithValidStringSource()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var source = " Goodbye, world!";
            var value = new VersionedStringSource(source);
            builder.Append(value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world! Goodbye, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenAppendVersionedStringSourceIsCalled_WithValidStringBuilderSource()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var source = new VersionedStringBuilder(" Goodbye, world!");
            var value = new VersionedStringSource(source);
            builder.Append(value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world! Goodbye, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsNotIncremented_WhenAppendVersionedStringSourceIsCalled_WithInvalidSource()
        {
            var builder = new VersionedStringBuilder("Hello, world!");
            
            var value = new VersionedStringSource();
            builder.Append(value);

            TheResultingValue(builder.Version).ShouldBe(0);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenAppendCharIsCalled()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var value = '!';
            builder.Append(value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenInsertStringIsCalled()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var value = " and goodbye";
            builder.Insert(5, value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello and goodbye, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsNotIncremented_WhenInsertStringIsCalled_AndValueIsNull()
        {
            var builder = new VersionedStringBuilder("Hello, world!");
            
            builder.Insert(0, (String)null);

            TheResultingValue(builder.Version).ShouldBe(0);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenInsertStringBuilderIsCalled()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var value = new StringBuilder(" and goodbye");
            builder.Insert(5, value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello and goodbye, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsNotIncremented_WhenInsertStringBuilderIsCalled_AndValueIsNull()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            builder.Insert(0, (StringBuilder)null);

            TheResultingValue(builder.Version).ShouldBe(0);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenInsertVersionedStringBuilderIsCalled()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var value = new VersionedStringBuilder(" and goodbye");
            builder.Insert(5, value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello and goodbye, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsNotIncremented_WhenInsertVersionedStringBuilderIsCalled_AndValueIsNull()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            builder.Insert(0, (VersionedStringBuilder)null);

            TheResultingValue(builder.Version).ShouldBe(0);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenInsertVersionedStringSourceIsCalled_WithValidStringSource()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var source = " and goodbye";
            var value = new VersionedStringSource(source);
            builder.Insert(5, value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello and goodbye, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenInsertVersionedStringSourceIsCalled_WithValidStringBuilderSource()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var source = new VersionedStringBuilder(" and goodbye");
            var value = new VersionedStringSource(source);
            builder.Insert(5, value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello and goodbye, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsNotIncremented_WhenInsertVersionedStringSourceIsCalled_WithInvalidSource()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var value = new VersionedStringSource();
            builder.Insert(5, value);

            TheResultingValue(builder.Version).ShouldBe(0);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!");
        }

        [Test]
        public void VersionedStringBuilder_VersionIsIncremented_WhenInsertCharIsCalled()
        {
            var builder = new VersionedStringBuilder("Hello, world!");

            var value = '!';
            builder.Insert(12, value);

            TheResultingValue(builder.Version).ShouldBe(1);
            TheResultingString(builder.ToString()).ShouldBe("Hello, world!!");
        }
    }
}
