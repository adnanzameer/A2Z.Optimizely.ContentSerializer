using A2Z.Optimizely.ContentSerializer.Internal.Default.ValueTypePropertyHandlers;
using A2Z.Optimizely.ContentSerializer.Tests.MockContent;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests.ValueTypePropertyHandlers
{
    public class IntPropertyHandlerTests
    {
        private readonly IntPropertyHandler _sut;

        public IntPropertyHandlerTests()
        {
            this._sut = new IntPropertyHandler(new ContentSerializerSettings());
        }

        [Fact]
        public void GivenIntProperty_WhenHandle_ThenReturnsCorrectValue()
        {
            var page = new ValueTypePropertyHandlerPage
            {
                Integer = 1000
            };

            var result = (int)this._sut.Handle(
                page.Integer,
                page.GetType().GetProperty(nameof(ValueTypePropertyHandlerPage.Integer)),
                page);

            result.ShouldBe(1000);
        }
    }
}
