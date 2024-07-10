using Microsoft.FeatureManagement.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EPR.Payment.Service.Helper
{
    public class FeatureEnabledDocumentFilter : IDocumentFilter
    {
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<FeatureEnabledDocumentFilter> _logger;

        public FeatureEnabledDocumentFilter(IFeatureManager featureManager, ILogger<FeatureEnabledDocumentFilter> logger)
        {
            _featureManager = featureManager;
            _logger = logger;
        }

        public async void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var pathsToRemove = new List<string>();

            foreach (var path in swaggerDoc.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    var apiDescription = context.ApiDescriptions.FirstOrDefault(desc => !string.IsNullOrEmpty (desc.RelativePath)  && desc.RelativePath.Equals(path.Key.Trim('/'), System.StringComparison.InvariantCultureIgnoreCase));

                    if (apiDescription != null)
                    {
                        var controllerActionDescriptor = apiDescription.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
                        if (controllerActionDescriptor != null)
                        {
                            // Check for controller-level feature gate
                            var controllerTypeInfo = controllerActionDescriptor.ControllerTypeInfo;
                            var controllerFeatureGate = controllerTypeInfo.GetCustomAttributes(typeof(FeatureGateAttribute), true).Cast<FeatureGateAttribute>().FirstOrDefault();

                            if (controllerFeatureGate != null)
                            {
                                var controllerFeatures = controllerFeatureGate.Features;
                                var isControllerEnabled = await AreAllFeaturesEnabled(controllerFeatures);

                                if (!isControllerEnabled)
                                {
                                    pathsToRemove.Add(path.Key);
                                    break;
                                }
                            }

                            // Check for action-level feature gate
                            var actionFeatureGate = apiDescription.ActionDescriptor.EndpointMetadata.OfType<FeatureGateAttribute>().FirstOrDefault();
                            if (actionFeatureGate != null)
                            {
                                var actionFeatures = actionFeatureGate.Features;
                                var isActionEnabled = await AreAllFeaturesEnabled(actionFeatures);

                                if (!isActionEnabled)
                                {
                                    pathsToRemove.Add(path.Key);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            // Remove collected paths
            foreach (var path in pathsToRemove)
            {
                _logger.LogInformation($"Removing path '{path}' from Swagger documentation because the feature gate is disabled.");
                swaggerDoc.Paths.Remove(path);
            }
        }

        private async Task<bool> AreAllFeaturesEnabled(IEnumerable<string> features)
        {
            foreach (var feature in features)
            {
                if (!await _featureManager.IsEnabledAsync(feature))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
