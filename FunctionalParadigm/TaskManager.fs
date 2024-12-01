module TaskManager
open Task
open Utilities
module TaskManager =

    // Add a new task
    let addTask description dueDate priority =
        { TaskId = 0; Description = description; DueDate = dueDate; Priority = priority; Status = Pending }

    // Mark task as completed
    let markCompleted task =
        { task with Status = Completed }

    // Delete a task (we'll just filter it out for this example)
    let deleteTask taskId tasks =
        tasks |> List.filter (fun t -> t.TaskId <> taskId)

    // Update task priority
    let updatePriority task newPriority =
        { task with Priority = newPriority }

    // Get overdue tasks
    let getOverdueTasks tasks =
        tasks |> Utilities.filter (fun t -> t.Status = Pending && t.DueDate < System.DateTime.Now)

    // Highlight tasks nearing deadlines (within a day)
    let getNearingDeadlineTasks tasks =
        tasks |> Utilities.filter (fun t -> t.Status = Pending && t.DueDate - System.DateTime.Now <= System.TimeSpan(1, 0, 0, 0))
