module UpdateTask

open System
open Microsoft.Data.SqlClient
open Config

let updateTask taskId newDescription newDueDate newPriority newStatus =
     try
        let connectionString = GetDataBaseConnection("ConstrAbdelwahed")

        let query = "UPDATE Tasks
                 SET Description = @Description,
                     DueDate = @DueDate,
                     Priority = @Priority,
                     Status = @Status
                 WHERE Task_ID = @Task_ID"

        use connection = new SqlConnection(connectionString)
        connection.Open()

        use command = new SqlCommand(query, connection)
        command.Parameters.AddWithValue("@Task_ID", taskId) |> ignore
        command.Parameters.AddWithValue("@Description", newDescription) |> ignore
        command.Parameters.AddWithValue("@DueDate", newDueDate) |> ignore
        command.Parameters.AddWithValue("@Priority", newPriority) |> ignore
        command.Parameters.AddWithValue("@Status", newStatus) |> ignore

        let rowsAffected = command.ExecuteNonQuery()
        printfn "%d row(s) updated." rowsAffected

     with
        | ex -> printfn "Error: %s" ex.Message


let getUserInputAndUpdateTask () =
    Console.Clear()

    printf "Enter Task ID to update: "
    let taskIdInput = Console.ReadLine()
    let taskId = Int32.Parse(taskIdInput)

    printf "Enter new task description: "
    let newDescription = Console.ReadLine()

    printf "Enter new due date (yyyy-MM-dd HH:mm:ss): "
    let newDueDateInput = Console.ReadLine()
    let newDueDate = DateTime.Parse(newDueDateInput)

    printf "Enter new priority (Low numbers have highest priority): "
    let newPriorityInput = Console.ReadLine()
    let newPriority = Int32.Parse(newPriorityInput)

    printf "Enter new status (Pending, Completed, Overdue): "
    let newStatus = Console.ReadLine()

    // Update the task in the database
    updateTask taskId newDescription newDueDate newPriority newStatus
