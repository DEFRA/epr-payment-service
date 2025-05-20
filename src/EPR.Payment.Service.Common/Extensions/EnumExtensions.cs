namespace EPR.Payment.Service.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            string description = enumValue.ToString();
            
            System.Reflection.FieldInfo? fieldInfo = enumValue.GetType().GetField(description);

            if (fieldInfo is not null)
            {
                object[] attributes = fieldInfo.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), true);
                
                if (attributes.Length > 0)
                {
                    description = ((System.ComponentModel.DescriptionAttribute)attributes[0]).Description;
                }
            }

            return description;
        }
    }
}
