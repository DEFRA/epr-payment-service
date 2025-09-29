using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.FeatureManagement;
using System.Reflection;
using EPR.Payment.Service.Constants;

namespace EPR.Payment.Service.Helper
{
    public class ConditionalEndpointMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<ConditionalEndpointMiddleware> _logger;

        public ConditionalEndpointMiddleware(RequestDelegate next, IFeatureManager featureManager, ILogger<ConditionalEndpointMiddleware> logger)
        {
            _next = next;
            _featureManager = featureManager;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                if (controllerActionDescriptor != null)
                {
                    _logger.LogInformation(LogMessages.ConditionalEndpointFeatureGateEvaluation, controllerActionDescriptor.ControllerName, controllerActionDescriptor.ActionName);

                    var featureAttributes = controllerActionDescriptor.ControllerTypeInfo
                        .GetCustomAttributes<FeatureGateAttribute>(true)
                        .Union(controllerActionDescriptor.MethodInfo.GetCustomAttributes<FeatureGateAttribute>(true))
                        .ToList();

                    foreach (var featureAttribute in featureAttributes)
                    {
                        foreach (var featureName in featureAttribute.Features)
                        {
                            var isEnabled = await _featureManager.IsEnabledAsync(featureName);
                            _logger.LogInformation(LogMessages.ConditionalEndpointFeatureGateEnabled, featureName, isEnabled);

                            if (!isEnabled)
                            {
                                _logger.LogInformation(LogMessages.ConditionalEndpointFeatureGateDisabled , featureName);
                                context.Response.StatusCode = StatusCodes.Status404NotFound;
                                await context.Response.WriteAsync("Feature not available.");
                                return;
                            }
                        }
                    }
                }
            }

            await _next(context);
        }

    }
}
