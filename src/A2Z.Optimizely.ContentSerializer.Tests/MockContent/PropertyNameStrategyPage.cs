using A2Z.Optimizely.ContentSerializer.Attributes;
using EPiServer.Core;

namespace A2Z.Optimizely.ContentSerializer.Tests.MockContent;

public class PropertyNameStrategyPage : PageData
{
    public virtual string Heading { get; set; }

    [ContentSerializerName("customAuthor")]
    public virtual string Author { get; set; }
}