module RetriveTasks

open TaskInfo
open Config
open Microsoft.Data.SqlClient
open System.Windows.Forms
open System.Drawing
open System

let showTasks() =

    //System.Console.Clear()
    try
        let connectionString = GetDataBaseConnection("ConstrAbdelrahman")
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
                        CreatedAt = reader.GetDateTime(4)
                        Status = if (DateTime.Now.Date.CompareTo(reader.GetDateTime(2)) < 0 || reader.GetString(5) = "Completed") then reader.GetString(5) else "Overdue"
                    }
                 }
            rows |> List.ofSeq
        results
    with | ex ->
        printfn "Error: %s" ex.Message
        []

// Create Display Tasks Form
let createDisplayForm () =
    let displayForm = new Form(Text = "Display Task", Width = 800, Height = 360)
    displayForm.BackColor <- Color.White

    // Title Label
    let titleLabel = new Label(Text = "Display all Tasks", Font = new Font("Arial", 16.0f, FontStyle.Bold),
                               AutoSize = true, ForeColor = Color.RoyalBlue, Location = Point(300, 20))
    //Indicator Labels
    let CompletedTasksLabel = new Label(Text = "Completed Tasks",AutoSize = true, BackColor = Color.LightGreen,
                               ForeColor = Color.Black, Location = Point(650, 40))

    let OverdueTasksLabel = new Label(Text = "Overdue Tasks",AutoSize = true, BackColor = Color.LightCoral,
                               ForeColor = Color.Black, Location = Point(650, 60))

    let NearingDeadlineLabel = new Label(Text = "Nearing Deadline Tasks", AutoSize = true,BackColor = Color.LightYellow,
                               ForeColor = Color.Black, Location = Point(650, 80))

    let OnTimeTasksLabel = new Label(Text = "On-time Tasks",AutoSize = true, BackColor = Color.White,
                              ForeColor = Color.Black, Location = Point(650, 100))

    // DataGridView to display tasks
    let taskGridView = new DataGridView(Width = 733, Height = 150, Location = Point(20, 130))
    taskGridView.ColumnHeadersHeightSizeMode <- DataGridViewColumnHeadersHeightSizeMode.AutoSize
    taskGridView.AllowUserToAddRows <- false
    taskGridView.AllowUserToDeleteRows <- false
    taskGridView.ReadOnly <- true
    taskGridView.RowHeadersVisible <- false

    // Define columns
    taskGridView.Columns.Clear()
    taskGridView.Columns.Add("TaskID", "TaskID") |> ignore
    taskGridView.Columns.Add("Description", "Description") |> ignore
    taskGridView.Columns.Add("DueDate", "Due Date") |> ignore
    taskGridView.Columns.Add("Priority", "Priority") |> ignore
    taskGridView.Columns.Add("CreatedAt", "Created At") |> ignore
    taskGridView.Columns.Add("Status", "Status") |> ignore

    taskGridView.Columns.[0].Width <- 50    // Id column
    taskGridView.Columns.[1].Width <- 300   // Description column
    taskGridView.Columns.[3].Width <- 80    //Priority column

    // Load tasks
    let tasks = showTasks()
    tasks |> List.iter (fun task ->
        // Explicitly box all properties before adding to the grid
        let rowValues =
            [|
                box task.TaskID
                box task.Description
                box (task.DueDate.ToString("yyyy/MM/dd"))
                box task.Priority
                box (task.CreatedAt.ToString("yyyy/MM/dd"))
                box task.Status
            |]
        taskGridView.Rows.Add(rowValues) |> ignore
    )

    // Event handler for CellFormatting to change row colors
    taskGridView.CellFormatting.Add(fun args ->
        if args.RowIndex >= 0 && args.RowIndex < taskGridView.Rows.Count then
            let row = taskGridView.Rows.[args.RowIndex]
            let status =
                match row.Cells.[5].Value with
                | null -> ""
                | value -> value.ToString()
            let dueDate =
                match row.Cells.[2].Value with
                | null -> DateTime.MaxValue // Assign a maximum date if DueDate is null
                | value -> DateTime.Parse(value.ToString())
            let today = DateTime.Now.Date
            let nearingDeadlineThreshold = today.AddDays(3.0)

            // Change row background color based on conditions
            row.DefaultCellStyle.BackColor <-
                if status = "Completed" then Color.LightGreen
                elif dueDate < today then Color.LightCoral
                elif dueDate <= nearingDeadlineThreshold then Color.LightYellow
                else Color.White
    )


    // Add Controls to the Display Tasks Form
    displayForm.Controls.AddRange([| titleLabel; taskGridView; CompletedTasksLabel; OverdueTasksLabel;
    NearingDeadlineLabel; OnTimeTasksLabel|])

    displayForm