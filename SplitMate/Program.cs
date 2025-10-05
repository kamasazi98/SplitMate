using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using SplitMate.Components;
using SplitMate.Infrastracture;
using SplitMate.Infrastracture.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddMudServices()
	.AddServerSideBlazor();
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
{
	opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddInfrastructureLayer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseWebAssemblyDebugging();
}
else
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
	.AddInteractiveWebAssemblyRenderMode()
	.AddAdditionalAssemblies(typeof(SplitMate.Client._Imports).Assembly);
app.MapControllers();

app.Run();
