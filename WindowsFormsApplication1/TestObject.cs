using System;
using System.Collections.Generic;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;
using TwistedLogik.Ultraviolet;
using TwistedLogik.Ultraviolet.Content;
using TwistedLogik.Ultraviolet.Design.Content;
using System.ComponentModel;
using TwistedLogik.Nucleus.Data;
using TwistedLogik.Nucleus.Design.Data;

namespace WindowsFormsApplication1
{
    public class Foo : ContentManifestTypeConverter
    {
        public Foo() : base("Global", "Fonts") { }
    }

    public class TestObject
    {
        public TestObject()
        {
            SomeMatrix = Matrix.Identity;
            Array = new List<object> { 1, 2, 3, 4 };
        }

        [TypeConverter(typeof(DataObjectTypeConverter<Data>))]
        public ResolvedDataObjectReference Data { get; set; }
        [TypeConverter(typeof(Foo))]
        public AssetID Font { get; set; }
        public MaskedUInt32 MaskedUInt32 { get; set; }
        public MaskedUInt64 MaskedUInt64 { get; set; }
        public Radians Radians { get; set; }
        public Color SomeColor { get; set; }
        public Vector2 SomeVector2 { get; set; }
        public Vector3 SomeVector3 { get; set; }
        public Vector4 SomeVector4 { get; set; }
        public Matrix SomeMatrix { get; set; }
        public Size2 SomeSize2 { get; set; }
        public Size2F SomeSize2F { get; set; }
        public Size3 SomeSize3 { get; set; }
        public Size3F SomeSize3F { get; set; }
        public Circle SomeCircle { get; set; }
        public CircleF SomeCircleF { get; set; }
        public Rectangle SomeRectangle { get; set; }
        public RectangleF SomeRectangleF { get; set; }
        public System.Drawing.Rectangle SomeGDIRect { get; set; }
        public List<Object> Array { get; set; }
        public StringResource StringResource { get; set; }
    }
}
