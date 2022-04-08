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

        [Fact]
        public void Ttttt()
        {
            using (var db = new TestDbContext())
            {
                var data = new List<Availability>()
                {
                    new() { Start = DateTime.Now.AddDays(-5), End = DateTime.Now },
                    new() { Start = DateTime.Now, End = DateTime.Now.AddHours(10) },
                    new() { Start = DateTime.Now.AddYears(1), End = DateTime.Now.AddYears(2) },
                    new() { Start = DateTime.Now, End = DateTime.Now }
                };
                db.Availabilities.AddRange(data);
                db.SaveChanges();
            }
        }
    }
}
