using TempleHill.Application.DTO.Request.Identity;
using TempleHill.Application.DTO.Response;
using TempleHill.Application.DTO.Rresponse.Identity;

namespace TempleHill.Application.Interfaces;

public interface IAccountRepository
{
    Task<ServiceResponse> LoginAsync(LoginUserRequestDto model);

    Task<ServiceResponse> CreateUserAsync(CreateUserRequestDto model);

    Task<IEnumerable<GetUserWithClaimsResponseDto>> GetUserWithClaimAsync();

    Task SetUpAsync();

    Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDto model);

    //Task  SaveActivityAsync(ActivityTrackerRequestDto model);

    //Task<IEnumerable<ActivityTrackerReponsetDto>> GetActivityAsync();


}
