module PrintFormatter
open TaskInfo
let Print results = 
    printfn "ID\tDescription\t\t\t\tDueDate\t\tPriority\tStatus\t\tCreatedAt"
    printfn "  \t           \t\t\t\t       \t\t        \t       \t\t        "
    results |> List.iter (fun task ->
               printfn "%d\t%-15s\t\t%s\t%-10d\t%-10s\t%s\t"
                (task.TaskID)
                (task.Description.PadRight(30))
                (task.DueDate.ToString("yyyy/MM/dd"))
                (task.Priority)
                (task.Status.ToString().PadRight(10))
                (task.CreatedAt.ToString("yyyy/MM/dd")))
