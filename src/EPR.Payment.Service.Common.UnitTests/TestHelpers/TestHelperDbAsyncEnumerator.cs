using System.Data.Entity.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace EPR.Payment.Service.Common.UnitTests.TestHelpers
{
    [ExcludeFromCodeCoverage]
    public class TestHelperDbAsyncEnumerator<T> : IDbAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestHelperDbAsyncEnumerator(IEnumerator<T> inner)
        {
            _inner = inner;
        }

        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
   
        }

        public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_inner.MoveNext());
        }

        public T Current
        {
            get { return _inner.Current; }
        }

        object? IDbAsyncEnumerator.Current
        {
            get { return Current; }
        }
    }
}