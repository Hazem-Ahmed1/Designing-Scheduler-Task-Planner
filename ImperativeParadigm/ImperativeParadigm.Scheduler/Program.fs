open System
open RetriveTasks
open AddTask
open UpdateTask
open DeleteTask
open FilterUsingQuery
open PrintFormatter
open SortUsingQuery
open FilterWithList
open SortWithList
open System.Windows.Forms
open System.Drawing

[<EntryPoint; STAThread>]
let main argv =
    Application.EnableVisualStyles()
    Application.SetCompatibleTextRenderingDefault(false)

    // Create the main form
    let homeForm = new Form(Text = "Task Manager", Width = 400, Height = 530)
    
    // Create a reusable function to generate styled buttons
    let createButton (text: string) (x: int) (y: int) =
        new Button(Text = text, Left = x, Top = y, Width = 250, Height = 40,
                   Font = new Font("Arial", 12.0f, FontStyle.Bold), BackColor = Color.RoyalBlue, ForeColor = Color.White,
                   FlatStyle = FlatStyle.Flat)

    // Title Label
    let titleLabel = new Label(Text = "Task Planner", Font = new Font("Arial", 30.0f, FontStyle.Bold),
                               AutoSize = true, ForeColor = Color.RoyalBlue, Location = Point(65, 20))
    // Create buttons
    let displayTasksButton = createButton "Display Tasks" 75 100
    displayTasksButton.Click.Add(fun _ ->
        let displayTasksButton = createDisplayForm ()
        displayTasksButton.ShowDialog() |> ignore
    )

    let addTaskButton = createButton "Add Task" 75 160
    addTaskButton.Click.Add(fun _ ->
        let addTaskForm = createAddTaskForm ()
        addTaskForm.ShowDialog() |> ignore
    )

    let deleteTaskButton = createButton "Delete Task" 75 220
    deleteTaskButton.Click.Add(fun _ ->
        let deleteTaskForm = createDeleteTaskForm ()
        deleteTaskForm.ShowDialog() |> ignore
    )

    let updateTaskButton = createButton "Update Task" 75 280
    updateTaskButton.Click.Add(fun _ ->
        let updateTaskForm = createUpdateTaskForm ()
        updateTaskForm.ShowDialog() |> ignore
    )

    let sortButton = createButton "Sort" 75 340
    sortButton.Click.Add(fun _ ->
        let sortTaskForm = createSortTaskForm ()
        sortTaskForm.ShowDialog() |> ignore
    )

    let filterButton = createButton "Filter" 75 400
    filterButton.Click.Add(fun _ ->
        let filterTaskForm = createFilterTaskForm ()
        filterTaskForm.ShowDialog() |> ignore
    )
   

    // Add buttons to the form
    homeForm.Controls.AddRange([| titleLabel; displayTasksButton; addTaskButton; deleteTaskButton; updateTaskButton; sortButton; filterButton |])

    Application.Run(homeForm)
    0

    //use form = new HomeForm()
    //Application.Run(form)
//    let rec mainLoop () =
//        printf "1) Show Tasks
//2) Add Task
//3) Update Task
//4) Delete Task
//5) Filter Tasks (Priority, DueDate, Status)
//6) Sort Tasks (Priority, CreationTime, DueDate)
//Q) Quit\n\n"

//        printf "Please enter operation number (or Q to quit): "
//        let input = System.Console.ReadLine()

//        if input.Trim().ToUpper() = "Q" then
//            printfn "Exiting the program. Goodbye!"
//        else
//            match System.Int32.TryParse(input) with
//            | (true, number) ->
//                match number with
//                | 1 ->
//                    printfn "You chose: Show Tasks"
//                    Print(showTasks())
//                | 2 ->
//                    printfn "You chose: Add Task"
//                    getUserInputAndInsertTask()
//                | 3 ->
//                    printfn "You chose: Update Task"
//                    getUserInputAndUpdateTask()
//                | 4 ->
//                    printfn "You chose: Delete Task"
//                    getUserInputAndDeleteTask()
//                | 5 ->
//                    printfn "You chose: Filter Tasks"
//                    //Print(getUserInputAndFilterTasks())
//                    Print(getUserInputAndFilterTasksImpartive())
//                | 6 ->
//                    printfn "You chose: Sort Tasks"
//                    // Call the function to sort tasks
//                    //Print(getUserInputAndSortTasks()) // Define this function
//                    Print(getUserInputAndSortTasksImperative())
//                | _ ->
//                    printfn "Invalid option. Please choose a number between 1 and 6."
//            | (false, _) -> printfn "Invalid input. Please enter a valid number."

//            // Restart the loop
//            mainLoop()

//    // Start the main loop
//    mainLoop()
    0
