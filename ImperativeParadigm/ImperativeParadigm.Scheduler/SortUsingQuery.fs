module SortUsingQuery

open System
open Microsoft.Data.SqlClient
open Config
open TaskInfo
open System.Windows.Forms
open System.Drawing

let SortTasks sortColumn =
    try
        let connectionString = GetDataBaseConnection("ConstrAbdelwahed")
        let query =
                    "SELECT *
                     FROM Tasks
                     ORDER BY
                        CASE @SortColumn
                            WHEN 'DueDate' THEN DueDate
                            WHEN 'Priority' THEN Priority
                            WHEN 'CreatedAt' THEN CreatedAt
                        END;"

        use connection = new SqlConnection(connectionString)
        connection.Open()

        use command = new SqlCommand(query, connection)

        command.Parameters.AddWithValue("@SortColumn", sortColumn) |> ignore

        use reader = command.ExecuteReader()
        Console.Clear()
        printfn "Sorted Tasks:"
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
    with
    | ex ->
        printfn "Error: %s" ex.Message
        []


let getUserInputAndSortTasks () =


    printf "Enter the column to sort by (DueDate, Priority, CreatedAt): "
    let sortColumn = Console.ReadLine()

    // Sort and display tasks
    SortTasks sortColumn



