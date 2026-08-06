using System.ComponentModel.DataAnnotations;

namespace Shared.DTOS.Courses
{
    public class CreateCourseRequest
    {
        [Required, StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [StringLength(2000)]
        public string? Description { get; set; }

        [Required, Url]
        public string ThumbnailUrl { get; set; } = null!;

        [Range(0, 99999.99)]
        public decimal Price { get; set; }

        [Range(0, 99999.99)]
        public decimal? DiscountPrice { get; set; }

        public DateTime? DiscountEndsAt { get; set; }

        [Required]
        public string Level { get; set; } = null!;

        [Required]
        public Guid CategoryId { get; set; }
    }
}
