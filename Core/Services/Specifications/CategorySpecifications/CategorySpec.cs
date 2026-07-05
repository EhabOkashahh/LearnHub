using Domain.Entities.Courses;

namespace Services.Specifications.CategorySpecifications
{
    public class CategorySpec : Specifications<Guid, Category>
    {
        public CategorySpec(Guid id) : base(C => C.Id == id)
        {
        }
        public CategorySpec() : base(null)
        {
        }
    }
}
