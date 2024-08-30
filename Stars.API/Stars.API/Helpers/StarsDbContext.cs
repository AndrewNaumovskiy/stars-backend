using Microsoft.EntityFrameworkCore;

namespace Stars.API.Helpers;

public class StarsDbContext : DbContext
{
    //public DbSet<StudentDbModel> Users { get; set; }

    public StarsDbContext(DbContextOptions<StarsDbContext> options) : base(options) { }

    protected override async void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<BookDbModel>()
        //    .HasMany(x => x.Authors)
        //    .WithMany(x => x.Books)
        //    .UsingEntity<BookAuthor>
        //    (
        //        l => l.HasOne<AuthorDbModel>().WithMany().HasForeignKey(x => x.AuthorId),
        //        r => r.HasOne<BookDbModel>().WithMany().HasForeignKey(x => x.BookId)
        //    );
        //
    }
}
