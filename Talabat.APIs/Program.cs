using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.APIs.Extensions;
using Talabat.APIs.Middlewares;
using Talabat.Core.Models.Identity;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services and Add Services to the Container

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ProductContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var configuration = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(configuration);
            });

            builder.Services.AddApplicationServices();
            builder.Services.AddIdentityServices(builder.Configuration);

            #endregion

            var app = builder.Build();

            #region Update Database Migrations

            // dependency injection scope to run the migration
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var logger = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var DbContext = Services.GetRequiredService<ProductContext>();
                var identityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                // update the database to the latest migration
                await DbContext.Database.MigrateAsync();
                await identityDbContext.Database.MigrateAsync();

                logger.CreateLogger<Program>().LogInformation("Migrations Applied Successfully");
                // Seed dummy Data to database
                await ProductContextSeed.SeedAsync(DbContext);
                await AppIdentityContextSeed.SeedAsync(Services.GetRequiredService<UserManager<AppUser>>());
            }
            catch (Exception ex)
            {
                logger.CreateLogger<Program>().LogError(ex, "An error occurred while applying migrations");
            }
            #endregion


            #region Configure the HTTP request pipeline.   

            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseSwaggerMiddleware();
            }
            app.UseStatusCodePagesWithRedirects("/errors/{0}");

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();


            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
