using Domain.Entities.Courses.Enums;
using Domain.Entities.Identity;

namespace Services.Specifications.UserSpecifications
{
    public class InstructorCountSpec : Specifications<Guid, InstructorRequest>
    {
        public InstructorCountSpec(RequestStatus? status) : base(
            x => !status.HasValue || x.Status == status.Value)
        {
        }
    }
}
