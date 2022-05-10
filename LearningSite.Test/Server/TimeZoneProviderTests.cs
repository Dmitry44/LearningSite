using LearningSite.Web.Server;
using LearningSite.Web.Server.Helpers;
using System;
using Xunit;

namespace LearningSite.Test.Server
{
    public class TimeZoneProviderTests
    {
        [Theory]
        [InlineData("Europe/Moscow")]
        [InlineData("Europe/Berlin")]
        [InlineData("America/New_York")]
        [InlineData("Asia/Tokyo")]
        public void TimeZoneProviderTests_ShouldContainTimeZone(string ianaId)
        {
            //arrange
            var sut = new TimeZoneProvider();

            //act

            //assert
            Assert.NotNull(sut.TimeZones);
            var predicate = PredicateBuilder.True<TimeZoneProvider.Info>()
                .And<TimeZoneProvider.Info>(x => x.IanaId == ianaId);
            Assert.Contains(sut.TimeZones, predicate.Compile().Invoke);
        }

    }
}
