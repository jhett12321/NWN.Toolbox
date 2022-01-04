using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jorteck.Toolbox
{
  internal sealed class BlueprintModel
  {
    public BlueprintObjectType Type { get; set; }

    public string Name { get; set; }

    public string Category { get; set; }

    public string Author { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public bool Active { get; set; }

    public byte[] BlueprintData { get; set; }

    public float? CR { get; set; }

    public string Faction { get; set; }
  }

  internal sealed class BlueprintModelConfiguration : IEntityTypeConfiguration<BlueprintModel>
  {
    public void Configure(EntityTypeBuilder<BlueprintModel> builder)
    {
      builder.HasKey(blueprint => new { Blueprint = blueprint.Type, blueprint.Name });
    }
  }
}
