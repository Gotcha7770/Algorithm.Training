module FS.Algorithm.Training.Task14

open Xunit
open Swensen.Unquote

let standard = [
        "1";
        "2";
        "Fizz";
        "4";
        "Buzz";
        "Fizz";
        "7";
        "8";
        "Fizz";
        "Buzz";
        "11";
        "Fizz";
        "13";
        "14";
        "FizzBuzz";
        "16";
        "17";
        "Fizz";
        "19";
        "Buzz";
        "Fizz";
        "22";
        "23";
        "Fizz";
        "Buzz"
    ]

let (|DivisibleBy|_|) divisor n = 
    if n % divisor = 0 then Some() else None

let fizzBuzz1 n =    
    let selector x =
        match x with
        | DivisibleBy 15 -> "FizzBuzz"
        | DivisibleBy 3 -> "Fizz"
        | DivisibleBy 5 -> "Buzz"
        | _ -> x.ToString()
    
    [1..n] |> List.map selector
    
[<Fact>]
let acceptance1 () =
    test <@ fizzBuzz1 25 = standard @>
    
let fizzBuzz2 n =
    let check condition result x = if condition x then result else ""
    let fizz = check (fun x -> x % 3 = 0) "Fizz"
    let buzz = check (fun x -> x % 5 = 0) "Buzz"

    let selector x =
        let result = fizz x + buzz x
        if result = "" then x.ToString() else result

    [1..n] |> List.map selector
    
[<Fact>]
let acceptance2 () =
    test <@ fizzBuzz2 25 = standard @>
    
let fizzBuzz3 n =
    let selector x =
        [(3, "Fizz"); (5, "Buzz")]
        |> List.fold (fun acc (divisor, word) -> if x % divisor = 0 then acc + word else acc) ""
        |> fun result -> if result = "" then x.ToString() else result

    [1..n] |> List.map selector
    
[<Fact>]
let acceptance3 () =
    test <@ fizzBuzz3 25 = standard @>