using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Courses.Enums;
using Domain.Entities.Identity;
using Domain.Exceptions.BadRequestExceptions;
using Services.Specifications.UserSpecifications;
using ServicesAbstraction.Auth;
using ServicesAbstraction.Users;
using Shared.DTOS.Admin;

namespace Services
{
    public class AdminServices(
        IUnitOfWork _uof,
         IMapper _mapper,
         IAuthService _auth) : IAdminService
    {
        public async Task<IEnumerable<InstructorRequestResponse>> GetInstructorRequestsAsync(RequestStatus? status, CancellationToken ct)
        {
            var spec = new InstructorSpecifications(status.Value);
            var res = await _uof.GetRepository<Guid,InstructorRequest>().GetAllAsync(spec,ct);

            return _mapper.Map<IEnumerable<InstructorRequestResponse>>(res);
        }

        public async Task<ApproveInstructorResponse> ApproveRequestAsync(Guid requestId, CancellationToken ct)
        {
            var spec = new InstructorSpecifications();
            var res = await _uof.GetRepository<Guid,InstructorRequest>().GetAsync(spec,requestId,ct);

            if(res.Status is RequestStatus.Approved || res.Status is RequestStatus.Rejected) throw new BadRequestException("this Instructor Request is already reviewd");
            
            res.Status = RequestStatus.Approved;
            var profile = new InstructorProfile()
            {
              AppUser = res.User,  
            };
            res.User.InstructorProfile = profile;

            await _uof.SaveChangesAsync(ct);

            return new ApproveInstructorResponse(){
                Message=$"Congrats {res.User.UserName}, now you're a instructor, you can publish your courses now",
                Token = await _auth.GenerateTokenAsync(res.User)
            };
        }


        public async Task<ApproveInstructorResponse> RejectRequestAsync(Guid requestId, CancellationToken ct)
        {
            var spec = new InstructorSpecifications();
            var res = await _uof.GetRepository<Guid,InstructorRequest>().GetAsync(spec,requestId,ct);

            res.Status = RequestStatus.Rejected;
            await _uof.SaveChangesAsync(ct);

            return new ApproveInstructorResponse(){
                Message=$"After Deep looking, Unfortunatily you're not Qualified to be an instructor"
            };
        }
    }
}