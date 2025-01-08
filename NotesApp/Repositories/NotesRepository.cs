using Dapper;
using NotesApp.Data;
using NotesApp.Models;
using NotesApp.Repositories.Interfaces;

namespace NotesApp.Repositories
{
    public class NotesRepository : INotesRepository
    {
        private readonly IDapperDbContext _dbContext;

        public NotesRepository(IDapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Note>> GetAllByUserIdAsync(int userId)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
                SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt 
                FROM Notes 
                WHERE UserId = @UserId";
            return await connection.QueryAsync<Note>(sql, new { UserId = userId });
        }

        public async Task<Note> GetByIdAsync(int id, int userId)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
                SELECT Id, UserId, Title, Content, CreatedAt, UpdatedAt 
                FROM Notes 
                WHERE Id = @Id AND UserId = @UserId";
            return await connection.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id, UserId = userId });
        }

        public async Task<int> CreateAsync(Note note)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
                INSERT INTO Notes (UserId, Title, Content, CreatedAt, UpdatedAt) 
                VALUES (@UserId, @Title, @Content, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            return await connection.QuerySingleAsync<int>(sql, note);
        }

        public async Task<bool> UpdateAsync(Note note)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
                UPDATE Notes 
                SET Title = @Title, 
                    Content = @Content, 
                    UpdatedAt = @UpdatedAt 
                WHERE Id = @Id AND UserId = @UserId";
            var rowsAffected = await connection.ExecuteAsync(sql, note);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
            return rowsAffected > 0;
        }
    }
}