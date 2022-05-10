using Microsoft.AspNetCore.Mvc.Rendering;

namespace LearningSite.Web.Server
{
    public class TimeZoneProvider
    {
        public List<Info> TimeZones { get; private set; } = new();

        public SelectListItem[] GetTimeZoneList(string selectedValue)
        {
            return TimeZones
                    .Select(tz => new SelectListItem(tz.SystemName, tz.IanaId, tz.IanaId == selectedValue))
                    .ToArray();
        }

        public TimeZoneProvider()
        {
            var tzCollection = TimeZoneInfo.GetSystemTimeZones();
            foreach (var tzi in tzCollection)
            {
                var row = new Info() { SystemName = tzi.DisplayName };

                if (tzi.HasIanaId)
                {
                    row.IanaId = tzi.Id;  // no conversion necessary
                    TimeZones.Add(row);
                    continue;
                }

                if (TimeZoneInfo.TryConvertWindowsIdToIanaId(tzi.Id, out string? ianaId))
                {
                    row.IanaId = ianaId;  // use the converted ID
                    TimeZones.Add(row);
                }
            }
        }

        public class Info
        {
            public string SystemName { get; set; } = "";
            public string IanaId { get; set; } = "";
        }

    }
}
