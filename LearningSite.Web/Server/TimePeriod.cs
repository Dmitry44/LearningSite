using System.Collections.Generic;

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

    public class TimePeriod : IComparable
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

        public bool InsideIn(TimePeriod other)
        {
            return (Start >= other.Start && End <= other.End);
        }

        public int CompareTo(object? obj)
        {
            if (obj is null) return 1;
            var other = (TimePeriod)obj;
            return Start.CompareTo(other.Start);
        }
    }

    public class TimeLine
    {
        private readonly SortedSet<TimePeriod> periods = new();

        public List<TimePeriod> Periods { get => periods.ToList(); }

        public void AddPeriod(TimePeriod newPeriod)
        {
            if (!periods.Any())
            {
                periods.Add(newPeriod);
                return;
            }

            var notIntersects = true;
            foreach (var period in periods)
            {
                if (!newPeriod.IntersectsWith(period)) continue;
                notIntersects = false;

                //intersects

                //ignore newPeriod when it is inside period
                if (newPeriod.InsideIn(period)) break;

                //expand aperiod when newPeriod starts earlier
                if (newPeriod.Start < period.Start) period.Start = newPeriod.Start;

                //process rest of newPeriod separately
                if (newPeriod.End > period.End)
                {
                    AddPeriod(new(period.End, newPeriod.End));
                    break;
                }
            }
            if (notIntersects) periods.Add(newPeriod);
        }

        public void SubstractPeriod(TimePeriod substPeriod)
        {
            if (!periods.Any()) return;

            foreach (var period in periods)
            {
                if (!substPeriod.IntersectsWith(period)) continue;

                //intersects

                //substPeriod inside period
                if (substPeriod.InsideIn(period))
                {
                    //remove original period
                    periods.Remove(period);
                    //add peace at the beginning
                    if (substPeriod.Start > period.Start) periods.Add(new(period.Start, substPeriod.Start));
                    //add peace at the end
                    if (substPeriod.End < period.End) periods.Add(new(substPeriod.End, period.End));
                    break;
                }

                //period inside substPeriod
                if (period.InsideIn(substPeriod))
                {
                    //remove such period
                    periods.Remove(period);
                    //process rest of substPeriod separately
                    if (substPeriod.End > period.End) SubstractPeriod(new(period.End, substPeriod.End));
                    break;
                }

                //substPeriod starts before period
                if (substPeriod.Start < period.Start) period.Start = substPeriod.End;

                //substPeriod starts after period
                if (substPeriod.End > period.End) period.End = substPeriod.Start;
            }
        }

        public void CombinePeriods()
        {
            if (periods.Count <= 1) return;

            List<TimePeriod> delete = new();

            var curPeriod = periods.First();
            foreach (var period in periods)
            {
                if (period == curPeriod) continue;

                if (curPeriod.End == period.Start)
                {
                    delete.Add(period);
                    curPeriod.End = period.End;
                    continue;
                }

                curPeriod = period;
            }

            if (delete.Any()) periods.RemoveWhere(period => delete.Contains(period));
        }

        public bool CheckLinearity()
        {
            var curDate = DateTime.MinValue;
            foreach (var period in periods)
            {
                if (period.Start < curDate) return false;
                curDate = period.End;
            }
            return true;
        }

    }

}
