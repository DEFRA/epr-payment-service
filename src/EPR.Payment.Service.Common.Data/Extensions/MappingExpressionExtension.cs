using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.Data.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class MappingExpressionExtension
    {
        public static IMappingExpression<TSource, TDestination> MapOnlyNonDefault<TSource, TDestination>(this IMappingExpression<TSource, TDestination> mappingExpression)
        {
            mappingExpression.ForAllMembers(opt => opt.Condition((src, dest, sourceProp, destinationProp, res) =>
            {
                return sourceProp != null && !sourceProp.IsDefaultValue();
            }));

            return mappingExpression;
        }

        public static bool IsDefaultValue(this object value)
        {
            if (value == null)
                return true;

            Type type = value.GetType();

            // if it's a boolean value, then we'll have to allow it through,
            // otherwise false never gets mapped
            if (type == typeof(bool))
                return false;

            object defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;
            return value.Equals(defaultValue);
        }
    }
}