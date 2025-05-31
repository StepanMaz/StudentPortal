using Microsoft.EntityFrameworkCore;
using StudentPortal.QuizService.DB.Entities;

namespace StudentPortal.QuizService.DB;

public class QuizContext : DbContext
{
    public DbSet<QuizData> QuizDatas { get; set; }

    public QuizContext(DbContextOptions<QuizContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuizData>().OwnsMany(x => x.Results, x =>
        {
            x.WithOwner().HasForeignKey("QuizDataId");
            x.Property<int>("Id");
            x.HasKey("Id");
        });
    }
}
