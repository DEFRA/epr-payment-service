using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.DataModels.BaseClasses
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseEntity
    {
        public int Id { get; set; }
    }
}
