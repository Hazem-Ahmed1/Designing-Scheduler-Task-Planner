module FilterWithList

open RetriveTasks
open TaskInfo
open System
open System.Windows.Forms
open System.Drawing

// Imperative filtering function
let filterTasksImperative (tasks: Task list) status priority dueDate =
    try
        let mutable filteredTasks = tasks

        if not (String.IsNullOrWhiteSpace(status)) then
            filteredTasks <-
                [ for task in filteredTasks do
                    if task.Status = status then yield task ]

        if priority <> 0 then
            filteredTasks <-
                [ for task in filteredTasks do
                    if task.Priority = priority then yield task ]

        if dueDate <> DateTime.MinValue then
            filteredTasks <-
                [ for task in filteredTasks do
                    if task.DueDate.Date = dueDate.Date then yield task ]

        filteredTasks

    with |ex -> printfn "Error: %s" ex.Message
                []


let getUserInputAndFilterTasksImpartive () =
    // Retrieve Tasks From Database
    let data = RetriveTasks.showTasks()

    printf "Enter status Pending, Completed or Overdue (or leave blank to skip): "
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
    filterTasksImperative data status priority dueDate

// Create the Filter Task Form
let createFilterTaskForm () =
    let filterTaskForm = new Form(Text = "Filter Tasks", Width = 800, Height = 450)
    filterTaskForm.BackColor <- Color.White

    // Title Label
    let titleLabel = new Label(Text = "Filter Tasks", Font = new Font("Arial", 16.0f, FontStyle.Bold),
                               AutoSize = true, ForeColor = Color.RoyalBlue, Location = Point(320, 20))

    // Status
    let statusLabel = new Label(Text = "Status: (Select a status or leave blank):", Location = Point(180, 70), AutoSize = true)
    let statusComboBox = new ComboBox(Width = 200, Location = Point(450, 65))
    statusComboBox.Items.AddRange([| "Completed"; "Pending"; "Overdue" |]) |> ignore
    statusComboBox.DropDownStyle <- ComboBoxStyle.DropDownList
    // Priority
    let priorityLabel = new Label(Text = "Priority (Enter a number or leave blank):", Location = Point(180, 110), AutoSize = true)
    let priorityTextBox = new TextBox(Width = 200, Location = Point(450, 105))

    // Due Date
    let dateLabel = new Label(Text = "Due Date (yyyy-MM-dd or leave blank):", Location = Point(180, 150), AutoSize = true)
    let dateTextBox = new TextBox(Width = 200, Location = Point(450, 145))

    // Filter Button
    let filterButton = new Button(Text = "Filter", BackColor = Color.RoyalBlue, ForeColor = Color.White,
                                  Font = new Font("Arial", 12.0f, FontStyle.Bold),
                                  Width = 100, Height = 40, Location = Point(350, 190))

    // DataGridView to display filtered tasks
    let taskGridView = new DataGridView(Width = 733, Height = 120, Location = Point(30, 250))
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
                if statusComboBox.SelectedItem = null  then "" else statusComboBox.SelectedItem.ToString()
            let priority =
                if String.IsNullOrWhiteSpace(priorityTextBox.Text) then 0 else Int32.Parse(priorityTextBox.Text)
            let dueDate =
                match DateTime.TryParse(dateTextBox.Text) with
                | (true, parsedDate) -> parsedDate
                | (false, _) -> DateTime.MinValue


            let data = RetriveTasks.showTasks()

            // Call the filtering function
            let filteredTasks = filterTasksImperative data status priority dueDate

            if filteredTasks.Length > 0 then
                // Clear previous rows
                taskGridView.Rows.Clear()

                // Add filtered tasks to the DataGridView
                filteredTasks |> List.iter (fun task ->
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
                MessageBox.Show("No tasks match the filter criteria.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information) |> ignore
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

    // Add controls to the form
    filterTaskForm.Controls.AddRange([| titleLabel; statusLabel; statusComboBox; priorityLabel; priorityTextBox;
                                        dateLabel; dateTextBox; filterButton; taskGridView |])

    filterTaskForm