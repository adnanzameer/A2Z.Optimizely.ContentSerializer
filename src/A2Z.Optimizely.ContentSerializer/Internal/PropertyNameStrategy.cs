using System.Reflection;
using A2Z.Optimizely.ContentSerializer.Attributes;

namespace A2Z.Optimizely.ContentSerializer.Internal;

public class PropertyNameStrategy : IPropertyNameStrategy
{
    public string GetPropertyName(PropertyInfo property)
    {
        var nameAttribute = property.GetCustomAttribute<ContentSerializerNameAttribute>();
        return nameAttribute == null ? property.Name : nameAttribute.Name;
    }
}