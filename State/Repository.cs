using System.Collections.Immutable;
using Model;

namespace State;

public class Repository
{
  private ImmutableHashSet<Building> buildings = [];

  public Building? AddBuilding(BuildingType type)
  {
    var building = new Building { Type = type };
    buildings = buildings.Add(building);
    return building;
  }
}