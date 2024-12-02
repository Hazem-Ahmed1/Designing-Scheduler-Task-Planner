open System
open IOManager
open Task
open TaskManager
open Utilities

let addTask () =
    printfn "Enter task description: "
    let description = Console.ReadLine()
    printfn "Enter due date (YYYY-MM-DD): Default (3 days from now)"
    let dueDate = dueDateGiven (Console.ReadLine())
    printfn "Enter priority (1-5): (Lower is higher)"
    let priority = Int32.Parse(Console.ReadLine())
    let Status = Pending
    let newTask = bindTask description dueDate priority Status
    IOManager.addTaskToDb newTask
    printfn "Task added."

let viewTasks () =
    let tasks = IOManager.loadTasks ()

    let length = len2 tasks 0
    printfn "%d" length

    iter2 tasks printTask

let updateTask () =
    printfn "Update Task"
    printfn "1. Mark task as completed"
    printfn "2. Update task priority"

    match Console.ReadLine() with
    | "1" -> addTask ()
    | "2" -> viewTasks ()
    | _ -> printfn "Invalid option, try again."

    printfn "Enter task ID to mark as completed: "
    let taskId = Int32.Parse(Console.ReadLine())
    let result = TaskManager.markCompleted taskId
    printf "%s" result

let deleteTask () =
    printfn "Enter task ID to delete: "
    let taskId = Int32.Parse(Console.ReadLine())
    IOManager.deletetaskfromdb taskId
    printfn "Task deleted."

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

[<EntryPoint>]
let main (_: string array) : int =
    let rec loop () : unit =
        printfn "Task Scheduler"
        printfn "1. Add Task"
        printfn "2. View Tasks"
        printfn "3. Update Task"
        printfn "4. Delete Task"
        printfn "5. Filtering Tasks"
        printfn "6. Sort Tasks"
        printfn "7. Exit"
        printfn "Choose an option: "

        match Console.ReadLine() with
        | "1" ->
            addTask ()
            loop ()
        | "2" ->
            viewTasks ()
            loop ()
        | "3" ->
            updateTask ()
            loop ()
        | "4" ->
            deleteTask ()
            loop ()
        | "5" ->
            filterTasks ()
            loop ()
        | "6" ->
            sortTasks ()
            loop ()
        | "7" -> printfn "Exiting..."
        | _ ->
            printfn "Invalid option, try again."
            loop ()

    loop ()
    0
