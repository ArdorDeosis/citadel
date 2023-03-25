namespace Generators;

public static partial class NameGenerator
{
  private static readonly Random Random = new();
  
  public static string GetRandomName()
  {
    return $"{Forenames[Random.Next(0, Forenames.Length)]} {Surnames[Random.Next(0, Surnames.Length)]}";
  }
}