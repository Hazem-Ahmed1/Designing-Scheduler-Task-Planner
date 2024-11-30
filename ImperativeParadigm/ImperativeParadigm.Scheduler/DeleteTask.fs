module DeleteTask

open System
open Microsoft.Data.SqlClient

let deleteTask taskId =
    let connectionString = "Data Source=DESKTOP-PAV8I18\SQLEXPRESS;Initial Catalog=Scheduler;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";

    let query = "DELETE FROM Tasks WHERE Task_ID = @Task_ID"

    use connection = new SqlConnection(connectionString)
    connection.Open()

    use command = new SqlCommand(query, connection)
    command.Parameters.AddWithValue("@Task_ID", taskId) |> ignore

    let rowsAffected = command.ExecuteNonQuery()
    if rowsAffected > 0 then
        printfn "Task with ID %d deleted successfully." taskId
    else
        printfn "No task with ID %d found." taskId

let getUserInputAndDeleteTask () =
    Console.Clear()
    printf "Enter Task ID to delete: "
    let taskIdInput = Console.ReadLine()

    match Int32.TryParse(taskIdInput) with
    | (true, taskId) -> deleteTask taskId
    | (false, _) -> printfn "Invalid Task ID. Please enter a valid integer."
