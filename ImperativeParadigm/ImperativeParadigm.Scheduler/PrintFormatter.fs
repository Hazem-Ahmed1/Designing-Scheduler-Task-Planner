module PrintFormatter
open TaskInfo
open System

let Print results =
    Console.Clear()
    //printfn "ID\tDescription\t\t\t\tDueDate\t\tPriority\tStatus\t\tCreatedAt"
    //printfn "  \t           \t\t\t\t       \t\t        \t       \t\t        "
    // Print the header
    printfn "+----+------------------------------------------------+------------+----------+-------------+------------+"
    printfn "| ID | Description                                    | Due Date   | Priority | Status      | Created At |"
    printfn "+----+------------------------------------------------+------------+----------+-------------+------------+"


    let today = DateTime.Now.Date
    let nearingDeadlineThreshold = today.AddDays(3.0) // Define the threshold for tasks nearing deadlines

    results |> List.iter (fun task ->
        // Change color based on task's deadline status
        if task.Status = "Completed" then
            Console.ForegroundColor <- ConsoleColor.Green // Completed tasks
        elif task.DueDate.Date < today then
            Console.ForegroundColor <- ConsoleColor.Red // Overdue tasks
        elif task.DueDate.Date <= nearingDeadlineThreshold then
            Console.ForegroundColor <- ConsoleColor.Yellow // Nearing deadline
        else
            Console.ForegroundColor <- ConsoleColor.White // Default for normal tasks

        // Print the task
        printfn "| %-2d | %-46s | %-10s | %-8d | %-11s | %-10s |"
            (task.TaskID)
            (task.Description.PadRight(30))
            (task.DueDate.ToString("yyyy/MM/dd"))
            (task.Priority)
            (task.Status.ToString().PadRight(8))
            (task.CreatedAt.ToString("yyyy/MM/dd")))

    Console.ForegroundColor <- ConsoleColor.White // Reset color to default
    // Print the footer
    printfn "+----+------------------------------------------------+------------+----------+-------------+------------+"
    // Add legend for color meanings inside a box
    printfn "\n\n+------------------------------------+"
    printfn "|            Color Legend            |"
    printfn "+------------------------------------+"

    Console.ForegroundColor <- ConsoleColor.Green
    printfn "| Green  - Completed tasks           |"
    Console.ForegroundColor <- ConsoleColor.Red
    printfn "| Red    - Overdue tasks             |"
    Console.ForegroundColor <- ConsoleColor.Yellow
    printfn "| Yellow - Tasks nearing deadlines   |"
    Console.ForegroundColor <- ConsoleColor.White
    printfn "| White  - Normal tasks              |"
    printfn "+------------------------------------+"


    printfn "\n\nPress ESC to go back."

    let rec waitForEsc () =
        let key = Console.ReadKey(true).Key
        if key = ConsoleKey.Escape then
            Console.Clear()
        else
            waitForEsc()

    waitForEsc()
