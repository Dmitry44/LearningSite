using LearningSite.Web.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server
{
    public class TimePeriodTests
    {
        [Theory]
        [InlineData("2022-04-06 10:00", "2022-04-06 09:59", true)]
        [InlineData("2022-04-06 10:00", "2022-04-06 10:00", false)]
        [InlineData("2022-04-06 10:00", "2022-04-06 10:01", false)]
        public void TimePeriod_ShouldCheckPeriod(DateTime start, DateTime end, bool shouldThrow)
        {
            //arrange

            //act
            var exception = Record.Exception(() => new TimePeriod(start, end));

            //assert
            if (shouldThrow) Assert.IsType<ArgumentException>(exception);
            else Assert.Null(exception);
        }

        [Theory]
        [InlineData("2022-04-06", "2022-04-10", "2022-04-01", "2022-04-07", true)]
        [InlineData("2022-04-06", "2022-04-10", "2022-04-01", "2022-04-05", false)]
        [InlineData("2022-04-06", "2022-04-10", "2022-04-01", "2022-04-06", false)]
        [InlineData("2022-04-06", "2022-04-10", "2022-04-06", "2022-04-10", true)]
        [InlineData("2022-04-06", "2022-04-10", "2022-04-07", "2022-04-09", true)]
        [InlineData("2022-04-06", "2022-04-10", "2022-01-01", "2022-12-31", true)]
        [InlineData("2022-04-06", "2022-04-10", "2022-04-07", "2022-04-10", true)]
        [InlineData("2022-04-06", "2022-04-10", "2022-04-07", "2022-04-12", true)]
        [InlineData("2022-04-06", "2022-04-10", "2022-04-11", "2022-04-12", false)]
        public void TimePeriod_ShouldCalcIntersection(DateTime start1, DateTime end1,
            DateTime start2, DateTime end2, bool isIntersected)
        {
            //arrange

            //act
            var rez = (new TimePeriod(start1, end1)).IntersectsWith(new TimePeriod(start2, end2));

            //assert
            Assert.Equal(isIntersected, rez);            
        }
    }
}
