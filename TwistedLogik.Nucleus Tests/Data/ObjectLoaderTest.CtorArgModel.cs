using System;
using TwistedLogik.Nucleus.Data;

namespace TwistedLogik.Nucleus.Tests.Data
{
    /// <summary>
    /// Represents a model used by the object loader tests to test constructor arguments.
    /// </summary>
    public class ObjectLoader_CtorArgModel : DataObject
    {
        public ObjectLoader_CtorArgModel(Guid globalID, Int32 x, Int32 y)
            : base(globalID)
        {
            this.x = x;
            this.y = y;
        }

        public ObjectLoader_CtorArgModel(Guid globalID, ObjectLoader_ComplexValueObject arg)
            : base(globalID)
        {
            this.Arg = arg;
        }

        public Int32 X
        {
            get { return x; }
        }

        public Int32 Y
        {
            get { return y; }
        }

        public readonly ObjectLoader_ComplexValueObject Arg;

        private Int32 x;
        private Int32 y;
    }
}
