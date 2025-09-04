using EPiServer.Core;
using A2Z.Optimizely.ContentSerializer.Attributes;

namespace A2Z.Optimizely.ContentSerializer.Tests.Pages
{
    public class PropertyNameStrategyPage : PageData
    {
        public virtual string Heading { get; set; }

        [ContentSerializerName("customAuthor")]
        public virtual string Author { get; set; }
    }
}
