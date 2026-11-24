var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();
app.MapRazorPages();




app.Run();
