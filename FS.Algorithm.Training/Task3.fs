module Task3

open Xunit
open Swensen.Unquote

// Напишите функцию, которая развернёт список.
// Последний элемент должен стать первым, а первый - последним.

// [1] :: [2, 3]
// [2] @ [1] :: [3]
// [3] @ [2, 1] :: []

let reverse list =
    let rec loop list acc =
        match list with
        | [] -> acc
        | [x] -> x::acc
        | head :: tail -> loop tail (head :: acc)
    loop list []
    
[<Fact>]
let acceptance_empty () =
    test <@ [] |> reverse = [] @>
    
[<Fact>]
let acceptance_single () =
    test <@ [1] |> reverse = [1] @>
    
[<Fact>]
let acceptance_many () =
    test <@ [1..5] |> reverse = [5; 4; 3; 2; 1] @>