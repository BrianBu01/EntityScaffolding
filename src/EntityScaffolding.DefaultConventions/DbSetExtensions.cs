using Microsoft.EntityFrameworkCore;

namespace EntityScaffolding.DefaultConventions
{
public static class DbSetExtensions
{
    public static TEntity FindEntity<TEntity, T1>(this DbSet<TEntity> set, T1 key1)
        where TEntity : class, IIdentity<T1>
    {
        return set.Find(key1);
    }

    public static TEntity FindEntity<TEntity, T1, T2>(this DbSet<TEntity> set, T1 key1, T2 key2)
        where TEntity : class, IIdentity<T1, T2>
    {
        return set.Find(key1, key2);
    }

        public static TEntity FindEntity<TEntity, T1, T2, T3>(this DbSet<TEntity> set, T1 key1, T2 key2, T3 key3)
            where TEntity : class, IIdentity<T1, T2, T3>
        {
            return set.Find(key1, key2, key3);
        }

        public static TEntity FindEntity<TEntity, T1, T2, T3, T4>(this DbSet<TEntity> set, T1 key1, T2 key2, T3 key3, T4 key4)
            where TEntity : class, IIdentity<T1, T2, T3, T4>
        {
            return set.Find(key1, key2, key3, key4);
        }

        public static TEntity FindEntity<TEntity, T1, T2, T3, T4, T5>(this DbSet<TEntity> set, T1 key1, T2 key2, T3 key3, T4 key4, T5 key5)
            where TEntity : class, IIdentity<T1, T2, T3, T4, T5>
        {
            return set.Find(key1, key2, key3, key4, key5);
        }

    }
}