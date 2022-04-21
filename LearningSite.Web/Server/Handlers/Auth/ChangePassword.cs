using LearningSite.Web.Server.Entities;
using LearningSite.Web.Server.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Handlers.Auth
{
    public class ChangePassword
    {
        public record Request(string Email, string OldPassword, string NewPassword) : IRequest<bool>;

        public class Handler : IRequestHandler<Request, bool>
        {
            private readonly AppDbContext db;

            public Handler(AppDbContext db)
            {
                this.db = db;
            }

            public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
            {
                var user = await db.AppUsers
                    .Where(x => x.IsActive && x.EmailAddress == request.Email)
                    .FirstOrDefaultAsync(cancellationToken);
                if (user is null) return false;
                if (HashHelper.GenerateHash(request.OldPassword, user.Salt) != user.PasswordHash) return false;

                user.Salt = HashHelper.GenerateSalt();
                user.PasswordHash = HashHelper.GenerateHash(request.NewPassword, user.Salt);

                var rez = await db.SaveChangesAsync(cancellationToken);

                return rez == 1;
            }

        }

    }
}
