using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Courses.Enums;
using Domain.Entities.Identity;
using Domain.Exceptions.BadRequestExceptions;
using Domain.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Identity;
using Services.Specifications.UserSpecifications;
using ServicesAbstraction.Users;
using Shared.DTOS.Admin;

namespace Services
{
    public class AdminServices(
        IUnitOfWork _uof,
         IMapper _mapper,
         UserManager<AppUser> _userManager) : IAdminService
    {
        public async Task<IEnumerable<InstructorRequestResponse>> GetInstructorRequestsAsync(RequestStatus? status, CancellationToken ct)
        {
            var spec = status.HasValue? new InstructorSpecifications(status.Value) : new InstructorSpecifications();
            var res = await _uof.GetRepository<Guid,InstructorRequest>().GetAllAsync(spec,ct);

            return _mapper.Map<IEnumerable<InstructorRequestResponse>>(res);
        }

        public async Task ApproveRequestAsync(Guid requestId, CancellationToken ct)
        {
            var spec = new InstructorSpecifications();
            var res = await _uof.GetRepository<Guid,InstructorRequest>().GetAsync(spec,requestId,ct);

            if(res is null) throw new UserNotFoundException(requestId.ToString());

            if(res.Status is RequestStatus.Approved || res.Status is RequestStatus.Rejected) 
                throw new BadRequestException("this Instructor Request is already reviewed");
            
            res.Status = RequestStatus.Approved;
            res.UpdatedAt = DateTime.UtcNow;
            await _userManager.AddToRoleAsync(res.User,"Instructor");
            await _uof.SaveChangesAsync(ct);
        }


        public async Task RejectRequestAsync(Guid requestId, CancellationToken ct)
        {
            var spec = new InstructorSpecifications();
            var res = await _uof.GetRepository<Guid,InstructorRequest>().GetAsync(spec,requestId,ct);

            if(res is null) throw new UserNotFoundException(requestId.ToString());

            if(res.Status is RequestStatus.Approved || res.Status is RequestStatus.Rejected) 
                throw new BadRequestException("this Instructor Request is already reviewed");

            res.Status = RequestStatus.Rejected;
            res.UpdatedAt= DateTime.UtcNow;
            await _uof.SaveChangesAsync(ct);
        }
    }
}