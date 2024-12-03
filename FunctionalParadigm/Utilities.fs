module Utilities

open Task
open System

let rec len2 lst A : int =
    match lst with
    | [] -> A
    | _ :: xs -> len2 xs A + 1

let reverse lst =
    let rec loop remaining acc =
        match remaining with
        | [] -> acc
        | x :: xs -> loop xs (x :: acc)

    loop lst []

let rec iter2 lst f =
    match lst with
    | [] -> ()
    | x :: xs ->
        f x
        iter2 xs f

let map2 lst f =
    let rec loop lst res =
        match lst with
        | [] -> reverse res
        | x :: xs -> loop xs (f x :: res)

    loop lst []

let filter2 lst f cond =
    let rec loop lst res =
        match lst with
        | [] -> reverse res
        | x :: xs -> if f x cond then loop xs (x :: res) else loop xs res

    loop lst []

let dueDateGiven (str: string) : DateTime =
    let duration = new TimeSpan(3, 0, 0, 0)

    if str = "" then
        DateTime.Now.Add(duration)
    else
        DateTime.Parse(str)
