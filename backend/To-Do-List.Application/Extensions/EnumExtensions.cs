using System.Reflection;

namespace To_Do_List.Application.Extensions;

public static class EnumExtensions
{
    public static PropertyInfo[] GetEnumProperties(this Type type)
    {
        if (type is null)
            throw new ArgumentNullException(nameof(type));

        return type.GetProperties()
            .Where(prop => prop.PropertyType.IsEnum)
            .ToArray();
    }
}