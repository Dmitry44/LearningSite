using LearningSite.Web.Server.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Handlers
{
    public class GetSetting
    {
        public record Request(string Key) : IRequest<string>;

        public class Handler : IRequestHandler<Request, string>
        {
            private readonly AppDbContext db;

            public Handler(AppDbContext db)
            {
                this.db = db;
            }

            public async Task<string> Handle(Request request, CancellationToken cancellationToken)
            {
                return await db.Settings.Where(x => x.Key == request.Key)
                    .Select(x => x.Value).FirstOrDefaultAsync(cancellationToken) ?? "";
            }

        }

    }
}
