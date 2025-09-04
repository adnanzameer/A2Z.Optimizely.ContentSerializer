using System.Collections.Generic;
using EPiServer.Core;

namespace A2Z.Optimizely.ContentSerializer
{
    public interface IPropertyManager
    {
        Dictionary<string, object> GetStructuredData(
            IContentData contentData,
            IContentSerializerSettings contentSerializerSettings);
    }
}
