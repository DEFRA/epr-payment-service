using EPR.Payment.Service.Common.Data.Interfaces.Repositories.RegistrationSubmission;
using EPR.Payment.Service.Common.Services.Interfaces.RegistrationSubmission;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.ComplianceScheme;
using EPR.Payment.Service.Services.Interfaces.RegistrationFees.Producer;
using EPR.Payment.Service.Services.Migration.RegistrationHistory;
using EPR.Payment.Service.Services.RegistrationSubmission;
using EPR.Payment.Service.Services.RegistrationSubmission.Csv;
using EPR.Payment.Service.Services.RegistrationSubmission.Storage;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace EPR.Payment.Service.UnitTests.Services.Migration.RegistrationHistory
{
    [TestClass]
    public class RegistrationReplayHandlerFactoryTests
    {
        [TestMethod]
        public void Create_BuildsLiveHandlersFromTheScopedProvider()
        {
            var decisionHandler = Mock.Of<IRegulatorRegistrationDecisionHandler>();
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(Mock.Of<IRegistrationSubmissionDataRepository>());
            services.AddSingleton(Mock.Of<IRegistrationSubmissionDataEventRepository>());
            services.AddSingleton(Mock.Of<IRegistrationFeeSnapshotRepository>());
            services.AddSingleton(Mock.Of<IBlobReader>());
            services.AddSingleton(Mock.Of<ICsvStreamParser>());
            services.AddSingleton(Mock.Of<IProducerFeesCalculatorService>());
            services.AddSingleton(Mock.Of<IComplianceSchemeCalculatorService>());
            services.AddSingleton(decisionHandler);
            using var provider = services.BuildServiceProvider();

            var handlers = new RegistrationReplayHandlerFactory().Create(provider, new ReplayTimeProvider());

            using (new AssertionScope())
            {
                handlers.SubmissionData.Should().BeOfType<RegistrationSubmissionDataHandler>();
                handlers.ApplicationSubmitted.Should().BeOfType<RegistrationSubmittedForRegulatorApprovalHandler>();
                handlers.Decision.Should().BeSameAs(decisionHandler);
            }
        }

        [TestMethod]
        public void Create_NullArguments_Throw()
        {
            var sut = new RegistrationReplayHandlerFactory();

            using (new AssertionScope())
            {
                ((Action)(() => sut.Create(null!, new ReplayTimeProvider()))).Should().Throw<ArgumentNullException>();
                ((Action)(() => sut.Create(Mock.Of<IServiceProvider>(), null!))).Should().Throw<ArgumentNullException>();
            }
        }
    }
}
