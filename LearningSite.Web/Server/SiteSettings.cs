using LearningSite.Web.Server.Handlers;
using MediatR;
using System.Text.Json;

namespace LearningSite.Web.Server
{
    public class SiteSettingsProps
    {
        public string AvailabilityTimeZoneId { get; set; } = "";
    }

    public class SiteSettings : SiteSettingsProps
    {
        private readonly IServiceProvider serviceProvider;

        public SiteSettings(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            Load().Wait();
        }

        public async Task Load(string key = "SiteSettings")
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var jsonString = await mediator.Send(new GetSetting.Request(key));

                SiteSettingsProps? siteSettings = null;
                try
                {
                    siteSettings = JsonSerializer.Deserialize<SiteSettingsProps>(jsonString)!;
                }
                catch { }

                if (siteSettings is null) return;

                AvailabilityTimeZoneId = siteSettings.AvailabilityTimeZoneId;
            }
        }

        public async Task Save(string key = "SiteSettings")
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var jsonString = JsonSerializer.Serialize(this);
                await mediator.Send(new SetSetting.Request(key, jsonString));
            }
        }

    }
}
