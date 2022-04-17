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
        private readonly IMediator mediator = Mock.Of<IMediator>();
        private readonly TimeZoneProvider timeZoneProvider = Mock.Of<TimeZoneProvider>();

        public IndexTests()
        {
        }

        [Fact]
        public void OnGet_ShouldReturnPage()
        {
            using var factory = new AppDbContextFactory();
            using var dbContext = factory.CreateContext();

            //arrange
            var sut = new IndexModel(timeZoneProvider, mediator);
            sut.PageContext = PageModelHelper.CreateContext();

            //act
            var response = sut.OnGet();

            //assert
            Assert.NotNull(response);

        }
    }
}
