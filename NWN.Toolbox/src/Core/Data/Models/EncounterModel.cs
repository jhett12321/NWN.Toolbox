using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jorteck.Toolbox.Core
{
  internal sealed class EncounterModel
  {
    public string Name { get; set; }

    public string Author { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public List<EncounterSpawn> Creatures { get; set; }

    public sealed class EncounterSpawn
    {
      public int Id { get; set; }

      public float LocalOffsetX { get; set; }
      public float LocalOffsetY { get; set; }
      public float LocalOffsetZ { get; set; }

      public byte[] CreatureData { get; set; }

      public EncounterModel Encounter { get; set; }
    }
  }

  internal sealed class EncounterModelConfiguration : IEntityTypeConfiguration<EncounterModel>
  {
    public void Configure(EntityTypeBuilder<EncounterModel> builder)
    {
      builder.HasKey(encounter => encounter.Name);
      builder.OwnsMany(encounter => encounter.Creatures, encounterNav =>
      {
        encounterNav.WithOwner(list => list.Encounter);
        encounterNav.HasKey(list => list.Id);
        encounterNav.Property(list => list.Id)
          .ValueGeneratedOnAdd();
      });
    }
  }
}
