using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Tools
{
    public class PagedApiResult : PagedApiResult<object>
    {
        public PagedApiResult(object data, int? total = null, int statusCode = StatusCodes.Status200OK, string[] messages = null, string[] errors = null) : base(data, total, statusCode, messages, errors)
        {

        }
    }

    public class PagedApiResult<T> : IActionResult, IDisposable, IStatusCodeActionResult
    {
        public string[] Errors { get; set; }

        public T Data { get; set; }

        public string[] Messages { get; set; }

        public int? StatusCode { get; set; }
        public int? Total { get; set; }

        public PagedApiResult(T data, int? total = null, int statusCode = StatusCodes.Status200OK, string[] messages = null, string[] errors = null)
        {
            Data = data;
            StatusCode = statusCode;
            Messages = messages;
            Errors = errors;
            Total = total;
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
