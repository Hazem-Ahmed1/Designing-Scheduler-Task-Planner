module AddTask
open System
open Microsoft.Data.SqlClient
open Config

let insertTask description dueDate priority  =
    try
        let connectionString = GetDataBaseConnection("ConstrAbdelwahed")

        let query = "INSERT INTO Tasks ([Description], DueDate, [Priority]) VALUES (@Description, @DueDate, @Priority);"

        use connection = new SqlConnection(connectionString)
        connection.Open()

        use command = new SqlCommand(query, connection)
        command.Parameters.AddWithValue("@Description", description) |> ignore
        command.Parameters.AddWithValue("@DueDate", dueDate) |> ignore
        command.Parameters.AddWithValue("@Priority", priority) |> ignore

        let rowsAffected = command.ExecuteNonQuery()
        printfn "%d row(s) inserted." rowsAffected
     with
        | ex -> printfn "Error: %s" ex.Message


let getUserInputAndInsertTask () =
    Console.Clear()
    printf "Enter task description: "
    let description = Console.ReadLine()

    printf "Enter due date (yyyy-MM-dd HH:mm:ss): "
    let dueDateInput = Console.ReadLine()
    let dueDate = DateTime.Parse(dueDateInput)

    printf "Enter priority (Low numbers have highest priority): "
    let priorityInput = Console.ReadLine()
    let priority = Int32.Parse(priorityInput)

    let sts = "Pending"
    printf "All The Tasks Inserted is Marked as "
    System.Console.ForegroundColor <- System.ConsoleColor.Green
    printfn "%s initially" sts
    System.Console.ResetColor()


    // Insert the task into the database
    insertTask description dueDate priority

