module IOManager

open Microsoft.Data.SqlClient
open Task
open Dapper

module IOManager =

    let connectionString = "Data Source=.;Initial Catalog=Scheduler;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";

    let loadTasks () =
        use connection = new SqlConnection(connectionString)
        connection.Open()
        connection.Query<Task>("SELECT * FROM Tasks")
        |> Seq.toList 

    // Add a task to the database
    let addTaskToDb task =
        use connection = new SqlConnection(connectionString)
        connection.Open()
        let sql = "INSERT INTO Tasks (Description, DueDate, Priority, Status) VALUES (@Description, @DueDate, @Priority, @Status)"
        connection.Execute(sql, task) |> ignore

    // Update a task in the database
    let updateTaskInDb task =
        use connection = new SqlConnection(connectionString)
        connection.Open()
        let sql = "UPDATE Tasks SET Description = @Description, DueDate = @DueDate, Priority = @Priority, Status = @Status WHERE TaskId = @TaskId"
        connection.Execute(sql, task) |> ignore


    let deleteTaskFromDb taskId =
        use connection = new SqlConnection(connectionString)
        connection.Open()
        let sql = "DELETE FROM Tasks WHERE TaskId = @TaskId"
        connection.Execute(sql, box {| TaskId = taskId |}) |> ignore



