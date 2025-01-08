using NotesApp.DTOs.Notes;

namespace NotesApp.Services.Interfaces
{
    public interface INotesService
    {
        Task<IEnumerable<NoteDTO>> GetAllNotesAsync(int userId);
        Task<NoteDTO> GetNoteByIdAsync(int id, int userId);
        Task<NoteDTO> CreateNoteAsync(CreateNoteDTO noteDto, int userId);
        Task<bool> UpdateNoteAsync(int id, UpdateNoteDTO noteDto, int userId);
        Task<bool> DeleteNoteAsync(int id, int userId);
    }
}
