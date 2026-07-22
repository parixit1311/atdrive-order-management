using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace OrderManagement.Web.Data;

/// <summary>
/// Sets the database up from Database/AtDriveOrders.sql — the script is the
/// single source of truth for the schema, stored procedure, and seed data.
/// </summary>
public static class DbInitializer
{
    public static void Initialize(OrderDbContext context)
    {
        // Create the (empty) database if it doesn't exist yet; the script
        // then creates all objects and seed data inside it.
        var databaseCreator = context.GetService<IRelationalDatabaseCreator>();
        if (!databaseCreator.Exists())
        {
            databaseCreator.Create();
        }

        var scriptPath = Path.Combine(AppContext.BaseDirectory, "Database", "AtDriveOrders.sql");
        var script = File.ReadAllText(scriptPath);

        // "GO" is a batch separator understood by SSMS/sqlcmd, not by SQL
        // Server itself, so the script is executed one batch at a time.
        var batches = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);

        foreach (var batch in batches)
        {
            if (!string.IsNullOrWhiteSpace(batch))
            {
                context.Database.ExecuteSqlRaw(batch);
            }
        }
    }
}
