module Task6

open Swensen.Unquote
open Xunit

// majority preferences
// Condorcet’s Paradox

let stringify a b =
    $"{a} -> {b}"
    
let findPersonPreference person a b =
    person
    |> Seq.filter(fun x -> x = a || x = b)
    |> Seq.map (fun x -> if x = a then stringify a b else stringify b a)
    |> Seq.head
    
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
    

[<Fact>]
let alice_preference_between_A_and_B () =
    test <@ Alice |> findPersonPreference <|"A" <| "B" = "A -> B" @>
    
[<Fact>]
let alice_preference_between_B_and_C () =
    test <@ Alice |> findPersonPreference <|"B" <| "C" = "B -> C" @>
    
[<Fact>]
let alice_preference_between_A_and_C () =
    test <@ Alice |> findPersonPreference <|"A" <| "C" = "A -> C" @>
    
[<Fact>]
let bob_preference_between_A_and_C () =
    test <@ Bob |> findPersonPreference <|"A" <| "C" = "C -> A" @>
    
[<Fact>]
let group_preference_between_A_and_B () =
    test <@ group |> findGroupPreference <| "A" <| "B" = "B -> A" @>
    
[<Fact>]
let group_preference_between_A_and_С () =
    test <@ group |> findGroupPreference <| "A" <| "C" = "C -> A" @>