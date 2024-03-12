module FS.Algorithm.Training.Task12

open FSharp.Control

let urls = ["test1.com/items"; "test2.com/items"]
let httpClient = new System.Net.Http.HttpClient()

let seqOfTasks = seq {
    for url in urls do
        httpClient.GetAsync(url)
}

let handle url response =
    "All Ok"

let seq = asyncSeq {
    for url in urls do
        let! response = httpClient.GetAsync(url) |> Async.AwaitTask
        if response.IsSuccessStatusCode then yield handle url response
    }