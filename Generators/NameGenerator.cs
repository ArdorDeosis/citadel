namespace Generators;

public static partial class NameGenerator
{
  private static readonly Random Random = new();
  
  public static string GetRandomName() => 
    $"{Random.Pick(Forenames)} {Random.Pick(Surnames)}";
}