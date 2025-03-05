WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Assembly membershipAssembly = typeof(MembershipsModule).Assembly;

//registers dependence
builder.Services.AddCarterWithAssemblies(membershipAssembly);

builder.Services.AddMediatRWithAssemblies(membershipAssembly);

builder.Services.AddMembershipModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("all");

app.MapCarter();

app.UseMembershipModule();

app.UseHttpsRedirection();

app.Run();
