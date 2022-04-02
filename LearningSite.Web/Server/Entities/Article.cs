using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Slug { get; set; } = "";
        public string Content { get; set; } = "";
        public DateTime? PublishedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
