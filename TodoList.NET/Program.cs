using Microsoft.AspNetCore.Mvc;
using TodoApi;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
                          policy =>
                          {
                              policy.WithOrigins("http://localhost:3000")
                                                  .AllowAnyHeader()
                                                  .AllowAnyMethod();
                          });
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
    });
});
builder.Services.AddDbContext<ToDoDbContext>();

var app = builder.Build();
app.UseCors("MyAllowSpecificOrigins");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API");
    options.RoutePrefix = string.Empty;
});

// app.MapGet("/", () => "Hello World!");

app.MapGet("/items", (ToDoDbContext context) =>
{
    return context.Items.ToList();
});

app.MapPost("/items", async(ToDoDbContext context, Item item)=>{
    context.Add(item);
    await context.SaveChangesAsync();
    return item;
});

app.MapPut("/items/{id}", async(ToDoDbContext context, [FromBody]Item item, int id)=>{
    var existItem = await context.Items.FindAsync(id);
    if(existItem is null) return Results.NotFound();

    existItem.Name = item.Name;
    existItem.IsComplete = item.IsComplete;

    await context.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/items/{id}", async(ToDoDbContext context, int id)=>{
    var existItem = await context.Items.FindAsync(id);
    if(existItem is null) return Results.NotFound();

    context.Items.Remove(existItem);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();

