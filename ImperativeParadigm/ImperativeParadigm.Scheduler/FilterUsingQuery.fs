﻿module FilterUsingQuery

open System
open Microsoft.Data.SqlClient
open Config
open TaskInfo

let filterTasks status priority dueDate =
     try
        let connectionString = GetDataBaseConnection("ConstrAbdelwahed")
        let query = 
                "SELECT *
                 FROM Tasks 
                 WHERE (@Status IS NULL OR Status = @Status) 
                 AND  (@Priority IS NULL OR Priority = @Priority) 
             AND (@DueDate IS NULL OR CONVERT(VARCHAR(10), DueDate, 120) = CONVERT(VARCHAR(10), @DueDate, 120));"
            // Create and open the database connection
        use connection = new SqlConnection(connectionString)
        connection.Open()

            // Create and execute the SQL command
        use command = new SqlCommand(query, connection)
        command.Parameters.AddWithValue("@Status", if String.IsNullOrWhiteSpace(status) then DBNull.Value :> obj else status) |> ignore
        command.Parameters.AddWithValue("@Priority", if priority = 0 then DBNull.Value :> obj else priority) |> ignore
        command.Parameters.AddWithValue("@DueDate", if dueDate = DateTime.MinValue then DBNull.Value :> obj else dueDate) |> ignore

        use reader = command.ExecuteReader()
        Console.Clear()
        printfn "Filtered Tasks:"
        let results =
            let rows =
                seq {
                while reader.Read() do
                    yield {
                        TaskID = reader.GetInt32(0)
                        Description = reader.GetString(1)
                        DueDate = reader.GetDateTime(2)
                        Priority = reader.GetInt32(3)
                        CreatedAt = reader.GetDateTime(4)
                        Status = reader.GetString(5)
                    }
                 }
            rows |> List.ofSeq
        results
      with | ex -> 
        printfn "Error: %s" ex.Message
        []

// Function to take user input for filtering tasks
let getUserInputAndFilterTasks () =
    // Get filter criteria from the user
    printf "Enter status (or leave blank to skip): "
    let status = Console.ReadLine()

    printf "Enter priority (Enter Valid Number or 0 to skip): "
    let priorityInput = Console.ReadLine()
    let priority = if String.IsNullOrWhiteSpace(priorityInput) then 0 else Int32.Parse(priorityInput)

    printf "Enter Specific due date (yyyy-MM-dd, or leave blank to skip): "
    let dueDateInput = Console.ReadLine()
    let dueDate =
        match DateTime.TryParse(dueDateInput) with
        | (true, parsedDate) -> parsedDate // Successfully parsed
        | (false, _) -> 
            printfn "NO FILTER USING DATE"
            DateTime.MinValue // Use this to skip filtering


    // Filter and display tasks
    filterTasks status priority dueDate