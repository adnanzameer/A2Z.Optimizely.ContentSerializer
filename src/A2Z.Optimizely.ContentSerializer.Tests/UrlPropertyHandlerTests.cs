using A2Z.Optimizely.ContentSerializer.Internal.Default;
using EPiServer;
using NSubstitute;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests;

public class UrlPropertyHandlerTests
{
    private readonly IContentSerializerSettings _contentSerializerSettings;
    private readonly UrlPropertyHandler _sut;
    private readonly IUrlHelper _urlHelper;
        
    public UrlPropertyHandlerTests()
    {
        _contentSerializerSettings = new ContentSerializerSettings();
        _urlHelper = Substitute.For<IUrlHelper>();
        _sut = new UrlPropertyHandler(_urlHelper, _contentSerializerSettings);
    }

    [Fact]
    public void GivenNullUrl_WhenHandle_ThenReturnsNull()
    {
        var result = _sut.Handle(null, null, null);

        result.ShouldBeNull();
    }

    [Fact]
    public void GivenMailToUrl_WhenHandle_ThenReturnsCorrectValue()
    {
        var value = "mailto:mail@example.com";
        var url = new Url(value);

        var result = _sut.Handle(url, null, null);

        result.ShouldBe(value);
    }

    [Fact]
    public void GivenExternalLink_WhenHandle_ThenReturnsAbsoluteUrlWithQuery()
    {
        var value = "https://josef.guru/example/page?anyQueryString=true&anyOtherQuery";
        var url = new Url(value);

        var result = _sut.Handle(url, null, null);

        result.ShouldBe(value);
    }

    [Fact]
    public void GivenExternalLink_WhenHandleWithAbsoluteUrlSetToFalse_ThenReturnsRelativeUrlWithQuery()
    {
        _contentSerializerSettings.UrlSettings = new UrlSettings {UseAbsoluteUrls = false};
        var value = "https://josef.guru/example/page?anyQueryString=true&anyOtherQuery";
        var url = new Url(value);

        var result = _sut.Handle(url, null, null);

        result.ShouldBe(url.PathAndQuery);
    }

    [Fact]
    public void GivenEpiserverPage_WhenHandle_ThenReturnsRewrittenPrettyAbsoluteUrl()
    {
        var siteUrl = "https://example.com";
        var prettyPath = "/rewritten/pretty-url/";
        var value = "/link/d40d0056ede847d5a2f3b4a02778d15b.aspx";
        var url = new Url(value);
        _urlHelper.ContentUrl(Arg.Any<Url>(), _contentSerializerSettings.UrlSettings).Returns($"{siteUrl}{prettyPath}");
         
        var result = _sut.Handle(url, null, null);

        result.ShouldBe($"{siteUrl}{prettyPath}");
    }

    [Fact]
    public void GivenEpiserverPage_WhenHandleWithAbsoluteUrlSetToFalse_ThenReturnsRewrittenPrettyRelativeUrl()
    {
        var prettyPath = "/rewritten/pretty-url/";
        var value = "/link/d40d0056ede847d5a2f3b4a02778d15b.aspx";
        var url = new Url(value);
        _contentSerializerSettings.UrlSettings = new UrlSettings { UseAbsoluteUrls = false };
        _urlHelper.ContentUrl(Arg.Any<Url>(), _contentSerializerSettings.UrlSettings).Returns(prettyPath);

        var result = _sut.Handle(url, null, null);

        result.ShouldBe(prettyPath);
    }
}