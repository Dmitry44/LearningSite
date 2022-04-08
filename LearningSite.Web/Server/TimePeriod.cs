namespace LearningSite.Web.Server
{
    public enum TimeSlotStatus : byte
    {
        None = 0,
        Enabled = 1,
        Disabled = 2,
        Closed = 3
    }

    public class TimeSlot : TimePeriod
    {
        public TimeSlotStatus Status { get; set; }

        public TimeSlot(DateTime start, DateTime end, TimeSlotStatus status = TimeSlotStatus.None)
            : base(start, end)
        {
            Status = status;
        }
    }

    public class TimePeriod
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public TimeSpan Duration { get => End - Start; }

        public TimePeriod(DateTime start, DateTime end)
        {
            if (start > end) throw new ArgumentException();
            Start = start;
            End = end;
        }

        public bool IntersectsWith(TimePeriod other)
        {
            return (Start >= other.Start && Start < other.End)
                || (End > other.Start && End <= other.End)
                || (Start < other.Start && End > other.End);
        }
    }

}
