using FCG.Users.Api.Erros;
using FCG.Users.Api.Middleware;
using FCG.Users.Application.Commands.AuthCommands.LoginCommand;
using FCG.Users.Application.Commands.AuthCommands.RefreshTokenCommand;
using FCG.Users.Application.Commands.UsuarioCommands.CadastrarUsuario;
using FCG.Users.Application.Interfaces.Security;
using FCG.Users.Application.Queries;
using FCG.Users.Core.Behaviors;
using FCG.Users.Core.UnitOfWork;
using FCG.Users.Infra;
using FCG.Users.Infra.Queries;
using FCG.Users.Infra.Security;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;   
})
.AddJwtBearer(options => 
{

    var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException();

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        var esquemaSeguranca = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = JwtBearerDefaults.AuthenticationScheme,
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "Cole aqui apenas o seu hash JWT gerado no login."
        };

        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes.Add("Bearer", esquemaSeguranca);

        var requisitoSeguranca = new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        };

        document.SecurityRequirements = new List<OpenApiSecurityRequirement> { requisitoSeguranca };
        return Task.CompletedTask;
    });
});

// Configuração do DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FcgUsersDbContext>(options => options.UseSqlServer(connectionString));

// Configuração do MassTransit com RabbitMQ
builder.Services.AddMassTransit(x =>
{

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(
            builder.Configuration["RabbitMQ:Host"],
            builder.Configuration["RabbitMQ:VirtualHost"],
            h =>
            {
                h.Username(builder.Configuration["RabbitMQ:Username"!]);
                h.Password(builder.Configuration["RabbitMQ:Password"]);
            });

        cfg.ConfigureEndpoints(context);
    });
});

#region DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<FcgUsersDbContext>>();
builder.Services.AddScoped<IHashSenha, HashSenha>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// Query Services
builder.Services.AddScoped<IUsuarioQueryService, UsuarioQueryService>();

#endregion

#region MEDIATR
//Usuario
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CadastrarUsuarioHandler).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(CadastrarUsuarioValidator).Assembly);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(LoginHandler).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(LoginValidator).Assembly);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(RefreshTokenHandler).Assembly));
builder.Services.AddValidatorsFromAssembly(typeof(RefreshTokenValidator).Assembly);
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FcgUsersDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseExceptionHandler();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
