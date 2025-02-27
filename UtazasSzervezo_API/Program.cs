using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UtazasSzervezo_Library;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //DBContext injection
            builder.Services.AddDbContext<UtazasSzervezoDbContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 2))
            ));

            //Service injections
            builder.Services.AddScoped<AccommodationService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
