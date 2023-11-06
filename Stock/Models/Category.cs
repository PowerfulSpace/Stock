using System.ComponentModel.DataAnnotations;

namespace Stock.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(25)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(75)]
        public string Description { get; set; } = null!;
    }
}
