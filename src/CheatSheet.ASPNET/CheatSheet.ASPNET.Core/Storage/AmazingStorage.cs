using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CheatSheet.ASPNET.Core.Storage;

public class AmazingStorage
{
    // Injection
    public string AllowInjection(string amazingId)
    {
        using var connection = new SqlConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM AmazingThings WHERE Id = " + amazingId;
        var reader = command.ExecuteReader();
        
        // Entity Framework
        var context = new AmazingContext();
        context.AmazingThings.FromSqlRaw(
            "SELECT * FROM AmazingThings WHERE Id = " + amazingId).FirstOrDefault();

        // Dapper
        var amazingThing = connection.Query(
            sql: "SELECT * FROM AmazingThings WHERE Id " + amazingId);

        return "What could go wrong?!?";
    }

    public string PreventInjection(string amazingId)
    {
        using var connection = new SqlConnection();
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM AmazingThings WHERE Id = @amazingId";
        command.Parameters.AddWithValue("@amazingId", amazingId);
        var reader = command.ExecuteReader();

        // Entity Framework
        var context = new AmazingContext();
        context.AmazingThings.FromSql(
            $"SELECT * FROM AmazingThings WHERE Id = {amazingId}").FirstOrDefault();

        // Dapper
        var amazingThing = connection.Query(
            sql: "SELECT * FROM AmazingThings WHERE Id = @amazingId",
            param: new { amazingId });

        return "Prepare to be amazed!";
    }
}

public class AmazingContext : DbContext
{
    public DbSet<AmazingThing> AmazingThings { get; set; }
}

public class AmazingThing
{
    public string? Id { get; set; }
    public string? AmazingInformation { get; set; }
}