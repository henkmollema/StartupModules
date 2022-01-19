using StartupModules;

var builder = WebApplication.CreateBuilder(args);
builder.UseStartupModules();
var app = builder.Build();

app.MapGet("/", () => "Hello, World!");
app.Run();