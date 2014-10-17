using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using TwistedLogik.Nucleus.Text;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents a scheme handler which exposes registered API methods as AJAX calls to
    /// the http://api/ domain using JSON to pass arguments.
    /// </summary>
    internal sealed class ApiSchemeHandler : CefResourceHandler
    {
        /// <summary>
        /// Initializes a new instance of the ApiSchemeHandler class.
        /// </summary>
        /// <param name="registry">The API method registry that exposes the layout's API calls.</param>
        public ApiSchemeHandler(ApiMethodRegistry registry)
        {
            this.registry = registry;
        }

        /// <summary>
        /// Gets a value indicating whether the specified cookie can be sent with the request.
        /// </summary>
        protected override Boolean CanGetCookie(CefCookie cookie)
        {
            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the specified cookie is returned with the response.
        /// </summary>
        protected override Boolean CanSetCookie(CefCookie cookie)
        {
            return false;
        }

        /// <summary>
        /// Indicates that request processing has been canceled.
        /// </summary>
        protected override void Cancel()
        {

        }

        /// <summary>
        /// Retrieves response header information.
        /// </summary>
        protected override void GetResponseHeaders(CefResponse response, out Int64 responseLength, out String redirectUrl)
        {
            var headers = new NameValueCollection();
            response.SetHeaderMap(headers);

            response.Status = responseStatusCode;
            response.StatusText = responseStatusText;

            responseLength = (responseData == null) ? 0 : responseData.Length;
            redirectUrl = null;
        }

        /// <summary>
        /// Begins processing a request.
        /// </summary>
        protected override Boolean ProcessRequest(CefRequest request, CefCallback callback)
        {
            // Determine which API method is being invoked.
            var methodPrefix = "http://api/";
            var methodName = new StringSegment(request.Url, methodPrefix.Length, request.Url.Length - methodPrefix.Length);
            var methodParameters = default(JObject);

            // Build the method's parameter list from the POSTed JSON values.
            if (request.PostData != null && request.PostData.Count > 0)
            {
                var methodParamsElement = request.PostData.GetElements().First();
                var methodParamsJsonRaw = Encoding.UTF8.GetString(methodParamsElement.GetBytes());
                methodParameters = JObject.Parse(methodParamsJsonRaw);
            }

            // Attempt to bind and invoke the requested method.
            try
            {
                var returnValue = registry.InvokeApiMethod(methodName, methodParameters);
                responseData = (returnValue == null) ? null : Encoding.UTF8.GetBytes(returnValue.ToString());
                responsePosition = 0;
                responseStatusCode = 200;
                responseStatusText = String.Empty;
            }
            catch (ApiBindingFailureException e)
            {
                responseData = null;
                responsePosition = 0;
                responseStatusCode = 500;
                responseStatusText = e.ToString();
            }
            callback.Continue();
            return true;
        }

        /// <summary>
        /// Reads response data.
        /// </summary>
        protected override Boolean ReadResponse(Stream response, Int32 bytesToRead, out Int32 bytesRead, CefCallback callback)
        {
            if (bytesToRead == 0 || responsePosition >= responseData.Length)
            {
                bytesRead = 0;
                return false;
            }
            else
            {
                response.Write(responseData, responsePosition, bytesToRead);
                responsePosition += bytesToRead;
                bytesRead = bytesToRead;
                return true;
            }
        }

        // The API method registry that contains the layout's registered API calls.
        private readonly ApiMethodRegistry registry;

        // The response data for the current request.
        private Byte[] responseData;
        private Int32 responsePosition;
        private Int32 responseStatusCode;
        private String responseStatusText;
    }
}
