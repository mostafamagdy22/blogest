using DotNetEnv;

using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using blogest.infrastructure;
using blogest.application.Interfaces.services;
using Serilog;
using blogest.api.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using blogest.api.SwaggerCofig;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using blogest.infrastructure.Configuration;
using System.Reflection;
using Hangfire;
using blogest.api.HangFireJobs;

Env.Load("../blogest.infrastructure/.env");

var builder = WebApplication.CreateBuilder(args);

// services 

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.Seq("http://localhost:5341")
                .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddTransient<UploadImageJob>();

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("Cloudinary")
);
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        string path = httpContext.Request.Path.Value;
        if (path.StartsWith("/swagger"))
            return RateLimitPartition.GetNoLimiter("swagger");

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anon",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 10,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            });
    });
    options.RejectionStatusCode = 429;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();

    options.SupportNonNullableReferenceTypes();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddInfraStructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies["jwt"];
            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }
            return Task.CompletedTask;
        }
    };
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"))),
    };
})
.AddGoogle(options =>
{
    options.ClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
    options.ClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
    // options.SignInScheme = JwtBearerDefaults.AuthenticationScheme;
    options.CallbackPath = "/login-google";
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();

app.UseRateLimiter(new RateLimiterOptions
{
    RejectionStatusCode = 429,
    OnRejected = async (context,token) =>
    {
        context.HttpContext.Response.StatusCode = 429;
        await context.HttpContext.Response.WriteAsync("Too many requests",token);
    }
});

app.UseAuthentication();
app.UseAuthorization();
app.UseHangfireDashboard("/dashboard-hangfire");
if (app.Environment.IsDevelopment())
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger(options => options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0);
    app.UseSwaggerUI(c =>
    {
        foreach (var description in provider.ApiVersionDescriptions)
        {
            c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"blogest API {description.GroupName.ToUpper()}");
        }
        c.RoutePrefix = "swagger";
    });
}

app.MapControllers();
app.Run();