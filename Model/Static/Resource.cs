namespace Model;

public record Resource
{
  public required Guid Id { get; init; }
  public required string Name { get; init; }
  
  public static ResourceQuantity operator *(Resource resource, float quantity) => 
    new() { Resource = resource, Quantity = quantity };

  public static ResourceQuantity operator *(float quantity, Resource resource) => 
    new() { Resource = resource, Quantity = quantity };
}