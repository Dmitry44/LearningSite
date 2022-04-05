using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class Package
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public bool IsActive { get; set; }

        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; } = new();
    }
}
