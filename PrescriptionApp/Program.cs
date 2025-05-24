using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PrescriptionApp.DAL;
using PrescriptionApp.Services;

namespace PrescriptionApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<PrescriptionDbContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));       } );

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        
        app.Run();
    }
}