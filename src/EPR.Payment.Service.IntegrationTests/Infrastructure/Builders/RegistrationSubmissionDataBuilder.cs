using EPR.Payment.Service.Common.Constants;
using EPR.Payment.Service.Common.Data.DataModels;

namespace EPR.Payment.Service.IntegrationTests.Infrastructure.Builders;

/// <summary>
/// Fluent construction of a <see cref="RegistrationSubmissionData"/> row - the shape the
/// RegistrationSubmittedForFeesCalculation consumer would already have persisted by the time a
/// SubmittedForRegulatorApproval message arrives. Tests seed this directly via EF rather than
/// replaying that earlier message.
/// </summary>
public sealed class RegistrationSubmissionDataBuilder(TestBuilders builders)
{
    private Guid _submissionId = Guid.NewGuid();
    private string _applicationReferenceNumber = $"PEPR{Guid.NewGuid():N}"[..15].ToUpperInvariant();
    private int _submissionPeriodId = SeededSubmissionPeriods.DirectLargeProducer2027;
    private Guid? _complianceSchemeId;
    private string _regulatorNation = "GB-ENG";
    private DateTime _submissionDate = DateTime.UtcNow;
    private string? _registrationBlobName;
    private readonly List<ProducerSpec> _producers = [];
    private readonly List<(string EventName, DateTime EventDate)> _events = [];

    public RegistrationSubmissionDataBuilder ForSubmissionId(Guid submissionId)
    {
        _submissionId = submissionId;
        return this;
    }

    public RegistrationSubmissionDataBuilder WithApplicationReferenceNumber(string applicationReferenceNumber)
    {
        _applicationReferenceNumber = applicationReferenceNumber;
        return this;
    }

    /// <summary>See <see cref="SeededSubmissionPeriods"/> for the available migration-seeded windows.</summary>
    public RegistrationSubmissionDataBuilder InSubmissionPeriod(int submissionPeriodId)
    {
        _submissionPeriodId = submissionPeriodId;
        return this;
    }

    public RegistrationSubmissionDataBuilder ForComplianceScheme(Guid complianceSchemeId)
    {
        _complianceSchemeId = complianceSchemeId;
        return this;
    }

    public RegistrationSubmissionDataBuilder WithRegulatorNation(string regulatorNation)
    {
        _regulatorNation = regulatorNation;
        return this;
    }

    public RegistrationSubmissionDataBuilder SubmittedOn(DateTime submissionDate)
    {
        _submissionDate = submissionDate;
        return this;
    }

    public RegistrationSubmissionDataBuilder WithRegistrationBlobName(string registrationBlobName)
    {
        _registrationBlobName = registrationBlobName;
        return this;
    }

    public RegistrationSubmissionDataBuilder WithProducer(Action<ProducerSpec> configure)
    {
        var spec = new ProducerSpec();
        configure(spec);
        _producers.Add(spec);
        return this;
    }

    /// <summary>Adds a SubmittedForRegulatorApproval event row directly (bypassing the topic) -
    /// useful when a test needs a prior lifecycle event already on record before it publishes
    /// the message under test. For triggering snapshot creation itself, publish the real message
    /// via <see cref="ServiceBusMessagePublisher"/> instead so the handler under test actually runs.</summary>
    public RegistrationSubmissionDataBuilder WithEvent(string eventName, DateTime? eventDate = null)
    {
        _events.Add((eventName, eventDate ?? _submissionDate));
        return this;
    }

    public RegistrationSubmissionDataBuilder Rejected(DateTime? eventDate = null) =>
        WithEvent(RegistrationEventNames.RejectedByRegulator, eventDate);

    public async Task<BuiltRegistrationSubmissionData> Build()
    {
        var entity = new RegistrationSubmissionData
        {
            SubmissionId = _submissionId,
            ApplicationReferenceNumber = _applicationReferenceNumber,
            SubmissionPeriodId = _submissionPeriodId,
            ComplianceSchemeId = _complianceSchemeId,
            RegulatorNation = _regulatorNation,
            SubmissionDate = _submissionDate,
            CreatedDate = DateTimeOffset.UtcNow,
            RegistrationBlobName = _registrationBlobName ?? $"registrations/{Guid.NewGuid()}.csv",
        };

        foreach (var producerSpec in _producers)
        {
            entity.Producers.Add(producerSpec.ToEntity());
        }

        foreach (var (eventName, eventDate) in _events)
        {
            entity.Events.Add(new RegistrationSubmissionDataEvent
            {
                EventName = eventName,
                EventDate = eventDate,
                CreatedDate = DateTimeOffset.UtcNow,
            });
        }

        await builders.WithDbContextAsync(ctx =>
        {
            ctx.RegistrationSubmissionData.Add(entity);
            return Task.CompletedTask;
        }, save: true);

        return new BuiltRegistrationSubmissionData(entity);
    }

