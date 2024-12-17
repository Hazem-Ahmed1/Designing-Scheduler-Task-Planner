module AddTask
open System
open Microsoft.Data.SqlClient
open Config
open System.Threading
open System.Windows.Forms
open System.Drawing

let insertTask description dueDate priority  =
    try
        let connectionString = GetDataBaseConnection("ConstrAbdelwahed")

        let query = "INSERT INTO Tasks ([Description], DueDate, [Priority]) VALUES (@Description, @DueDate, @Priority);"

        use connection = new SqlConnection(connectionString)
        connection.Open()

        use command = new SqlCommand(query, connection)
        command.Parameters.AddWithValue("@Description", description) |> ignore
        command.Parameters.AddWithValue("@DueDate", dueDate) |> ignore
        command.Parameters.AddWithValue("@Priority", priority) |> ignore

        let rowsAffected = command.ExecuteNonQuery()
        printfn "%d row(s) inserted." rowsAffected
        Thread.Sleep(2500)
        Console.Clear()
     with
        | ex -> printfn "Error: %s" ex.Message


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

    let sts = "Pending"
    printf "\nAll The Tasks Inserted is Marked as "
    Console.ForegroundColor <- System.ConsoleColor.Green
    printfn "%s initially" sts
    Console.ResetColor()


    // Insert the task into the database
    insertTask description dueDate priority

// Create the Add Task Form
let createAddTaskForm () =
    let addTaskForm = new Form(Text = "Add Task", Width = 600, Height = 300)
    addTaskForm.BackColor <- Color.White

    // Title Label
    let titleLabel = new Label(Text = "Add New Task", Font = new Font("Arial", 16.0f, FontStyle.Bold),
                               AutoSize = true, ForeColor = Color.RoyalBlue, Location = Point(220, 20))

    // Task Description
    let taskLabel = new Label(Text = "Enter task description:", Location = Point(30, 70), AutoSize = true)
    let taskTextBox = new TextBox(Width = 300, Location = Point(200, 65))

    // Due Date
    let dateLabel = new Label(Text = "Enter due date:", Location = Point(30, 110), AutoSize = true)
    let dateTextBox = new TextBox(Width = 300, Location = Point(200, 105))

    // Priority
    let priorityLabel = new Label(Text = "Enter priority:", Location = Point(30, 150), AutoSize = true)
    let priorityTextBox = new TextBox(Width = 300, Location = Point(200, 145))

    // Add Button
    let addButton = new Button(Text = "Add", BackColor = Color.RoyalBlue, ForeColor = Color.White,
                               Font = new Font("Arial", 12.0f, FontStyle.Bold),
                               Width = 100, Height = 40, Location = Point(250, 200))
    // Add Button Click Event
    addButton.Click.Add(fun _ ->
            try
                let description = taskTextBox.Text
                let dueDate = System.DateTime.Parse(dateTextBox.Text)
                let priority = Int32.Parse priorityTextBox.Text
                insertTask description dueDate priority
                MessageBox.Show("Task Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)  |> ignore

                // Clear inputs
                taskTextBox.Clear()
                dateTextBox.Clear()
                priorityTextBox.Clear()

            with
            | ex -> MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
        )

    // Add Controls to the Add Task Form
    addTaskForm.Controls.AddRange([| titleLabel; taskLabel; taskTextBox; dateLabel; dateTextBox;
                                      priorityLabel; priorityTextBox; addButton |])

    addTaskForm