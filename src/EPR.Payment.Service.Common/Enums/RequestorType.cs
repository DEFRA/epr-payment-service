using System.ComponentModel;
using System.Text.Json.Serialization;

namespace EPR.Payment.Service.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter<RequestorType>))]
    public enum RequestorType
    {
        [Description("Exporter")]
        Exporter = Group.Exporters,

        [Description("Reprocessor")]
        Reprocessor = Group.Reprocessors,
    }
}
