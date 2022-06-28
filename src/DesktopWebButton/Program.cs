using DesktopWebButton.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped(_ => new ButtonDataSetClient("buttons.json"));
builder.Services.AddScoped(s => s.GetRequiredService<ButtonDataSetClient>().Load());

builder.Services.AddControllersWithViews();
builder.Host.UseWindowsService(options =>
{
    options.ServiceName = "DesktopWebButton";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

app.MapControllers();
// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");;

app.Run();
