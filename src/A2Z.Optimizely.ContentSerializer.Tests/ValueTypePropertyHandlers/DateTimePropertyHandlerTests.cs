using A2Z.Optimizely.ContentSerializer.Internal.Default.ValueTypePropertyHandlers;
using A2Z.Optimizely.ContentSerializer.Tests.MockContent;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests.ValueTypePropertyHandlers
{
    public class DateTimePropertyHandlerTests
    {
        private readonly DateTimePropertyHandler _sut;
        public DateTimePropertyHandlerTests()
        {
            this._sut = new DateTimePropertyHandler(new ContentSerializerSettings());
        }

        [Fact]
        public void GivenDateTimeProperty_WhenHandle_ThenReturnsCorrectValue()
        {
            var expected = new DateTime(2000, 01, 01, 12, 00, 30);
            var page = new ValueTypePropertyHandlerPage
            {
                DateTime = expected
            };

            var result = (DateTime)this._sut.Handle(
                page.DateTime,
                page.GetType().GetProperty(nameof(ValueTypePropertyHandlerPage.DateTime)),
                page);

            result.ShouldBe(expected);
        }
    }
}
