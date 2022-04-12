using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Decription { get; set; } = "";
        public bool IsActive { get; set; }

        public virtual List<Package> Packages { get; set; } = new();
    }
}
