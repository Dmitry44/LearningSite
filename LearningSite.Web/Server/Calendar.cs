namespace LearningSite.Web.Server
{
    public class Calendar
    {
        public List<TimePeriod> GetDaySlots(DateTime day, int minutes)
        {
            List<TimePeriod> slots = new();
            DateTime curTime = day.Date;

            while (curTime.Date == day.Date)
            {
                slots.Add(new TimePeriod(curTime, curTime = curTime.AddMinutes(minutes)));
            }

            return slots;
        }

        public void UpdateSlotStatuses(List<TimeSlot> slots, List<TimePeriod> periods, TimeSlotStatus status)
        {

        }
    }
}
