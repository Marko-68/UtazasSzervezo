using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UtazasSzervezo_Library;
using UtazasSzervezo_UI.Data;
using UtazasSzervezo_UI.Services;

namespace UtazasSzervezo_UI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        // builder.Services.AddDbContext<UtazasSzervezoDbContext>(options =>
        //     options.UseSqlServer(connectionString));
        //var connectionString = "server=localhost;database=UtazasSzervezoIdentityDB;user=root;password=;";
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                "server=localhost;database=UtazasSzervezoIdentityDB;user=root;password=;"
                , ServerVersion.AutoDetect("server=localhost;database=UtazasSzervezoIdentityDB;user=root;password=;")
            )
        );
        
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        builder.Services.AddRazorPages();

        //CORS engedélyezése a frontend számára
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        });

        builder.Services.AddHttpClient<ApiService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}
