## Compilation

- Uses `.NET 7.0 Framework`, download latest SDK [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- `Release` config is targetted for Linux Arm64, `Debug` config targets Windows x64. Other architecture may be specified within the command line.

The command below will compile the project into a executable program.
```bash
dotnet publish -c Release -o <output_folder>
```
**Note:** Use Debug instead of Release to run on Windows x64 runtime.

## Execution

1. **Set Enviroment Variables**
    - **ASPNETCORE_ENVIRONMENT:** Specifies which enviroment the application runs in using linked `appsettings.ASPNETCORE_ENVIRONMENT.json`. Options include *Development*, *Staging*, and *Production*.
    - **ASPNETCORE_URLS:** Determines URLs the application will listen on. 

    You can set these via the commands:
    ```bash
    export ASPNETCORE_ENVIRONMENT=Development
    export ASPNETCORE_URLS="http://localhost:80;https://localhost:443"
    ```
    **Note:** Use `set` instead of `export` on Windows.

2. **Configure Database Connection**
    In the associated appsettings.json, set a `ConnectionString` for `local` to the database you are connecting. Example:
    ```json
    "ConnectionStrings": {
        "local": "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=[password]"
    }
    ```

3. **Enable HTTPS**
    For local testing, you can enable HTTPS using the included dev-certs tool in .NET Core.
    ```bash
    dotnet dev-certs https --trust
    ```

4. **Execute the program**
    Use dotnet to build and run the program or run the executable made using publish above.
    ```bash
    dotnet run
    ```
    The API for the linked database can then be accessed using the specified URLs. (e.g. `https://localhost:443`). While running in a **Development** enviroment you can view the API at /swagger/index. (e.g. `https://localhost:443/swagger/index`)


## Dependencies

- Microsoft.AspNetCore.OpenApi (Version 7.0.10)
- Microsoft.EntityFrameworkCore.Tools (Version 7.0.10)
- Microsoft.VisualStudio.Web.CodeGeneration.Design (Version 7.0.9)
- Npgsql.EntityFrameworkCore.PostgreSQL (Version 7.0.4)
- Swashbuckle.AspNetCore (Version 6.5.0)