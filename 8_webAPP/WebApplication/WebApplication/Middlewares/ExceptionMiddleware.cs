using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleException(httpContext, e);
            }
        }

        private async Task HandleException(HttpContext httpContext, Exception ex)
        {
          //   httpContext.Response
          // logika zapisywania bledu do pliku logs.txt
          var pathOut = "Output/log.txt";
          var log = new [] 
              {"***************** " + DateTime.Now + " *****************", 
               httpContext.Response + "\n",
               ex + "" 
              };
          
          await File.AppendAllLinesAsync(pathOut, log);

        }
        
        
    }
}