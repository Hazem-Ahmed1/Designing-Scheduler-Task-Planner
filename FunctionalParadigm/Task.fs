module Task

type TaskStatus =
    | Pending
    | Completed
    | Overdue

type Task =
    { TaskId: int
      Description: string
      DueDate: System.DateTime
      Priority: int
      CreatedAt: System.DateTime
      Status: TaskStatus }

type TaskDTO =
    { Description: string
      DueDate: System.DateTime
      Priority: int
      Status: TaskStatus }

let bindTask taskId description dueDate priority createdAt status =
    { TaskId = taskId
      Description = description
      DueDate = dueDate
      Priority = priority
      CreatedAt = createdAt
      Status = status }

let bindTaskDTO description dueDate priority status =
    { Description = description
      DueDate = dueDate
      Priority = priority
      Status = status }

let stringToStatus (statusStr: string) : TaskStatus =
    match statusStr with
    | "Pending" -> Pending
    | "Completed" -> Completed
    | "Overdue" -> Overdue
    | _ -> failwith $"Unknown status: {statusStr}"

let statusToString (statusStr: TaskStatus) : string =
    match statusStr with
    | Pending -> "Pending"
    | Completed -> "Completed"
    | Overdue -> "Overdue"

let printTask (t: Task) =
    printfn
        "| %-2d | %-46s | %-10s | %-8d | %-11s | %-10s |"
        (t.TaskId)
        (t.Description.PadRight(30))
        (t.DueDate.ToString("yyyy/MM/dd"))
        (t.Priority)
        ((statusToString t.Status).PadRight(8))
        (t.CreatedAt.ToString("yyyy/MM/dd"))

let printTasks lst f =
    printfn "+----+------------------------------------------------+------------+----------+-------------+------------+"

    printfn "| ID | Description                                    | Due Date   | Priority | Status      | Created At |"

    printfn "+----+------------------------------------------------+------------+----------+-------------+------------+"

    f lst printTask

    printfn "+----+------------------------------------------------+------------+----------+-------------+------------+"

    printfn "\nPress ESC to go back."

    let rec waitForEsc () =
        let key = System.Console.ReadKey(true).Key

        if key = System.ConsoleKey.Escape then
            System.Console.Clear()
        else
            waitForEsc ()

    waitForEsc ()

let compareByPriority (task1: Task) (task2: Task) = task1.Priority <= task2.Priority

let compareByDueDate (task1: Task) (task2: Task) = task1.DueDate <= task2.DueDate

let compareByCreatedTime (task1: Task) (task2: Task) = task1.CreatedAt >= task2.CreatedAt

let filterByStatus (task: Task) cond = task.Status = cond

let filterByPriority (task: Task) cond = task.Priority = cond

let filterByDueDate (task: Task) (cond: System.DateTime) = task.DueDate.Date = cond.Date
