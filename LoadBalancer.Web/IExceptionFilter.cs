using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoadBalancer.Web
{
    /// <summary>
    /// Set HTTP code if exception is not handled.
    /// </summary>
    public class UnhandledExceptionCoverage : ExceptionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnException(ExceptionContext context)
        {
            if (context.ExceptionHandled) 
                return;
            
            context.Result = new StatusCodeResult(500);
            context.ExceptionHandled = true;
        }
    }
}