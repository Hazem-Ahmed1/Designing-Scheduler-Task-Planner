open System
open TaskManager

[<EntryPoint>]
let main (_: string array) : int =
    let rec loop () : unit =
        printfn "Task Scheduler"
        printfn "1. Add Task"
        printfn "2. View Tasks"
        printfn "3. Update Task"
        printfn "4. Delete Task"
        printfn "5. Filter Tasks"
        printfn "6. Sort Tasks"
        printfn "7. Exit"
        printf "Choose an option: "

        match Console.ReadLine() with
        | "1" ->
            Console.Clear()
            TaskManager.addTask ()
            loop ()
        | "2" ->
            Console.Clear()
            TaskManager.viewTasks ()
            loop ()
        | "3" ->
            Console.Clear()
            TaskManager.updateTask ()
            loop ()
        | "4" ->
            Console.Clear()
            TaskManager.deleteTask ()
            loop ()
        | "5" ->
            Console.Clear()
            TaskManager.filterTasks ()
            loop ()
        | "6" ->
            Console.Clear()
            TaskManager.sortTasks ()
            loop ()
        | "7" -> printfn "Exiting..."
        | _ ->
            Console.Clear()
            printfn "Invalid option, try again."
            loop ()

    loop ()
    0
