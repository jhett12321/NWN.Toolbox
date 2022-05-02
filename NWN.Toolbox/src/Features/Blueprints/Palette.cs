using System;
using System.Collections.Generic;
using System.IO;
using Anvil.API;
using Anvil.Services;
using NLog;

namespace Jorteck.Toolbox.Features.Blueprints
{
  internal sealed class Palette
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private const string StandardPaletteSuffix = "std";
    private const string CustomPaletteSuffix = "cus";

    private readonly BlueprintObjectType paletteType;
    private readonly string standardPaletteResRef;
    private readonly string customPaletteResRef;

    [Inject]
    public ResourceManager ResourceManager { private get; init; }

    private readonly List<PaletteBlueprint> blueprints = new List<PaletteBlueprint>();

    public Palette(string palettePrefix, BlueprintObjectType paletteType)
    {
      this.paletteType = paletteType;
      standardPaletteResRef = palettePrefix + StandardPaletteSuffix;
      customPaletteResRef = palettePrefix + CustomPaletteSuffix;
    }

    public List<PaletteBlueprint> GetBlueprints()
    {
      blueprints.Clear();

      TryLoadPalette(standardPaletteResRef, "Standard");
      TryLoadPalette(customPaletteResRef, "Custom");

      return blueprints;
    }

    private void TryLoadPalette(string resRef, string rootPath)
    {
      using GffResource palette = ResourceManager.GetGenericFile(resRef, ResRefType.ITP);
      if (palette == null)
      {
        Log.Error("Failed to load palette {Palette}", resRef);
        return;
      }

      try
      {
        ProcessList(palette["MAIN"], rootPath);
      }
      catch (Exception e)
      {
        Log.Error(e, "Failed to parse palette file {Palette}", resRef);
      }
    }

    private void ProcessList(GffResourceField field, string path)
    {
      foreach (GffResourceField child in field.Values)
      {
        ProcessStruct(child, path);
      }
    }

    private void ProcessStruct(GffResourceField field, string path)
    {
      if (field.TryGetValue("RESREF", out GffResourceField resRefField))
      {
        string resRef = resRefField.Value<string>();
        string name = "Unknown";
        float? cr = null;
        string faction = null;

        if (field.TryGetValue("NAME", out GffResourceField creatureNameField))
        {
          name = creatureNameField.Value<string>();
        }
        else if (field.TryGetValue("STRREF", out GffResourceField creatureNameStrRefField))
        {
          name = new StrRef(creatureNameStrRefField.Value<uint>()).ToString();
        }

        if (field.TryGetValue("CR", out GffResourceField creatureChallengeRatingField))
        {
          cr = creatureChallengeRatingField.Value<float>();
        }

        if (field.TryGetValue("FACTION", out GffResourceField creatureFactionField))
        {
          faction = creatureFactionField.Value<string>();
        }

        blueprints.Add(new PaletteBlueprint
        {
          ResRef = resRef,
          Name = name,
          Category = path,
          CR = cr,
          Faction = faction,
          FullName = path + "/" + name,
          ObjectType = paletteType,
        });
      }
      else
      {
        if (field.TryGetValue("STRREF", out GffResourceField groupStrRef))
        {
          path = Path.Combine(path, new StrRef(groupStrRef.Value<uint>()).ToString());
        }

        if (field.TryGetValue("LIST", out GffResourceField list))
        {
          ProcessList(list, path);
        }
      }
    }
  }
}
