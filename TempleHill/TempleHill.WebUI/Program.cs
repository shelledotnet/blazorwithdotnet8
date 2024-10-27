using TempleHill.Infrastructure.DependencyInjection;
using TempleHill.Application.DependencyInjection;
using TempleHill.WebUI.Components;
using TempleHill.WebUI.Components.Layout.Identity;
using Microsoft.AspNetCore.Components.Authorization;

#region IOC container
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructureService(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthStateProvider>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); 
#endregion

#region Middleweare is the link between the server and the client
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapSignOutEndPoint();
app.Run(); 
#endregion
