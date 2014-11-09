using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Ultraviolet.Content;

namespace WindowsFormsApplication1
{
    public class DataRegistry : UltravioletDataObjectRegistry<Data>
    {
        public override string ReferenceResolutionName
        {
            get { return "data"; }
        }

        public override string DataElementName
        {
            get { return "Data"; }
        }
    }

    public class Data : DataObject
    {
        public Data(String key, Guid globalID)
            : base(key, globalID)
        {
        }

        public String Foo { get; protected set; }
    }
}
