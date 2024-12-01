module FilterWithList

open RetriveTasks
open TaskInfo
open System

// Imperative filtering function
let filterTasksImperative (tasks: Task list) status priority dueDate =
    try
        let mutable filteredTasks = tasks

        if not (String.IsNullOrWhiteSpace(status)) then
            filteredTasks <-
                [ for task in filteredTasks do
                    if task.Status = status then yield task ]

        if priority <> 0 then
            filteredTasks <-
                [ for task in filteredTasks do
                    if task.Priority = priority then yield task ]

        if dueDate <> DateTime.MinValue then
            filteredTasks <-
                [ for task in filteredTasks do
                    if task.DueDate.Date = dueDate.Date then yield task ]

        filteredTasks

    with |ex -> printfn "Error: %s" ex.Message
                []


let getUserInputAndFilterTasksImpartive () =
    // Retrieve Tasks From Database
    let data = RetriveTasks.showTasks()

    printf "Enter status Pending, Completed or Overdue (or leave blank to skip): "
    let status = Console.ReadLine()

    printf "Enter priority (Enter Valid Number or 0 to skip): "
    let priorityInput = Console.ReadLine()
    let priority = if String.IsNullOrWhiteSpace(priorityInput) then 0 else Int32.Parse(priorityInput)

    printf "Enter Specific due date (yyyy-MM-dd, or leave blank to skip): "
    let dueDateInput = Console.ReadLine()
    let dueDate =
        match DateTime.TryParse(dueDateInput) with
        | (true, parsedDate) -> parsedDate
        | (false, _) ->
            printfn "NO FILTER USING DATE"
            DateTime.MinValue


    // Filter and display tasks
    filterTasksImperative data status priority dueDate