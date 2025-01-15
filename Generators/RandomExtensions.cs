namespace Generators;

public static class RandomExtensions
{
  public static T Pick<T>(this Random random, IReadOnlyList<T> source) => source[random.Next(source.Count)];
}