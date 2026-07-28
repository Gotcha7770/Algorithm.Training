module FS.Algorithm.Training.Task18

open Xunit
open Swensen.Unquote

// calque from C# LINQ implementation
let product1 (input: seq<seq<'a>>) =
    (seq { Seq.empty }, input)
    ||> Seq.fold (fun acc cur ->
        seq {
            for prefix in acc do
                for item in cur do
                    yield
                        seq {
                            yield! prefix
                            yield item
                        }
        })

// optimized using lists
let product2 (input: seq<seq<'a>>) =
    (seq { [] }, input)
    ||> Seq.fold (fun acc cur ->
        seq {
            for prefix in acc do
                for item in cur do
                    yield item :: prefix
        })
    |> Seq.map List.rev

// recursive using lists
let rec product3 (input: list<seq<'a>>) =
    match input with
    | [] -> [ [] ]
    | head :: tail ->
        let suffixes = product3 tail
        [ for item in head do
              for rest in suffixes do
                  yield item :: rest ]

// let productWithCondition (condition: 'a -> 'a -> bool) (input: seq<seq<'a>>) : 'a list list =
//     ([[]], input)
//     ||> Seq.fold (fun acc cur ->
//         acc
//         |> List.collect (fun prev ->
//             cur
//             // |> Seq.filter (fun item -> match List.tryLast prev with
//             //                             | Some x -> condition x item
//             //                             | None -> true)
//             |> Seq.map (fun item -> prev @ item))
//         )


type ProductCases() as this =
    inherit TheoryData<int list list, int list list>()
    do this.Add([ [ 1; 2 ]; [ 3; 4 ] ], [ [ 1; 3 ]; [ 1; 4 ]; [ 2; 3 ]; [ 2; 4 ] ])

[<Fact>]
let ``seq equivalence`` () =
    test
        <@
            seq {
                1
                2
                3
            } = seq {
                1
                2
                3
            }
        @>

[<Fact>]
let ``built-in cartesian product only for 2 sequences`` () =
    test <@ ([ 1; 2 ], [ 3; 4 ]) ||> Seq.allPairs |> Seq.toList = [ (1, 3); (1, 4); (2, 3); (2, 4) ] @>

[<Theory>]
[<ClassData(typeof<ProductCases>)>]
let ``get cartesian product`` input expected = test <@ product3  (List.ofSeq input) = expected @>
