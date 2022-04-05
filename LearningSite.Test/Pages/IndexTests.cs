using LearningSite.Test.Helpers;
using LearningSite.Web.Pages;
using Moq;
using System;
using Xunit;

namespace LearningSite.Test.Pages
{
    public class IndexTests
    {
        private readonly IndexModel sut;
        private readonly IServiceProvider serviceProvider = Mock.Of<IServiceProvider>();

        public IndexTests()
        {
            sut = new IndexModel(serviceProvider);
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
