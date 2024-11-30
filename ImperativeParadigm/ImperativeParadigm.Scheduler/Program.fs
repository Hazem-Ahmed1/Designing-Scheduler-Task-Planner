open System
open RetriveTasks
open AddTask
open UpdateTask
open DeleteTask

[<EntryPoint>]
let main argv =
    printf "1) Show Tasks
2) Add Task
3) Update Task
4) Delete Task
5) Filter Tasks (Priority, DueDate, Status)
6) Sort Tasks (Priority, CreationTime, DueDate)\n\n"

    printf "Please enter operation number: "
    let input = System.Console.ReadLine()

    match System.Int32.TryParse(input) with
    | (true, number) ->
        match number with
        | 1 ->
            printfn "You choose: Show Tasks"
            showTasks()
        | 2 ->
            printfn "You choose: Add Task"
            getUserInputAndInsertTask()
        | 3 ->
            printfn "You choose: Update Task"
            getUserInputAndUpdateTask()
        | 4 ->
            printfn "You choose: Delete Task"
            getUserInputAndDeleteTask()
        | 5 ->
            printfn "You choose: Filter Tasks"
            // Call the function to filter tasks
            //filterTasks() // Define this function
        | 6 ->
            printfn "You choose: Sort Tasks"
            // Call the function to sort tasks
            //sortTasks() // Define this function
        | _ ->
        printfn "Invalid option. Please choose a number between 1 and 6."
    | (false, _) -> printfn "Invalid input. Please enter a valid number."

    System.Console.ReadKey()
    0





