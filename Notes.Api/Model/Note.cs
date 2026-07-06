namespace Notes.Api.Model
{
    public class Note
    {
        public int Id { get; set; }

        public string Title { get; set; } =string.Empty;    

        public string Content { get; set; } = string.Empty;

        public DateTime Created { get; set; } = DateTime.Now;
        // Foreign Key
        public string UserId { get; set; } = string.Empty;

        // Navigation Property
        public AppUser User { get; set; }=null;


    }
}
