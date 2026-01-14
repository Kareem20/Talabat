
namespace Talabat.APIs.Errors
{
    public class ApiError
    {
        public string? Message { get; set; }
        public int StatusCode { get; set; }

        public ApiError(int StatusCode, string? Message = null)
        {
            this.StatusCode = StatusCode;
            this.Message = Message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "Bad Request",
                401 => "Unauthorized",
                403 => "Forbidden",
                404 => "Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
