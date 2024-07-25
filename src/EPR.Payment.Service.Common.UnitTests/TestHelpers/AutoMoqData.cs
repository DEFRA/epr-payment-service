using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.MSTest;

namespace EPR.Payment.Service.Common.UnitTests.TestHelpers
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() =>
        {
            var fixture = new Fixture().Customize(new CompositeCustomization(
                new AutoMoqCustomization { ConfigureMembers = true },
                new SupportMutableValueTypesCustomization()));

            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(delegate (ThrowingRecursionBehavior b)
            {
                fixture.Behaviors.Remove((ISpecimenBuilderTransformation)(object)b);
            });
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        })
        { }
    }
}
