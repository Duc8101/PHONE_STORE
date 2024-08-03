using System.Net;

namespace Common.Base
{
    public class ResponseBase
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Data { get; set; }

        public ResponseBase(object data, string message, int code)
        {
            Data = data;
            Message = message;
            Code = code;
        }

        public ResponseBase(object data, string message)
        {
            Data = data;
            Message = message;
            Code = (int)HttpStatusCode.OK;
        }

        public ResponseBase(string message, int code)
        {
            Data = false;
            Message = message;
            Code = code;
        }

        public ResponseBase(object data)
        {
            Data = data;
            Code = (int)HttpStatusCode.OK;
        }

    }

}
