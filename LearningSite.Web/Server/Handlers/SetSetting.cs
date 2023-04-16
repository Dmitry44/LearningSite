using LearningSite.Web.Server.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LearningSite.Web.Server.Handlers
{
    public class SetSetting
    {
        public record Request(string Key, string Value) : IRequest;

        public class Handler : IRequestHandler<Request>
        {
            private readonly AppDbContext db;

            public Handler(AppDbContext db)
            {
                this.db = db;
            }

            public async Task Handle(Request request, CancellationToken cancellationToken)
            {
                var setting = await db.Settings.FirstOrDefaultAsync(x => x.Key == request.Key, cancellationToken);
                if (setting is null)
                {
                    setting = new Setting() { Key = request.Key, Value = request.Value };
                    db.Settings.Add(setting);
                }
                else
                {
                    setting.Value = request.Value;
                }
                await db.SaveChangesAsync();
            }

        }

    }
}
