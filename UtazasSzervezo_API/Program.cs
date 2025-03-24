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
            options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
                b => b.MigrationsAssembly("UtazasSzervezo_Library")
            ));


            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddEntityFrameworkStores<UtazasSzervezoDbContext>();


            //CORS engedélyezése a frontend számára
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            //Service injections
            builder.Services.AddScoped<AccommodationService>();
            builder.Services.AddScoped<BookingService>(); 
            builder.Services.AddScoped<FlightService>();
            builder.Services.AddScoped<ReviewService>();
            builder.Services.AddScoped<AmenityService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                SeedData.EnsurePopulated(app);
            }


            app.UseCors("AllowAllOrigins");

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
