module FilterUsingQuery

open System
open Microsoft.Data.SqlClient
open Config
open TaskInfo
open System.Windows.Forms
open System.Drawing

let filterTasks status priority dueDate =
     try
        let connectionString = GetDataBaseConnection("ConstrAbdelrahman")
        let query =
                "SELECT *
                 FROM Tasks
                 WHERE (@Status IS NULL OR Status = @Status)
                 AND  (@Priority IS NULL OR Priority = @Priority)
             AND (@DueDate IS NULL OR CONVERT(VARCHAR(10), DueDate, 120) = CONVERT(VARCHAR(10), @DueDate, 120));"
        use connection = new SqlConnection(connectionString)
        connection.Open()

        use command = new SqlCommand(query, connection)
        command.Parameters.AddWithValue("@Status", if String.IsNullOrWhiteSpace(status) then DBNull.Value :> obj else status) |> ignore
        command.Parameters.AddWithValue("@Priority", if priority = 0 then DBNull.Value :> obj else priority) |> ignore
        command.Parameters.AddWithValue("@DueDate", if dueDate = DateTime.MinValue then DBNull.Value :> obj else dueDate) |> ignore

        use reader = command.ExecuteReader()
        Console.Clear()
        printfn "Filtered Tasks:"
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
      with | ex ->
        printfn "Error: %s" ex.Message
        []

let getUserInputAndFilterTasks () =
    
    printf "Enter status (or leave blank to skip): "
    let status = Console.ReadLine()

    printf "Enter priority (Enter Valid Number or 0 to skip): "
    let priorityInput = Console.ReadLine()
    let priority = if String.IsNullOrWhiteSpace(priorityInput) then 0 else Int32.Parse(priorityInput)

    printf "Enter Specific due date (yyyy-MM-dd, or leave blank to skip): "
    let dueDateInput = Console.ReadLine()
    let dueDate =
        match DateTime.TryParse(dueDateInput) with
        | (true, parsedDate) -> parsedDate
        | (false, _) ->
            printfn "NO FILTER USING DATE"
            DateTime.MinValue


    // Filter and display tasks
    filterTasks status priority dueDate


// Create the Filter Task Form
let createFilterTaskForm () =
    let filterTaskForm = new Form(Text = "Filter Tasks", Width = 800, Height = 500)
    filterTaskForm.BackColor <- Color.White

    // Title Label
    let titleLabel = new Label(Text = "Filter Tasks", Font = new Font("Arial", 16.0f, FontStyle.Bold),
                               AutoSize = true, ForeColor = Color.RoyalBlue, Location = Point(320, 20))

    // Status
    let statusLabel = new Label(Text = "Status (Pending, Completed, Overdue):", Location = Point(30, 70), AutoSize = true)
    let statusTextBox = new TextBox(Width = 200, Location = Point(300, 65))

    // Priority
    let priorityLabel = new Label(Text = "Priority (Enter a number or leave blank):", Location = Point(30, 110), AutoSize = true)
    let priorityTextBox = new TextBox(Width = 200, Location = Point(300, 105))

    // Due Date
    let dateLabel = new Label(Text = "Due Date (yyyy-MM-dd or leave blank):", Location = Point(30, 150), AutoSize = true)
    let dateTextBox = new TextBox(Width = 200, Location = Point(300, 145))

    // Filter Button
    let filterButton = new Button(Text = "Filter", BackColor = Color.RoyalBlue, ForeColor = Color.White,
                                  Font = new Font("Arial", 12.0f, FontStyle.Bold),
                                  Width = 100, Height = 40, Location = Point(350, 190))

    // DataGridView to display filtered tasks
    let taskGridView = new DataGridView(Width = 733, Height = 200, Location = Point(30, 250))
    taskGridView.ColumnHeadersHeightSizeMode <- DataGridViewColumnHeadersHeightSizeMode.AutoSize
    taskGridView.AllowUserToAddRows <- false
    taskGridView.AllowUserToDeleteRows <- false
    taskGridView.ReadOnly <- true
    taskGridView.RowHeadersVisible <- false

    // Define columns
    taskGridView.Columns.Add("Id", "ID") |> ignore
    taskGridView.Columns.Add("Description", "Description") |> ignore
    taskGridView.Columns.Add("DueDate", "Due Date") |> ignore
    taskGridView.Columns.Add("Priority", "Priority") |> ignore
    taskGridView.Columns.Add("CreatedAt", "Created At") |> ignore
    taskGridView.Columns.Add("Status", "Status") |> ignore

    taskGridView.Columns.[0].Width <- 50    // Id column
    taskGridView.Columns.[1].Width <- 300   // Description column
    taskGridView.Columns.[3].Width <- 80    //Priority column

    // Event handler for Filter button
    filterButton.Click.Add(fun _ ->
        try
            let status = 
                if String.IsNullOrWhiteSpace(statusTextBox.Text) then null else statusTextBox.Text
            let priority =
                if String.IsNullOrWhiteSpace(priorityTextBox.Text) then 0 else Int32.Parse(priorityTextBox.Text)
            let dueDate =
                match DateTime.TryParse(dateTextBox.Text) with
                | (true, parsedDate) -> parsedDate
                | (false, _) -> DateTime.MinValue

            // Call the filtering function
            let filteredTasks = filterTasks status priority dueDate

            if filteredTasks.Length > 0 then
                // Clear previous rows
                taskGridView.Rows.Clear()

                // Add filtered tasks to the DataGridView
                filteredTasks |> List.iter (fun task ->
                    taskGridView.Rows.Add([| task.TaskID.ToString()
                                             task.Description
                                             task.DueDate.ToString("yyyy/MM/dd")
                                             task.Priority.ToString()
                                             task.CreatedAt.ToString("yyyy/MM/dd")
                                             task.Status |]) |> ignore
                )
            else
                MessageBox.Show("No tasks match the filter criteria.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        with
        | ex -> MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
    )

    // Add controls to the form
    filterTaskForm.Controls.AddRange([| titleLabel; statusLabel; statusTextBox; priorityLabel; priorityTextBox;
                                        dateLabel; dateTextBox; filterButton; taskGridView |])

    filterTaskForm

