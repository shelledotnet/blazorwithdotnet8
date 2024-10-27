using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TempleHill.Application.Extensions.Identity;


public class ApplicationUser : IdentityUser
{
    public string? Name { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime CreatedDate { get; set; }

    public bool Active { get; set; }

    public bool Blocked { get; set; }

    public bool Expired { get; set; }
}
