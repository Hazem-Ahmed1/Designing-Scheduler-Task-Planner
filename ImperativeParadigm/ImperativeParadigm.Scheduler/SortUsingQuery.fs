module SortUsingQuery

open System
open Microsoft.Data.SqlClient
open Config
open TaskInfo
open System.Windows.Forms
open System.Drawing

let SortTasks sortColumn =
    try
        let connectionString = GetDataBaseConnection("ConstrAbdelrahman")
        let query =
                    "SELECT *
                     FROM Tasks
                     ORDER BY
                        CASE @SortColumn
                            WHEN 'DueDate' THEN DueDate
                            WHEN 'Priority' THEN Priority
                            WHEN 'CreatedAt' THEN CreatedAt
                        END;"

        use connection = new SqlConnection(connectionString)
        connection.Open()

        use command = new SqlCommand(query, connection)

        command.Parameters.AddWithValue("@SortColumn", sortColumn) |> ignore

        use reader = command.ExecuteReader()
        Console.Clear()
        printfn "Sorted Tasks:"
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
    with
    | ex ->
        printfn "Error: %s" ex.Message
        []


let getUserInputAndSortTasks () =


    printf "Enter the column to sort by (DueDate, Priority, CreatedAt): "
    let sortColumn = Console.ReadLine()

    // Sort and display tasks
    SortTasks sortColumn


// Create the Sort Task Form
let createSortTaskForm () =
    let sortTaskForm = new Form(Text = "Sort Tasks", Width = 800, Height = 400)
    sortTaskForm.BackColor <- Color.White

    // Title Label
    let titleLabel = new Label(Text = "Sort Tasks", Font = new Font("Arial", 16.0f, FontStyle.Bold),
                               AutoSize = true, ForeColor = Color.RoyalBlue, Location = Point(330, 20))

    // Sort Column Label and Dropdown
    let sortLabel = new Label(Text = "Select the column to sort by:", Location = Point(130, 70), AutoSize = true)
    let sortComboBox = new ComboBox(Width = 200, Location = Point(300, 65))
    sortComboBox.Items.AddRange([| "DueDate"; "Priority"; "CreatedAt" |]) |> ignore
    sortComboBox.DropDownStyle <- ComboBoxStyle.DropDownList

    // Sort Button
    let sortButton = new Button(Text = "Sort", BackColor = Color.RoyalBlue, ForeColor = Color.White,
                               Font = new Font("Arial", 12.0f, FontStyle.Bold),
                               Width = 100, Height = 40, Location = Point(550, 60))

    // DataGridView to display sorted tasks
    let taskGridView = new DataGridView(Width = 733, Height = 180, Location = Point(20, 150))
    taskGridView.ColumnHeadersHeightSizeMode <- DataGridViewColumnHeadersHeightSizeMode.AutoSize
    taskGridView.AllowUserToAddRows <- false
    taskGridView.AllowUserToDeleteRows <- false
    taskGridView.ReadOnly <- true
    taskGridView.RowHeadersVisible <- false

    // Define columns
    taskGridView.Columns.Add("Id", "Id") |> ignore
    taskGridView.Columns.Add("Description", "Description") |> ignore
    taskGridView.Columns.Add("DueDate", "Due Date") |> ignore
    taskGridView.Columns.Add("Priority", "Priority") |> ignore
    taskGridView.Columns.Add("CreatedAt", "Created At") |> ignore
    taskGridView.Columns.Add("Status", "Status") |> ignore

    taskGridView.Columns.[0].Width <- 50    // Id column
    taskGridView.Columns.[1].Width <- 300   // Description column
    taskGridView.Columns.[3].Width <- 80    //Priority column

    // Event handler for Sort button
    sortButton.Click.Add(fun _ ->
        try
            let sortColumn = sortComboBox.SelectedItem
            if sortColumn = null then
                MessageBox.Show("Please select a column to sort by!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) |> ignore
            else
                let sortedTasks = SortTasks(sortColumn.ToString())
                if sortedTasks.Length > 0 then
                    // Clear previous rows
                    taskGridView.Rows.Clear()
                    
                    // Add sorted tasks to the DataGridView
                    sortedTasks |> List.iter (fun task ->
                        taskGridView.Rows.Add([| task.TaskID.ToString()
                                                 task.Description
                                                 task.DueDate.ToString("yyyy/MM/dd")
                                                 task.Priority.ToString()
                                                 task.CreatedAt.ToString("yyyy/MM/dd")
                                                 task.Status |]) |> ignore
                    )
                else
                    MessageBox.Show("No tasks available to sort.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        with
        | ex -> MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
    )

    // Add Controls to the Sort Task Form
    sortTaskForm.Controls.AddRange([| titleLabel; sortLabel; sortComboBox; sortButton; taskGridView |])

    sortTaskForm
