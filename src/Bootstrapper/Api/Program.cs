using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.IdentityModel.Tokens;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuration for Google OAuth and JWT
Assembly membershipAssembly = typeof(MembershipsModule).Assembly;

// Register dependencies
builder.Services.AddCarterWithAssemblies(membershipAssembly);
builder.Services.AddMediatRWithAssemblies(membershipAssembly);
builder.Services.AddMembershipModule(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS setup
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "all",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    );
});

// Setup Serilog logging
builder.Host.UseSerilog(
    (context, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddHttpClient();

// Configurar autenticación con cookies y JWT
builder.Services.AddAuthenticationServices(builder.Configuration);

builder.Services.AddAuthorization();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("all");

app.MapCarter();
app.UseMembershipModule();
app.UseHttpsRedirection();

app.Run();
