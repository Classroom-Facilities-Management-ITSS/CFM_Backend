using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ClassroomManagerAPI.Common
{
    public class Response<T> : BadResponse
    {
        public T? Data { get; set; }

        public override IActionResult GetResult()
        {
            ObjectResult objectResult = new ObjectResult(this);
            if (!StatusCode.HasValue)
            {
                if (base.IsOk)
                {
                    objectResult.StatusCode = (Data == null) ? (int) HttpStatusCode.NoContent : (int) HttpStatusCode.OK;
                }
                else
                {
                    objectResult.StatusCode = (int) HttpStatusCode.InternalServerError;
                }

                return objectResult;
            }

            objectResult.StatusCode = base.StatusCode;
            return objectResult;
        }
    }
}
