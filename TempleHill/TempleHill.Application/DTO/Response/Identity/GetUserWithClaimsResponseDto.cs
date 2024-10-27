using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleHill.Application.DTO.Rresponse.Identity;

public class GetUserWithClaimsResponseDto : BaseUserClaimsDto
{
    public string? Email { get; set; } 
}
