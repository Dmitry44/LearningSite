using LearningSite.Test.Helpers;
using LearningSite.Web.Server;
using LearningSite.Web.Server.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server
{
    public class CalendarTests
    {
        [Fact]
        public void Calendar_ShouldGetDaySlots()
        {
            //arrange
            var sut = new Calendar();

            //act
            var slots = sut.GetDaySlots(DateTime.Now, 60);

            //assert
            Assert.NotNull(slots);
            Assert.Equal(24, slots.Count);
        }

    }
}
