using NotesApp.Models;
using NotesApp.DTOs.Notes;
using NotesApp.Repositories.Interfaces;
using NotesApp.Services.Interfaces;

namespace NotesApp.Services
{
    public class NotesService : INotesService
    {
        private readonly INotesRepository _notesRepository;

        public NotesService(INotesRepository notesRepository)
        {
            _notesRepository = notesRepository;
        }

        public async Task<IEnumerable<NoteDTO>> GetAllNotesAsync(int userId)
        {
            var notes = await _notesRepository.GetAllByUserIdAsync(userId);
            return notes.Select(n => new NoteDTO
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                CreatedAt = n.CreatedAt,
                UpdatedAt = n.UpdatedAt
            });
        }

        public async Task<NoteDTO> GetNoteByIdAsync(int id, int userId)
        {
            var note = await _notesRepository.GetByIdAsync(id, userId);
            if (note == null)
                throw new KeyNotFoundException("Note not found");

            return new NoteDTO
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }

        public async Task<NoteDTO> CreateNoteAsync(CreateNoteDTO noteDto, int userId)
        {
            var note = new Note
            {
                Title = noteDto.Title,
                Content = noteDto.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            note.Id = await _notesRepository.CreateAsync(note);
            return new NoteDTO
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }

        public async Task<bool> UpdateNoteAsync(int id, UpdateNoteDTO noteDto, int userId)
        {
            var existingNote = await _notesRepository.GetByIdAsync(id, userId);
            if (existingNote == null)
                throw new KeyNotFoundException("Note not found");

            existingNote.Title = noteDto.Title;
            existingNote.Content = noteDto.Content;
            existingNote.UpdatedAt = DateTime.UtcNow;

            return await _notesRepository.UpdateAsync(existingNote);
        }

        public async Task<bool> DeleteNoteAsync(int id, int userId)
        {
            return await _notesRepository.DeleteAsync(id, userId);
        }
    }
}
