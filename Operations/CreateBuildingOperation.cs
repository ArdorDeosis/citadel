using Fyremoss.DependencyInjection;
using Model;
using State;

namespace Operations;

public class CreateBuildingOperation : IOperation
{
  [Inject] private Repository Repository { get; init; } = null!;
  
  private BuildingType Type { get; init; }
  
  public CreateBuildingOperation(BuildingType type)
  {
    Type = type;
  }

  /// <inheritdoc />
  public void Perform()
  {
    Repository.AddBuilding(Type);
  }
}