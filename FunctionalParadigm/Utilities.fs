module Utilities

open Task
open System

let rec iter2 lst f =
    match lst with
    | [] -> ()
    | x :: xs ->
        f x
        iter2 xs f

let rec len2 lst A : int =
    match lst with
    | [] -> A
    | x :: xs -> len2 xs A + 1

// Function to filter a list
let filter pred lst =
    List.foldBack (fun x acc -> if pred x then x :: acc else acc) lst []

// Function to sort tasks by due date
let sortByDueDate tasks = List.sortBy (fun t -> t.DueDate) tasks

let dueDateGiven (str: string) : DateTime =
    let duration = new TimeSpan(3, 0, 0, 0)

    if str = "" then
        DateTime.Now.Add(duration)
    else
        DateTime.Parse(str)
