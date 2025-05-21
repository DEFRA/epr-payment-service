using EPR.Payment.Service.Common.Enums;

namespace EPR.Payment.Service.Validations.Common
{
    public static class AccreditationRequestTypeValidationHelper
    {
        public static readonly List<Group> ValidRequestTypes = new List<Group> { Group.Exporters, Group.Reprocessors };

        public static bool IsValidRequestType(Group group)
        {
            return ValidRequestTypes.Contains(group);
        }
    }
}
