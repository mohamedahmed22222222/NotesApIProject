using System.ComponentModel.DataAnnotations;

namespace Notes.Api.DTOUser
{
    public class DtoNewUser
    {
        [Required]
        public string UserName { get; set; }= string.Empty;
        [Required]
        [StringLength(50,MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string? Phone { get; set; } = string.Empty;
    }
}
