using LearningSite.Web.Server.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Handlers
{
    public class UpdateUserInfo
    {
        public record Request(AppUser User) : IRequest;

        public class Handler : IRequestHandler<Request>
        {
            private readonly AppDbContext db;

            public Handler(AppDbContext db)
            {
                this.db = db;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await db.AppUsers.FirstAsync(x => x.Id == request.User.Id, cancellationToken);

                user.EmailAddress = request.User.EmailAddress;
                user.Name = request.User.Name;
                user.TimeZoneId = request.User.TimeZoneId;

                await db.SaveChangesAsync(cancellationToken);
            }

        }

    }
}
