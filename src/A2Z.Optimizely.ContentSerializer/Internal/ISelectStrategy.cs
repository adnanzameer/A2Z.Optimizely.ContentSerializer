using System.Reflection;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;

namespace A2Z.Optimizely.ContentSerializer.Internal
{
    public interface ISelectStrategy
    {
        object Execute(PropertyInfo property, IContentData contentData, ISelectionFactory selectionFactory);
    }
}
