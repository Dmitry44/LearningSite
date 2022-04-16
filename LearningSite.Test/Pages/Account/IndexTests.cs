using LearningSite.Test.Helpers;
using LearningSite.Web.Pages.Account;
using LearningSite.Web.Server;
using MediatR;
using Moq;
using System;
using Xunit;

namespace LearningSite.Test.Pages.Account
{
    public class IndexTests
    {
        private readonly IServiceProvider serviceProvider = Mock.Of<IServiceProvider>();
        private readonly IMediator mediator = Mock.Of<IMediator>();

        public IndexTests()
        {
        }

        [Fact]
        public void OnGet_ShouldReturnPage()
        {
            using var factory = new AppDbContextFactory();
            using var dbContext = factory.CreateContext();

            //arrange
            var sut = new IndexModel(serviceProvider, new TimeZoneProvider(), mediator);
            sut.PageContext = PageModelHelper.CreateContext();

            //act
            var response = sut.OnGet();

            //assert
            Assert.NotNull(response);

        }
    }
}
