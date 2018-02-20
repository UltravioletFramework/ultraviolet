using System.Linq;
using System;
using System.IO;
using System.Reflection;
using Ultraviolet.Platform;

namespace Ultraviolet.Presentation
{
    partial class PresentationFoundation
    {
        partial void LoadCompiledExpressions_Android(ref Assembly asm)
        {
            var activity = AndroidActivityService.Create().Activity;

            var path = String.Empty;
            var list = default(String[]);

            list = activity.Assets.List(String.Empty);
            if (list.Contains(CompiledExpressionsAssemblyName))
                path = CompiledExpressionsAssemblyName;
            else
            {
                list = activity.Assets.List("Presentation");
                if (list.Contains(CompiledExpressionsAssemblyName))
                    path = Path.Combine("Presentation", CompiledExpressionsAssemblyName);
            }

            if (String.IsNullOrEmpty(path))
                return;

            using (var stream = activity.Assets.Open(path))
            {
                var data = default(Byte[]);

                using (var memstr = new MemoryStream())
                {
                    var buffer = new Byte[4096];
                    while (stream.IsDataAvailable())
                    {
                        var read = stream.Read(buffer, 0, buffer.Length);
                        memstr.Write(buffer, 0, read);
                    }
                    data = memstr.ToArray();
                }

                asm = Assembly.Load(data);
            }
        }
    }
}