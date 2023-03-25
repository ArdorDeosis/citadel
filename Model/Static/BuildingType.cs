namespace Model;

public record BuildingType
{
  public required Guid Id { get; init; }
  public required string Name { get; init; }
  public required int WorkplaceCount { get; init; }
  public required IReadOnlyCollection<Recipe> Recipes { get; init; }
}