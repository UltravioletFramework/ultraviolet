using System;

namespace Ultraviolet.Core.Tests.Data
{
    /// <summary>
    /// Represents a simple model used by the object loader tests.
    /// </summary>
    public class ObjectLoader_SimpleModel : ObjectLoader_SimpleModelBase
    {
        public ObjectLoader_SimpleModel(String key, Guid globalID)
            : base(key, globalID)
        {

        }

        public ObjectLoader_SimpleModel(String key, Guid globalID, String stringValue)
            : base(key, globalID)
        {
            this.StringValue = stringValue;
        }

        public ObjectLoader_SimpleModel(String key, Guid globalID, String stringValue, ObjectLoader_ComplexRefObject refObject)
            : base(key, globalID)
        {
            this.StringValue = stringValue;
            this.ComplexReference = refObject;
        }

        public String ReadOnlyProperty
        {
            get { return "unsettable"; }
        }

        public String StringValue
        {
            get;
            set;
        }

        public readonly Boolean BooleanValue;
        
        public readonly SByte SByteValue;

        public readonly Byte ByteValue;

        public readonly Char CharValue;

        public readonly Int16 Int16Value;

        public readonly Int32 Int32Value;

        public readonly Int64 Int64Value;

        public readonly UInt16 UInt16Value;

        public readonly UInt32 UInt32Value;

        public readonly UInt64 UInt64Value;

        public readonly Single SingleValue;

        public readonly Double DoubleValue;

        public readonly ObjectLoader_SimpleEnum EnumValue;

        public readonly ObjectLoader_SimpleFlags FlagsValue;

        public readonly ObjectLoader_ComplexValueObject ComplexValue;

        public readonly ObjectLoader_ComplexRefObject ComplexReference;

        public readonly ObjectLoader_ComplexRefObjectF ComplexReferenceF;

        public readonly ObjectLoader_ComplexList ComplexList;
    }
}
