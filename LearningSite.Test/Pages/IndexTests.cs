using LearningSite.Test.Helpers;
using LearningSite.Web.Pages;
using MediatR;
using Moq;
using System;
using Xunit;

namespace LearningSite.Test.Pages
{
    public class IndexTests
    {
        private readonly IndexModel sut;
        private readonly IServiceProvider serviceProvider = Mock.Of<IServiceProvider>();
        private readonly IMediator mediator = Mock.Of<IMediator>();

        public IndexTests()
        {
            sut = new IndexModel(serviceProvider, mediator);
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
