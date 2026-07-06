
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Notes.Api.DTO
{
    public class CreateDtoNote
    {
        [Required]
        [StringLength(100,MinimumLength =5)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(400)]
        public string Content { get; set; } = string.Empty ;
    }
}
