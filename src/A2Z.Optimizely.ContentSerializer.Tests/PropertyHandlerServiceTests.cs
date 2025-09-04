using A2Z.Optimizely.ContentSerializer.Internal;
using Shouldly;
using Xunit;

namespace A2Z.Optimizely.ContentSerializer.Tests
{
    public class PropertyHandlerServiceTests
    {
        private readonly PropertyHandlerService _sut;

        public PropertyHandlerServiceTests()
        {
            this._sut = new PropertyHandlerService();
        }

        [Fact]
        public void GivenNullPropertyInfo_WhenGetPropertyHandler_ThenReturnsNull()
        {
            var result = this._sut.GetPropertyHandler(null);

            result.ShouldBeNull();
        }
    }
}
