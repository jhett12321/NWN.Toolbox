using Microsoft.EntityFrameworkCore;

namespace Jorteck.Toolbox
{
  internal static class DbContextExtensions
  {
    public static void AddOrUpdate<T>(this DbContext context, T entity) where T : class
    {
      if (context.Entry(entity).State == EntityState.Detached)
      {
        context.Add(entity);
      }
    }

    public static void SafeRemove<T>(this DbContext context, T entity)
    {
      if (entity is {})
      {
        context.Remove(entity);
      }
    }
  }
}
