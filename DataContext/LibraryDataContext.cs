using LibraryV2.Models;
using LibraryV2.Utilities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryV2.DataContext;

public class LibraryDataContext : DbContext
{
    public LibraryDataContext(DbContextOptions<LibraryDataContext> options) : base(options)
    {        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().Property(x => x.Id).HasConversion<UlidToBytesConverter>();
        modelBuilder.Entity<Book>().Property(x => x.Id).HasConversion<UlidToBytesConverter>();
        modelBuilder.Entity<BookEdition>().Property(x => x.Id).HasConversion<UlidToBytesConverter>();
        modelBuilder.Entity<Reader>().Property(x => x.Id).HasConversion<UlidToBytesConverter>();

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<BookEdition> BookEditions { get; set; }
    public DbSet<Reader> Readers { get; set; }
}
