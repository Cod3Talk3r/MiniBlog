using Microsoft.AspNetCore.Mvc.Filters;

namespace MiniBlog.Web.Filters
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Executing {Action} with args {@Args}",
                context.ActionDescriptor.DisplayName, context.ActionArguments);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Executed {Action}, Result: {ResultType}",
                context.ActionDescriptor.DisplayName, context.Result?.GetType().Name);
        }
    }
}