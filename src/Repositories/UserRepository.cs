using ApiMovies.Models.Entities;
using ApiMovies.Interfaces.Repositories;
using ApiMovies.Data;
using Microsoft.EntityFrameworkCore;
using ApiMovies.Common.Exceptions;

namespace ApiMovies.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db) {
        _db = db;
    }

    public ICollection<User> GetUsers(bool isActive = true) {
        return _db.User
            .Where(u => u.IsActive == isActive)
            .OrderBy(u => u.Name)
            .ToList();
    }

    public async Task<User?> GetUser(string userId) {
        return await _db.User.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User?> GetByUserNameOrEmail(string? userName, string? email, bool isActive = true) {
        var normalizedUserName = string.IsNullOrWhiteSpace(userName)
            ? null
            : userName.Trim().ToUpperInvariant();
        var normalizedEmail = string.IsNullOrWhiteSpace(email)
            ? null
            : email.Trim().ToUpperInvariant();

        if (normalizedUserName is not null) {
            return await _db.User.FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName && u.IsActive == isActive);
        }

        if (normalizedEmail is not null) {
            return await _db.User.FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail && u.IsActive == isActive);
        }

        return null;
    }

    public ICollection<User> SearchUsers(string search, bool isActive = true) {
        var normalizedSearch = search.Trim().ToLower();

        return _db.User
            .Where(u =>
                u.IsActive == isActive && (
                    (u.Name ?? string.Empty).ToLower().Contains(normalizedSearch) ||
                    (u.Email ?? string.Empty).ToLower().Contains(normalizedSearch)
                )
            )
            .OrderBy(u => u.Name)
            .ToList();
    }

    public async Task<bool> UserExists(string userId) {
        return await _db.User.AnyAsync(u => u.Id == userId && u.IsActive);
    }

    public async Task<bool> UserNameExists(string userName) {
        var normalizedUserName = userName.Trim().ToLower();
        return await _db.User.AnyAsync(u => u.IsActive && (u.UserName ?? string.Empty).ToLower() == normalizedUserName);
    }

    public async Task<bool> EmailExists(string email) {
        var normalizedEmail = email.Trim().ToLower();
        return await _db.User.AnyAsync(u => u.IsActive && (u.Email ?? string.Empty).ToLower() == normalizedEmail);
    }

    public async Task<bool> UpdateUser(string userId, User user) {
        if (!await UserExists(userId)) return false;

        var existingUser = await GetUser(userId);
        if (existingUser == null) return false;

        existingUser.Name = user.Name;
        existingUser.UserName = user.UserName;
        existingUser.Email = user.Email;
        existingUser.UpdatedAt = DateTime.UtcNow;

        _db.User.Update(existingUser);
        return await Save();
    }

    public async Task<bool> ActivateUser(string userId) {
        var existingUser = await GetUser(userId);
        if (existingUser is null) throw new NotFoundException($"User with id {userId} was not found.");

        existingUser.IsActive = true;
        existingUser.LockoutEnabled = false;
        existingUser.LockoutEnd = null;
        existingUser.UpdatedAt = DateTime.UtcNow;

        _db.User.Update(existingUser);
        return await Save();
    }

    public async Task<bool> DisableUser(string userId) {
        var existingUser = await GetUser(userId);
        if (existingUser is null) throw new NotFoundException($"User with id {userId} was not found.");

        existingUser.IsActive = false;
        existingUser.LockoutEnabled = true;
        existingUser.LockoutEnd = DateTimeOffset.MaxValue;
        existingUser.UpdatedAt = DateTime.UtcNow;

        _db.User.Update(existingUser);
        return await Save();
    }

    public async Task<bool> Save() {
        return await _db.SaveChangesAsync() > 0;
    }

}