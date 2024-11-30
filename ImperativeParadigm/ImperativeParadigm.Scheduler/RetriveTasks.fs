module RetriveTasks

open TaskInfo
open Microsoft.Data.SqlClient

let connectionString = "Data Source=DESKTOP-PAV8I18\SQLEXPRESS;Initial Catalog=Scheduler;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";;

let showTasks() =
    System.Console.Clear()
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
                    Status = reader.GetString(4)
                    CreatedAt = reader.GetDateTime(5)
                }
             }
        rows |> List.ofSeq

    printfn "ID\tDescription\t\t\t\tDueDate\t\tPriority\tStatus\t\tCreatedAt"
    printfn "  \t           \t\t\t\t       \t\t        \t       \t\t        "
    results |> List.iter (fun task ->
                                    printfn "%d\t%-15s\t\t%s\t%-10d\t%-10s\t%s\t"
                                        (task.TaskID)
                                        (task.Description.PadRight(30))
                                        (task.DueDate.ToString("MM/dd/yyyy"))
                                        (task.Priority)
                                        (task.Status.ToString().PadRight(10))
                                        (task.CreatedAt.ToString("MM/dd/yyyy")))