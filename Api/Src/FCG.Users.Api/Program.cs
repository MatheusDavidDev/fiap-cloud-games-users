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
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Configuração do DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<FcgUsersDbContext>(options => options.UseSqlServer(connectionString));

// Configuração do RabbitMQ com MassTransit
//builder.Services.AddMassTransit(x =>
//{
//    x.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host("rabbitmq", "/", h =>
//        {
//            h.Username("fcgrabbtmq");
//            h.Password("fcgrabbtmq");
//        });

//        cfg.ConfigureEndpoints(context);
//    });
//});

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

#endregion


// Query Services
builder.Services.AddScoped<IUsuarioQueryService, UsuarioQueryService>();

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

app.UseExceptionHandler();

app.UseMiddleware<RequestLoggingMiddleware>();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseHttpsRedirection();

app.MapControllers();



app.Run();
