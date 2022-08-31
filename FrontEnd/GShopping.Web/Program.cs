using GShopping.Web.Services;
using GShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;

namespace GShopping.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient<IProductService, ProductService>(c =>
                c.BaseAddress = new Uri(builder.Configuration["ServicesUrls:ProductAPI"]));

            builder.Services.AddHttpClient<ICartService, CartService>(c =>
                c.BaseAddress = new Uri(builder.Configuration["ServicesUrls:CartAPI"]));

            builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
               c.BaseAddress = new Uri(builder.Configuration["ServicesUrls:OrderAPI"]));
            
            builder.Services.AddHttpClient<IEmailService, EmailService>(c =>
               c.BaseAddress = new Uri(builder.Configuration["ServicesUrls:EmailAPI"]));

            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies", c =>  c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
            .AddOpenIdConnect("oidc", options =>
            {
                options.Authority = builder.Configuration["ServicesUrls:IdentityServer"];
                options.GetClaimsFromUserInfoEndpoint = true;
                options.ClientId = "g_shopping";
                options.ClientSecret = "g_shopping_secret";
                options.ResponseType = "code";
                options.ClaimActions.MapJsonKey("role", "role", "role");
                options.ClaimActions.MapJsonKey("sub", "sub", "sub");
                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";
                options.Scope.Add("g_shopping");
                options.SaveTokens = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}