open System
open TaskManager
open IOManager

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
            let newTask = TaskManager.addTask description dueDate priority
            IOManager.addTaskToDb newTask
            printfn "Task added."
            loop () // Tail recursion for repeating the menu
        | "2" ->
            let tasks = IOManager.loadTasks()
            tasks |> List.iter (fun t ->
                let statusStr = 
                    match t.Status with
                    | Task.Pending -> "Pending"
                    | Task.Completed -> "Completed"
                    | Task.Overdue -> "Overdue"
                printfn "%d: %s - %A - %s" t.TaskId t.Description t.DueDate statusStr)
            loop ()
        | "3" ->
            printfn "Enter task ID to mark as completed: "
            let taskId = Int32.Parse(Console.ReadLine())
            let tasks = IOManager.loadTasks()
            match tasks |> List.tryFind (fun t -> t.TaskId = taskId) with
            | Some task ->
                let updatedTask = TaskManager.markCompleted task
                IOManager.updateTaskInDb updatedTask
                printfn "Task marked as completed."
            | None -> printfn "Task not found."
            loop ()
        | "4" ->
            printfn "Enter task ID to delete: "
            let taskId = Int32.Parse(Console.ReadLine())
            IOManager.deleteTaskFromDb taskId
            printfn "Task deleted."
            loop ()
        | "5" -> 
            printfn "Exiting..."
        | _ -> 
            printfn "Invalid option, try again."
            loop ()
    loop () // Start the program loop
    0 // Exit code