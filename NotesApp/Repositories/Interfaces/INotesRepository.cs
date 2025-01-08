using NotesApp.Models;

namespace NotesApp.Repositories.Interfaces
{
    public interface INotesRepository
    {
        Task<IEnumerable<Note>> GetAllByUserIdAsync(int userId);
        Task<Note> GetByIdAsync(int id, int userId);
        Task<int> CreateAsync(Note note);
        Task<bool> UpdateAsync(Note note);
        Task<bool> DeleteAsync(int id, int userId);
    }
}
