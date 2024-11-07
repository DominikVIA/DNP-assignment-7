using BlazorApp1.Auth;
using BlazorApp1.Components;
using BlazorApp1.Services;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Authorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient<IUserService, HttpUserService>(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7065/")
});
builder.Services.AddHttpClient<IPostService, HttpPostService>(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7065/")
});
builder.Services.AddHttpClient<ICommentService, HttpCommentService>(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7065/")
});
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7065") });
builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthProvider>();
builder.Services.AddAuthorizationCore();


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

app.Run();