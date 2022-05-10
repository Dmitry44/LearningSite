using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class AvailabilityDef
    {
        [Key]
        public int Id { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }
}
