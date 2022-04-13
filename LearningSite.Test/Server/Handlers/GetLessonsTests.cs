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

namespace LearningSite.Test.Server.Handlers
{
    public class GetLessonsTests
    {
        [Fact]
        public async Task GetLessons_ShoulReturnLessons()
        {
            using var factory = new AppDbContextFactory();
            using var db = factory.CreateContext();

            //arrange
            var sut = new GetLessons.Handler(db);

            var name = Guid.NewGuid().ToString();
            db.Database.ExecuteSqlRaw("update Lessons set IsActive = 0 where IsActive = 1");
            db.Add(new Lesson()
            {
                Name = name,
                IsActive = true,
                Packages = new List<Package>()
                {
                    new Package() { IsActive = true, Name = name },
                    new Package() { IsActive = false }
                }
            });
            db.Add(new Lesson()
            {
                Name = Guid.NewGuid().ToString(),
                IsActive = false,
                Packages = new List<Package>()
                {
                    new Package() { IsActive = true },
                    new Package() { IsActive = false }
                }
            });
            db.SaveChanges();

            //act
            var rez = await sut.Handle(new GetLessons.Query(), new System.Threading.CancellationToken());

            //assert
            Assert.NotNull(rez);
            Assert.IsType<GetLessons.Vm>(rez);
            Assert.Single(rez.Lessons);
            Assert.Equal(name, rez.Lessons.First().LessonName);
            Assert.Equal(name, rez.Lessons.First().PackageName);
        }

    }
}
