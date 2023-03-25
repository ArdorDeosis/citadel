namespace Model;

public class Building
{
  public required Guid Id { get; set; }
  public required BuildingType Type { get; set; }
  public required List<Citizen> Workers { get; set; }
  public Recipe? CurrentRecipe { get; set; }
}