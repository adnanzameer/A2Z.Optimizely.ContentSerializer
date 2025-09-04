using System.Reflection;

namespace A2Z.Optimizely.ContentSerializer
{
    public interface IPropertyNameStrategy
    {
        string GetPropertyName(PropertyInfo property);
    }
}
