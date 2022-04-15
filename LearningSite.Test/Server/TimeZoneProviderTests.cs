using LearningSite.Web.Server;
using LearningSite.Web.Server.Helpers;
using System;
using Xunit;

namespace LearningSite.Test.Server
{
    public class TimeZoneProviderTests
    {
        [Theory]
        [InlineData("Russian Standard Time", "Europe/Moscow")]
        [InlineData("W. Europe Standard Time", "Europe/Berlin")]
        [InlineData("Eastern Standard Time", "America/New_York")]
        [InlineData("Tokyo Standard Time", "Asia/Tokyo")]
        public void TimeZoneProviderTests_ShouldContainTimeZone(string systemId, string ianaId)
        {
            //arrange
            var sut = new TimeZoneProvider();

            //act

            //assert
            Assert.NotNull(sut.TimeZones);
            var predicate = PredicateBuilder.True<TimeZoneProvider.Info>()
                .And<TimeZoneProvider.Info>(x => x.SystemId == systemId)
                .And<TimeZoneProvider.Info>(x => x.IanaId == ianaId);
            Assert.Contains(sut.TimeZones, predicate.Compile().Invoke);
        }

    }
}
