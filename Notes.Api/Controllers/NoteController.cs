using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.DTO;
using Notes.Api.Model;
using Notes.Api.Repository;
using System.Security.Claims;

namespace Notes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        RepoNote repo;

        public NoteController(RepoNote _repo)
        {
            
            repo = _repo;
        }

        [HttpGet("withPaging")]

        public IActionResult GetAllNotes(int page, int pageSize)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (pageSize <= 0 || pageSize <= 0) return BadRequest("Page and PageSize must be greater than 0.");
            List<Note> notes = repo.GetNoteswithPaging(page,pageSize,userId);

            var result = notes.Select(n => new GetDtoNote
            {
                Id = n.Id,
                Content = n.Content,
                Created = n.Created,
                Title = n.Title



            }).ToList();
            return Ok(result);
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetAllNotes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            List<Note> notes = repo.GetNotes(userId);

            var result = notes.Select(n => new GetDtoNote
            {
                Id = n.Id,
                Content = n.Content,
                Created = n.Created,
                Title = n.Title



            }).ToList();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize]

        public IActionResult GetNoteById(int id) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var note = repo.GetNoteByID(id,userId);
            if(note == null ) { return NotFound(); }
            var result = new GetDtoNote
            {

                Id = note.Id,
                Content = note.Content,
                Created = note.Created,
                Title = note.Title
            };

            return Ok(result);
        
        
        }
        [HttpGet("{name}")]
        [Authorize]

        public IActionResult GetNoteByName(string name) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var note = repo.GetNoteByName(name,userId);
            if(note == null) { return NotFound(); }
            var result = new GetDtoNote
            {

                Id = note.Id,
                Content = note.Content,
                Created = note.Created,
                Title = note.Title
            };

            return Ok(result);
        
        
        }

        [HttpGet("search")]
        [Authorize]

        public IActionResult SearchByWord(string search) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
             

            var notes =repo.searchByKeyword(search,userId);
            var result = notes.Select(m=>new  GetDtoNote
            {
                
                Id = m.Id,
                Content = m.Content,
                Title = m.Title,

            
            
            }).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]

        public IActionResult AddNote(CreateDtoNote note)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Note note1 = new Note()
            {
                Content = note.Content,
                Title = note.Title,
                Created= DateTime.Now,
                UserId = userId!



            };
            repo.AddNote(note1);
            repo.Save();
            return Created();

        }

        [HttpPut("{id}")]
        [Authorize]

        public IActionResult UpdateNote(UpdateDtoNote note, int id) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var note1 = repo.GetNoteByID(id,userId);
            if (note1 == null) { return NotFound(); }

            note1.Title = note.Title;
            note1.Content = note.Content;
            repo.UpdateNote(note1);
            repo.Save();
            return NoContent();
        
        
        }
        [HttpDelete]
        [Authorize]

        public IActionResult DeleteNote(int id) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var note = repo.GetNoteByID(id,userId);
            if (note == null) { return NotFound(); }
            repo.DeleteNote(note);
            repo.Save();
            return NoContent();      
        }

    }
}
