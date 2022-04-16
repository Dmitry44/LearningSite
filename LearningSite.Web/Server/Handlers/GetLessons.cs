using LearningSite.Web.Server.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Handlers
{
    public class GetLessons
    {
        public class Query : IRequest<Vm> { }

        public class Handler : IRequestHandler<Query, Vm>
        {
            private readonly AppDbContext db;

            public Handler(AppDbContext db)
            {
                this.db = db;
            }

            public async Task<Vm> Handle(Query request, CancellationToken cancellationToken)
            {
                var vm = new Vm();
                vm.Lessons = await db.Packages.Where(x => x.IsActive && x.Lesson.IsActive)
                    .OrderBy(x => x.Lesson.Name).ThenBy(x => x.Minutes).ThenBy(x => x.Quantity)
                    .Select(x => new LessonRow()
                    {
                        PackageId = x.Id,
                        LessonName = x.Lesson.Name,
                        PackageName = x.Name,
                        Decription = String.IsNullOrWhiteSpace(x.Decription) ? x.Lesson.Decription : x.Decription,
                        Minutes = x.Minutes,
                        Quantity = x.Quantity,
                        PriceStr = x.PriceStr,
                        PaymentUrl = x.PaymentUrl
                    }).ToListAsync(cancellationToken);
                return vm;
            }

        }

        public class Vm
        {
            public string Title { get; set; } = "Classes";
            public List<LessonRow> Lessons { get; internal set; } = new();
        }

        public class LessonRow
        {
            public int PackageId { get; internal set; }
            public string LessonName { get; internal set; } = "";
            public string PackageName { get; internal set; } = "";
            public string Decription { get; internal set; } = "";
            public int Minutes { get; internal set; }
            public int Quantity { get; internal set; }
            public string PriceStr { get; internal set; } = "";
            public string PaymentUrl { get; internal set; } = "";
        }

    }
}
