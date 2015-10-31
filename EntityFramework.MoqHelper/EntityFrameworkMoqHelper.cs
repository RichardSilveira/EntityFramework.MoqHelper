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
        /// <summary>
        /// Create a mock instance for DbSet
        /// </summary>
        /// <typeparam name="T">Associated type (class) on dbset of the DbContext</typeparam>
        /// <returns>Simple mocked DbSet</returns>
        public static Mock<DbSet<T>> CreateMockForDbSet<T>() where T : class
        {
            return new Mock<DbSet<T>>();
        }

        /// <summary>
        /// Create a mock instance for DbContext
        /// </summary>
        /// <typeparam name="T">Dbcontext type that will be mocked</typeparam>
        /// <returns>Simple mocked DbContext</returns>
        public static Mock<T> CreateMockForDbContext<T>() where T : DbContext
        {
            return new Mock<T>();
        }

        /// <summary>
        /// Create a mock instance for DbContext and set it up with an instance of DbSet
        /// </summary>
        /// <typeparam name="TContext">DbContext type that will be mocked</typeparam>
        /// <typeparam name="TEntity">Associated type (class) on dbset of the DbContext</typeparam>
        /// <param name="mockSet">Mocked DbSet instance</param>
        /// <returns>Mocked DbContext instance already configured with a mockSet Object</returns>
        public static Mock<TContext> CreateMockForDbContext<TContext, TEntity>(Mock<DbSet<TEntity>> mockSet)
            where TContext : DbContext
            where TEntity : class
        {
            var mockContext = new Mock<TContext>();
            mockContext.Setup(c => c.Set<TEntity>()).Returns(mockSet.Object);

            return mockContext;
        }

        /// <summary>
        /// Sets an in-memory table to simulate real data access made from DbSet 
        /// </summary>
        /// <typeparam name="T">Associated type (class) on dbset of the DbContext</typeparam>
        /// <param name="dbSet">Mocked DbSet instance</param>
        /// <param name="table">In-memory list of items</param>
        /// <returns>Configured DbSet instance for queries on it</returns>
        public static Mock<DbSet<T>> SetupForQueryOn<T>(this Mock<DbSet<T>> dbSet, List<T> table) where T : class
        {
            dbSet.As<IQueryable<T>>().Setup(q => q.Provider).Returns(() => table.AsQueryable().Provider);
            dbSet.As<IQueryable<T>>().Setup(q => q.Expression).Returns(() => table.AsQueryable().Expression);
            dbSet.As<IQueryable<T>>().Setup(q => q.ElementType).Returns(() => table.AsQueryable().ElementType);
            dbSet.As<IQueryable<T>>().Setup(q => q.GetEnumerator()).Returns(() => table.AsQueryable().GetEnumerator());

            return dbSet;
        }

        /// <summary>
        /// Mock for Entity Framework 'DbSet.Add' method to work with in-memory table
        /// </summary>
        /// <typeparam name="T">Associated type (class) on dbset of the DbContext</typeparam>
        /// <param name="dbSet">Mocked DbSet instance</param>
        /// <param name="table">In-memory list of items</param>
        /// <returns>Configured DbSet instance for mock calls on 'DbSet.Add' method</returns>
        public static Mock<DbSet<T>> WithAdd<T>(this Mock<DbSet<T>> dbSet, List<T> table) where T : class
        {
            dbSet.Setup(set => set.Add(It.IsAny<T>())).Callback<T>(table.Add);
            return dbSet;
        }

        /// <summary>
        /// Mock for Entity Framework 'DbSet.Add' method to work with in-memory table
        /// </summary>
        /// <typeparam name="T">Associated type (class) on dbset of the DbContext</typeparam>
        /// <param name="dbSet">Mocked DbSet instance</param>
        /// <param name="table">In-memory list of items</param>
        /// <param name="IDPropertyNameToAutoIncrement">Basically is the name of the referenced property as the primary key</param>
        /// <param name="IDValueToAdd">Value to be inserted in 'IDPropertyNameToAutoIncrement'</param>
        /// <returns>Configured DbSet instance for mock calls on 'DbSet.Add' method</returns>
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

        /// <summary>
        /// Mock for Entity Framework 'DbSet.Attach' method to work with in-memory table
        /// </summary>
        /// <typeparam name="T">Associated type (class) on dbset of the DbContext</typeparam>
        /// <param name="dbSet">Mocked DbSet instance</param>
        /// <param name="table">In-memory list of items</param>
        /// <param name="IDPropertyNameToCompare">Property name referred to as primary key that will be used to find and update your respective entity on in-memory table</param>
        /// <returns>Configured DbSet instance for mock calls on 'DbSet.Attach' method</returns>
        public static Mock<DbSet<T>> WithAttach<T>(this Mock<DbSet<T>> dbSet, List<T> table, string IDPropertyNameToCompare) where T : class
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

        /// <summary>
        /// Mock for Entity Framework 'DbSet.AddRange' method to work with in-memory table
        /// </summary>
        /// <typeparam name="T">Associated type (class) on dbset of the DbContext</typeparam>
        /// <param name="dbSet">Mocked DbSet instance</param>
        /// <param name="table">In-memory list of items</param>
        /// <returns>Configured DbSet instance for mock calls on 'DbSet.AddRange' method</returns>
        public static Mock<DbSet<T>> WithAddRange<T>(this Mock<DbSet<T>> dbSet, List<T> table) where T : class
        {
            dbSet.Setup(set => set.AddRange(It.IsAny<IEnumerable<T>>())).Callback<IEnumerable<T>>(table.AddRange);
            return dbSet;
        }

        /// <summary>
        /// Mock for Entity Framework 'DbSet.Find' method to work with in-memory table
        /// </summary>
        /// <typeparam name="T">Associated type (class) on dbset of the DbContext</typeparam>
        /// <param name="dbSet">Mocked DbSet instance</param>
        /// <param name="table">In-memory list of items</param>
        /// <param name="IDPropertyName">Basically is the name of the referenced property as the primary key</param>
        /// <returns>Configured DbSet instance for mock calls on 'DbSet.Find' method</returns>
        public static Mock<DbSet<T>> WithFind<T>(this Mock<DbSet<T>> dbSet, List<T> table, string IDPropertyName) where T : class
        {
            dbSet.Setup(set => set.Find(It.IsAny<int>()))
                .Returns<object[]>(ids => table.FirstOrDefault(y => (int)typeof(T).GetProperty(IDPropertyName).GetValue(y, null) == (int)ids[0]));

            return dbSet;
        }

        /// <summary>
        /// Mock for Entity Framework 'DbSet.Remove' method to work with in-memory table
        /// </summary>
        /// <typeparam name="T">Associated type (class) on dbset of the DbContext</typeparam>
        /// <param name="dbSet">Mocked DbSet instance</param>
        /// <param name="table">In-memory list of items</param>
        /// <returns>Configured DbSet instance for mock calls on 'DbSet.Remove' method</returns>
        public static Mock<DbSet<T>> WithRemove<T>(this Mock<DbSet<T>> dbSet, List<T> table) where T : class
        {
            dbSet.Setup(set => set.Remove(It.IsAny<T>())).Callback<T>(t => table.Remove(t));
            return dbSet;
        }

        /// <summary>
        /// Mock for Entity Framework 'DbSet.RemoveRange' method to work with in-memory table
        /// </summary>
        /// <typeparam name="T">Associated type (class) on dbset of the DbContext</typeparam>
        /// <param name="dbSet">Mocked DbSet instance</param>
        /// <param name="table">In-memory list of items</param>
        /// <returns>Configured DbSet instance for mock calls on 'DbSet.RemoveRange' method</returns>
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
