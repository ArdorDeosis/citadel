using Model;
using State;

namespace Operations;

public class CreateBuildingOperation : Operation
{
  private readonly Repository repository;
  private readonly BuildingType type;
  
  public CreateBuildingOperation(Repository repository, BuildingType type)
  {
    this.repository = repository;
    this.type = type;
  }

  public override bool Perform() => repository.AddBuilding(type) is not null;
}