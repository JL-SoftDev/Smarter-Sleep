using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Models;

public class SeedData
{
    public static void Initialize(postgresContext context)
    {
        context.Database.EnsureCreated();

        //No native support for upserting within EF Core 7.0, use seed_data.sql instead.
    }
}
