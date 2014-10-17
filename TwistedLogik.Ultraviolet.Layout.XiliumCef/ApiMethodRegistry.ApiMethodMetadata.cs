using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TwistedLogik.Nucleus;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    partial class ApiMethodRegistry
    {
        /// <summary>
        /// Represents the metadata for a registered API method which can be invoked from layout scripts.
        /// </summary>
        internal struct ApiMethodMetadata
        {
            /// <summary>
            /// Initializes a new instance of the ApiMethodMetadata structure.
            /// </summary>
            /// <param name="name">The method's overridden name.</param>
            /// <param name="method">The method's reflection information.</param>
            /// <param name="target">The target object on which the method will be invoked.</param>
            public ApiMethodMetadata(String name, MethodInfo method, Object target)
            {
                Contract.RequireNotEmpty(name, "name");
                Contract.Require(method, "method");

                this.Target = target;
                this.Name = method.Name;
                this.MethodInfo = method;
                this.MethodParameters = method.GetParameters().ToDictionary(x => x.Name);
            }

            /// <summary>
            /// The target object on which the method will be invoked.
            /// </summary>
            public readonly Object Target;

            /// <summary>
            /// Gets the method's name.
            /// </summary>
            public readonly String Name;

            /// <summary>
            /// Gets the method's reflection information.
            /// </summary>
            public readonly MethodInfo MethodInfo;

            /// <summary>
            /// Gets the method's parameter dictionary.
            /// </summary>
            public readonly Dictionary<String, ParameterInfo> MethodParameters;
        }
    }
}
