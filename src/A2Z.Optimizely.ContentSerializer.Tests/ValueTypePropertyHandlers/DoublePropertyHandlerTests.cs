using A2Z.Optimizely.ContentSerializer.Internal.Default.ValueTypePropertyHandlers;
using A2Z.Optimizely.ContentSerializer.Tests.MockContent;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests.ValueTypePropertyHandlers;

public class DoublePropertyHandlerTests
{
    private readonly DoublePropertyHandler _sut;
    public DoublePropertyHandlerTests()
    {
        _sut = new DoublePropertyHandler(new ContentSerializerSettings());
    }

    [Fact]
    public void GivenDoubleProperty_WhenHandle_ThenReturnsCorrectValue()
    {
        var page = new ValueTypePropertyHandlerPage
        {
            Double = 10.50
        };

        var result = (double)_sut.Handle(
            page.Double,
            page.GetType().GetProperty(nameof(ValueTypePropertyHandlerPage.Double)),
            page);

        result.ShouldBe(10.50);
    }
}