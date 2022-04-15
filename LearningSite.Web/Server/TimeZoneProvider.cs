namespace LearningSite.Web.Server
{
    public class TimeZoneProvider
    {
        public List<Info> TimeZones { get; private set; } = new();

        public TimeZoneProvider()
        {
            System.Collections.ObjectModel.ReadOnlyCollection<TimeZoneInfo> tzCollection = TimeZoneInfo.GetSystemTimeZones();
            foreach (var tzi in tzCollection)
            {
                var row = new Info() { SystemId = tzi.Id, SystemName = tzi.DisplayName };

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
            public string SystemId { get; set; } = "";
            public string SystemName { get; set; } = "";
            public string IanaId { get; set; } = "";
        }

    }
}
