using Deepin.Storage.API.Infrastructure.Entitites;
using Deepin.Storage.API.Infrastructure.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Storage.API.Infrastructure;
public class StorageDbContext : DbContext
{
    public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
    {
    }
    public DbSet<FileObject> FileObjects { get; set; } 
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("storage");
        modelBuilder.ApplyConfiguration(new FileObjectEntityTypeConfiguration());
    }
}
