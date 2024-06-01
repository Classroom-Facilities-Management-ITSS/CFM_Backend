using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Common
{
    public class BadResponse
    {
        public int? StatusCode { get; set; }
        public string? Message { get; set; } = "Successfully";
        private string ErrorMessage { get; set; }
        public bool IsOk => string.IsNullOrEmpty(ErrorMessage);

        public virtual IActionResult GetResult()
        {
            ObjectResult objectResult = new ObjectResult(this);
            objectResult.StatusCode = !StatusCode.HasValue ? (int) HttpStatusCode.InternalServerError : StatusCode;
            return objectResult;
        }

        public void AddBadRequest(string message)
        {
            Message = ErrorMessage = message;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
