using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyFinance.Presentation.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ValidateModelStateAttribute> _logger;

        public ValidateModelStateAttribute(ILogger<ValidateModelStateAttribute> logger)
            => _logger = logger;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllername = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];

            if (!context.ModelState.IsValid)
            {
                _logger.LogWarning("{ControllerName}[{Action}] Invalid model data received", controllername, actionName); ;

                var errors = context.ModelState.Values
                    .SelectMany(error => error.Errors)
                    .Select(error => error.ErrorMessage);

                context.Result = new BadRequestObjectResult(new ValidationProblemDetails(new Dictionary<string, string[]>
                {
                    { "Messages", errors.ToArray() }
                }));
            }
        }
    }
}
