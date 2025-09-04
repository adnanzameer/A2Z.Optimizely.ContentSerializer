using A2Z.Optimizely.ContentSerializer.Internal.Default.ValueListPropertyHandlers;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests.ValueTypeListPropertyHandlers;

public class IntListPropertyHandlerTests
{
    private readonly IntListPropertyHandler _sut;

    public IntListPropertyHandlerTests()
    {
        _sut = new IntListPropertyHandler(new ContentSerializerSettings());
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
        var result = _sut.Handle(Enumerable.Empty<int>(), null, null);

        ((IEnumerable<int>)result).ShouldBeEmpty();
    }

    [Fact]
    public void GivenPopulatedList_WhenHandle_ThenReturnsSameList()
    {
        var items = new List<int> { 1000, 2000 };

        var result = _sut.Handle(items, null, null);

        ((IEnumerable<int>)result).ShouldContain(1000);
        ((IEnumerable<int>)result).ShouldContain(2000);
        ((IEnumerable<int>)result).Count().ShouldBe(2);
    }
}