module Task
open System

type TaskStatus = 
    | Pending
    | Completed
    | Overdue

type Task = {
    TaskId: int
    Description: string
    DueDate: System.DateTime
    Priority: int
    Status: TaskStatus
}
