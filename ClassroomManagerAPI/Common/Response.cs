using ClassroomManagerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace ClassroomManagerAPI.Common
{
    public class Response<T> : BadResponse
    {
        public T? Data { get; set; }
        private bool AddPage {  get; set; } = false;
        private FilterModel Filter { get; set; } 
        private PaginationModel Pagination { get; set; }

        public void AddPagination(PaginationModel pagination)
        {
            AddPage = true;
            Pagination = pagination;
        }

        public void AddFilter(FilterModel filter)
        {
            filter.page = filter.page <= 0 ? 1 : filter.page;
            filter.limit = filter.limit <= 0 ? 10 : filter.limit;
            Filter = filter;
        }
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
            if (AddPage)
            {
                objectResult.Value = new { StatusCode, IsOk, Filter, Data, Message, Pagination };
            }
            return objectResult;
        }
    }
}
