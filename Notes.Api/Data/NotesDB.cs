using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notes.Api.Model;
namespace Notes.Api.Data
{
    public class NotesDB : IdentityDbContext<AppUser> 
    {
        public NotesDB(DbContextOptions<NotesDB> options) : base(options)
        {
            
        }

        public DbSet<Note> Notes { get; set; }
    }
}
