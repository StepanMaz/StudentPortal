using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;
using StudentPortalServer.Entities;

namespace StudentPortalServer.Services;

public class StudentPortalDBContext : DbContext
{
    public DbSet<User> Users { get; init; }

    public StudentPortalDBContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToCollection("users");
    }
}