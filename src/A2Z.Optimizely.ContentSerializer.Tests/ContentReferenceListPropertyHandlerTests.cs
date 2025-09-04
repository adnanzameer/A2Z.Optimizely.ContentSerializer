using A2Z.Optimizely.ContentSerializer.Internal.Default;
using EPiServer.Core;
using NSubstitute;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests;

public class ContentReferenceListPropertyHandlerTests
{
    private readonly ContentReferenceListPropertyHandler _sut;
    private readonly IUrlHelper _urlHelper;
    private readonly IContentSerializerSettings _contentSerializerSettings;

    public ContentReferenceListPropertyHandlerTests()
    {
        _urlHelper = Substitute.For<IUrlHelper>();
        _contentSerializerSettings = Substitute.For<IContentSerializerSettings>();
        _contentSerializerSettings.UrlSettings = new UrlSettings();
        _sut = new ContentReferenceListPropertyHandler(
            new ContentReferencePropertyHandler(
                _urlHelper,
                _contentSerializerSettings),
            _contentSerializerSettings);
    }

    [Fact]
    public void GivenNullList_WhenHandle_ThenReturnsNull()
    {
        var result = _sut.Handle(null, null, null);

        result.ShouldBeNull();
    }

    [Fact]
    public void GivenEmptyList_WhenHandle_ThenReturnsEmptyList()
    {
        var contentReferences = Enumerable.Empty<ContentReference>();

        var result = _sut.Handle(contentReferences, null, null);

        ((IEnumerable<object>)result).ShouldBeEmpty();
    }

    [Fact]
    public void GivenContentReferences_WhenHandle_ThenReturnsEmptyList()
    {
        var host = "example.com";
        var scheme = "https://";
        var baseUrl = $"{scheme}{host}";
        var prettyPath = "/any-path/to/page/?anyQueryParam=value&anotherQuery";
        var contentReference = new ContentReference(1000);
        var contentReferences = new List<ContentReference>{contentReference};

        _urlHelper.ContentUrl(contentReference, _contentSerializerSettings.UrlSettings)
            .Returns($"{baseUrl}{prettyPath}");

        var result = _sut.Handle(contentReferences, null, null);
        var items = ((IEnumerable<object>)result).Cast<string>().ToList();

        items.Count.ShouldBe(1);
        items.First().ShouldBe($"{baseUrl}{prettyPath}");
    }
}