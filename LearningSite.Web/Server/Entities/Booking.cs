using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public int UserId { get; set; }
        public virtual AppUser User { get; set; } = new();

        public int PurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; } = new();
    }
}
