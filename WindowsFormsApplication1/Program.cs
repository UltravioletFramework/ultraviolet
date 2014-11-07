using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Vector2.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Vector3.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Vector4.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Color.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Matrix.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Size2.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Size2F.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Size3.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Size3F.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Circle.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\CircleF.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Rectangle.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\RectangleF.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Ultraviolet.Design\Radians.xml");

            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Nucleus.Design\MaskedUInt32.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Nucleus.Design\MaskedUInt64.xml");
            TwistedLogik.Nucleus.Design.TypeDescriptionMetadataLoader.Load(@"..\..\..\TwistedLogik.Nucleus.Design\Text\StringResource.xml");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
