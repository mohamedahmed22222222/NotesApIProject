namespace Notes.Api.DTO
{
    public class GetDtoNote
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime Created { get; set; }
    }
}
