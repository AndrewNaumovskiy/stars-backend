using Stars.API.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Stars.API.Helpers;

public class StarsDbContext : DbContext
{
    public DbSet<StudentDbModel> Students { get; set; }
    public DbSet<GroupDbModel> Groups { get; set; }
    public DbSet<ClassDbModel> Classes { get; set; }

    public StarsDbContext(DbContextOptions<StarsDbContext> options) : base(options) { }

    protected override async void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentDbModel>()
                    .HasOne(x => x.Group)
                    .WithMany(x => x.Students)
                    .HasForeignKey(x => x.GroupFk);

        modelBuilder.Entity<ClassDbModel>()
                    .HasOne(x => x.Group)
                    .WithMany(x => x.Classes)
                    .HasForeignKey(x => x.GroupFk);
    }
}
