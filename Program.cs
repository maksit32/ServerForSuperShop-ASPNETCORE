using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Context;
using Server.Entities;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//внедряем БД (connectionString) // secrets.json (key:value)
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlServer(builder.Configuration["ConnectionStrings:MicrosoftSQL"]));



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


//CRUD
app.MapGet("/get_products", async (AppDbContext context) => await context.Products.ToListAsync());
app.MapPost("/add_product", async (AppDbContext context, [FromBody] Product product) =>
{
	await context.Products.AddAsync(product);
	context.SaveChanges();
});
app.MapGet("/get_by_id", (AppDbContext context, [FromQuery] int id) =>
{
	try
	{
		return context.Products.ToList().Find(e => e.Id == id);
	}
	catch (Exception)
	{
		app.Logger.LogError("Попытка достать id, которого не существует!");
	}
	return null;
});
app.MapDelete("/delete_by_id", (AppDbContext context, [FromQuery] int id) =>
{
	try
	{
		var obj = context.Products.ToList().Find(e => e.Id == id);
		if(obj != null)
		{
			context.Products.Remove(obj);
			context.SaveChanges();
			return "Ok!";
		}
	}
	catch (Exception)
	{
		app.Logger.LogError("Попытка удалить элемент по id, которого не существует!");
	}
	return null;
});
app.MapPost("/update_product", async (AppDbContext context, [FromBody] Product product) =>
{
	try
	{
		//id менять нельзя!
		var res = context.Products.ToList().Find(e => e.Id == product.Id);

		res.Name = product.Name;
		res.Description = product.Description;
		res.Price = product.Price;
		res.Category = product.Category;
		res.LinkFullDescription = product.LinkFullDescription;
		res.LinkImg = product.LinkImg;

		context.SaveChanges();
	}
	catch (Exception)
	{
		app.Logger.LogError("Попытка изменить элемент по id, которого не существует!");
	}
});
app.MapPost("/updateproduct/{id:int}/{price:int}/{name}/{description}/{linkDescription}/{linkImg}/{category:int}", 
	async (AppDbContext context, int id, int price, string name,
	string description, string linkDescription, string linkImg, int category) =>
{
	try
	{
		//id менять нельзя!
		var res = context.Products.ToList().Find(e => e.Id == id);

		res.Name = name;
		res.Description = description;
		res.Price = price;
		res.Category = category;
		res.LinkFullDescription = linkDescription;
		res.LinkImg = linkImg;

		context.SaveChanges();
	}
	catch (Exception)
	{
		app.Logger.LogError("Попытка изменить элемент по id, которого не существует!");
	}
});
app.MapDelete("/clear_product_db", async (AppDbContext context) =>
{
	await context.Products.ExecuteDeleteAsync();
	context.SaveChanges();
});



app.Run();


//post - что SaveChanges