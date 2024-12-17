module IOManager

open Dapper
open Microsoft.Data.SqlClient
open Task
open Utilities

module IOManager =
    let private readerToTask (reader: SqlDataReader) =
        { TaskId = reader.GetInt32(0)
          Description = reader.GetString(1)
          DueDate = reader.GetDateTime(2)
          Priority = reader.GetInt32(3)
          CreatedAt = reader.GetDateTime(4)
          Status = stringToStatus (reader.GetString(5)) }

    let private readAllRows (reader: SqlDataReader) =
        let rec readRows acc =
            match reader.Read() with
            | true -> readRows (readerToTask reader :: acc)
            | false -> List.rev acc
        readRows []

    let loadTasks () =
        let connectionString =
            "Data Source=localhost;Initial Catalog=Scheduler;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"
        System.Console.Clear()
        try
            use connection = new SqlConnection(connectionString)
            connection.Open()
            
            use command = new SqlCommand("SELECT * FROM Tasks", connection)
            use reader = command.ExecuteReader()
            
            readAllRows reader
        with ex ->
            printfn "Error: %s" ex.Message
            []

    // add a task to the database
    let addTaskToDb (task: TaskDTO) =
        let connectionString =
            "Data Source=localhost;Initial Catalog=Scheduler;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"
        use connection = new SqlConnection(connectionString)
        connection.Open()

        let sql =
            "INSERT INTO Tasks (Description, DueDate, Priority) VALUES (@Description, @DueDate, @Priority)"

        connection.Execute(sql, task) |> ignore

    // update a task in the database
    let updatetaskindb (task: Task) =
        let connectionString =
            "Data Source=localhost;Initial Catalog=Scheduler;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"
    
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
        let connectionString =
            "Data Source=localhost;Initial Catalog=Scheduler;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"
        use connection = new SqlConnection(connectionString)
        connection.Open()
        let sql = "delete from tasks where Task_ID = @taskid"
        connection.Execute(sql, box {| taskid = taskid |}) |> ignore
