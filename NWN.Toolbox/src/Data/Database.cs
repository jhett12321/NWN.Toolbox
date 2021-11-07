using Microsoft.EntityFrameworkCore;

namespace Jorteck.Toolbox
{
  internal sealed class Database : DbContext
  {
    // Model
    public DbSet<EncounterModel> EncounterPresets { get; set; }
    public DbSet<BlueprintModel> BlueprintPresets { get; set; }

    private readonly string connectString;

    public Database(string connectString)
    {
      this.connectString = connectString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
      builder.UseSqlite(connectString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.ApplyConfigurationsFromAssembly(typeof(Database).Assembly);
    }
  }
}
