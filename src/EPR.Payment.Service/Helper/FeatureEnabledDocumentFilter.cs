using Microsoft.FeatureManagement.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EPR.Payment.Service.Helper
{
    public class FeatureEnabledDocumentFilter : IDocumentFilter
    {
        private readonly IFeatureManager _featureManager;


        public FeatureEnabledDocumentFilter(IFeatureManager featureManager)
        {
            _featureManager = featureManager;
        }

        public async void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var pathsToRemove = new List<string>();

            foreach (var path in swaggerDoc.Paths)
            {
                Console.WriteLine($"Checking path: {path.Key}");
                foreach (var operation in path.Value.Operations)
                {
                    var apiDescription = context.ApiDescriptions.FirstOrDefault(desc => desc.RelativePath!.Equals(path.Key.Trim('/'), StringComparison.InvariantCultureIgnoreCase));
                    if (apiDescription != null)
                    {
                        var controllerActionDescriptor = apiDescription.ActionDescriptor as Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor;
                        if (controllerActionDescriptor != null)
                        {
                            var controllerTypeInfo = controllerActionDescriptor.ControllerTypeInfo;
                            var controllerFeatureGate = controllerTypeInfo.GetCustomAttributes(typeof(FeatureGateAttribute), true).Cast<FeatureGateAttribute>().FirstOrDefault();
                            if (controllerFeatureGate != null)
                            {
                                var controllerFeatures = controllerFeatureGate.Features;
                                var isControllerEnabled = await AreAllFeaturesEnabled(controllerFeatures);
                                if (!isControllerEnabled)
                                {
                                    Console.WriteLine($"Controller feature '{string.Join(", ", controllerFeatures)}' is disabled, removing path: {path.Key}");
                                    pathsToRemove.Add(path.Key);
                                    break;
                                }
                            }

                            var actionFeatureGate = apiDescription.ActionDescriptor.EndpointMetadata.OfType<FeatureGateAttribute>().FirstOrDefault();
                            if (actionFeatureGate != null)
                            {
                                var actionFeatures = actionFeatureGate.Features;
                                var isActionEnabled = await AreAllFeaturesEnabled(actionFeatures);
                                if (!isActionEnabled)
                                {
                                    Console.WriteLine($"Action feature '{string.Join(", ", actionFeatures)}' is disabled, removing path: {path.Key}");
                                    pathsToRemove.Add(path.Key);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            foreach (var path in pathsToRemove)
            {
                Console.WriteLine($"Removing path '{path}' from Swagger documentation because the feature gate is disabled.");
                swaggerDoc.Paths.Remove(path);
            }
        }

        private async Task<bool> AreAllFeaturesEnabled(IEnumerable<string> features)
        {
            foreach (var feature in features)
            {
                if (!await _featureManager.IsEnabledAsync(feature))
                {
                    Console.WriteLine($"Feature '{feature}' is disabled.");
                    return false;
                }
            }
            return true;
        }
    }
}
