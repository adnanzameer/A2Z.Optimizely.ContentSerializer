using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using A2Z.Optimizely.ContentSerializer.Internal;
using A2Z.Optimizely.ContentSerializer.Internal.Default;
using A2Z.Optimizely.ContentSerializer.Internal.Default.ValueListPropertyHandlers;
using A2Z.Optimizely.ContentSerializer.Internal.Default.ValueTypePropertyHandlers;
using A2Z.Optimizely.ContentSerializer.Tests.Pages;
using NSubstitute;
using Shouldly;
using Xunit;
using StringPropertyHandler = A2Z.Optimizely.ContentSerializer.Internal.Default.StringPropertyHandler;

namespace A2Z.Optimizely.ContentSerializer.Tests
{
    public class PropertyManagerTests
    {
        private readonly PropertyManager _sut;
        private readonly StandardPage _page;
        private readonly IContentSerializerSettings _contentSerializerSettings;
        private readonly IContentLoader _contentLoader;
        public PropertyManagerTests()
        {
            this._contentSerializerSettings = Substitute.For<IContentSerializerSettings>();
            this._contentSerializerSettings.UrlSettings = new UrlSettings();
            this._contentLoader = Substitute.For<IContentLoader>();
            SetupContentLoader(this._contentLoader);
            this._sut = new PropertyManager(
                new PropertyNameStrategy(),
                new PropertyResolver(),
                new PropertyHandlerService()
            );
            this._page = new StandardPageBuilder().Build();
        }

        [Fact]
        public void GivenStringProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var selectStrategy = new DefaultSelectStrategy();
            var stringHandler = new StringPropertyHandler(selectStrategy, selectStrategy, _contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<string>))
                .Returns(stringHandler);

