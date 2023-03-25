namespace Model;

public readonly struct ResourceQuantity
{
  public required Resource Resource { get; init; }
  public required float Quantity { get; init; }
  
  public static ResourceQuantity operator *(ResourceQuantity resource, float factor) => 
    resource with { Quantity = resource.Quantity * factor };

  public static ResourceQuantity operator *(float factor, ResourceQuantity resource) => 
    resource with { Quantity = resource.Quantity * factor };
    
  public static ResourceQuantity operator /(ResourceQuantity resource, float factor) => 
    resource with { Quantity = resource.Quantity / factor };
}