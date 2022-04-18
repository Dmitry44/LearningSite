using LearningSite.Web.Server.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Handlers.Auth
{
    public class IsEmailExists
    {
        public record Request(string Email) : IRequest<bool>;

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly AppDbContext db;

            public Handler(AppDbContext db)
            {
                this.db = db;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                return await db.AppUsers.AnyAsync(x => x.EmailAddress == request.Email, cancellationToken);
            }

        }
    }

}
