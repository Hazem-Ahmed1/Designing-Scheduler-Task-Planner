module Config

open Microsoft.Extensions.Configuration

let GetDataBaseConnection connectionDevice = 
    try
        let configuration : IConfiguration =
            ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .Build()

        // Example of accessing a configuration value
        let connectionString = configuration.GetSection(connectionDevice).Value
        //printfn "Connection String: %s" connectionString
        connectionString

    with
        | ex -> ex.Message


