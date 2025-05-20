using System.ComponentModel;

namespace EPR.Payment.Service.Common.Data.Enums
{
    public enum SubGroup
    {
        [Description("Large producer")]
        Large = 1,
        
        [Description("Small producer")]
        Small = 2,

        [Description("Registration")]
        Registration = 3,

        [Description("Online Market")]
        Online = 4,

        [Description("Up to 20")]
        UpTo20 = 5,

        [Description("More than 20")]
        MoreThan20 = 6,

        [Description("Re-submitting a report")]
        ReSubmitting = 7,

        [Description("Late Fee")]
        LateFee = 8,

        [Description("Aluminium")]
        Aluminium = 9,

        [Description("Glass")]
        Glass = 10,

        [Description("Paper, board or fibre-based composite material")]
        PaperOrBoardOrFibreBasedCompositeMaterial = 11,

        [Description("Plastic")]
        Plastic = 12,

        [Description("Steel")]
        Steel = 13,

        [Description("Wood")]
        Wood = 14
    }
}
