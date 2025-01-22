using System.Collections.Immutable;
using Model;

namespace State;

public class Repository
{
  public readonly Guid Id = Guid.NewGuid();

  public ImmutableDictionary<Guid, Building> Buildings { get; private set; } = 
    ImmutableDictionary<Guid, Building>.Empty;

  public Building? AddBuilding(BuildingType type)
  {
    var building = new Building { Type = type };
    Buildings = Buildings.Add(building.Id, building);
    return building;
  }
}