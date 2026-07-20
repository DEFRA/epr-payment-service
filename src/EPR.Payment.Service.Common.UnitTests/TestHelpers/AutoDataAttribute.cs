using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EPR.Payment.Service.Common.UnitTests.TestHelpers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AutoDataAttribute : Attribute, ITestDataSource
    {
        private readonly Func<IFixture> _fixtureFactory;

        public AutoDataAttribute() : this(() => new Fixture()) { }

        public AutoDataAttribute(Func<IFixture> fixtureFactory)
        {
            _fixtureFactory = fixtureFactory ?? throw new ArgumentNullException(nameof(fixtureFactory));
        }

        public IEnumerable<object?[]> GetData(MethodInfo methodInfo)
        {
            ArgumentNullException.ThrowIfNull(methodInfo);

            var fixture = _fixtureFactory();
            var parameters = methodInfo.GetParameters();
            var specimens = new object?[parameters.Length];
            var context = new SpecimenContext(fixture);

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                foreach (var source in parameter.GetCustomAttributes(inherit: false)
                                                .OfType<IParameterCustomizationSource>())
                {
                    fixture.Customize(source.GetCustomization(parameter));
                }
                specimens[i] = context.Resolve(parameter);
            }

            yield return specimens;
        }

        public string? GetDisplayName(MethodInfo methodInfo, object?[]? data) => null;
    }
}
