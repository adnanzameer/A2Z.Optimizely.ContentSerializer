using System.Reflection;
using EPiServer;
using EPiServer.Core;
using EPiServer.Shell.ObjectEditing;
using A2Z.Optimizely.ContentSerializer.Internal;
using A2Z.Optimizely.ContentSerializer.Internal.Default;
using A2Z.Optimizely.ContentSerializer.Tests.Pages;

namespace A2Z.Optimizely.ContentSerializer.Tests.SelectStrategies
{
    public class CustomSelectOneStrategy : ISelectOneStrategy
    {
        private static readonly IReadOnlyDictionary<Type, Dictionary<string, bool>> CustomProperties =
            new Dictionary<Type, Dictionary<string, bool>>
            {
                {
                    typeof(StringPropertyHandlerPage), new Dictionary<string, bool>
                    {
                        {nameof(StringPropertyHandlerPage.SelectedOnlyOne), false},
                        {nameof(StringPropertyHandlerPage.SelectedOnlyValueOnlyOne), true}
                    }
                }
            };

        private readonly ISelectOneStrategy _defaultSelectOneStrategy;
        public CustomSelectOneStrategy()
        {
            this._defaultSelectOneStrategy = new DefaultSelectStrategy();
        }

        public object Execute(PropertyInfo property, IContentData contentData, ISelectionFactory selectionFactory)
        {
            var result = (IEnumerable<SelectOption>)this._defaultSelectOneStrategy.Execute(property, contentData, selectionFactory);
            var type = contentData.GetOriginalType();
            if (IsCustomContentType(type, property.Name))
            {
                var onlyValue = CustomProperties[type][property.Name];
                if (onlyValue)
                {
                    return result.FirstOrDefault(x => x.Selected)?.Value;
                }

                return result.FirstOrDefault(x => x.Selected);
            }

            return result;
        }

        private static bool IsCustomContentType(Type type, string propertyName)
        {
            return CustomProperties.ContainsKey(type) && CustomProperties[type].ContainsKey(propertyName);
        }
    }
}
