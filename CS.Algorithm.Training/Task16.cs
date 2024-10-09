using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Algorithm.Training;

/// <summary>
/// Задача:
/// Существует клиент, который возвращает данные с сервера по Ids запроса.
/// Во входных параметрах передается IDs и максимальный уровень параллелизма
/// то есть как пример передаем IDs [1, 2, 3, 4, 5, 6, 7, 8] и maxDegreeOfParallelism 2
/// в клиент запрашивается пачками по 2 ID.
/// Сервер - сервер возвращает ответы по мере их отработки, то есть некоторые запросы могут быть обратаны сразу, некоторые с задержкой.
/// </summary>
public class Task16
{
    private record Response(long Id);

    private interface IClient
    {
        Task<Response> GetAsync(long id, CancellationToken cancellationToken = default);
    }

    private class Client : IClient
    {
        private readonly Random _random = new();

        public async Task<Response> GetAsync(long id, CancellationToken cancellationToken)
        {
            var nextDelay = _random.Next(1, 5);
            await Task.Delay(TimeSpan.FromSeconds(nextDelay), cancellationToken);

            return new Response(id);
        }
    }

    private IAsyncEnumerable<Response> GetTaskArrayAsync1(
        IClient client,
        long[] ids,
        int maxDegreeOfParallelism)
    {
        return AsyncEnumerable.Create(Iterator);

        async IAsyncEnumerator<Response> Iterator(CancellationToken cancellationToken)
        {
            var tasks = ids.AsParallel()
                .WithDegreeOfParallelism(maxDegreeOfParallelism)
                .Select(id => client.GetAsync(id, cancellationToken));

            foreach (var task in tasks)
            {
                yield return await task;
            }
        }
    }
    
    private IAsyncEnumerable<Response> GetTaskArrayAsync2(
        IClient client,
        long[] ids,
        int maxDegreeOfParallelism)
    {
        return AsyncEnumerable.Create(Iterator);

        async IAsyncEnumerator<Response> Iterator(CancellationToken cancellationToken)
        {
            foreach (var batch in ids.Buffer(maxDegreeOfParallelism))
            {
                var tasks = batch.Select(x => client.GetAsync(x, cancellationToken));

                foreach (var response in await Task.WhenAll(tasks))
                {
                    yield return response;
                }
            }
        }
    }

    private IAsyncEnumerable<Response> GetTaskArrayAsync3(
        IClient client,
        long[] ids,
        int maxDegreeOfParallelism)
    {
        return AsyncEnumerable.Create(Iterator);

        async IAsyncEnumerator<Response> Iterator(CancellationToken cancellationToken)
        {
            using var semaphore = new SemaphoreSlim(maxDegreeOfParallelism);
        
            var tasks = ids.Select(async id =>
            {
                await semaphore.WaitAsync(cancellationToken);
                try
                {
                    return await client.GetAsync(id, cancellationToken);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToArray();

            foreach (var response in await Task.WhenAll(tasks))
            {
                yield return response;
            }
        }
    }
    
    [Fact]
    public async Task Acceptance()
    {
        IClient client = new Client();
        var result = await GetTaskArrayAsync1(client, [1, 2, 3, 4, 5, 6, 7, 8], 2).ToArrayAsync();

        result.Should()
            .BeEquivalentTo(
            [
                new Response(1),
                new Response(2),
                new Response(3),
                new Response(4),
                new Response(5),
                new Response(6),
                new Response(7),
                new Response(8),
            ]);
    }
}