using System;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Ultraviolet.Core.Data;
using Ultraviolet.Core.TestFramework;

namespace Ultraviolet.Core.Tests.Data
{
    [TestFixture]
    public class ObjectLoaderTest : CoreTestFramework
    {
        [Test]
        public void ObjectLoader_MustSpecifyClass()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <StringValue>foo</StringValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_MustSpecifyKey()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <StringValue>foo</StringValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_MustSpecifyID()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1'>
                        <StringValue>foo</StringValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_ClassMustExist()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ClassWhichDoesNotExist, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <StringValue>foo</StringValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_ClassAlias_MustSpecifyName()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <Aliases>
                        <Alias>Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, Ultraviolet.Core.Tests</Alias>
                    </Aliases>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1'>
                        <StringValue>foo</StringValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_ClassAlias_MustSpecifyValue()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <Aliases>
                        <Alias Name='Foo'></Alias>
                    </Aliases>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1'>
                        <StringValue>foo</StringValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_ClassAlias_LoadsSuccessfullyWhenSpecified()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <Aliases>
                        <Alias Name='Foo'>Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"</Alias>
                    </Aliases>
                    <SimpleModel Class='Foo' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <StringValue>Hello, world!</StringValue>
                    </SimpleModel>
                    <SimpleModel Class='Foo' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <StringValue>Goodbye, world!</StringValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingString(results[0].StringValue).ShouldBe("Hello, world!");
            TheResultingString(results[1].StringValue).ShouldBe("Goodbye, world!");
        }

