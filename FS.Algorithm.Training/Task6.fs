module Task6

open Swensen.Unquote
open Xunit

// majority preferences
// Condorcet’s Paradox

let stringify a b =
    $"{a} -> {b}"
    
let rec findPersonPreference person a b =
    match person with
    | head :: _ when head = a -> stringify a b
    | head :: _ when head = b -> stringify b a
    | _ :: tail -> findPersonPreference tail a b
    | [] -> ""
    
let findGroupPreference group a b =
    group
    |> Seq.map (fun x -> findPersonPreference x a b)
    |> Seq.countBy id
    |> Seq.sortByDescending snd
    |> Seq.map fst
    |> Seq.head
    
let Alice = ["A"; "B"; "C"]
let Bob = ["B"; "C"; "A"]

let group =
    [Seq.replicate 23 ["A"; "B"; "C"];
     Seq.replicate 19 ["B"; "C"; "A"]
     Seq.replicate 16 ["C"; "B"; "A"]
     Seq.replicate 2 ["C"; "A"; "B"]]
    |> Seq.concat

[<Theory>]
[<InlineData("A", "B", "A -> B")>]
[<InlineData("B", "C", "B -> C")>]
[<InlineData("A", "C", "A -> C")>]
[<InlineData("C", "A", "A -> C")>]
let alice_preferences a b expected=
    test <@ findPersonPreference Alice a b = expected @>
    
[<Theory>]
[<InlineData("A", "B", "B -> A")>]
[<InlineData("B", "C", "B -> C")>]
[<InlineData("A", "C", "C -> A")>]
[<InlineData("C", "A", "C -> A")>]
let bob_preferences a b expected =
    test <@ findPersonPreference Bob a b = expected @>
    
[<Theory>]
[<InlineData("A", "B", "B -> A")>]
[<InlineData("B", "C", "B -> C")>]
[<InlineData("A", "C", "C -> A")>]
let group_preferences a b expected =
    test <@ findGroupPreference group a b = expected @>