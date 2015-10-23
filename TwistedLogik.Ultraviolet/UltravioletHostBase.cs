using System;
using System.Collections.Generic;
using System.Text;

namespace TwistedLogik.Ultraviolet
{
    public abstract class UltravioletHostBase
    {
        protected UltravioletContext Foo(Func<UltravioletContext> uv)
        {
            try
            {
                return uv();
            }
            catch
            {
                var current = UltravioletContext.RequestCurrent();
                if (current != null)
                {
                    current.Dispose();
                }
                throw;
            }
        }

    }
}
