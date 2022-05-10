using LearningSite.Web.Server.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LearningSite.Web.Server.Handlers
{
    public class GetSchedule
    {
        public record Request(int UserId, string TimeZoneId) : IRequest<Response>
        {
            public int PurchaseId { get; set; }
            public DateOnly Date1 { get; set; }
            public int Days { get; set; } = 7;
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly AppDbContext db;
            private readonly SiteSettings siteSettings;

            public Handler(AppDbContext db, SiteSettings siteSettings)
            {
                this.db = db;
                this.siteSettings = siteSettings;
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
                }

                TimeZoneInfo avalTz = TimeZoneInfo.FindSystemTimeZoneById(siteSettings.AvailabilityTimeZoneId);
                TimeZoneInfo userTz = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZoneId);

                var date2 = request.Date1.AddDays(request.Days - 1);
                var date1Ext = request.Date1.AddDays(-1);
                var date2Ext = date2.AddDays(1);
                var timeLine = new TimeLine();

                //read default teacher availability (teacher time zone)
                var availDef = (await db.AvailabilityDefs.AsNoTracking().ToListAsync())
                    .ToLookup(x => x.DayOfWeek);
                for (DateOnly d = date1Ext; d < date2Ext; d = d.AddDays(1))
                {
                    var avails = availDef[d.DayOfWeek];
                    foreach (var av in avails)
                    {
                        var start = TimeZoneInfo.ConvertTimeToUtc(d.ToDateTime(av.Start), avalTz);
                        var end = TimeZoneInfo.ConvertTimeToUtc(d.ToDateTime(av.End), avalTz);
                        TimePeriod period = new(start, end);
                        timeLine.AddPeriod(period);
                    }
                }

                //read teacher custom days (teacher time zone)
                var customDays = await db.CustomDays
                    .Where(x => x.Day >= date1Ext && x.Day <= date2Ext)
                    .Select(x => x.Day).ToArrayAsync();
                foreach (var d in customDays)
                {
                    var start = TimeZoneInfo.ConvertTimeToUtc(d.ToDateTime(TimeOnly.MinValue), avalTz);
                    var end = TimeZoneInfo.ConvertTimeToUtc(d.ToDateTime(TimeOnly.MaxValue), avalTz);
                    TimePeriod period = new(start, end);
                    timeLine.SubstractPeriod(period);
                }

                //read custom teacher availability (UTC)
                var availabilities = await db.Availabilities.AsNoTracking().ToListAsync();
                foreach (var av in availabilities)
                {
                    TimePeriod period = new(av.Start, av.End);
                    timeLine.AddPeriod(period);
                }

                //read bookings
                var bookings = await db.Bookings
                    .Where(x => x.Start >= date1Ext.ToDateTime(TimeOnly.MinValue)
                        && x.Start <= date2Ext.ToDateTime(TimeOnly.MinValue))
                    .Select(x => new
                    {
                        IsCurUser = x.UserId == request.UserId,
                        x.Start,
                        x.End
                    })
                    .ToArrayAsync();
                foreach (var b in bookings)
                {
                    TimePeriod period = new(b.Start, b.End);
                    timeLine.SubstractPeriod(period);
                }

                //convert to user time
                foreach (var period in timeLine.Periods)
                {
                    period.Start = TimeZoneInfo.ConvertTimeFromUtc(period.Start, userTz);
                    period.End = TimeZoneInfo.ConvertTimeFromUtc(period.End, userTz);
                }

                var response = new Response();
                response.Caption = request.Date1.ToString("MMM yyyy", CultureInfo.InvariantCulture);
                if (date2.Month > request.Date1.Month || date2.Year > request.Date1.Year)
                    response.Caption += $" - {date2.ToString("MMM yyyy", CultureInfo.InvariantCulture)}";

                for (DateOnly d = request.Date1; d <= date2; d = d.AddDays(1))
                {
                    response.Days.Add(new(d.ToString("ddd, MMM d", CultureInfo.InvariantCulture), d));
                    var periods = timeLine.Periods
                        .Where(x => x.Start.Date == d.ToDateTime(TimeOnly.MinValue)
                            || x.End.Date == d.ToDateTime(TimeOnly.MinValue))
                        .ToList();
                }

                return response;
            }

        }

        public record Response
        {
            public bool IsEmpty {
                get => this.Days.SelectMany(x => x.Slots).Any(s => s. Status == TimeRowStatus.Enabled);
            }
            public string Caption { get; set; } = "";
            public List<DayCol> Days { get; set; } = new();
        }

        public record DayCol(string Caption, DateOnly Day)
        {
            public List<TimeRow> Slots { get; set; } = new();
        }

        public record TimeRow(
            string Caption,
            TimeOnly Start,
            TimeOnly End,
            TimeRowStatus Status);

        public enum TimeRowStatus : byte
        {
            Disabled = 0,
            Enabled = 1,
            Selected = 2
        }

    }
}
