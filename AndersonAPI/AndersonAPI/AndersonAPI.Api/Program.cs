using AndersonAPI.Api.Configuration;
using AndersonAPI.Api.Filters;
using AndersonAPI.Api.Logging;
using AndersonAPI.Api.Services;
using AndersonAPI.Application;
using AndersonAPI.Application.Account;
using AndersonAPI.Infrastructure;
using Intent.RoslynWeaver.Attributes;
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
                .CreateBootstrapLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Destructure.With(new BoundedLoggingDestructuringPolicy()));

                builder.Services.AddControllers(
                    opt =>
                    {
                        opt.Filters.Add<ExceptionFilter>();
                    });
                builder.Services.AddApplication(builder.Configuration);
                builder.Services.ConfigureApplicationSecurity(builder.Configuration);
                builder.Services.ConfigureHealthChecks(builder.Configuration);
                builder.Services.ConfigureIdentity();
                builder.Services.ConfigureProblemDetails();
                builder.Services.ConfigureApiVersioning();

                builder.Services.AddInfrastructure(builder.Configuration);
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

                //app.Lifetime.ApplicationStarted.Register(() =>
                //{
                //    Console.WriteLine(">>> ApplicationStarted fired");
                //});

                // Configure the HTTP request pipeline.
                app.Use(async (ctx, next) =>
                {
                    if (ctx.Request.Path.Value != "/openapi/v1.json")
                    {
                        Console.WriteLine($">>> REQ {ctx.Request.Method} {ctx.Request.Path}");
                    }
                    
                    // Capture response body for error logging
                    var originalBodyStream = ctx.Response.Body;
                    using var responseBody = new MemoryStream();
                    ctx.Response.Body = responseBody;
                    
                    try
                    {
                        await next();
                        
                        // Copy the response body to the original stream
                        responseBody.Seek(0, SeekOrigin.Begin);
                        await responseBody.CopyToAsync(originalBodyStream);
                        
                        if (ctx.Request.Path.Value != "/openapi/v1.json")
                        {
                            Console.WriteLine($">>> RES {ctx.Response.StatusCode} {ctx.Request.Method} {ctx.Request.Path}");
                            
                            if (ctx.Response.StatusCode >= 400)
                            {
                                Console.WriteLine($"TraceIdentifier: {ctx.TraceIdentifier}");
                                
                                // Read the response body
                                responseBody.Seek(0, SeekOrigin.Begin);
                                using var reader = new StreamReader(responseBody);
                                var body = await reader.ReadToEndAsync();
                                Console.WriteLine($"Response Body: {body}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($">>> EX for {ctx.Request.Method} {ctx.Request.Path}\n{ex}");
                        
                        // Make sure to restore the original stream on exception
                        ctx.Response.Body = originalBodyStream;
                        throw;
                    }
                    finally
                    {
                        ctx.Response.Body = originalBodyStream;
                    }
                });


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
                Console.WriteLine(ex.ToString());
                logger.Write(LogEventLevel.Fatal, ex, "Unhandled exception");
            }
        }
    }
}