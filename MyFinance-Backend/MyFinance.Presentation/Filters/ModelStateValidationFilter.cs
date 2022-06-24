using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyFinance.Application.Services.RequestValidation;

namespace MyFinance.Presentation.Filters
{
    internal sealed class ModelStateValidationFilter : IAsyncActionFilter
    {
        private readonly ILogger<ModelStateValidationFilter> _logger;
        private readonly IRequestValidationService _requestValidationService;

        public ModelStateValidationFilter(
            ILogger<ModelStateValidationFilter> logger,
            IRequestValidationService requestValidationService)
            => (_logger, _requestValidationService) = (logger, requestValidationService);

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllername = context.RouteData.Values["controller"];
            var actionName = context.RouteData.Values["action"];
            var controllerInfo = string.Format("{0}[{1}]", controllername, actionName);

            foreach (var (_, value) in context.ActionArguments)
            {
                if (value is null || !_requestValidationService.IsValidatableRequest(value)) continue;

                var requestName = value.GetType().Name;
                _logger.LogInformation("{ControllerInfo} Validating {RequestName} data", controllerInfo, requestName);

                var (isSuccess, errors) = await _requestValidationService.ValidateRequest(value);
                if(!isSuccess)
                {
                    _logger.LogWarning("{ControllerInfo} Invalid {RequestName} data received", controllerInfo, requestName);
                    context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors!));
                    return;
                }

                _logger.LogInformation("{ControllerInfo} {RequestName} successfully validated", controllerInfo, requestName);
            }

            await next();
        }
    }
}
