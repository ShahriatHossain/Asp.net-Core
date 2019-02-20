using System.Net;

namespace Core.Models
{
    public class ApiError
    {
        public ApiError(HttpStatusCode status, string title, string details = null)
        {
            this.StatusCode = status;
            this.Title = title;
            this.Details = details;
        }


        public HttpStatusCode StatusCode { get; }

        public string Title { get; }

        public string Details { get; }

    }
}
