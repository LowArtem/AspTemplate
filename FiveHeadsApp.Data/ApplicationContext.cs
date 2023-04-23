using FiveHeadsApp.Core.Model.Auth;
using Microsoft.EntityFrameworkCore;

namespace FiveHeadsApp.Data;

/// <summary>
/// Контекст базы данных приложения
/// </summary>
public class ApplicationContext : DbContext
{
    /// <inheritdoc />
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    public ApplicationContext()
    {
    }

    #region Auth

    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}