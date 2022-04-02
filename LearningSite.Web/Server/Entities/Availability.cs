using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class Availability
    {
        [Key]
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public string StartTime { get; set; } = "";
        public string EndTime { get; set; } = "";
    }
}
