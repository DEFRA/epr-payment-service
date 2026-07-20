using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;

namespace EPR.Payment.Service.Common.UnitTests.TestHelpers
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class GreedyAttribute : Attribute, IParameterCustomizationSource
    {
        public ICustomization GetCustomization(ParameterInfo parameter)
        {
            ArgumentNullException.ThrowIfNull(parameter);
            return new ConstructorCustomization(parameter.ParameterType, new GreedyConstructorQuery());
        }
    }

    internal sealed class ConstructorCustomization : ICustomization
    {
        private readonly Type _targetType;
        private readonly IMethodQuery _query;

        public ConstructorCustomization(Type targetType, IMethodQuery query)
        {
            _targetType = targetType;
            _query = query;
        }

        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new FilteringSpecimenBuilder(
                new AutoFixture.Kernel.MethodInvoker(_query),
                new ExactTypeSpecification(_targetType)));
        }
    }
}
