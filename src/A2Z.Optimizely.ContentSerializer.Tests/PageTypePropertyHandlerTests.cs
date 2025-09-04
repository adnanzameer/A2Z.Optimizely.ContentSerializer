using A2Z.Optimizely.ContentSerializer.Internal.Default;
using EPiServer.DataAbstraction;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests;

public class PageTypePropertyHandlerTests
{
    private readonly PageTypePropertyHandler _sut;

    public PageTypePropertyHandlerTests()
    {
        _sut = new PageTypePropertyHandler(new ContentSerializerSettings());
    }

    [Fact]
    public void GivenNullPageType_WhenHandle_ThenReturnsNull()
    {
        var result = _sut.Handle(null, null, null);

        result.ShouldBeNull();
    }

    [Fact]
    public void GivenPageType_WhenHandle_ThenReturnsCorrectValue()
    {
        var pageType = new PageType {Name = "anytype"};

        var result = _sut.Handle(pageType, null, null);

        ((PageTypeModel)result).Name.ShouldBe(pageType.Name);
    }
}