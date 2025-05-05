using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;
using UtazasSzervezo_Library;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_UI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var defaultCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentCulture = defaultCulture;
        CultureInfo.DefaultThreadCurrentUICulture = defaultCulture;

        var connectionString = builder.Configuration.GetConnectionString("UtazasSzervezoDbContextConnection") ?? throw new InvalidOperationException("Connection string 'UtazasSzervezoDbContextConnection' not found.");

        try
        {
            builder.Services.AddDbContext<UtazasSzervezoDbContext>(options =>
            options.UseMySql(
                builder.Configuration.GetConnectionString("UtazasSzervezoDbContextConnection"),
                ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("UtazasSzervezoDbContextConnection"))
            ));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error connecting to the database: " + ex.Message);
        }

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
            var roles = new List<string> { "Admin"};
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
            //admin felhasználó hozzáadása
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminemail = builder.Configuration["AdminData:Email"];
            string adminpassword = builder.Configuration["AdminData:Password"];

            if (await userManager.FindByEmailAsync(adminemail) == null)
            {
                var user = new User 
                {
                    UserName = adminemail,
                    Email = adminemail,
                    Name = "Admin",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, adminpassword);

                await userManager.AddToRoleAsync(user, "Admin");
                   
            }

            //sima user hozzáadása
            string userEmail = builder.Configuration["DefaultUser:Email"];
            string userPassword = builder.Configuration["DefaultUser:Password"];

            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var regularUser = new User
                {
                    UserName = userEmail, 
                    Email = userEmail
                };

                await userManager.CreateAsync(regularUser, userPassword);
            }
        }

        app.Run();
    }
}
