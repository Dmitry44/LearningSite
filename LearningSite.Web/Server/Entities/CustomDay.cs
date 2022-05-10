using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class CustomDay
    {
        [Key]
        public DateOnly Day { get; set; }
    }
}
