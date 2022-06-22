using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyFinance.Presentation.Filters
{
    internal sealed class ModelStateValidationFilter : ActionFilterAttribute
    {
        private readonly ILogger<ModelStateValidationFilter> _logger;

        public ModelStateValidationFilter(ILogger<ModelStateValidationFilter> logger)
            => _logger = logger;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controllername = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];

            if (!context.ModelState.IsValid)
            {
                _logger.LogWarning("{ControllerName}[{Action}] Invalid model data received", controllername, actionName);

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
