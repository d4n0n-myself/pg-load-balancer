using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoadBalancer.Web
{
    public class UnhandledExceptionCoverage : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled) 
                return;
            
            context.Result = new StatusCodeResult(500);
            context.ExceptionHandled = true;
        }
    }
}