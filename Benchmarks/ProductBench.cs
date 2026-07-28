using Algorithm.Training;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Microsoft.FSharp.Collections;

namespace Benchmarks;

[MemoryDiagnoser]
public class ProductBench
{
    public static IEnumerable<ProductCase> Cases =>
    [
        // фиксированная глубина
        new(2, 5),
        new(5, 5),
        new(10, 5),

        // фиксированная ширина
        new(2, 10),
        new(2, 15),
        new(2, 20),
    ];

    [ParamsSource(nameof(Cases))] 
    public ProductCase Case { get; set; }

    private IEnumerable<IEnumerable<int>> Items =>
        Enumerable.Range(0, Case.Depth)
            .Select(_ => Enumerable.Range(1, Case.Width));

    private FSharpList<IEnumerable<int>> ItemsList => ListModule.OfSeq(Items);

    private readonly Consumer _consumer = new();

    [Benchmark(Baseline = true)]
    public void CSharp_Product()
    {
        Items
            .Product()
            .Select(x => x.ToArray())
            .Consume(_consumer);
    }
    
    [Benchmark]
    public void FSharp_Product1()
    {
        FS.Algorithm.Training.Task18.product1(Items)
            .Select(x => x.ToArray())
            .Consume(_consumer);
    }

    [Benchmark]
    public void FSharp_Product2()
    {
        FS.Algorithm.Training.Task18.product2(Items)
            .Select(x => x.ToArray())
            .Consume(_consumer);
    }

    [Benchmark]
    public void FSharp_Product3()
    {
        FS.Algorithm.Training.Task18.product3(ItemsList)
            .Consume(_consumer);
    }
}

public record ProductCase(int Width, int Depth)
{
    public override string ToString()
    {
        return Width == 2
            ? $"2^{Depth}"
            : string.Join("×", Enumerable.Repeat(Width, Depth));
    }
}