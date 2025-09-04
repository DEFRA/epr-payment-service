namespace EPR.Payment.Service.Constants
{
    public static class LogMessages
    {
        public const string ConditionalEndpointFeatureGateEvaluation = "Evaluating feature gate for {controllerActionDescriptor.ControllerName}.{controllerActionDescriptor.ActionName}";
        public const string ConditionalEndpointFeatureGateEnabled = "Feature '{featureName}' is enabled: {isEnabled}";
        public const string ConditionalEndpointFeatureGateDisabled = "Feature '{featureName}' is disabled. Returning 404.";
    }
}
