using System;
using System.Net;

namespace CleanTemplate.Common
{
    public class CleanArchAppException : Exception
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public ApiResultStatusCode ApiStatusCode { get; set; }
        public object AdditionalData { get; set; }

        public CleanArchAppException()
            : this(ApiResultStatusCode.ServerError)
        {
        }

        public CleanArchAppException(ApiResultStatusCode statusCode)
            : this(statusCode, null)
        {
        }

        public CleanArchAppException(string message)
            : this(ApiResultStatusCode.ServerError, message)
        {
        }

        public CleanArchAppException(ApiResultStatusCode statusCode, string message)
            : this(statusCode, message, HttpStatusCode.InternalServerError)
        {
        }

        public CleanArchAppException(string message, object additionalData)
            : this(ApiResultStatusCode.ServerError, message, additionalData)
        {
        }

        public CleanArchAppException(ApiResultStatusCode statusCode, object additionalData)
            : this(statusCode, null, additionalData)
        {
        }

        public CleanArchAppException(ApiResultStatusCode statusCode, string message, object additionalData)
            : this(statusCode, message, HttpStatusCode.InternalServerError, additionalData)
        {
        }

        public CleanArchAppException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode)
            : this(statusCode, message, httpStatusCode, null)
        {
        }

        public CleanArchAppException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, object additionalData)
            : this(statusCode, message, httpStatusCode, null, additionalData)
        {
        }

        public CleanArchAppException(string message, Exception exception)
            : this(ApiResultStatusCode.ServerError, message, exception)
        {
        }

        public CleanArchAppException(string message, Exception exception, object additionalData)
            : this(ApiResultStatusCode.ServerError, message, exception, additionalData)
        {
        }

        public CleanArchAppException(ApiResultStatusCode statusCode, string message, Exception exception)
            : this(statusCode, message, HttpStatusCode.InternalServerError, exception)
        {
        }

        public CleanArchAppException(ApiResultStatusCode statusCode, string message, Exception exception, object additionalData)
            : this(statusCode, message, HttpStatusCode.InternalServerError, exception, additionalData)
        {
        }

        public CleanArchAppException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception)
            : this(statusCode, message, httpStatusCode, exception, null)
        {
        }

        public CleanArchAppException(ApiResultStatusCode statusCode, string message, HttpStatusCode httpStatusCode, Exception exception, object additionalData)
            : base(message, exception)
        {
            ApiStatusCode = statusCode;
            HttpStatusCode = httpStatusCode;
            AdditionalData = additionalData;
        }
    }
}
