using System.ComponentModel;

namespace EPR.Payment.Service.Common.UnitTests.Extensions
{
    public enum Test
    {
        [Description("First Value Description")]
        FirstValue,

        [Description("Second Value Description")]
        SecondValue,

        NoDescriptionValue
    }
}
