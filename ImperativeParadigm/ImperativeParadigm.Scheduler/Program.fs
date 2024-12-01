open System
open RetriveTasks
open AddTask
open UpdateTask
open DeleteTask
open FilterUsingQuery
open PrintFormatter
open SortUsingQuery
open FilterWithList
open SortWithList

[<EntryPoint>]
let main argv =
    let rec mainLoop () =
        printf "1) Show Tasks
2) Add Task
3) Update Task
4) Delete Task
5) Filter Tasks (Priority, DueDate, Status)
6) Sort Tasks (Priority, CreationTime, DueDate)
Q) Quit\n\n"

        printf "Please enter operation number (or Q to quit): "
        let input = System.Console.ReadLine()

        if input.Trim().ToUpper() = "Q" then
            printfn "Exiting the program. Goodbye!"
        else
            match System.Int32.TryParse(input) with
            | (true, number) ->
                match number with
                | 1 ->
                    printfn "You chose: Show Tasks"
                    Print(showTasks())
                | 2 ->
                    printfn "You chose: Add Task"
                    getUserInputAndInsertTask()
                | 3 ->
                    printfn "You chose: Update Task"
                    getUserInputAndUpdateTask()
                | 4 ->
                    printfn "You chose: Delete Task"
                    getUserInputAndDeleteTask()
                | 5 ->
                    printfn "You chose: Filter Tasks"
                    //Print(getUserInputAndFilterTasks())
                    Print(getUserInputAndFilterTasksImpartive())
                | 6 ->
                    printfn "You chose: Sort Tasks"
                    // Call the function to sort tasks
                    //Print(getUserInputAndSortTasks()) // Define this function
                    Print(getUserInputAndSortTasksImperative())
                | _ ->
                    printfn "Invalid option. Please choose a number between 1 and 6."
            | (false, _) -> printfn "Invalid input. Please enter a valid number."

            // Restart the loop
            mainLoop()

    // Start the main loop
    mainLoop()
    0
