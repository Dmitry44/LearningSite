using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class Availability
    {
        [Key]
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsDef { get; set; }
    }
}
