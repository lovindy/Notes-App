using Dapper;
using NotesApp.Data;
using NotesApp.Models;

namespace NotesApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDapperDbContext _dbContext;

        public UserRepository(IDapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
            SELECT Id, Username, PasswordHash, CreatedAt 
            FROM Users 
            WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
            SELECT Id, Username, PasswordHash, CreatedAt 
            FROM Users 
            WHERE Username = @Username";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }

        public async Task<int> CreateAsync(User user)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
            INSERT INTO Users (Username, PasswordHash, CreatedAt) 
            VALUES (@Username, @PasswordHash, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int)";

            var newUserId = await connection.ExecuteScalarAsync<int>(sql, new
            {
                user.Username,
                user.PasswordHash,
                CreatedAt = DateTime.UtcNow
            });

            return newUserId;
        }

        public async Task<bool> UpdateAsync(User user)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = @"
            UPDATE Users 
            SET Username = @Username,
                PasswordHash = @PasswordHash 
            WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, user);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _dbContext.CreateConnection();
            const string sql = "DELETE FROM Users WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}