using System.Reflection;
using AutoFixture;

namespace EPR.Payment.Service.Common.UnitTests.TestHelpers
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class FrozenAttribute : Attribute, IParameterCustomizationSource
    {
        public ICustomization GetCustomization(ParameterInfo parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter);
            return new FreezingCustomization(parameter.ParameterType);
        }
    }
}
