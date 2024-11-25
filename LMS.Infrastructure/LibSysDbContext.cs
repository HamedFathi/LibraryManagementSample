using HamedStack.TheRepository.EntityFrameworkCore;
using LMS.Infrastructure.Configs;
using LMS.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LMS.Infrastructure;

public class LibSysDbContext : DbContextBase
{
    public LibSysDbContext(DbContextOptions<LibSysDbContext> options, ILogger<DbContextBase> logger) : base(options, logger)
    {
    }

    public virtual DbSet<AuthorModel> Authors { get; set; }
    public virtual DbSet<BookModel> Books { get; set; }
    public virtual DbSet<BookAuthorModel> BookAuthors { get; set; }
    public virtual DbSet<BookCopyModel> BookCopies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookModelConfig());
        modelBuilder.ApplyConfiguration(new AuthorModelConfig());
        modelBuilder.ApplyConfiguration(new BookAuthorModelConfig());
        modelBuilder.ApplyConfiguration(new BookCopyModelConfig());
        base.OnModelCreating(modelBuilder);
    }
}