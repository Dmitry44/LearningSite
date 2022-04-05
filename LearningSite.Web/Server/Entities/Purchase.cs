using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Minutes { get; set; }
        public int Quantity { get; set; }
        public int BookedQuantity { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }

        public int LessonId { get; set; }
        public virtual Lesson Lesson { get; set; } = new();

        public int UserId { get; set; }
        public virtual AppUser User { get; set; } = new();

        public int PackageId { get; set; }
        public virtual Package Package { get; set; } = new();

    }
}