            // Register this fake provider into the CMS service locator scope
            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            // Act
            var result = _sut.GetStructuredData(_page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x =>
                x.Key.Equals(nameof(StandardPage.Heading)) &&
                x.Value.Equals(_page.Heading));
        }


        [Fact]
        public void GivenIntProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            var selectStrategy = new DefaultSelectStrategy();
            var stringHandler = new StringPropertyHandler(selectStrategy, selectStrategy, _contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            var intHandler = new IntPropertyHandler(_contentSerializerSettings);
            serviceProvider.GetService(typeof(IPropertyHandler<int>))
                .Returns(intHandler);
            // Register this fake provider into the CMS service locator scope
            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            var result = this._sut.GetStructuredData(_page, this._contentSerializerSettings);

            result.ShouldContain(x => x.Key.Equals(nameof(StandardPage.Age)) && x.Value.Equals(_page.Age));
        }

        [Fact]
        public void GivenDoubleProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var doubleHandler = new DoublePropertyHandler(_contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<double>))
                .Returns(doubleHandler);

            // Build a service locator substitute for APIs still pulling ServiceLocator.Current
            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            // Act
            var result = _sut.GetStructuredData(_page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x => x.Key.Equals(nameof(StandardPage.Degrees)) &&
                                      x.Value.Equals(_page.Degrees));
        }

        [Fact]
        public void GivenBoolProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var boolHandler = new BoolPropertyHandler(_contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<bool>))
                .Returns(boolHandler);

            // Fake ServiceLocator only if your _sut resolves handlers that way
            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            var page = new StandardPageBuilder().WithPrivate(true).Build();

            // Act
            var result = _sut.GetStructuredData(page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x =>
                x.Key.Equals(nameof(StandardPage.Private)) &&
                x.Value.Equals(page.Private));
        }


        [Fact]
        public void GivenDateTimeProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var dateTimeHandler = new DateTimePropertyHandler(_contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<DateTime>))
                           .Returns(dateTimeHandler);

            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            var expectedStartingDate = new DateTime(3000, 1, 1);
            var page = new StandardPageBuilder().WithStarting(expectedStartingDate).Build();

            // Act
            var result = _sut.GetStructuredData(page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x =>
                x.Key.Equals(nameof(StandardPage.Starting)) &&
                x.Value.Equals(page.Starting));
        }

        [Fact]
        public void GivenContentReferenceProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var contentReference = new ContentReference(2000);
            var page = new StandardPageBuilder().WithContentReference(contentReference).Build();

            var urlHelper = Substitute.For<IUrlHelper>();
            var contentReferencePageUrl = "https://josefottosson.se/";
            urlHelper.ContentUrl(contentReference, Arg.Any<IUrlSettings>())
                     .Returns(contentReferencePageUrl);

            var contentReferenceHandler = new ContentReferencePropertyHandler(urlHelper, _contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<ContentReference>))
                           .Returns(contentReferenceHandler);

            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            // Act
            var result = _sut.GetStructuredData(page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x =>
                x.Key.Equals(nameof(StandardPage.ContentReference)) &&
                x.Value.Equals(contentReferencePageUrl));
        }

        [Fact]
        public void GivenPageReferenceProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var pageReference = new PageReference(3000);
            var page = new StandardPageBuilder().WithPageReference(pageReference).Build();

            var urlHelper = Substitute.For<IUrlHelper>();
            var pageReferenceUrl = "https://josefottosson.se/";
            urlHelper.ContentUrl(pageReference, Arg.Any<IUrlSettings>())
                     .Returns(pageReferenceUrl);

            var contentReferenceHandler = new ContentReferencePropertyHandler(urlHelper, _contentSerializerSettings);
            var pageReferenceHandler = new PageReferencePropertyHandler(contentReferenceHandler, _contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<PageReference>))
                           .Returns(pageReferenceHandler);

            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            // Act
            var result = _sut.GetStructuredData(page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x =>
                x.Key.Equals(nameof(StandardPage.PageReference)) &&
                x.Value.Equals(pageReferenceUrl));
        }


        [Fact]
        public void GivenContentAreaProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var contentArea = CreateContentArea();
            var page = new StandardPageBuilder().WithMainContentArea(contentArea).Build();

            var contentAreaHandler = new ContentAreaPropertyHandler(_contentLoader, _sut, _contentSerializerSettings);
            var selectStrategy = new DefaultSelectStrategy();
            var stringHandler = new StringPropertyHandler(selectStrategy, selectStrategy, _contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<ContentArea>))
                .Returns(contentAreaHandler);
            serviceProvider.GetService(typeof(IPropertyHandler<string>))
                .Returns(stringHandler);

            // Fake ServiceLocator only if _sut resolves via ServiceLocator
            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            // Act
            var result = _sut.GetStructuredData(page, _contentSerializerSettings);

            // Assert
            result.ShouldContainKey(nameof(StandardPage.MainContentArea));
        }


        [Fact]
        public void GivenStringArrayProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var strings = new[] { "any", "value" };
            var page = new StandardPageBuilder().WithStrings(strings).Build();

            var stringListHandler = new StringListPropertyHandler(_contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<IEnumerable<string>>))
                .Returns(stringListHandler);

            // Only needed if _sut resolves via ServiceLocator
            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            // Act
            var result = _sut.GetStructuredData(page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x =>
                x.Key.Equals(nameof(StandardPage.Strings)) &&
                x.Value.Equals(strings));
        }


        [Fact]
        public void GivenStringListProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var strings = new List<string> { "any", "value" };
            var page = new StandardPageBuilder().WithStrings(strings).Build();

            var stringListHandler = new StringListPropertyHandler(_contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<IEnumerable<string>>))
                .Returns(stringListHandler);

            // Needed only if _sut resolves via ServiceLocator internally
            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            // Act
            var result = _sut.GetStructuredData(page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x =>
                x.Key.Equals(nameof(StandardPage.Strings)) &&
                x.Value.Equals(strings));
        }


        [Fact]
        public void GivenIntListProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var ints = new List<int> { 1000, 2000 };
            var page = new StandardPageBuilder().WithInts(ints).Build();

            var intListHandler = new IntListPropertyHandler(_contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<IEnumerable<int>>))
                           .Returns(intListHandler);

            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            // Act
            var result = _sut.GetStructuredData(page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x => x.Key.Equals(nameof(StandardPage.Ints)) &&
                                      x.Value.Equals(ints));
        }

        [Fact]
        public void GivenDoubleListProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var doubles = new List<double> { 1000, 2000.50 };
            var page = new StandardPageBuilder().WithDoubles(doubles).Build();

            var doubleListHandler = new DoubleListPropertyHandler(_contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<IEnumerable<double>>))
                           .Returns(doubleListHandler);

            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            // Act
            var result = _sut.GetStructuredData(page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x => x.Key.Equals(nameof(StandardPage.Doubles)) &&
                                      x.Value.Equals(doubles));
        }

        [Fact]
        public void GivenDateTimeListProperty_WhenGetStructuredData_ThenReturnsCorrectValue()
        {
            // Arrange
            var dateTimes = new List<DateTime> { new DateTime(2000, 1, 12), new DateTime(3000, 1, 20) };
            var page = new StandardPageBuilder().WithDateTimes(dateTimes).Build();

            var dateTimeListHandler = new DateTimeListPropertyHandler(_contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<IEnumerable<DateTime>>))
                           .Returns(dateTimeListHandler);

            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            // Act
            var result = _sut.GetStructuredData(page, _contentSerializerSettings);

            // Assert
            result.ShouldContain(x => x.Key.Equals(nameof(StandardPage.DateTimes)) &&
                                      x.Value.Equals(dateTimes));
        }

        [Fact]
        public void ShouldUsePassedInContentSerializerSettings()
        {
            // Arrange
            var pageReference = new PageReference(3000);
            var page = new StandardPageBuilder().WithPageReference(pageReference).Build();

            var urlHelper = Substitute.For<IUrlHelper>();
            var pageReferenceUrl = "https://josefottosson.se/some-path";
            urlHelper.ContentUrl(pageReference, Arg.Any<IUrlSettings>())
                     .Returns(pageReferenceUrl);

            var contentReferenceHandler = new ContentReferencePropertyHandler(urlHelper, _contentSerializerSettings);
            var pageReferenceHandler = new PageReferencePropertyHandler(contentReferenceHandler, _contentSerializerSettings);

            var serviceProvider = Substitute.For<IServiceProvider>();
            serviceProvider.GetService(typeof(IPropertyHandler<PageReference>))
                           .Returns(pageReferenceHandler);

            ServiceLocator.SetScopedServiceProvider(serviceProvider);

            var customContentSerializerSettings = new ContentSerializerSettings
            {
                UrlSettings = new UrlSettings { UseAbsoluteUrls = false }
            };

            // Act
            var result = _sut.GetStructuredData(page, customContentSerializerSettings);

            // Assert
            result.ShouldContainKey("PageReference");
            result["PageReference"].ShouldBe("/some-path");
        }


        private static void SetupContentLoader(IContentLoader contentLoader)
        {
            contentLoader.Get<ContentData>(new ContentReference(1000))
                .Returns(new VideoBlock
                {
                    Name = "My name",
                    Url = new Url("https://josef.guru")
                });
        }

        private static ContentArea CreateContentArea()
        {
            var contentArea = Substitute.For<ContentArea>();
            var items = new List<ContentAreaItem>
            {
                new ContentAreaItem
                {
                    ContentLink = new ContentReference(1000)
                }
            };
            contentArea.Count.Returns(items.Count);
            contentArea.FilteredItems.Returns(items);
            
            return contentArea;
        }
    }
}
