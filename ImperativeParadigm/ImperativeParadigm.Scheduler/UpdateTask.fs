module UpdateTask

open System
open Microsoft.Data.SqlClient
open Config
open System.Threading
open System.Windows.Forms
open System.Drawing

let updateTask taskId newDescription newDueDate newPriority newStatus =
     try
        let connectionString = GetDataBaseConnection("ConstrAbdelwahed")

        let query = "UPDATE Tasks
                 SET Description = @Description,
                     DueDate = @DueDate,
                     Priority = @Priority,
                     Status = @Status
                 WHERE Task_ID = @Task_ID"

        use connection = new SqlConnection(connectionString)
        connection.Open()

        use command = new SqlCommand(query, connection)
        command.Parameters.AddWithValue("@Task_ID", taskId) |> ignore
        command.Parameters.AddWithValue("@Description", newDescription) |> ignore
        command.Parameters.AddWithValue("@DueDate", newDueDate) |> ignore
        command.Parameters.AddWithValue("@Priority", newPriority) |> ignore
        command.Parameters.AddWithValue("@Status", newStatus) |> ignore

        let rowsAffected = command.ExecuteNonQuery()
        printfn "%d row(s) updated." rowsAffected
        Thread.Sleep(1500)
        Console.Clear()

     with
        | ex -> printfn "Error: %s" ex.Message


let getUserInputAndUpdateTask () =
    Console.Clear()

    printf "Enter Task ID to update: "
    let taskIdInput = Console.ReadLine()
    let taskId = Int32.Parse(taskIdInput)

    printf "Enter new task description: "
    let newDescription = Console.ReadLine()

    printf "Enter new due date (yyyy-MM-dd HH:mm:ss): "
    let newDueDateInput = Console.ReadLine()
    let newDueDate = DateTime.Parse(newDueDateInput)

    printf "Enter new priority (Low numbers have highest priority): "
    let newPriorityInput = Console.ReadLine()
    let newPriority = Int32.Parse(newPriorityInput)

    printf "Enter new status (Pending, Completed, Overdue): "
    let newStatus = Console.ReadLine()

    // Update the task in the database
    updateTask taskId newDescription newDueDate newPriority newStatus

// Create the Update Task Form
let createUpdateTaskForm () =
    let updateTaskForm = new Form(Text = "Update Task", Width = 600, Height = 380)
    updateTaskForm.BackColor <- Color.White

    // Title Label
    let titleLabel = new Label(Text = "Update Task", Font = new Font("Arial", 16.0f, FontStyle.Bold),
                               AutoSize = true, ForeColor = Color.RoyalBlue, Location = Point(230, 20))

    // Task ID
    let idLabel = new Label(Text = "Enter Task ID to update: ", Location = Point(30, 70), AutoSize = true)
    let idTextBox = new TextBox(Width = 300, Location = Point(200, 65))

    // Task Description
    let taskLabel = new Label(Text = "Enter new task description:", Location = Point(30, 110), AutoSize = true)
    let taskTextBox = new TextBox(Width = 300, Location = Point(200, 105))

    // Due Date
    let dateLabel = new Label(Text = "Enter new due date: ", Location = Point(30, 150), AutoSize = true)
    let dateTextBox = new TextBox(Width = 300, Location = Point(200, 145))

    // Priority
    let priorityLabel = new Label(Text = "Enter new priority: ", Location = Point(30, 190), AutoSize = true)
    let priorityTextBox = new TextBox(Width = 300, Location = Point(200, 185))

    // Status
    let statusLabel = new Label(Text = "Select new status: ", Location = Point(30, 230), AutoSize = true)
    let statusComboBox = new ComboBox(Width = 300, Location = Point(200, 225))
    statusComboBox.Items.AddRange([| "Completed"; "Pending"; "Overdue" |]) |> ignore
    statusComboBox.DropDownStyle <- ComboBoxStyle.DropDownList

    // Save Button
    let saveButton = new Button(Text = "Save", BackColor = Color.RoyalBlue, ForeColor = Color.White,
                               Font = new Font("Arial", 12.0f, FontStyle.Bold),
                               Width = 100, Height = 40, Location = Point(250, 285))

    saveButton.Click.Add(fun _ ->
            try
                let id = Int32.Parse idTextBox.Text
                let description = taskTextBox.Text
                let dueDate = System.DateTime.Parse(dateTextBox.Text)
                let priority = Int32.Parse priorityTextBox.Text
                let status = statusComboBox.SelectedItem.ToString()
                updateTask id description dueDate priority status
                MessageBox.Show("Task Updated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)  |> ignore

                // Clear inputs
                idTextBox.Clear()
                taskTextBox.Clear()
                dateTextBox.Clear()
                priorityTextBox.Clear()
                statusComboBox.Items.Clear()

            with
            | ex -> MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )

    // Add Controls to the Update Task Form
    updateTaskForm.Controls.AddRange([| titleLabel; taskLabel; idLabel; idTextBox; taskTextBox; dateLabel; dateTextBox;
                                      priorityLabel; priorityTextBox; statusLabel; statusComboBox; saveButton |])

    updateTaskForm