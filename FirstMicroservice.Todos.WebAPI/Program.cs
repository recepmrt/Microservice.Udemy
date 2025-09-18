using FirstMicroservice.Todos.WebAPI.Components;
using FirstMicroservice.Todos.WebAPI.Context;
using FirstMicroservice.Todos.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseInMemoryDatabase("MyDb");
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.MapGet("/todos/create", (string work, ApplicationDbContext context) =>
{
    Todo todo = new()
    {
        Work = work
    };

    context.Add(todo);
    context.SaveChanges();
    
    return new { Message = "Todo created successfully" };
});

app.MapGet("/todos/getall", (ApplicationDbContext context) =>
{
    var todos = context.Todos.ToList();
    return todos;
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();