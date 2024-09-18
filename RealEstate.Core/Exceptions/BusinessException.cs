using System.Net;

namespace RealEstate.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public HttpStatusCode Status { get; set; }
        public string Description { get; set; }

        public BusinessException()
        {
            Status = HttpStatusCode.BadRequest;
            Description = string.Empty;
        }

        public BusinessException(HttpStatusCode status, string description, string message) : base(message)
        {
            Status = status;
            Description = description;
        }
    }
}
