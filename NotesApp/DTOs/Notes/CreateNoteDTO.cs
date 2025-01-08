namespace NotesApp.DTOs.Notes
{
    public class CreateNoteDTO
    {
        public required string Title { get; set; }
        public string? Content { get; set; }
    }
}
