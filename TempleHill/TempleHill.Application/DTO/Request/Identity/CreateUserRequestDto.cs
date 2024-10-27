using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleHill.Application.DTO.Request.Identity;

public class CreateUserRequestDto : LoginUserRequestDto
{
    [Required]
    public string? Name { get; set; }

    [Required]
    public string? ConfirmPassword { get; set; }    

    [Required]
    public string? Policy { get; set; }   
}
