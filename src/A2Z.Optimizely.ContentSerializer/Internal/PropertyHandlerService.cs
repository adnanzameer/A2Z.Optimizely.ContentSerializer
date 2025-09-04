using System;
using System.Reflection;
using A2Z.Optimizely.ContentSerializer.Attributes;
using EPiServer.ServiceLocation;

namespace A2Z.Optimizely.ContentSerializer.Internal;

public class PropertyHandlerService : IPropertyHandlerService
{
    private readonly Type _propertyHandlerType = typeof(IPropertyHandler<>);

    public object GetPropertyHandler(PropertyInfo property)
    {
        if (property == null)
        {
            return null;
        }
        var customPropertyHandlerAttribute = property.GetCustomAttribute<ContentSerializerPropertyHandlerAttribute>();
        if (customPropertyHandlerAttribute != null)
        {
            ServiceLocator.Current.TryGetExistingInstance(customPropertyHandlerAttribute.PropertyHandler, out var attributePropertyHandler);
            return attributePropertyHandler;
        }

        var propertyHandlerType = _propertyHandlerType.MakeGenericType(property.PropertyType);
        ServiceLocator.Current.TryGetExistingInstance(propertyHandlerType, out var propertyHandler);
        return propertyHandler;
    }
}