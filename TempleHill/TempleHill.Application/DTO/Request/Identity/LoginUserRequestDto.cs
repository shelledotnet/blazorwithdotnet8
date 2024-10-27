using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempleHill.Application.DTO.Request.Identity;

public class LoginUserRequestDto
{
    [EmailAddress]
    [RegularExpression("[^@ \\t\\r\\n]+@[^@ \\t\\r\\n]+\\.[^@ \\t\\r\\n]+", ErrorMessage = "Your {0} is invalid")]
    public string? Email { get; set; }

    [Required]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$",ErrorMessage ="Your {0} is invalid")]
    [MinLength(8,ErrorMessage ="{0} min length is 8"),MaxLength(100, ErrorMessage = "{0} max length is 100")]
    public string? Password { get; set; }

    
}
