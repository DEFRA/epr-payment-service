using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace EPR.Payment.Service.Common.UnitTests.TestHelpers
{
    [ExcludeFromCodeCoverage]
    public class TestHelperDbAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
    {
        public TestHelperDbAsyncEnumerable(IEnumerable<T> enumerable)
            : base(enumerable)
        { }

        public TestHelperDbAsyncEnumerable(Expression expression)
            : base(expression)
        { }

        public IDbAsyncEnumerator<T> GetAsyncEnumerator()
        {
            return new TestHelperDbAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }

        IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
        {
            return GetAsyncEnumerator();
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestHelperDbAsyncQueryProvider<T>(this); }
        }
    }
}