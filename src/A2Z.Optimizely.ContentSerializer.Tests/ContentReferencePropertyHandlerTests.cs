using A2Z.Optimizely.ContentSerializer.Internal.Default;
using EPiServer.Core;
using NSubstitute;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests;

public class ContentReferencePropertyHandlerTests
{
    private readonly ContentReferencePropertyHandler _sut;
    private readonly IUrlHelper _urlHelper;
    private IContentSerializerSettings _contentSerializerSettings;

    public ContentReferencePropertyHandlerTests()
    {
        _contentSerializerSettings = Substitute.For<IContentSerializerSettings>();
        _contentSerializerSettings.UrlSettings = new UrlSettings();
        _urlHelper = Substitute.For<IUrlHelper>();
        _sut = new ContentReferencePropertyHandler(_urlHelper, _contentSerializerSettings);
    }

    [Fact]
    public void GivenNullContentReference_WhenHandle_ThenReturnsNull()
    {
        var result = _sut.Handle(null, null, null);

        result.ShouldBeNull();
    }

    [Fact]
    public void GivenEmptyContentReference_WhenHandle_ThenReturnsNull()
    {
        var result = _sut.Handle(ContentReference.EmptyReference, null, null);

        result.ShouldBeNull();
    }

    [Fact]
    public void GivenContentReference_WhenHandle_ThenReturnsAbsoluteUrlString()
    {
        var host = "example.com";
        var scheme = "https://";
        var baseUrl = $"{scheme}{host}";
        var prettyPath = "/any-path/to/page/?anyQueryParam=value&anotherQuery";
        var contentReference = new ContentReference(1000);
        _urlHelper.ContentUrl(contentReference, _contentSerializerSettings.UrlSettings)
            .Returns($"{baseUrl}{prettyPath}");

        var result = _sut.Handle(contentReference, null, null);

        result.ShouldBe($"{baseUrl}{prettyPath}");
    }

    [Fact]
    public void GivenContentReference_WhenHandleWithUseAbsoluteUrlsSetToFalse_ThenReturnsRelativeUrlString()
    {
        var host = "example.com";
        var scheme = "https://";
        var baseUrl = $"{scheme}{host}";
        var prettyPath = "/any-path/to/page/?anyQueryParam=value&anotherQuery";
        var contentReference = new ContentReference(1000);
        _contentSerializerSettings.UrlSettings.Returns(new UrlSettings { UseAbsoluteUrls = false });
        _urlHelper.ContentUrl(Arg.Any<ContentReference>(), _contentSerializerSettings.UrlSettings)
            .Returns($"{baseUrl}{prettyPath}");

        var result = _sut.Handle(contentReference, null, null);

        result.ShouldBe(prettyPath);
    }
}