        [Test]
        public void ObjectLoader_ClassAlias_LoadsSuccessfullyWhenDefault()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <Aliases>
                        <Alias Name='Foo' Default='true'>Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"</Alias>
                    </Aliases>
                    <SimpleModel Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <StringValue>Hello, world!</StringValue>
                    </SimpleModel>
                    <SimpleModel Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <StringValue>Goodbye, world!</StringValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingString(results[0].StringValue).ShouldBe("Hello, world!");
            TheResultingString(results[1].StringValue).ShouldBe("Goodbye, world!");
        }

        [Test]
        public void ObjectLoader_GlobalClassAlias_LoadsSuccessfullyWhenSpecified()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Foo' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <StringValue>Hello, world!</StringValue>
                    </SimpleModel>
                    <SimpleModel Class='Foo' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <StringValue>Goodbye, world!</StringValue>
                    </SimpleModel>
                </SimpleModels>");

            ObjectLoader.UnregisterGlobalAlias("Foo");
            ObjectLoader.RegisterGlobalAlias("Foo", typeof(ObjectLoader_SimpleModel));

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingString(results[0].StringValue).ShouldBe("Hello, world!");
            TheResultingString(results[1].StringValue).ShouldBe("Goodbye, world!");
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnMissingFields()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <FooValue>Hello, world!</FooValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<MissingMemberException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnMissingFields_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel FooValue='Hello, world!' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<MissingMemberException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsStringSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <StringValue>Hello, world!</StringValue>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <StringValue>Goodbye, world!</StringValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingString(results[0].StringValue).ShouldBe("Hello, world!");
            TheResultingString(results[1].StringValue).ShouldBe("Goodbye, world!");
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsStringSuccessfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel StringValue='Hello, world!' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel StringValue='Goodbye, world!' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingString(results[0].StringValue).ShouldBe("Hello, world!");
            TheResultingString(results[1].StringValue).ShouldBe("Goodbye, world!");
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsBooleanSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <BooleanValue>true</BooleanValue>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <BooleanValue>false</BooleanValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].BooleanValue).ShouldBe(true);
            TheResultingValue(results[1].BooleanValue).ShouldBe(false);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsBooleanSuccessfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel BooleanValue='True' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel BooleanValue='False' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].BooleanValue).ShouldBe(true);
            TheResultingValue(results[1].BooleanValue).ShouldBe(false);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidBoolean()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <BooleanValue>foo</BooleanValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidBoolean_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel BooleanValue='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsSByteSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <SByteValue>-64</SByteValue>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <SByteValue>64</SByteValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].SByteValue).ShouldBe(-64);
            TheResultingValue(results[1].SByteValue).ShouldBe(+64);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsSByteSuccessfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel SByteValue='-64' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel SByteValue='64' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].SByteValue).ShouldBe(-64);
            TheResultingValue(results[1].SByteValue).ShouldBe(+64);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidSByte()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <SByteValue>foo</SByteValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidSByte_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel SByteValue='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsByteSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ByteValue>64</ByteValue>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <ByteValue>128</ByteValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].ByteValue).ShouldBe(64);
            TheResultingValue(results[1].ByteValue).ShouldBe(128);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsByteSuccessfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel ByteValue='64' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel ByteValue='128' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].ByteValue).ShouldBe(64);
            TheResultingValue(results[1].ByteValue).ShouldBe(128);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidByte()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ByteValue>foo</ByteValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidByte_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel ByteValue='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsCharSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <CharValue>A</CharValue>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <CharValue>F</CharValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].CharValue).ShouldBe('A');
            TheResultingValue(results[1].CharValue).ShouldBe('F');
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsCharSuccessfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel CharValue='A' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel CharValue='F' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].CharValue).ShouldBe('A');
            TheResultingValue(results[1].CharValue).ShouldBe('F');
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidChar()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <CharValue>foo</CharValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidChar_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel CharValue='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsInt16Successfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Int16Value>-123</Int16Value>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <Int16Value>456</Int16Value>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].Int16Value).ShouldBe(-123);
            TheResultingValue(results[1].Int16Value).ShouldBe(+456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsInt16Successfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Int16Value='-123' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel Int16Value='456' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].Int16Value).ShouldBe(-123);
            TheResultingValue(results[1].Int16Value).ShouldBe(+456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidInt16()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Int16Value>foo</Int16Value>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidInt16_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Int16Value='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsInt32Successfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Int32Value>-123</Int32Value>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <Int32Value>456</Int32Value>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].Int32Value).ShouldBe(-123);
            TheResultingValue(results[1].Int32Value).ShouldBe(+456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsInt32Successfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Int32Value='-123' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel Int32Value='456' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].Int32Value).ShouldBe(-123);
            TheResultingValue(results[1].Int32Value).ShouldBe(+456);

        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidInt32()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Int32Value>foo</Int32Value>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidInt32_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Int32Value='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsInt64Successfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Int64Value>-123</Int64Value>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <Int64Value>456</Int64Value>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].Int64Value).ShouldBe(-123);
            TheResultingValue(results[1].Int64Value).ShouldBe(+456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsInt64Successfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Int64Value='-123' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel Int64Value='456' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].Int64Value).ShouldBe(-123);
            TheResultingValue(results[1].Int64Value).ShouldBe(+456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidInt64()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Int64Value>foo</Int64Value>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidInt64_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Int64Value='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsUInt16Successfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <UInt16Value>123</UInt16Value>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <UInt16Value>456</UInt16Value>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].UInt16Value).ShouldBe(123);
            TheResultingValue(results[1].UInt16Value).ShouldBe(456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsUInt16Successfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel UInt16Value='123' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel UInt16Value='456' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].UInt16Value).ShouldBe(123);
            TheResultingValue(results[1].UInt16Value).ShouldBe(456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidUInt16()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <UInt16Value>foo</UInt16Value>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidUInt16_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel UInt16Value='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsUInt32Successfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <UInt32Value>123</UInt32Value>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <UInt32Value>456</UInt32Value>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].UInt32Value).ShouldBe(123);
            TheResultingValue(results[1].UInt32Value).ShouldBe(456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsUInt32Successfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel UInt32Value='123' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel UInt32Value='456' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].UInt32Value).ShouldBe(123);
            TheResultingValue(results[1].UInt32Value).ShouldBe(456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidUInt32()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <UInt32Value>foo</UInt32Value>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidUInt32_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel UInt32Value='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsUInt64Successfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <UInt64Value>123</UInt64Value>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <UInt64Value>456</UInt64Value>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].UInt64Value).ShouldBe(123);
            TheResultingValue(results[1].UInt64Value).ShouldBe(456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsUInt64Successfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel UInt64Value='123' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel UInt64Value='456' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].UInt64Value).ShouldBe(123);
            TheResultingValue(results[1].UInt64Value).ShouldBe(456);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidUInt64()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <UInt64Value>foo</UInt64Value>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidUInt64_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel UInt64Value='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsSingleSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <SingleValue>-123.456</SingleValue>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <SingleValue>456.789</SingleValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].SingleValue).ShouldBe(-123.456f);
            TheResultingValue(results[1].SingleValue).ShouldBe(+456.789f);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsSingleSuccessfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel SingleValue='-123.456' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel SingleValue='456.789' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].SingleValue).ShouldBe(-123.456f);
            TheResultingValue(results[1].SingleValue).ShouldBe(+456.789f);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidSingle()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <SingleValue>foo</SingleValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidSingle_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel SingleValue='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsDoubleSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <DoubleValue>-123.456</DoubleValue>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <DoubleValue>456.789</DoubleValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].DoubleValue).ShouldBe(-123.456);
            TheResultingValue(results[1].DoubleValue).ShouldBe(+456.789);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsDoubleSuccessfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel DoubleValue='-123.456' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel DoubleValue='456.789' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].DoubleValue).ShouldBe(-123.456);
            TheResultingValue(results[1].DoubleValue).ShouldBe(+456.789);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidDouble()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <DoubleValue>foo</DoubleValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidDouble_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel DoubleValue='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsEnumSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <EnumValue>ValueOne</EnumValue>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <EnumValue>ValueTwo</EnumValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].EnumValue).ShouldBe(ObjectLoader_SimpleEnum.ValueOne);
            TheResultingValue(results[1].EnumValue).ShouldBe(ObjectLoader_SimpleEnum.ValueTwo);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsEnumSuccessfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel EnumValue='ValueOne' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel EnumValue='ValueTwo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].EnumValue).ShouldBe(ObjectLoader_SimpleEnum.ValueOne);
            TheResultingValue(results[1].EnumValue).ShouldBe(ObjectLoader_SimpleEnum.ValueTwo);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidEnum()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <EnumValue>foo</EnumValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidEnum_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel EnumValue='foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsFlagsSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <FlagsValue>ValueOne, ValueThree</FlagsValue>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <FlagsValue>ValueTwo</FlagsValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].FlagsValue).ShouldBe(ObjectLoader_SimpleFlags.ValueOne | ObjectLoader_SimpleFlags.ValueThree);
            TheResultingValue(results[1].FlagsValue).ShouldBe(ObjectLoader_SimpleFlags.ValueTwo);
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsFlagsSuccessfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel FlagsValue='ValueOne, ValueThree' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel FlagsValue='ValueTwo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingValue(results[0].FlagsValue).ShouldBe(ObjectLoader_SimpleFlags.ValueOne | ObjectLoader_SimpleFlags.ValueThree);
            TheResultingValue(results[1].FlagsValue).ShouldBe(ObjectLoader_SimpleFlags.ValueTwo);
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidFlags()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <FlagsValue>ValueOne, foo</FlagsValue>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnInvalidFlags_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel FlagsValue='ValueOne, foo' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<FormatException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsInheritedValuesSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <StringValueOnBaseClass>Hello</StringValueOnBaseClass>
                    </SimpleModel>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109'>
                        <StringValueOnBaseClass>Goodbye</StringValueOnBaseClass>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingString(results[0].StringValueOnBaseClass).ShouldBe("Hello");
            TheResultingString(results[1].StringValueOnBaseClass).ShouldBe("Goodbye");
        }

        [Test]
        public void ObjectLoader_SimpleObject_LoadsInheritedValuesSuccessfully_Attribute()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel StringValueOnBaseClass='Hello' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                    <SimpleModel StringValueOnBaseClass='Goodbye' Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT2' ID='2dcf947d-6bc4-4f98-85ae-ca8e56db3109' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(2);

            TheResultingString(results[0].StringValueOnBaseClass).ShouldBe("Hello");
            TheResultingString(results[1].StringValueOnBaseClass).ShouldBe("Goodbye");
        }

        [Test]
        public void ObjectLoader_SimpleObject_DefaultsLoadSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <Defaults>
                        <Default Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"'>
                            <StringValue>This is a default!</StringValue>
                        </Default>
                    </Defaults>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingString(results[0].StringValue).ShouldBe("This is a default!");
        }

        [Test]
        public void ObjectLoader_SimpleObject_InheritedDefaultsLoadSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <Defaults>
                        <Default Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModelBase, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"'>
                            <StringValue>This is a default!</StringValue>
                        </Default>
                    </Defaults>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingString(results[0].StringValue).ShouldBe("This is a default!");
        }

        [Test]
        public void ObjectLoader_SimpleObject_DefaultsLoadInCorrectOrder()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <Defaults>
                        <Default Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"'>
                            <StringValue>But this overrides it!</StringValue>
                        </Default>
                        <Default Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModelBase, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"'>
                            <StringValue>This is a default!</StringValue>
                        </Default>
                    </Defaults>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a' />
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingString(results[0].StringValue).ShouldBe("But this overrides it!");
        }

        [Test]
        public void ObjectLoader_SimpleObject_HandlesAdditionalConstructorArguments()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Constructor>
                            <Argument>Passed in through constructor!</Argument>
                        </Constructor>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingString(results[0].StringValue).ShouldBe("Passed in through constructor!");
        }

        [Test]
        public void ObjectLoader_SimpleObject_ThrowsOnSettingReadOnlyProperty()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ReadOnlyProperty>foo</ReadOnlyProperty>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_SimpleObject_HandlesReservedKeywordsInElements()
        {
            var xml = XDocument.Parse(@"
                <KeywordModels>
                    <KeywordModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ReservedKeywordModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Constructor>
                            <Argument>Hello, world!</Argument>
                        </Constructor>
                        <Class>Set Class</Class>
                        <ID>Set ID</ID>
                        <Type>Set Type</Type>
                    </KeywordModel>
                </KeywordModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ReservedKeywordModel>(xml, "KeywordModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingString(results[0].SetByConstructor).ShouldBe("Hello, world!");
            TheResultingString(results[0].Class).ShouldBe("Set Class");
            TheResultingString(results[0].ID).ShouldBe("Set ID");
            TheResultingString(results[0].Type).ShouldBe("Set Type");
        }

        [Test]
        public void ObjectLoader_ConstructorArgs_LoadsSimpleValuesSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <CtorArgModels>
                    <CtorArgModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_CtorArgModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Constructor>
                            <Argument>123</Argument>
                            <Argument>456</Argument>
                        </Constructor>             
                    </CtorArgModel>
                </CtorArgModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_CtorArgModel>(xml, "CtorArgModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0].X).ShouldBe(123);
            TheResultingValue(results[0].Y).ShouldBe(456);
        }

        [Test]
        public void ObjectLoader_ConstructorArgs_LoadsComplexValuesSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <CtorArgModels>
                    <CtorArgModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_CtorArgModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Constructor>
                            <Argument>
                                <X>123</X>
                                <Y>456</Y>
                                <Z>789</Z>
                                <Child>
                                    <Foobar>Hello, world!</Foobar>
                                </Child>
                            </Argument>
                        </Constructor>             
                    </CtorArgModel>
                </CtorArgModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_CtorArgModel>(xml, "CtorArgModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0].Arg.X).ShouldBe(123);
            TheResultingValue(results[0].Arg.Y).ShouldBe(456);
            TheResultingValue(results[0].Arg.Z).ShouldBe(789);

            TheResultingString(results[0].Arg.Child.Foobar).ShouldBe("Hello, world!");
        }

        [Test]
        public void ObjectLoader_ConstructorArgs_SubstitutesTypesCorrectly()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Constructor>
                            <Argument>Hello, world!</Argument>
                            <Argument Type='Ultraviolet.Core.Tests.Data.ObjectLoader_ComplexRefObjectDerived, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"'>
                                <Foobar>Goodbye, world!</Foobar>
                            </Argument>
                        </Constructor>             
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingString(results[0].StringValue).ShouldBe("Hello, world!");

            var derived = results[0].ComplexReference as ObjectLoader_ComplexRefObjectDerived;

            TheResultingObject(derived).ShouldNotBeNull();
            TheResultingString(derived.Foobar).ShouldBe("Goodbye, world!");
        }

        [Test]
        public void ObjectLoader_ConstructorArgs_ThrowsOnNoMatch()
        {
            var xml = XDocument.Parse(@"
                <CtorArgModels>
                    <CtorArgModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_CtorArgModelNoMatch, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Constructor>
                            <Argument>123</Argument>
                            <Argument>456</Argument>
                        </Constructor>
                    </CtorArgModel>
                </CtorArgModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_CtorArgModelNoMatch>(xml, "CtorArgModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_ConstructorArgs_ThrowsOnAmbiguousMatch()
        {
            var xml = XDocument.Parse(@"
                <CtorArgModels>
                    <CtorArgModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_CtorArgModelAmbiguousMatch, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Constructor>
                            <Argument>123</Argument>
                            <Argument>456</Argument>
                        </Constructor>
                    </CtorArgModel>
                </CtorArgModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_CtorArgModelNoMatch>(xml, "CtorArgModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_Indexer_SingleIndexSetsCorrectly()
        {
            var xml = XDocument.Parse(@"
                <IndexerModels>
                    <IndexerModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_IndexerModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Item ix='0'>5</Item>
                        <Item ix='1'>6</Item>
                        <Item ix='3'>8</Item>
                    </IndexerModel>
                </IndexerModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_IndexerModel>(xml, "IndexerModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0][0]).ShouldBe(5);
            TheResultingValue(results[0][1]).ShouldBe(6);
            TheResultingValue(results[0][2]).ShouldBe(0);
            TheResultingValue(results[0][3]).ShouldBe(8);
        }

        [Test]
        public void ObjectLoader_Indexer_MultipleIndexSetsCorrectly()
        {
            var xml = XDocument.Parse(@"
                <IndexerModels>
                    <IndexerModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_MultiIndexerModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Item x='0' y='0'>5</Item>
                        <Item x='1' y='2'>6</Item>
                        <Item x='3' y='3'>8</Item>
                    </IndexerModel>
                </IndexerModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_MultiIndexerModel>(xml, "IndexerModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0][0, 0]).ShouldBe(5);
            TheResultingValue(results[0][1, 2]).ShouldBe(6);
            TheResultingValue(results[0][2, 2]).ShouldBe(0);
            TheResultingValue(results[0][3, 3]).ShouldBe(8);
        }

        [Test]
        public void ObjectLoader_Indexer_SingleIndexArraySetsCorrectly()
        {
            var xml = XDocument.Parse(@"
                <IndexerModels>
                    <IndexerModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ArrayIndexerModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Item ix='1'>
                            <Items>
                                <Item>5</Item>
                                <Item>6</Item>
                                <Item>7</Item>
                            </Items>
                        </Item>
                    </IndexerModel>
                </IndexerModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ArrayIndexerModel>(xml, "IndexerModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingObject(results[0][0])
                .ShouldBeNull();

            TheResultingCollection(results[0][1])
                .ShouldContainTheSpecifiedNumberOfItems(3)
                .ShouldBeExactly(5, 6, 7);
        }

        [Test]
        public void ObjectLoader_Indexer_SingleIndexListSetsCorrectly()
        {
            var xml = XDocument.Parse(@"
                <IndexerModels>
                    <IndexerModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ListIndexerModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Item ix='1'>
                            <Items>
                                <Item>5</Item>
                                <Item>6</Item>
                                <Item>7</Item>
                            </Items>
                        </Item>
                    </IndexerModel>
                </IndexerModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ListIndexerModel>(xml, "IndexerModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingObject(results[0][0])
                .ShouldBeNull();

            TheResultingCollection(results[0][1])
                .ShouldContainTheSpecifiedNumberOfItems(3)
                .ShouldBeExactly(5, 6, 7);
        }

        [Test]
        public void ObjectLoader_Indexer_ComplexValueSetsCorrectly()
        {
            var xml = XDocument.Parse(@"
                <IndexerModels>
                    <IndexerModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ComplexIndexerModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Item ix='1'>
                            <X>1</X>
                            <Y>2</Y>
                            <Z>3</Z>
                        </Item>
                    </IndexerModel>
                </IndexerModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ComplexIndexerModel>(xml, "IndexerModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0][1].X).ShouldBe(1);
            TheResultingValue(results[0][1].Y).ShouldBe(2);
            TheResultingValue(results[0][1].Z).ShouldBe(3);
        }

        [Test]
        public void ObjectLoader_Indexer_ComplexValueSubstitutesTypeCorrectly()
        {
            var xml = XDocument.Parse(@"
                <IndexerModels>
                    <IndexerModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ComplexIndexerModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Item Type='Ultraviolet.Core.Tests.Data.ObjectLoader_ComplexRefObjectDerived, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' ix='1'>
                            <X>1</X>
                            <Y>2</Y>
                            <Z>3</Z>
                            <Foobar>baz</Foobar>
                        </Item>
                    </IndexerModel>
                </IndexerModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ComplexIndexerModel>(xml, "IndexerModel").ToList();

            TheResultingValue(results[0][1].X).ShouldBe(1);
            TheResultingValue(results[0][1].Y).ShouldBe(2);
            TheResultingValue(results[0][1].Z).ShouldBe(3);

            var derived = results[0][1] as ObjectLoader_ComplexRefObjectDerived;

            TheResultingObject(derived).ShouldNotBeNull();
            TheResultingString(derived.Foobar).ShouldBe("baz");
        }

        [Test]
        public void ObjectLoader_Indexer_ThrowsIfMissingIndexParameter()
        {
            var xml = XDocument.Parse(@"
                <IndexerModels>
                    <IndexerModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_MultiIndexerModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Item x='0'>5</Item>
                    </IndexerModel>
                </IndexerModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_MultiIndexerModel>(xml, "IndexerModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_Indexer_ThrowsIfTooManyIndexParameters()
        {
            var xml = XDocument.Parse(@"
                <IndexerModels>
                    <IndexerModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_MultiIndexerModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Item x='0' y='0' z='0'>5</Item>
                    </IndexerModel>
                </IndexerModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_MultiIndexerModel>(xml, "IndexerModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_Array_PopulatesValues()
        {
            var xml = XDocument.Parse(@"
                <ArrayModels>
                    <ArrayModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ArrayModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ArrayValue>
                            <Items>
                                <Item>5</Item>
                                <Item>6</Item>
                                <Item>7</Item>
                            </Items>
                        </ArrayValue>
                    </ArrayModel>
                </ArrayModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ArrayModel>(xml, "ArrayModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].ArrayValue)
                .ShouldBeExactly(5, 6, 7);
        }

        [Test]
        public void ObjectLoader_Array_PopulatesEmptyArray()
        {
            var xml = XDocument.Parse(@"
                <ArrayModels>
                    <ArrayModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ArrayModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ArrayValue>
                        </ArrayValue>
                    </ArrayModel>
                </ArrayModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ArrayModel>(xml, "ArrayModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].ArrayValue)
                .ShouldBeEmpty();
        }

        [Test]
        public void ObjectLoader_Array_OverwritesExistingArray()
        {
            var xml = XDocument.Parse(@"
                <ArrayModels>
                    <ArrayModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ArrayModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Constructor>
                            <Argument>true</Argument>
                        </Constructor>
                        <ArrayValue>
                        </ArrayValue>
                    </ArrayModel>
                </ArrayModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ArrayModel>(xml, "ArrayModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].ArrayValue)
                .ShouldBeEmpty();
        }

        [Test]
        public void ObjectLoader_Array_LoadsComplexElements()
        {
            var xml = XDocument.Parse(@"
                <ArrayModels>
                    <ArrayModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ArrayComplexModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ArrayValue>
                            <Items>
                                <Item>
                                    <X>1</X>
                                    <Y>2</Y>
                                    <Z>3</Z>
                                </Item>                                
                            </Items>
                        </ArrayValue>
                    </ArrayModel>
                </ArrayModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ArrayComplexModel>(xml, "ArrayModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].ArrayValue)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0].ArrayValue[0].X).ShouldBe(1);
            TheResultingValue(results[0].ArrayValue[0].Y).ShouldBe(2);
            TheResultingValue(results[0].ArrayValue[0].Z).ShouldBe(3);
        }

        [Test]
        public void ObjectLoader_Array_ThrowsOnInvalidArrayDefinition()
        {
            var xml = XDocument.Parse(@"
                <ArrayModels>
                    <ArrayModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ArrayModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ArrayValue>
                            <Items>
                                <Item>0</Item>
                                <Foo>1</Foo>
                            </Items>
                        </ArrayValue>
                    </ArrayModel>
                </ArrayModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_ArrayModel>(xml, "ArrayModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_List_PopulatesValues()
        {
            var xml = XDocument.Parse(@"
                <ListModels>
                    <ListModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ListModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ListValue>
                            <Items>
                                <Item>5</Item>
                                <Item>6</Item>
                                <Item>7</Item>
                            </Items>
                        </ListValue>
                    </ListModel>
                </ListModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ListModel>(xml, "ListModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].ListValue)
                .ShouldBeExactly(5, 6, 7);
        }

        [Test]
        public void ObjectLoader_List_PopulatesEmptyList()
        {
            var xml = XDocument.Parse(@"
                <ListModels>
                    <ListModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ListModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ListValue>
                        </ListValue>
                    </ListModel>
                </ListModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ListModel>(xml, "ListModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].ListValue)
                .ShouldBeEmpty();
        }

        [Test]
        public void ObjectLoader_List_OverwritesExistingList()
        {
            var xml = XDocument.Parse(@"
                <ListModels>
                    <ListModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ListModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Constructor>
                            <Argument>true</Argument>
                        </Constructor>
                        <ListValue>
                        </ListValue>
                    </ListModel>
                </ListModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ListModel>(xml, "ListModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].ListValue)
                .ShouldBeEmpty();
        }

        [Test]
        public void ObjectLoader_List_LoadsComplexElements()
        {
            var xml = XDocument.Parse(@"
                <ListModels>
                    <ListModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ListComplexModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ListValue>
                            <Items>
                                <Item>
                                    <X>1</X>
                                    <Y>2</Y>
                                    <Z>3</Z>
                                </Item>                                
                            </Items>
                        </ListValue>
                    </ListModel>
                </ListModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_ListComplexModel>(xml, "ListModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].ListValue)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0].ListValue[0].X).ShouldBe(1);
            TheResultingValue(results[0].ListValue[0].Y).ShouldBe(2);
            TheResultingValue(results[0].ListValue[0].Z).ShouldBe(3);
        }

        [Test]
        public void ObjectLoader_List_ThrowsOnInvalidListDefinition()
        {
            var xml = XDocument.Parse(@"
                <ListModels>
                    <ListModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ListModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ListValue>
                            <Items>
                                <Item>0</Item>
                                <Foo>1</Foo>
                            </Items>
                        </ListValue>
                    </ListModel>
                </ListModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_ListModel>(xml, "ListModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_Enumerable_PopulatesValues()
        {
            var xml = XDocument.Parse(@"
                <EnumerableModels>
                    <EnumerableModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_EnumerableModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <EnumerableValue>
                            <Items>
                                <Item>5</Item>
                                <Item>6</Item>
                                <Item>7</Item>
                            </Items>
                        </EnumerableValue>
                    </EnumerableModel>
                </EnumerableModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_EnumerableModel>(xml, "EnumerableModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].EnumerableValue)
                .ShouldBeExactly(5, 6, 7);
        }

        [Test]
        public void ObjectLoader_Enumerable_PopulatesEmptyEnumerable()
        {
            var xml = XDocument.Parse(@"
                <EnumerableModels>
                    <EnumerableModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_EnumerableModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <EnumerableValue>
                        </EnumerableValue>
                    </EnumerableModel>
                </EnumerableModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_EnumerableModel>(xml, "EnumerableModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].EnumerableValue)
                .ShouldBeEmpty();
        }

        [Test]
        public void ObjectLoader_Enumerable_OverwritesExistingEnumerable()
        {
            var xml = XDocument.Parse(@"
                <EnumerableModels>
                    <EnumerableModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_EnumerableModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <Constructor>
                            <Argument>true</Argument>
                        </Constructor>
                        <EnumerableValue>
                        </EnumerableValue>
                    </EnumerableModel>
                </EnumerableModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_EnumerableModel>(xml, "EnumerableModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].EnumerableValue)
                .ShouldBeEmpty();
        }

        [Test]
        public void ObjectLoader_Enumerable_LoadsComplexElements()
        {
            var xml = XDocument.Parse(@"
                <EnumerableModels>
                    <EnumerableModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_EnumerableComplexModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <EnumerableValue>
                            <Items>
                                <Item>
                                    <X>1</X>
                                    <Y>2</Y>
                                    <Z>3</Z>
                                </Item>                                
                            </Items>
                        </EnumerableValue>
                    </EnumerableModel>
                </EnumerableModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_EnumerableComplexModel>(xml, "EnumerableModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingCollection(results[0].EnumerableValue)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            var first = results[0].EnumerableValue.First();
            TheResultingValue(first.X).ShouldBe(1);
            TheResultingValue(first.Y).ShouldBe(2);
            TheResultingValue(first.Z).ShouldBe(3);
        }

        [Test]
        public void ObjectLoader_Enumerable_ThrowsOnInvalidListDefinition()
        {
            var xml = XDocument.Parse(@"
                <ListModels>
                    <ListModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_ListModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ListValue>
                            <Items>
                                <Item>0</Item>
                                <Foo>1</Foo>
                            </Items>
                        </ListValue>
                    </ListModel>
                </ListModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_ListModel>(xml, "ListModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_ComplexObjects_LoadsReferenceTypesSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ComplexReference>
                            <X>1</X>
                            <Y>2</Y>
                            <Z>3</Z>
                        </ComplexReference>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0].ComplexReference.X).ShouldBe(1);
            TheResultingValue(results[0].ComplexReference.Y).ShouldBe(2);
            TheResultingValue(results[0].ComplexReference.Z).ShouldBe(3);
        }

        [Test]
        public void ObjectLoader_ComplexObjects_LoadsReferenceTypesSuccessfullyWhenNested()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ComplexReference>
                            <X>1</X>
                            <Y>2</Y>
                            <Z>3</Z>
                            <Child>
                                <X>4</X>
                                <Y>5</Y>
                                <Z>6</Z>
                            </Child>
                        </ComplexReference>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0].ComplexReference.X).ShouldBe(1);
            TheResultingValue(results[0].ComplexReference.Y).ShouldBe(2);
            TheResultingValue(results[0].ComplexReference.Z).ShouldBe(3);

            TheResultingValue(results[0].ComplexReference.Child.X).ShouldBe(4);
            TheResultingValue(results[0].ComplexReference.Child.Y).ShouldBe(5);
            TheResultingValue(results[0].ComplexReference.Child.Z).ShouldBe(6);
        }

        [Test]
        public void ObjectLoader_ComplexObjects_LoadsDerivedReferenceTypesSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ComplexReference Type='Ultraviolet.Core.Tests.Data.ObjectLoader_ComplexRefObjectDerived, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"'>
                            <X>1</X>
                            <Y>2</Y>
                            <Z>3</Z>
                            <Foobar>Hello, world!</Foobar>
                        </ComplexReference>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0].ComplexReference.X).ShouldBe(1);
            TheResultingValue(results[0].ComplexReference.Y).ShouldBe(2);
            TheResultingValue(results[0].ComplexReference.Z).ShouldBe(3);

            var derived = results[0].ComplexReference as ObjectLoader_ComplexRefObjectDerived;

            TheResultingObject(derived).ShouldNotBeNull();
            TheResultingString(derived.Foobar).ShouldBe("Hello, world!");
        }

        [Test]
        public void ObjectLoader_ComplexObjects_ThrowsIfLoadingIncompatibleType()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ComplexReference Type='Ultraviolet.Core.Tests.Data.ObjectLoader_ComplexRefObjectNotDerived, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"'>
                            <X>1</X>
                            <Y>2</Y>
                            <Z>3</Z>
                            <Foobar>Hello, world!</Foobar>
                        </ComplexReference>
                    </SimpleModel>
                </SimpleModels>");

            Assert.That(() => ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList(),
                Throws.TypeOf<InvalidOperationException>());
        }

        [Test]
        public void ObjectLoader_ComplexObjects_LoadsValueTypesSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ComplexValue>
                            <X>1</X>
                            <Y>2</Y>
                            <Z>3</Z>
                        </ComplexValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0].ComplexValue.X).ShouldBe(1);
            TheResultingValue(results[0].ComplexValue.Y).ShouldBe(2);
            TheResultingValue(results[0].ComplexValue.Z).ShouldBe(3);
        }

        [Test]
        public void ObjectLoader_ComplexObjects_LoadsValueTypesSuccessfullyWhenNested()
        {
            var xml = XDocument.Parse(@"
                <SimpleModels>
                    <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ComplexValue>
                            <X>1</X>
                            <Y>2</Y>
                            <Z>3</Z>
                            <Child>
                                <Foobar>Hello, world!</Foobar>
                            </Child>
                        </ComplexValue>
                    </SimpleModel>
                </SimpleModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0].ComplexValue.X).ShouldBe(1);
            TheResultingValue(results[0].ComplexValue.Y).ShouldBe(2);
            TheResultingValue(results[0].ComplexValue.Z).ShouldBe(3);

            TheResultingString(results[0].ComplexValue.Child.Foobar).ShouldBe("Hello, world!");
        }

        [Test]
        public void ObjectLoader_ComplexObjects_LoadsComplexListsSuccessfully()
        {
            var xml = XDocument.Parse(@"
                <ListModels>
                    <ListModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                        <ComplexList>
                            <X>1</X>
                            <Y>2</Y>
                            <Z>3</Z>
                            <Items>
                                <Item>5</Item>
                                <Item>6</Item>
                                <Item>7</Item>
                            </Items>
                        </ComplexList>
                    </ListModel>
                </ListModels>");

            var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "ListModel").ToList();

            TheResultingCollection(results)
                .ShouldContainTheSpecifiedNumberOfItems(1);

            TheResultingValue(results[0].ComplexList.X).ShouldBe(1);
            TheResultingValue(results[0].ComplexList.Y).ShouldBe(2);
            TheResultingValue(results[0].ComplexList.Z).ShouldBe(3);

            TheResultingCollection(results[0].ComplexList)
                .ShouldBeExactly(5, 6, 7);
        }

        [Test]
        public void ObjectLoader_UsesCultureSpecifiedInSource()
        {
            UsingCulture("en-US", () =>
            {
                var xml = XDocument.Parse(@"
                    <SimpleModels Culture='fr-FR'>
                        <SimpleModel Class='Ultraviolet.Core.Tests.Data.ObjectLoader_SimpleModel, " + typeof(ObjectLoaderTest).Assembly.GetName().Name + @"' Key='OBJECT1' ID='6610e29a-57b3-4960-8f40-1466ee82f40a'>
                            <ComplexReferenceF>
                                <X>123,4</X>
                                <Y>456,7</Y>
                                <Z>890,1</Z>
                            </ComplexReferenceF>
                        </SimpleModel>
                    </SimpleModels>");

                var results = ObjectLoader.LoadDefinitions<ObjectLoader_SimpleModel>(xml, "SimpleModel").ToList();

                TheResultingCollection(results)
                    .ShouldContainTheSpecifiedNumberOfItems(1);

                TheResultingValue(results[0].ComplexReferenceF.X).ShouldBe(123.4f);
                TheResultingValue(results[0].ComplexReferenceF.Y).ShouldBe(456.7f);
                TheResultingValue(results[0].ComplexReferenceF.Z).ShouldBe(890.1f);
            });
        }
    }
}
