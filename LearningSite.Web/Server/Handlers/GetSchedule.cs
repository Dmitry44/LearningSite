using LearningSite.Web.Server.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Handlers
{
    public class GetSchedule
    {
        public record Request(int UserId) : IRequest<Response>
        {
            public int PurchaseId { get; set; }
            public DateOnly Date1 { get; set; }
            public DateOnly Date2 { get; set; }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly AppDbContext db;

            public Handler(AppDbContext db)
            {
                this.db = db;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.PurchaseId == default)
                {
                    request.PurchaseId = await db.Purchases
                        .Where(x => x.IsActive && x.UserId == request.UserId)
                        .OrderByDescending(x => x.CreatedAt).ThenBy(x => x.Package.Lesson.Name)
                        .Select(x => x.Id)
                        .FirstOrDefaultAsync(cancellationToken);
                }

                if (request.Date1 == default)
                {
                    request.Date1 = DateOnly.FromDateTime(DateTime.Today);
                    request.Date2 = request.Date1.AddDays(5);
                }

                //var tt = timePeriod. 

                return new Response();
            }

        }

        public record Response
        {
            public List<TimeRow> Times { get; set; } = new();
        }

        public record TimeRow(
            string Caption,
            TimeOnly Start,
            TimeOnly End,
            List<TimeRow> Days);

        public record DayRow(string Caption, DateOnly Day, int Status);

    }
}
