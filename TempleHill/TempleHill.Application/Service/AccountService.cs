using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempleHill.Application.DTO.Request.Identity;
using TempleHill.Application.DTO.Response;
using TempleHill.Application.DTO.Rresponse.Identity;
using TempleHill.Application.Interfaces;

namespace TempleHill.Application.Service;

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    public async Task<ServiceResponse> CreateUserAsync(CreateUserRequestDto model)
        => await accountRepository.CreateUserAsync(model);
    
    public async Task<IEnumerable<GetUserWithClaimsResponseDto>> GetUserWithClaimAsync()
        =>await accountRepository.GetUserWithClaimAsync();
    

    public async Task<ServiceResponse> LoginAsync(LoginUserRequestDto model)
        =>await accountRepository.LoginAsync(model);
    

    public async Task SetUpAsync()
        =>await accountRepository.SetUpAsync();
    

    public async Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDto model)
        =>await accountRepository.UpdateUserAsync(model);
    
}
