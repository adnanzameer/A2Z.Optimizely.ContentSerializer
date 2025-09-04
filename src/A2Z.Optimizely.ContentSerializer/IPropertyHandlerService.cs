using System.Reflection;

namespace A2Z.Optimizely.ContentSerializer;

public interface IPropertyHandlerService
{
    object GetPropertyHandler(PropertyInfo property);
}