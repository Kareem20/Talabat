using System.Net;

namespace Talabat.APIs.Errors
{
    public class ValidationErrorResponse : ApiError
    {
        public IEnumerable<string> Errors { get; set; }
        public ValidationErrorResponse() : base((int)HttpStatusCode.BadRequest)
        {
            Errors = new List<string>();
        }
    }
}
