using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class AvailabilityDef
    {
        [Key]
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
