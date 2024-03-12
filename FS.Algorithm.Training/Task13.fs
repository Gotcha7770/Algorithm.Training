module FS.Algorithm.Training.Task13

open System
open Xunit
open Swensen.Unquote

// Есть товар, цена на который может меняться. Цена задается с помощью массива,
// элементы которого содержат цену и дату, с которой цена вступает в действие.
// Написать функцию, которая возвращает цену на указанную дату.

type PriceWithDate = { Price: decimal; Date: DateTime }

let maxByOrOption selector items =
     match items with
     | [] -> None
     | items -> items |> List.maxBy selector |> Option.Some
    
let getPriceByDate date (prices: PriceWithDate list option) =
    match prices with
    | Some items -> items |> List.filter (fun x -> x.Date <= date) |> maxByOrOption (_.Date) |> Option.map (_.Price)
    | None -> None

type GetPriceByDateCases() as this = 
    inherit TheoryData<DateTime, PriceWithDate list option, decimal option>()
    do  this.Add(DateTime(2000, 01, 01), None, None)
        this.Add(DateTime(2000, 01, 01), Some [], None)
        this.Add(DateTime(2000, 01, 01), Some [
            { Price = 1M; Date =  DateTime(2000, 02, 02) }
            { Price = 2M; Date =  DateTime(2000, 03, 03) }
            { Price = 4M; Date =  DateTime(2000, 04, 04) }
        ], None)
        this.Add(DateTime(2000, 01, 01), Some [
            { Price = 1M; Date =  DateTime(2000, 01, 01) }
            { Price = 2M; Date =  DateTime(2000, 03, 03) }
            { Price = 4M; Date =  DateTime(2000, 04, 04) }
        ], Some 1M)
        this.Add(DateTime(2000, 01, 01), Some [
            { Price = 1M; Date =  DateTime(2000, 02, 02) }
            { Price = 2M; Date =  DateTime(1999, 10, 10) }
            { Price = 4M; Date =  DateTime(2000, 04, 04) }
        ], Some 2M)
        this.Add(DateTime(2013, 02, 28), Some [
            { Price = 1M; Date =  DateTime(2000, 02, 02) }
            { Price = 2M; Date =  DateTime(2000, 04, 04) }
            { Price = 4M; Date =  DateTime(2014, 04, 04) }
        ], Some 2M)
    
[<Theory>]
[<ClassData(typeof<GetPriceByDateCases>)>]
let ``get price by date test`` date prices expected =
    test <@ getPriceByDate date prices = expected @>

