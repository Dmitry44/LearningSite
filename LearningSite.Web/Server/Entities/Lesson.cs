using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Minutes { get; set; }
        public bool IsActive { get; set; }
    }
}
