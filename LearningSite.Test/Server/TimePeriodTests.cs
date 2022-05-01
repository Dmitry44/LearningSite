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
        [InlineData("2022-04-06", "2022-04-11 00:00:00", "2022-04-11", "2022-04-12", false)]
        [InlineData("2022-04-06", "2022-04-11 00:00:01", "2022-04-11", "2022-04-12", true)]
        public void TimePeriod_ShouldCalcIntersection(DateTime start1, DateTime end1,
            DateTime start2, DateTime end2, bool isIntersected)
        {
            //arrange

            //act
            var rez = (new TimePeriod(start1, end1)).IntersectsWith(new TimePeriod(start2, end2));

            //assert
            Assert.Equal(isIntersected, rez);            
        }

        [Theory]
        [InlineData("2022-04-06", "2022-04-10", "2022-04-05", "2022-04-11", true)]
        [InlineData("2022-04-06", "2022-04-10", "2022-04-06", "2022-04-10", true)]
        [InlineData("2022-04-01", "2022-04-01", "2022-04-06", "2022-04-10", false)]
        [InlineData("2022-04-01", "2022-04-06", "2022-04-06", "2022-04-10", false)]
        [InlineData("2022-04-01", "2022-04-08", "2022-04-06", "2022-04-10", false)]
        [InlineData("2022-04-06", "2022-04-08", "2022-04-06", "2022-04-10", true)]
        [InlineData("2022-04-08", "2022-04-10", "2022-04-06", "2022-04-10", true)]
        [InlineData("2022-04-08", "2022-04-11", "2022-04-06", "2022-04-10", false)]
        [InlineData("2022-04-11", "2022-04-12", "2022-04-06", "2022-04-10", false)]
        public void TimePeriod_ShouldCalcInsideIn(DateTime start1, DateTime end1,
            DateTime start2, DateTime end2, bool isIntersected)
        {
            //arrange

            //act
            var rez = (new TimePeriod(start1, end1)).InsideIn(new TimePeriod(start2, end2));

            //assert
            Assert.Equal(isIntersected, rez);
        }

        [Fact]
        public void TimeLineAddPeriod_ShouldAddPeriod_WhenEmpty()
        {
            //arrange
            var sut = new TimeLine();

            //act
            sut.AddPeriod(new TimePeriod(DateTime.MinValue, DateTime.MaxValue));

            //assert
            Assert.Single(sut.Periods);
            Assert.Equal(DateTime.MinValue, sut.Periods.First().Start);
            Assert.Equal(DateTime.MaxValue, sut.Periods.Last().End);
            Assert.True(sut.CheckLinearity());
        }

        [Fact]
        public void TimeLineAddPeriod_ShouldAddPeriod_WhenNoIntersection()
        {
            //arrange
            var sut = new TimeLine();

            //act
            sut.AddPeriod(new TimePeriod(new(2022, 5, 1), new(2022, 5, 2)));
            sut.AddPeriod(new TimePeriod(new(2022, 6, 1), new(2022, 6, 2)));
            sut.AddPeriod(new TimePeriod(new(2022, 4, 1), new(2022, 4, 2)));

            //assert
            Assert.Equal(3, sut.Periods.Count);
            Assert.Equal(new(2022, 4, 1), sut.Periods.First().Start);
            Assert.Equal(new(2022, 4, 2), sut.Periods.First().End);
            Assert.Equal(new(2022, 6, 1), sut.Periods.Last().Start);
            Assert.Equal(new(2022, 6, 2), sut.Periods.Last().End);
            Assert.True(sut.CheckLinearity());
        }

        [Theory]
        [InlineData("2022-05-10", "2022-05-12", "2022-05-08", "2022-05-12")]
        [InlineData("2022-05-10", "2022-05-12", "2022-05-08", "2022-05-11")]
        public void TimeLineAddPeriod_ShouldExpandPeriod_WhenStartsEarlier(DateTime start1, DateTime end1,
            DateTime start2, DateTime end2)
        {
            //arrange
            var sut = new TimeLine();

            //act
            sut.AddPeriod(new TimePeriod(start1, end1));
            sut.AddPeriod(new TimePeriod(start2, end2));

            //assert
            Assert.Single(sut.Periods);
            Assert.Equal(start2, sut.Periods.First().Start);
            Assert.Equal(end1, sut.Periods.First().End);
            Assert.True(sut.CheckLinearity());
        }

        [Theory]
        [InlineData("2022-05-10", "2022-05-12", "2022-05-11", "2022-05-14")]
        public void TimeLineAddPeriod_ShouldSplitPeriod_WhenEndsLater(DateTime start1, DateTime end1,
            DateTime start2, DateTime end2)
        {
            //arrange
            var sut = new TimeLine();

            //act
            sut.AddPeriod(new TimePeriod(start1, end1));
            sut.AddPeriod(new TimePeriod(start2, end2));

            //assert
            Assert.Equal(2, sut.Periods.Count);
            Assert.Equal(start1, sut.Periods.First().Start);
            Assert.Equal(end1, sut.Periods.First().End);
            Assert.Equal(end1, sut.Periods.Last().Start);
            Assert.Equal(end2, sut.Periods.Last().End);
            Assert.True(sut.CheckLinearity());
        }

        [Fact]
        public void TimeLineAddPeriod_ShouldExpandAndSplitPeriod()
        {
            //arrange
            var sut = new TimeLine();

            //act
            sut.AddPeriod(new TimePeriod(new(2022, 4, 10), new(2022, 4, 15)));
            sut.AddPeriod(new TimePeriod(new(2022, 4, 16), new(2022, 4, 20)));
            sut.AddPeriod(new TimePeriod(new(2022, 4, 1), new(2022, 4, 25)));

            //assert
            Assert.Equal(3, sut.Periods.Count);
            Assert.Equal(new(2022, 4, 1), sut.Periods.First().Start);
            Assert.Equal(new(2022, 4, 15), sut.Periods.First().End);
            Assert.Equal(new(2022, 4, 20), sut.Periods.Last().Start);
            Assert.Equal(new(2022, 4, 25), sut.Periods.Last().End);
            Assert.True(sut.CheckLinearity());
        }

        [Fact]
        public void TimeLineCombinePeriods_ShouldCombinePeriods_WhenNoGap()
        {
            //arrange
            var sut = new TimeLine();
            sut.AddPeriod(new TimePeriod(new(2022, 4, 10), new(2022, 4, 15)));
            sut.AddPeriod(new TimePeriod(new(2022, 4, 15), new(2022, 4, 20)));

            //act
            sut.CombinePeriods();

            //assert
            Assert.Single(sut.Periods);
            Assert.Equal(new(2022, 4, 10), sut.Periods.First().Start);
            Assert.Equal(new(2022, 4, 20), sut.Periods.First().End);
            Assert.True(sut.CheckLinearity());
        }

        [Fact]
        public void TimeLineCombinePeriods_ShouldCombineOnlyPeriods_WhenNoGap()
        {
            //arrange
            var sut = new TimeLine();
            sut.AddPeriod(new TimePeriod(new(2022, 4, 10), new(2022, 4, 15)));
            sut.AddPeriod(new TimePeriod(new(2022, 4, 15), new(2022, 4, 20)));
            sut.AddPeriod(new TimePeriod(new(2022, 4, 22), new(2022, 4, 24)));
            sut.AddPeriod(new TimePeriod(new(2022, 4, 24), new(2022, 4, 30)));

            //act
            sut.CombinePeriods();

            //assert
            Assert.Equal(2, sut.Periods.Count);
            Assert.Equal(new(2022, 4, 10), sut.Periods.First().Start);
            Assert.Equal(new(2022, 4, 20), sut.Periods.First().End);
            Assert.Equal(new(2022, 4, 22), sut.Periods.Last().Start);
            Assert.Equal(new(2022, 4, 30), sut.Periods.Last().End);
            Assert.True(sut.CheckLinearity());
        }

        [Fact]
        public void TimeLineSubstractPeriod_ShouldDoNothing_WhenNoIntersection()
        {
            //arrange
            var sut = new TimeLine();
            sut.AddPeriod(new TimePeriod(new(2022, 4, 10), new(2022, 4, 15)));

            //act
            sut.SubstractPeriod(new(new(2020, 1,1), new(2020,2,1)));

            //assert
            Assert.Single(sut.Periods);
            Assert.Equal(new(2022, 4, 10), sut.Periods.First().Start);
            Assert.Equal(new(2022, 4, 15), sut.Periods.First().End);
            Assert.True(sut.CheckLinearity());
        }

        [Fact]
        public void TimeLineSubstractPeriod_ShouldSplitPeriod_WhenInside()
        {
            //arrange
            var sut = new TimeLine();
            sut.AddPeriod(new TimePeriod(new(2022, 4, 10), new(2022, 4, 20)));

            //act
            sut.SubstractPeriod(new(new(2022, 4, 12), new(2022, 4, 14)));

            //assert
            Assert.Equal(2, sut.Periods.Count);
            Assert.Equal(new(2022, 4, 10), sut.Periods.First().Start);
            Assert.Equal(new(2022, 4, 12), sut.Periods.First().End);
            Assert.Equal(new(2022, 4, 14), sut.Periods.Last().Start);
            Assert.Equal(new(2022, 4, 20), sut.Periods.Last().End);
            Assert.True(sut.CheckLinearity());
        }

        [Theory]
        [InlineData("2022-04-09", "2022-04-21")]
        [InlineData("2022-04-10", "2022-04-21")]
        [InlineData("2022-04-09", "2022-04-20")]
        [InlineData("2022-04-10", "2022-04-20")]
        public void TimeLineSubstractPeriod_ShouldRemovePeriod_WhenOverlapped(DateTime start1, DateTime end1)
        {
            //arrange
            var sut = new TimeLine();
            sut.AddPeriod(new TimePeriod(new(2022, 4, 10), new(2022, 4, 20)));

            //act
            sut.SubstractPeriod(new(start1, end1));

            //assert
            Assert.Empty(sut.Periods);
            Assert.True(sut.CheckLinearity());
        }

        [Fact]
        public void TimeLineSubstractPeriod_ShouldCutPeriod_WhenStartsEarlier()
        {
            //arrange
            var sut = new TimeLine();
            sut.AddPeriod(new TimePeriod(new(2022, 4, 10), new(2022, 4, 20)));

            //act
            sut.SubstractPeriod(new(new(2022, 4, 1), new(2022, 4, 12)));

            //assert
            Assert.Single(sut.Periods);
            Assert.Equal(new(2022, 4, 12), sut.Periods.First().Start);
            Assert.Equal(new(2022, 4, 20), sut.Periods.First().End);
            Assert.True(sut.CheckLinearity());
        }

        [Fact]
        public void TimeLineSubstractPeriod_ShouldCutPeriod_WhenEndsLater()
        {
            //arrange
            var sut = new TimeLine();
            sut.AddPeriod(new TimePeriod(new(2022, 4, 10), new(2022, 4, 20)));

            //act
            sut.SubstractPeriod(new(new(2022, 4, 15), new(2022, 4, 30)));

            //assert
            Assert.Single(sut.Periods);
            Assert.Equal(new(2022, 4, 10), sut.Periods.First().Start);
            Assert.Equal(new(2022, 4, 15), sut.Periods.First().End);
            Assert.True(sut.CheckLinearity());
        }

    }
}
