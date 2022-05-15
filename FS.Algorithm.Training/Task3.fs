module Tests

open Swensen.Unquote
open Xunit

// Напишите функцию, которая развернёт список.
// Последний элемент должен стать первым, а первый - последним.

// [1] :: [2, 3]
// [1] @ [2] :: [3]
// [1, 2] @ [3]

let rec reverse list =
    match list with
    | head :: tail -> (reverse tail) @ [head] 
    | _ -> list
    
[<Fact>]
let acceptance_empty () =
    test <@ [] |> reverse = [] @>
    
[<Fact>]
let acceptance_single () =
    test <@ [1] |> reverse = [1] @>
    
[<Fact>]
let acceptance_many () =
    test <@ [1..5] |> reverse = [5; 4; 3; 2; 1] @>