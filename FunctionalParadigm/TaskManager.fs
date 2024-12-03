module TaskManager

open System
open Task
open IOManager
open Utilities

module TaskManager =

    let rec merge (left: Task list) (right: Task list) (compare: Task -> Task -> bool) : Task list =
        match left, right with
        | [], ys -> ys
        | xs, [] -> xs
        | x :: xs, y :: _ when compare x y -> x :: merge xs right compare
        | _ :: _, y :: ys -> y :: merge left ys compare

    let rec mergeSort (tasks: Task list) (compare: Task -> Task -> bool) : Task list =
        match tasks with
        | []
        | [ _ ] -> tasks
        | _ ->
            let middle = List.length tasks / 2
            let left = tasks |> List.take middle
            let right = tasks |> List.skip middle
            merge (mergeSort left compare) (mergeSort right compare) compare

    let updateTaskStatus taskStatus (task: Task) = { task with Status = taskStatus }

    let updateTaskStatusById taskId taskStatus =
        let tasks = IOManager.loadTasks ()

        match tasks |> List.tryFind (fun t -> t.TaskId = taskId) with
        | Some task ->
            IOManager.updatetaskindb (updateTaskStatus taskStatus task)
            printfn "Task marked as completed."
        | None -> printfn "Task not found."

    let updatePriorityById taskId priority =
        let tasks = IOManager.loadTasks ()

        match tasks |> List.tryFind (fun t -> t.TaskId = taskId) with
        | Some task ->
            let updatedTask = { task with Priority = priority }

            IOManager.updatetaskindb updatedTask
            printfn "Task priority updated."
        | None -> printfn "Task not found."

    let addTask () =
        printfn "Add Task"

        try
            printf "Enter task description: "
            let description = Console.ReadLine()
            printf "Enter due date (YYYY-MM-DD), Default is +3 days: "
            let dueDate = dueDateGiven (Console.ReadLine())

            if validDueDate (dueDate) then
                printf "Enter priority (1-5), Lower is higher: "
                let priority = Int32.Parse(Console.ReadLine())

                if priority <= 5 && priority >= 1 then
                    let Status = Pending
                    let newTask = bindTaskDTO description dueDate priority Status
                    IOManager.addTaskToDb newTask
                    Console.Clear()
                    printfn "Task added successfully."
                else
                    Console.Clear()
                    printfn "Invalid priority."
            else
                Console.Clear()
                printfn "Invalid due date."
        with _ ->
            Console.Clear()
            printfn "Invalid Data."

    let viewTasks () =
        printfn "View Tasks"
        let tasks = IOManager.loadTasks ()

        printTasks tasks

    let updateTask () =
        printfn "Update Task"
        printf "Enter task ID to update: "
        let taskId = Int32.Parse(Console.ReadLine())

        printfn "1. Mark task as completed"
        printfn "2. Update task priority"
        printf "Choose an option: "

        match Console.ReadLine() with
        | "1" -> updateTaskStatusById taskId Completed
        | "2" ->
            printf "Enter updated priority (1-5): "
            let newPriority = Int32.Parse(Console.ReadLine())
            updatePriorityById taskId newPriority
        | _ ->
            printfn "Invalid option"
            Console.Clear()

    let deleteTask () =
        printfn "Delete Task"
        printf "Enter task ID to delete: "
        let taskId = Int32.Parse(Console.ReadLine())
        IOManager.deletetaskfromdb taskId
        Console.Clear()
        printfn "Task deleted successfully."

    let filterTasks () =
        printfn "Filter Tasks"
        printfn "Please select a column to filter by"
        printfn "1. Status"
        printfn "2. Priority"
        printfn "3. Due date"

        match Console.ReadLine() with
        | "1" ->
            Console.Clear()
            printfn "Choose status:"
            printfn "1. Pending"
            printfn "2. Completed"
            printfn "3. Overdue"

            match Console.ReadLine() with
            | "1" ->
                let tasks = IOManager.loadTasks ()
                let filteredList = filter2 tasks filterByStatus Pending
                printTasks filteredList
            | "2" ->
                let tasks = IOManager.loadTasks ()
                let filteredList = filter2 tasks filterByStatus Completed
                printTasks filteredList
            | "3" ->
                let tasks = IOManager.loadTasks ()
                let filteredList = filter2 tasks filterByStatus Overdue
                printTasks filteredList
            | _ ->
                Console.Clear()
                printfn "Invalid option."
        | "2" ->
            Console.Clear()
            printf "Enter priority (1-5): "

            let priority = Int32.Parse(Console.ReadLine())

            if priority > 5 || priority < 1 then
                Console.Clear()
                printfn "Invalid priority."
            else
                let tasks = IOManager.loadTasks ()
                let filteredList = filter2 tasks filterByPriority priority
                printTasks filteredList
        | "3" ->
            Console.Clear()
            printf "Enter Date (YYYY-MM-DD): "

            try
                let dueDate = DateTime.Parse(Console.ReadLine())
                let tasks = IOManager.loadTasks ()
                let filteredList = filter2 tasks filterByDueDate dueDate
                printTasks filteredList
            with _ ->
                Console.Clear()
                printfn "Invalid Date."
        | _ ->
            Console.Clear()
            printfn "Invalid option."

    let sortTasks () =
        printfn "Sort Tasks"
        printfn "Please select a column to sort by"
        printfn "1. Priority"
        printfn "2. Due date"
        printfn "3. Creation time"

        match Console.ReadLine() with
        | "1" ->
            let tasks = IOManager.loadTasks ()
            let sortedList = mergeSort tasks compareByPriority
            printTasks sortedList
        | "2" ->
            let tasks = IOManager.loadTasks ()
            let sortedList = mergeSort tasks compareByDueDate
            printTasks sortedList
        | "3" ->
            let tasks = IOManager.loadTasks ()
            let sortedList = mergeSort tasks compareByCreatedTime
            printTasks sortedList
        | _ ->
            Console.Clear()
            printfn "Invalid option."

    let checkOverdue () =
        let tasks = IOManager.loadTasks ()
        let filteredList = filter2 tasks filterByOverDueDate DateTime.Now
        let overdueList = map2 filteredList (updateTaskStatus Overdue)

        if len2 overdueList = 0 then
            printf "No Tasks nearing deadline"
        else
            printfn "Overdue tasks:"
            printTasks overdueList

    let closeDeadline () =
        let tasks = IOManager.loadTasks ()
        let filteredList = filter2 tasks filterByDeadline (DateTime.Now.AddDays(1))
        let pendingList = filter2 filteredList filterByStatus Pending

        if len2 pendingList = 0 then
            printfn "No Tasks nearing deadline"
        else
            printfn "Overdue In a Day..."
            printTasks pendingList
