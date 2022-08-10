using GShopping.IdentityServer.Configuration;
using GShopping.IdentityServer.Initializer;
using GShopping.IdentityServer.Model;
using GShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GShopping.IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];

            builder.Services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 30))));

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MySQLContext>()
                .AddDefaultTokenProviders();

            var builderOption = builder.Services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.EmitStaticAudienceClaim = true;
            }).AddInMemoryIdentityResources(
                        IdentityConfiguration.IdentityResources)
                    .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
                    .AddInMemoryClients(IdentityConfiguration.Clients)
                    .AddAspNetIdentity<ApplicationUser>();

            builder.Services.AddScoped<IDbInitializer, DbInitializer>();
            //builder.Services.AddScoped<IProfileService, ProfileService>();

            builderOption.AddDeveloperSigningCredential();

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}