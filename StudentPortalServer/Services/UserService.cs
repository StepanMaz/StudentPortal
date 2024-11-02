using Microsoft.EntityFrameworkCore;
using StudentPortalServer.Entities;

namespace StudentPortalServer.Services;

public class UserService(StudentPortalDBContext db) {
    public async Task<User> AddUserAsync(User user)
    {
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await db.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await db.Users.ToListAsync();
    }

    public async Task<User?> UpdateUserAsync(User user)
    {
        var existingUser = await db.Users.FindAsync(user.Id);
        if (existingUser == null) return null;

        existingUser.FirstName = user.FirstName;
        existingUser.Role = user.Role;
        existingUser.Email = user.Email;
        existingUser.PasswordHash = user.PasswordHash; 

        await db.SaveChangesAsync();
        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null) return false;

        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return true;
    }
}