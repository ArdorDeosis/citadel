namespace Model;

public class Citizen
{
  public required Guid Id { get; set; }
  public required string Name { get; set; }
  public Building? Workplace { get; set; }
}