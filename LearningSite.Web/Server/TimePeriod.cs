namespace LearningSite.Web.Server
{
    public struct TimePeriod
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }
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
