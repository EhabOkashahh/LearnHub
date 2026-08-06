using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Enums;
using Domain.Entities.Identity;
using Domain.Exceptions.BadRequestExceptions;
using Domain.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Identity;
using Services.Specifications.UserSpecifications;
using ServicesAbstraction.Users;
using Shared.DTOS;
using Shared.DTOS.Admin;
using Shared.DTOS.Courses;

namespace Services
{
    public class AdminServices(
        IUnitOfWork _uof,
         IMapper _mapper,
         UserManager<AppUser> _userManager) : IAdminService
    {
        public async Task<PaginatedResponse<InstructorRequestResponse>> GetInstructorRequestsAsync(RequestStatus? status, int pageIndex, int pageSize, CancellationToken ct)
        {
            var spec = new InstructorSpecifications(status, pageIndex, pageSize);
            var res = await _uof.GetRepository<Guid, InstructorRequest>().GetAllAsync(spec, ct);

            var countSpec = new InstructorCountSpec(status);
            var totalCount = await _uof.GetRepository<Guid, InstructorRequest>().GetCountAsync(countSpec);

            var mapped = _mapper.Map<IEnumerable<InstructorRequestResponse>>(res);
            return new PaginatedResponse<InstructorRequestResponse>(pageIndex, pageSize, totalCount, mapped);
        }

        public async Task ApproveRequestAsync(Guid requestId, CancellationToken ct)
        {
            var spec = new InstructorSpecifications(requestId);
            var res = await _uof.GetRepository<Guid,InstructorRequest>().GetAsync(spec,ct);

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
            var spec = new InstructorSpecifications(requestId);
            var res = await _uof.GetRepository<Guid,InstructorRequest>().GetAsync(spec,ct);

            if(res is null) throw new UserNotFoundException(requestId.ToString());

            if(res.Status is RequestStatus.Approved || res.Status is RequestStatus.Rejected) 
                throw new BadRequestException("this Instructor Request is already reviewed");

            res.Status = RequestStatus.Rejected;
            res.UpdatedAt= DateTime.UtcNow;
            await _uof.SaveChangesAsync(ct);
        }
    }
}
