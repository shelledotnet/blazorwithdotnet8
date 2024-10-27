using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TempleHill.Application.DTO.Request.Identity;
using TempleHill.Application.DTO.Response;
using TempleHill.Application.DTO.Rresponse.Identity;
using TempleHill.Application.Extensions.Identity;
using TempleHill.Application.Interfaces;

namespace TempleHill.Infrastructure.Repository;

public class AccountRepository
    (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IAccountRepository
{
    #region ActionMethod
    public async Task<ServiceResponse> CreateUserAsync(CreateUserRequestDto model)
    {
        ApplicationUser? user = await FindUserByEmail(model.Email);
        if (user != null)
        {
            return new ServiceResponse(false, "user already exist");
        }

        ApplicationUser newUser = new()
        {
            UserName = model.Email,
            PasswordHash = model.Password,
            Email = model.Email,
            Name = model.Name
        };

        var result = CheckResult(await userManager.CreateAsync(newUser, model.Password));
        if (!result.Flag)
            return result;
        else
            return await CreateUserClaims(model);
        //as soon as you have your acct we are creating default claims for you

    }

    public async Task<IEnumerable<GetUserWithClaimsResponseDto>> GetUserWithClaimAsync()
    {
        List<GetUserWithClaimsResponseDto> userList = new();
        List<ApplicationUser> allUser = userManager.Users.ToList();
        if(allUser.Count == 0)
            return userList;
        foreach (ApplicationUser user in allUser)
        {
            var currentUser = await userManager.FindByIdAsync(user.Id);
            var getCurrentUserclaims = await userManager.GetClaimsAsync(currentUser);
            if (getCurrentUserclaims.Any())
            {
                userList.Add(new GetUserWithClaimsResponseDto()
                {
                    UserId = user.Id,
                    Email = getCurrentUserclaims.FirstOrDefault(_ => _.Type == ClaimTypes.Email).Value,
                    RoleName = getCurrentUserclaims.FirstOrDefault(_ => _.Type == ClaimTypes.Role).Value,
                    Name = getCurrentUserclaims.FirstOrDefault(_ => _.Type == ClaimTypes.Name).Value,
                    ManageUser = Convert.ToBoolean(getCurrentUserclaims.FirstOrDefault(_ => _.Type == "ManageUser").Value),
                    Create = Convert.ToBoolean(getCurrentUserclaims.FirstOrDefault(_ => _.Type == "Create").Value),
                    Update = Convert.ToBoolean(getCurrentUserclaims.FirstOrDefault(_ => _.Type == "Update").Value),
                    Delete = Convert.ToBoolean(getCurrentUserclaims.FirstOrDefault(_ => _.Type == "Delete").Value),
                    Read = Convert.ToBoolean(getCurrentUserclaims.FirstOrDefault(_ => _.Type == "Read").Value)
                });
            }

        }

        return userList;
    }

    public async Task<ServiceResponse> LoginAsync(LoginUserRequestDto model)
    {
        ApplicationUser? user = await FindUserByEmail(model.Email);
        if (user is null)
            return new ServiceResponse(false, "user not found");
        var verifyPassword = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if(!verifyPassword.Succeeded)
            return new ServiceResponse(false, "incorrect username or password");
        var result = await signInManager.PasswordSignInAsync(user, model.Password,false,false);
        if (!result.Succeeded)
            return new ServiceResponse(false, "unknown error occurred while logging you in");
        else
          return new ServiceResponse(true, null);
    }

    //this is for seeding admin details  at application startup
    public async Task SetUpAsync() => await CreateUserAsync(new CreateUserRequestDto()
    {
        Name = "Administrator",
        Email = "admin@admin.com",
        Policy = Policy.AdminPolicy,
        Password = "Admin@123",
        ConfirmPassword = "Admin@123"
    });
    

    public async Task<ServiceResponse> UpdateUserAsync(ChangeUserClaimRequestDto model)
    {
        var user = await userManager.FindByIdAsync(model.UserId);
        if (user == null)
            return new ServiceResponse(false, "user not found");
        var oldUserClaims = await userManager.GetClaimsAsync(user);
        Claim[] claims = 
            [
                     new Claim(ClaimTypes.Email,user.Email),
                     new Claim(ClaimTypes.Role,model.RoleName),
                     new Claim("Name",model.Name),
                     new Claim("Create",model.Create.ToString()),
                     new Claim("Update",model.UserId.ToString()),
                     new Claim("Read",model.Read.ToString()),
                     new Claim("Delete",model.Delete.ToString()),
                     new Claim("ManageUser",model.ManageUser.ToString())

            ];
        var removeClaims = CheckResult(await userManager.RemoveClaimsAsync(user,oldUserClaims));
        if(!removeClaims.Flag) return removeClaims;

        var result = CheckResult(await userManager.AddClaimsAsync(user, claims));
        if (result.Flag) return new ServiceResponse(true, "user updated successfully");
        else return result;

    }

    #endregion

    #region NonActionMethod
    private static ServiceResponse CheckResult(IdentityResult result)
    {
        if (result.Succeeded) return new ServiceResponse(true, null);

        IEnumerable<string>? error = result.Errors.Select(_ => _.Description);
        return new ServiceResponse(false, string.Join(Environment.NewLine, error));
    }

    private async Task<ApplicationUser> FindUserByEmail(string email)
        => await userManager.FindByEmailAsync(email);


    private async Task<ApplicationUser> FindUserById(string id)
       => await userManager.FindByIdAsync(id);


    private async Task<ServiceResponse> CreateUserClaims(CreateUserRequestDto model)
    {
        if (string.IsNullOrEmpty(model.Policy)) return new ServiceResponse(false, "no policy specify");

        #region Claims
        Claim[] userClaims = [];
        if (model.Policy.Equals(Policy.AdminPolicy, StringComparison.OrdinalIgnoreCase))
        {
            userClaims =
                [
                     new Claim(ClaimTypes.Email,model.Email),
                     new Claim(ClaimTypes.Role,"Admin"),
                     new Claim("Name","true"),
                     new Claim("Create","true"),
                     new Claim("Update","true"),
                     new Claim("Read","true"),
                     new Claim("Delete","true"),
                     new Claim("ManageUser","true")
                ];

        }
        else if (model.Policy.Equals(Policy.ManagerPolicy, StringComparison.OrdinalIgnoreCase))
        {
            userClaims =
               [
                    new Claim(ClaimTypes.Email,model.Email),
                     new Claim(ClaimTypes.Role,"Manager"),
                     new Claim("Name","true"),
                     new Claim("Create","true"),
                     new Claim("Update","true"),
                     new Claim("Read","true"),
                     new Claim("Delete","false"),
                     new Claim("ManageUser","false")
               ];
        }
        else if (model.Policy.Equals(Policy.UserPolicy, StringComparison.OrdinalIgnoreCase))
        {
            userClaims =
               [
                    new Claim(ClaimTypes.Email,model.Email),
                     new Claim(ClaimTypes.Role,"User"),
                     new Claim("Name","true"),
                     new Claim("Create","false"),
                     new Claim("Update","false"),
                     new Claim("Read","false"),
                     new Claim("Delete","false"),
                     new Claim("ManageUser","false")
               ];
        }

        #endregion

        ServiceResponse result = CheckResult(await userManager.AddClaimsAsync((await FindUserByEmail(model.Email)),userClaims));
        if (result.Flag)
            return new ServiceResponse(true, "user created successfully");
        else
            return result;
    } 
    #endregion

}
 