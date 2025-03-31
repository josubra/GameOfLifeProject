
using GameOfLifeApi.Data;
using GameOfLifeApi.Data.Repository;
using GameOfLifeApi.Data.Repository.Abstraction;
using GameOfLifeApi.Exception;
using GameOfLifeApi.Service;
using GameOfLifeApi.Service.Abstractions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GameOfLifeApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            string mySqlConnectionStr = builder.Configuration.GetConnectionString("DefaultConnection")!;
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IGameOfLifeService), typeof(GameOfLifeService));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeature != null)
                    {
                        var exception = errorFeature.Error;
                        var endpoint = errorFeature.Endpoint?.ToString();
                        var response = new ErrorResponse()
                        {
                            Endpoint = endpoint,
                            StatusCode = StatusCodes.Status500InternalServerError,
                            ExceptionMessage = exception.Message,
                            StackTrace = exception.StackTrace,
                            Title = $"[{endpoint}] - Someting went wrong"
                        };
                        app.Logger.LogError(errorFeature.Error, JsonSerializer.Serialize(response));
                        await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = exception.Message }));
                    }
                });
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }
    }
}
