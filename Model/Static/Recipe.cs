namespace Model;

public record Recipe
{
  public required Guid Id { get; init; }
  public required string Name { get; init; }
  public required IReadOnlyCollection<ResourceQuantity> Input { get; init; }
  public required IReadOnlyCollection<ResourceQuantity> Output { get; init; }
}