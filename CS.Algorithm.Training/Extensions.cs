using System;
using System.Collections.Generic;
using System.Threading;

namespace Algorithm.Training;

public static partial class AsyncEnumerable
{
    public static IAsyncEnumerable<T> Create<T>(Func<CancellationToken, IAsyncEnumerator<T>> factory)
    {
        return new AnonymousAsyncEnumerable<T>(factory);
    }

    private sealed class AnonymousAsyncEnumerable<T> : IAsyncEnumerable<T>
    {
        private readonly Func<CancellationToken, IAsyncEnumerator<T>> _factory;

        public AnonymousAsyncEnumerable(Func<CancellationToken, IAsyncEnumerator<T>> factory) => _factory = factory;

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken)
        {
            cancellationToken
                .ThrowIfCancellationRequested(); // NB: [LDM-2018-11-28] Equivalent to async iterator behavior.

            return _factory(cancellationToken);
        }
    }
}