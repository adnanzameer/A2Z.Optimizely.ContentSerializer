using System.Reflection;
using EPiServer.Core;

namespace A2Z.Optimizely.ContentSerializer
{
    public interface IPropertyHandler<in T>
    {
        object Handle(T value, PropertyInfo property, IContentData contentData);
        object Handle(T value, PropertyInfo property, IContentData contentData, IContentSerializerSettings settings);
    }
}
