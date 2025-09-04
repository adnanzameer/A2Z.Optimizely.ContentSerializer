using A2Z.Optimizely.ContentSerializer.Internal.Default.ValueListPropertyHandlers;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests.ValueTypeListPropertyHandlers;

public class DoubleListPropertyHandlerTests
{
    private readonly DoubleListPropertyHandler _sut;

    public DoubleListPropertyHandlerTests()
    {
        _sut = new DoubleListPropertyHandler(new ContentSerializerSettings());
    }

    [Fact]
    public void GivenNullList_WhenHandle_ThenReturnsNull()
    {
        var result = _sut.Handle(null, null, null);

        result.ShouldBeNull();
    }

    [Fact]
    public void GivenEmptyList_WhenHandle_ThenReturnsSameList()
    {
        var result = _sut.Handle(Enumerable.Empty<double>(), null, null);

        ((IEnumerable<double>)result).ShouldBeEmpty();
    }

    [Fact]
    public void GivenPopulatedList_WhenHandle_ThenReturnsSameList()
    {
        var items = new List<double> { 1000, 20.50 };

        var result = _sut.Handle(items, null, null);

        ((IEnumerable<double>)result).ShouldContain(1000);
        ((IEnumerable<double>)result).ShouldContain(20.50);
        ((IEnumerable<double>)result).Count().ShouldBe(2);
    }
}