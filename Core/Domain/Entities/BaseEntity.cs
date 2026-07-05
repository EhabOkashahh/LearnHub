namespace Domain.Entities
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; init; } = default!;
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}