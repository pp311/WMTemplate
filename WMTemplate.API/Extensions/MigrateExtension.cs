using Microsoft.EntityFrameworkCore;
using WMTemplate.Infrastructure.Data;

namespace WMTemplate.Extensions;

public static class MigrateExtension
{
    public static async Task MigrateDatabaseAsync(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var dbContext = services.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    } 
}
