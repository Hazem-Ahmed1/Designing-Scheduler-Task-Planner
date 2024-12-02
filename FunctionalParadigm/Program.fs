open System
open IOManager
open Task
open TaskManager

[<EntryPoint>]
let main (_: string array): int =
    let rec loop (): unit =
        printfn "Task Scheduler"
        printfn "1. Add Task"
        printfn "2. View Tasks"
        printfn "3. Mark Task as Completed"
        printfn "4. Delete Task"
        printfn "5. Exit"
        printfn "Choose an option: "
        match Console.ReadLine() with
        | "1" ->
            printfn "Enter task description: "
            let description = Console.ReadLine()
            printfn "Enter due date (YYYY-MM-DD): "
            let dueDate = DateTime.Parse(Console.ReadLine())
            printfn "Enter priority (1-5): "
            let priority = Int32.Parse(Console.ReadLine())
            let Status = Task.Pending
            let newTask = TaskManager.addTask description dueDate priority Status
            IOManager.addTaskToDb newTask
            printfn "Task added."
            loop () // Tail recursion for repeating the menu
        | "2" ->
            let tasks = IOManager.loadTasks()
            tasks |> List.iter (fun t ->
                printfn "%d: %s - %A - %A" t.TaskId t.Description t.DueDate t.Status)
            loop ()
        | "3" ->
            printfn "Enter task ID to mark as completed: "
            let taskId = Int32.Parse(Console.ReadLine())
            let result = TaskManager.markCompleted taskId
            printf "%s" result
            loop ()
        | "4" ->
            printfn "Enter task ID to delete: "
            let taskId = Int32.Parse(Console.ReadLine())
            IOManager.deletetaskfromdb taskId
            printfn "Task deleted."
            loop ()
        | "5" ->
            printfn "Filtering Tasks: "
            let tasks = IOManager.loadTasks()
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


            loop ()
        | "6" ->
            printfn "Enter task ID to delete: "
            let taskId = Int32.Parse(Console.ReadLine())
            IOManager.deletetaskfromdb taskId
            printfn "Task deleted."
            loop ()
        | "7" -> 
            printfn "Exiting..."
        | _ -> 
            printfn "Invalid option, try again."
            loop ()
    loop () // Start the program loop
    0 // Exit code