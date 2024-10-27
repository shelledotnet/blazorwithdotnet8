using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TempleHill.Application.Extensions.Identity;

namespace TempleHill.Infrastructure.DataAccess
{
    public class THDbContext(DbContextOptions<THDbContext> options) :
        IdentityDbContext<ApplicationUser>(options)
    {
    }
}
