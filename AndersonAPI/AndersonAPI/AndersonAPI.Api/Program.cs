using AndersonAPI.Api.Configuration;
using AndersonAPI.Api.Filters;
using AndersonAPI.Api.Logging;
using AndersonAPI.Api.Services;
using AndersonAPI.Application;
using AndersonAPI.Application.Account;
using AndersonAPI.Infrastructure;
using Azure.Identity;
using Intent.RoslynWeaver.Attributes;
using Microsoft.AspNetCore.DataProtection;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.AspNetCore.Program", Version = "1.0")]

namespace AndersonAPI.Api
{
    public class Program
    {
        [IntentManaged(Mode.Merge, Body = Mode.Merge)]
        public static void Main(string[] args)
        {
            using var logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.

                // IntentIgnore                                                                                                                  builder.Host.UseSerilog((context, services, configuration) => configuration
                builder.Host.UseSerilog((context, services, configuration) => configuration
                     .ReadFrom.Configuration(context.Configuration)
                     .ReadFrom.Services(services)
                     .WriteTo.Console()
                     .Destructure.With(new BoundedLoggingDestructuringPolicy()));

                builder.Services.AddControllers(
                    opt =>
                    {
                        opt.Filters.Add<ExceptionFilter>();
                    });

                var dp = builder.Services
                    .AddDataProtection()
                    .SetApplicationName("AndersonAPI");

                if (builder.Environment.IsProduction())
                {
                    var blobUri = builder.Configuration["DataProtection:BlobUri"];

                    Log.Information($"Configuring data protection keys storage. BlobUri: {blobUri}");

                    if (!string.IsNullOrWhiteSpace(blobUri))
                    {
                        dp.PersistKeysToAzureBlobStorage(
                            new Uri(blobUri),
                            new DefaultAzureCredential());
                    }
                    else
                    {
                        var keysPath = "/home/aspnet/DataProtection-Keys";
                        Directory.CreateDirectory(keysPath);
                        dp.PersistKeysToFileSystem(new DirectoryInfo(keysPath));
                    }
                }
                else
                {
                    var keysPath = Path.Combine(builder.Environment.ContentRootPath, "DataProtectionKeys");
                    Directory.CreateDirectory(keysPath);

                    dp.PersistKeysToFileSystem(new DirectoryInfo(keysPath));
                }

                builder.Services.AddApplication(builder.Configuration);
                builder.Services.ConfigureApplicationSecurity(builder.Configuration);
                builder.Services.ConfigureHealthChecks(builder.Configuration);
                builder.Services.ConfigureIdentity();
                builder.Services.ConfigureProblemDetails();
                builder.Services.ConfigureApiVersioning();

                // IntentIgnore
                builder.Services.AddInfrastructure(builder.Configuration, builder.Environment.IsDevelopment());
                builder.Services.ConfigureOpenApi();

                builder.Services.AddTransient<IAccountEmailSender, AccountEmailSender>();
                builder.Services.AddTransient<ITokenService, TokenService>();

                builder.Services.AddCors(options =>
                {
                    options.AddDefaultPolicy(policy =>
                    {
                        var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
                        if (origins != null && origins.Length > 0)
                        {
                            policy.WithOrigins(origins)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                        }
                    });
                });

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseSerilogRequestLogging();
                app.UseExceptionHandler();
                app.UseHttpsRedirection();
                app.UseRouting();
                app.UseCors();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapScalarApiReference();
                app.MapOpenApi();
                app.MapDefaultHealthChecks();
                app.MapControllers();

                logger.Write(LogEventLevel.Information, "Starting web host");

                app.Run();
            }
            catch (HostAbortedException)
            {
                // Excluding HostAbortedException from being logged, as this is an expected
                // exception when working with EF Core migrations (as per the .NET team on the below link)
                // https://github.com/dotnet/efcore/issues/29809#issuecomment-1344101370
            }
            catch (Exception ex)
            {
                logger.Write(LogEventLevel.Fatal, ex, "Unhandled exception");
                Log.Fatal(ex, "Unhandled exception");
            }
        }
    }
}