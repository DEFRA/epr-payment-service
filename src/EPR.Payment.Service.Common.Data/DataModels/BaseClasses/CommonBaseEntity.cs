using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.BaseClasses
{
    [ExcludeFromCodeCoverage]
    public abstract class CommonBaseEntity : BaseEntity
    {
        public string Type { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
