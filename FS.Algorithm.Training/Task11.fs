module Task11

open System
open Xunit
open Swensen.Unquote

let (|Prefix|_|) (prefix:string) (value:string) =
    if value.StartsWith(prefix) then
        Some(value.Substring(prefix.Length))
    else
        None

type List<'T>
    with member this.JoinToString(separator:char) =
            this
            |> Seq.map(string)
            |> (fun x -> String.Join(separator, x))

let getNextSegment (value:string) (separator:char) =
    match value.IndexOf(separator) with
    | -1 -> (value, 0)
    | 0 -> ("", 1)
    | x -> (value[..x - 1], x)

let split (value:string) (separator:char) =
    let rec loop curr acc =
        match getNextSegment curr separator with
        | "", 0 -> acc
        | s, 0 -> acc @ [s]
        | s, x -> loop curr[x..] (acc @ [s])
    loop value []

let splitTestData : obj[] list =
    [
        [| ""; List.empty<string> |]
        [| "/"; [""] |]
        [| "/one"; [""; "one"] |]
        [| "//one"; [""; ""; "one"] |]
        [| "one"; ["one"] |]
        [| "one/"; ["one"; ""] |]
        [| "one//"; ["one"; ""; ""] |]
        [| "one/second"; ["one"; ""; "second"] |]
        [| "one//second"; ["one"; ""; ""; "second"] |]
    ]

[<Theory>]
[<MemberData(nameof(splitTestData))>]
let ``split string test`` value expected =
    test <@ split value '/' = expected @>

type Segment =
    | LevelUp
    | Ordinal of string
    override this.ToString() =
        match this with
        | Ordinal value -> value
        | LevelUp       -> ".."

let getSegment value =
    match value with
    | ".." -> LevelUp
    | _    -> Ordinal value

type Path = 
    | Absolute of Segment list
    | Relative of Segment list
    override this.ToString() =
        match this with
        | Absolute segments -> "/" + segments.JoinToString '/'
        | Relative segments -> segments.JoinToString '/'
    
let getSegments items =
    [ for item in items do
         if item <> "" then yield getSegment item ]

let getPath (value:string) =
    match split value '/' with
    | ""::tail -> getSegments tail |> Absolute
    | x  -> getSegments x |> Relative