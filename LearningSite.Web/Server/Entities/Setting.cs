using System.ComponentModel.DataAnnotations;

namespace LearningSite.Web.Server.Entities
{
    public class Setting
    {
        [Key]
        public string Key { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
