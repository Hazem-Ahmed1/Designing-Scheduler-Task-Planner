module Config

open Microsoft.Extensions.Configuration

let GetDataBaseConnection connectionDevice =
    try
        let configuration : IConfiguration =
            ConfigurationBuilder()
                .AddJsonFile("AppSettings.json")
                .Build()

        let connectionString = configuration.GetSection(connectionDevice).Value

        connectionString

    with
        | ex -> ex.Message


