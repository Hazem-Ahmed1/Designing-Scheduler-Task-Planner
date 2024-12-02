module DeleteTask

open System
open Microsoft.Data.SqlClient
open Config
open System.Threading
open System.Windows.Forms
open System.Drawing

let deleteTask taskId =
   try
        let connectionString = GetDataBaseConnection("ConstrAbdelrahman")

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
        Thread.Sleep(1300)
        Console.Clear()
    with
        | ex -> printfn "Error: %s" ex.Message

let getUserInputAndDeleteTask () =
    Console.Clear()
    printf "Enter Task ID to delete: "
    let taskIdInput = Console.ReadLine()

    match Int32.TryParse(taskIdInput) with
    | (true, taskId) -> deleteTask taskId
    | (false, _) -> printfn "Invalid Task ID. Please enter a valid integer."

// Create the Delete Task Form
let createDeleteTaskForm () =
    let deleteTaskForm = new Form(Text = "Delete Task", Width = 350, Height = 200)
    deleteTaskForm.BackColor <- Color.White

    // Title Label
    let titleLabel = new Label(Text = "Delete Task", Font = new Font("Arial", 16.0f, FontStyle.Bold),
                               AutoSize = true, ForeColor = Color.RoyalBlue, Location = Point(110, 20))

    // Task Id
    let idLabel = new Label(Text = "Enter task id:", Location = Point(30, 70), AutoSize = true)
    let idTextBox = new TextBox(Width = 150, Location = Point(130, 65))

    // Add Button
    let deleteButton = new Button(Text = "Delete", BackColor = Color.RoyalBlue, ForeColor = Color.White,
                               Font = new Font("Arial", 12.0f, FontStyle.Bold),
                               Width = 100, Height = 40, Location = Point(110, 105))

    deleteButton.Click.Add(fun _ ->
            try
                let id = Int32.Parse idTextBox.Text
                deleteTask id 
                MessageBox.Show("Task Deleted Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)  |> ignore

                // Clear inputs
                idTextBox.Clear()

            with
            | ex -> MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )

    // Add Controls to the Delete Task Form
    deleteTaskForm.Controls.AddRange([| titleLabel; idLabel; idTextBox; deleteButton |])

    deleteTaskForm