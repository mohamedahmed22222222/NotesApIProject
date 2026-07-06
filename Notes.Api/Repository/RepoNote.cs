using Microsoft.EntityFrameworkCore;
using Notes.Api.Data;
using Notes.Api.Model;

namespace Notes.Api.Repository
{
    public class RepoNote
    {
        NotesDB context;
        public RepoNote(NotesDB _context)
        {
            context= _context;
        }

        public List<Note> GetNoteswithPaging(int page,int pageSize,string userId) 
        {
            var skip = (page - 1) * pageSize;
            return context.Notes.Where(n => n.UserId == userId).Skip(skip).Take(pageSize).ToList();

        }
        public List<Note> GetNotes(string userId) 
        {
            
            return context.Notes.Where(n=>n.UserId==userId).ToList();

        }

        public Note? GetNoteByID(int id, string userId) 
        {
        
        return context.Notes.FirstOrDefault(x => x.Id == id && x.UserId==userId);
            
        
        }
        public Note? GetNoteByName(string name,string userId) 
        {
        
        return context.Notes.FirstOrDefault(x => x.Title == name&&x.UserId==userId);
            
        
        }

        public List<Note> searchByKeyword(string search,string userId)
        {

            return context.Notes.Where(n=>n.UserId==userId&&(n.Title.Contains(search)||n.Content.Contains(search))).ToList();
        }



        public void AddNote(Note note) 
        { 
        context.Notes.Add(note);
        
        }

        public void UpdateNote(Note note) 
        {
        context.Notes.Update(note);
        
        }

        public void DeleteNote(Note note) 
        {

            context.Notes.Remove(note!); 
        
        
        }

        public void Save()
        {
            context.SaveChanges();
        }


    }
}
