module Utilities

open System

let len2 lst =
    let rec loop lst A : int =
        match lst with
        | [] -> A
        | _ :: xs -> loop xs A + 1

    loop lst 0

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
    if str = "" then
        DateTime.Now.AddDays(3)
    else
        DateTime.Parse(str)

let validDueDate (dueDate: DateTime) = dueDate.Day >= DateTime.Now.Day

let rec waitForEsc () =
    let key = Console.ReadKey(true).Key

    if key <> ConsoleKey.Escape then
        waitForEsc ()
    else
        Console.Clear()
