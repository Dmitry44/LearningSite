﻿using LearningSite.Test.Helpers;
using LearningSite.Web.Pages.Account;
using LearningSite.Web.Server;
using Moq;
using System;
using Xunit;

namespace LearningSite.Test.Pages.Account
{
    public class IndexTests
    {
        private readonly IServiceProvider serviceProvider = Mock.Of<IServiceProvider>();

        public IndexTests()
        {
        }

        [Fact]
        public void OnGet_ShouldReturnPage()
        {
            using var factory = new AppDbContextFactory();
            using var dbContext = factory.CreateContext();

            //arrange
            var sut = new IndexModel(serviceProvider, new TimeZoneProvider(), dbContext);
            sut.PageContext = PageModelHelper.CreateContext();

            //act
            var response = sut.OnGet();

            //assert
            Assert.NotNull(response);

        }
    }
}
