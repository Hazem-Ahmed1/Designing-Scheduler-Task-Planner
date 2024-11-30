module TaskInfo

type Status =
    | Pending = 0
    | Completed = 1
    | Overdue = -1

type Task = {
    TaskID: int
    Description: string
    DueDate: System.DateTime
    Priority: int
    Status: string
    CreatedAt: System.DateTime
}


