using System.ComponentModel.DataAnnotations;

namespace CakeVault.Models
{
    public class CakeDTO
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, MinimumLength = 5)]
        public string Comment { get; set; } = string.Empty;

        [Required, Url]
        public string ImageUrl { get; set; } = string.Empty;

        [Range(1, 5)]
        public int YumFactor { get; set; }
    }
}
