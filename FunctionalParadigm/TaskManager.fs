module TaskManager

open Task
open IOManager
open Utilities

module TaskManager =



    let markCompleted taskId =
        let tasks = IOManager.loadTasks ()

        match tasks |> List.tryFind (fun t -> t.TaskId = taskId) with
        | Some task ->
            let updatedTask =
                { task with
                    Status = TaskStatus.Completed }

            IOManager.updatetaskindb updatedTask
            "Task marked as completed."
        | None -> "Task not found."

    let rec merge (left: Task list) (right: Task list) : Task list =
        match left, right with
        | [], ys -> ys
        | xs, [] -> xs
        | x :: xs, y :: ys when x.Priority <= y.Priority -> x :: merge xs right
        | x :: xs, y :: ys -> y :: merge left ys


    let rec mergeSort (tasks : Task List) = 
        match tasks with
        |[] | [_] -> tasks
        | _ ->
            let mutable middle = len2 tasks 0
            middle <- middle / 2
            let left = tasks |> List.take middle
            let right = tasks |> List.skip middle
            merge( mergeSort left ) ( mergeSort right)