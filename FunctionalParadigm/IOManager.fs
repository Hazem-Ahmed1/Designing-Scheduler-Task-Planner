module IOManager

open Dapper
open Microsoft.Data.SqlClient
open Task
open Utilities

module IOManager =
    let connectionString =
        "Data Source=LAPTOP-7EVQ62QN\\SQLEXPRESS;Initial Catalog=Scheduler;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"

    let loadTasks () =
        System.Console.Clear()

        try
            let query = "SELECT * FROM Tasks"
            use connection = new SqlConnection(connectionString)
            connection.Open()

            let command = new SqlCommand(query, connection)
            use reader = command.ExecuteReader()

            let results =
                let rows =
                    seq {
                        while reader.Read() do
                            yield
                                { TaskId = reader.GetInt32(0)
                                  Description = reader.GetString(1)
                                  DueDate = reader.GetDateTime(2)
                                  Priority = reader.GetInt32(3)
                                  CreatedAt = reader.GetDateTime(4)
                                  Status = stringToStatus (reader.GetString(5)) }
                    }
                    |> List.ofSeq

                rows

            results
        with ex ->
            printfn "Error: %s" ex.Message
            []

    // add a task to the database
    let addTaskToDb (task: TaskDTO) =
        use connection = new SqlConnection(connectionString)
        connection.Open()

        let sql =
            "INSERT INTO Tasks (Description, DueDate, Priority) VALUES (@Description, @DueDate, @Priority)"

        connection.Execute(sql, task) |> ignore

    // update a task in the database
    let updatetaskindb (task: Task) =
        use connection = new SqlConnection(connectionString)
        connection.Open()

        let sql =
            "update tasks set description = @description, duedate = @duedate, priority = @priority, status = @status where Task_ID = @taskid"

        connection.Execute(
            sql,
            {| TaskId = task.TaskId
               Description = task.Description
               DueDate = task.DueDate
               Priority = task.Priority
               Status = statusToString task.Status |}
        )
        |> ignore

    let deletetaskfromdb taskid =
        use connection = new SqlConnection(connectionString)
        connection.Open()
        let sql = "delete from tasks where Task_ID = @taskid"
        connection.Execute(sql, box {| taskid = taskid |}) |> ignore
