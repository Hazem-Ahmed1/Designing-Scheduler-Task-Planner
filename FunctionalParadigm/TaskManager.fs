module TaskManager

open Task
open IOManager
open Utilities

module TaskManager =

    let addTask description dueDate priority status =
        { Description = description; DueDate = dueDate; Priority = priority; Status = status }

    let markCompleted taskId =
        let tasks = IOManager.loadTasks()
        match tasks |> List.tryFind (fun t -> t.TaskId = taskId) with
        | Some task ->
            let updatedTask = { task with Status = TaskStatus.Completed }
            IOManager.updatetaskindb updatedTask
            "Task marked as completed."
        | None ->
            "Task not found."
