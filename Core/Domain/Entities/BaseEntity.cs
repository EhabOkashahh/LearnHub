namespace Domain.Entities
{
    public class BaseEntity<TKey> : ISoftDeletable
    {
        public TKey Id { get; init; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}