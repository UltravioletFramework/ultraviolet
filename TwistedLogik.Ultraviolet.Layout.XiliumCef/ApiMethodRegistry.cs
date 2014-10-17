using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using TwistedLogik.Nucleus;
using TwistedLogik.Nucleus.Text;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents a single layout's registered API methods.
    /// </summary>
    internal sealed partial class ApiMethodRegistry
    {
        /// <summary>
        /// Invokes the API method with the specified name.
        /// </summary>
        /// <param name="method">The name of the API method to invoke.</param>
        /// <param name="parameters">A JSON object containing the method's parameters.</param>
        /// <returns>The method's return value.</returns>
        public JObject InvokeApiMethod(StringSegment method, JObject parameters = null)
        {
            // Retrieve the specified API call's method metadata.
            ApiMethodMetadata metadata;
            if (!registeredApiMethods.TryGetValue(method, out metadata))
                throw new ApiBindingFailureException(XiliumStrings.UnableToBindApiCall.Format(method));

            // If the script method has no parameters, we can take an easier path
            // that skips the JSON deserialization.  Otherwise, we have to go through the whole thing.
            var returnValue = default(Object);
            if (parameters != null)
            {
                // Retrieve the method parameter lists from the JSON and the metadata and make sure they match.
                var parametersFromJson = parameters.Properties().ToDictionary(x => x.Name);
                var parametersFromMeta = metadata.MethodParameters;
                if (parametersFromMeta.Count() != parametersFromJson.Count())
                    throw new ApiBindingFailureException(XiliumStrings.UnableToBindApiCall.Format(method));

                // Deserialize the parameter values from the JSON data.
                var parameterValues = new List<Object>();
                foreach (var parameter in parametersFromMeta)
                {
                    JProperty jsonProperty;
                    if (!parametersFromJson.TryGetValue(parameter.Key, out jsonProperty))
                        throw new ApiBindingFailureException(XiliumStrings.UnableToBindApiCall.Format(method));

                    var parameterValue = jsonProperty.Value.ToObject(parameter.Value.ParameterType);
                    parameterValues.Add(parameterValue);
                }

                // Invoke the requested API method.
                var parameterValuesArray = parameterValues.ToArray();
                returnValue = metadata.MethodInfo.Invoke(metadata.Target, parameterValuesArray);
            }
            else
            {
                if (metadata.MethodParameters.Count > 0)
                    throw new ApiBindingFailureException(XiliumStrings.UnableToBindApiCall.Format(method));

                returnValue = metadata.MethodInfo.Invoke(metadata.Target, null);
            }
            return (returnValue == null) ? null : new JObject(new JProperty("ReturnValue", JToken.FromObject(returnValue)));
        }

        /// <summary>
        /// Registers a static API method.
        /// </summary>
        /// <param name="method">The name of the method to register.</param>
        /// <param name="name">The name under which the method can be invoked by a layout script.</param>
        public void RegisterApiMethod<T>(String method, String name)
        {
            Contract.RequireNotEmpty(method, "method");

            var minfo = typeof(T).GetMethod(method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            if (minfo == null)
                throw new MissingMethodException();

            registeredApiMethods[name ?? method] = new ApiMethodMetadata(name ?? method, minfo, null);
        }

        /// <summary>
        /// Registers an API method.
        /// </summary>
        /// <param name="method">The name of the method to register.</param>
        /// <param name="name">The name under which the method can be invoked by a layout script.</param>
        /// <param name="target">The object on whic hthe method will be invoked.</param>
        public void RegisterApiMethod<T>(String method, String name, T target) where T : class
        {
            Contract.RequireNotEmpty(method, "method");
            Contract.Require(target, "target");

            var minfo = typeof(T).GetMethod(method, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (minfo == null)
                throw new MissingMethodException();

            registeredApiMethods[name ?? method] = new ApiMethodMetadata(name ?? method, minfo, target);
        }

        // Registered API methods hashed by invocation name.
        private readonly Dictionary<StringSegment, ApiMethodMetadata> registeredApiMethods = 
            new Dictionary<StringSegment, ApiMethodMetadata>();
    }
}