    /// <summary>Per-producer configuration, including its subsidiaries. Mirrors the shape of
    /// <see cref="Common.Data.DataModels.RegistrationSubmissionProducer"/> without exposing the
    /// entity itself, so callers can't accidentally attach a detached/tracked instance twice.</summary>
    public sealed class ProducerSpec
    {
        // RegistrationSubmissionProducer.OrganisationId is varchar(20) - shorter than a full Guid.
        internal string OrganisationId { get; private set; } = Guid.NewGuid().ToString("N")[..20];
        internal string OrganisationSize { get; private set; } = "Large";
        internal int NationId { get; private set; } = 1;
        internal bool IsOnlineMarketplace { get; private set; }
        internal bool IsClosedLoopRecycling { get; private set; }
        internal bool IsNewJoiner { get; private set; }
        internal List<SubsidiarySpec> Subsidiaries { get; } = [];

        public ProducerSpec WithOrganisationId(string organisationId)
        {
            OrganisationId = organisationId;
            return this;
        }

        public ProducerSpec AsLarge()
        {
            OrganisationSize = "Large";
            return this;
        }

        public ProducerSpec AsSmall()
        {
            OrganisationSize = "Small";
            return this;
        }

        public ProducerSpec AsOnlineMarketplace(bool isOnlineMarketplace = true)
        {
            IsOnlineMarketplace = isOnlineMarketplace;
            return this;
        }

        public ProducerSpec AsClosedLoopRecycling(bool isClosedLoopRecycling = true)
        {
            IsClosedLoopRecycling = isClosedLoopRecycling;
            return this;
        }

        public ProducerSpec AsNewJoiner(bool isNewJoiner = true)
        {
            IsNewJoiner = isNewJoiner;
            return this;
        }

        public ProducerSpec WithSubsidiary(Action<SubsidiarySpec>? configure = null)
        {
            var spec = new SubsidiarySpec();
            configure?.Invoke(spec);
            Subsidiaries.Add(spec);
            return this;
        }

        public ProducerSpec WithSubsidiaries(int count, Action<SubsidiarySpec>? configure = null)
        {
            for (var i = 0; i < count; i++)
            {
                WithSubsidiary(configure);
            }

            return this;
        }

        internal RegistrationSubmissionProducer ToEntity()
        {
            var producer = new RegistrationSubmissionProducer
            {
                OrganisationId = OrganisationId,
                OrganisationSize = OrganisationSize,
                NationId = NationId,
                IsOnlineMarketplace = IsOnlineMarketplace,
                IsClosedLoopRecycling = IsClosedLoopRecycling,
                IsNewJoiner = IsNewJoiner,
                CreatedDate = DateTimeOffset.UtcNow,
            };

            foreach (var subsidiary in Subsidiaries)
            {
                producer.Subsidiaries.Add(subsidiary.ToEntity());
            }

            return producer;
        }
    }

    public sealed class SubsidiarySpec
    {
        internal string SubsidiaryId { get; private set; } = Guid.NewGuid().ToString("N");
        internal bool IsOnlineMarketplace { get; private set; }
        internal bool IsClosedLoopRecycling { get; private set; }
        internal bool IsNewJoiner { get; private set; }

        public SubsidiarySpec AsOnlineMarketplace(bool isOnlineMarketplace = true)
        {
            IsOnlineMarketplace = isOnlineMarketplace;
            return this;
        }

        public SubsidiarySpec AsClosedLoopRecycling(bool isClosedLoopRecycling = true)
        {
            IsClosedLoopRecycling = isClosedLoopRecycling;
            return this;
        }

        public SubsidiarySpec AsNewJoiner(bool isNewJoiner = true)
        {
            IsNewJoiner = isNewJoiner;
            return this;
        }

        internal RegistrationSubmissionSubsidiary ToEntity() => new()
        {
            SubsidiaryId = SubsidiaryId,
            IsOnlineMarketplace = IsOnlineMarketplace,
            IsClosedLoopRecycling = IsClosedLoopRecycling,
            IsNewJoiner = IsNewJoiner,
            CreatedDate = DateTimeOffset.UtcNow,
        };
    }
}
