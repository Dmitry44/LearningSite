using LearningSite.Web.Server.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Handlers
{
    public class GetLessons
    {
        public record Request : IRequest<Response>;

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly AppDbContext db;

            public Handler(AppDbContext db)
            {
                this.db = db;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var lessons = await db.Packages.Where(x => x.IsActive && x.Lesson.IsActive)
                    .OrderBy(x => x.Lesson.Name).ThenBy(x => x.Minutes).ThenBy(x => x.Quantity)
                    .Select(x => new LessonRow(
                        x.Id,
                        x.Lesson.Name,
                        x.Name,
                        String.IsNullOrWhiteSpace(x.Decription) ? x.Lesson.Decription : x.Decription,
                        x.Minutes,
                        x.Quantity,
                        x.PriceStr,
                        x.PaymentUrl
                    )).ToListAsync(cancellationToken);
                return new Response(Lessons: lessons);
            }

        }

        public record Response(List<LessonRow> Lessons, string Title = "Classes");

        public record LessonRow(
            int PackageId,
            string LessonName,
            string PackageName,
            string Decription,
            int Minutes,
            int Quantity,
            string PriceStr,
            string PaymentUrl);

    }
}
