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

let printTask t =
    printfn "%d: %s - %A - %A" t.TaskId t.Description t.DueDate t.Status

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
