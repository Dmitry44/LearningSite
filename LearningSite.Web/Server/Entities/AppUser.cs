using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningSite.Web.Server.Entities
{
    [Index(nameof(EmailAddress), IsUnique = true)]
    public class AppUser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string Salt { get; set; } = "";
        public bool IsAdmin { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
