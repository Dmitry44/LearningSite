using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class AvailabilityDef
    {
        [Key]
        public int Id { get; set; }
        public int DayOfWeek { get; set; }
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
    }
}
