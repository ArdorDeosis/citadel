namespace Model;

public class Building
{
  public Guid Id { get; init; } = Guid.CreateVersion7();
  public required BuildingType Type { get; init; }
  public ICollection<Citizen> Workers { get; } = new List<Citizen>();
  public Recipe? CurrentRecipe { get; set; }
}