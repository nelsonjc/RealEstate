using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using RealEstate.API.App_Start;
using RealEstate.Infraestructure.Data;
using RealEstate.Infraestructure.Filter;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services
            .AddControllers(opt =>
            {
                opt.Filters.Add<GlobalExepctionFilter>();
            })
            .AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .ConfigureApiBehaviorOptions(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });

        // Add dependencies injection
        builder.Services.AddDependencyInjectionConfiguration();
        builder.Services.AddOptions(builder.Configuration);
        builder.Services.AddSwaggerConfig();
        builder.Services.AddJwtConfig(builder.Configuration);
        builder.Services.AddUtoMapperConfig();

        // Add database context
        var conn = SwitchConnectionConfiguration.GetConnection();
        builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(conn, p => p.MigrationsAssembly("RealEstate.API")));

        // Optional: Add CORS configuration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Optional: Enable CORS
        app.UseCors("AllowAll");

        app.UseHttpsRedirection();
        // Serve files from the custom directory
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files/PropertyImages")),
            RequestPath = "/Files/PropertyImages"
        });
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
