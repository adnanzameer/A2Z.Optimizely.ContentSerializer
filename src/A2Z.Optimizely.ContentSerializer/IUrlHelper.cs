using EPiServer;
using EPiServer.Core;

namespace A2Z.Optimizely.ContentSerializer
{
    public interface IUrlHelper
    {
        string ContentUrl(Url url);
        string ContentUrl(Url url, IUrlSettings urlSettings);
        string ContentUrl(ContentReference contentReference);
        string ContentUrl(ContentReference contentReference, IUrlSettings contentReferenceSettings);
    }
}