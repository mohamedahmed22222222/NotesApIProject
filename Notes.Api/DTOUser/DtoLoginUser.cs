
using System.ComponentModel.DataAnnotations;

namespace Notes.Api.DTOUser
{
    public class DtoLoginUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
