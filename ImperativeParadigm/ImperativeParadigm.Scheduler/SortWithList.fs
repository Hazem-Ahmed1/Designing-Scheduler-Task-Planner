module SortWithList

open System
open TaskInfo

// Imperative sorting function
let sortTasksImperative (tasks: Task list) sortColumn =
    let mutable sortedTasks = tasks

    // Sort based on the specified column
    if not (String.IsNullOrWhiteSpace(sortColumn)) then
        match sortColumn with
        | "DueDate" ->
            sortedTasks <-
                List.sortBy (fun task -> task.DueDate) sortedTasks
        | "Priority" ->
            sortedTasks <-
                List.sortBy (fun task -> task.Priority) sortedTasks
        | "CreatedAt" ->
            sortedTasks <-
                List.sortBy (fun task -> task.CreatedAt) sortedTasks
        | _ ->
            printfn "Invalid sort column. No sorting applied."

    sortedTasks


// Function to take user input and sort tasks
let getUserInputAndSortTasksImperative () =
    // Retrieve tasks
    let tasks = RetriveTasks.showTasks()

    // Get sort criteria from the user
    printf "Enter the column to sort by (DueDate, Priority, CreatedAt): "
    let sortColumn = Console.ReadLine()

    let data = RetriveTasks.showTasks()

    // Sort tasks based on the user's choice
    sortTasksImperative tasks sortColumn