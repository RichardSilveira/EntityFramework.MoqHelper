using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.MoqHelper
{
    public static class EntityFrameworkMoqHelper
    {
        public static Mock<DbSet<T>> CreateMockForDbSet<T>() where T : class
        {
            return new Mock<DbSet<T>>();
        }

        public static Mock<T> CreateMockForDbContext<T>() where T : DbContext
        {
            return new Mock<T>();
        }

        public static Mock<TContext> CreateMockForDbContext<TContext, TEntity>(Mock<DbSet<TEntity>> mockSet)
            where TContext : DbContext
            where TEntity : class
        {
            var mockContext = new Mock<TContext>();
            mockContext.Setup(c => c.Set<TEntity>()).Returns(mockSet.Object);

            return mockContext;
        }

        public static Mock<DbSet<T>> SetupForQueryOn<T>(this Mock<DbSet<T>> dbSet, List<T> table) where T : class
        {
            dbSet.As<IQueryable<T>>().Setup(q => q.Provider).Returns(() => table.AsQueryable().Provider);
            dbSet.As<IQueryable<T>>().Setup(q => q.Expression).Returns(() => table.AsQueryable().Expression);
            dbSet.As<IQueryable<T>>().Setup(q => q.ElementType).Returns(() => table.AsQueryable().ElementType);
            dbSet.As<IQueryable<T>>().Setup(q => q.GetEnumerator()).Returns(() => table.AsQueryable().GetEnumerator());

            return dbSet;
        }

        public static Mock<DbSet<T>> WithAdd<T>(this Mock<DbSet<T>> dbSet, List<T> table) where T : class
        {
            dbSet.Setup(set => set.Add(It.IsAny<T>())).Callback<T>(table.Add);
            return dbSet;
        }

        public static Mock<DbSet<T>> WithAdd<T>(this Mock<DbSet<T>> dbSet, List<T> table, string IDPropertyNameToAutoIncrement, int IDValueToAdd = 1) where T : class
        {
            dbSet.Setup(set => set.Add(It.IsAny<T>())).Returns<T>(x =>
            {
                typeof(T).GetProperty(IDPropertyNameToAutoIncrement).SetValue(x, IDValueToAdd);

                return x;
            })
            .Callback<T>(table.Add);

            return dbSet;
        }

        public static Mock<DbSet<T>> WithAttach<T>(this Mock<DbSet<T>> dbSet, List<T> table, string IDPropertyNameToCompare, int IDValueToAdd = 1) where T : class
        {
            dbSet.Setup(set => set.Attach(It.IsAny<T>())).Returns<T>(x =>
            {
                var propertyToCompare = (int)typeof(T).GetProperty(IDPropertyNameToCompare).GetValue(x, null);

                var itemToRemove = table.SingleOrDefault(y => { return (int)typeof(T).GetProperty(IDPropertyNameToCompare).GetValue(y, null) == propertyToCompare; });

                table.Remove(itemToRemove);

                return x;
            })
            .Callback<T>(table.Add);

            return dbSet;
        }

        public static Mock<DbSet<T>> WithAddRange<T>(this Mock<DbSet<T>> dbSet, List<T> table) where T : class
        {
            dbSet.Setup(set => set.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(table.AddRange);
            return dbSet;
        }

        public static Mock<DbSet<T>> WithFind<T>(this Mock<DbSet<T>> dbSet, List<T> table, string IDPropertyName) where T : class
        {
            dbSet.Setup(m => m.Find(It.IsAny<int>()))
                .Returns<object[]>(ids => table.FirstOrDefault(y => (int)typeof(T).GetProperty(IDPropertyName).GetValue(y, null) == (int)ids[0]));

            return dbSet;
        }

        public static Mock<DbSet<T>> WithRemove<T>(this Mock<DbSet<T>> dbSet, List<T> table) where T : class
        {
            dbSet.Setup(set => set.Remove(It.IsAny<T>())).Callback<T>(t => table.Remove(t));
            return dbSet;
        }

        public static Mock<DbSet<T>> WithRemoveRange<T>(this Mock<DbSet<T>> dbSet, List<T> table) where T : class
        {
            dbSet.Setup(set => set.RemoveRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(ts =>
            {
                foreach (var t in ts) { table.Remove(t); }
            });

            return dbSet;
        }
    }
}
