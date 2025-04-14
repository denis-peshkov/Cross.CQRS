var builder = WebApplication.CreateBuilder(args);

// Add services to the container. Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

// MediatR
builder.Services
    .AddCQRS(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/testEventsCommand", async (IMediator mediator) =>
{
    await mediator.Send(new TestEventsCommand());
});

app.MapGet("/testGenericCommand", async (IMediator mediator) =>
{
    return await mediator.Send(new TestGenericCommand("bcdhbcjdbcjdbcjd"));
});

app.MapGet("/testGenericQuery", async (IMediator mediator) =>
{
    return await mediator.Send(new TestGenericQuery());
});

app.Run();
