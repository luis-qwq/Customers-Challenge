using Microsoft.EntityFrameworkCore;

namespace Customers.Entities;
public partial class SqlDbContext : DbContext
{
  public SqlDbContext() { }

  public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options) { }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    optionsBuilder.UseSqlServer(
      "Server=localhost;port=57000;Database=CustomerDev;User Id=sa;Password=SysPwd#01;");
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

