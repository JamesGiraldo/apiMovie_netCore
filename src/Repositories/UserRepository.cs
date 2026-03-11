using ApiMovies.Models.Entities;
using ApiMovies.Interfaces.Repositories;
using ApiMovies.Data;
using ApiMovies.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace ApiMovies.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db) {
        _db = db;
    }

    public ICollection<User> GetUsers(bool isActive = true) {
        return _db.User.Where(u => u.IsActive == isActive)
            .OrderBy(u => u.Name)
            .ToList();
    }

    public async Task<User?> GetUser(int userId, bool isActive = true) {
        return await _db.User.FirstOrDefaultAsync(u => u.Id == userId && u.IsActive == isActive);
    }

    public async Task<User?> GetByUserNameOrEmail(string? userName, string? email, bool isActive = true) {
        var normalizedUserName = userName?.Trim().ToLower();
        var normalizedEmail = email?.Trim().ToLower();

        return await _db.User.FirstOrDefaultAsync(u =>
            u.IsActive == isActive &&
            (
                (!string.IsNullOrWhiteSpace(normalizedUserName) && u.UserName.ToLower() == normalizedUserName) ||
                (!string.IsNullOrWhiteSpace(normalizedEmail) && u.Email.ToLower() == normalizedEmail)
            )
        );
    }

    public async Task<bool> UserExists(int userId, bool isActive = true) {
        return await _db.User.AnyAsync(u => u.Id == userId && u.IsActive == isActive);
    }

    public async Task<bool> UserNameExists(string userName, bool isActive = true) {
        return await _db.User.AnyAsync(u => u.UserName.ToLower() == userName.ToLower() && u.IsActive == isActive);
    }

    public async Task<bool> EmailExists(string email, bool isActive = true) {
        return await _db.User.AnyAsync(u => u.Email.ToLower() == email.ToLower() && u.IsActive == isActive);
    }

    public async Task<bool> CreateUser(User user) {
        await _db.User.AddAsync(user);
        return await Save();
    }

    public async Task<bool> UpdateUser(User user) {
        _db.User.Update(user);
        return await Save();
    }

    public async Task<bool> DisableUser(int userId, bool isActive = true) {
        var user = await GetUser(userId, isActive);
        if (user == null) return false;

        user.IsActive = false;
        return await Save();
    }

    public async Task<bool> Save() {
        return await _db.SaveChangesAsync() > 0;
    }

}