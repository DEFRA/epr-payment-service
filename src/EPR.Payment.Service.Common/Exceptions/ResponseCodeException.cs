using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EPR.Payment.Service.Common.Exceptions
{
    public class ResponseCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ResponseCodeException(
            HttpStatusCode statusCode,
            string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public ResponseCodeException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
    }
}
