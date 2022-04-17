using LearningSite.Web.Server.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Handlers
{
    public class GetUserInfo
    {
        public record Request(int UserId) : IRequest<Response>;

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly AppDbContext db;

            public Handler(AppDbContext db)
            {
                this.db = db;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var vm = await db.AppUsers
                    .Where(x => x.Id == request.UserId)
                    .Select(x => new Response(x.EmailAddress, x.Name, x.TimeZoneId))
                    .SingleOrDefaultAsync(cancellationToken);

                return vm!;
            }

        }

        public record Response(string EmailAddress, string Name, string TimeZoneId);

    }
}
