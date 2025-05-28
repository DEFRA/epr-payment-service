using System.ComponentModel;
using System.Text.Json.Serialization;
namespace EPR.Payment.Service.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<MaterialType>))]
    public enum MaterialType
    {
        [Description("Aluminium")]
        Aluminium = SubGroup.Aluminium,

        [Description("Glass")]
        Glass = SubGroup.Glass,

        [Description("Paper, board or fibre-based composite material")]
        PaperOrBoardOrFibreBasedCompositeMaterial = SubGroup.PaperOrBoardOrFibreBasedCompositeMaterial,

        [Description("Plastic")]
        Plastic = SubGroup.Plastic,

        [Description("Steel")]
        Steel = SubGroup.Steel,

        [Description("Wood")]
        Wood = SubGroup.Wood
    }
}
