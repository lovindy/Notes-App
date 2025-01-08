namespace NotesApp.DTOs.Notes
{
    public class UpdateNoteDTO
    {
        public required string Title { get; set; }
        public string? Content { get; set; }
    }
}
