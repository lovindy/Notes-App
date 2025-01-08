﻿namespace NotesApp.DTOs.Notes
{
    public class NoteDTO
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
