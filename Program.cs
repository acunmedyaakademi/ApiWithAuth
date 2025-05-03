using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using ApiWithAuth.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;

namespace ApiWithAuth;

public sealed class SlugifyParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value == null) { return null; }
        string? str = value.ToString();
        if (string.IsNullOrEmpty(str)) { return null; }

        return Regex.Replace(str, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}



public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(
                new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        }).AddJsonOptions(options =>
        {
            //options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
        });
        
        builder.Services.AddAuthorization();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
            options.Password.RequiredLength = 3;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        });
        
        builder.Services.AddIdentityApiEndpoints<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        builder.Services.AddTransient<IEmailService, EmailService>();
        builder.Services.AddTransient<IEmailSender, EmailSender>();
        
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await SeedRoles(services);
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        // identity endpointlerinin başına ekleme yapmak için
        // prefix ön ek, suffix son ek
        app.MapGroup("/auth").MapIdentityApi<IdentityUser>();

        app.MapControllers();

        app.Run();
    }

    private static async Task SeedRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    
        if (await roleManager.RoleExistsAsync("Admin")) { return; }
    
        await roleManager.CreateAsync(new IdentityRole("Admin"));
        //await roleManager.CreateAsync(new IdentityRole("User"));
    
        var adminUser = new IdentityUser { UserName = "admin", Email = "orhanekici@gmail.com" };
        adminUser.EmailConfirmed = true;
        await userManager.CreateAsync(adminUser, "P99yG-wSd8T$");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

