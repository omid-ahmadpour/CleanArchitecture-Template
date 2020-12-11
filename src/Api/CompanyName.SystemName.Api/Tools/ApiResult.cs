using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Tools
{
    public class ApiResult : ApiResult<object>
    {
        public ApiResult(object data, int statusCode = StatusCodes.Status200OK, string[] errors = null) : base(data, statusCode, errors)
        {

        }
    }

    public class ApiResult<T> : IActionResult, IDisposable, IStatusCodeActionResult
    {
        public string[] Errors { get; set; }

        public T Data { get; set; }

        public int? StatusCode { get; set; }

        public ApiResult(T data, int statusCode = StatusCodes.Status200OK, string[] errors = null)
        {
            Data = data;
            StatusCode = statusCode;
            Errors = errors;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            await new OkObjectResult(this).ExecuteResultAsync(context);
        }

        public void Dispose()
        {
            if (Data != null && typeof(T).GetInterfaces().Contains(typeof(IDisposable)))
            {
                ((IDisposable)Data).Dispose();
            }
        }
    }
}