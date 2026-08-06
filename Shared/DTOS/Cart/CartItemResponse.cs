namespace Shared.DTOS.Cart
{
    public class CartItemResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string ThumbnailUrl { get; set; } = null!;
        public decimal Price { get; set; }
        public string Level { get; set; } = null!;
        public int TotalDurationMinutes { get; set; }
        public string InstructorName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
    }
}
