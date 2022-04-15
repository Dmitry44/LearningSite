using LearningSite.Test.Helpers;
using LearningSite.Web.Pages.Account;
using LearningSite.Web.Server;
using Moq;
using System;
using Xunit;

namespace LearningSite.Test.Pages.Account
{
    public class IndexTests
    {
        private readonly IndexModel sut;
        private readonly IServiceProvider serviceProvider = Mock.Of<IServiceProvider>();

        public IndexTests()
        {
            sut = new IndexModel(serviceProvider, new TimeZoneProvider());
        }

        [Fact]
        public void OnGet_ShouldReturnPage()
        {
            //arrange
            sut.PageContext = PageModelHelper.CreateContext();

            //act
            var response = sut.OnGet();

            //assert
            Assert.NotNull(response);

        }
    }
}
