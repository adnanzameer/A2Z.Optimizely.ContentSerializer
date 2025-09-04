using A2Z.Optimizely.ContentSerializer.Internal;
using A2Z.Optimizely.ContentSerializer.Tests.MockContent;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests;

public class PropertyResolverTests
{
    private readonly PropertyResolver _sut;

    public PropertyResolverTests()
    {
        _sut = new PropertyResolver();
    }

    [Fact]
    public void GivenPage_WhenGetProperties_ThenReturnsCorrectProperties()
    {
        var page = new PropertyResolverPage();
        var expected = new List<string>
        {
            nameof(PropertyResolverPage.Heading),
            nameof(PropertyResolverPage.Description),
            nameof(PropertyResolverPage.Age),
            nameof(PropertyResolverPage.Starting),
            nameof(PropertyResolverPage.Private),
            nameof(PropertyResolverPage.Degrees),
            nameof(PropertyResolverPage.MainBody),
            nameof(PropertyResolverPage.MainContentArea),
            nameof(PropertyResolverPage.MainVideo),
            nameof(PropertyResolverPage.Include)
        };

        var result = _sut.GetProperties(page).ToList();
        var includedPropertyNames = result.Select(x => x.Name);
            
        includedPropertyNames.ShouldBe(expected);
        result.ShouldNotContain(x => x.Name.Equals(nameof(PropertyResolverPage.Author)));
        result.ShouldNotContain(x => x.Name.Equals(nameof(PropertyResolverPage.Phone)));
    }
}