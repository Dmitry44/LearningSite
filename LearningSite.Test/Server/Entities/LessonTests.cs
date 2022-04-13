using LearningSite.Test.Helpers;
using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Handlers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LearningSite.Test.Server.Entities
{
    public class LessonTests
    {
        [Fact]
        public async Task Lesson_ShoulCheckNameDuplication()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var name = $"{Guid.NewGuid}";
            db.Lessons.Add(new Lesson() { Name = name });
            db.Lessons.Add(new Lesson() { Name = name });

            //act
            var ex = await Record.ExceptionAsync(() => db.SaveChangesAsync());

            //assert
            Assert.NotNull(ex);
            Assert.NotNull(ex.InnerException);
            Assert.IsAssignableFrom<DbUpdateException>(ex);
            Assert.Contains("UNIQUE", ex?.InnerException?.Message);
        }

    }
}
