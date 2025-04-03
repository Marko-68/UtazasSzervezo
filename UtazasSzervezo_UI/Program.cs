using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UtazasSzervezo_Library;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_UI.Services;

namespace UtazasSzervezo_UI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("UtazasSzervezoDbContextConnection") ?? throw new InvalidOperationException("Connection string 'UtazasSzervezoDbContextConnection' not found.");

        builder.Services.AddDbContext<UtazasSzervezoDbContext>(options =>
        options.UseMySql(
            builder.Configuration.GetConnectionString("UtazasSzervezoDbContextConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("UtazasSzervezoDbContextConnection"))
        ));

        builder.Services.AddDefaultIdentity<User>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
        })
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<UtazasSzervezoDbContext>();

        builder.Services.AddRazorPages();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        //apply CORS
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

        //Roles
        using(var scope = app.Services.CreateScope())
        {
            var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new List<string> { "Admin" ,"User" };
            foreach (var role in roles)
            {
                if (!RoleManager.RoleExistsAsync(role).Result)
                {
                    RoleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }
        }

        using (var scope = app.Services.CreateScope())
        {
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string email = "admin@gmail.com";
            string password = "Admin1@";

            if (await userManager.FindByEmailAsync(email) == null)
            {
                var user = new User 
                {
                    UserName = email,
                    Email = email,
                    Name = "Admin" 
                };

                await userManager.CreateAsync(user, password);

                await userManager.AddToRoleAsync(user, "Admin");
                   
            }
        }

        app.Run();
    }
}
