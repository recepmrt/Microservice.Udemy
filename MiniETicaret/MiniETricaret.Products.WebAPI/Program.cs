using Bogus;
using Microsoft.EntityFrameworkCore;
using MiniETricaret.Products.WebAPI.Context;
using MiniETricaret.Products.WebAPI.Dtos;
using MiniETricaret.Products.WebAPI.Models;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/seedData", (ApplicationDbContext context) =>
{
    for (int i = 0; i < 100; i++)
    {
        Faker faker = new Faker();
        
        Product product = new Product()
        {
            Name = faker.Commerce.ProductName(),
            Price = Convert.ToDecimal(faker.Commerce.Price()),
            Stock = faker.Commerce.Random.Int(1, 100)
        };
        
        context.Products.Add(product);
    }
    
    context.SaveChanges();

    return Results.Ok(Result<string>.Succeed("Seed data başarıyla çalıştırıldı ve ürünler oluşturuldu!"));
});

app.MapGet("/getall", async (ApplicationDbContext context, CancellationToken cancellationToken) =>
{
    var products = await context.Products.OrderBy(p => p.Name).ToListAsync(cancellationToken);
    Result<List<Product>> response = products;

    return response;
});

app.MapPost("/create", async (CreateProductDto request, ApplicationDbContext context,
    CancellationToken cancellationToken) =>
{
    bool isNameExist = await context.Products.AnyAsync(p => p.Name == request.Name, cancellationToken);

    if (isNameExist)
    {
        var response = Result<string>.Failure("Ürün adı daha önce oluşturulmuş");

        return Results.BadRequest(response);
    }
    
    Product product = new Product()
    {
        Name = request.Name,
        Price = request.Price,
        Stock = request.Stock
    };

    await context.AddAsync(product, cancellationToken);
    await context.SaveChangesAsync(cancellationToken);

    return Results.Ok(Result<string>.Succeed("Ürün kaydı başarıyla oluşturuldu"));
});
// Uygulama her çalıştığında Migrate edilecek bir değişiklik varsa onu otomatik olarak Db'ye migrate edecektir.
    using (var scoped = app.Services.CreateScope())
    {
        var srv = scoped.ServiceProvider;
        var context = srv.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }

    app.Run();