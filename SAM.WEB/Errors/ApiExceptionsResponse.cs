using System;
using System.Collections.Generic;

namespace SAM.WEB.Errors
{
    public class ApiExceptionsResponse
    {

        public string StatusCode { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Details { get; set; }

        public ApiExceptionsResponse(string statusCode, string message = null, string details = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Details = details;
        }

        private string GetDefaultMessageForStatusCode(string statusCode)
        {
            return statusCode.ToLower() switch
            {
                //400 => "A bad request made",
                //401 => "You are not Authorized to make this request",
                //404 => "Resource it was not found",
                //500 => "Server side error",
                "internalservererror" => "Server side error",
                _ => null
            };
        }
    }
}