using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UtazasSzervezo_Library;
using UtazasSzervezo_UI.Services;

namespace UtazasSzervezo_UI;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        //var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        // builder.Services.AddDbContext<UtazasSzervezoDbContext>(options =>
        //     options.UseSqlServer(connectionString));
        //var connectionString = "server=localhost;database=UtazasSzervezoIdentityDB;user=root;password=;";
        builder.Services.AddDbContext<UtazasSzervezoDbContext>(options =>
            options.UseMySql(
                "server=localhost;database=UtazasSzervezoDb;user=root;password=;"
                , ServerVersion.AutoDetect("server=localhost;database=UtazasSzervezoDb;user=root;password=;")
            )
        );
        
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<UtazasSzervezoDbContext>();
        builder.Services.AddRazorPages();

        //CORS engedélyezéses
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
            var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string email = "admin@gmail.com";
            string password = "Admin1@";
            
            if(await UserManager.FindByEmailAsync(email) == null)
            {
                var user = new IdentityUser();
                user.UserName = email;
                user.Email = email;

                await UserManager.CreateAsync(user, password);

                await UserManager.AddToRoleAsync(user, "Admin");
            }
        }

        app.Run();
    }
}
