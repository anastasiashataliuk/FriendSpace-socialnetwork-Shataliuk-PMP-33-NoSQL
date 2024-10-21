using DAL1.Concrete;
using MongoDB.Driver;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DAL1.Concreate;

var builder = WebApplication.CreateBuilder(args);

// MongoDB setup
var client = new MongoClient("mongodb://localhost:27017"); // Change this connection string as needed
var database = client.GetDatabase("FriendSpace");

// Register services with Dependency Injection
builder.Services.AddSingleton<IMongoDatabase>(database);
builder.Services.AddScoped<UserDAL>();
builder.Services.AddScoped<PostsDAL>();
builder.Services.AddScoped<CommentsDAL>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
