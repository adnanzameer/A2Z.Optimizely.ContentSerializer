using A2Z.Optimizely.ContentSerializer.Internal;
using A2Z.Optimizely.ContentSerializer.Internal.Default;
using A2Z.Optimizely.ContentSerializer.Tests.MockContent;
using A2Z.Optimizely.ContentSerializer.Tests.MockContent.SelectStrategies;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests;

public class StringPropertyHandlerWithCustomSelectStrategiesTests
{
    private readonly StringPropertyHandler _sut;

    public StringPropertyHandlerWithCustomSelectStrategiesTests()
    {
        var customSelectOneStrategy = new CustomSelectOneStrategy();
        var customSelectManyStrategy = new CustomSelectManyStrategy();
        _sut = new StringPropertyHandler(customSelectOneStrategy, customSelectManyStrategy, new ContentSerializerSettings());
    }

    [Fact]
    public void GivenStringPropertyWithSelectOneAndSelectedOptionsOnly_WhenHandle_ThenReturnsCorrectValue()
    {
        var page = new StringPropertyHandlerPage
        {
            SelectedOnlyOne = "option4"
        };

        var result = (SelectOption)_sut.Handle(page.SelectedOnlyOne,
            page.GetType().GetProperty(nameof(StringPropertyHandlerPage.SelectedOnlyOne)),
            page);

        result.Selected.ShouldBeTrue();
        result.Text.ShouldBe("Option 4");
        result.Value.ShouldBe("option4");
    }

    [Fact]
    public void GivenStringPropertyWithSelectOneAndSelectedOptionsOnly_ValueOnly_WhenHandle_ThenReturnsCorrectValue()
    {
        var page = new StringPropertyHandlerPage
        {
            SelectedOnlyValueOnlyOne = "option5"
        };

        var result = (string)_sut.Handle(page.SelectedOnlyValueOnlyOne,
            page.GetType().GetProperty(nameof(StringPropertyHandlerPage.SelectedOnlyValueOnlyOne)),
            page);

        result.ShouldBe("option5");
    }

    [Fact]
    public void GivenStringPropertyWithSelectManyAndSelectedOptionsOnly_WhenHandle_ThenReturnsCorrectValue()
    {
        var page = new StringPropertyHandlerPage
        {
            SelectedOnlyMany = "option5,option6,option7"
        };

        var result = ((IEnumerable<SelectOption>)_sut.Handle(page.SelectedOnlyMany,
            page.GetType().GetProperty(nameof(StringPropertyHandlerPage.SelectedOnlyMany)),
            page)).ToList();

        result.ShouldContain(x => x.Selected && x.Value.Equals("option5") && x.Text.Equals("Option 5"));
        result.ShouldContain(x => x.Selected && x.Value.Equals("option6") && x.Text.Equals("Option 6"));
        result.ShouldContain(x => x.Selected && x.Value.Equals("option7") && x.Text.Equals("Option 7"));
        result.Count(x => x.Selected).ShouldBe(3);
        result.Count.ShouldBe(3);
    }

    [Fact]
    public void GivenStringPropertyWithSelectManyAndSelectedOptionsOnly_ValueOnly_WhenHandle_ThenReturnsCorrectValue()
    {
        var page = new StringPropertyHandlerPage
        {
            SelectedOnlyValueOnlyMany = "option5,option6,option7"
        };

        var result = ((IEnumerable<string>)_sut.Handle(page.SelectedOnlyValueOnlyMany,
            page.GetType().GetProperty(nameof(StringPropertyHandlerPage.SelectedOnlyValueOnlyMany)),
            page)).ToList();

        result.ShouldContain(x => x == "option5");
        result.ShouldContain(x => x == "option6");
        result.ShouldContain(x => x == "option7");
        result.Count.ShouldBe(3);
    }
}