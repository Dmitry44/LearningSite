using LearningSite.Web.Server.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Handlers
{
    public class GetUserInfo
    {
        public class Query : IRequest<Vm> {
            public int UserId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Vm>
        {
            private readonly AppDbContext db;

            public Handler(AppDbContext db)
            {
                this.db = db;
            }

            public async Task<Vm> Handle(Query request, CancellationToken cancellationToken)
            {
                var vm = await db.AppUsers
                    .Where(x => x.Id == request.UserId)
                    .Select(x => new Vm()
                    {
                        EmailAddress = x.EmailAddress,
                        Name = x.Name,
                        TimeZoneId = x.TimeZoneId
                    })
                    .SingleOrDefaultAsync(cancellationToken);

                return vm!;
            }

        }

        public class Vm
        {
            public string EmailAddress { get; set; } = "";
            public string Name { get; set; } = "";
            public string TimeZoneId { get; set; } = "";
        }

    }
}
