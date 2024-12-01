module RetriveTasks

open TaskInfo
open Config
open Microsoft.Data.SqlClient

let showTasks() =

    System.Console.Clear()
    try
        let connectionString = GetDataBaseConnection("ConstrAbdelrahman")
        let query = "SELECT * FROM Tasks"
        use connection = new SqlConnection(connectionString)
        connection.Open()

        let command = new SqlCommand(query, connection)

        use reader = command.ExecuteReader()

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