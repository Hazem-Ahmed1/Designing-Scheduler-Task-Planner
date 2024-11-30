module Utilities
open Task

let map f lst =
    List.foldBack (fun x acc -> f x :: acc) lst []

    // Function to filter a list
let filter pred lst =
    List.foldBack (fun x acc -> if pred x then x :: acc else acc) lst []

    // Function to sort tasks by due date
let sortByDueDate tasks =
    List.sortBy (fun t -> t.DueDate) tasks
