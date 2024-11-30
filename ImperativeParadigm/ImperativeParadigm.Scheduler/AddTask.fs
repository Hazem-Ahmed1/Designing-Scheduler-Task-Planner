module AddTask
open System
open Microsoft.Data.SqlClient

let insertTask description dueDate priority status =
    let connectionString = "Data Source=DESKTOP-PAV8I18\SQLEXPRESS;Initial Catalog=Scheduler;Integrated Security=True;Encrypt=False;Trust Server Certificate=True";

    let query = "INSERT INTO Tasks (Description, DueDate, Priority, Status, CreatedAt) VALUES (@Description, @DueDate, @Priority, @Status, GETDATE())"

    use connection = new SqlConnection(connectionString)
    connection.Open()

    use command = new SqlCommand(query, connection)
    command.Parameters.AddWithValue("@Description", description) |> ignore
    command.Parameters.AddWithValue("@DueDate", dueDate) |> ignore
    command.Parameters.AddWithValue("@Priority", priority) |> ignore
    command.Parameters.AddWithValue("@Status", status) |> ignore

    let rowsAffected = command.ExecuteNonQuery()
    printfn "%d row(s) inserted." rowsAffected



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

    printf "Enter status (Pending, Completed, Overdue): "
    let status = Console.ReadLine()

    // Insert the task into the database
    insertTask description dueDate priority status

