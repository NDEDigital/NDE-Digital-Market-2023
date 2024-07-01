using System.Net;

namespace NDE_Digital_Market.Model
{
    public class APIResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string>? ErrorMessages { get; set; }
        public object? Result { get; set; }

    }
}
