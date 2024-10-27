using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleHill.Application.DTO.Request.Identity;
using TempleHill.Application.DTO.Response;
using TempleHill.Application.DTO.Rresponse.Identity;

namespace TempleHill.Application.Service
{
    public interface IAccountService
    {

        Task<ServiceResponse> LoginAsync(LoginUserRequestDto model);

        Task<ServiceResponse> CreateUserAsync(CreateUserRequestDto model);

        Task<IEnumerable<GetUserWithClaimsResponseDto>> GetUserWithClaimAsync();

        Task SetUpAsync();

        Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDto model);

        //Task  SaveActivityAsync(ActivityTrackerRequestDto model);

        //Task<IEnumerable<ActivityTrackerReponsetDto>> GetActivityAsync();
    }
}
