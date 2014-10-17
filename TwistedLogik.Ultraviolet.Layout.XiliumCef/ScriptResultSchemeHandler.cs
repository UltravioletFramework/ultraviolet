using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Xilium.CefGlue;

namespace TwistedLogik.Ultraviolet.Layout.XiliumCef
{
    /// <summary>
    /// Represents a scheme handler which allows layout scripts to asynchronously
    /// return values to the layout engine.
    /// </summary>
    internal sealed class ScriptResultSchemeHandler : CefResourceHandler
    {
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

            responseLength = 0;
            redirectUrl = null;
        }

        /// <summary>
        /// Begins processing a request.
        /// </summary>
        protected override Boolean ProcessRequest(CefRequest request, CefCallback callback)
        {
            try
            {
                // Ensure that the request is properly formed.
                if (request.PostData == null || request.PostData.Count == 0)
                    throw new InvalidOperationException(XiliumStrings.InvalidScriptResult);

                // Retrieve the JSON that contains the task result.
                var returnValueElement = request.PostData.GetElements().First();
                var returnValueJsonRaw = Encoding.UTF8.GetString(returnValueElement.GetBytes());
                var returnValueJson = JObject.Parse(returnValueJsonRaw);

                var taskIDToken = returnValueJson["TaskID"];
                if (taskIDToken == null)
                    throw new InvalidOperationException(XiliumStrings.InvalidScriptResult);

                var resultToken = returnValueJson["Result"];
                if (resultToken == null)
                    throw new InvalidOperationException(XiliumStrings.InvalidScriptResult);

                // Inform the asynchronous task coordinator that we've received a result.
                AsyncResultCoordinator.SetTaskResult(taskIDToken.ToObject<Int64>(), resultToken);
                responseStatusCode = 200;
                responseStatusText = String.Empty;
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException ||
                    e is InvalidCastException ||
                    e is ArgumentException)  
                {
                    responseStatusCode = 500;
                    responseStatusText = e.Message;
                }
                else
                {
                    throw;
                }
            }
            callback.Continue();
            return true;
        }

        /// <summary>
        /// Reads response data.
        /// </summary>
        protected override Boolean ReadResponse(Stream response, Int32 bytesToRead, out Int32 bytesRead, CefCallback callback)
        {
            bytesRead = 0;
            return false;
        }

        // The response data for the current request.
        private Int32 responseStatusCode;
        private String responseStatusText;
    }
}
