using System.Collections.Generic;
using System.Reflection;
using EPiServer.Core;

namespace A2Z.Optimizely.ContentSerializer
{
    public interface IPropertyResolver
    {
        IEnumerable<PropertyInfo> GetProperties(IContentData contentData);
    }
}