using MiniSimulator.Core;
using MiniSimulator.Core.Simulators;
using MiniSimulator.Domain.Interfaces;
using MiniSimulator.Domain.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IGameService, GameService>();
builder.Services.AddSingleton<IMatchSimulator, SimpleMatchSimulator>();

// Bind the settings options from appsettings
builder.Services.Configure<SimulationSettings>(builder.Configuration.GetSection(nameof(SimulationSettings)));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
