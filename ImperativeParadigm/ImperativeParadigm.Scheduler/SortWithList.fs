module SortWithList

open System
open TaskInfo
open System.Windows.Forms
open System.Drawing

// Imperative sorting function
let sortTasksImperative (tasks: Task list) sortColumn =
    let mutable sortedTasks = tasks

    // Sort based on the specified column
    if not (String.IsNullOrWhiteSpace(sortColumn)) then
        match sortColumn with
        | "DueDate" ->
            sortedTasks <-
                List.sortBy (fun task -> task.DueDate) sortedTasks
        | "Priority" ->
            sortedTasks <-
                List.sortBy (fun task -> task.Priority) sortedTasks
        | "CreatedAt" ->
            sortedTasks <-
                List.sortBy (fun task -> task.CreatedAt) sortedTasks
        | _ ->
            printfn "Invalid sort column. No sorting applied."

    sortedTasks


// Function to take user input and sort tasks
let getUserInputAndSortTasksImperative () =
    // Retrieve tasks
    let tasks = RetriveTasks.showTasks()

    // Get sort criteria from the user
    printf "Enter the column to sort by (DueDate, Priority, CreatedAt): "
    let sortColumn = Console.ReadLine()

    let data = RetriveTasks.showTasks()

    // Sort tasks based on the user's choice
    sortTasksImperative tasks sortColumn


// Create the Sort Task Form
let createSortTaskForm () =
    let sortTaskForm = new Form(Text = "Sort Tasks", Width = 800, Height = 380)
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
    let taskGridView = new DataGridView(Width = 733, Height = 150, Location = Point(20, 150))
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
                let tasks = RetriveTasks.showTasks()
                let sortedTasks = sortTasksImperative tasks (sortColumn.ToString())
                if sortedTasks.Length > 0 then
                    // Clear previous rows
                    taskGridView.Rows.Clear()
                    
                    // Add sorted tasks to the DataGridView
                    sortedTasks |> List.iter (fun task ->
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

                else
                    MessageBox.Show("No tasks available to sort.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
        with
        | ex -> MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) |> ignore
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

    // Add Controls to the Sort Task Form
    sortTaskForm.Controls.AddRange([| titleLabel; sortLabel; sortComboBox; sortButton; taskGridView |])

    sortTaskForm