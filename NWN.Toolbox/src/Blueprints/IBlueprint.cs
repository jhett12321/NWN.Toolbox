using Anvil.API;

namespace Jorteck.Toolbox
{
  public interface IBlueprint
  {
    public string FullName => Category + "/" + Name;

    public string Name { get; }

    public string Category { get; }

    public BlueprintObjectType ObjectType { get; }

    public NwObject Create(Location location);
  }
}
