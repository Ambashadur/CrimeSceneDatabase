using CSD.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CSD.Common.DataAccess;

public class CsdContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;

    public DbSet<Scene> Scenes { get; set; } = null!;

    public DbSet<Comment> Comments { get; set; } = null!;

    public CsdContext(DbContextOptions<CsdContext> options) : base(options) {
        Database.EnsureCreated();
    }
}
