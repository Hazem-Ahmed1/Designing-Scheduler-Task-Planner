module TaskManager

open System
open Task
open IOManager
open Utilities

module TaskManager =

    let markAsCompleted taskId =
        let tasks = IOManager.loadTasks ()

        match tasks |> List.tryFind (fun t -> t.TaskId = taskId) with
        | Some task ->
            let updatedTask = { task with Status = Completed }

            IOManager.updatetaskindb updatedTask
            printfn "Task marked as completed."
        | None -> printfn "Task not found."

    let changePriority taskId priority =
        let tasks = IOManager.loadTasks ()

        match tasks |> List.tryFind (fun t -> t.TaskId = taskId) with
        | Some task ->
            let updatedTask = { task with Priority = priority }

            IOManager.updatetaskindb updatedTask
            printfn "Task priority updated."
        | None -> printfn "Task not found."

    let addTask () =
        printf "Enter task description: "
        let description = Console.ReadLine()
        printf "Enter due date (YYYY-MM-DD): Default (3 days from now)"
        let dueDate = dueDateGiven (Console.ReadLine())
        printf "Enter priority (1-5): (Lower is higher)"
        let priority = Int32.Parse(Console.ReadLine())
        let Status = Pending
        let newTask = bindTaskDTO description dueDate priority Status
        IOManager.addTaskToDb newTask
        Console.Clear()
        printfn "Task added successfully."

    let viewTasks () =
        let tasks = IOManager.loadTasks ()

        iter2 tasks printTask

    let updateTask () =
        printfn "Update Task"
        printf "Enter task ID to update: "
        let taskId = Int32.Parse(Console.ReadLine())

        printfn "1. Mark task as completed"
        printfn "2. Update task priority"
        printf "Choose an option: "

        match Console.ReadLine() with
        | "1" -> markAsCompleted taskId
        | "2" ->
            printf "Enter updated priority (1-5): "
            let newPriority = Int32.Parse(Console.ReadLine())
            changePriority taskId newPriority
        | _ ->
            printfn "Invalid option"
            Console.Clear()

    let deleteTask () =
        printf "Enter task ID to delete: "
        let taskId = Int32.Parse(Console.ReadLine())
        IOManager.deletetaskfromdb taskId
        Console.Clear()
        printfn "Task deleted successfully."

    let filterTasks () =
        printfn "Filtering Tasks: "
        let tasks = IOManager.loadTasks ()
        printf "Enter status Pending, Completed or Overdue (or leave blank to skip): "
        let status = Console.ReadLine()
        printf "Enter priority (Enter Valid Number or 0 to skip): "
        let priorityInput = Console.ReadLine()

        let priority =
            if String.IsNullOrWhiteSpace(priorityInput) then
                0
            else
                Int32.Parse(priorityInput)

        printf "Enter Specific due date (yyyy-MM-dd, or leave blank to skip): "
    // let dueDateInput = Console.ReadLine()

    // let dueDate =
    //     match DateTime.TryParse(dueDateInput) with
    //     | (true, parsedDate) -> parsedDate
    //     | (false, _) ->
    //         printfn "NO FILTER USING DATE"
    //         DateTime.MinValue

    let sortTasks () = printf "Not yet implemented"

    let rec merge (left: Task list) (right: Task list) : Task list =
        match left, right with
        | [], ys -> ys
        | xs, [] -> xs
        | x :: xs, y :: ys when x.Priority <= y.Priority -> x :: merge xs right
        | x :: xs, y :: ys -> y :: merge left ys


    let rec mergeSort (tasks: Task List) =
        match tasks with
        | []
        | [ _ ] -> tasks
        | _ ->
            let mutable middle = len2 tasks 0
            middle <- middle / 2
            let left = tasks |> List.take middle
            let right = tasks |> List.skip middle
            merge (mergeSort left) (mergeSort right)
